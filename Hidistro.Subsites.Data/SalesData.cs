namespace Hidistro.Subsites.Data
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Sales;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class SalesData : SubsiteSalesProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddMemberPoint(UserPointInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
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

        public override bool AddOrderGift(string orderId, GiftInfo gift, int quantity, int promotype, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from distro_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    DbCommand command2 = this.database.GetSqlStringCommand("update distro_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
                    this.database.AddInParameter(command2, "OrderId", DbType.String, orderId);
                    this.database.AddInParameter(command2, "GiftId", DbType.Int32, gift.GiftId);
                    this.database.AddInParameter(command2, "Quantity", DbType.Int32, ((int) reader["Quantity"]) + quantity);
                    if (dbTran != null)
                    {
                        return (this.database.ExecuteNonQuery(command2, dbTran) == 1);
                    }
                    return (this.database.ExecuteNonQuery(command2) == 1);
                }
                DbCommand command = this.database.GetSqlStringCommand("INSERT INTO distro_OrderGifts(OrderId,GiftId,DistributorUserId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@DistributorUserId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity,@PromoType)");
                this.database.AddInParameter(command, "OrderId", DbType.String, orderId);
                this.database.AddInParameter(command, "GiftId", DbType.Int32, gift.GiftId);
                this.database.AddInParameter(command, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
                this.database.AddInParameter(command, "GiftName", DbType.String, gift.Name);
                this.database.AddInParameter(command, "CostPrice", DbType.Currency, gift.PurchasePrice);
                this.database.AddInParameter(command, "ThumbnailsUrl", DbType.String, gift.ThumbnailUrl40);
                this.database.AddInParameter(command, "Quantity", DbType.Int32, quantity);
                this.database.AddInParameter(command, "PromoType", DbType.Int32, promotype);
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(command, dbTran) == 1);
                }
                return (this.database.ExecuteNonQuery(command) == 1);
            }
        }

        public override bool AddPurchaseItem(PurchaseShoppingCartItemInfo item)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PurchaseShoppingCarts (SkuId, ProductId,SKU,DistributorUserId,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ThumbnailsUrl,Weight,SKUContent)VALUES(@SkuId, @ProductId,@SKU,@DistributorUserId,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ThumbnailsUrl,@Weight,@SKUContent)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, item.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, item.ProductId);
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, item.SKU);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, item.Quantity);
            this.database.AddInParameter(sqlStringCommand, "ItemListPrice", DbType.Currency, item.ItemListPrice);
            this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", DbType.Currency, item.ItemPurchasePrice);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency, item.CostPrice);
            this.database.AddInParameter(sqlStringCommand, "ItemDescription", DbType.String, item.ItemDescription);
            this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", DbType.String, item.ThumbnailsUrl);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Decimal, item.ItemWeight);
            this.database.AddInParameter(sqlStringCommand, "SKUContent", DbType.String, item.SKUContent);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
            }
            catch
            {
                return false;
            }
        }

        public override bool AddPurchaseOrderGift(string purchaseOrderId, GiftInfo gift, int quantity, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId AND GiftId=@GiftId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, gift.GiftId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    DbCommand command2 = this.database.GetSqlStringCommand("update Hishop_PurchaseOrderGifts set Quantity=@Quantity where PurchaseOrderId=@PurchaseOrderId AND GiftId=@GiftId");
                    this.database.AddInParameter(command2, "PurchaseOrderId", DbType.String, purchaseOrderId);
                    this.database.AddInParameter(command2, "GiftId", DbType.Int32, gift.GiftId);
                    this.database.AddInParameter(command2, "Quantity", DbType.Int32, ((int) reader["Quantity"]) + quantity);
                    if (dbTran != null)
                    {
                        return (this.database.ExecuteNonQuery(command2, dbTran) == 1);
                    }
                    return (this.database.ExecuteNonQuery(command2) == 1);
                }
                DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId,GiftId,GiftName,CostPrice,PurchasePrice,ThumbnailsUrl,Quantity) VALUES(@PurchaseOrderId,@GiftId,@GiftName,@CostPrice,@PurchasePrice,@ThumbnailsUrl,@Quantity)");
                this.database.AddInParameter(command, "PurchaseOrderId", DbType.String, purchaseOrderId);
                this.database.AddInParameter(command, "GiftId", DbType.Int32, gift.GiftId);
                this.database.AddInParameter(command, "GiftName", DbType.String, gift.Name);
                this.database.AddInParameter(command, "CostPrice", DbType.Currency, gift.CostPrice);
                this.database.AddInParameter(command, "PurchasePrice", DbType.Currency, gift.PurchasePrice);
                this.database.AddInParameter(command, "ThumbnailsUrl", DbType.String, gift.ThumbnailUrl40);
                this.database.AddInParameter(command, "Quantity", DbType.Int32, quantity);
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(command, dbTran) == 1);
                }
                return (this.database.ExecuteNonQuery(command) == 1);
            }
        }

        public override bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            StringBuilder builder = new StringBuilder("INSERT INTO Hishop_PurchaseOrderItems (PurchaseOrderId,SkuId, ProductId,SKU,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ItemHomeSiteDescription,ThumbnailsUrl,Weight,SKUContent)");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Shippers (DistributorUserId,IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark) VALUES (@DistributorUserId, 0, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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

        public override bool ApplyForPurchaseRefund(string purchaseOrderId, string remark, int refundType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" insert into Hishop_PurchaseOrderRefund(PurchaseOrderId,ApplyForTime,RefundRemark,HandleStatus,RefundType) values(@PurchaseOrderId,@ApplyForTime,@RefundRemark,0,@RefundType);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 6);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ApplyForPurchaseReplace(string purchaseOrderId, string remark)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" insert into Hishop_PurchaseOrderReplace(PurchaseOrderId,ApplyForTime,Comments,HandleStatus) values(@PurchaseOrderId,@ApplyForTime,@Comments,0);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 8);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ApplyForPurchaseReturn(string purchaseOrderId, string remark, int refundType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
            builder.Append(" insert into Hishop_PurchaseOrderReturns(PurchaseOrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@PurchaseOrderId,@ApplyForTime,@Comments,0,@RefundType,0);");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 7);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "ApplyForTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Comments", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "RefundType", DbType.Int32, refundType);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool BatchConfirmPay(string purchaseOrderIds)
        {
            bool flag = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    string str = "";
                    foreach (string str2 in purchaseOrderIds.Split(new char[] { ',' }))
                    {
                        PurchaseOrderInfo purchaseOrder = this.GetPurchaseOrder(str2);
                        if (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
                        {
                            str = str + str2 + ",";
                            if (purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
                            {
                                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
                                this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, str2);
                                this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
                                this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.String, HiContext.Current.User.UserId);
                                this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 2);
                                if (this.database.ExecuteNonQuery(sqlStringCommand, transaction) <= 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                this.UpdateProductStock(purchaseOrder.PurchaseOrderId);
                                this.UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
                                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                                flag = true;
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool BatchConfirmPay(BalanceDetailInfo balance, string purchaseOrderIds)
        {
            bool flag = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    string str = "";
                    foreach (string str2 in purchaseOrderIds.Split(new char[] { ',' }))
                    {
                        PurchaseOrderInfo purchaseOrder = this.GetPurchaseOrder(str2);
                        if (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
                        {
                            str = str + str2 + ",";
                            if (purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
                            {
                                DbCommand command = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Gateway = @Gateway, PaymentTypeId = (select ModeId from Hishop_PaymentTypes where Gateway = @Gateway),PaymentType = (select Name from Hishop_PaymentTypes where Gateway = @Gateway),PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId");
                                this.database.AddInParameter(command, "Gateway", DbType.String, "hishop.plugins.payment.advancerequest");
                                this.database.AddInParameter(command, "PurchaseOrderId", DbType.String, str2);
                                this.database.AddInParameter(command, "PayDate", DbType.DateTime, DateTime.Now);
                                this.database.AddInParameter(command, "PurchaseStatus", DbType.Int32, 2);
                                if (this.database.ExecuteNonQuery(command, transaction) <= 0)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                this.UpdateProductStock(purchaseOrder.PurchaseOrderId);
                                this.UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
                                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                                flag = true;
                            }
                        }
                    }
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Expenses,Balance, Remark) VALUES(@UserId,@UserName,@TradeDate,@TradeType,@Expenses,@Balance, @Remark)");
                    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balance.UserId);
                    this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balance.UserName);
                    this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balance.TradeDate);
                    this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balance.TradeType);
                    this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balance.Expenses);
                    this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balance.Balance);
                    this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balance.Remark);
                    if (this.database.ExecuteNonQuery(sqlStringCommand, transaction) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
        {
            string str = string.Empty;
            switch (saleStatisticsType)
            {
                case SaleStatisticsType.SaleCounts:
                    str = "SELECT COUNT(OrderId)";
                    break;

                case SaleStatisticsType.SaleTotal:
                    str = "SELECT Isnull(SUM(OrderTotal),0)";
                    break;

                case SaleStatisticsType.Profits:
                    str = "SELECT IsNull(SUM(OrderProfit),0)";
                    break;
            }
            return (str + string.Format(" FROM distro_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate) AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND DistributorUserId={3}", new object[] { 1, 4, 9, HiContext.Current.User.UserId }));
        }

        private static string BuildOrdersQuery(OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM distro_Orders WHERE DistributorUserId ='{0}' ", HiContext.Current.User.UserId);
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
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_OrderItems WHERE DistributorUserId ={0} AND ItemDescription LIKE '%{1}%')", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    builder.AppendFormat(" AND Username = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.Status == OrderStatus.History)
                {
                    builder.AppendFormat(" AND OrderStatus = {0} AND OrderDate < '{1}'", 5, DateTime.Now.AddMonths(-3));
                }
                else if (query.Status != OrderStatus.All)
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate >= '{0}'", query.StartDate.Value);
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate <= '{0}'", query.EndDate.Value);
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
            builder.AppendFormat(" FROM distro_OrderItems o  WHERE 0=0 and DistributorUserId={0}", HiContext.Current.User.UserId);
            builder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM distro_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} and  DistributorUserId={3})", new object[] { 4, 1, 9, HiContext.Current.User.UserId });
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_Orders WHERE OrderDate >= '{0}' and  DistributorUserId={1})", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value), HiContext.Current.User.UserId);
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_Orders WHERE OrderDate <= '{0}' and  DistributorUserId={1})", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value), HiContext.Current.User.UserId);
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.AppendFormat(" (SELECT ProductId FROM distro_Products where DistributorUserId={0})", HiContext.Current.User.UserId);
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
            builder.AppendFormat(" FROM distro_products where SaleCounts>0 and DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildUserOrderQuery(UserOrderQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT OrderId FROM distro_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND DistributorUserId = {3}", new object[] { 1, 4, 9, HiContext.Current.User.UserId });
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

        public override bool CanPurchaseRefund(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderRefund where PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            bool flag = false;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    if (Convert.ToInt32(reader["HandleStatus"]) == 2)
                    {
                        flag = true;
                    }
                    return flag;
                }
                return true;
            }
        }

        public override bool CanPurchaseReplace(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderReplace where PurchaseOrderId = @PurchaseOrderId AND HandleStatus = 0");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            return (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) < 1);
        }

        public override bool CanPurchaseReturn(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderReturns where PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            bool flag = false;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    if (Convert.ToInt32(reader["HandleStatus"]) == 2)
                    {
                        flag = true;
                    }
                    return flag;
                }
                return true;
            }
        }

        public override bool ChangeMemberGrade(int userId, int gradId, int points)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM distro_MemberGrades WHERE CreateUserId=@CreateUserId Order by Point Desc ");
            this.database.AddInParameter(sqlStringCommand, "CreateUserId", DbType.Int32, HiContext.Current.User.UserId);
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

        public override bool CheckRefund(string orderId, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
            builder.Append(" update distro_OrderRefund set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
            if ((refundType == 1) && accept)
            {
                OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
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
                    builder.Append("insert into distro_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark,DistributorUserId)");
                    builder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}',{7})", new object[] { user.UserId, user.Username, "getdate()", 5, total, num2, "订单" + orderId + "退款", HiContext.Current.User.UserId });
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
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.String, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckReplace(string orderId, string adminRemark, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
            builder.Append(" update distro_OrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
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
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.String, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool CheckReturn(string orderId, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
            builder.Append(" update distro_OrderReturns set RefundMoney=@RefundMoney,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
            if ((refundType == 1) && accept)
            {
                builder.Append(" insert into distro_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
                builder.AppendFormat(",Balance,Remark,DistributorUserId) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
                builder.Append("@RefundMoney as Income,isnull((select top 1 Balance from distro_BalanceDetails b");
                builder.Append(" where b.UserId=a.UserId and b.DistributorUserId=a.DistributorUserId ");
                builder.AppendFormat("order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark,DistributorUserId", orderId);
                builder.Append(" from distro_Orders a where OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
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
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.String, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "RefundMoney", DbType.Decimal, refundMoney);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearOrderGifts(string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderGifts WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ClearPurchaseOrderGifts(string purchaseOrderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId =@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void ClearPurchaseShoppingCart()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseShoppingCarts WHERE  DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,CloseReason=@CloseReason WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, purchaseOrder.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool CloseTransaction(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,CloseReason=@CloseReason WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ConfirmOrderFinish(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,FinishDate=@FinishDate WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ConfirmPay(string purchaseOrderId)
        {
            bool flag;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId");
                    this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
                    this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
                    this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 2);
                    if (this.database.ExecuteNonQuery(sqlStringCommand, transaction) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                    flag = true;
                }
                catch
                {
                    transaction.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override bool ConfirmPay(BalanceDetailInfo balance, string purchaseOrderId)
        {
            bool flag;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                try
                {
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Expenses,Balance, Remark) VALUES(@UserId,@UserName,@TradeDate,@TradeType,@Expenses,@Balance, @Remark)");
                    this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balance.UserId);
                    this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balance.UserName);
                    this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balance.TradeDate);
                    this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balance.TradeType);
                    this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balance.Expenses);
                    this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balance.Balance);
                    this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balance.Remark);
                    if (this.database.ExecuteNonQuery(sqlStringCommand, transaction) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    DbCommand command = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
                    this.database.AddInParameter(command, "PurchaseOrderId", DbType.String, purchaseOrderId);
                    this.database.AddInParameter(command, "PayDate", DbType.DateTime, DateTime.Now);
                    this.database.AddInParameter(command, "DistributorId", DbType.String, HiContext.Current.User.UserId);
                    this.database.AddInParameter(command, "PurchaseStatus", DbType.Int32, 2);
                    if (this.database.ExecuteNonQuery(command, transaction) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                    flag = true;
                }
                catch
                {
                    transaction.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public override int ConfirmPay(OrderInfo order, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET PayDate = @PayDate, OrderStatus = @OrderStatus,OrderPoint=@OrderPoint WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, order.Points);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
            }
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,FinishDate=@FinishDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order)
        {
            if (order == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            string query = "";
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                builder.AppendFormat("'" + info.SkuId + "',", new object[0]);
            }
            if (builder.Length > 0)
            {
                builder = builder.Remove(builder.Length - 1, 1);
                query = string.Format("SELECT S.SkuId, S.CostPrice, p.ProductName FROM Hishop_Products P LEFT OUTER JOIN Hishop_SKUs S ON P.ProductId = S.ProductId WHERE S.SkuId IN({0});", builder);
            }
            if (order.Gifts.Count > 0)
            {
                StringBuilder builder2 = new StringBuilder();
                foreach (OrderGiftInfo info2 in order.Gifts)
                {
                    builder2.AppendFormat(info2.GiftId.ToString() + ",", new object[0]);
                }
                builder2.Remove(builder2.Length - 1, 1);
                query = query + string.Format(" SELECT GiftId, CostPrice FROM Hishop_Gifts WHERE GiftId IN({0});", builder2.ToString());
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            Dictionary<string, PurchaseOrderItemInfo> dictionary = new Dictionary<string, PurchaseOrderItemInfo>();
            Dictionary<int, decimal> dictionary2 = new Dictionary<int, decimal>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (order.LineItems.Values.Count > 0)
                {
                    while (reader.Read())
                    {
                        PurchaseOrderItemInfo info3 = new PurchaseOrderItemInfo();
                        if (reader["CostPrice"] != DBNull.Value)
                        {
                            info3.ItemCostPrice = (decimal) reader["CostPrice"];
                        }
                        info3.ItemHomeSiteDescription = (string) reader["ProductName"];
                        dictionary.Add((string) reader["SkuId"], info3);
                    }
                }
                if (order.Gifts.Count > 0)
                {
                    if (order.LineItems.Count > 0)
                    {
                        reader.NextResult();
                    }
                    while (reader.Read())
                    {
                        dictionary2.Add((int) reader["GiftId"], (DBNull.Value == reader["CostPrice"]) ? 0M : Convert.ToDecimal(reader["CostPrice"]));
                    }
                }
            }
            Distributor user = HiContext.Current.User as Distributor;
            PurchaseOrderInfo info4 = new PurchaseOrderInfo();
            info4.PurchaseOrderId = "PO" + order.OrderId;
            info4.OrderId = order.OrderId;
            info4.Remark = order.Remark;
            info4.PurchaseStatus = OrderStatus.WaitBuyerPay;
            info4.DistributorId = user.UserId;
            info4.Distributorname = user.Username;
            info4.DistributorEmail = user.Email;
            info4.DistributorRealName = user.RealName;
            info4.DistributorQQ = user.QQ;
            info4.DistributorWangwang = user.Wangwang;
            info4.DistributorMSN = user.MSN;
            info4.ShippingRegion = order.ShippingRegion;
            info4.Address = order.Address;
            info4.ZipCode = order.ZipCode;
            info4.ShipTo = order.ShipTo;
            info4.TelPhone = order.TelPhone;
            info4.CellPhone = order.CellPhone;
            info4.ShipToDate = order.ShipToDate;
            info4.ShippingModeId = order.ShippingModeId;
            info4.ModeName = order.ModeName;
            info4.RegionId = order.RegionId;
            info4.Freight = order.Freight;
            info4.AdjustedFreight = order.Freight;
            info4.ShipOrderNumber = order.ShipOrderNumber;
            info4.Weight = order.Weight;
            info4.RefundStatus = RefundStatus.None;
            info4.OrderTotal = order.GetTotal();
            info4.ExpressCompanyAbb = order.ExpressCompanyAbb;
            info4.ExpressCompanyName = order.ExpressCompanyName;
            info4.Tax = order.Tax;
            info4.InvoiceTitle = order.InvoiceTitle;
            foreach (LineItemInfo info5 in order.LineItems.Values)
            {
                PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                item.PurchaseOrderId = info4.PurchaseOrderId;
                item.SkuId = info5.SkuId;
                item.ProductId = info5.ProductId;
                item.SKU = info5.SKU;
                item.Quantity = info5.ShipmentQuantity;
                foreach (KeyValuePair<string, PurchaseOrderItemInfo> pair in dictionary)
                {
                    if (pair.Key == info5.SkuId)
                    {
                        item.ItemCostPrice = pair.Value.ItemCostPrice;
                        item.ItemHomeSiteDescription = pair.Value.ItemHomeSiteDescription;
                    }
                }
                item.ItemPurchasePrice = info5.ItemCostPrice;
                item.ItemListPrice = info5.ItemListPrice;
                item.ItemDescription = info5.ItemDescription;
                item.SKUContent = info5.SKUContent;
                item.ThumbnailsUrl = info5.ThumbnailsUrl;
                item.ItemWeight = info5.ItemWeight;
                if (string.IsNullOrEmpty(item.ItemHomeSiteDescription))
                {
                    item.ItemHomeSiteDescription = item.ItemDescription;
                }
                info4.PurchaseOrderItems.Add(item);
            }
            foreach (OrderGiftInfo info7 in order.Gifts)
            {
                PurchaseOrderGiftInfo info8 = new PurchaseOrderGiftInfo();
                info8.PurchaseOrderId = info4.PurchaseOrderId;
                foreach (KeyValuePair<int, decimal> pair2 in dictionary2)
                {
                    if (pair2.Key == info7.GiftId)
                    {
                        info8.CostPrice = pair2.Value;
                    }
                }
                info8.PurchasePrice = info7.CostPrice;
                info8.GiftId = info7.GiftId;
                info8.GiftName = info7.GiftName;
                info8.Quantity = info7.Quantity;
                info8.ThumbnailsUrl = info7.ThumbnailsUrl;
                info4.PurchaseOrderGifts.Add(info8);
            }
            return info4;
        }

        public override bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrder, DbTransaction dbTran)
        {
            string str;
            if (purchaseOrder == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PurchaseOrders(PurchaseOrderId, OrderId, Remark, ManagerMark, ManagerRemark, AdjustedDiscount,PaymentTypeId,PaymentType,Gateway,PurchaseStatus, CloseReason, PurchaseDate, DistributorId, Distributorname,DistributorEmail, DistributorRealName, DistributorQQ, DistributorWangwang, DistributorMSN,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName,RealShippingModeId, RealModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, Weight,ExpressCompanyName,ExpressCompanyAbb,RefundStatus, RefundAmount, RefundRemark, OrderTotal, PurchaseProfit, PurchaseTotal, TaobaoOrderId,Tax,InvoiceTitle)VALUES (@PurchaseOrderId, @OrderId, @Remark, @ManagerMark, @ManagerRemark, @AdjustedDiscount,@PaymentTypeId,@PaymentType,@Gateway,@PurchaseStatus, @CloseReason, @PurchaseDate, @DistributorId, @Distributorname,@DistributorEmail, @DistributorRealName, @DistributorQQ, @DistributorWangwang, @DistributorMSN,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName,@RealShippingModeId, @RealModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @PurchaseWeight,@ExpressCompanyName,@ExpressCompanyAbb,@RefundStatus, @RefundAmount, @RefundRemark, @OrderTotal, @PurchaseProfit, @PurchaseTotal, @TaobaoOrderId,@Tax,@InvoiceTitle);");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            if (purchaseOrder.OrderId == null)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, DBNull.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, purchaseOrder.OrderId);
            }
            if (purchaseOrder.Remark == null)
            {
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, DBNull.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, purchaseOrder.Remark);
            }
            if (purchaseOrder.ManagerMark.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int) purchaseOrder.ManagerMark.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, purchaseOrder.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, purchaseOrder.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, (int) purchaseOrder.PurchaseStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, purchaseOrder.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PurchaseDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, purchaseOrder.DistributorId);
            this.database.AddInParameter(sqlStringCommand, "Distributorname", DbType.String, purchaseOrder.Distributorname);
            this.database.AddInParameter(sqlStringCommand, "DistributorEmail", DbType.String, purchaseOrder.DistributorEmail);
            this.database.AddInParameter(sqlStringCommand, "DistributorRealName", DbType.String, purchaseOrder.DistributorRealName);
            this.database.AddInParameter(sqlStringCommand, "DistributorQQ", DbType.String, purchaseOrder.DistributorQQ);
            this.database.AddInParameter(sqlStringCommand, "DistributorWangwang", DbType.String, purchaseOrder.DistributorWangwang);
            this.database.AddInParameter(sqlStringCommand, "DistributorMSN", DbType.String, purchaseOrder.DistributorMSN);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, purchaseOrder.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, purchaseOrder.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, purchaseOrder.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, purchaseOrder.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, purchaseOrder.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, purchaseOrder.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShipToDate", DbType.String, purchaseOrder.ShipToDate);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, purchaseOrder.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, purchaseOrder.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, purchaseOrder.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, purchaseOrder.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, purchaseOrder.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Freight", DbType.Currency, purchaseOrder.Freight);
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, purchaseOrder.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, purchaseOrder.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "PurchaseWeight", DbType.Decimal, purchaseOrder.Weight);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, purchaseOrder.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, purchaseOrder.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "RefundStatus", DbType.Int32, (int) purchaseOrder.RefundStatus);
            this.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Currency, purchaseOrder.RefundAmount);
            this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, purchaseOrder.RefundRemark);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, purchaseOrder.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, purchaseOrder.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, purchaseOrder.Gateway);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, purchaseOrder.OrderTotal);
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, purchaseOrder.GetPurchaseProfit());
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Currency, purchaseOrder.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", DbType.String, purchaseOrder.TaobaoOrderId);
            this.database.AddInParameter(sqlStringCommand, "Tax", DbType.Currency, purchaseOrder.Tax);
            this.database.AddInParameter(sqlStringCommand, "InvoiceTitle", DbType.String, purchaseOrder.InvoiceTitle);
            int num = 0;
            foreach (PurchaseOrderItemInfo info in purchaseOrder.PurchaseOrderItems)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderItems(PurchaseOrderId, SkuId, ProductId, SKU, Quantity,  CostPrice, ").Append("ItemListPrice, ItemPurchasePrice, ItemDescription, ItemHomeSiteDescription, SKUContent, ThumbnailsUrl, Weight) VALUES( @PurchaseOrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemPurchasePrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ItemHomeSiteDescription").Append(str).Append(",@SKUContent").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice" + str, DbType.Currency, info.ItemPurchasePrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription" + str, DbType.String, info.ItemHomeSiteDescription);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                num++;
            }
            foreach (PurchaseOrderGiftInfo info2 in purchaseOrder.PurchaseOrderGifts)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId, GiftId, GiftName, CostPrice, PurchasePrice, ").Append("ThumbnailsUrl, Quantity) VALUES( @PurchaseOrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@PurchasePrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info2.GiftId);
                this.database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info2.GiftName);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info2.Quantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info2.CostPrice);
                this.database.AddInParameter(sqlStringCommand, "PurchasePrice" + str, DbType.Currency, info2.PurchasePrice);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info2.ThumbnailsUrl);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_PaymentType_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
                this.database.AddInParameter(storedProcCommand, "Charge", DbType.Currency, paymentMode.Charge);
                this.database.AddInParameter(storedProcCommand, "IsPercent", DbType.Boolean, paymentMode.IsPercent);
                this.database.AddInParameter(storedProcCommand, "Settings", DbType.String, paymentMode.Settings);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (PaymentModeActionStatus) ((int) this.database.GetParameterValue(storedProcCommand, "Status"));
        }

        public override bool DelDebitNote(string noteId)
        {
            string query = string.Format("DELETE FROM distro_OrderDebitNote WHERE NoteId='{0}'", noteId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool DeleteOrderProduct(string sku, string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId AND DistributorUserId=@DistributorUserId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, sku);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteOrders(string orderIds)
        {
            string query = string.Format("DELETE FROM distro_Orders WHERE OrderId IN({0}) AND DistributorUserId={1}", orderIds, HiContext.Current.User.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeletePurchaseOrde(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrders WHERE PurchaseOrderId=@PurchaseOrderId AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeletePurchaseOrderGift(string purchaseOrderId, int giftId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId =@PurchaseOrderId AND GiftId=@GiftId ");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool DeletePurchaseOrderItem(string POrderId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeletePurchaseShoppingCartItem(string sku)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseShoppingCarts WHERE SkuId=@SkuId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, sku);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool DelRefundApply(int refundId)
        {
            string query = string.Format("DELETE FROM distro_OrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelReplaceApply(int replaceId)
        {
            string query = string.Format("DELETE FROM distro_OrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DelReturnsApply(int returnsId)
        {
            string query = string.Format("DELETE FROM distro_OrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnsId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool GetAlertStock(string skuId)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_SKUs WHERE SKuId=@SkuId AND Stock<=AlertStock");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            if (((int) this.database.ExecuteScalar(sqlStringCommand)) >= 1)
            {
                flag = true;
            }
            return flag;
        }

        public override DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId='{0}'", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId='{0}'", query.OrderId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderDebitNote", "NoteId", builder.ToString(), "*");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM distro_PointDetails WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override LineItemInfo GetLineItemInfo(string sku, string orderId)
        {
            LineItemInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_OrderItems WHERE SkuId=@SkuId AND OrderId=@OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, sku);
            this.database.AddInParameter(sqlStringCommand, "orderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateLineItem(reader);
                }
            }
            return info;
        }

        public override void GetLineItemPromotions(int productId, int quantity, out int purchaseGiftId, out string purchaseGiftName, out int giveQuantity, out int wholesaleDiscountId, out string wholesaleDiscountName, out decimal? discountRate, int gradeId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_LineItem_GetPromotionsInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                purchaseGiftId = 0;
                giveQuantity = 0;
                wholesaleDiscountId = 0;
                purchaseGiftName = null;
                wholesaleDiscountName = null;
                discountRate = 0;
                if (reader.Read())
                {
                    if (DBNull.Value != reader["ActivityId"])
                    {
                        purchaseGiftId = (int) reader["ActivityId"];
                    }
                    if (DBNull.Value != reader["Name"])
                    {
                        purchaseGiftName = reader["Name"].ToString();
                    }
                    if ((DBNull.Value != reader["BuyQuantity"]) && (DBNull.Value != reader["GiveQuantity"]))
                    {
                        giveQuantity = (quantity / ((int) reader["BuyQuantity"])) * ((int) reader["GiveQuantity"]);
                    }
                }
                if (reader.NextResult() && reader.Read())
                {
                    if (DBNull.Value != reader["ActivityId"])
                    {
                        wholesaleDiscountId = (int) reader["ActivityId"];
                    }
                    if (DBNull.Value != reader["Name"])
                    {
                        wholesaleDiscountName = reader["Name"].ToString();
                    }
                    if (DBNull.Value != reader["DiscountValue"])
                    {
                        discountRate = new decimal?(Convert.ToDecimal(reader["DiscountValue"]));
                    }
                }
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

        public override ShippersInfo GetMyShipper()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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

        public override DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT top {0} * from distro_OrderGifts where OrderId=@OrderId", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.Append(" ORDER BY GiftId ASC");
            }
            else
            {
                builder.AppendFormat(" and GiftId > (SELECT max(GiftId) from (SELECT top {0} GiftId from distro_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";SELECT count(GiftId) as Total from distro_OrderGifts where OrderId=@OrderId", new object[0]);
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

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Orders Where OrderId = @OrderId AND DistributorUserId=@DistributorUserId; SELECT * FROM distro_OrderGifts Where OrderId = @OrderId; SELECT * FROM distro_OrderItems Where OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    OrderGiftInfo item = DataMapper.PopulateOrderGift(reader);
                    info.Gifts.Add(item);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add((string) reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public override DbQueryResult GetOrders(OrderQuery query)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Orders_Get");
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

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM distro_PaymentTypes WHERE Gateway = @Gateway AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE DistributorUserId=@DistributorUserId Order by DisplaySequence desc");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductVisitAndBuyStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductVisitAndBuyStatisticsQuery(query));
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            builder.AppendFormat("FROM distro_Products WHERE SaleCounts>0 and DistributorUserId={0} ORDER BY BuyPercentage DESC;", HiContext.Current.User.UserId);
            builder.AppendFormat("SELECT COUNT(*) as TotalProductSales FROM distro_Products WHERE SaleCounts>0 and DistributorUserId={0};", HiContext.Current.User.UserId);
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

        public override PurchaseOrderInfo GetPurchaseByOrderId(string orderId)
        {
            PurchaseOrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where OrderId = @OrderId AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePurchaseOrder(reader);
                }
            }
            return info;
        }

        public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            PurchaseOrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderGifts Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderItems Where PurchaseOrderId = @PurchaseOrderId ");
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

        public override DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT top {0} * from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.Append(" ORDER BY GiftId ASC");
            }
            else
            {
                builder.AppendFormat(" and GiftId > (SELECT max(GiftId) from (SELECT top {0} GiftId from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";SELECT count(GiftId) as Total from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId", new object[0]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, query.PurchaseOrderId);
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

        public override DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorId = {0}", HiContext.Current.User.UserId);
            if (query.IsManualPurchaseOrder)
            {
                builder.AppendFormat("AND OrderId IS NULL", new object[0]);
            }
            else
            {
                builder.AppendFormat("AND OrderId IS NOT NULL", new object[0]);
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (!string.IsNullOrEmpty(query.PurchaseOrderId))
            {
                builder.AppendFormat(" AND PurchaseOrderId = '{0}'", query.PurchaseOrderId);
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
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_PurchaseOrders", "PurchaseOrderId", builder.ToString(), "OrderId,ShipTo,RefundStatus, PurchaseOrderId, TaobaoOrderId, PurchaseDate, OrderTotal, PurchaseTotal, PurchaseStatus,PaymentTypeId,ShipOrderNumber,Gateway");
        }

        public override PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId)
        {
            PurchaseOrderTaobaoInfo info = new PurchaseOrderTaobaoInfo();
            if (tbOrderId.Trim() != "")
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where TaobaoOrderId like @TaobaoOrderId");
                this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", DbType.String, "%" + tbOrderId + "%");
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    if (!reader.Read())
                    {
                        return info;
                    }
                    DateTime time = (DateTime) reader["PurchaseDate"];
                    TimeSpan span = (TimeSpan) (DateTime.Now - time.AddDays(7.0));
                    if (span.Days > 0)
                    {
                        info.expire_time = span.Days.ToString();
                    }
                    info.order_id = tbOrderId;
                    info.created = "true";
                    info.status = "已下单";
                    info.time = reader["PurchaseDate"].ToString();
                }
            }
            return info;
        }

        public override IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseShoppingCarts WHERE DistributorUserId=@DistributorUserId AND ProductId IN (SELECT ProductId FROM Hishop_Products WHERE PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId=@DistributorUserId))");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            IList<PurchaseShoppingCartItemInfo> list = new List<PurchaseShoppingCartItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePurchaseShoppingCartItemInfo(reader));
                }
            }
            return list;
        }

        public override DataTable GetRecentlyManualPurchaseOrders(out int number)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders WHERE   DistributorId=@DistributorId AND OrderId IS NULL ORDER BY PurchaseDate DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE   PurchaseStatus=1 AND DistributorId=@DistributorId AND OrderId IS NULL");
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DataTable GetRecentlyOrders(out int number)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal, PaymentType,ISNULL(GroupBuyId,0) as GroupBuyId, ISNULL(GroupBuyStatus,0) as GroupBuyStatus,ManagerMark, OrderStatus, RefundStatus,ManagerRemark, (SELECT COUNT(*) FROM Hishop_PurchaseOrders WHERE OrderId=OrderId) AS PurchaseOrders FROM distro_Orders WHERE  DistributorUserId=@DistributorUserId ORDER BY OrderDate DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            DataTable table = new DataTable();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM distro_Orders WHERE  OrderStatus=2 AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DataTable GetRecentlyPurchaseOrders(out int number)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders WHERE DistributorId=@DistributorId ORDER BY PurchaseDate DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE   PurchaseStatus=1 AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            return table;
        }

        public override DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderRefund", "RefundId", builder.ToString(), "*");
        }

        public override decimal GetRefundMoney(PurchaseOrderInfo purchaseOrder, out decimal refundMoney)
        {
            decimal num = 0M;
            string query = string.Format("SELECT RefundMoney FROM Hishop_PurchaseOrderReturns WHERE HandleStatus=1 AND PurchaseOrderId='{0}'", purchaseOrder.PurchaseOrderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            num = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand));
            return (refundMoney = num);
        }

        public override void GetRefundType(string orderId, out int refundType, out string refundRemark)
        {
            refundType = 0;
            refundRemark = "";
            string query = string.Concat(new object[] { "select RefundType,RefundRemark from distro_OrderRefund where HandleStatus=0 and OrderId='", orderId, "' and DistributorUserId='", HiContext.Current.User.UserId, "'" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                refundRemark = (string) reader["RefundRemark"];
            }
        }

        public override void GetRefundTypeFromReturn(string orderId, out int refundType, out string refundRemark)
        {
            refundType = 0;
            refundRemark = "";
            string query = string.Concat(new object[] { "select RefundType,Comments from distro_OrderReturns where HandleStatus=0 and OrderId='", orderId, "' and DistributorUserId='", HiContext.Current.User.UserId, "'" });
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                refundType = (int) reader["RefundType"];
                refundRemark = (string) reader["Comments"];
            }
        }

        public override DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderReplace", "ReplaceId", builder.ToString(), "*");
        }

        public override string GetReplaceComments(string orderId)
        {
            string query = string.Concat(new object[] { "select Comments from distro_OrderReplace where HandleStatus=0 and OrderId='", orderId, "' and DistributorUserId=", HiContext.Current.User.UserId });
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
            builder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
            }
            if (query.HandleStatus.HasValue)
            {
                builder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderReturns", "ReturnsId", builder.ToString(), "*");
        }

        public override DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND orderDate >= '{0}'", query.StartDate.Value);
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND orderDate <= '{0}'", query.EndDate.Value);
            }
            if (builder.Length > 0)
            {
                builder.Append(" AND ");
            }
            builder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_SaleDetails", "OrderId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override DbQueryResult GetSaleTargets()
        {
            DbQueryResult result = new DbQueryResult();
            string query = string.Empty;
            query = string.Format("SELECT " + string.Format(" (SELECT Count(OrderId) from distro_orders where DistributorUserId={0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderStatus != {3}) as OrderNumb ,", new object[] { HiContext.Current.User.UserId, 1, 4, 9 }) + string.Format("ISNULL((SELECT sum(OrderTotal) from distro_orders where DistributorUserId={0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderStatus != {3}),0) as OrderPrice, ", new object[] { HiContext.Current.User.UserId, 1, 4, 9 }) + string.Format(" (SELECT COUNT(*) from vw_distro_Members where ParentUserId={0}) as UserNumb, ", HiContext.Current.User.UserId) + string.Format(" (SELECT count(*) from vw_distro_Members where UserID in (SELECT userid from distro_orders where DistributorUserId={0})) as UserOrderedNumb, ", HiContext.Current.User.UserId) + string.Format(" ISNULL((SELECT sum(VistiCounts) from distro_products where DistributorUserId={0}),0) as ProductVisitNumb ", HiContext.Current.User.UserId), new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
            }
            return result;
        }

        public override DbQueryResult GetSendGoodsOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DistributorUserId ='{0}' AND OrderStatus = 2", HiContext.Current.User.UserId);
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
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_OrderItems WHERE DistributorUserId ={0} AND ItemDescription LIKE '%{1}%')", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.ProductName));
                }
                if (!string.IsNullOrEmpty(query.ShipTo))
                {
                    builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
                }
                if (!string.IsNullOrEmpty(query.UserName))
                {
                    builder.AppendFormat(" AND Username = '{0}' ", DataHelper.CleanSearchString(query.UserName));
                }
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate >= '{0}'", query.StartDate.Value);
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate <= '{0}'", query.EndDate.Value);
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "distro_Orders do", "OrderId", builder.ToString(), "OrderId, OrderDate,RefundStatus, ShipTo, OrderTotal, OrderStatus,ShippingRegion,Address,ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,(SELECT ShipOrderNumber FROM Hishop_PurchaseOrders WHERE OrderId=do.OrderId) ShipOrderNumber");
        }

        public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
            if (includeDetail)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_ShippingTypeGroups Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId)";
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " SELECT * FROM Hishop_ShippingRegions Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId)";
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

        public override StatisticsInfo GetStatistics()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT " + string.Format(" (SELECT COUNT(OrderId) FROM distro_Orders WHERE OrderStatus={0} AND DistributorUserId={1}) AS orderNumbWaitConsignment,", 2, HiContext.Current.User.UserId) + string.Format(" (SELECT COUNT(*) FROM distro_BalanceDrawRequest WHERE DistributorUserId={0} ) AS applyRequestWaitDispose,", HiContext.Current.User.UserId) + string.Format(" (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus={0} AND DistributorId={1}) AS purchaseOrderNumbWaitConsignment, ", 1, HiContext.Current.User.UserId) + " 0 as productNumStokWarning," + string.Format("(SELECT Count(LeaveId) from distro_LeaveComments l where (SELECT count(replyId) from distro_LeaveCommentReplys where leaveId =l.leaveId)=0 and DistributorUserId={0} )  as leaveComments,", HiContext.Current.User.UserId) + string.Format("(SELECT Count(ConsultationId) from distro_ProductConsultations where ReplyUserId is null AND DistributorUserId={0}) as productConsultations,", HiContext.Current.User.UserId) + string.Format("(SELECT Count(*) from Hishop_DistributorMessageBox where IsRead=0 AND Accepter='{0}' AND Sernder <> 'admin') as messages,", HiContext.Current.User.Username) + string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) and  DistributorUserId={0} ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "') as orderNumbToday," + string.Format(" isnull((SELECT sum(OrderTotal)-isnull(sum(RefundAmount),0) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderPriceToday," + string.Format(" isnull((SELECT sum(orderProfit) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderProfitToday," + string.Format(" (SELECT count(*) from vw_distro_Members where ParentUserId={0} and CreateDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' ) as userNewAddToday,", HiContext.Current.User.UserId) + " 0 as agentNewAddToday, 0 as userNumbBirthdayToday ," + string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)) + "' and OrderDate<='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "') as orderNumbYesterday," + string.Format(" isnull((SELECT sum(orderTotal) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)) + "' and OrderDate<='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderPriceYesterday," + string.Format(" isnull((SELECT sum(orderProfit) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId) + " and OrderDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)) + "' and OrderDate<='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "'),0) as orderProfitYesterday," + string.Format(" (SELECT count(*) from vw_distro_Members where ParentUserId={0}) as userNumb,", HiContext.Current.User.UserId) + " 0 as agentNumb," + string.Format(" isnull((SELECT sum(Balance) from vw_distro_Members WHERE ParentUserId={0}),0) as memberBalance,", HiContext.Current.User.UserId) + " 0.00 as BalanceDrawRequested," + string.Format(" (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 1 AND DistributorId={0}) AS purchaseOrderNumbWaitConsignment,", HiContext.Current.User.UserId) + string.Format(" (SELECT count(productId) from distro_Products where SaleStatus={0} and DistributorUserId={1}) as productNumbOnSale,", 1, HiContext.Current.User.UserId) + string.Format(" (SELECT count(productId) from distro_Products where SaleStatus={0} and DistributorUserId={1}) as productNumbInStock,", 1, HiContext.Current.User.UserId) + string.Format(" (SELECT count(productId) from distro_Products where  DistributorUserId={0}) as authorizeProductCount,", HiContext.Current.User.UserId) + string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ) as arealdyPaidNum,", HiContext.Current.User.UserId) + string.Format(" (SELECT sum(OrderTotal) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ) as arealdyPaidTotal,", HiContext.Current.User.UserId) + string.Format(" (SELECT count(*) from distro_Products where DistributorUserId={0} and ProductId in (SELECT productId from Hishop_SKUs where Stock<=AlertStock group by productId)) as ProductAlert", HiContext.Current.User.UserId));
            StatisticsInfo info = new StatisticsInfo();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateStatistics(reader);
                }
            }
            return info;
        }

        public override OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_OrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, userOrder.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, userOrder.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, userOrder.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_OrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUserOrderQuery(userOrder));
            this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(SELECT count(*) from distro_Members where ParentUserId={0}) as AllUserCounts FROM distro_Members where ParentUserId={1}  GROUP BY TopRegionId ", HiContext.Current.User.UserId, HiContext.Current.User.UserId));
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

        public override bool IsExitPurchaseOrder(long tid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_PurchaseOrders WHERE TaobaoOrderId = @TaobaoOrderId");
            this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", DbType.String, tid.ToString());
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_PurchaseOrders set PurchaseTotal=@PurchaseTotal,PurchaseProfit=@PurchaseProfit where PurchaseOrderId=@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Decimal, purchaseOrder.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Decimal, purchaseOrder.GetPurchaseProfit());
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SaveDebitNote(DebitNote note)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" insert into distro_OrderDebitNote(DistributorUserId,NoteId,OrderId,Operator,Remark) values(@DistributorUserId,@NoteId,@OrderId,@Operator,@Remark)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, note.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SavePurchaseDebitNote(PurchaseDebitNote note)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" insert into Hishop_PurchaseDebitNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "NoteId", DbType.String, note.NoteId);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, note.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, note.Operator);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, note.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SavePurchaseOrderRemark(string purchaseOrderId, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Remark=@Remark WHERE PurchaseOrderId=@PurchaseOrderId AND DistributorId=@DistributorId");
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SaveRemark(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE OrderId=@OrderId AND DistributorUserId=@DistributorUserId");
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
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SaveShippingAddress(OrderInfo order)
        {
            if (order == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int SendGoods(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool SetPayment(string purchaseOrderId, int paymentTypeId, string paymentType, string gateway)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType,Gateway=@Gateway WHERE PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, paymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, paymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateDistributorAccount(decimal expenditureAdd)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET Expenditure=Expenditure+@expenditureAdd, PurchaseOrder = PurchaseOrder + 1 WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "expenditureAdd", DbType.Decimal, expenditureAdd);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_OrderItems SET ShipmentQuantity=@ShipmentQuantity, ItemAdjustedPrice=@ItemAdjustedPrice,Weight=@Weight,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
            this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Int32, lineItem.ItemWeight);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderAmount(OrderInfo order, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderTotal = @OrderTotal, OrderProfit=@OrderProfit, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount, OrderPoint=@OrderPoint,OrderCostPrice=@OrderCostPrice, Amount=@Amount WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, order.GetTotal());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, Convert.ToInt32((decimal) ((((order.GetTotal() - order.AdjustedFreight) - order.PayCharge) - order.Tax) / masterSettings.PointsRate)));
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderPaymentType(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET PaymentTypeId=@PaymentTypeId ,PaymentType=@PaymentType WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateOrderShippingMode(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void UpdatePayOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
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
                builder.Append("UPDATE distro_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts + @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(" AND DistributorUserId=@DistributorUserId").Append(";");
                this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + num, DbType.Int32, info.ProductId);
                num++;
            }
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateProductStock(string purchaseOrderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT oi.Quantity FROM Hishop_PurchaseOrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND oi.PurchaseOrderId =@PurchaseOrderId ))<=0 Then 0 ELSE Stock - (SELECT oi.Quantity FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND  oi.PurchaseOrderId =@PurchaseOrderId ) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId )");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Weight=@Weight,PurchaseProfit=@PurchaseProfit,PurchaseTotal=@PurchaseTotal,AdjustedFreight=@AdjustedFreight WHERE PurchaseOrderId=@PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Decimal, purchaseOrder.Weight);
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Decimal, purchaseOrder.GetPurchaseProfit());
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Decimal, purchaseOrder.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Decimal, purchaseOrder.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrder.PurchaseOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrderItems SET Quantity=@Quantity WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, Quantity);
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, POrderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, SkuId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set  Stock = Stock + (SELECT oi.ShipmentQuantity FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateShipper(ShippersInfo shipper)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ShipperTag", DbType.String, shipper.ShipperTag);
            this.database.AddInParameter(sqlStringCommand, "ShipperName", DbType.String, shipper.ShipperName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shipper.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shipper.Address);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shipper.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shipper.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shipper.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, shipper.Remark);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1, Points = @Points WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, totalPoint);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool UpdateUserRank(int userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET GradeId = @GradeId WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET Expenditure = ISNULL(Expenditure,0) - @refundAmount, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
            this.database.AddInParameter(sqlStringCommand, "refundAmount", DbType.Decimal, refundAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
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

