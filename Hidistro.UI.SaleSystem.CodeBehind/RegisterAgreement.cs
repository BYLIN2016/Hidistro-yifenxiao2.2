namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class RegisterAgreement : HtmlTemplatedWebControl
    {
        private Literal litlAgreemen;

        protected override void AttachChildControls()
        {
            this.litlAgreemen = (Literal) this.FindControl("litlAgreemen");
            if (!this.Page.IsPostBack && (this.litlAgreemen != null))
            {
                this.litlAgreemen.Text = HiContext.Current.SiteSettings.RegisterAgreement;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-RegisterAgreement.html";
            }
            base.OnInit(e);
        }
    }
}

