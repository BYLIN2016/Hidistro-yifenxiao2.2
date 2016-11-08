namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;

    public class Category : HtmlTemplatedWebControl
    {
        private ThemedTemplatedRepeater rptCategories;

        protected override void AttachChildControls()
        {
            this.rptCategories = (ThemedTemplatedRepeater) this.FindControl("rptCategories");
            if (!this.Page.IsPostBack)
            {
                DataSet threeLayerCategories = CategoryBrowser.GetThreeLayerCategories();
                this.rptCategories.DataSource = threeLayerCategories.Tables[0].DefaultView;
                this.rptCategories.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Category.html";
            }
        }
    }
}

