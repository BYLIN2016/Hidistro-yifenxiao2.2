namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class SiteUrlDetails : DistributorPage
    {
        protected Literal litFirstSiteUrl;
        protected Literal litUserName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.litUserName.Text = HiContext.Current.User.Username;
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.litFirstSiteUrl.Text = siteSettings.SiteUrl;
                }
            }
        }
    }
}

