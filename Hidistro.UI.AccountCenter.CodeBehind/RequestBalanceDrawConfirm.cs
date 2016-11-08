namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class RequestBalanceDrawConfirm : MemberTemplatedWebControl
    {
        private IButton btnDrawConfirm;
        private FormatedMoneyLabel lblDrawBanlance;
        private Literal litAccountName;
        private Literal litBankName;
        private Literal litMerchantCode;
        private Literal litRemark;
        private Literal litUserName;

        protected override void AttachChildControls()
        {
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.lblDrawBanlance = (FormatedMoneyLabel) this.FindControl("lblDrawBanlance");
            this.litBankName = (Literal) this.FindControl("litBankName");
            this.litAccountName = (Literal) this.FindControl("litAccountName");
            this.litMerchantCode = (Literal) this.FindControl("litMerchantCode");
            this.litRemark = (Literal) this.FindControl("litRemark");
            this.btnDrawConfirm = ButtonManager.Create(this.FindControl("btnDrawConfirm"));
            PageTitle.AddSiteNameTitle("确认申请提现", HiContext.Current.Context);
            this.btnDrawConfirm.Click += new EventHandler(this.btnDrawConfirm_Click);
            if (!this.Page.IsPostBack)
            {
                BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
                this.litUserName.Text = HiContext.Current.User.Username;
                this.lblDrawBanlance.Money = balanceDrawRequest.Amount;
                this.litBankName.Text = balanceDrawRequest.BankName;
                this.litAccountName.Text = balanceDrawRequest.AccountName;
                this.litMerchantCode.Text = balanceDrawRequest.MerchantCode;
                this.litRemark.Text = balanceDrawRequest.Remark;
            }
        }

        private void btnDrawConfirm_Click(object sender, EventArgs e)
        {
            Member user = Users.GetUser(HiContext.Current.User.UserId) as Member;
            if (user.RequestBalance > 0M)
            {
                this.ShowMessage("上笔提现管理员还没有处理，只有处理完后才能再次申请提现", false);
            }
            else
            {
                BalanceDrawRequestInfo balanceDrawRequest = this.GetBalanceDrawRequest();
                if (this.ValidateBalanceDrawRequest(balanceDrawRequest))
                {
                    if (PersonalHelper.BalanceDrawRequest(balanceDrawRequest))
                    {
                        this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_MyBalanceDetails"));
                    }
                    else
                    {
                        this.ShowMessage("申请提现过程中出现未知错误", false);
                    }
                }
            }
        }

        private BalanceDrawRequestInfo GetBalanceDrawRequest()
        {
            decimal num;
            BalanceDrawRequestInfo info = new BalanceDrawRequestInfo();
            info.UserId = HiContext.Current.User.UserId;
            info.UserName = HiContext.Current.User.Username;
            info.RequestTime = DateTime.Now;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["bankName"]))
            {
                info.BankName = Globals.UrlDecode(this.Page.Request.QueryString["bankName"]);
            }
            else
            {
                info.BankName = string.Empty;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["accountName"]))
            {
                info.AccountName = Globals.UrlDecode(this.Page.Request.QueryString["accountName"]);
            }
            else
            {
                info.AccountName = string.Empty;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["merchantCode"]))
            {
                info.MerchantCode = Globals.UrlDecode(this.Page.Request.QueryString["merchantCode"]);
            }
            else
            {
                info.MerchantCode = string.Empty;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["amount"]) && decimal.TryParse(this.Page.Request.QueryString["amount"], out num))
            {
                info.Amount = num;
            }
            else
            {
                info.Amount = 0M;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["remark"]))
            {
                info.Remark = Globals.UrlDecode(this.Page.Request.QueryString["remark"]);
                return info;
            }
            info.Remark = string.Empty;
            return info;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-RequestBalanceDrawConfirm.html";
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

