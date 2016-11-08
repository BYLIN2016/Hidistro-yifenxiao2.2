namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditReleteProducts : AdminPage
    {
        protected Button btnAddSearch;
        protected Button btnClear;
        protected Button btnSearch;
        private int? categoryId;
        protected DataList dlstProducts;
        protected DataList dlstSearchProducts;
        protected ProductCategoriesDropDownList dropCategories;
        private string keywords;
        protected Pager pager;
        protected Pager pagerSubject;
        protected Panel Panel1;
        private int productId;
        protected TextBox txtSearchText;

        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = this.keywords;
            query.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            query.PageSize = 10;
            query.PageIndex = this.pager.PageIndex;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult products = ProductHelper.GetProducts(query);
            this.dlstProducts.DataSource = products.Data;
            this.dlstProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
        }

        private void BindRelatedProducts()
        {
            Pagination page = new Pagination();
            page.PageSize = 10;
            page.PageIndex = this.pagerSubject.PageIndex;
            page.SortOrder = SortAction.Desc;
            page.SortBy = "DisplaySequence";
            DbQueryResult relatedProducts = ProductHelper.GetRelatedProducts(page, this.productId);
            this.dlstSearchProducts.DataSource = relatedProducts.Data;
            this.dlstSearchProducts.DataBind();
            this.pagerSubject.TotalRecords = relatedProducts.TotalRecords;
        }

        private void btnAddSearch_Click(object sender, EventArgs e)
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = this.keywords;
            query.CategoryId = this.categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;
            foreach (int num in ProductHelper.GetProductIds(query))
            {
                if (this.productId != num)
                {
                    ProductHelper.AddRelatedProduct(this.productId, num);
                }
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ProductHelper.ClearRelatedProducts(this.productId);
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBindPage(true);
        }

        private void dlstProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                int relatedProductId = int.Parse(this.dlstProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
                if (this.productId != relatedProductId)
                {
                    ProductHelper.AddRelatedProduct(this.productId, relatedProductId);
                }
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void dlstSearchProducts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int relatedProductId = int.Parse(this.dlstSearchProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
            ProductHelper.RemoveRelatedProduct(this.productId, relatedProductId);
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void LoadParameters()
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            if (!string.IsNullOrEmpty(base.Request.QueryString["Keywords"]))
            {
                this.keywords = base.Request.QueryString["Keywords"];
            }
            if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryId"]))
            {
                int result = 0;
                if (int.TryParse(base.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.dlstProducts.ItemCommand += new DataListCommandEventHandler(this.dlstProducts_ItemCommand);
            this.dlstSearchProducts.DeleteCommand += new DataListCommandEventHandler(this.dlstSearchProducts_DeleteCommand);
            this.btnAddSearch.Click += new EventHandler(this.btnAddSearch_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.BindProducts();
                this.BindRelatedProducts();
            }
        }

        private void ReBindPage(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productId", this.productId.ToString());
            queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("pageIndex1", this.pagerSubject.PageIndex.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

