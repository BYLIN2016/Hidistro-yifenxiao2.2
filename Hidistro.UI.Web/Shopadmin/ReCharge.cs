namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ReCharge : DistributorPage
    {
        protected Button btnReChargeNext;
        protected FormatedMoneyLabel lblUseableBalance;
        protected Literal litRealName;
        protected PaymentRadioButtonList radioBtnPayment;
        protected TextBox txtReChargeBalance;
        protected HtmlGenericControl txtReChargeBalanceTip;

        private void btnReChargeNext_Click(object sender, EventArgs e)
        {
            if (this.radioBtnPayment.Items.Count == 0)
            {
                this.ShowMsg("主站没有添加支付方式", false);
            }
            else if (this.radioBtnPayment.SelectedValue == null)
            {
                this.ShowMsg("请选择支付方式", false);
            }
            else
            {
                decimal num2;
                int length = 0;
                if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
                {
                    length = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
                }
                if (!decimal.TryParse(this.txtReChargeBalance.Text.Trim(), out num2) || (length > 2))
                {
                    this.ShowMsg("充值金额只能是数值，且不能超过2位小数", false);
                }
                else if ((num2 <= 0M) || (num2 > 10000000M))
                {
                    this.ShowMsg("充值金额只能是非负数值，限制在1000万以内", false);
                }
                else
                {
                    base.Response.Redirect(Globals.ApplicationPath + string.Format("/Shopadmin/store/ReChargeConfirm.aspx?modeId={0}&blance={1}", this.radioBtnPayment.SelectedValue, num2), true);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnReChargeNext.Click += new EventHandler(this.btnReChargeNext_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                this.litRealName.Text = distributor.RealName;
            }
        }
    }
}

