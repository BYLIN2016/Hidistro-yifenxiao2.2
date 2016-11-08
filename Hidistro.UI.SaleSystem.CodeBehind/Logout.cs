namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    public class Logout : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                cookie.Expires = DateTime.Now;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
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
                this.Context.Response.Cookies["hishopLoginStatus"].Value = "";
            }
            this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
        }
    }
}

