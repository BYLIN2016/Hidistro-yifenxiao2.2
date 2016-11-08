namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AdminMaster : MasterPage
    {
        protected ContentPlaceHolder contentHolder;
        protected ContentPlaceHolder headHolder;
        protected HyperLink hlinkAdminDefault;
        protected HyperLink hlinkDefault;
        protected HyperLink hlinkLogout;
        protected HyperLink hlinkService;
        protected Image imgLogo;
        protected Label lblUserName;
        protected Literal mainMenuHolder;
        protected PageTitle PageTitle1;
        protected Script Script1;
        protected Script Script2;
        protected Script Script3;
        protected Script Script4;
        protected Script Script5;
        protected Script Script6;
        protected Script Script7;
        protected Literal subMenuHolder;
        protected HtmlForm thisForm;
        protected ContentPlaceHolder validateHolder;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PageTitle.AddTitle(HiContext.Current.SiteSettings.SiteName, this.Context);
            foreach (Control control in this.Page.Header.Controls)
            {
                if (control is HtmlLink)
                {
                    HtmlLink link = control as HtmlLink;
                    if (link.Href.StartsWith("/"))
                    {
                        link.Href = Globals.ApplicationPath + link.Href;
                    }
                    else
                    {
                        link.Href = Globals.ApplicationPath + "/" + link.Href;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

