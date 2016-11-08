namespace Hidistro.Subsites.Sales
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public static class SubsiteSalesHelper
    {
        public static bool AddOrderGift(OrderInfo order, GiftInfo gift, int quantity, int promotype)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().AddOrderGift(order.OrderId, gift, quantity, promotype, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    bool flag2 = false;
                    foreach (OrderGiftInfo info in order.Gifts)
                    {
                        if (info.GiftId == gift.GiftId)
                        {
                            flag2 = true;
                            info.Quantity += quantity;
                        }
                    }
                    if (!flag2)
                    {
                        OrderGiftInfo item = new OrderGiftInfo();
                        item.GiftId = gift.GiftId;
                        item.OrderId = order.OrderId;
                        item.GiftName = gift.Name;
                        item.Quantity = quantity;
                        item.CostPrice = gift.PurchasePrice;
                        item.ThumbnailsUrl = gift.ThumbnailUrl40;
                        item.PromoteType = promotype;
                        order.Gifts.Add(item);
                    }
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
            return flag;
        }

        public static bool AddPurchaseItem(PurchaseShoppingCartItemInfo item)
        {
            return SubsiteSalesProvider.Instance().AddPurchaseItem(item);
        }

        public static bool AddPurchaseOrderGift(PurchaseOrderInfo purchaseOrder, GiftInfo gift, int quantity)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().AddPurchaseOrderGift(purchaseOrder.PurchaseOrderId, gift, quantity, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    bool flag2 = false;
                    foreach (PurchaseOrderGiftInfo info in purchaseOrder.PurchaseOrderGifts)
                    {
                        if (info.GiftId == gift.GiftId)
                        {
                            flag2 = true;
                            info.Quantity += quantity;
                        }
                    }
                    if (!flag2)
                    {
                        PurchaseOrderGiftInfo item = new PurchaseOrderGiftInfo();
                        item.GiftId = gift.GiftId;
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.GiftName = gift.Name;
                        item.Quantity = quantity;
                        item.PurchasePrice = gift.PurchasePrice;
                        item.ThumbnailsUrl = gift.ThumbnailUrl40;
                        purchaseOrder.PurchaseOrderGifts.Add(item);
                    }
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
            return flag;
        }

        public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
            int currentPOrderItemQuantity = provider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
            if (currentPOrderItemQuantity == 0)
            {
                return provider.AddPurchaseOrderItem(item, POrderId);
            }
            return provider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
        }

        public static bool ApplyForPurchaseRefund(string purchaseOrderId, string remark, int refundType)
        {
            return SubsiteSalesProvider.Instance().ApplyForPurchaseRefund(purchaseOrderId, remark, refundType);
        }

        public static bool ApplyForPurchaseReplace(string purchaseOrderId, string remark)
        {
            return SubsiteSalesProvider.Instance().ApplyForPurchaseReplace(purchaseOrderId, remark);
        }

        public static bool ApplyForPurchaseReturn(string purchaseOrderId, string remark, int refundType)
        {
            return SubsiteSalesProvider.Instance().ApplyForPurchaseReturn(purchaseOrderId, remark, refundType);
        }

        public static bool BatchConfirmPay(string purchaseOrderIds)
        {
            return SubsiteSalesProvider.Instance().BatchConfirmPay(purchaseOrderIds);
        }

        public static bool BatchConfirmPay(BalanceDetailInfo balance, string purchaseOrderIds)
        {
            return SubsiteSalesProvider.Instance().BatchConfirmPay(balance, purchaseOrderIds);
        }

        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0M)))
            {
                decimal num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal)((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value) + 1));
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static bool CanPurchaseRefund(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().CanPurchaseRefund(purchaseOrderId);
        }

        public static bool CanPurchaseReplace(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().CanPurchaseReplace(purchaseOrderId);
        }

        public static bool CanPurchaseReturn(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().CanPurchaseReturn(purchaseOrderId);
        }

        public static bool CheckRefund(OrderInfo order, string adminRemark, int refundType, bool accept)
        {
            if (order.OrderStatus != OrderStatus.ApplyForRefund)
            {
                return false;
            }
            bool flag = SubsiteSalesProvider.Instance().CheckRefund(order.OrderId, adminRemark, refundType, accept);
            if (flag && accept)
            {
                IUser user = Users.GetUser(order.UserId, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    ReducedPoint(order, user as Member);
                    ReduceDeduct(order.OrderId, order.GetTotal(), user as Member);
                    Users.ClearUserCache(user);
                }
                UpdateUserStatistics(order.UserId, order.RefundAmount, true);
            }
            return flag;
        }

        public static bool CheckReplace(string orderId, string adminRemark, bool accept)
        {
            if (GetOrderInfo(orderId).OrderStatus != OrderStatus.ApplyForReplacement)
            {
                return false;
            }
            return SubsiteSalesProvider.Instance().CheckReplace(orderId, adminRemark, accept);
        }

        public static bool CheckReturn(OrderInfo order, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            if (order.OrderStatus != OrderStatus.ApplyForReturns)
            {
                return false;
            }
            bool flag = SubsiteSalesProvider.Instance().CheckReturn(order.OrderId, refundMoney, adminRemark, refundType, accept);
            if (flag && accept)
            {
                IUser user = Users.GetUser(order.UserId, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    order.RefundAmount = refundMoney;
                    ReducedPoint(order, user as Member);
                    ReduceDeduct(order.OrderId, order.RefundAmount, user as Member);
                    Users.ClearUserCache(user);
                }
                UpdateUserStatistics(order.UserId, order.RefundAmount, false);
            }
            return flag;
        }

        public static bool ClearOrderGifts(OrderInfo order)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().ClearOrderGifts(order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    order.Gifts.Clear();
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
            return flag;
        }

        public static bool ClearPurchaseOrderGifts(PurchaseOrderInfo purchaseOrder)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().ClearPurchaseOrderGifts(purchaseOrder.PurchaseOrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    purchaseOrder.PurchaseOrderGifts.Clear();
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
            return flag;
        }

        public static void ClearPurchaseShoppingCart()
        {
            SubsiteSalesProvider.Instance().ClearPurchaseShoppingCart();
        }

        public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CLOSE) && SubsiteSalesProvider.Instance().ClosePurchaseOrder(purchaseOrder));
        }

        public static bool CloseTransaction(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_CLOSE) && SubsiteSalesProvider.Instance().CloseTransaction(order));
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_FINISH_TRADE) && SubsiteSalesProvider.Instance().ConfirmOrderFinish(order));
        }

        public static bool ConfirmPay(OrderInfo order)
        {
            if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
            {
                bool flag;
                using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                        if (provider.ConfirmPay(order, dbTran) <= 0)
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        if (order.GroupBuyId <= 0)
                        {
                            PurchaseOrderInfo purchaseOrder = provider.ConvertOrderToPurchaseOrder(order);
                            if (!provider.CreatePurchaseOrder(purchaseOrder, dbTran))
                            {
                                dbTran.Rollback();
                                return false;
                            }
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
                    SubsiteSalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
                    UpdateUserAccount(order);
                }
                return flag;
            }
            return false;
        }

        public static bool ConfirmPay(PurchaseOrderInfo purchaseOrder)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
            {
                return false;
            }
            bool flag = SubsiteSalesProvider.Instance().ConfirmPay(purchaseOrder.PurchaseOrderId);
            if (flag)
            {
                SubsiteSalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                SubsiteSalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
            }
            return flag;
        }

        public static bool ConfirmPay(BalanceDetailInfo balance, PurchaseOrderInfo purchaseOrder)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
            {
                return false;
            }
            bool flag = SubsiteSalesProvider.Instance().ConfirmPay(balance, purchaseOrder.PurchaseOrderId);
            if (flag)
            {
                SubsiteSalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                SubsiteSalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
            }
            return flag;
        }

        public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            return (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS) && SubsiteSalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder));
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static bool CreatePurchaseOrder(OrderInfo order)
        {
            if (order.CheckAction(OrderActions.SUBSITE_CREATE_PURCHASEORDER))
            {
                using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                        PurchaseOrderInfo purchaseOrder = provider.ConvertOrderToPurchaseOrder(order);
                        if (!provider.CreatePurchaseOrder(purchaseOrder, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        dbTran.Commit();
                        return true;
                    }
                    catch
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return false;
        }

        public static bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrderInfo)
        {
            return SubsiteSalesProvider.Instance().CreatePurchaseOrder(purchaseOrderInfo, null);
        }

        public static bool DelDebitNote(string[] noteIds, out int count)
        {
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= SubsiteSalesProvider.Instance().DelDebitNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool DeleteOrderGift(OrderInfo order, int giftId)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeleteOrderGift(order.OrderId, giftId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    foreach (OrderGiftInfo info in order.Gifts)
                    {
                        if (info.GiftId == giftId)
                        {
                            order.Gifts.Remove(info);
                            break;
                        }
                    }
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
            return flag;
        }

        public static bool DeleteOrderProduct(string sku, OrderInfo order)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeleteOrderProduct(sku, order.OrderId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    order.LineItems.Remove(sku);
                    if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTran))
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
            return flag;
        }

        public static int DeleteOrders(string orderIds)
        {
            return SubsiteSalesProvider.Instance().DeleteOrders(orderIds);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = new PaymentModeInfo();
            paymentMode.ModeId = modeId;
            return (SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete) == PaymentModeActionStatus.Success);
        }

        public static int DeletePurchaseOrde(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseOrde(purchaseOrderId);
        }

        public static bool DeletePurchaseOrderGift(PurchaseOrderInfo purchaseOrder, int giftId)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteSalesProvider.Instance().DeletePurchaseOrderGift(purchaseOrder.PurchaseOrderId, giftId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    foreach (PurchaseOrderGiftInfo info in purchaseOrder.PurchaseOrderGifts)
                    {
                        if (info.GiftId == giftId)
                        {
                            purchaseOrder.PurchaseOrderGifts.Remove(info);
                            break;
                        }
                    }
                    if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTran))
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
            return flag;
        }

        public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
        }

        public static bool DeletePurchaseShoppingCartItem(string sku)
        {
            return SubsiteSalesProvider.Instance().DeletePurchaseShoppingCartItem(sku);
        }

        public static bool DelRefundApply(string[] refundIds, out int count)
        {
            bool flag = true;
            count = 0;
            foreach (string str in refundIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SubsiteSalesProvider.Instance().DelRefundApply(int.Parse(str)))
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
                    if (SubsiteSalesProvider.Instance().DelReplaceApply(int.Parse(str)))
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
            bool flag = true;
            count = 0;
            foreach (string str in returnsIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SubsiteSalesProvider.Instance().DelReturnsApply(int.Parse(str)))
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

        public static bool GetAlertStock(string skuId)
        {
            return SubsiteSalesProvider.Instance().GetAlertStock(skuId);
        }

        public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
        {
            return SubsiteSalesProvider.Instance().GetAllDebitNote(query);
        }

        public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
        }

        public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, day, saleStatisticsType);
        }

        public static IList<string> GetExpressCompanysByMode(int modeId)
        {
            return SubsiteSalesProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static GiftInfo GetGiftDetails(int giftId)
        {
            return SubsiteSalesProvider.Instance().GetGiftDetails(giftId);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetGifts(query);
        }

        public static LineItemInfo GetLineItemInfo(string sku, string orderId)
        {
            return SubsiteSalesProvider.Instance().GetLineItemInfo(sku, orderId);
        }

        public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
        }

        public static ShippersInfo GetMyShipper()
        {
            return SubsiteSalesProvider.Instance().GetMyShipper();
        }

        public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetOrderGifts(query);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return SubsiteSalesProvider.Instance().GetOrderInfo(orderId);
        }

        public static DbQueryResult GetOrders(OrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetOrders(query);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return SubsiteSalesProvider.Instance().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return SubsiteSalesProvider.Instance().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return SubsiteSalesProvider.Instance().GetPaymentModes();
        }

        public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
        }

        public static PurchaseOrderInfo GetPurchaseByOrderId(string orderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseByOrderId(orderId);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrderGifts(query);
        }

        public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrders(query);
        }

        public static PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId)
        {
            return SubsiteSalesProvider.Instance().GetPurchaseOrderTaobaoInfo(tbOrderId);
        }

        public static IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos()
        {
            return SubsiteSalesProvider.Instance().GetPurchaseShoppingCartItemInfos();
        }

        public static DataTable GetRecentlyManualPurchaseOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyManualPurchaseOrders(out number);
        }

        public static DataTable GetRecentlyOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyOrders(out number);
        }

        public static DataTable GetRecentlyPurchaseOrders(out int number)
        {
            return SubsiteSalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
        }

        public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
        {
            return SubsiteSalesProvider.Instance().GetRefundApplys(query);
        }

        public static decimal GetRefundMoney(PurchaseOrderInfo purchaseOrder, out decimal refundMoney)
        {
            return SubsiteSalesProvider.Instance().GetRefundMoney(purchaseOrder, out refundMoney);
        }

        public static void GetRefundType(string orderId, out int refundType, out string refundRemark)
        {
            SubsiteSalesProvider.Instance().GetRefundType(orderId, out refundType, out refundRemark);
        }

        public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string refundRemark)
        {
            SubsiteSalesProvider.Instance().GetRefundTypeFromReturn(orderId, out refundType, out refundRemark);
        }

        public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
        {
            return SubsiteSalesProvider.Instance().GetReplaceApplys(query);
        }

        public static string GetReplaceComments(string orderId)
        {
            return SubsiteSalesProvider.Instance().GetReplaceComments(orderId);
        }

        public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
        {
            return SubsiteSalesProvider.Instance().GetReturnsApplys(query);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return SubsiteSalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
        }

        public static DbQueryResult GetSaleTargets()
        {
            return SubsiteSalesProvider.Instance().GetSaleTargets();
        }

        public static DbQueryResult GetSendGoodsOrders(OrderQuery query)
        {
            return SubsiteSalesProvider.Instance().GetSendGoodsOrders(query);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return SubsiteSalesProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static int GetSkuStock(string skuId)
        {
            return SubsiteSalesProvider.Instance().GetSkuStock(skuId);
        }

        public static StatisticsInfo GetStatistics()
        {
            return SubsiteSalesProvider.Instance().GetStatistics();
        }

        public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            return SubsiteSalesProvider.Instance().GetUserOrders(userOrder);
        }

        public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            return SubsiteSalesProvider.Instance().GetUserOrdersNoPage(userOrder);
        }

        public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
        {
            if (page == null)
            {
                totalProductSaleVisits = 0;
                return null;
            }
            return SubsiteSalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
        }

        public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SubsiteSalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
        }

        public static bool IsExitPurchaseOrder(long tid)
        {
            return SubsiteSalesProvider.Instance().IsExitPurchaseOrder(tid);
        }

        public static bool MondifyAddress(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS) && SubsiteSalesProvider.Instance().SaveShippingAddress(order));
        }

        private static void ReduceDeduct(string orderId, decimal refundAmount, Member member)
        {
            int referralDeduct = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId).ReferralDeduct;
            if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
            {
                IUser user = Users.GetUser(member.ReferralUserId.Value, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    Member member2 = user as Member;
                    if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                    {
                        decimal num2 = member2.Balance - ((refundAmount * referralDeduct) / 100M);
                        BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
                        balanceDetails.UserId = member2.UserId;
                        balanceDetails.UserName = member2.Username;
                        balanceDetails.TradeDate = DateTime.Now;
                        balanceDetails.TradeType = TradeTypes.ReferralDeduct;
                        balanceDetails.Expenses = new decimal?((refundAmount * referralDeduct) / 100M);
                        balanceDetails.Balance = num2;
                        balanceDetails.Remark = string.Format("退回提成因为{0}的订单{1}已退款", member.Username, orderId);
                        UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails);
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
            SubsiteSalesProvider.Instance().AddMemberPoint(point);
        }

        public static bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder)
        {
            return SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, null);
        }

        public static bool SaveDebitNote(DebitNote note)
        {
            return SubsiteSalesProvider.Instance().SaveDebitNote(note);
        }

        public static bool SavePurchaseDebitNote(PurchaseDebitNote note)
        {
            return SubsiteSalesProvider.Instance().SavePurchaseDebitNote(note);
        }

        public static bool SavePurchaseOrderRemark(string purchaseOrderId, string remark)
        {
            return SubsiteSalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrderId, remark);
        }

        public static bool SaveRemark(OrderInfo order)
        {
            return SubsiteSalesProvider.Instance().SaveRemark(order);
        }

        public static bool SendGoods(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                order.OrderStatus = OrderStatus.SellerAlreadySent;
                flag = SubsiteSalesProvider.Instance().SendGoods(order) > 0;
            }
            return flag;
        }

        public static bool SetMyShipper(ShippersInfo shipper)
        {
            ShippersInfo myShipper = SubsiteSalesProvider.Instance().GetMyShipper();
            Globals.EntityCoding(shipper, true);
            if (myShipper == null)
            {
                return SubsiteSalesProvider.Instance().AddShipper(shipper);
            }
            return SubsiteSalesProvider.Instance().UpdateShipper(shipper);
        }

        public static bool SetPayment(string purchaseOrderId, int paymentTypeId, string paymentType, string gateway)
        {
            return SubsiteSalesProvider.Instance().SetPayment(purchaseOrderId, paymentTypeId, paymentType, gateway);
        }

        public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteSalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateLineItem(string sku, OrderInfo order, int quantity)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    SubsiteSalesProvider provider = SubsiteSalesProvider.Instance();
                    order.LineItems[sku].ShipmentQuantity = quantity;
                    order.LineItems[sku].Quantity = quantity;
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
            return flag;
        }

        public static bool UpdateOrderAmount(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && SubsiteSalesProvider.Instance().UpdateOrderAmount(order, null));
        }

        public static bool UpdateOrderItem(string orderId, LineItemInfo lineItem)
        {
            return SubsiteSalesProvider.Instance().UpdateLineItem(orderId, lineItem, null);
        }

        public static bool UpdateOrderPaymentType(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE) && SubsiteSalesProvider.Instance().UpdateOrderPaymentType(order));
        }

        public static bool UpdateOrderShippingMode(OrderInfo order)
        {
            return (order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE) && SubsiteSalesProvider.Instance().UpdateOrderShippingMode(order));
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return SubsiteSalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
        }

        public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
        {
            return SubsiteSalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
        }

        private static void UpdateUserAccount(OrderInfo order)
        {
            int userId = order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Underling))
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
                SubsiteSalesProvider.Instance().AddMemberPoint(point);
                int referralDeduct = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId).ReferralDeduct;
                if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
                {
                    IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
                    if ((user2 != null) && (user2.UserRole == UserRole.Underling))
                    {
                        Member member2 = user2 as Member;
                        if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                        {
                            decimal num3 = member2.Balance + ((order.GetTotal() * referralDeduct) / 100M);
                            BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
                            balanceDetails.UserId = member2.UserId;
                            balanceDetails.UserName = member2.Username;
                            balanceDetails.TradeDate = DateTime.Now;
                            balanceDetails.TradeType = TradeTypes.ReferralDeduct;
                            balanceDetails.Income = new decimal?((order.GetTotal() * referralDeduct) / 100M);
                            balanceDetails.Balance = num3;
                            balanceDetails.Remark = string.Format("提成来自{0}的订单{1}", order.Username, order.OrderId);
                            UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails);
                        }
                    }
                }
                SubsiteSalesProvider.Instance().UpdateUserAccount(order.GetTotal(), point.Points, order.UserId);
                int historyPoint = SubsiteSalesProvider.Instance().GetHistoryPoint(member.UserId);
                SubsiteSalesProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
            }
            Users.ClearUserCache(user);
        }

        private static void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
        {
            SubsiteSalesProvider.Instance().UpdateUserStatistics(userId, refundAmount, isAllRefund);
        }
    }
}

