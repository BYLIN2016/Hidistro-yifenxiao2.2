namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class RequestBalanceDraw : MemberTemplatedWebControl
    {
        private IButton btnDrawNext;
        private FormatedMoneyLabel lblBanlance;
        private Literal litUserName;
        private TextBox txtAccountName;
        private TextBox txtAmount;
        private TextBox txtBankName;
        private TextBox txtMerchantCode;
        private TextBox txtRemark;
        private TextBox txtTradePassword;

        protected override void AttachChildControls()
        {
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.lblBanlance = (FormatedMoneyLabel) this.FindControl("lblBanlance");
            this.txtAmount = (TextBox) this.FindControl("txtAmount");
            this.txtBankName = (TextBox) this.FindControl("txtBankName");
            this.txtAccountName = (TextBox) this.FindControl("txtAccountName");
            this.txtMerchantCode = (TextBox) this.FindControl("txtMerchantCode");
            this.txtRemark = (TextBox) this.FindControl("txtRemark");
            this.txtTradePassword = (TextBox) this.FindControl("txtTradePassword");
            this.btnDrawNext = ButtonManager.Create(this.FindControl("btnDrawNext"));
            this.btnDrawNext.Click += new EventHandler(this.btnDrawNext_Click);
            PageTitle.AddSiteNameTitle("申请提现", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
                this.litUserName.Text = HiContext.Current.User.Username;
                this.lblBanlance.Money = user.Balance - user.RequestBalance;
            }
        }

        private void btnDrawNext_Click(object sender, EventArgs e)
        {
            Member user = HiContext.Current.User as Member;
            if (user.RequestBalance > 0M)
            {
                this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false);
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(this.txtAmount.Text.Trim(), out result))
                {
                    this.ShowMessage("提现金额输入错误,请重新输入提现金额", false);
                }
                else if (result > ((decimal) this.lblBanlance.Money))
                {
                    this.ShowMessage("预付款余额不足,请重新输入提现金额", false);
                }
                else if (string.IsNullOrEmpty(this.txtTradePassword.Text))
                {
                    this.ShowMessage("请输入交易密码", false);
                }
                else
                {
                    user.TradePassword = this.txtTradePassword.Text;
                    if (!Users.ValidTradePassword(user))
                    {
                        this.ShowMessage("交易密码不正确,请重新输入", false);
                    }
                    else
                    {
                        BalanceDrawRequestInfo balanceDrawRequest = new BalanceDrawRequestInfo();
                        balanceDrawRequest.BankName = this.txtBankName.Text.Trim();
                        balanceDrawRequest.AccountName = this.txtAccountName.Text.Trim();
                        balanceDrawRequest.MerchantCode = this.txtMerchantCode.Text.Trim();
                        balanceDrawRequest.Amount = result;
                        balanceDrawRequest.Remark = this.txtRemark.Text.Trim();
                        if (this.ValidateBalanceDrawRequest(balanceDrawRequest))
                        {
                            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_RequestBalanceDrawConfirm", new object[] { Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequest.BankName)), Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequest.AccountName)), Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequest.MerchantCode)), balanceDrawRequest.Amount, Globals.UrlEncode(Globals.HtmlEncode(balanceDrawRequest.Remark)) }));
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-RequestBalanceDraw.html";
            }
            base.OnInit(e);
        }

        private bool ValidateBalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDrawRequestInfo>(balanceDrawRequest, new string[] { "ValBalanceDrawRequestInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMessage(msg, false);
            }
            return results.IsValid;
        }
    }
}

