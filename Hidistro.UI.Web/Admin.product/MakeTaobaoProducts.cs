namespace Hidistro.UI.Web.Admin.product
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MakeProductsPack)]
    public class MakeTaobaoProducts : AdminPage
    {
        protected Button btnSearch;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private int? categoryId;
        protected DropDownList dpispub;
        protected ProductCategoriesDropDownList dropCategories;
        protected ProductLineDropDownList dropLines;
        private DateTime? endDate;
        protected Grid grdProducts;
        protected PageSize hrefPageSize;
        private int? isPub = -1;
        private int? lineId;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        private string productName;
        private DateTime? startDate;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery query2 = new ProductQuery();
            query2.IsMakeTaobao = this.isPub;
            query2.Keywords = this.productName;
            query2.ProductCode = this.productCode;
            query2.CategoryId = this.categoryId;
            query2.ProductLineId = this.lineId;
            query2.PageSize = this.pager.PageSize;
            query2.PageIndex = this.pager.PageIndex;
            query2.SaleStatus = ProductSaleStatus.All;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "DisplaySequence";
            query2.StartDate = this.startDate;
            query2.EndDate = this.endDate;
            ProductQuery entity = query2;
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = entity.Keywords;
            this.txtSKU.Text = entity.ProductCode;
            this.dropCategories.SelectedValue = entity.CategoryId;
            this.dropLines.SelectedValue = entity.ProductLineId;
            this.dpispub.SelectedValue = entity.IsMakeTaobao.ToString();
            this.pager1.TotalRecords = this.pager.TotalRecords = products.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        protected void dpispub_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            int result = -1;
            if (int.TryParse(this.Page.Request.QueryString["ismaketaobao"], out result))
            {
                this.isPub = new int?(result);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            int num2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out num2))
            {
                this.categoryId = new int?(num2);
            }
            int num3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["lineId"], out num3))
            {
                this.lineId = new int?(num3);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            this.txtSearchText.Text = this.productName;
            this.txtSKU.Text = this.productCode;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
            this.dropLines.DataBind();
            this.dropLines.SelectedValue = this.lineId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropLines.DataBind();
                this.BindProducts();
            }
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            if (this.dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", this.dropLines.SelectedValue.ToString());
            }
            if (!string.IsNullOrEmpty(this.dpispub.SelectedValue))
            {
                queryStrings.Add("ismaketaobao", this.dpispub.SelectedValue.ToString());
            }
            queryStrings.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

