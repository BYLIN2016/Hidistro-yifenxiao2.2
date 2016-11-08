namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class SaleSyetemResourceNotFound : HtmlTemplatedWebControl
    {
        private Literal litMsg;

        protected override void AttachChildControls()
        {
            this.litMsg = (Literal) this.FindControl("litMsg");
            if (this.litMsg != null)
            {
                this.litMsg.Text = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["errorMsg"]));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SaleSyetemResourceNotFound.html";
            }
            base.OnInit(e);
        }
    }
}

