namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Link_OpenId_Address : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            int result = 0;
            if (!int.TryParse(this.Page.Request.QueryString["buyAmount"], out result) || string.IsNullOrEmpty(this.Page.Request.QueryString["from"]))
            {
                if (!string.IsNullOrEmpty(base.ImageUrl))
                {
                    if (base.ImageUrl.StartsWith("~"))
                    {
                        base.ImageUrl = base.ResolveUrl(base.ImageUrl);
                    }
                    else if (base.ImageUrl.StartsWith("/"))
                    {
                        base.ImageUrl = HiContext.Current.GetSkinPath() + base.ImageUrl;
                    }
                    else
                    {
                        base.ImageUrl = HiContext.Current.GetSkinPath() + "/" + base.ImageUrl;
                    }
                }
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    base.NavigateUrl = Globals.ApplicationPath + "/OpenID/LogisticsAddress.aspx?alipaytoken=" + cookie.Value;
                    base.Render(writer);
                }
            }
        }
    }
}

