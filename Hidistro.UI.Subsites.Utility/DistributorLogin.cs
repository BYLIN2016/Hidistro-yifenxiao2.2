namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    public class DistributorLogin : HtmlTemplatedWebControl
    {
        private Button btnLogin;
        private TextBox txtCode;
        private TextBox txtPassword;
        private TextBox txtUserName;
        private string verifyCodeKey = "VerifyCode";

        protected override void AttachChildControls()
        {
            HiContext current = HiContext.Current;
            if (this.Context.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
                IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
                if (userCookie != null)
                {
                    userCookie.DeleteCookie(authCookie);
                }
                RoleHelper.SignOut(HiContext.Current.User.Username);
                this.Page.Response.Cookies["hishopLoginStatus"].Value = "";
            }
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.btnLogin = (Button) this.FindControl("btnLogin");
            this.txtCode = (TextBox) this.FindControl("txtCode");
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim()))
            {
                this.ShowMessage("验证码不正确", false);
            }
            else
            {
                IUser user = Users.GetUser(0, this.txtUserName.Text, false, true);
                if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
                {
                    this.ShowMessage("无效的用户信息", false);
                }
                else
                {
                    Distributor distributor = user as Distributor;
                    distributor.Password = this.txtPassword.Text;
                    if (HiContext.Current.SiteSettings.IsDistributorSettings && (user.UserId != HiContext.Current.SiteSettings.UserId.Value))
                    {
                        this.ShowMessage("分销商只能在自己的站点或主站上登录", false);
                    }
                    else
                    {
                        LoginUserStatus status = SubsiteStoreHelper.ValidLogin(distributor);
                        if (status == LoginUserStatus.Success)
                        {
                            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(distributor.Username, false);
                            distributor.GetUserCookie().WriteCookie(authCookie, 30, false);
                            this.Page.Response.Cookies["hishopLoginStatus"].Value = "true";
                            HiContext.Current.User = distributor;
                            distributor.OnLogin();
                            if (SettingsManager.GetSiteSettings(HiContext.Current.User.UserId) == null)
                            {
                                this.Page.Response.Redirect("nositedefault.aspx", true);
                            }
                            else
                            {
                                this.Page.Response.Redirect("default.aspx", true);
                            }
                        }
                        else
                        {
                            switch (status)
                            {
                                case LoginUserStatus.AccountPending:
                                    this.ShowMessage("用户账号还没有通过审核", false);
                                    return;

                                case LoginUserStatus.AccountLockedOut:
                                    this.ShowMessage("用户账号已被锁定，暂时不能登录系统", false);
                                    return;

                                case LoginUserStatus.InvalidCredentials:
                                    this.ShowMessage("用户名或密码错误", false);
                                    return;
                            }
                            this.ShowMessage("登录失败，未知错误", false);
                        }
                    }
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (HttpContext.Current.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorLogin.html";
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["isCallback"]) && (HttpContext.Current.Request["isCallback"] == "true"))
            {
                string verifyCode = HttpContext.Current.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write("{ ");
                HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                HttpContext.Current.Response.Write("}");
                HttpContext.Current.Response.End();
            }
            base.OnInit(e);
        }

        private void ShowMessage(string msg)
        {
        }
    }
}

