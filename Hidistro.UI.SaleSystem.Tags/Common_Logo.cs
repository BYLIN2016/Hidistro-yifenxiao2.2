namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Logo : PlaceHolder
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.LogoUrl))
            {
                writer.Write(this.RendHtml());
            }
        }

        public string RendHtml()
        {
            string logoUrl = HiContext.Current.SiteSettings.LogoUrl;
            if (!HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                logoUrl = SettingsManager.GetMasterSettings(false).LogoUrl;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"logo cssEdite\" type=\"logo\" id=\"logo_1\" >").AppendLine();
            builder.AppendFormat("<a href=\"{0}\">", Globals.GetSiteUrls().UrlData.FormatUrl("home")).AppendLine();
            builder.AppendFormat("<img src=\"{0}\" />", logoUrl).AppendLine();
            builder.Append("</a>").AppendLine();
            builder.Append("</div>").AppendLine();
            return builder.ToString();
        }
    }
}

