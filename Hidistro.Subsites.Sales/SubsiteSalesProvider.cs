namespace Hidistro.Subsites.Sales
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using Hidistro.Core;

    public abstract class SubsiteSalesProvider
    {
        private static readonly SubsiteSalesProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.SalesData,Hidistro.Subsites.Data") as SubsiteSalesProvider);

        protected SubsiteSalesProvider()
        {
        }

        public abstract bool AddMemberPoint(UserPointInfo point);
        public abstract bool AddOrderGift(string orderId, GiftInfo gift, int quantity, int promotype, DbTransaction dbTran);
        public abstract bool AddPurchaseItem(PurchaseShoppingCartItemInfo item);
        public abstract bool AddPurchaseOrderGift(string purchaseOrderId, GiftInfo gift, int quantity, DbTransaction dbTran);
        public abstract bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId);
        public abstract bool AddShipper(ShippersInfo shipper);
        public abstract bool ApplyForPurchaseRefund(string purchaseOrderId, string remark, int refundType);
        public abstract bool ApplyForPurchaseReplace(string purchaseOrderId, string remark);
        public abstract bool ApplyForPurchaseReturn(string purchaseOrderId, string remark, int refundType);
        public abstract bool BatchConfirmPay(string purchaseOrderIds);
        public abstract bool BatchConfirmPay(BalanceDetailInfo balance, string purchaseOrderId);
        public abstract bool CanPurchaseRefund(string purchaseOrderId);
        public abstract bool CanPurchaseReplace(string purchaseOrderId);
        public abstract bool CanPurchaseReturn(string purchaseOrderId);
        public abstract bool ChangeMemberGrade(int userId, int gradId, int points);
        public abstract bool CheckRefund(string orderId, string adminRemark, int refundType, bool accept);
        public abstract bool CheckReplace(string orderId, string adminRemark, bool accept);
        public abstract bool CheckReturn(string orderId, decimal refundMoney, string adminRemark, int refundType, bool accept);
        public abstract bool ClearOrderGifts(string orderId, DbTransaction dbTran);
        public abstract bool ClearPurchaseOrderGifts(string purchaseOrderId, DbTransaction dbTran);
        public abstract void ClearPurchaseShoppingCart();
        public abstract bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract bool CloseTransaction(OrderInfo order);
        public abstract bool ConfirmOrderFinish(OrderInfo order);
        public abstract bool ConfirmPay(string purchaseOrderId);
        public abstract bool ConfirmPay(BalanceDetailInfo balance, string purchaseOrderId);
        public abstract int ConfirmPay(OrderInfo order, DbTransaction dbTran);
        public abstract bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder);
        public abstract PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order);
        public abstract bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrder, DbTransaction dbTran);
        public abstract PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action);
        public abstract bool DelDebitNote(string noteId);
        public abstract bool DeleteOrderGift(string orderId, int giftId, DbTransaction dbTran);
        public abstract bool DeleteOrderProduct(string sku, string orderId, DbTransaction dbTran);
        public abstract int DeleteOrders(string orderIds);
        public abstract int DeletePurchaseOrde(string purchaseOrderId);
        public abstract bool DeletePurchaseOrderGift(string purchaseOrderId, int giftId, DbTransaction dbTran);
        public abstract bool DeletePurchaseOrderItem(string POrderId, string skuId);
        public abstract bool DeletePurchaseShoppingCartItem(string sku);
        public abstract bool DelRefundApply(int refundId);
        public abstract bool DelReplaceApply(int replaceId);
        public abstract bool DelReturnsApply(int returnsId);
        public abstract bool GetAlertStock(string skuId);
        public abstract DbQueryResult GetAllDebitNote(DebitNoteQuery query);
        public abstract int GetCurrentPOrderItemQuantity(string POrderId, string skuId);
        public abstract DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType);
        public abstract IList<string> GetExpressCompanysByMode(int modeId);
        public abstract GiftInfo GetGiftDetails(int giftId);
        public abstract DbQueryResult GetGifts(GiftQuery query);
        public abstract int GetHistoryPoint(int userId);
        public abstract LineItemInfo GetLineItemInfo(string sku, string orderId);
        public abstract void GetLineItemPromotions(int productId, int quantity, out int purchaseGiftId, out string purchaseGiftName, out int giveQuantity, out int wholesaleDiscountId, out string wholesaleDiscountName, out decimal? discountRate, int gradeId);
        public abstract DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType);
        public abstract decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract ShippersInfo GetMyShipper();
        public abstract DbQueryResult GetOrderGifts(OrderGiftQuery query);
        public abstract OrderInfo GetOrderInfo(string orderId);
        public abstract DbQueryResult GetOrders(OrderQuery query);
        public abstract PaymentModeInfo GetPaymentMode(int modeId);
        public abstract PaymentModeInfo GetPaymentMode(string gateway);
        public abstract IList<PaymentModeInfo> GetPaymentModes();
        public abstract DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales);
        public abstract DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales);
        public abstract PurchaseOrderInfo GetPurchaseByOrderId(string orderId);
        public abstract PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId);
        public abstract DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query);
        public abstract DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query);
        public abstract PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId);
        public abstract IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos();
        public abstract DataTable GetRecentlyManualPurchaseOrders(out int number);
        public abstract DataTable GetRecentlyOrders(out int number);
        public abstract DataTable GetRecentlyPurchaseOrders(out int number);
        public abstract DbQueryResult GetRefundApplys(RefundApplyQuery query);
        public abstract decimal GetRefundMoney(PurchaseOrderInfo purchaseOrder, out decimal refundMoney);
        public abstract void GetRefundType(string orderId, out int refundType, out string refundRemark);
        public abstract void GetRefundTypeFromReturn(string orderId, out int refundType, out string refundRemark);
        public abstract DbQueryResult GetReplaceApplys(ReplaceApplyQuery query);
        public abstract string GetReplaceComments(string orderId);
        public abstract DbQueryResult GetReturnsApplys(ReturnsApplyQuery query);
        public abstract DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query);
        public abstract DbQueryResult GetSaleTargets();
        public abstract DbQueryResult GetSendGoodsOrders(OrderQuery query);
        public abstract ShippingModeInfo GetShippingMode(int modeId, bool includeDetail);
        public abstract int GetSkuStock(string skuId);
        public abstract StatisticsInfo GetStatistics();
        public abstract OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder);
        public abstract OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder);
        public abstract IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits);
        public abstract decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType);
        public static SubsiteSalesProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool IsExitPurchaseOrder(long tid);
        public abstract bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder, DbTransaction dbTran);
        public abstract bool SaveDebitNote(DebitNote note);
        public abstract bool SavePurchaseDebitNote(PurchaseDebitNote note);
        public abstract bool SavePurchaseOrderRemark(string purchaseOrderId, string remark);
        public abstract bool SaveRemark(OrderInfo order);
        public abstract bool SaveShippingAddress(OrderInfo order);
        public abstract int SendGoods(OrderInfo order);
        public abstract bool SetPayment(string purchaseOrderId, int paymentTypeId, string paymentType, string gateway);
        public abstract void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence);
        public abstract void UpdateDistributorAccount(decimal expenditure);
        public abstract bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran);
        public abstract bool UpdateOrderAmount(OrderInfo order, DbTransaction dbTran);
        public abstract bool UpdateOrderPaymentType(OrderInfo order);
        public abstract bool UpdateOrderShippingMode(OrderInfo order);
        public abstract void UpdatePayOrderStock(string orderId);
        public abstract bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems);
        public abstract void UpdateProductStock(string purchaseOrderId);
        public abstract bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder);
        public abstract bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity);
        public abstract void UpdateRefundOrderStock(string orderId);
        public abstract bool UpdateShipper(ShippersInfo shipper);
        public abstract bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId);
        public abstract void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund);
    }
}

