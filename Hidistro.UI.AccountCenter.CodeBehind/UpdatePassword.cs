namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UpdatePassword : MemberTemplatedWebControl
    {
        private IButton btnChangePassword;
        private HtmlGenericControl LkUpdateTradePassword;
        private SmallStatusMessage status;
        private TextBox txtNewPassword;
        private TextBox txtNewPassword2;
        private TextBox txtOdlPassword;

        protected override void AttachChildControls()
        {
            this.status = (SmallStatusMessage) this.FindControl("status");
            this.txtOdlPassword = (TextBox) this.FindControl("txtOdlPassword");
            this.txtNewPassword = (TextBox) this.FindControl("txtNewPassword");
            this.txtNewPassword2 = (TextBox) this.FindControl("txtNewPassword2");
            this.btnChangePassword = ButtonManager.Create(this.FindControl("btnChangePassword"));
            this.LkUpdateTradePassword = (HtmlGenericControl) this.FindControl("one2");
            PageTitle.AddSiteNameTitle("修改登录密码", HiContext.Current.Context);
            this.btnChangePassword.Click += new EventHandler(this.btnChangePassword_Click);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.LkUpdateTradePassword.Visible = false;
                }
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Member user = HiContext.Current.User as Member;
            if ((user.MembershipUser == null) || !user.MembershipUser.IsLockedOut)
            {
                user.Password = this.txtOdlPassword.Text;
                if (user.ChangePassword(this.txtOdlPassword.Text, this.txtNewPassword2.Text))
                {
                    Messenger.UserPasswordChanged(user, this.txtNewPassword2.Text);
                    user.OnPasswordChanged(new UserEventArgs(user.Username, this.txtNewPassword2.Text, null));
                    this.ShowMessage("你已经成功的修改了登录密码", true);
                }
                else
                {
                    this.ShowMessage("当前登录密码输入错误", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UpdatePassword.html";
            }
            base.OnInit(e);
        }
    }
}

