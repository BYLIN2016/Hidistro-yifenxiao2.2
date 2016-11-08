namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SearchBindProduct : DistributorPage
    {
        protected Button btnSearch;
        private int? categoryId;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string productName;
        protected Repeater rp_bindproduct;
        protected TextBox txtSearchText;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请选择一件商品！", false);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        protected void DoCallback()
        {
            this.LoadParameters();
            ProductQuery query = new ProductQuery();
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.Keywords = this.productName;
            query.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            query.IsIncludePromotionProduct = false;
            DbQueryResult products = SubSiteProducthelper.GetProducts(query);
            DataTable data = (DataTable) products.Data;
            this.pager1.TotalRecords = this.pager.TotalRecords = products.TotalRecords;
            this.rp_bindproduct.DataSource = data;
            this.rp_bindproduct.DataBind();
        }

        public string GetSkuContent(string skuId)
        {
            string str = "";
            if (!string.IsNullOrEmpty(skuId.Trim()))
            {
                foreach (DataRow row in ControlProvider.Instance().GetSkuContentBySku(skuId).Rows)
                {
                    if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                    {
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], "; " });
                    }
                }
            }
            if (!(str == ""))
            {
                return str;
            }
            return "无规格";
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            this.txtSearchText.Text = this.productName;
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.rp_bindproduct.ItemDataBound += new RepeaterItemEventHandler(this.rp_bindproduct_ItemDataBound);
            if (!base.IsPostBack)
            {
                this.DoCallback();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        protected void rp_bindproduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int productId = (int) DataBinder.Eval(e.Item.DataItem, "ProductId");
            if (productId > 0)
            {
                Repeater repeater = e.Item.FindControl("rp_sku") as Repeater;
                DataTable skusByProductId = SubSiteProducthelper.GetSkusByProductId(productId);
                repeater.DataSource = skusByProductId;
                repeater.DataBind();
            }
        }
    }
}

