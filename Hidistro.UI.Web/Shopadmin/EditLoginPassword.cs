namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditLoginPassword : DistributorPage
    {
        protected Button btnEditLoginPassword;
        protected TextBox txtNewPassword;
        protected HtmlGenericControl txtNewPasswordTip;
        protected TextBox txtOldPassword;
        protected HtmlGenericControl txtOldPasswordTip;
        protected TextBox txtPasswordCompare;
        protected HtmlGenericControl txtPasswordCompareTip;

        private void btnEditLoginPassword_Click(object sender, EventArgs e)
        {
            Distributor distributor = SubsiteStoreHelper.GetDistributor();
            if (string.IsNullOrEmpty(this.txtOldPassword.Text))
            {
                this.ShowMsg("旧登录密码不能为空", false);
            }
            else if ((string.IsNullOrEmpty(this.txtNewPassword.Text) || (this.txtNewPassword.Text.Length > 20)) || (this.txtNewPassword.Text.Length < 6))
            {
                this.ShowMsg("新登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (this.txtNewPassword.Text != this.txtPasswordCompare.Text)
            {
                this.ShowMsg("两次输入的密码不一致", false);
            }
            else if (distributor.ChangePassword(this.txtOldPassword.Text, this.txtNewPassword.Text))
            {
                distributor.OnPasswordChanged(new UserEventArgs(distributor.Username, this.txtNewPassword.Text, null));
                this.ShowMsg("登录密码修改成功", true);
            }
            else
            {
                this.ShowMsg("登录密码修改失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnEditLoginPassword.Click += new EventHandler(this.btnEditLoginPassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

