namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class Default : HtmlTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            HiContext current = HiContext.Current;
            PageTitle.AddTitle(current.SiteSettings.SiteName + " - " + current.SiteSettings.SiteDescription, HiContext.Current.Context);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Default.html";
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.Params["OrderId"]))
            {
                this.SearchOrder();
            }
            base.OnLoad(e);
        }

        private void SearchOrder()
        {
            string s = "[{";
            string orderId = this.Page.Request["OrderId"];
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                string str3 = s;
                s = str3 + "\"OrderId\":\"" + orderInfo.OrderId + "\",\"ShippingStatus\":\"" + OrderInfo.GetOrderStatusName(orderInfo.OrderStatus) + "\",\"ShipOrderNumber\":\"" + orderInfo.ShipOrderNumber + "\",\"ShipModeName\":\"" + orderInfo.RealModeName + "\"";
            }
            s = s + "}]";
            this.Page.Response.ContentType = "text/plain";
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }
    }
}

