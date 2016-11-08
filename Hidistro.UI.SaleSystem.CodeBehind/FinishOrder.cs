namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class FinishOrder : HtmlTemplatedWebControl
    {
        private Button btnSubMitOrder;
        private Literal litOrderId;
        private FormatedMoneyLabel litOrderPrice;
        private Literal litPaymentName;
        private string orderId;

        protected override void AttachChildControls()
        {
            if (this.Page.Request.QueryString["orderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.litOrderPrice = (FormatedMoneyLabel) this.FindControl("litOrderPrice");
            this.litPaymentName = (Literal) this.FindControl("litPaymentName");
            this.btnSubMitOrder = (Button) this.FindControl("btnSubMitOrder");
            this.btnSubMitOrder.Click += new EventHandler(this.btnSubMitOrder_Click);
            if (!this.Page.IsPostBack)
            {
                this.LoadOrderInfo();
            }
        }

        private void btnSubMitOrder_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo != null)
            {
                if (orderInfo.Gateway != "hishop.plugins.payment.advancerequest")
                {
                    HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[] { this.orderId }));
                }
                else
                {
                    HttpContext.Current.Response.Redirect(string.Format("/user/pay.aspx?OrderId={0}", this.orderId));
                }
            }
        }

        public void LoadOrderInfo()
        {
            OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
            if (orderInfo != null)
            {
                this.litOrderId.Text = orderInfo.OrderId;
                this.litOrderPrice.Money = orderInfo.GetTotal();
                this.litPaymentName.Text = orderInfo.PaymentType;
                if (orderInfo.Gateway == "hishop.plugins.payment.podrequest")
                {
                    this.btnSubMitOrder.Visible = false;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-FinishOrder.html";
            }
            base.OnInit(e);
        }
    }
}

