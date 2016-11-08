namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class Editethems : AdminPage
    {
        protected Literal litThemeName;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.litThemeName.Text = HiContext.Current.SiteSettings.Theme;
        }
    }
}

