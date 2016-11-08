namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_LoginLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling))
            {
                base.Text = "退出";
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("logout");
            }
            else
            {
                base.Text = "登录";
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("login_clean");
            }
            base.Render(writer);
        }
    }
}

