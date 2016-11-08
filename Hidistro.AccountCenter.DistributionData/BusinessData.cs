namespace Hidistro.AccountCenter.DistributionData
{
    using Hidistro.AccountCenter.Business;
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
    using System.Text;

    public class BusinessData : TradeSubsiteProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override int AddClaimCodeToUser(string claimCode, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_CouponItems SET UserId = @UserId, UserName=@UserName WHERE ClaimCode = @ClaimCode");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, HiContext.Current.User.Username);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

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

        private static string BuildMemberOrdersQuery(int userId, OrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" UserId = {0}", userId);
            if (!string.IsNullOrEmpty(query.OrderId))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId).ToLower());
            }
            else
            {
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
                    builder.AppendFormat(" AND OrderDate > '{0}'", query.StartDate);
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" AND OrderDate < '{0}'", query.EndDate);
                }
            }
            return builder.ToString();
        }

        public override bool ChangeMemberGrade(int userId, int gradId, int points)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM distro_MemberGrades WHERE CreateUserId=@CreateUserId Order by Point Desc ");
            this.database.AddInParameter(sqlStringCommand, "CreateUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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

        public override bool CloseOrder(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 4);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ConfirmOrderFinish(OrderInfo order)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,FinishDate=@FinishDate WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 5);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CountDownInfo CountDownBuy(int CountDownId)
        {
            CountDownInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "CountDownId", DbType.Int32, CountDownId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCountDown(reader);
                }
            }
            return info;
        }

        public override bool ExitCouponClaimCode(string claimCode)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ClaimCode) FROM distro_CouponItems WHERE ClaimCode = @ClaimCode AND ISNULL(UserId, 0) = 0");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, claimCode);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override DataTable GetChangeCoupons()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Coupons WHERE NeedPoint > 0 AND ClosingTime > @ClosingTime AND DistributorUserId = @DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            GroupBuyInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateGroupBuy(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    GropBuyConditionInfo item = new GropBuyConditionInfo {
                        Count = (int) reader["Count"],
                        Price = (decimal) reader["Price"]
                    };
                    info.GroupBuyConditions.Add(item);
                }
            }
            return info;
        }

        public override int GetHistoryPoint(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM distro_PointDetails WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override int GetOrderCount(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4 AND DistributorUserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            string query = "SELECT *,null as Tax,null as InvoiceTitle FROM distro_Orders WHERE OrderId = @OrderId;";
            query = query + "SELECT * FROM distro_OrderGifts WHERE OrderId = @OrderId;" + "SELECT * FROM distro_OrderItems WHERE OrderId = @OrderId;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
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

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId;");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            PaymentModeInfo info = null;
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
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override DataTable GetUserCoupons(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus FROM distro_CouponItems ci INNER JOIN distro_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId AND c.ClosingTime > @ClosingTime");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "ClosingTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetUserOrder(int userId, OrderQuery query)
        {
            if (string.IsNullOrEmpty(query.SortBy))
            {
                query.SortBy = "OrderDate";
            }
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "distro_Orders", null, BuildMemberOrdersQuery(userId, query), "*");
        }

        public override DbQueryResult GetUserPoints(int pageIndex)
        {
            return DataHelper.PagingByRownumber(pageIndex, 10, "JournalNumber", SortAction.Desc, true, "distro_PointDetails", "JournalNumber", string.Format("UserId={0}", HiContext.Current.User.UserId), "*");
        }

        public override bool SendClaimCodes(CouponItemInfo couponItem)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_CouponItems(CouponId, ClaimCode, GenerateTime, UserId, UserName, EmailAddress,LotNumber) VALUES(@CouponId, @ClaimCode, @GenerateTime, @UserId, @UserName, @EmailAddress,@LotNumber)");
            this.database.AddInParameter(sqlStringCommand, "CouponId", DbType.Int32, couponItem.CouponId);
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponItem.ClaimCode);
            this.database.AddInParameter(sqlStringCommand, "GenerateTime", DbType.DateTime, couponItem.GenerateTime);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, couponItem.UserName);
            this.database.AddInParameter(sqlStringCommand, "LotNumber", DbType.Guid, Guid.NewGuid());
            if (couponItem.UserId.HasValue)
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
            }
            else
            {
                this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, couponItem.EmailAddress);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "GroupBuyId", DbType.Int32, groupBuyId);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
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
                builder.Append("UPDATE distro_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts +  @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(" AND DistributorUserId=@DistributorUserId").Append(";");
                this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + num, DbType.Int32, info.ProductId);
                num++;
            }
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateStockPayOrder(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1, Points = @Points WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "OrderPrice", DbType.Decimal, orderTotal);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, totalPoint);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool UpdateUserRank(int userId, int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET GradeId = @GradeId WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UPDATE distro_Orders SET OrderStatus = {0}, PayDate = '{1}', GatewayOrderId = @GatewayOrderId WHERE OrderId = '{2}';", 2, DateTime.Now, order.OrderId);
            if (isBalancePayOrder)
            {
                Member user = Users.GetUser(order.UserId, false) as Member;
                decimal num = user.Balance - order.GetTotal();
                if ((user.Balance - user.RequestBalance) < order.GetTotal())
                {
                    return false;
                }
                builder.AppendFormat("INSERT INTO distro_BalanceDetails(UserId, UserName,DistributorUserId, TradeDate, TradeType, Expenses, Balance, Remark) VALUES({0},'{1}', {2}, '{3}', {4}, {5}, {6}, '{7}');", new object[] { order.UserId, HiContext.Current.User.Username, HiContext.Current.SiteSettings.UserId.Value, DateTime.Now, 3, order.GetTotal(), num, string.Format("对订单{0}付款", order.OrderId) });
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
        }
    }
}

