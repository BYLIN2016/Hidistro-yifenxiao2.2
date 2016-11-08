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
    public class EditDistributorTradePassword : AdminPage
    {
        protected Button btnEditDistributorTradePassword;
        protected Literal litUserName;
        protected TextBox txtNewTradePassword;
        protected HtmlGenericControl txtNewTradePasswordTip;
        protected TextBox txtTradePasswordCompare;
        protected HtmlGenericControl txtTradePasswordCompareTip;
        private int userId;
        protected Hidistro.UI.Common.Controls.WangWangConversations WangWangConversations;

        private void btnEditDistributorTradePassword_Click(object sender, EventArgs e)
        {
            Distributor user = DistributorHelper.GetDistributor(this.userId);
            if ((string.IsNullOrEmpty(this.txtNewTradePassword.Text) || (this.txtNewTradePassword.Text.Length > 20)) || (this.txtNewTradePassword.Text.Length < 6))
            {
                this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (this.txtNewTradePassword.Text != this.txtTradePasswordCompare.Text)
            {
                this.ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangeTradePassword(this.txtNewTradePassword.Text))
            {
                Messenger.UserDealPasswordChanged(user, this.txtNewTradePassword.Text);
                user.OnDealPasswordChanged(new UserEventArgs(user.Username, null, this.txtNewTradePassword.Text));
                this.ShowMsg("交易密码修改成功", true);
            }
            else
            {
                this.ShowMsg("交易密码修改失败", false);
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
            this.btnEditDistributorTradePassword.Click += new EventHandler(this.btnEditDistributorTradePassword_Click);
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

