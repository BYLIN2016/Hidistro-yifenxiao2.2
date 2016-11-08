namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class DistributorsRegisterAgreement : HtmlTemplatedWebControl
    {
        private Literal litRequestInstruction;
        private Literal litRequestProtocols;

        protected override void AttachChildControls()
        {
            this.litRequestInstruction = (Literal) this.FindControl("litRequestInstruction");
            this.litRequestProtocols = (Literal) this.FindControl("litRequestProtocols");
            if (!this.Page.IsPostBack)
            {
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                this.litRequestInstruction.Text = siteSettings.DistributorRequestInstruction;
                this.litRequestProtocols.Text = siteSettings.DistributorRequestProtocols;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorsRegisterAgreement.html";
            }
            base.OnInit(e);
        }
    }
}

