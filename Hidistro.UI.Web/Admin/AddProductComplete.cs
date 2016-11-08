namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProducts)]
    public class AddProductComplete : AdminPage
    {
        private int categoryId;
        protected HyperLink hlinkAddProduct;
        protected HyperLink hlinkProductDetails;
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!int.TryParse(base.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            else if (!this.Page.IsPostBack)
            {
                this.hlinkProductDetails.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.productId });
                this.hlinkAddProduct.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/AddProduct.aspx?categoryId={0}", this.categoryId));
            }
        }
    }
}

