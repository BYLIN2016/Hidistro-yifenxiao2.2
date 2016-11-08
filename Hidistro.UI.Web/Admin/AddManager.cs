namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class AddManager : AdminPage
    {
        protected Button btnCreate;
        protected RoleDropDownList dropRole;
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected TextBox txtPasswordagain;
        protected TextBox txtUserName;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateUserStatus unknownFailure = CreateUserStatus.UnknownFailure;
            SiteManager siteManager = new SiteManager();
            siteManager.IsApproved = true;
            siteManager.Username = this.txtUserName.Text.Trim();
            siteManager.Email = this.txtEmail.Text.Trim();
            siteManager.Password = this.txtPassword.Text.Trim();
            siteManager.PasswordFormat = MembershipPasswordFormat.Hashed;
            if (string.Compare(this.txtPassword.Text, this.txtPasswordagain.Text) != 0)
            {
                this.ShowMsg("请确保两次输入的密码相同", false);
            }
            else if (this.ValidationAddManager(siteManager))
            {
                try
                {
                    string text = this.dropRole.SelectedItem.Text;
                    if (string.Compare(text, "超级管理员") == 0)
                    {
                        text = "SystemAdministrator";
                    }
                    unknownFailure = ManagerHelper.Create(siteManager, text);
                }
                catch (CreateUserException exception)
                {
                    unknownFailure = exception.CreateUserStatus;
                }
                switch (unknownFailure)
                {
                    case CreateUserStatus.UnknownFailure:
                        this.ShowMsg("未知错误", false);
                        return;

                    case CreateUserStatus.Created:
                        this.txtEmail.Text = string.Empty;
                        this.txtUserName.Text = string.Empty;
                        this.ShowMsg("成功添加了一个管理员", true);
                        return;

                    case CreateUserStatus.DuplicateUsername:
                        this.ShowMsg("您输入的用户名已经被注册使用", false);
                        return;

                    case CreateUserStatus.DuplicateEmailAddress:
                        this.ShowMsg("您输入的电子邮件地址已经被注册使用", false);
                        return;

                    case CreateUserStatus.InvalidFirstCharacter:
                    case CreateUserStatus.Updated:
                    case CreateUserStatus.Deleted:
                    case CreateUserStatus.InvalidQuestionAnswer:
                        return;

                    case CreateUserStatus.DisallowedUsername:
                        this.ShowMsg("用户名被禁止注册", false);
                        return;

                    case CreateUserStatus.InvalidPassword:
                        this.ShowMsg("无效的密码", false);
                        return;

                    case CreateUserStatus.InvalidEmail:
                        this.ShowMsg("无效的电子邮件地址", false);
                        return;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRole.DataBind();
            }
        }

        private bool ValidationAddManager(SiteManager siteManager)
        {
            bool flag = true;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerName" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                flag = false;
            }
            results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerPassword" });
            if (!results.IsValid)
            {
                foreach (ValidationResult result2 in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result2.Message);
                }
                flag = false;
            }
            results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerEmail" });
            if (!results.IsValid)
            {
                foreach (ValidationResult result3 in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result3.Message);
                }
                flag = false;
            }
            if (!flag)
            {
                this.ShowMsg(msg, false);
            }
            return flag;
        }
    }
}

