namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BalanceDraw : DistributorPage
    {
        protected Button btnDrawNext;
        protected FormatedMoneyLabel lblUseableBalance;
        protected Literal litRealName;
        protected TextBox txtAccountName;
        protected HtmlGenericControl txtAccountNameTip;
        protected TextBox txtBankName;
        protected TextBox txtDrawBalance;
        protected HtmlGenericControl txtDrawBalanceTip;
        protected TextBox txtMerchantCode;
        protected TextBox txtMerchantCodeCompare;
        protected HtmlGenericControl txtMerchantCodeCompareTip;
        protected HtmlGenericControl txtMerchantCodeTip;
        protected TextBox txtTradePassword;
        protected HtmlGenericControl txtTradePasswordTip;

        private void btnDrawNext_Click(object sender, EventArgs e)
        {
            if (SubsiteStoreHelper.DistroHasDrawRequest())
            {
                this.ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
            }
            else
            {
                decimal num;
                if (!decimal.TryParse(this.txtDrawBalance.Text.Trim(), out num))
                {
                    this.ShowMsg(" 提现金额只能是数值，限制在1000万以内", false);
                }
                else if (string.Compare(this.txtMerchantCodeCompare.Text.Trim(), this.txtMerchantCode.Text.Trim()) != 0)
                {
                    this.ShowMsg(" 两次输入的帐号不一致,请重新输入", false);
                }
                else if (num > ((decimal) this.lblUseableBalance.Money))
                {
                    this.ShowMsg(" 您的可用金额不足", false);
                }
                else if (string.IsNullOrEmpty(this.txtTradePassword.Text))
                {
                    this.ShowMsg("请输入交易密码", false);
                }
                else
                {
                    Distributor user = SubsiteStoreHelper.GetDistributor();
                    BalanceDrawRequestInfo target = new BalanceDrawRequestInfo();
                    target.UserId = user.UserId;
                    target.UserName = user.Username;
                    target.RequestTime = DateTime.Now;
                    target.MerchantCode = this.txtMerchantCode.Text.Trim();
                    target.BankName = this.txtBankName.Text.Trim();
                    target.Amount = num;
                    target.AccountName = this.txtAccountName.Text.Trim();
                    target.Remark = string.Empty;
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDrawRequestInfo>(target, new string[] { "ValBalanceDrawRequestInfo" });
                    string msg = string.Empty;
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                        }
                        this.ShowMsg(msg, false);
                    }
                    else
                    {
                        user.TradePassword = this.txtTradePassword.Text;
                        if (Users.ValidTradePassword(user))
                        {
                            this.Session["BalanceDrawRequest"] = target;
                            base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ConfirmBalanceDrawRequest.aspx", true);
                        }
                        else
                        {
                            this.ShowMsg("交易密码不正确,请重新输入", false);
                        }
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnDrawNext.Click += new EventHandler(this.btnDrawNext_Click);
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

