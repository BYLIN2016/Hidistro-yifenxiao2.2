namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;

    [AdministerCheck(true)]
    public class CnzzStatisticTotal : AdminPage
    {
        protected HtmlGenericControl framcnz;

        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!string.IsNullOrEmpty(siteSettings.CnzzPassword) && !string.IsNullOrEmpty(siteSettings.CnzzUsername))
            {
                this.framcnz.Attributes["src"] = "http://wss.cnzz.com/user/companion/92hi_login.php?site_id=" + siteSettings.CnzzUsername + "&password=" + siteSettings.CnzzPassword;
            }
            else
            {
                this.Page.Response.Redirect("cnzzstatisticsset.aspx");
            }
        }
    }
}

