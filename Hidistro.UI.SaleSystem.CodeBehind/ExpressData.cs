namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Shopping;
    using System;
    using System.Web;

    public class ExpressData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.SearchExpressData(context);
            }
            catch
            {
            }
        }

        private void SearchExpressData(HttpContext context)
        {
            string s = "{";
            if (!string.IsNullOrEmpty(context.Request["OrderId"]))
            {
                string orderId = context.Request["OrderId"];
                OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
                if (((orderInfo != null) && ((orderInfo.OrderStatus == OrderStatus.SellerAlreadySent) || (orderInfo.OrderStatus == OrderStatus.Finished))) && !string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb))
                {
                    string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                    s = s + "\"Express\":\"" + expressData + "\"";
                }
            }
            else if (!string.IsNullOrEmpty(context.Request["PurchaseOrderId"]))
            {
                string purchaseOrderId = context.Request["PurchaseOrderId"];
                PurchaseOrderInfo purchaseOrder = ShoppingProcessor.GetPurchaseOrder(purchaseOrderId);
                if (((purchaseOrder != null) && ((purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent) || (purchaseOrder.PurchaseStatus == OrderStatus.Finished))) && !string.IsNullOrEmpty(purchaseOrder.ExpressCompanyAbb))
                {
                    string str5 = Express.GetExpressData(purchaseOrder.ExpressCompanyAbb, purchaseOrder.ShipOrderNumber);
                    s = s + "\"Express\":\"" + str5 + "\"";
                }
            }
            s = s + "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(s);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

