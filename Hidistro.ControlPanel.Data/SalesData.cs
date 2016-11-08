namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SalesData : SalesProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddExpressTemplate(string expressName, string xmlFile)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ExpressTemplates(ExpressName, XmlFile, IsUse) VALUES(@ExpressName, @XmlFile, 1)");
            this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            this.database.AddInParameter(sqlStringCommand, "XmlFile", DbType.String, xmlFile);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddMemberPoint(UserPointInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) point.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddOrderGift(string orderId, OrderGiftInfo gift, int quantity, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    DbCommand command2 = this.database.GetSqlStringCommand("update Hishop_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
                    this.database.AddInParameter(command2, "OrderId", DbType.String, orderId);
                    this.database.AddInParameter(command2, "GiftId", DbType.Int32, gift.GiftId);
                    this.database.AddInParameter(command2, "Quantity", DbType.Int32, ((int) reader["Quantity"]) + quantity);
                    if (dbTran != null)
                    {
                        return (this.database.ExecuteNonQuery(command2, dbTran) == 1);
                    }
                    return (this.database.ExecuteNonQuery(command2) == 1);
                }
                DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Hishop_OrderGifts(OrderId,GiftId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity,@PromoType)");
                this.database.AddInParameter(command, "OrderId", DbType.String, orderId);
                this.database.AddInParameter(command, "GiftId", DbType.Int32, gift.GiftId);
                this.database.AddInParameter(command, "GiftName", DbType.String, gift.GiftName);
                this.database.AddInParameter(command, "CostPrice", DbType.Currency, gift.CostPrice);
                this.database.AddInParameter(command, "ThumbnailsUrl", DbType.String, gift.ThumbnailsUrl);
                this.database.AddInParameter(command, "Quantity", DbType.Int32, quantity);
                this.database.AddInParameter(command, "PromoType", DbType.Int16, gift.PromoteType);
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(command, dbTran) == 1);
                }
                return (this.database.ExecuteNonQuery(command) == 1);
            }
        }

        public override bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PurchaseOrderItems (PurchaseOrderId,SkuId, ProductId,SKU,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ItemHomeSiteDescription,ThumbnailsUrl,Weight,SKUContent)");
            builder.Append("VALUES(@PurchaseOrderId,@SkuId, @ProductId,@SKU,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ItemHomeSiteDescription,@ThumbnailsUrl,@Weight,@SKUContent);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, item.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, item.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, item.SKU);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, item.Quantity);
            this.database.AddInParameter(sqlStringCommand, "ItemListPrice", DbType.Currency, item.ItemListPrice);
            this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", DbType.Currency, item.ItemPurchasePrice);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency, item.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "ItemDescription", DbType.String, item.ItemDescription);
            this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription", DbType.String, item.ItemDescription);
            this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", DbType.String, item.ThumbnailsUrl);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, item.ItemWeight);
            this.database.AddInParameter(sqlStringCommand, "SKUContent", DbType.String, item.SKUContent);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
            }
            catch
            {
                return false;
            }
        }

        public override bool AddShipper(ShippersInfo shipper)
        {
            string query = string.Empty;
            if (shipper.IsDefault)
            {
                query = query + "UPDATE Hishop_Shippers SET IsDefault = 0";
            }
            query = query + " INSERT INTO Hishop_Shippers (IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark) VALUES (@IsDefault, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, shipper.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
        {
            StringBuilder builder = new StringBuilder();
            switch (saleStatisticsType)
            {
                case SaleStatisticsType.SaleCounts:
                    builder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.SaleTotal:
                    builder.Append("SELECT Isnull(SUM(OrderTotal),0)");
                    builder.Append(" FROM Hishop_orders WHERE  (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.Profits:
                    builder.Append("SELECT IsNull(SUM(OrderProfit),0) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                    break;
            }
            return builder.ToString();
        }

        private static string BuildMemberStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_aspnet_Members.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(OrderId) from Hishop_Orders where OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_aspnet_Members.UserId) as OrderCount ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            builder.Append(" from vw_aspnet_Members where Expenditure > 0");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE 1 = 1 ", new object[0]);
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            else
            {
                if (query.PaymentType.HasValue)
                {
                    builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
                }
                if (query.GroupBuyId.HasValue)
                {
                    builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (query.RegionId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.Status == OrderStatus.History)
                {
                    builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
                }
                else if (query.Status != OrderStatus.All)
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.ShippingModeId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
            builder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
            builder.AppendFormat(" FROM Hishop_OrderItems o  WHERE 0=0 ", new object[0]);
            builder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2})", 1, 4, 9);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.Append(" (SELECT ProductId FROM Hishop_Products)");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
            builder.Append(" FROM Hishop_products where SaleCounts>0");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public string BuildPurchaOrdersQuery(PurchaseOrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE 1 = 1 ", new object[0]);
            if (!string.IsNullOrEmpty(query.PurchaseOrderId))
            {
                builder.AppendFormat("AND PurchaseOrderId = '{0}'", query.PurchaseOrderId);
            }
            else
            {
                if (!string.IsNullOrEmpty(query.DistributorName))
                {
                    builder.AppendFormat(" AND DistributorName = '{0}'", query.DistributorName);
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", query.ShipTo);
                }
                if (!string.IsNullOrEmpty(query.OrderId))
                {
                    builder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    builder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (query.PurchaseStatus != OrderStatus.All)
                {
                    builder.AppendFormat(" AND PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',PurchaseDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND datediff(dd,'{0}',PurchaseDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                if (query.ShippingModeId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            return builder.ToString();
        }

        private static string BuildRegionsUserQuery(Pagination page)
        {
            if (null == page)
            {
                throw new ArgumentNullException("page");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT r.RegionId, r.RegionName, SUM(au.UserCount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT SUM(COUNT) FROM vw_aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r ");
            builder.Append(" WHERE (r.AreaId IS NOT NULL) AND ((au.path LIKE r.path + LTRIM(RTRIM(STR(r.RegionId))) + ',%') OR au.RegionId = r.RegionId)");
            builder.Append(" group by r.RegionId, r.RegionName ");
            builder.Append(" UNION SELECT 0, '0', sum(au.Usercount) AS Usercounts,");
            builder.Append(" (SELECT (SELECT count(*) FROM vw_aspnet_Members)) AS AllUserCounts ");
            builder.Append(" FROM vw_Allregion_Members au, Hishop_Regions r  ");
            builder.Append(" WHERE au.regionid IS NULL OR au.regionid = 0 group by r.RegionId, r.RegionName");
            if (!string.IsNullOrEmpty(page.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(page.SortBy), page.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildUserOrderQuery(UserOrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM Hishop_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
                return builder.ToString();
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public override bool ChangeMemberGrade(int userId, int gradId, int points)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM aspnet_MemberGrades Order by Point Desc ");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (((int) reader["GradeId"]) == gradId)
                    {
                        break;
                    }
                    if (((int) reader["Point"]) <= points)
                    {
                        return this.UpdateUserRank(userId, (int) reader["GradeId"]);
                    }
                }
                return true;
            }
        }

        public override bool CheckPurchaseRefund(string purchaseOrderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" update Hishop_PurchaseOrderRefund set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
            if ((refundType == 1) && accept)
            {
                builder.Append(" insert into Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                builder.AppendFormat(",Balance,Remark) select DistributorId as UserId,Distributorname as Username,getdate() as TradeDate,{0} as TradeType,", 5);
                builder.Append("PurchaseTotal as Income,isnull((select top 1 Balance from Hishop_DistributorBalanceDetails b");
                builder.AppendFormat(" where b.UserId=a.DistributorId order by JournalNumber desc),0)+PurchaseTotal as Balance,'采购单{0}退款' as Remark ", purchaseOrderId);
                builder.Append("from Hishop_PurchaseOrders a where PurchaseOrderId = @PurchaseOrderId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckPurchaseReplace(string purchaseOrderId, string adminRemark, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" update Hishop_PurchaseOrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckPurchaseReturn(string purchaseOrderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" update Hishop_PurchaseOrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,RefundMoney=@RefundMoney,HandleTime=@HandleTime where HandleStatus=0 and PurchaseOrderId = @PurchaseOrderId;");
            if ((refundType == 1) && accept)
            {
                builder.Append(" insert into Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                builder.AppendFormat(",Balance,Remark) select DistributorId as UserId,Distributorname as Username,getdate() as TradeDate,{0} as TradeType,", 7);
                builder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Hishop_DistributorBalanceDetails b");
                builder.AppendFormat(" where b.UserId=a.DistributorId order by JournalNumber desc),0)+@RefundMoney as Balance,'采购单{0}退货' as Remark ", purchaseOrderId);
                builder.Append("from Hishop_PurchaseOrders a where PurchaseOrderId = @PurchaseOrderId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 10);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
            if ((refundType == 1) && accept)
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                if (orderInfo != null)
                {
                    Member user = Users.GetUser(orderInfo.UserId, false) as Member;
                    decimal total = orderInfo.GetTotal();
                    decimal num2 = user.Balance + total;
                    if (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
                    {
                        total -= orderInfo.NeedPrice;
                        num2 -= orderInfo.NeedPrice;
                    }
                    builder.Append("insert into Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark)");
                    builder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}')", new object[] { user.UserId, user.Username, "getdate()", 5, total, num2, "订单" + orderId + "退款" });
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckReplace(string orderId, string adminRemark, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckReturn(string orderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderReturns set Operator=@Operator, AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime,RefundMoney=@RefundMoney where HandleStatus=0 and OrderId = @OrderId;");
            if ((refundType == 1) && accept)
            {
                builder.Append(" insert into Hishop_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                builder.AppendFormat(",Balance,Remark) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
                builder.Append("@RefundMoney as Income,isnull((select top 1 Balance from Hishop_BalanceDetails b");
                builder.AppendFormat(" where b.UserId=a.UserId order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark ", orderId);
                builder.Append("from Hishop_Orders a where OrderId = @OrderId;");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 10);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearOrderGifts(string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,CloseReason=@CloseReason WHERE PurchaseOrderId = @PurchaseOrderId ");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, purchaseOrder.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 4);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CloseTransaction(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderStatus=@OrderStatus,CloseReason=@CloseReason WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ConfirmOrderFinish(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET FinishDate = @FinishDate, OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int ConfirmPay(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PayDate = @PayDate, OrderStatus = @OrderStatus ,OrderPoint=@OrderPoint WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PayDate = @PayDate, PurchaseStatus=@PurchaseStatus WHERE PurchaseOrderId = @PurchaseOrderId ");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 2);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET  PurchaseStatus = @PurchaseStatus, FinishDate=@FinishDate WHERE PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Create");
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    try
                    {
                        this.database.ExecuteNonQuery(storedProcCommand, transaction);
                        flag = ((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0;
                        if (flag)
                        {
                            int parameterValue = (int) this.database.GetParameterValue(storedProcCommand, "ModeId");
                            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, parameterValue);
                            StringBuilder builder = new StringBuilder();
                            int num2 = 0;
                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                            foreach (string str in shippingMode.ExpressCompany)
                            {
                                builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num2, DbType.String, str);
                                num2++;
                            }
                            sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();
                            int num3 = (int) this.database.ExecuteScalar(sqlStringCommand, transaction);
                            if (num3 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        flag = false;
                    }
                    return flag;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ShippingTemplates(TemplateName,Weight,AddWeight,Price,AddPrice) VALUES(@TemplateName,@Weight,@AddWeight,@Price,@AddPrice);SELECT @@Identity");
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, shippingMode.Weight);
            if (shippingMode.AddWeight.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, shippingMode.AddWeight);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Int32, 0);
            }
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            if (shippingMode.AddPrice.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, 0);
            }
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    try
                    {
                        object obj2 = this.database.ExecuteScalar(sqlStringCommand, transaction);
                        int result = 0;
                        if ((obj2 != null) && (obj2 != DBNull.Value))
                        {
                            int.TryParse(obj2.ToString(), out result);
                            flag = result > 0;
                        }
                        if (flag)
                        {
                            DbCommand command = this.database.GetSqlStringCommand(" ");
                            this.database.AddInParameter(command, "TemplateId", DbType.Int32, result);
                            if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                            {
                                StringBuilder builder = new StringBuilder();
                                int num2 = 0;
                                int num3 = 0;
                                builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                                builder.Append(" DECLARE @GroupId Int;");
                                foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                                {
                                    builder.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num2).Append(",@AddPrice").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    this.database.AddInParameter(command, "Price" + num2, DbType.Currency, info.Price);
                                    this.database.AddInParameter(command, "AddPrice" + num2, DbType.Currency, info.AddPrice);
                                    builder.Append("Set @GroupId =@@identity;");
                                    foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                    {
                                        builder.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num3).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                        this.database.AddInParameter(command, "RegionId" + num3, DbType.Int32, info2.RegionId);
                                        num3++;
                                    }
                                    num2++;
                                }
                                command.CommandText = builder.Append("SELECT @ERR;").ToString();
                                int num4 = (int) this.database.ExecuteScalar(command, transaction);
                                if (num4 != 0)
                                {
                                    transaction.Rollback();
                                    flag = false;
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        flag = false;
                    }
                    return flag;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Date", typeof(int));
            DataColumn column2 = new DataColumn("SaleTotal", typeof(decimal));
            DataColumn column3 = new DataColumn("Percentage", typeof(decimal));
            DataColumn column4 = new DataColumn("Lenth", typeof(decimal));
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            return table;
        }

        public override PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PaymentType_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action == DataProviderAction.Create)
            {
                this.database.AddOutParameter(storedProcCommand, "ModeId", DbType.Int32, 4);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, paymentMode.ModeId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "Name", DbType.String, paymentMode.Name);
                this.database.AddInParameter(storedProcCommand, "Description", DbType.String, paymentMode.Description);
                this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, paymentMode.Gateway);
                this.database.AddInParameter(storedProcCommand, "IsUseInpour", DbType.Boolean, paymentMode.IsUseInpour);
                this.database.AddInParameter(storedProcCommand, "IsUseInDistributor", DbType.Boolean, paymentMode.IsUseInDistributor);
                this.database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
                this.database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
                this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, paymentMode.Settings);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (PaymentModeActionStatus) ((int) this.database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override bool DelDebitNote(string noteId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderDebitNote WHERE NoteId='{0}'", noteId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteExpressTemplate(int expressId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ExpressTemplates WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override int DeleteOrders(string orderIds)
        {
            string query = string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeletePurchaseOrderItem(string POrderId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DeletePurchaseOrders(string purchaseOrderIds)
        {
            string query = string.Format("DELETE FROM Hishop_PurchaseOrders WHERE PurchaseOrderId IN({0})", purchaseOrderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteShippingMode(int modeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_TemplateRelatedShipping Where ModeId=@ModeId;DELETE FROM Hishop_ShippingTypes Where ModeId=@ModeId;UPDATE Hishop_PurchaseOrders set ShippingModeId=0 where ShippingModeId=@ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            this.database.AddOutParameter(sqlStringCommand, "Status", DbType.Int32, 4);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteShippingTemplate(int templateId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShippingTemplates Where TemplateId=@TemplateId");
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelPurchaseDebitNote(string noteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseDebitNote WHERE NoteId='{0}'", noteId));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelPurchaseRefundApply(int refundId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelPurchaseReplaceApply(int replaceId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelPurchaseReturnsApply(int returnId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseOrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnId));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelPurchaseSendNote(string noteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_PurchaseSendNote WHERE NoteId='{0}'", noteId));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelRefundApply(int refundId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelReplaceApply(int replaceId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelReturnsApply(int returnsId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnsId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelSendNote(string noteId)
        {
            string query = string.Format("DELETE FROM Hishop_OrderSendNote WHERE NoteId='{0}'", noteId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool EditOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool EditPurchaseOrderShipNumber(string purchaseOrderId, string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber=@ShipOrderNumber WHERE PurchaseOrderId =@PurchaseOrderId");
            if (!string.IsNullOrEmpty(orderId))
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " UPDATE distro_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId";
            }
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            if (!string.IsNullOrEmpty(orderId))
            {
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override decimal GetAddUserTotal(int year)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate)  AS UserAdd");
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            return ((obj2 == null) ? 0M : Convert.ToDecimal(obj2));
        }

        public override DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderDebitNote", "NoteId", builder.ToString(), "*");
        }

        public override DbQueryResult GetAllPurchaseDebitNote(DebitNoteQuery query)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                filter = string.Format("PurchaseOrderId = '{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseDebitNote", "NoteId", filter, "*");
        }

        public override DbQueryResult GetAllPurchaseSendNote(RefundApplyQuery query)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                filter = string.Format("PurchaseOrderId = '{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseSendNote", "NoteId", filter, "*");
        }

        public override DbQueryResult GetAllSendNote(RefundApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderSendNote", "OrderId", builder.ToString(), "*");
        }

        public override DataSet GetAPIPurchaseOrders(PurchaseOrderQuery query, out int records)
        {
            DataSet set = new DataSet();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_PurchaseOrder_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildPurchaOrdersQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", DbType.Int32, 4);
            using (set = this.database.ExecuteDataSet(storedProcCommand))
            {
                set.Relations.Add("PurchaseOrderRelation", set.Tables[0].Columns["PurchaseOrderId"], set.Tables[1].Columns["PurchaseOrderId"]);
            }
            records = (int) this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
            return set;
        }

        public override int GetCurrentPOrderItemQuantity(string POrderId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Quantity FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != DBNull.Value) && (obj2 != null))
            {
                return (int) obj2;
            }
            return 0;
        }

        private int GetDayCount(int year, int month)
        {
            if (month == 2)
            {
                if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0))
                {
                    return 0x1d;
                }
                return 0x1c;
            }
            if (((((month == 1) || (month == 3)) || ((month == 5) || (month == 7))) || ((month == 8) || (month == 10))) || (month == 12))
            {
                return 0x1f;
            }
            return 30;
        }

        public override DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            decimal allSalesTotal = this.GetMonthSaleTotal(year, month, saleStatisticsType);
            int dayCount = this.GetDayCount(year, month);
            int num3 = ((year == DateTime.Now.Year) && (month == DateTime.Now.Month)) ? DateTime.Now.Day : dayCount;
            for (int i = 1; i <= num3; i++)
            {
                DateTime time = new DateTime(year, month, i);
                DateTime time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                this.InsertToTable(table, i, salesTotal, allSalesTotal);
            }
            return table;
        }

        public override decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, day);
            DateTime time2 = time.AddDays(1.0);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public override IList<string> GetExpressCompanysByMode(int modeId)
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        list.Add((string) reader["ExpressCompanyName"]);
                    }
                }
            }
            return list;
        }

        public override DataTable GetExpressTemplates()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<GiftInfo> GetGiftList(GiftQuery query)
        {
            IList<GiftInfo> list = new List<GiftInfo>();
            string str = string.Format("SELECT * FROM Hishop_Gifts WHERE [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateGift(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetGifts(GiftQuery query)
        {
            string filter = null;
            if (!string.IsNullOrEmpty(query.Name))
            {
                filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            Pagination page = query.Page;
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
        }

        public override int GetHistoryPoint(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM Hishop_PointDetails WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DataTable GetIsUserExpressTemplates()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ExpressTemplates WHERE IsUse = 1");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<LineItemInfo> GetLineItemInfo(string orderId)
        {
            List<LineItemInfo> list = new List<LineItemInfo>();
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(DataMapper.PopulateLineItem(reader));
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public override LineItemInfo GetLineItemInfo(string skuId, string orderId)
        {
            LineItemInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderItems WHERE SkuId=@SkuId AND OrderId=@OrderId");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateLineItem(reader);
                }
            }
            return info;
        }

        public override DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildMemberStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(BuildMemberStatisticsQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
            for (int i = 1; i <= num; i++)
            {
                DateTime time = new DateTime(year, i, 1);
                DateTime time2 = time.AddMonths(1);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                decimal yearSaleTotal = this.GetYearSaleTotal(year, saleStatisticsType);
                this.InsertToTable(table, i, salesTotal, yearSaleTotal);
            }
            return table;
        }

        public override decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, 1);
            DateTime time2 = time.AddMonths(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public override OrderGiftInfo GetOrderGift(int giftId, string orderId)
        {
            OrderGiftInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrderGift(reader);
                }
            }
            return info;
        }

        public override DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("select top {0} * from Hishop_OrderGifts where OrderId=@OrderId", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.Append(" ORDER BY GiftId ASC");
            }
            else
            {
                builder.AppendFormat(" and GiftId > (select max(GiftId) from (select top {0} GiftId from Hishop_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(GiftId) as Total from Hishop_OrderGifts where OrderId=@OrderId", new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, query.OrderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (query.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override DataSet GetOrderGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
            builder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
            builder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId; SELECT  * FROM Hishop_OrderGifts Where OrderId = @OrderId; SELECT * FROM Hishop_OrderItems Where OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.Gifts.Add(DataMapper.PopulateOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add((string) reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public override IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
        {
            IList<OrderPriceStatisticsForChartInfo> list = new List<OrderPriceStatisticsForChartInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT isnull((SELECT sum(Amount) FROM Hishop_Orders WHERE OrderDate BETWEEN @StartDate AND @EndDate),0) AS Price");
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DateTime date = new DateTime();
            DateTime time2 = new DateTime();
            date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double) -days);
            for (int i = 1; i <= days; i++)
            {
                OrderPriceStatisticsForChartInfo item = new OrderPriceStatisticsForChartInfo();
                if (i > 1)
                {
                    date = time2;
                }
                time2 = date.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                item.Price = (decimal) this.database.ExecuteScalar(sqlStringCommand);
                item.TimePoint = date.Day;
                list.Add(item);
            }
            return list;
        }

        public override DbQueryResult GetOrders(OrderQuery query)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Orders_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildOrdersQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
            DbQueryResult result = new DbQueryResult();
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            result.TotalRecords = (int) this.database.GetParameterValue(storedProcCommand, "TotalOrders");
            return result;
        }

        public override DataSet GetOrdersAndLines(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM Hishop_Orders WHERE OrderStatus > 0 AND OrderStatus < 4 AND OrderId IN ({0}) order by OrderDate desc ", orderIds);
            builder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
            builder.AppendFormat(" SELECT * FROM Hishop_OrderGifts WHERE OrderId IN ({0});", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override DataSet GetProductGoods(string orderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
            builder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            builder.AppendFormat("SELECT OrderId AS GiftOrderId,GiftName,Quantity FROM dbo.Hishop_OrderGifts WHERE OrderId IN({0}) AND OrderId IN(SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductVisitAndBuyStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductVisitAndBuyStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
            builder.Append("FROM Hishop_Products WHERE SaleCounts>0 ORDER BY BuyPercentage DESC;");
            builder.Append("SELECT COUNT(*) as TotalProductSales FROM Hishop_Products WHERE SaleCounts>0;");
            sqlStringCommand.CommandText = builder.ToString();
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    table = DataHelper.ConverDataReaderToDataTable(reader);
                }
                if (reader.NextResult() && reader.Read())
                {
                    totalProductSales = (int) reader["TotalProductSales"];
                    return table;
                }
                totalProductSales = 0;
            }
            return table;
        }

        public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            PurchaseOrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId SELECT  * FROM Hishop_PurchaseOrderGifts Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderItems Where PurchaseOrderId = @PurchaseOrderId ");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePurchaseOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.PurchaseOrderGifts.Add(DataMapper.PopulatePurchaseOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.PurchaseOrderItems.Add(DataMapper.PopulatePurchaseOrderItem(reader));
                }
            }
            return info;
        }

        public override DataSet GetPurchaseOrderGoods(string purchaseorderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT PurchaseOrderId, ItemHomeSiteDescription AS ProductName, SKU, SKUContent, Quantity as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = po.SkuId) + Quantity AS Stock, (SELECT Remark FROM Hishop_PurchaseOrders WHERE PurchaseOrderId = po.PurchaseOrderId) AS Remark");
            builder.Append(" FROM Hishop_PurchaseOrderItems po WHERE PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.AppendFormat(" AND PurchaseOrderId IN ({0}) ORDER BY PurchaseOrderId;", purchaseorderIds);
            builder.AppendFormat("SELECT PurchaseOrderId AS GiftPurchaseOrderId,GiftName,Quantity FROM dbo.Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN({0}) AND PurchaseOrderId IN(SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", purchaseorderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.PurchaseOrderId))
            {
                builder.AppendFormat("PurchaseOrderId = '{0}'", query.PurchaseOrderId);
            }
            else
            {
                builder.Append(" 1=1");
                if (!string.IsNullOrEmpty(query.DistributorName))
                {
                    builder.AppendFormat(" AND DistributorName = '{0}'", query.DistributorName);
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", query.ShipTo);
                }
                if (!string.IsNullOrEmpty(query.OrderId))
                {
                    builder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
                }
                if (!string.IsNullOrEmpty(query.ProductName))
                {
                    builder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND PurchaseDate >= '{0}'", query.StartDate.Value);
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND PurchaseDate <= '{0}'", query.EndDate.Value);
                }
                if (query.PurchaseStatus != OrderStatus.All)
                {
                    builder.AppendFormat(" AND PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
                }
                if (query.ShippingModeId.HasValue)
                {
                    builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
                }
                if (query.IsPrinted.HasValue)
                {
                    builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_PurchaseOrders", "PurchaseOrderId", builder.ToString(), "OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal,AdjustedDiscount, PaymentType,Gateway,PurchaseStatus, Distributorname,DistributorWangwang,ManagerMark,ManagerRemark,DistributorId,ISNULL(IsPrinted,0) IsPrinted,ShipOrderNumber");
        }

        public override DataSet GetPurchaseOrdersAndLines(string purchaseOrderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM Hishop_PurchaseOrders WHERE PurchaseStatus > 0 AND PurchaseStatus < 4 AND PurchaseOrderId IN ({0}) order by PurchaseDate desc ", purchaseOrderIds);
            builder.AppendFormat(";SELECT * FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId IN ({0});", purchaseOrderIds);
            builder.AppendFormat("SELECT * FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN ({0});", purchaseOrderIds);
            builder.Append("SELECT * FROM Hishop_Shippers ORDER BY DistributorUserId, IsDefault DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override DataSet GetPurchaseProductGoods(string purchaseorderIds)
        {
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ItemHomeSiteDescription AS ProductName, SKU, SKUContent, sum(Quantity) as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = po.SkuId) + sum(Quantity) AS Stock FROM Hishop_PurchaseOrderItems po");
            builder.Append(" WHERE PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.AppendFormat(" AND PurchaseOrderId in ({0}) GROUP BY ItemHomeSiteDescription, SkuId, SKU, SKUContent;", purchaseorderIds);
            builder.AppendFormat("SELECT PurchaseOrderId AS GiftPurchaseOrderId,GiftName,Quantity FROM dbo.Hishop_PurchaseOrderGifts WHERE PurchaseOrderId IN({0}) AND PurchaseOrderId IN(SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2 or (PurchaseStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))", purchaseorderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override DbQueryResult GetPurchaseRefundApplys(RefundApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderRefund", "RefundId", builder.ToString(), "*");
        }

        public override void GetPurchaseRefundType(string purchaseOrderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,RefundRemark from Hishop_PurchaseOrderRefund where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["RefundRemark"];
            }
        }

        public override void GetPurchaseRefundTypeFromReturn(string purchaseOrderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,Comments from Hishop_PurchaseOrderReturns where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["Comments"];
            }
        }

        public override DbQueryResult GetPurchaseReplaceApplys(ReplaceApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderReplace", "ReplaceId", builder.ToString(), "*");
        }

        public override string GetPurchaseReplaceComments(string purchaseOrderId)
        {
            string query = "select Comments from Hishop_PurchaseOrderReplace where HandleStatus=0 and PurchaseOrderId='" + purchaseOrderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 is DBNull))
            {
                return "";
            }
            return obj2.ToString();
        }

        public override DbQueryResult GetPurchaseReturnsApplys(ReturnsApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and PurchaseOrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PurchaseOrderReturns", "ReturnsId", builder.ToString(), "*");
        }

        public override DataTable GetRecentlyOrders(out int number)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal,ISNULL(GroupBuyId,0) as GroupBuyId,ISNULL(GroupBuyStatus,0) as GroupBuyStatus, PaymentType,ManagerMark, OrderStatus, RefundStatus,ManagerRemark FROM Hishop_Orders ORDER BY OrderDate DESC");
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_Orders WHERE  OrderStatus=2");
            number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DataTable GetRecentlyPurchaseOrders(out int number)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders ORDER BY PurchaseDate DESC ");
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE  PurchaseStatus=2 ");
            number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderRefund", "RefundId", builder.ToString(), "*");
        }

        public override void GetRefundType(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,RefundRemark from Hishop_OrderRefund where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["RefundRemark"];
            }
        }

        public override void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            refundType = 0;
            remark = "";
            string query = "select RefundType,Comments from Hishop_OrderReturns where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                remark = (string) reader["Comments"];
            }
        }

        public override DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReplace", "ReplaceId", builder.ToString(), "*");
        }

        public override string GetReplaceComments(string orderId)
        {
            string query = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 is DBNull))
            {
                return "";
            }
            return obj2.ToString();
        }

        public override DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1");
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderReturns", "ReturnsId", builder.ToString(), "*");
        }

        public override DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat("orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat("orderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_SaleDetails", "OrderId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_SaleDetails WHERE 1=1");
            if (query.StartDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate >= '{0}'", query.StartDate);
            }
            if (query.EndDate.HasValue)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format(" AND OrderDate <= '{0}'", query.EndDate.Value.AddDays(1.0));
            }
            sqlStringCommand.CommandText = sqlStringCommand.CommandText + string.Format("AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public override DbQueryResult GetSaleTargets()
        {
            DbQueryResult result = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("select (select Count(OrderId) from Hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1}  AND OrderStatus != {2}) as OrderNumb,", 1, 4, 9) + string.Format("(select ISNULL(sum(OrderTotal) - sum(RefundAmount),0) from hishop_orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}) as OrderPrice, ", 1, 4, 9) + " (select COUNT(*) from vw_aspnet_Members) as UserNumb,  (select count(*) from vw_aspnet_Members where UserID in (select userid from Hishop_orders)) as UserOrderedNumb,  ISNULL((select sum(VistiCounts) from Hishop_products),0) as ProductVisitNumb ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public override DataTable GetSendGoodsOrders(string orderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId, OrderDate,RefundStatus, ShipTo, OrderTotal, OrderStatus,ShippingRegion,Address,ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber,ExpressCompanyAbb," + string.Format(" ExpressCompanyName FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId, PurchaseOrderId, PurchaseDate,RefundStatus, ShipTo, OrderTotal, PurchaseTotal, PurchaseStatus,Distributorname,ShippingRegion,Address, ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,ShipOrderNumber,ExpressCompanyAbb,ExpressCompanyName FROM Hishop_PurchaseOrders" + string.Format(" WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0}) order by PurchaseDate desc", purchaseOrderIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ShippersInfo GetShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            ShippersInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShipper(reader);
                }
            }
            return info;
        }

        public override IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers");
            if (!includeDistributor)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " WHERE DistributorUserId = 0";
            }
            IList<ShippersInfo> list = new List<ShippersInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShipper(reader));
                }
            }
            return list;
        }

        public override DataTable GetShippingAllTemplates()
        {
            string query = "SELECT * FROM Hishop_ShippingTemplates ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId";
            }
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingMode(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        info.ExpressCompany.Add((string) reader["ExpressCompanyName"]);
                    }
                }
            }
            return info;
        }

        public override ShippingModeInfo GetShippingModeByCompany(string companyname)
        {
            ShippingModeInfo info = new ShippingModeInfo();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId AND st.ModeId IN (SELECT ModeId FROM Hishop_TemplateRelatedShipping WHERE ExpressCompanyName='" + DataHelper.CleanSearchString(companyname) + "')";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingMode(reader);
                }
            }
            return info;
        }

        public override IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId";
            if (!string.IsNullOrEmpty(paymentGateway))
            {
                query = query + " WHERE Gateway = @Gateway)";
            }
            query = query + " Order By DisplaySequence";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            if (!string.IsNullOrEmpty(paymentGateway))
            {
                this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, paymentGateway);
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public override ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTemplates Where TemplateId =@TemplateId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId =@TemplateId";
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT sr.TemplateId,sr.GroupId,sr.RegionId FROM Hishop_ShippingRegions sr Where sr.TemplateId =@TemplateId";
            }
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, templateId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingTemplate(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    foreach (ShippingModeGroupInfo info2 in info.ModeGroup)
                    {
                        if (info2.GroupId == ((int) reader["GroupId"]))
                        {
                            info2.ModeRegions.Add(DataMapper.PopulateShippingRegion(reader));
                        }
                    }
                }
            }
            return info;
        }

        public override DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return DataHelper.PagingByRownumber(pagin.PageIndex, pagin.PageSize, pagin.SortBy, pagin.SortOrder, pagin.IsCount, "Hishop_ShippingTemplates", "TemplateId", "", "*");
        }

        public override int GetSkuStock(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override AdminStatisticsInfo GetStatistics()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "SELECT  (SELECT COUNT(OrderId) FROM Hishop_Orders WHERE OrderStatus = 2) AS orderNumbWaitConsignment, (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 2) AS purchaseOrderNumbWaitConsignment, (select Count(LeaveId) from Hishop_LeaveComments l where (select count(replyId) from Hishop_LeaveCommentReplys where leaveId =l.leaveId)=0) as leaveComments,(select Count(ConsultationId) from Hishop_ProductConsultations where ReplyUserId is null) as productConsultations,(select Count(*) from Hishop_ManagerMessageBox where IsRead=0 and Accepter='admin' and Sernder in (select UserName from vw_aspnet_Members)) as messages, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and OrderDate>='", DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date), "'),0) as orderPriceToday, isnull((select sum(PurchaseProfit) from Hishop_PurchaseOrders where  (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and PurchaseDate>='", DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date), "'),0) as PurchaseOrderProfitToday, isnull((select sum(OrderProfit) from Hishop_Orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)  and OrderDate>='", DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date), "'),0) as orderProfitToday, (select count(*) from vw_aspnet_Members where CreateDate>='", DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date), "' ) as userNewAddToday, (select count(*) from vw_aspnet_Distributors where CreateDate>='", DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date), "' and IsApproved=1) as distroNewAddToday, isnull((select sum(balance) from vw_aspnet_Members),0) as memberBalance, isnull((select sum(balance) from vw_aspnet_Distributors),0) as distroBalance,(select count(*) from (select ProductId from Hishop_SKUs where Stock<=AlertStock group by ProductId) as a) as productAlert,(select count(PurchaseOrderId) from Hishop_PurchaseOrders where PurchaseStatus=", 1, ") as purchaseOrderNumbWait,(select count(*) from Hishop_BalanceDrawRequest) as memberBlancedraw,(select count(*) from Hishop_DistributorBalanceDrawRequest) as distributorBlancedraw,(select count(*) from Hishop_SiteRequest where RequestStatus=1) as distributorSiteRequest,(select count(*) from Hishop_Orders where datediff(dd,getdate(),OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as todayFinishOrder,(select count(*) from Hishop_Orders where datediff(dd,getdate()-1,OrderDate)=0 and (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)) as yesterdayFinishOrder, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,getdate()-1,OrderDate)=0),0) as orderPriceYesterDay,(select count(*) from Hishop_PurchaseOrders where datediff(dd,getdate(),PurchaseDate)=0 and (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)) as todayFinishPurchaseOrder,(select count(*) from Hishop_PurchaseOrders where datediff(dd,getdate()-1,PurchaseDate)=0 and (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)) as yesterdayFinishPurchaseOrder, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,getdate(),PurchaseDate)=0),0) as purchaseorderPriceToDay, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,getdate()-1,PurchaseDate)=0),0) as purchaseorderPriceYesterDay,(select count(*) from vw_aspnet_Members where datediff(dd,getdate()-1,CreateDate)=0) as userNewAddYesterToday,(select count(*) from vw_aspnet_Distributors where datediff(dd,getdate()-1,CreateDate)=0) as distroNewAddYesterToday,(select count(*) from vw_aspnet_Members) as TotalMembers,(select count(*) from vw_aspnet_Distributors) as TotalDistributors,(select count(*) from Hishop_Products where SaleStatus!=0) as TotalProducts, isnull((select sum(OrderTotal)-isnull(sum(RefundAmount),0) from hishop_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9)   and datediff(dd,OrderDate,getdate())<=30),0) as orderPriceMonth, isnull((select sum(PurchaseTotal)-isnull(sum(RefundAmount),0) from Hishop_PurchaseOrders where (PurchaseStatus<>1 AND PurchaseStatus<>4 AND PurchaseStatus=9)   and datediff(dd,PurchaseDate,getdate())<=30),0) as purchaseorderPriceMonth" }));
            AdminStatisticsInfo info = new AdminStatisticsInfo();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info.OrderNumbWaitConsignment = (int) reader["orderNumbWaitConsignment"];
                    info.PurchaseOrderNumbWaitConsignment = (int) reader["purchaseOrderNumbWaitConsignment"];
                    info.LeaveComments = (int) reader["LeaveComments"];
                    info.ProductConsultations = (int) reader["ProductConsultations"];
                    info.Messages = (int) reader["Messages"];
                    info.PurchaseOrderProfitToday = (decimal) reader["PurchaseOrderProfitToday"];
                    info.OrderProfitToday = (decimal) reader["orderProfitToday"];
                    info.UserNewAddToday = (int) reader["userNewAddToday"];
                    info.DistroButorsNewAddToday = (int) reader["distroNewAddToday"];
                    info.MembersBalance = (decimal) reader["memberBalance"];
                    info.DistrosBalance = (decimal) reader["distroBalance"];
                    info.OrderPriceToday = (decimal) reader["orderPriceToday"];
                    info.ProductAlert = (int) reader["productAlert"];
                    info.PurchaseOrderNumbWait = (int) reader["purchaseOrderNumbWait"];
                    info.MemberBlancedrawRequest = (int) reader["memberBlancedraw"];
                    info.DistributorBlancedrawRequest = (int) reader["distributorBlancedraw"];
                    info.DistributorSiteRequest = (int) reader["distributorSiteRequest"];
                    info.TodayFinishOrder = (int) reader["todayFinishOrder"];
                    info.YesterdayFinishOrder = (int) reader["yesterdayFinishOrder"];
                    info.OrderPriceYesterDay = (decimal) reader["orderPriceYesterDay"];
                    info.TodayFinishPurchaseOrder = (int) reader["todayFinishPurchaseOrder"];
                    info.YesterdayFinishPurchaseOrder = (int) reader["yesterdayFinishPurchaseOrder"];
                    info.PurchaseorderPriceToDay = (decimal) reader["purchaseorderPriceToDay"];
                    info.PurchaseorderPriceYesterDay = (decimal) reader["purchaseorderPriceYesterDay"];
                    info.UserNewAddYesterToday = (int) reader["userNewAddYesterToday"];
                    info.DistroNewAddYesterToday = (int) reader["distroNewAddYesterToday"];
                    info.TotalMembers = (int) reader["TotalMembers"];
                    info.TotalDistributors = (int) reader["TotalDistributors"];
                    info.TotalProducts = (int) reader["TotalProducts"];
                    info.OrderPriceMonth = (decimal) reader["OrderPriceMonth"];
                    info.PurchaseorderPriceMonth = (decimal) reader["PurchaseorderPriceMonth"];
                }
            }
            return info;
        }

        public override DataSet GetTradeOrders(string orderId)
        {
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OrderId,0 as SellerUid,Username,EmailAddress,ShipTo,ShippingRegion,Address,ZipCode,CellPhone,TelPhone,Remark,ManagerMark,ManagerRemark,(select sum(Quantity) from Hishop_OrderItems where Hishop_OrderItems.OrderId=p.OrderId) as Nums,OrderTotal,OrderTotal,AdjustedFreight,DiscountValue,AdjustedDiscount,PayDate,ShippingDate,ReFundStatus,OrderStatus FROM Hishop_Orders as p Where OrderId in (" + orderId + ") order by OrderId; SELECT 0 as Tid,OrderId,ProductId,ItemDescription,SKU,SKUContent,Quantity,ItemListPrice,ItemAdjustedPrice,'0.00' as DiscountFee,'0.00' as Fee,'-1' as RefundStatus,'-1' as [Types],'-1' as [Status] FROM Hishop_OrderItems Where OrderId in (" + orderId + ")  order by OrderId");
            using (set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                set.Relations.Add("OrderDetailsRelation", set.Tables[0].Columns["OrderId"], set.Tables[1].Columns["OrderId"]);
            }
            return set;
        }

        public override DataSet GetTradeOrders(OrderQuery query, out int records)
        {
            DataSet set = new DataSet();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_Orders_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildOrdersQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
            using (set = this.database.ExecuteDataSet(storedProcCommand))
            {
                set.Relations.Add("OrderRelation", set.Tables[0].Columns["OrderId"], set.Tables[1].Columns["OrderId"]);
            }
            records = (int) this.database.GetParameterValue(storedProcCommand, "TotalOrders");
            return set;
        }

        public override IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            int num;
            UserStatisticsForDate date;
            int num4;
            IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate) AS UserAdd ");
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DateTime time = new DateTime();
            DateTime time2 = new DateTime();
            if (days.HasValue)
            {
                time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double) -days.Value);
            }
            else if (year.HasValue && month.HasValue)
            {
                time = new DateTime(year.Value, month.Value, 1);
            }
            else if (!(!year.HasValue || month.HasValue))
            {
                time = new DateTime(year.Value, 1, 1);
            }
            if (!days.HasValue)
            {
                if (year.HasValue && month.HasValue)
                {
                    int num2 = DateTime.DaysInMonth(year.Value, month.Value);
                    for (num = 1; num <= num2; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddDays(1.0);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                    return list;
                }
                if (year.HasValue && !month.HasValue)
                {
                    int num3 = 12;
                    for (num = 1; num <= num3; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddMonths(1);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                }
                return list;
            }
            num = 1;
        Label_01A9:
            num4 = num;
            if (num4 <= days)
            {
                date = new UserStatisticsForDate();
                if (num > 1)
                {
                    time = time2;
                }
                time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                date.TimePoint = time.Day;
                list.Add(date);
                num++;
                goto Label_01A9;
            }
            return list;
        }

        public override OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfPage += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfPage += (decimal) reader["Profits"];
                    }
                }
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public override OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_OrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["OrderTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["OrderTotal"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
            return info;
        }

        public override IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(select count(*) from aspnet_Members) as AllUserCounts FROM aspnet_Members  GROUP BY TopRegionId ");
            IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                UserStatisticsInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateUserStatistics(reader);
                    list.Add(item);
                }
                if (item != null)
                {
                    totalRegionsUsers = int.Parse(item.AllUserCounts.ToString());
                    return list;
                }
                totalRegionsUsers = 0;
            }
            return list;
        }

        public override DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = DateTime.Now.AddDays(-7.0);
            DateTime time2 = new DateTime(time.Year, time.Month, time.Day);
            DateTime now = DateTime.Now;
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time2);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, now);
            decimal allSalesTotal = 0M;
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                allSalesTotal = Convert.ToDecimal(obj2);
            }
            DataTable table = this.CreateTable();
            for (int i = 0; i < 7; i++)
            {
                DateTime time4 = DateTime.Now.AddDays((double) -i);
                decimal salesTotal = this.GetDaySaleTotal(time4.Year, time4.Month, time4.Day, saleStatisticsType);
                this.InsertToTable(table, time4.Day, salesTotal, allSalesTotal);
            }
            return table;
        }

        public override decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        private void InsertToTable(DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
        {
            DataRow row = table.NewRow();
            row["Date"] = date;
            row["SaleTotal"] = salesTotal;
            if (allSalesTotal != 0M)
            {
                row["Percentage"] = (salesTotal / allSalesTotal) * 100M;
            }
            else
            {
                row["Percentage"] = 0;
            }
            row["Lenth"] = ((decimal) row["Percentage"]) * 4M;
            table.Rows.Add(row);
        }

        public override bool SaveDebitNote(DebitNote note)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" insert into Hishop_OrderDebitNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, note.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SaveOrderRemark(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE OrderId=@OrderId");
            if (order.ManagerMark.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int) order.ManagerMark.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SavePurchaseDebitNote(PurchaseDebitNote note)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_PurchaseDebitNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, note.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE PurchaseOrderId=@PurchaseOrderId");
            if (purchaseOrder.ManagerMark.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int) purchaseOrder.ManagerMark.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, purchaseOrder.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
        {
            if (purchaseOrder == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, purchaseOrder.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, purchaseOrder.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, purchaseOrder.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, purchaseOrder.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, purchaseOrder.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, purchaseOrder.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, purchaseOrder.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SavePurchaseSendNote(SendNote note)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into Hishop_PurchaseSendNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, note.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SaveSendNote(SendNote note)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" insert into Hishop_OrderSendNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, note.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SaveShippingAddress(OrderInfo order)
        {
            if (order == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int SendGoods(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, PurchaseStatus = @PurchaseStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName , ExpressCompanyAbb = @ExpressCompanyAbb WHERE PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, purchaseOrder.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, purchaseOrder.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, purchaseOrder.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, purchaseOrder.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, purchaseOrder.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SetDefalutShipper(int shipperId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET IsDefault = 0;UPDATE Hishop_Shippers SET IsDefault = 1 WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipperId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool SetExpressIsUse(int expressId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET IsUse = ~IsUse WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderPrinted(string orderId, bool isPrinted)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET IsPrinted=@isPrinted WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "isPrinted", DbType.Boolean, isPrinted);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0})", purchaseOrderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderPrinted(string purchaseOrderIds, bool isPrinted)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET IsPrinted=@isPrinted WHERE PurchaseOrderId =@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderIds);
            this.database.AddInParameter(sqlStringCommand, "isPrinted", DbType.Boolean, isPrinted);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderShipNumber(string purchaseOrderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShipOrderNumber=@ShipOrderNumber WHERE PurchaseOrderId =@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetPurchaseOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_PurchaseOrders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (PurchaseStatus = 2 or (PurchaseStatus=1 AND Gateway='hishop.plugins.payment.podrequest')) AND PurchaseOrderId IN ({0})", purchaseOrderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_ShippingTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateDistributorAccount(decimal expenditure, int distributorId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET Expenditure=Expenditure+@expenditureAdd, PurchaseOrder = PurchaseOrder + 1 WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorId);
            this.database.AddInParameter(sqlStringCommand, "expenditureAdd", DbType.Decimal, expenditure);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateExpressTemplate(int expressId, string expressName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ExpressTemplates SET ExpressName = @ExpressName WHERE ExpressId = @ExpressId");
            this.database.AddInParameter(sqlStringCommand, "ExpressName", DbType.String, expressName);
            this.database.AddInParameter(sqlStringCommand, "ExpressId", DbType.Int32, expressId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
            this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderAmount(OrderInfo order, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET OrderTotal = @OrderTotal, OrderProfit=@OrderProfit, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount, OrderPoint=@OrderPoint, Amount=@Amount,OrderCostPrice=@OrderCostPrice WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, Convert.ToInt32((decimal) ((((order.GetTotal() - order.AdjustedFreight) - order.PayCharge) - order.Tax) / HiContext.Current.SiteSettings.PointsRate)));
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderPaymentType(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderShippingMode(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void UpdatePayOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems)
        {
            if (lineItems.Count <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            int num = 0;
            foreach (LineItemInfo info in lineItems.Values)
            {
                builder.Append("UPDATE Hishop_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts + @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(";");
                this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + num, DbType.Int32, info.ProductId);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateProductStock(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Weight=@Weight,PurchaseProfit=@PurchaseProfit,PurchaseTotal=@PurchaseTotal,AdjustedFreight=@AdjustedFreight WHERE PurchaseOrderId=@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, purchaseOrder.Weight);
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Decimal, purchaseOrder.GetPurchaseProfit());
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Decimal, purchaseOrder.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, purchaseOrder.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseTotal=@PurchaseTotal, PurchaseProfit=@PurchaseProfit, AdjustedDiscount=@AdjustedDiscount WHERE PurchaseOrderId=@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Currency, purchaseOrder.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, purchaseOrder.GetPurchaseProfit());
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, purchaseOrder.AdjustedDiscount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrderItems SET Quantity=@Quantity WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, Quantity);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName WHERE PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, purchaseOrder.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, purchaseOrder.ModeName);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void UpdateRefundSubmitPurchaseOrderStock(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.Quantity) FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND PurchaseOrderId =@PurchaseOrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId)");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateShipper(ShippersInfo shipper)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE ShipperId = @ShipperId");
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            this.database.AddInParameter(sqlStringCommand, "ShipperId", DbType.Int32, shipper.ShipperId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateShippingMode(ShippingModeInfo shippingMode)
        {
            bool flag;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ShippingMode_Update");
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, shippingMode.Name);
            this.database.AddInParameter(storedProcCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            this.database.AddInParameter(storedProcCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, shippingMode.Description);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    try
                    {
                        this.database.ExecuteNonQuery(storedProcCommand, transaction);
                        flag = ((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0;
                        if (flag)
                        {
                            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, shippingMode.ModeId);
                            StringBuilder builder = new StringBuilder();
                            int num = 0;
                            builder.Append("DECLARE @ERR INT; Set @ERR =0;");
                            foreach (string str in shippingMode.ExpressCompany)
                            {
                                builder.Append(" INSERT INTO Hishop_TemplateRelatedShipping(ModeId,ExpressCompanyName) VALUES( @ModeId,").Append("@ExpressCompanyName").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName" + num, DbType.String, str);
                                num++;
                            }
                            sqlStringCommand.CommandText = builder.Append("SELECT @ERR;").ToString();
                            int num2 = (int) this.database.ExecuteScalar(sqlStringCommand, transaction);
                            if (num2 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        flag = false;
                    }
                    return flag;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(new StringBuilder("UPDATE Hishop_ShippingTemplates SET TemplateName=@TemplateName,Weight=@Weight,AddWeight=@AddWeight,Price=@Price,AddPrice=@AddPrice WHERE TemplateId=@TemplateId;").ToString());
            this.database.AddInParameter(sqlStringCommand, "TemplateName", DbType.String, shippingMode.Name);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Currency, shippingMode.Weight);
            this.database.AddInParameter(sqlStringCommand, "AddWeight", DbType.Currency, shippingMode.AddWeight);
            this.database.AddInParameter(sqlStringCommand, "Price", DbType.Currency, shippingMode.Price);
            this.database.AddInParameter(sqlStringCommand, "AddPrice", DbType.Currency, shippingMode.AddPrice);
            this.database.AddInParameter(sqlStringCommand, "TemplateId", DbType.Int32, shippingMode.TemplateId);
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    try
                    {
                        flag = this.database.ExecuteNonQuery(sqlStringCommand, transaction) > 0;
                        if (flag)
                        {
                            DbCommand command = this.database.GetSqlStringCommand(" ");
                            this.database.AddInParameter(command, "TemplateId", DbType.Int32, shippingMode.TemplateId);
                            StringBuilder builder2 = new StringBuilder();
                            int num = 0;
                            int num2 = 0;
                            builder2.Append("DELETE Hishop_ShippingTypeGroups WHERE TemplateId=@TemplateId;");
                            builder2.Append("DELETE Hishop_ShippingRegions WHERE TemplateId=@TemplateId;");
                            builder2.Append("DECLARE @ERR INT; Set @ERR =0;");
                            builder2.Append(" DECLARE @GroupId Int;");
                            if ((shippingMode.ModeGroup != null) && (shippingMode.ModeGroup.Count > 0))
                            {
                                foreach (ShippingModeGroupInfo info in shippingMode.ModeGroup)
                                {
                                    builder2.Append(" INSERT INTO Hishop_ShippingTypeGroups(TemplateId,Price,AddPrice) VALUES( @TemplateId,").Append("@Price").Append(num).Append(",@AddPrice").Append(num).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                    this.database.AddInParameter(command, "Price" + num, DbType.Currency, info.Price);
                                    this.database.AddInParameter(command, "AddPrice" + num, DbType.Currency, info.AddPrice);
                                    builder2.Append("Set @GroupId =@@identity;");
                                    foreach (ShippingRegionInfo info2 in info.ModeRegions)
                                    {
                                        builder2.Append(" INSERT INTO Hishop_ShippingRegions(TemplateId,GroupId,RegionId) VALUES(@TemplateId,@GroupId").Append(",@RegionId").Append(num2).Append("); SELECT @ERR=@ERR+@@ERROR;");
                                        this.database.AddInParameter(command, "RegionId" + num2, DbType.Int32, info2.RegionId);
                                        num2++;
                                    }
                                    num++;
                                }
                            }
                            command.CommandText = builder2.Append("SELECT @ERR;").ToString();
                            int num3 = (int) this.database.ExecuteScalar(command, transaction);
                            if (num3 != 0)
                            {
                                transaction.Rollback();
                                flag = false;
                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        if (transaction.Connection != null)
                        {
                            transaction.Rollback();
                        }
                        flag = false;
                    }
                    return flag;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool UpdateUserAccount(decimal orderTotal, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1 WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool UpdateUserRank(int userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Expenditure = ISNULL(Expenditure,0) - @refundAmount, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "refundAmount", DbType.Decimal, refundAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            if (isAllRefund)
            {
                this.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "refundNum", DbType.Int32, 0);
            }
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

