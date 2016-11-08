namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Affiches : HtmlTemplatedWebControl
    {
        private ThemedTemplatedRepeater rptAffiches;

        protected override void AttachChildControls()
        {
            this.rptAffiches = (ThemedTemplatedRepeater) this.FindControl("rptAffiches");
            if (!this.Page.IsPostBack)
            {
                this.rptAffiches.DataSource = CommentBrowser.GetAfficheList();
                this.rptAffiches.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Affiches.html";
            }
            base.OnInit(e);
        }
    }
}

