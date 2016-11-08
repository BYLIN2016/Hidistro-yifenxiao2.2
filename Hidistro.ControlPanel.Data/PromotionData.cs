namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;
    using Hidistro.Entities.Members;

    public class PromotionData : PromotionsProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override int AddBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BundlingProducts;INSERT INTO Hishop_BundlingProducts(Name,ShortDescription,Num,Price,SaleStatus,AddTime,DisplaySequence) VALUES(@Name,@ShortDescription,@Num,@Price,@SaleStatus,@AddTime,@DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
            this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
            this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool AddBundlingProductItems(int bundlingID, List<BundlingItemInfo> BundlingItemInfos, DbTransaction dbTran)
        {
            if (BundlingItemInfos.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BundlingProductItems(BundlingID,ProductID,SkuId,ProductNum) VALUES(@BundlingID,@ProductID,@Skuid,@ProductNum)");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            this.database.AddInParameter(sqlStringCommand, "ProductID", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "ProductNum", DbType.Int32);
            try
            {
                foreach (BundlingItemInfo info in BundlingItemInfos)
                {
                    this.database.SetParameterValue(sqlStringCommand, "ProductID", info.ProductID);
                    this.database.SetParameterValue(sqlStringCommand, "SkuId", info.SkuId);
                    this.database.SetParameterValue(sqlStringCommand, "ProductNum", info.ProductNum);
                    if (dbTran != null)
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool AddCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_CountDown;INSERT INTO Hishop_CountDown(ProductId,CountDownPrice,StartDate,EndDate,Content,DisplaySequence,MaxCount ) VALUES(@ProductId,@CountDownPrice,@StartDate,@EndDate,@Content,@DisplaySequence,@MaxCount );");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int AddGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_GroupBuy;INSERT INTO Hishop_GroupBuy(ProductId,NeedPrice,StartDate,EndDate,MaxCount,Content,Status,DisplaySequence) VALUES(@ProductId,@NeedPrice,@StartDate,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
            this.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, groupBuy.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, DbTransaction dbTran)
        {
            if (gropBuyConditions.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_GroupBuyCondition(GroupBuyId,Count,Price) VALUES(@GroupBuyId,@Count,@Price)");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "Count", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency);
            try
            {
                foreach (GropBuyConditionInfo info in gropBuyConditions)
                {
                    this.database.SetParameterValue(sqlStringCommand, "Count", info.Count);
                    this.database.SetParameterValue(sqlStringCommand, "Price", info.Price);
                    if (dbTran != null)
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override int AddPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Promotions(Name, PromoteType, Condition, DiscountValue, StartDate, EndDate, Description) VALUES(@Name, @PromoteType, @Condition, @DiscountValue, @StartDate, @EndDate, @Description); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int) promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            object obj2 = null;
            if (dbTran != null)
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand, dbTran);
            }
            else
            {
                obj2 = this.database.ExecuteScalar(sqlStringCommand);
            }
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM Hishop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
            foreach (int num in memberGrades)
            {
                builder.AppendFormat(" INSERT INTO Hishop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, num);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddPromotionProducts(int activityId, string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Hishop_PromotionProducts SELECT @ActivityId, ProductId FROM Hishop_Products WHERE ProductId IN ({0})", productIds) + " AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts)");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            CouponActionStatus unknowError = CouponActionStatus.UnknowError;
            lotNumber = string.Empty;
            if (count <= 0)
            {
                lotNumber = string.Empty;
                if (null == coupon)
                {
                    return CouponActionStatus.UnknowError;
                }
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint); SELECT @@IDENTITY");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    unknowError = CouponActionStatus.CreateClaimCodeSuccess;
                }
                return unknowError;
            }
            unknowError = CouponActionStatus.CreateClaimCodeSuccess;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
            this.database.AddInParameter(storedProcCommand, "CouponId", DbType.Int32, coupon.CouponId);
            this.database.AddInParameter(storedProcCommand, "row", DbType.Int32, count);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, null);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, null);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, null);
            this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", DbType.String, 300);
            try
            {
                this.database.ExecuteNonQuery(storedProcCommand);
                lotNumber = (string) this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
            }
            catch
            {
                unknowError = CouponActionStatus.CreateClaimCodeError;
            }
            return unknowError;
        }

        public override GiftActionStatus CreateUpdateDeleteGift(GiftInfo gift, DataProviderAction action)
        {
            if (null == gift)
            {
                return GiftActionStatus.UnknowError;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Gift_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (DataProviderAction.Create != action)
            {
                this.database.AddInParameter(storedProcCommand, "GiftId", DbType.Int32, gift.GiftId);
            }
            else
            {
                this.database.AddOutParameter(storedProcCommand, "GiftId", DbType.Int32, 4);
            }
            if (DataProviderAction.Delete != action)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, gift.Name);
                this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, gift.ShortDescription);
                this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, gift.Unit);
                this.database.AddInParameter(storedProcCommand, "LongDescription", DbType.String, gift.LongDescription);
                this.database.AddInParameter(storedProcCommand, "Title", DbType.String, gift.Title);
                this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, gift.Meta_Description);
                this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, gift.Meta_Keywords);
                this.database.AddInParameter(storedProcCommand, "CostPrice", DbType.Currency, gift.CostPrice);
                this.database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, gift.ImageUrl);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, gift.ThumbnailUrl40);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, gift.ThumbnailUrl60);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, gift.ThumbnailUrl100);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, gift.ThumbnailUrl160);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, gift.ThumbnailUrl180);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, gift.ThumbnailUrl220);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, gift.ThumbnailUrl310);
                this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, gift.ThumbnailUrl410);
                this.database.AddInParameter(storedProcCommand, "PurchasePrice", DbType.Currency, gift.PurchasePrice);
                this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, gift.MarketPrice);
                this.database.AddInParameter(storedProcCommand, "NeedPoint", DbType.Int32, gift.NeedPoint);
                this.database.AddInParameter(storedProcCommand, "IsDownLoad", DbType.Boolean, gift.IsDownLoad);
                this.database.AddInParameter(storedProcCommand, "IsPromotion", DbType.Boolean, gift.IsPromotion);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "IsPromotion", DbType.Boolean, false);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (GiftActionStatus) ((int) this.database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override bool DeleteBundlingByID(int BundlingID, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProductItems WHERE  BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteBundlingProduct(int BundlingID)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, BundlingID);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteCountDown(int countDownId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteCoupon(int couponId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteGroupBuy(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteGroupBuyCondition(int groupBuyId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePromotion(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Promotions WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePromotionProducts(int activityId, int? productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId");
            if (productId.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND ProductId = {0}", productId.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool EditPromotion(PromotionInfo promotion, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Promotions SET Name = @Name, PromoteType = @PromoteType, Condition = @Condition, DiscountValue = @DiscountValue, StartDate = @StartDate, EndDate = @EndDate, Description = @Description WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, promotion.Name);
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int) promotion.PromoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, promotion.Condition);
            this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, promotion.DiscountValue);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, promotion.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, promotion.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, promotion.Description);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, promotion.ActivityId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override BundlingInfo GetBundlingInfo(int bundlingID)
        {
            BundlingInfo info = null;
            StringBuilder builder = new StringBuilder("SELECT * FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID;");
            builder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName,");
            builder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
            builder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bundlingID);
            List<BundlingItemInfo> list = new List<BundlingItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBindInfo(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    BundlingItemInfo item = new BundlingItemInfo();
                    item.ProductID = (int) reader["ProductID"];
                    item.ProductNum = (int) reader["ProductNum"];
                    if (reader["SkuId"] != DBNull.Value)
                    {
                        item.SkuId = (string) reader["SkuId"];
                    }
                    if (reader["ProductName"] != DBNull.Value)
                    {
                        item.ProductName = (string) reader["ProductName"];
                    }
                    if (reader["ProductPrice"] != DBNull.Value)
                    {
                        item.ProductPrice = (decimal) reader["ProductPrice"];
                    }
                    item.BundlingID = bundlingID;
                    list.Add(item);
                }
            }
            info.BundlingItemInfos = list;
            return info;
        }

        public override DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND Name Like '%{0}%' ", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "Bundlingid,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BundlingProducts", "Bundlingid", builder.ToString(), selectFields);
        }

        public override CountDownInfo GetCountDownInfo(int countDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
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

        public override DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat("ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "CountDownId,productId,ProductName,CountDownPrice,StartDate,EndDate,DisplaySequence,MaxCount ";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", builder.ToString(), selectFields);
        }

        public override CouponInfo GetCouponDetails(int couponId)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE CouponId = @CouponId");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCoupon(reader);
                }
            }
            return info;
        }

        public override IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            IList<CouponItemInfo> list = new List<CouponItemInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.String, lotNumber);
            CouponItemInfo item = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    item = DataMapper.PopulateCouponItem(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetCouponsList(CouponItemInfoQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.CouponId.HasValue)
            {
                builder.AppendFormat("CouponId = {0}", query.CouponId.Value);
            }
            if (!string.IsNullOrEmpty(query.CounponName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Name = '{0}'", query.CounponName);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.CouponStatus.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CouponInfo", "ClaimCode", builder.ToString(), "*");
        }

        public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", DbType.Int32, prodcutQuantity);
            return (decimal) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override GiftInfo GetGiftDetails(int giftId)
        {
            GiftInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId = @GiftId");
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGift(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            if (query.IsPromotion)
            {
                filter = filter + " AND IsPromotion = 1";
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public override GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId;SELECT * FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
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
                    info.GroupBuyConditions.Add(item);
                }
            }
            return info;
        }

        public override DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
            }
            string selectFields = "GroupBuyId,ProductId,ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,StartDate,EndDate,DisplaySequence";
            return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_GroupBuy", "GroupBuyId", builder.ToString(), selectFields);
        }

        public override IList<Member> GetMembersByRank(int? gradeId)
        {
            DbCommand sqlStringCommand;
            IList<Member> list = new List<Member>();
            if (gradeId > 0)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
                this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Member item = new Member(UserRole.Member);
                    item.UserId = (int) reader["UserId"];
                    item.Email = reader["Email"].ToString();
                    item.Username = reader["UserName"].ToString();
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetNewCoupons(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", string.Empty, "*");
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

        public override string GetPriceByProductId(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SalePrice FROM vw_Hishop_BrowseProductList WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return this.database.ExecuteScalar(sqlStringCommand).ToString();
        }

        public override IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public override PromotionInfo GetPromotion(int activityId)
        {
            PromotionInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId = @ActivityId; SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.MemberGradeIds.Add((int) reader["GradeId"]);
                }
            }
            return info;
        }

        public override DataTable GetPromotionProducts(int activityId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_BrowseProductList WHERE ProductId IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId) ORDER BY DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetPromotions(bool isProductPromote, bool isWholesale)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions");
            if (isProductPromote)
            {
                if (isWholesale)
                {
                    sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType = {0}", 4);
                }
                else
                {
                    sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType <> {0} AND PromoteType < 10", 4);
                }
            }
            else if (isWholesale)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType = {0} OR PromoteType = {1}", 13, 14);
            }
            else
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10", 13, 14);
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + " ORDER BY ActivityId DESC";
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override List<int> GetSendIds(int? gradeId, string userName)
        {
            List<int> list = new List<int>();
            string query = string.Format("SELECT UserId FROM vw_aspnet_Members WHERE UserName Like '%{0}%' ", DataHelper.CleanSearchString(userName));
            if (gradeId.HasValue)
            {
                string str2 = string.Format(" AND GradeId={0}", gradeId);
                query = query + str2;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    int item = Convert.ToInt32(reader[0]);
                    list.Add(item);
                }
            }
            return list;
        }

        public override bool ProductCountDownExist(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_CountDown WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ProductGroupBuyExist(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status=@Status");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus)");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponId);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            if (couponItem.UserId.HasValue)
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            if (!string.IsNullOrEmpty(couponItem.UserName))
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.String, 0);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId;UPDATE Hishop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, (int) status);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapCountDownSequence(int countDownId, int displaySequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET DisplaySequence = @DisplaySequence WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void SwapGroupBuySequence(int groupBuyId, int displaySequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET DisplaySequence = @DisplaySequence WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "DisplaySequence", DbType.Int32, displaySequence);
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateBundlingProduct(BundlingInfo bind, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BundlingProducts  SET Name=@Name,ShortDescription=@ShortDescription,Num=@Num,Price=@Price,SaleStatus=@SaleStatus,AddTime=@AddTime WHERE BundlingID=@BundlingID");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, bind.Name);
            this.database.AddInParameter(sqlStringCommand, "BundlingID", DbType.Int32, bind.BundlingID);
            this.database.AddInParameter(sqlStringCommand, "ShortDescription", DbType.String, bind.ShortDescription);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, bind.Num);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, bind.Price);
            this.database.AddInParameter(sqlStringCommand, "SaleStatus", DbType.Int32, bind.SaleStatus);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, bind.AddTime);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,StartDate=@StartDate,EndDate=@EndDate,Content=@Content,MaxCount=@MaxCount  WHERE CountDownId=@CountDownId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, countDownInfo.CountDownId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, countDownInfo.ProductId);
            this.database.AddInParameter(sqlStringCommand, "CountDownPrice", DbType.Currency, countDownInfo.CountDownPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, countDownInfo.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, countDownInfo.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, countDownInfo.Content);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, countDownInfo.MaxCount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            if (null != coupon)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, coupon.CouponId);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    return CouponActionStatus.DuplicateName;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint WHERE CouponId=@CouponId");
                this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.String, coupon.CouponId);
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, coupon.Name);
                this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, coupon.ClosingTime);
                this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, coupon.StartTime);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, coupon.Description);
                this.database.AddInParameter(sqlStringCommand, "DiscountValue", DbType.Currency, coupon.DiscountValue);
                this.database.AddInParameter(sqlStringCommand, "NeedPoint", DbType.Int32, coupon.NeedPoint);
                if (coupon.Amount.HasValue)
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, coupon.Amount.Value);
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, DBNull.Value);
                }
                if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                {
                    return CouponActionStatus.Success;
                }
            }
            return CouponActionStatus.UnknowError;
        }

        public override bool UpdateGroupBuy(GroupBuyInfo groupBuy, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,StartDate=@StartDate,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuy.GroupBuyId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, groupBuy.ProductId);
            this.database.AddInParameter(sqlStringCommand, "NeedPrice", DbType.Currency, groupBuy.NeedPrice);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, groupBuy.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, groupBuy.EndDate);
            this.database.AddInParameter(sqlStringCommand, "MaxCount", DbType.Int32, groupBuy.MaxCount);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, groupBuy.Content);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateIsDownLoad(int giftId, bool isdownload)
        {
            string query = "update Hishop_Gifts set IsDownLoad=@IsDownLoad  where GiftId = @GiftId;";
            if (!isdownload)
            {
                query = query + "delete from distro_Gifts where GiftId=@GiftId;";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "IsDownLoad", DbType.Boolean, isdownload);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }
    }
}

