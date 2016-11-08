namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserDefault : MemberTemplatedWebControl
    {
        private HtmlGenericControl divBalance;
        private HtmlGenericControl divOpenBalance;
        private HyperLink hpMes;
        private HyperLink hpOrder;
        private HyperLink hpRepay;
        private FormatedMoneyLabel litAccountAmount;
        private Literal litNoPayOrderNum;
        private Literal litNoReplyLeaveWordNum;
        private FormatedMoneyLabel litRequestBalance;
        private FormatedMoneyLabel litUseableBalance;
        private Literal litUserLink;
        private Literal litUserName;
        private Literal litUserPoint;
        private Literal litUserRank;

        protected override void AttachChildControls()
        {
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.litUserPoint = (Literal) this.FindControl("litUserPoint");
            this.litUserRank = (Literal) this.FindControl("litUserRank");
            this.litUserLink = (Literal) this.FindControl("litUserLink");
            this.litNoPayOrderNum = (Literal) this.FindControl("litNoPayOrderNum");
            this.litNoReplyLeaveWordNum = (Literal) this.FindControl("litNoReplyLeaveWordNum");
            this.litAccountAmount = (FormatedMoneyLabel) this.FindControl("litAccountAmount");
            this.litRequestBalance = (FormatedMoneyLabel) this.FindControl("litRequestBalance");
            this.litUseableBalance = (FormatedMoneyLabel) this.FindControl("litUseableBalance");
            this.hpOrder = (HyperLink) this.FindControl("hpOrder");
            this.hpMes = (HyperLink) this.FindControl("hpMes");
            this.hpRepay = (HyperLink) this.FindControl("hpRepaly");
            this.divBalance = (HtmlGenericControl) this.FindControl("divBalance");
            this.divOpenBalance = (HtmlGenericControl) this.FindControl("divOpenBalance");
            PageTitle.AddSiteNameTitle("会员中心首页", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.divBalance.Visible = false;
                    this.divOpenBalance.Visible = true;
                }
                this.litUserPoint.Text = user.Points.ToString();
                this.litUserName.Text = user.Username;
                MemberGradeInfo memberGrade = PersonalHelper.GetMemberGrade(user.GradeId);
                if (memberGrade != null)
                {
                    this.litUserRank.Text = memberGrade.Name;
                }
                int noPayOrderNum = 0;
                int noReadMessageNum = 0;
                int noReplyLeaveCommentNum = 0;
                PersonalHelper.GetStatisticsNum(out noPayOrderNum, out noReadMessageNum, out noReplyLeaveCommentNum);
                this.litNoPayOrderNum.Text = noPayOrderNum.ToString();
                this.litNoReplyLeaveWordNum.Text = noReplyLeaveCommentNum.ToString();
                this.hpMes.Text = noReadMessageNum.ToString();
                this.litAccountAmount.Money = user.Balance;
                this.litRequestBalance.Money = user.RequestBalance;
                this.litUseableBalance.Money = user.Balance - user.RequestBalance;
                if (noPayOrderNum > 0)
                {
                    this.hpOrder.Visible = true;
                    this.hpOrder.NavigateUrl = "UserOrders.aspx?orderStatus=" + 1;
                }
                this.hpMes.NavigateUrl = "UserReceivedMessages.aspx";
                if (noReplyLeaveCommentNum > 0)
                {
                    this.hpRepay.Visible = true;
                    this.hpRepay.NavigateUrl = "UserConsultations.aspx";
                }
                Uri url = HttpContext.Current.Request.Url;
                string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
                this.litUserLink.Text = string.Concat(new object[] { string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, HiContext.Current.SiteSettings.SiteUrl, str }), Globals.ApplicationPath, "/?ReferralUserId=", HiContext.Current.User.UserId });
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserDefault.html";
            }
            base.OnInit(e);
        }
    }
}

