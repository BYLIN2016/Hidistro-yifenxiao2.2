namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class ChoosePayment : DistributorPage
    {
        protected Button btnSubmit;
        protected DistributorPaymentRadioButtonList radioPaymentMode;

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string purchaseOrderId = base.Request["PurchaseOrderId"];
            PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(int.Parse(this.radioPaymentMode.SelectedValue));
            if (paymentMode != null)
            {
                SubsiteSalesHelper.SetPayment(purchaseOrderId, paymentMode.ModeId, paymentMode.Name, paymentMode.Gateway);
            }
            if ((paymentMode != null) && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.podrequest"))
            {
                this.ShowMsg("您选择的是货到付款方式，请等待主站发货", true);
            }
            else if ((paymentMode != null) && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.bankrequest"))
            {
                this.ShowMsg("您选择的是线下付款方式，请与主站管理员联系", true);
            }
            else
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=" + purchaseOrderId + "&PayMode=" + this.radioPaymentMode.SelectedValue);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            int.TryParse(base.Request["PayMode"], out num);
            if (!this.Page.IsPostBack)
            {
                this.radioPaymentMode.DataBind();
                this.radioPaymentMode.SelectedValue = num.ToString();
            }
        }
    }
}

