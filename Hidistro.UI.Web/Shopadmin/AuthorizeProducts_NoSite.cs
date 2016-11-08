namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class AuthorizeProducts_NoSite : DistributorPage
    {
        protected Button btnSearch;
        protected Grid grdAuthorizeProducts;
        protected PageSize hrefPageSize;
        private int? lineId;
        protected Literal litPageTitle;
        private string name;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        protected TextBox txtName;
        protected TextBox txtSKU;

        private void BindData()
        {
            ProductQuery entity = new ProductQuery();
            entity.PageSize = this.pager.PageSize;
            entity.PageIndex = this.pager.PageIndex;
            entity.ProductCode = this.productCode;
            entity.Keywords = this.name;
            entity.ProductLineId = this.lineId;
            if (this.grdAuthorizeProducts.SortOrder.ToLower() == "desc")
            {
                entity.SortOrder = SortAction.Desc;
            }
            entity.SortBy = this.grdAuthorizeProducts.SortOrderBy;
            Globals.EntityCoding(entity, true);
            DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(entity, false);
            this.grdAuthorizeProducts.DataSource = authorizeProducts.Data;
            this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = authorizeProducts.TotalRecords;
            this.pager1.TotalRecords = authorizeProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBindData(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["name"]))
                {
                    this.name = base.Server.UrlDecode(this.Page.Request.QueryString["name"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]))
                {
                    int result = 0;
                    if (int.TryParse(this.Page.Request.QueryString["lineId"], out result))
                    {
                        this.lineId = new int?(result);
                    }
                    this.litPageTitle.Text = "＂" + base.Server.UrlDecode(this.Page.Request.QueryString["lineName"]) + "＂产品线下商品列表";
                }
                this.txtSKU.Text = this.productCode;
                this.txtName.Text = this.name;
            }
            else
            {
                this.productCode = this.txtSKU.Text;
                this.name = this.txtName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBindData(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.txtSKU.Text))
            {
                queryStrings.Add("ProductCode", this.txtSKU.Text);
            }
            if (!string.IsNullOrEmpty(this.txtName.Text))
            {
                queryStrings.Add("name", this.txtName.Text);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]))
            {
                queryStrings.Add("lineId", this.Page.Request.QueryString["lineId"]);
                queryStrings.Add("lineName", base.Server.UrlDecode(this.Page.Request.QueryString["lineName"]));
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

