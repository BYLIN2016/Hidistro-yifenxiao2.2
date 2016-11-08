namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public static class OrderHelper
    {
        public static bool AddOrderGift(OrderInfo order, GiftInfo giftinfo, int quantity, int promotype)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SalesProvider provider = SalesProvider.Instance();
                    OrderGiftInfo item = new OrderGiftInfo();
                    item.OrderId = order.OrderId;
                    item.Quantity = quantity;
                    item.GiftName = giftinfo.Name;
                    decimal costPrice = item.CostPrice;
                    item.CostPrice = Convert.ToDecimal(giftinfo.CostPrice);
                    item.GiftId = giftinfo.GiftId;
                    item.ThumbnailsUrl = giftinfo.ThumbnailUrl40;
                    item.PromoteType = promotype;
                    bool flag2 = false;
                    foreach (OrderGiftInfo info2 in order.Gifts)
                    {
                        if (giftinfo.GiftId == info2.GiftId)
                        {
                            flag2 = true;
                            info2.Quantity = quantity;
                            info2.PromoteType = promotype;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        order.Gifts.Add(item);
                    }
                    if (!provider.AddOrderGift(order.OrderId, item, quantity, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "成功的为订单号为\"{0}\"的订单添加了礼品", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool CheckRefund(OrderInfo order, string Operator, string adminRemark, int refundType, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
            if (order.OrderStatus != OrderStatus.ApplyForRefund)
            {
                return false;
            }
            bool flag = SalesProvider.Instance().CheckRefund(order.OrderId, Operator, adminRemark, refundType, accept);
            if (flag)
            {
                if (accept)
                {
                    IUser user = Users.GetUser(order.UserId, false);
                    if ((user != null) && (user.UserRole == UserRole.Member))
                    {
                        ReducedPoint(order, user as Member);
                        ReduceDeduct(order.OrderId, order.GetTotal(), user as Member);
                        Users.ClearUserCache(user);
                    }
                    UpdateUserStatistics(order.UserId, order.RefundAmount, true);
                    SalesProvider.Instance().UpdateRefundOrderStock(order.OrderId);
                }
                if (accept && (order.GroupBuyId > 0))
                {
                    EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的扣除违约金后退款", new object[] { order.OrderId }));
                }
                else
                {
                    EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了全额退款", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool CheckReplace(string orderId, string adminRemark, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApply);
            if (GetOrderInfo(orderId).OrderStatus != OrderStatus.ApplyForReplacement)
            {
                return false;
            }
            return SalesProvider.Instance().CheckReplace(orderId, adminRemark, accept);
        }

        public static bool CheckReturn(OrderInfo order, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
            if (order.OrderStatus != OrderStatus.ApplyForReturns)
            {
                return false;
            }
            bool flag = SalesProvider.Instance().CheckReturn(order.OrderId, Operator, refundMoney, adminRemark, refundType, accept);
            if (flag)
            {
                if (accept)
                {
                    order.RefundAmount = refundMoney;
                    IUser user = Users.GetUser(order.UserId, false);
                    if ((user != null) && (user.UserRole == UserRole.Member))
                    {
                        ReducedPoint(order, user as Member);
                        ReduceDeduct(order.OrderId, order.RefundAmount, user as Member);
                        Users.ClearUserCache(user);
                    }
                    UpdateUserStatistics(order.UserId, order.RefundAmount, false);
                }
                EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool ClearOrderGifts(OrderInfo order)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SalesProvider provider = SalesProvider.Instance();
                    order.Gifts.Clear();
                    if (!provider.ClearOrderGifts(order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "清空了订单号为\"{0}\"的订单礼品", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool CloseTransaction(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (order.CheckAction(OrderActions.SELLER_CLOSE))
            {
                bool flag = SalesProvider.Instance().CloseTransaction(order);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[] { order.OrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE))
            {
                flag = SalesProvider.Instance().ConfirmOrderFinish(order);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool ConfirmPay(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                flag = SalesProvider.Instance().ConfirmPay(order) > 0;
                if (flag)
                {
                    SalesProvider.Instance().UpdatePayOrderStock(order.OrderId);
                    SalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
                    UpdateUserAccount(order);
                    EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool DelDebitNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= SalesProvider.Instance().DelDebitNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool DeleteLineItem(string sku, OrderInfo order)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SalesProvider provider = SalesProvider.Instance();
                    order.LineItems.Remove(sku);
                    if (!provider.DeleteLineItem(sku, order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool DeleteOrderGift(OrderInfo order, int giftId)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SalesProvider provider = SalesProvider.Instance();
                    OrderGiftInfo orderGift = provider.GetOrderGift(giftId, order.OrderId);
                    order.Gifts.Remove(orderGift);
                    if (!provider.DeleteOrderGift(order.OrderId, orderGift.GiftId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单礼品", new object[] { order.OrderId }));
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static int DeleteOrders(string orderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            int num = SalesProvider.Instance().DeleteOrders(orderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[] { orderIds }));
            }
            return num;
        }

        public static bool DelRefundApply(string[] refundIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
            bool flag = true;
            count = 0;
            foreach (string str in refundIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelRefundApply(int.Parse(str)))
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static bool DelReplaceApply(string[] replaceIds, out int count)
        {
            bool flag = true;
            count = 0;
            foreach (string str in replaceIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelReplaceApply(int.Parse(str)))
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static bool DelReturnsApply(string[] returnsIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
            bool flag = true;
            count = 0;
            foreach (string str in returnsIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelReturnsApply(int.Parse(str)))
                    {
                        count++;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        public static bool DelSendNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= SalesProvider.Instance().DelSendNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool EditPurchaseOrderShipNumber(string purchaseOrderId, string orderId, string startNumber)
        {
            return SalesProvider.Instance().EditPurchaseOrderShipNumber(purchaseOrderId, orderId, startNumber);
        }

        public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            return SalesProvider.Instance().GetAllDebitNote(query);
        }

        public static DbQueryResult GetAllSendNote(RefundApplyQuery query)
        {
            return SalesProvider.Instance().GetAllSendNote(query);
        }

        public static IList<GiftInfo> GetGiftList(GiftQuery query)
        {
            return SalesProvider.Instance().GetGiftList(query);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return SalesProvider.Instance().GetGifts(query);
        }

        public static IList<LineItemInfo> GetLineItemInfo(string orderId)
        {
            return SalesProvider.Instance().GetLineItemInfo(orderId);
        }

        public static LineItemInfo GetLineItemInfo(string sku, string orderId)
        {
            return SalesProvider.Instance().GetLineItemInfo(sku, orderId);
        }

        public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            return SalesProvider.Instance().GetOrderGifts(query);
        }

        public static DataSet GetOrderGoods(string orderIds)
        {
            return SalesProvider.Instance().GetOrderGoods(orderIds);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return SalesProvider.Instance().GetOrderInfo(orderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return SalesProvider.Instance().GetOrders(query);
        }

        public static DataSet GetOrdersAndLines(string orderIds)
        {
            return SalesProvider.Instance().GetOrdersAndLines(orderIds);
        }

        public static DataSet GetProductGoods(string orderIds)
        {
            return SalesProvider.Instance().GetProductGoods(orderIds);
        }

        public static DataSet GetPurchaseOrderGoods(string purchaseorderIds)
        {
            return SalesProvider.Instance().GetPurchaseOrderGoods(purchaseorderIds);
        }

        public static DataSet GetPurchaseProductGoods(string purchaseorderIds)
        {
            return SalesProvider.Instance().GetPurchaseProductGoods(purchaseorderIds);
        }

        public static DataTable GetRecentlyOrders(out int number)
        {
            return SalesProvider.Instance().GetRecentlyOrders(out number);
        }

        public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            return SalesProvider.Instance().GetRefundApplys(query);
        }

        public static void GetRefundType(string orderId, out int refundType, out string remark)
        {
            SalesProvider.Instance().GetRefundType(orderId, out refundType, out remark);
        }

        public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
        {
            SalesProvider.Instance().GetRefundTypeFromReturn(orderId, out refundType, out remark);
        }

        public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            return SalesProvider.Instance().GetReplaceApplys(query);
        }

        public static string GetReplaceComments(string orderId)
        {
            return SalesProvider.Instance().GetReplaceComments(orderId);
        }

        public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            return SalesProvider.Instance().GetReturnsApplys(query);
        }

        public static DataTable GetSendGoodsOrders(string orderIds)
        {
            return SalesProvider.Instance().GetSendGoodsOrders(orderIds);
        }

        public static int GetSkuStock(string skuId)
        {
            return SalesProvider.Instance().GetSkuStock(skuId);
        }

        public static DataSet GetTradeOrders(string orderId)
        {
            return SalesProvider.Instance().GetTradeOrders(orderId);
        }

        public static DataSet GetTradeOrders(OrderQuery query, out int records)
        {
            return SalesProvider.Instance().GetTradeOrders(query, out records);
        }

        public static bool MondifyAddress(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
            {
                bool flag = SalesProvider.Instance().SaveShippingAddress(order);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[] { order.OrderId }));
                }
                return flag;
            }
            return false;
        }

        private static void ReduceDeduct(string orderId, decimal refundAmount, Member member)
        {
            int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
            if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
            {
                IUser user = Users.GetUser(member.ReferralUserId.Value, false);
                if ((user != null) && (user.UserRole == UserRole.Member))
                {
                    Member member2 = user as Member;
                    if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                    {
                        decimal num2 = member2.Balance - ((refundAmount * referralDeduct) / 100M);
                        BalanceDetailInfo balanceDetail = new BalanceDetailInfo();
                        balanceDetail.UserId = member2.UserId;
                        balanceDetail.UserName = member2.Username;
                        balanceDetail.TradeDate = DateTime.Now;
                        balanceDetail.TradeType = TradeTypes.ReferralDeduct;
                        balanceDetail.Expenses = new decimal?((refundAmount * referralDeduct) / 100M);
                        balanceDetail.Balance = num2;
                        balanceDetail.Remark = string.Format("退回提成因为{0}的订单{1}已退款", member.Username, orderId);
                        MemberProvider.Instance().InsertBalanceDetail(balanceDetail);
                    }
                }
            }
        }

        private static void ReducedPoint(OrderInfo order, Member member)
        {
            UserPointInfo point = new UserPointInfo();
            point.OrderId = order.OrderId;
            point.UserId = member.UserId;
            point.TradeDate = DateTime.Now;
            point.TradeType = UserPointTradeType.Refund;
            point.Reduced = new int?(order.Points);
            point.Points = member.Points - point.Reduced.Value;
            SalesProvider.Instance().AddMemberPoint(point);
        }

        public static bool SaveDebitNote(DebitNote note)
        {
            return SalesProvider.Instance().SaveDebitNote(note);
        }

        public static bool SaveRemark(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
            bool flag = SalesProvider.Instance().SaveOrderRemark(order);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SaveRemarkAPI(OrderInfo order)
        {
            bool flag = SalesProvider.Instance().SaveOrderRemark(order);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SaveSendNote(SendNote note)
        {
            return SalesProvider.Instance().SaveSendNote(note);
        }

        public static bool SendAPIGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                flag = SalesProvider.Instance().SendGoods(order) > 0;
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool SendGoods(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                flag = SalesProvider.Instance().SendGoods(order) > 0;
                if (!flag)
                {
                    return flag;
                }
                if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
                {
                    SalesProvider.Instance().UpdatePayOrderStock(order.OrderId);
                    SalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
                    UpdateUserAccount(order);
                }
                EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool SetOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return SalesProvider.Instance().SetOrderExpressComputerpe(purchaseOrderIds, expressCompanyName, expressCompanyAbb);
        }

        public static void SetOrderPrinted(string[] orderIds, bool isPrinted)
        {
            int num = 0;
            for (int i = orderIds.Length - 1; i >= 0; i--)
            {
                if (SalesProvider.Instance().SetOrderPrinted(orderIds[i], isPrinted))
                {
                    num++;
                }
            }
        }

        public static bool SetOrderShipNumber(string orderId, string startNumber)
        {
            return SalesProvider.Instance().EditOrderShipNumber(orderId, startNumber);
        }

        public static void SetOrderShipNumber(string[] orderIds, string startNumber)
        {
            int num = 0;
            for (int i = 0; i < orderIds.Length; i++)
            {
                long num3 = long.Parse(startNumber) + num;
                if (SalesProvider.Instance().EditOrderShipNumber(orderIds[i], num3.ToString()))
                {
                    num++;
                }
            }
        }

        public static bool SetOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName)
        {
            return SalesProvider.Instance().SetOrderShippingMode(purchaseOrderIds, realShippingModeId, realModeName);
        }

        public static bool UpdateLineItem(string sku, OrderInfo order, int quantity)
        {
            bool flag;
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SalesProvider provider = SalesProvider.Instance();
                    order.LineItems[sku].Quantity = quantity;
                    order.LineItems[sku].ShipmentQuantity = quantity;
                    order.LineItems[sku].ItemAdjustedPrice = order.LineItems[sku].ItemListPrice;
                    if (!provider.UpdateLineItem(order.OrderId, order.LineItems[sku], dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.UpdateOrderAmount(order, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单号为\"{0}\"的订单商品数量", new object[] { order.OrderId }));
            }
            return flag;
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                flag = SalesProvider.Instance().UpdateOrderAmount(order, null);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[] { order.OrderId }));
                }
            }
            return flag;
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
            {
                bool flag = SalesProvider.Instance().UpdateOrderPaymentType(order);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[] { order.OrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool UpdateOrderShippingMode(OrderInfo order)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditOrders);
            if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE))
            {
                bool flag = SalesProvider.Instance().UpdateOrderShippingMode(order);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的配送方式", new object[] { order.OrderId }));
                }
                return flag;
            }
            return false;
        }

        private static void UpdateUserAccount(OrderInfo order)
        {
            int userId = order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Member))
            {
                Member member = user as Member;
                UserPointInfo point = new UserPointInfo();
                point.OrderId = order.OrderId;
                point.UserId = member.UserId;
                point.TradeDate = DateTime.Now;
                point.TradeType = UserPointTradeType.Bounty;
                point.Increased = new int?(order.Points);
                point.Points = order.Points + member.Points;
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                SalesProvider.Instance().AddMemberPoint(point);
                int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
                if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
                {
                    IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
                    if ((user2 != null) && (user2.UserRole == UserRole.Member))
                    {
                        Member member2 = user2 as Member;
                        if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                        {
                            decimal num3 = member2.Balance + ((order.GetTotal() * referralDeduct) / 100M);
                            BalanceDetailInfo balanceDetail = new BalanceDetailInfo();
                            balanceDetail.UserId = member2.UserId;
                            balanceDetail.UserName = member2.Username;
                            balanceDetail.TradeDate = DateTime.Now;
                            balanceDetail.TradeType = TradeTypes.ReferralDeduct;
                            balanceDetail.Income = new decimal?((order.GetTotal() * referralDeduct) / 100M);
                            balanceDetail.Balance = num3;
                            balanceDetail.Remark = string.Format("提成来自{0}的订单{1}", order.Username, order.OrderId);
                            MemberProvider.Instance().InsertBalanceDetail(balanceDetail);
                        }
                    }
                }
                SalesProvider.Instance().UpdateUserAccount(order.GetTotal(), order.UserId);
                int historyPoint = SalesProvider.Instance().GetHistoryPoint(member.UserId);
                SalesProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
                Users.ClearUserCache(user);
            }
        }

        private static void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            SalesProvider.Instance().UpdateUserStatistics(userId, refundAmount, isAllRefund);
        }
    }
}

