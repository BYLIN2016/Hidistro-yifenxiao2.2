namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ViewProduct_Consultation : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = "我要咨询";
            }
            base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("ProductConsultations", new object[] { this.Page.Request.QueryString["productId"] });
            base.Render(writer);
        }
    }
}

