namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Member;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ProductData : ProductMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            string filter = ProductMasterProvider.BuildProductBrowseQuerySearch(query);
            string selectFields = "ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                selectFields = selectFields + string.Format(",CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", user.GradeId) + string.Format("THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", user.GradeId, memberDiscount);
            }
            else
            {
                selectFields = selectFields + ",SalePrice as RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", filter, selectFields);
        }

        public override BundlingInfo GetBundlingInfo(int bundlingID)
        {
            BundlingInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBindInfo(reader);
                }
            }
            return info;
        }

        public override List<BundlingItemInfo> GetBundlingItemsByID(int bundlingID)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName, ");
            builder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
            builder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            List<BundlingItemInfo> list = new List<BundlingItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    BundlingItemInfo item = new BundlingItemInfo();
                    item.ProductID = (int) reader["ProductID"];
                    item.ProductNum = (int) reader["ProductNum"];
                    item.SkuId = (string) reader["SkuId"];
                    item.ProductName = (string) reader["ProductName"];
                    item.ProductPrice = (decimal) reader["ProductPrice"];
                    item.BundlingID = bundlingID;
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetBundlingProductList(BundlingInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" salestatus=1 ");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND Name Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "Bundlingid,Name,Num,price,SaleStatus,AddTime,ShortDescription,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_BundlingProducts", "Bundlingid", builder.ToString(), selectFields);
        }

        public override DataTable GetCounDownProducList(int maxnum)
        {
            DataTable table = new DataTable();
            string query = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_Hishop_CountDown where datediff(hh,EndDate,getdate())<=0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) order by DisplaySequence desc", 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CountDownInfo GetCountDownInfo(int productId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public override CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0})", 1);
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", filter, "*");
        }

        public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
            return (decimal) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override GiftInfo GetGift(int giftId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            GiftInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGift(reader);
                }
            }
            return info;
        }

        public override DataTable GetGroupByProductList(int maxnum)
        {
            DataTable table = new DataTable();
            string query = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,G.[Count],G.Price from vw_Hishop_BrowseProductList as P inner join Hishop_GroupBuy as S on P.ProductId=s.ProductId inner join  Hishop_GroupBuyCondition as G on G.GroupBuyId=S.GroupBuyId where datediff(hh,S.EndDate,getdate())<=0 and P.SaleStatus={0} order by S.DisplaySequence desc", 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataSet GetGroupByProductList(ProductBrowseQuery query, out int TotalGroupBuyProducts)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_GroupBuyProducts_Get");
            string str = string.Format("SELECT GroupBuyId,ProductId,StartDate,EndDate FROM Hishop_GroupBuy WHERE datediff(hh,EndDate,getdate())<=0 AND Status={0}", 1) + string.Format(" AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) ORDER BY DisplaySequence DESC", 1);
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, str);
            this.database.AddOutParameter(storedProcCommand, "TotalGroupBuyProducts", DbType.Int32, 4);
            DataSet set = this.database.ExecuteDataSet(storedProcCommand);
            TotalGroupBuyProducts = (int) this.database.GetParameterValue(storedProcCommand, "TotalGroupBuyProducts");
            return set;
        }

        public override int GetLineItemNumber(int productId)
        {
            string query = string.Format("select count(*) from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId", 1, 4);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetLineItems(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId = {0} ", productId);
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_OrderItem", "OrderId", builder.ToString(), "*");
        }

        public override DataTable GetLineItems(int productId, int maxNum)
        {
            DataTable table = new DataTable();
            string query = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  order by orders.PayDate desc", 1, 4);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetOnlineGifts(Pagination page)
        {
            string selectFields = "GiftId,Name,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220, MarketPrice,NeedPoint";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", "NeedPoint > 0", selectFields);
        }

        public override IList<GiftInfo> GetOnlinePromotionGifts()
        {
            List<GiftInfo> list = new List<GiftInfo>();
            string query = "SELECT * FROM Hishop_Gifts WHERE IsPromotion=1";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateGift(reader));
                }
            }
            return list;
        }

        public override int GetOrderCount(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            int memberDiscount = 100;
            ProductBrowseInfo info = new ProductBrowseInfo();
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId;");
            builder.Append(" SELECT * ,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName");
            builder.Append(" FROM Hishop_Products p where ProductId=@ProductId;");
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member user = HiContext.Current.User as Member;
                memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", user.GradeId, memberDiscount);
                builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            if (maxReviewNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC ; ", maxReviewNum);
            }
            if (maxConsultationNum.HasValue)
            {
                builder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
            }
            builder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
            builder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
            builder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
            builder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
            builder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {1})", 1, productId);
            builder.Append(" UNION SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
            builder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Hishop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
            builder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.Product = DataMapper.PopulateProduct(reader);
                    if (reader["BrandName"] != DBNull.Value)
                    {
                        info.BrandName = (string) reader["BrandName"];
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        info.Product.Skus.Add((string) reader["SkuId"], DataMapper.PopulateSKU(reader));
                    }
                }
                if (maxReviewNum.HasValue && reader.NextResult())
                {
                    info.DBReviews = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (maxConsultationNum.HasValue && reader.NextResult())
                {
                    info.DBConsultations = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult())
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        DataTable table2 = table.Clone();
                        foreach (DataRow row in table.Rows)
                        {
                            bool flag = false;
                            if (table2.Rows.Count > 0)
                            {
                                foreach (DataRow row2 in table2.Rows)
                                {
                                    if (((int) row2["AttributeId"]) == ((int) row["AttributeId"]))
                                    {
                                        DataRow row4;
                                        flag = true;
                                        (row4 = row2)["ValueStr"] = row4["ValueStr"] + ", " + row["ValueStr"];
                                    }
                                }
                            }
                            if (!flag)
                            {
                                DataRow row3 = table2.NewRow();
                                row3["AttributeId"] = row["AttributeId"];
                                row3["AttributeName"] = row["AttributeName"];
                                row3["ValueStr"] = row["ValueStr"];
                                table2.Rows.Add(row3);
                            }
                        }
                        info.DbAttribute = table2;
                    }
                }
                if (reader.NextResult())
                {
                    info.DbSKUs = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult())
                {
                    info.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            return info;
        }

        public override int GetProductConsultationNumber(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductConsultations WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetProductConsultations(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId = {0}", productId);
            builder.Append(" AND ReplyUserId IS NOT NULL ");
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_ProductConsultations", "ConsultationId", builder.ToString(), "*");
        }

        public override GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status; SELECT * FROM Hishop_GroupBuyCondition WHERE GroupBuyId=(SELECT GroupBuyId FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGroupBuy(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    GropBuyConditionInfo item = new GropBuyConditionInfo();
                    item.Count = (int) reader["Count"];
                    item.Price = (decimal) reader["Price"];
                    if (info != null)
                    {
                        info.GroupBuyConditions.Add(item);
                    }
                }
            }
            return info;
        }

        public override int GetProductReviewNumber(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DataTable GetProductReviews(int maxNum)
        {
            DataTable table = new DataTable();
            string query = "select top " + maxNum + " review.*,products.ProductName,products.ThumBnailUrl40,products.ThumBnailUrl60,products.ThumBnailUrl100,products.ThumBnailUrl160,products.ThumBnailUrl180,products.ThumBnailUrl220 from Hishop_ProductReviews as review inner join Hishop_Products as products on review.ProductId=products.ProductId order by review.ReviewDate desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetProductReviews(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductId = {0}", productId);
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_ProductReviews", "reviewId", builder.ToString(), "*");
        }

        public override ProductInfo GetProductSimpleInfo(int productId)
        {
            ProductInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
            }
            return info;
        }

        public override DataTable GetSaleProductRanking(int? categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} ProductId, ProductName, ProductCode, ShowSaleCounts AS SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100,", maxNum);
            builder.AppendFormat("  ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, SalePrice, MarketPrice FROM vw_Hishop_BrowseProductList WHERE SaleStatus = {0}", 1);
            if (categoryId.HasValue)
            {
                CategoryInfo category = CategoryBrowser.GetCategory(categoryId.Value);
                if (category != null)
                {
                    builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
                }
            }
            builder.Append("ORDER BY ShowSaleCounts DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSubjectList(SubjectListQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
                builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,");
                builder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", user.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", user.GradeId, memberDiscount);
            }
            else
            {
                builder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
                builder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,SalePrice AS RankPrice");
            }
            builder.Append(" FROM vw_Hishop_BrowseProductList p WHERE ");
            builder.Append(ProductMasterProvider.BuildProductSubjectQuerySearch(query));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), DataHelper.CleanSearchString(query.SortOrder.ToString()));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
        {
            string filter = ProductMasterProvider.BuildUnSaleProductBrowseQuerySearch(query);
            string selectFields = "ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,MarketPrice, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,SalePrice,Stock";
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                selectFields = selectFields + string.Format(",CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", user.GradeId) + string.Format("THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", user.GradeId, memberDiscount);
            }
            else
            {
                selectFields = selectFields + ",SalePrice as RankPrice";
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", filter, selectFields);
        }

        public override DataTable GetVistiedProducts(IList<int> productIds)
        {
            if (productIds.Count == 0)
            {
                return null;
            }
            string str = string.Empty;
            for (int i = 0; i < productIds.Count; i++)
            {
                str = str + productIds[i] + ",";
            }
            str = str.Remove(str.Length - 1);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Hishop_BrowseProductList WHERE ProductId IN({0})", str));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override bool InsertProductConsultation(ProductConsultationInfo productConsultation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductConsultations(ProductId, UserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productConsultation.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.String, productConsultation.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, productConsultation.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, productConsultation.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ConsultationText", DbType.String, productConsultation.ConsultationText);
            this.database.AddInParameter(sqlStringCommand, "ConsultationDate", DbType.DateTime, productConsultation.ConsultationDate);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool InsertProductReview(ProductReviewInfo review)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, review.ProductId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReviewText", DbType.String, review.ReviewText);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, review.UserName);
            this.database.AddInParameter(sqlStringCommand, "UserEmail", DbType.String, review.UserEmail);
            this.database.AddInParameter(sqlStringCommand, "ReviewDate", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool IsBuyProduct(int productId)
        {
            bool flag = false;
            try
            {
                string query = "select top 1 orders.UserId from Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where ProductId=@ProductId and orders.UserId=@UserId and orders.OrderStatus=@OrderStatus";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (reader.Read())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception)
            {
                flag = true;
            }
            return flag;
        }

        public override void LoadProductReview(int productId, out int buyNum, out int reviewNum)
        {
            buyNum = 0;
            reviewNum = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId SELECT ISNULL(SUM(Quantity), 0) FROM Hishop_OrderItems WHERE ProductId=@ProductId AND OrderId IN" + string.Format(" (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND OrderStatus != {0} AND OrderStatus != {1})", 1, 4));
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    reviewNum = (int) reader[0];
                }
                reader.NextResult();
                if (reader.Read())
                {
                    buyNum = (int) reader[0];
                }
            }
        }

        public override bool ProductExists(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(ProductId) from dbo.Hishop_Products where ProductId=@ProductId ");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }
    }
}

