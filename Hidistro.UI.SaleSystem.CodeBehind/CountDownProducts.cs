namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class CountDownProducts : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptProduct;

        protected override void AttachChildControls()
        {
            this.rptProduct = (ThemedTemplatedRepeater) this.FindControl("rptProduct");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }

        private void BindProduct()
        {
            DbQueryResult countDownProductList = ProductBrowser.GetCountDownProductList(this.GetProductBrowseQuery());
            this.rptProduct.DataSource = countDownProductList.Data;
            this.rptProduct.DataBind();
            this.pager.TotalRecords = countDownProductList.TotalRecords;
        }

        private ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery query = new ProductBrowseQuery();
            query.IsCount = true;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-CountDownProducts.html";
            }
            base.OnInit(e);
        }
    }
}

