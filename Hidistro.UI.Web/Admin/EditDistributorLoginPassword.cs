namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditDistributor)]
    public class EditDistributorLoginPassword : AdminPage
    {
        protected Button btnEditDistributorLoginPassword;
        protected Literal litUserName;
        protected TextBox txtNewPassword;
        protected HtmlGenericControl txtNewPasswordTip;
        protected TextBox txtPasswordCompare;
        protected HtmlGenericControl txtPasswordCompareTip;
        private int userId;
        protected Hidistro.UI.Common.Controls.WangWangConversations WangWangConversations;

        private void btnEditDistributorLoginPassword_Click(object sender, EventArgs e)
        {
            Distributor user = DistributorHelper.GetDistributor(this.userId);
            if ((string.IsNullOrEmpty(this.txtNewPassword.Text) || (this.txtNewPassword.Text.Length > 20)) || (this.txtNewPassword.Text.Length < 6))
            {
                this.ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (this.txtNewPassword.Text != this.txtPasswordCompare.Text)
            {
                this.ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangePassword(this.txtNewPassword.Text))
            {
                Messenger.UserPasswordChanged(user, this.txtNewPassword.Text);
                user.OnPasswordChanged(new UserEventArgs(user.Username, this.txtNewPassword.Text, null));
                this.ShowMsg("登录密码修改成功", true);
            }
            else
            {
                this.ShowMsg("登录密码修改失败", false);
            }
        }

        private void LoadControl()
        {
            Distributor distributor = DistributorHelper.GetDistributor(this.userId);
            if (distributor == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.litUserName.Text = distributor.Username;
                this.WangWangConversations.WangWangAccounts = distributor.Wangwang;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnEditDistributorLoginPassword.Click += new EventHandler(this.btnEditDistributorLoginPassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                this.LoadControl();
            }
        }
    }
}

