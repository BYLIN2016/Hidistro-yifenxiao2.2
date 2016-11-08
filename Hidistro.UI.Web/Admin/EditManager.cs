namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class EditManager : AdminPage
    {
        protected Button btnEditProfile;
        protected RoleDropDownList dropRole;
        protected FormatedTimeLabel lblLastLoginTimeValue;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected TextBox txtprivateEmail;
        private int userId;

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                SiteManager siteManager = ManagerHelper.GetManager(this.userId);
                siteManager.Email = this.txtprivateEmail.Text;
                if (this.ValidationManageEamilr(siteManager))
                {
                    foreach (string str in RoleHelper.GetUserRoleNames(siteManager.Username))
                    {
                        if (!RoleHelper.IsBuiltInRole(str) || (string.Compare(str, "SystemAdministrator") == 0))
                        {
                            RoleHelper.RemoveUserFromRole(siteManager.Username, str);
                        }
                    }
                    string text = this.dropRole.SelectedItem.Text;
                    if (string.Compare(text, "超级管理员") == 0)
                    {
                        text = "SystemAdministrator";
                    }
                    RoleHelper.AddUserToRole(siteManager.Username, text);
                    if (ManagerHelper.Update(siteManager))
                    {
                        this.ShowMsg("成功修改了当前管理员的个人资料", true);
                    }
                    else
                    {
                        this.ShowMsg("当前管理员的个人信息修改失败", false);
                    }
                }
            }
        }

        private void GetAccountInfo(SiteManager user)
        {
            this.lblLoginNameValue.Text = user.Username;
            this.lblRegsTimeValue.Time = user.CreateDate;
            this.lblLastLoginTimeValue.Time = user.LastLoginDate;
            foreach (string str in RoleHelper.GetUserRoleNames(user.Username))
            {
                if (string.Compare(str, "SystemAdministrator") == 0)
                {
                    this.dropRole.SelectedIndex = this.dropRole.Items.IndexOf(this.dropRole.Items.FindByText("超级管理员"));
                }
                if (HiContext.Current.User.UserId == this.userId)
                {
                    this.dropRole.Enabled = false;
                }
                if (!RoleHelper.IsBuiltInRole(str))
                {
                    this.dropRole.SelectedIndex = this.dropRole.Items.IndexOf(this.dropRole.Items.FindByText(str));
                    return;
                }
            }
        }

        private void GetPersonaInfo(SiteManager user)
        {
            this.txtprivateEmail.Text = user.Email;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditProfile.Click += new EventHandler(this.btnEditProfile_Click);
                if (!this.Page.IsPostBack)
                {
                    this.dropRole.DataBind();
                    SiteManager user = ManagerHelper.GetManager(this.userId);
                    if (user == null)
                    {
                        this.ShowMsg("匿名用户或非管理员用户不能编辑", false);
                    }
                    else
                    {
                        this.GetAccountInfo(user);
                        this.GetPersonaInfo(user);
                    }
                }
            }
        }

        private bool ValidationManageEamilr(SiteManager siteManager)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerEmail" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

