namespace Hidistro.UI.Web.OpenID
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class OpenIdEntry : Page
    {
        protected HtmlForm form1;
        private string openIdType;
        private NameValueCollection parameters;

        private string GeneratePassword()
        {
            return this.GenerateRndString(8, "");
        }

        private string GenerateRndString(int length, string prefix)
        {
            string str = string.Empty;
            Random random = new Random();
            while (str.Length < 10)
            {
                char ch;
                int num = random.Next();
                if ((num % 3) == 0)
                {
                    ch = (char) (0x61 + ((ushort) (num % 0x1a)));
                }
                else
                {
                    ch = (char) (0x30 + ((ushort) (num % 10)));
                }
                str = str + ch.ToString();
            }
            return (prefix + str);
        }

        private string GenerateUsername()
        {
            return this.GenerateRndString(10, "u_");
        }

        private string GenerateUsername(int length)
        {
            return this.GenerateRndString(length, "u_");
        }

        private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
        {
            HttpCookie cookie;
            string str2;
            this.parameters.Add("CurrentOpenId", e.OpenId);
            HiContext current = HiContext.Current;
            string usernameWithOpenId = UserHelper.GetUsernameWithOpenId(e.OpenId, this.openIdType);
            if (string.IsNullOrEmpty(usernameWithOpenId))
            {
                string str3 = this.openIdType.ToLower();
                if (str3 == null)
                {
                    goto Label_024D;
                }
                if (!(str3 == "hishop.plugins.openid.alipay.alipayservice"))
                {
                    if (str3 == "hishop.plugins.openid.qq.qqservice")
                    {
                        this.SkipQQOpenId();
                        goto Label_0267;
                    }
                    if (str3 == "hishop.plugins.openid.taobao.taobaoservice")
                    {
                        this.SkipTaoBaoOpenId();
                        goto Label_0267;
                    }
                    if (str3 == "hishop.plugins.openid.sina.sinaservice")
                    {
                        this.SkipSinaOpenId();
                        goto Label_0267;
                    }
                    goto Label_024D;
                }
                this.SkipAlipayOpenId();
                goto Label_0267;
            }
            Member member = Users.GetUser(0, usernameWithOpenId, false, true) as Member;
            if (member == null)
            {
                base.Response.Write("登录失败，信任登录只能用于会员登录。");
                return;
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                if (member.ParentUserId.HasValue)
                {
                    if (member.ParentUserId.Value == HiContext.Current.SiteSettings.UserId)
                    {
                        goto Label_00FE;
                    }
                }
                base.Response.Write("账号已经与本平台的其它子站绑定，不能在此域名上登录。");
                return;
            }
            if (member.ParentUserId.HasValue && (member.ParentUserId.Value != 0))
            {
                base.Response.Write("账号已经与本平台的其它子站绑定，不能在此域名上登录。");
                return;
            }
        Label_00FE:
            cookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(cookie, 30, false);
            HiContext.Current.User = member;
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(this.parameters["token"]))
            {
                HttpCookie cookie3 = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie3.Expires = DateTime.Now.AddMinutes(30.0);
                cookie3.Value = this.parameters["token"];
                HttpContext.Current.Response.Cookies.Add(cookie3);
            }
            goto Label_0267;
        Label_024D:
            this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
        Label_0267:
            str2 = this.parameters["HITO"];
            if (str2 == "1")
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
            }
        }

        private void Notify_Failed(object sender, FailedEventArgs e)
        {
            base.Response.Write("登录失败，" + e.Message);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
            }
            this.openIdType = this.Page.Request.QueryString["HIGW"];
            OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(this.openIdType);
            if (openIdSettings == null)
            {
                base.Response.Write("登录失败，没有找到对应的插件配置信息。");
            }
            else
            {
                NameValueCollection values = new NameValueCollection();
                values.Add(this.Page.Request.Form);
                values.Add(this.Page.Request.QueryString);
                this.parameters = values;
                OpenIdNotify notify = OpenIdNotify.CreateInstance(this.openIdType, this.parameters);
                notify.Authenticated += new EventHandler<AuthenticatedEventArgs>(this.Notify_Authenticated);
                notify.Failed += new EventHandler<FailedEventArgs>(this.Notify_Failed);
                try
                {
                    notify.Verify(0x7530, HiCryptographer.Decrypt(openIdSettings.Settings));
                }
                catch
                {
                    this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
                }
            }
        }

        protected void SkipAlipayOpenId()
        {
            Member member = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                member = new Member(UserRole.Underling);
                member.ParentUserId = HiContext.Current.SiteSettings.UserId;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.Username = this.parameters["real_name"];
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "支付宝会员_" + this.parameters["user_id"];
            }
            member.Email = this.parameters["email"];
            if (string.IsNullOrEmpty(member.Email))
            {
                member.Email = this.GenerateUsername() + "@localhost.com";
            }
            string str = this.GeneratePassword();
            member.Password = str;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "支付宝会员_" + this.parameters["user_id"];
                member.Password = member.TradePassword = str;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = this.GenerateUsername();
                    member.Email = this.GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        base.Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(this.parameters["token"]))
            {
                HttpCookie cookie = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie.Expires = DateTime.Now.AddMinutes(30.0);
                cookie.Value = this.parameters["token"];
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (!string.IsNullOrEmpty(this.parameters["target_url"]))
            {
                this.Page.Response.Redirect(this.parameters["target_url"]);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }

        protected void SkipQQOpenId()
        {
            Member member = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                member = new Member(UserRole.Underling);
                member.ParentUserId = HiContext.Current.SiteSettings.UserId;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            HttpCookie cookie = HttpContext.Current.Request.Cookies["NickName"];
            if (cookie != null)
            {
                member.Username = HttpUtility.UrlDecode(cookie.Value);
            }
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "腾讯会员_" + this.GenerateUsername(8);
            }
            member.Email = this.GenerateUsername() + "@localhost.com";
            string str = this.GeneratePassword();
            member.Password = str;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "腾讯会员_" + this.GenerateUsername(8);
                member.Password = member.TradePassword = str;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = this.GenerateUsername();
                    member.Email = this.GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        base.Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(this.parameters["token"]))
            {
                HttpCookie cookie4 = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie4.Expires = DateTime.Now.AddMinutes(30.0);
                cookie4.Value = this.parameters["token"];
                HttpContext.Current.Response.Cookies.Add(cookie4);
            }
            if (!string.IsNullOrEmpty(this.parameters["target_url"]))
            {
                this.Page.Response.Redirect(this.parameters["target_url"]);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }

        protected void SkipSinaOpenId()
        {
            Member member = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                member = new Member(UserRole.Underling);
                member.ParentUserId = HiContext.Current.SiteSettings.UserId;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            member.Username = this.parameters["CurrentOpenId"];
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "新浪微博会员_" + this.GenerateUsername(8);
            }
            member.Email = this.GenerateUsername() + "@localhost.com";
            string str = this.GeneratePassword();
            member.Password = str;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "新浪微博会员_" + this.GenerateUsername(9);
                member.Password = member.TradePassword = str;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = this.GenerateUsername();
                    member.Email = this.GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        base.Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(this.parameters["token"]))
            {
                HttpCookie cookie = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie.Expires = DateTime.Now.AddMinutes(30.0);
                cookie.Value = this.parameters["token"];
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }

        protected void SkipTaoBaoOpenId()
        {
            Member member = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                member = new Member(UserRole.Underling);
                member.ParentUserId = HiContext.Current.SiteSettings.UserId;
            }
            else
            {
                member = new Member(UserRole.Member);
            }
            if (HiContext.Current.ReferralUserId > 0)
            {
                member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
            }
            member.GradeId = MemberProcessor.GetDefaultMemberGrade();
            string str = this.parameters["CurrentOpenId"];
            if (!string.IsNullOrEmpty(str))
            {
                member.Username = HttpUtility.UrlDecode(str);
            }
            if (string.IsNullOrEmpty(member.Username))
            {
                member.Username = "淘宝会员_" + this.GenerateUsername(8);
            }
            member.Email = this.GenerateUsername() + "@localhost.com";
            if (string.IsNullOrEmpty(member.Email))
            {
                member.Email = this.GenerateUsername() + "@localhost.com";
            }
            string str2 = this.GeneratePassword();
            member.Password = str2;
            member.PasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
            member.TradePassword = str2;
            member.IsApproved = true;
            member.RealName = string.Empty;
            member.Address = string.Empty;
            if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
            {
                member.Username = "淘宝会员_" + this.GenerateUsername(8);
                member.Password = member.TradePassword = str2;
                if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                {
                    member.Username = this.GenerateUsername();
                    member.Email = this.GenerateUsername() + "@localhost.com";
                    member.Password = member.TradePassword = str2;
                    if (MemberProcessor.CreateMember(member) != CreateUserStatus.Created)
                    {
                        base.Response.Write("为您创建随机账户时失败，请重试。");
                        return;
                    }
                }
            }
            UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
            member.GetUserCookie().WriteCookie(authCookie, 30, false);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            CookieShoppingProvider.Instance().ClearShoppingCart();
            HiContext.Current.User = member;
            if (shoppingCart != null)
            {
                ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
            }
            if (!string.IsNullOrEmpty(this.parameters["token"]))
            {
                HttpCookie cookie = new HttpCookie("Token_" + HiContext.Current.User.UserId.ToString());
                cookie.Expires = DateTime.Now.AddMinutes(30.0);
                cookie.Value = this.parameters["token"];
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            if (!string.IsNullOrEmpty(this.parameters["target_url"]))
            {
                this.Page.Response.Redirect(this.parameters["target_url"]);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
        }
    }
}

