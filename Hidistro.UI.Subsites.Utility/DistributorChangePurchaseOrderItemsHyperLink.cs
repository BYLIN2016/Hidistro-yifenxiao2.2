namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DistributorChangePurchaseOrderItemsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            OrderStatus purchaseStatusCode = (OrderStatus) this.PurchaseStatusCode;
            if (purchaseStatusCode == OrderStatus.WaitBuyerPay)
            {
                base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ChangePurchaseOrderItems.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
            }
            else
            {
                base.Visible = false;
                base.Text = string.Empty;
            }
            base.Render(writer);
        }

        public object PurchaseOrderId
        {
            get
            {
                if (this.ViewState["PurchaseOrderId"] == null)
                {
                    return null;
                }
                return this.ViewState["PurchaseOrderId"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["PurchaseOrderId"] = value;
                }
            }
        }

        public object PurchaseStatusCode
        {
            get
            {
                if (this.ViewState["purchaseStatusCode"] == null)
                {
                    return null;
                }
                return this.ViewState["purchaseStatusCode"];
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["purchaseStatusCode"] = value;
                }
            }
        }
    }
}

