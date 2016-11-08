namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;

    public class DistributorsRegisterComplete : HtmlTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-DistributorsRegisterComplete.html";
            }
            base.OnInit(e);
        }
    }
}

