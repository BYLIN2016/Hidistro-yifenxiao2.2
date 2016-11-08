namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyProducts : DistributorPage
    {
        protected ImageLinkButton btnDelete;
        protected Button btnSearch;
        private int? categoryId;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected Grid grdProducts;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        private string productName;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindProducts()
        {
            ProductQuery entity = new ProductQuery();
            entity.Keywords = this.productName;
            entity.ProductCode = this.productCode;
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            entity.SaleStatus = ProductSaleStatus.OnSale;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult products = SubSiteProducthelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
            this.pager1.TotalRecords = products.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的商品", false);
            }
            else if (SubSiteProducthelper.DeleteProducts(str) > 0)
            {
                this.ShowMsg("成功删除了选择的商品", true);
                this.ReBindProducts();
            }
            else
            {
                this.ShowMsg("删除商品失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Concat(new object[] { Globals.ApplicationPath, "/Shopadmin/product/MyProducts.aspx?productName=", Globals.UrlEncode(this.txtSearchText.Text), "&categoryId=", this.dropCategories.SelectedValue, "&productCode=", Globals.UrlEncode(this.txtSKU.Text), "&pageSize=", this.pager.PageSize }));
        }

        private void grdProducts_ReBindData(object sender)
        {
            this.BindProducts();
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SubSiteProducthelper.DeleteProducts(this.grdProducts.DataKeys[e.RowIndex].Value.ToString());
            this.BindProducts();
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
                    {
                        this.categoryId = new int?(result);
                    }
                }
                this.txtSearchText.Text = this.productName;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
            }
            else
            {
                this.productName = this.txtSearchText.Text;
                this.productCode = this.txtSKU.Text;
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
            this.grdProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
                {
                    this.grdProducts.SortOrder = this.Page.Request.QueryString["SortOrder"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrderBy"]))
                {
                    this.grdProducts.SortOrderBy = this.Page.Request.QueryString["SortOrderBy"];
                }
                this.BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBindProducts()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", this.txtSearchText.Text);
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", this.txtSKU.Text);
            queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        private IList<int> SelectedProducts
        {
            get
            {
                IList<int> list = new List<int>();
                foreach (GridViewRow row in this.grdProducts.Rows)
                {
                    CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        int item = (int) this.grdProducts.DataKeys[row.RowIndex].Value;
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}

