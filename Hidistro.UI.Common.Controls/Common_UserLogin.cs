namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Web.UI.WebControls;

    public class Common_UserLogin : AscxTemplatedWebControl
    {
        private Literal litAccount;
        private Literal litMemberGrade;
        private Literal litNum;
        private Literal litPoint;
        private Panel pnlLogin;
        private Panel pnlLogout;

        protected override void AttachChildControls()
        {
            this.pnlLogin = (Panel) this.FindControl("pnlLogin");
            this.pnlLogout = (Panel) this.FindControl("pnlLogout");
            this.litAccount = (Literal) this.FindControl("litAccount");
            this.litMemberGrade = (Literal) this.FindControl("litMemberGrade");
            this.litPoint = (Literal) this.FindControl("litPoint");
            this.litNum = (Literal) this.FindControl("litNum");
            this.pnlLogout.Visible = !HiContext.Current.User.IsAnonymous;
            this.pnlLogin.Visible = HiContext.Current.User.IsAnonymous;
            if (!this.Page.IsPostBack && ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)))
            {
                string str;
                int num;
                Member user = HiContext.Current.User as Member;
                this.litAccount.Text = Globals.FormatMoney(user.Balance);
                this.litPoint.Text = user.Points.ToString();
                ControlProvider.Instance().GetMemberExpandInfo(user.GradeId, user.Username, out str, out num);
                this.litMemberGrade.Text = str;
                this.litNum.Text = num.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Skin-Common_UserLogin.ascx";
            }
            base.OnInit(e);
        }
    }
}

