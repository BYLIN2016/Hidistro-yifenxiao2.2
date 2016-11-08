namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditTradePassword : DistributorPage
    {
        protected Button btnEditTradePassword;
        protected TextBox txtNewTradePassword;
        protected HtmlGenericControl txtNewTradePasswordTip;
        protected TextBox txtOldTradePassword;
        protected HtmlGenericControl txtOldTradePasswordTip;
        protected TextBox txtTradePasswordCompare;
        protected HtmlGenericControl txtTradePasswordCompareTip;

        private void btnEditTradePassword_Click(object sender, EventArgs e)
        {
            Distributor distributor = SubsiteStoreHelper.GetDistributor();
            if (string.IsNullOrEmpty(this.txtOldTradePassword.Text))
            {
                this.ShowMsg("请输入旧交易密码", false);
            }
            else if ((string.IsNullOrEmpty(this.txtNewTradePassword.Text) || (this.txtNewTradePassword.Text.Length > 20)) || (this.txtNewTradePassword.Text.Length < 6))
            {
                this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (this.txtNewTradePassword.Text != this.txtTradePasswordCompare.Text)
            {
                this.ShowMsg("输入的两次密码不一致", false);
            }
            else if (distributor.ChangeTradePassword(this.txtOldTradePassword.Text, this.txtNewTradePassword.Text))
            {
                distributor.OnDealPasswordChanged(new UserEventArgs(distributor.Username, null, this.txtNewTradePassword.Text));
                this.ShowMsg("交易密码修改成功", true);
            }
            else
            {
                this.ShowMsg("交易密码修改失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnEditTradePassword.Click += new EventHandler(this.btnEditTradePassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

