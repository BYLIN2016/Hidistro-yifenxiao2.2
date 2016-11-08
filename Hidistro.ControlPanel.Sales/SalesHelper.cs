namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public sealed class SalesHelper
    {
        private SalesHelper()
        {
        }

        public static bool AddExpressTemplate(string expressName, string xmlFile)
        {
            return SalesProvider.Instance().AddExpressTemplate(expressName, xmlFile);
        }

        public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
        {
            SalesProvider provider = SalesProvider.Instance();
            int currentPOrderItemQuantity = provider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
            if (currentPOrderItemQuantity == 0)
            {
                return provider.AddPurchaseOrderItem(item, POrderId);
            }
            return provider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
        }

        public static bool AddShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return SalesProvider.Instance().AddShipper(shipper);
        }

        public static decimal CalcFreight(int regionId, int totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            decimal num4 = 1M;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0M)))
            {
                decimal num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = (num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value;
                }
                else
                {
                    num4 = ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value) + 1;
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

        public static bool CheckPurchaseRefund(PurchaseOrderInfo purchaseOrder, string Operator, string adminRemark, int refundType, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderRefundApply);
            if (purchaseOrder.PurchaseStatus != OrderStatus.ApplyForRefund)
            {
                return false;
            }
            bool flag = SalesProvider.Instance().CheckPurchaseRefund(purchaseOrder.PurchaseOrderId, Operator, adminRemark, refundType, accept);
            if (flag)
            {
                if (accept)
                {
                    SalesProvider.Instance().UpdateRefundSubmitPurchaseOrderStock(purchaseOrder);
                }
                EventLogs.WriteOperationLog(Privilege.PurchaseorderRefund, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单退款", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool CheckPurchaseReplace(string purchaseOrderId, string adminRemark, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReplaceApply);
            if (GetPurchaseOrder(purchaseOrderId).PurchaseStatus != OrderStatus.ApplyForReplacement)
            {
                return false;
            }
            return SalesProvider.Instance().CheckPurchaseReplace(purchaseOrderId, adminRemark, accept);
        }

        public static bool CheckPurchaseReturn(string purchaseOrderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReturnsApply);
            if (GetPurchaseOrder(purchaseOrderId).PurchaseStatus != OrderStatus.ApplyForReturns)
            {
                return false;
            }
            bool flag = SalesProvider.Instance().CheckPurchaseReturn(purchaseOrderId, Operator, refundMoney, adminRemark, refundType, accept);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.PurchaseorderRefund, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单退货", new object[] { purchaseOrderId }));
            }
            return flag;
        }

        public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            bool flag = SalesProvider.Instance().ClosePurchaseOrder(purchaseOrder);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "关闭了编号为\"{0}\"的采购单", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            bool flag = SalesProvider.Instance().ConfirmPayPurchaseOrder(purchaseOrder);
            if (flag)
            {
                SalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                SalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal(), purchaseOrder.DistributorId);
                Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单线下收款", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
            {
                bool flag = SalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的采购单", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
        }

        public static bool CreateShippingMode(ShippingModeInfo shippingMode)
        {
            if (null == shippingMode)
            {
                return false;
            }
            return SalesProvider.Instance().CreateShippingMode(shippingMode);
        }

        public static bool CreateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return SalesProvider.Instance().CreateShippingTemplate(shippingMode);
        }

        public static bool DeleteExpressTemplate(int expressId)
        {
            return SalesProvider.Instance().DeleteExpressTemplate(expressId);
        }

        public static bool DeletePaymentMode(int modeId)
        {
            PaymentModeInfo paymentMode = new PaymentModeInfo();
            paymentMode.ModeId = modeId;
            return (SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Delete) == PaymentModeActionStatus.Success);
        }

        public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
        {
            return SalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
        }

        public static int DeletePurchaseOrders(string purchaseOrderIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
            int num = SalesProvider.Instance().DeletePurchaseOrders(purchaseOrderIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeletePurchaseorder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的采购单", new object[] { purchaseOrderIds }));
            }
            return num;
        }

        public static bool DeleteShipper(int shipperId)
        {
            return SalesProvider.Instance().DeleteShipper(shipperId);
        }

        public static bool DeleteShippingMode(int modeId)
        {
            return SalesProvider.Instance().DeleteShippingMode(modeId);
        }

        public static bool DeleteShippingTemplate(int templateId)
        {
            return SalesProvider.Instance().DeleteShippingTemplate(templateId);
        }

        public static bool DelPurchaseDebitNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= SalesProvider.Instance().DelPurchaseDebitNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static bool DelPurchaseRefundApply(string[] refundIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderRefundApply);
            bool flag = true;
            count = 0;
            foreach (string str in refundIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelPurchaseRefundApply(int.Parse(str)))
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

        public static bool DelPurchaseReplaceApply(string[] replaceIds, out int count)
        {
            bool flag = true;
            count = 0;
            foreach (string str in replaceIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelPurchaseReplaceApply(int.Parse(str)))
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

        public static bool DelPurchaseReturnsApply(string[] returnsIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReturnsApply);
            bool flag = true;
            count = 0;
            foreach (string str in returnsIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    if (SalesProvider.Instance().DelPurchaseReturnsApply(int.Parse(str)))
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

        public static bool DelPurchaseSendNote(string[] noteIds, out int count)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
            bool flag = true;
            count = 0;
            foreach (string str in noteIds)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    flag &= SalesProvider.Instance().DelPurchaseSendNote(str);
                    if (flag)
                    {
                        count++;
                    }
                }
            }
            return flag;
        }

        public static DbQueryResult GetAllPurchaseDebitNote(DebitNoteQuery query)
        {
            return SalesProvider.Instance().GetAllPurchaseDebitNote(query);
        }

        public static DbQueryResult GetAllPurchaseSendNote(RefundApplyQuery query)
        {
            return SalesProvider.Instance().GetAllPurchaseSendNote(query);
        }

        public static DataSet GetAPIPurchaseOrders(PurchaseOrderQuery query, out int totalrecord)
        {
            return SalesProvider.Instance().GetAPIPurchaseOrders(query, out totalrecord);
        }

        public static DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
        }

        public static decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetDaySaleTotal(year, month, day, saleStatisticsType);
        }

        public static IList<string> GetExpressCompanysByMode(int modeId)
        {
            return SalesProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static DataTable GetExpressTemplates()
        {
            return SalesProvider.Instance().GetExpressTemplates();
        }

        public static DataTable GetIsUserExpressTemplates()
        {
            return SalesProvider.Instance().GetIsUserExpressTemplates();
        }

        public static DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetMemberStatistics(query, out totalProductSales);
        }

        public static DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetMemberStatisticsNoPage(query);
        }

        public static DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
        }

        public static IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
        {
            return SalesProvider.Instance().GetOrderPriceStatisticsOfSevenDays(days);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return SalesProvider.Instance().GetPaymentMode(modeId);
        }

        public static PaymentModeInfo GetPaymentMode(string gateway)
        {
            return SalesProvider.Instance().GetPaymentMode(gateway);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return SalesProvider.Instance().GetPaymentModes();
        }

        public static DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return SalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
        }

        public static DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
        {
            return SalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return SalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
        {
            return SalesProvider.Instance().GetPurchaseOrders(query);
        }

        public static DataSet GetPurchaseOrdersAndLines(string purchaseOrderIds)
        {
            return SalesProvider.Instance().GetPurchaseOrdersAndLines(purchaseOrderIds);
        }

        public static DbQueryResult GetPurchaseRefundApplys(RefundApplyQuery query)
        {
            return SalesProvider.Instance().GetPurchaseRefundApplys(query);
        }

        public static void GetPurchaseRefundType(string purchaseOrderId, out int refundType, out string remark)
        {
            SalesProvider.Instance().GetPurchaseRefundType(purchaseOrderId, out refundType, out remark);
        }

        public static void GetPurchaseRefundTypeFromReturn(string purchaseOrderId, out int refundType, out string remark)
        {
            SalesProvider.Instance().GetPurchaseRefundTypeFromReturn(purchaseOrderId, out refundType, out remark);
        }

        public static DbQueryResult GetPurchaseReplaceApplys(ReplaceApplyQuery query)
        {
            return SalesProvider.Instance().GetPurchaseReplaceApplys(query);
        }

        public static string GetPurchaseReplaceComments(string purchaseOrderId)
        {
            return SalesProvider.Instance().GetPurchaseReplaceComments(purchaseOrderId);
        }

        public static DbQueryResult GetPurchaseReturnsApplys(ReturnsApplyQuery query)
        {
            return SalesProvider.Instance().GetPurchaseReturnsApplys(query);
        }

        public static DataTable GetRecentlyPurchaseOrders(out int number)
        {
            return SalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
        }

        public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
        {
            return SalesProvider.Instance().GetSaleOrderLineItemsStatisticsNoPage(query);
        }

        public static DbQueryResult GetSaleTargets()
        {
            return SalesProvider.Instance().GetSaleTargets();
        }

        public static DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
        {
            return SalesProvider.Instance().GetSendGoodsPurchaseOrders(purchaseOrderIds);
        }

        public static ShippersInfo GetShipper(int shipperId)
        {
            return SalesProvider.Instance().GetShipper(shipperId);
        }

        public static IList<ShippersInfo> GetShippers(bool includeDistributor)
        {
            return SalesProvider.Instance().GetShippers(includeDistributor);
        }

        public static DataTable GetShippingAllTemplates()
        {
            return SalesProvider.Instance().GetShippingAllTemplates();
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return SalesProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static ShippingModeInfo GetShippingModeByCompany(string companyname)
        {
            return SalesProvider.Instance().GetShippingModeByCompany(companyname);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return SalesProvider.Instance().GetShippingModes(string.Empty);
        }

        public static IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
        {
            return SalesProvider.Instance().GetShippingModes(paymentGateway);
        }

        public static ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
        {
            return SalesProvider.Instance().GetShippingTemplate(templateId, includeDetail);
        }

        public static DbQueryResult GetShippingTemplates(Pagination pagin)
        {
            return SalesProvider.Instance().GetShippingTemplates(pagin);
        }

        public static AdminStatisticsInfo GetStatistics()
        {
            return SalesProvider.Instance().GetStatistics();
        }

        public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
        {
            return SalesProvider.Instance().GetUserAdd(year, month, days);
        }

        public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
        {
            return SalesProvider.Instance().GetUserOrders(userOrder);
        }

        public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
        {
            return SalesProvider.Instance().GetUserOrdersNoPage(userOrder);
        }

        public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
        {
            if (page == null)
            {
                totalProductSaleVisits = 0;
                return null;
            }
            return SalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
        }

        public static DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetWeekSaleTota(saleStatisticsType);
        }

        public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return SalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
        }

        public static bool SaveAPIPurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
        {
            bool flag = SalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrder);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.PurchaseorderMark, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单备注", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool SavePurchaseDebitNote(PurchaseDebitNote note)
        {
            return SalesProvider.Instance().SavePurchaseDebitNote(note);
        }

        public static bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.PurchaseorderMark);
            bool flag = SalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrder);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.PurchaseorderMark, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单备注", new object[] { purchaseOrder.PurchaseOrderId }));
            }
            return flag;
        }

        public static bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS))
            {
                bool flag = SalesProvider.Instance().SavePurchaseOrderShippingAddress(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的收货地址", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool SavePurchaseSendNote(SendNote note)
        {
            return SalesProvider.Instance().SavePurchaseSendNote(note);
        }

        public static bool SendAPIPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
        {
            Globals.EntityCoding(purchaseOrder, true);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
            {
                bool flag = SalesProvider.Instance().SendPurchaseOrderGoods(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.PurchaseorderSendGood, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单发货", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
        {
            Globals.EntityCoding(purchaseOrder, true);
            ManagerHelper.CheckPrivilege(Privilege.PurchaseorderSendGood);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
            {
                bool flag = SalesProvider.Instance().SendPurchaseOrderGoods(purchaseOrder);
                if (flag)
                {
                    if (purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")
                    {
                        SalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
                        SalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal(), purchaseOrder.DistributorId);
                        Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
                    }
                    EventLogs.WriteOperationLog(Privilege.PurchaseorderSendGood, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单发货", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static void SetDefalutShipper(int shipperId)
        {
            SalesProvider.Instance().SetDefalutShipper(shipperId);
        }

        public static bool SetExpressIsUse(int expressId)
        {
            return SalesProvider.Instance().SetExpressIsUse(expressId);
        }

        public static bool SetPurchaseOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            return SalesProvider.Instance().SetPurchaseOrderExpressComputerpe(orderIds, expressCompanyName, expressCompanyAbb);
        }

        public static void SetPurchaseOrderPrinted(string[] purchaseOrderIds, bool isPrinted)
        {
            int num = 0;
            for (int i = purchaseOrderIds.Length - 1; i >= 0; i--)
            {
                if (SalesProvider.Instance().SetPurchaseOrderPrinted(purchaseOrderIds[i], isPrinted))
                {
                    num++;
                }
            }
        }

        public static bool SetPurchaseOrderShipNumber(string purchaseOrderId, string startNumber)
        {
            return SalesProvider.Instance().SetPurchaseOrderShipNumber(purchaseOrderId, startNumber);
        }

        public static void SetPurchaseOrderShipNumber(string[] orderIds, string startNumber)
        {
            int num = 0;
            for (int i = 0; i < orderIds.Length; i++)
            {
                long num3 = long.Parse(startNumber) + num;
                if (SalesProvider.Instance().SetPurchaseOrderShipNumber(orderIds[i], num3.ToString()))
                {
                    num++;
                }
            }
        }

        public static bool SetPurchaseOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            return SalesProvider.Instance().SetPurchaseOrderShippingMode(orderIds, realShippingModeId, realModeName);
        }

        public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
        {
            SalesProvider.Instance().SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateExpressTemplate(int expressId, string expressName)
        {
            return SalesProvider.Instance().UpdateExpressTemplate(expressId, expressName);
        }

        public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
        {
            if (null == paymentMode)
            {
                return PaymentModeActionStatus.UnknowError;
            }
            Globals.EntityCoding(paymentMode, true);
            return SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
        }

        public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
        {
            return SalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
        }

        public static bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_AMOUNT))
            {
                bool flag = SalesProvider.Instance().UpdatePurchaseOrderAmount(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改编号为\"{0}\"的采购单的金额", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
        {
            return SalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
        }

        public static bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
        {
            ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
            if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE))
            {
                bool flag = SalesProvider.Instance().UpdatePurchaseOrderShippingMode(purchaseOrder);
                if (flag)
                {
                    EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的配送方式", new object[] { purchaseOrder.PurchaseOrderId }));
                }
                return flag;
            }
            return false;
        }

        public static bool UpdateShipper(ShippersInfo shipper)
        {
            Globals.EntityCoding(shipper, true);
            return SalesProvider.Instance().UpdateShipper(shipper);
        }

        public static bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
        {
            return SalesProvider.Instance().UpdateShippingTemplate(shippingMode);
        }

        public static bool UpdateShippMode(ShippingModeInfo shippingMode)
        {
            if (shippingMode == null)
            {
                return false;
            }
            Globals.EntityCoding(shippingMode, true);
            return SalesProvider.Instance().UpdateShippingMode(shippingMode);
        }
    }
}

