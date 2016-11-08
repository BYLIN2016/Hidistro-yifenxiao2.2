namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_MyAccountLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling))
            {
                base.Text = "我的账户";
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("user_UserDefault");
            }
            else
            {
                base.Text = "注册";
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("register");
            }
            base.Render(writer);
        }
    }
}

