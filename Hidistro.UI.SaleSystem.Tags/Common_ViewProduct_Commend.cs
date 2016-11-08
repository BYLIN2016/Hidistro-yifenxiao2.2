namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ViewProduct_Commend : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("IntroducedToFriend", new object[] { this.Page.Request.QueryString["productId"] });
            base.Render(writer);
        }
    }
}

