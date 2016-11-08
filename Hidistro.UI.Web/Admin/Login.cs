namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Login : Page
    {
        protected Button btnAdminLogin;
        protected HtmlForm form1;
        protected HeadContainer HeadContainer1;
        protected SmallStatusMessage lblStatus;
        private readonly string licenseMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的易分销系统未经官方授权，无法登录后台管理。请联系易分销官方(www.shopefx.com)购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 海商网络技术有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        private readonly string noticeMsg = ("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的易分销系统已过授权有效期，无法登录后台管理。请续费。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 海商网络技术有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>");
        protected PageTitle PageTitle1;
        protected Panel Panel1;
        protected TextBox txtAdminName;
        protected TextBox txtAdminPassWord;
        protected TextBox txtCode;
        private string verifyCodeKey = "VerifyCode";

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (!HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim()))
            {
                this.ShowMessage("验证码不正确");
            }
            else
            {
                IUser user = Users.GetUser(0, this.txtAdminName.Text, false, true);
                if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.SiteManager))
                {
                    this.ShowMessage("无效的用户信息");
                }
                else
                {
                    string referralLink = null;
                    SiteManager manager = user as SiteManager;
                    manager.Password = this.txtAdminPassWord.Text;
                    LoginUserStatus status = ManagerHelper.ValidLogin(manager);
                    if (status == LoginUserStatus.Success)
                    {
                        HttpCookie authCookie = FormsAuthentication.GetAuthCookie(manager.Username, false);
                        manager.GetUserCookie().WriteCookie(authCookie, 30, false);
                        HttpCookie cookie = new HttpCookie("Admin-system");
                        cookie.Value = manager.Username;
                        cookie.Expires = DateTime.Now.AddMinutes(30.0);
                        HttpContext.Current.Response.Cookies.Add(cookie);
                        HiContext.Current.User = manager;
                        if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
                        {
                            referralLink = this.Page.Request.QueryString["returnUrl"];
                        }
                        if (((referralLink == null) && (this.ReferralLink != null)) && !string.IsNullOrEmpty(this.ReferralLink.Trim()))
                        {
                            referralLink = this.ReferralLink;
                        }
                        if (!string.IsNullOrEmpty(referralLink) && (((referralLink.ToLower().IndexOf(Globals.GetSiteUrls().Logout.ToLower()) >= 0) || (referralLink.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("register").ToLower()) >= 0)) || ((referralLink.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("vote").ToLower()) >= 0) || (referralLink.ToLower().IndexOf("loginexit") >= 0))))
                        {
                            referralLink = null;
                        }
                        if (referralLink != null)
                        {
                            this.Page.Response.Redirect(referralLink, true);
                        }
                        else
                        {
                            this.Page.Response.Redirect("default.html", true);
                        }
                    }
                    else
                    {
                        switch (status)
                        {
                            case LoginUserStatus.AccountPending:
                                this.ShowMessage("用户账号还没有通过审核");
                                return;

                            case LoginUserStatus.AccountLockedOut:
                                this.ShowMessage("用户账号已被锁定，暂时不能登录系统");
                                return;

                            case LoginUserStatus.InvalidCredentials:
                                this.ShowMessage("用户名或密码错误");
                                return;
                        }
                        this.ShowMessage("登录失败，未知错误");
                    }
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (base.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(base.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.Page.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
                IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
                if (userCookie != null)
                {
                    userCookie.DeleteCookie(authCookie);
                }
                RoleHelper.SignOut(HiContext.Current.User.Username);
            }
            base.OnInit(e);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAdminLogin.Click += new EventHandler(this.btnAdminLogin_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string verifyCode = base.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{ ");
                base.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                base.Response.Write("}");
                base.Response.End();
            }
            if (!this.Page.IsPostBack)
            {
                Uri urlReferrer = this.Context.Request.UrlReferrer;
                if (urlReferrer != null)
                {
                    this.ReferralLink = urlReferrer.ToString();
                }
                this.txtAdminName.Focus();
                PageTitle.AddSiteNameTitle("后台登录", HiContext.Current.Context);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            int num;
            bool flag;
            bool flag2;
            LicenseChecker.Check(out flag, out flag2, out num);
            if (!flag)
            {
                writer.Write(this.licenseMsg);
            }
            else if (flag2)
            {
                writer.Write(this.noticeMsg);
            }
            else
            {
                base.Render(writer);
            }
        }

        private void ShowMessage(string msg)
        {
            this.lblStatus.Text = msg;
            this.lblStatus.Success = false;
            this.lblStatus.Visible = true;
        }

        private string ReferralLink
        {
            get
            {
                return (this.ViewState["ReferralLink"] as string);
            }
            set
            {
                this.ViewState["ReferralLink"] = value;
            }
        }
    }
}

