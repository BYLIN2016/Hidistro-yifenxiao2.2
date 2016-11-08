namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Bank : MemberTemplatedWebControl
    {
        private Label lblDescription;
        private Label lblPaymentName;
        private string orderId;

        protected override void AttachChildControls()
        {
            this.lblPaymentName = (Label) this.FindControl("lblPaymentName");
            this.lblDescription = (Label) this.FindControl("lblDescription");
            this.orderId = this.Page.Request.QueryString["orderId"];
            PageTitle.AddSiteNameTitle("订单线下支付", HiContext.Current.Context);
            if (string.IsNullOrEmpty(this.orderId))
            {
                base.GotoResourceNotFound();
            }
            if (!this.Page.IsPostBack)
            {
                PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(TradeHelper.GetOrderInfo(this.orderId).PaymentTypeId);
                if (paymentMode != null)
                {
                    this.lblPaymentName.Text = paymentMode.Name;
                    this.lblDescription.Text = paymentMode.Description;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-Bank.html";
            }
            base.OnInit(e);
        }
    }
}

