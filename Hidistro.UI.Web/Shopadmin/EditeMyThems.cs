namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditeMyThems : DistributorPage
    {
        protected HtmlAnchor alinkBrand;
        protected HtmlAnchor alinkDefault;
        protected HtmlAnchor alinkLogin;
        protected Literal litThemeName;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litThemeName.Text = HiContext.Current.SiteSettings.Theme;
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if (siteSettings != null)
            {
                this.alinkDefault.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=default";
                this.alinkLogin.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=login";
                this.alinkBrand.HRef = "http://" + siteSettings.SiteUrl + Globals.ApplicationPath + "/Desig_Templete.aspx?skintemp=brand";
            }
        }
    }
}

