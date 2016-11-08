namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using System;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    public class LoginExit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(HiContext.Current.User.Username, true);
            IUserCookie userCookie = HiContext.Current.User.GetUserCookie();
            if (userCookie != null)
            {
                userCookie.DeleteCookie(authCookie);
            }
            RoleHelper.SignOut(HiContext.Current.User.Username);
            base.Response.Redirect("Login.aspx", true);
        }
    }
}

