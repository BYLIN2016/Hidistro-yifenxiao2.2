namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;

    public class MyAccountSummary : MemberTemplatedWebControl
    {
        private FormatedMoneyLabel litAccountAmount;
        private FormatedMoneyLabel litRequestBalance;
        private FormatedMoneyLabel litUseableBalance;

        protected override void AttachChildControls()
        {
            this.litAccountAmount = (FormatedMoneyLabel) this.FindControl("litAccountAmount");
            this.litRequestBalance = (FormatedMoneyLabel) this.FindControl("litRequestBalance");
            this.litUseableBalance = (FormatedMoneyLabel) this.FindControl("litUseableBalance");
            PageTitle.AddSiteNameTitle("预付款账户", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
                this.litAccountAmount.Money = user.Balance;
                this.litRequestBalance.Money = user.RequestBalance;
                this.litUseableBalance.Money = user.Balance - user.RequestBalance;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-MyAccountSummary.html";
            }
            base.OnInit(e);
        }
    }
}

