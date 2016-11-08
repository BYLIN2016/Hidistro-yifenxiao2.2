namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class ProductUnSales : HtmlTemplatedWebControl
    {
        private Common_CutdownSearch cutdownSearch;
        private Literal litSearchResultPage;
        private Pager pager;
        private ThemedTemplatedRepeater rptProducts;

        protected override void AttachChildControls()
        {
            this.rptProducts = (ThemedTemplatedRepeater) this.FindControl("rptProducts");
            this.pager = (Pager) this.FindControl("pager");
            this.litSearchResultPage = (Literal) this.FindControl("litSearchResultPage");
            this.cutdownSearch = (Common_CutdownSearch) this.FindControl("search_Common_CutdownSearch");
            this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
            if (!this.Page.IsPostBack)
            {
                string title = "商品下架区";
                PageTitle.AddSiteNameTitle(title, HiContext.Current.Context);
                this.BindProducts();
            }
        }

        protected void BindProducts()
        {
            DbQueryResult unSaleProductList = ProductBrowser.GetUnSaleProductList(this.GetProductBrowseQuery());
            this.rptProducts.DataSource = unSaleProductList.Data;
            this.rptProducts.DataBind();
            int totalRecords = unSaleProductList.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            int num2 = 0;
            if ((totalRecords % this.pager.PageSize) > 0)
            {
                num2 = (totalRecords / this.pager.PageSize) + 1;
            }
            else
            {
                num2 = totalRecords / this.pager.PageSize;
            }
            this.litSearchResultPage.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[] { totalRecords, this.pager.PageSize, num2, this.pager.PageIndex });
        }

        private void cutdownSearch_ReSearch(object sender, EventArgs e)
        {
            this.ReLoadSearch();
        }

        protected ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery entity = new ProductBrowseQuery();
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                entity.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal result = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out result))
                {
                    entity.MinSalePrice = new decimal?(result);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal num2 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out num2))
                {
                    entity.MaxSalePrice = new decimal?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
            {
                entity.SortBy = this.Page.Request.QueryString["sortOrderBy"];
            }
            else
            {
                entity.SortBy = "DisplaySequence";
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
            {
                entity.TagIds = this.Page.Request.QueryString["TagIds"].Replace("_", ",");
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                entity.SortOrder = (SortAction) Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
            }
            else
            {
                entity.SortOrder = SortAction.Desc;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductUnSales.html";
            }
            base.OnInit(e);
        }

        public void ReLoadSearch()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
            queryStrings.Add("tagIds", Globals.UrlEncode(this.cutdownSearch.Item.TagIds));
            queryStrings.Add("minSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString()));
            queryStrings.Add("maxSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString()));
            queryStrings.Add("pageIndex", "1");
            base.ReloadPage(queryStrings);
        }
    }
}

