namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class EditManagerPassword : AdminPage
    {
        protected Button btnEditPassWordOK;
        protected Literal lblLoginNameValue;
        protected HtmlGenericControl panelOld;
        protected TextBox txtNewPassWord;
        protected TextBox txtOldPassWord;
        protected TextBox txtPassWordCompare;
        private int userId;

        private void btnEditPassWordOK_Click(object sender, EventArgs e)
        {
            SiteManager manager = ManagerHelper.GetManager(this.userId);
            if ((string.IsNullOrEmpty(this.txtNewPassWord.Text) || (this.txtNewPassWord.Text.Length > 20)) || (this.txtNewPassWord.Text.Length < 6))
            {
                this.ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (string.Compare(this.txtNewPassWord.Text, this.txtPassWordCompare.Text) != 0)
            {
                this.ShowMsg("两次输入的密码不一样", false);
            }
            else
            {
                HiConfiguration config = HiConfiguration.GetConfig();
                if ((string.IsNullOrEmpty(this.txtNewPassWord.Text) || (this.txtNewPassWord.Text.Length < Membership.Provider.MinRequiredPasswordLength)) || (this.txtNewPassWord.Text.Length > config.PasswordMaxLength))
                {
                    this.ShowMsg(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength), false);
                }
                else if (this.userId == HiContext.Current.User.UserId)
                {
                    if (manager.ChangePassword(this.txtOldPassWord.Text, this.txtNewPassWord.Text))
                    {
                        this.ShowMsg("密码修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
                    }
                }
                else
                {
                    HttpContext context = HiContext.Current.Context;
                    if (manager.ChangePassword(this.txtNewPassWord.Text))
                    {
                        this.ShowMsg("密码修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
                    }
                }
            }
        }

        private void GetSecurity()
        {
            if (HiContext.Current.User.UserId != this.userId)
            {
                this.panelOld.Visible = false;
            }
            else
            {
                this.panelOld.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditPassWordOK.Click += new EventHandler(this.btnEditPassWordOK_Click);
                if (!this.Page.IsPostBack)
                {
                    SiteManager manager = ManagerHelper.GetManager(this.userId);
                    if (manager == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.lblLoginNameValue.Text = manager.Username;
                        this.GetSecurity();
                    }
                }
            }
        }
    }
}

