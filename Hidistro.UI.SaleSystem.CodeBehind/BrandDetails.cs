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
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class BrandDetails : HtmlTemplatedWebControl
    {
        private int brandId;
        private Common_Search_SortPopularity btnSortPopularity;
        private Common_Search_SortPrice btnSortPrice;
        private Common_Search_SortSaleCounts btnSortSaleCounts;
        private Common_Search_SortTime btnSortTime;
        private Common_CutdownSearch cutdownSearch;
        private Literal litBrandName;
        private Literal litBrandProductResult;
        private Literal litBrandRemark;
        private Pager pager;
        private ThemedTemplatedRepeater rptProduct;

        public BrandDetails()
        {
            if (!int.TryParse(this.Page.Request.QueryString["brandId"], out this.brandId))
            {
                base.GotoResourceNotFound();
            }
            BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.brandId);
            if (((brandCategory != null) && !string.IsNullOrEmpty(brandCategory.Theme)) && File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/brandcategorythemes/" + brandCategory.Theme)))
            {
                this.SkinName = "/brandcategorythemes/" + brandCategory.Theme;
            }
        }

        protected override void AttachChildControls()
        {
            this.litBrandName = (Literal) this.FindControl("litBrandName");
            this.litBrandRemark = (Literal) this.FindControl("litBrandRemark");
            this.rptProduct = (ThemedTemplatedRepeater) this.FindControl("rptProduct");
            this.pager = (Pager) this.FindControl("pager");
            this.litBrandProductResult = (Literal) this.FindControl("litBrandProductResult");
            this.cutdownSearch = (Common_CutdownSearch) this.FindControl("search_Common_CutdownSearch");
            this.btnSortPrice = (Common_Search_SortPrice) this.FindControl("btn_Common_Search_SortPrice");
            this.btnSortTime = (Common_Search_SortTime) this.FindControl("btn_Common_Search_SortTime");
            this.btnSortPopularity = (Common_Search_SortPopularity) this.FindControl("btn_Common_Search_SortPopularity");
            this.btnSortSaleCounts = (Common_Search_SortSaleCounts) this.FindControl("btn_Common_Search_SortSaleCounts");
            this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
            this.btnSortPrice.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortPrice_Sorting);
            this.btnSortTime.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortTime_Sorting);
            if (this.btnSortPopularity != null)
            {
                this.btnSortPopularity.Sorting += new Common_Search_SortPopularity.SortingHandler(this.btnSortPopularity_Sorting);
            }
            if (this.btnSortSaleCounts != null)
            {
                this.btnSortSaleCounts.Sorting += new Common_Search_SortSaleCounts.SortingHandler(this.btnSortSaleCounts_Sorting);
            }
            if (!this.Page.IsPostBack)
            {
                BrandCategoryInfo brandCategory = CategoryBrowser.GetBrandCategory(this.brandId);
                if (brandCategory == null)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该品牌已经不存在"));
                }
                else
                {
                    this.LoadCategoryHead(brandCategory);
                    this.litBrandName.Text = brandCategory.BrandName;
                    this.litBrandRemark.Text = brandCategory.Description;
                    PageTitle.AddSiteNameTitle(brandCategory.BrandName, HiContext.Current.Context);
                    this.BindBrandProduct();
                }
            }
        }

        private void BindBrandProduct()
        {
            DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(this.GetProductBrowseQuery());
            this.rptProduct.DataSource = browseProductList.Data;
            this.rptProduct.DataBind();
            this.pager.TotalRecords = browseProductList.TotalRecords;
            int num = 0;
            if ((Convert.ToDouble(browseProductList.TotalRecords) % ((double) this.pager.PageSize)) > 0.0)
            {
                num = (browseProductList.TotalRecords / this.pager.PageSize) + 1;
            }
            else
            {
                num = browseProductList.TotalRecords / this.pager.PageSize;
            }
            this.litBrandProductResult.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[] { browseProductList.TotalRecords, this.pager.PageSize, num, this.pager.PageIndex });
        }

        private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

        private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

        private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

        private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

        private void cutdownSearch_ReSearch(object sender, EventArgs e)
        {
            this.ReloadBrand(string.Empty, string.Empty);
        }

        private ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery entity = new ProductBrowseQuery();
            entity.BrandId = new int?(this.brandId);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
            {
                entity.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
            }
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
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                entity.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
            {
                entity.SortBy = this.Page.Request.QueryString["sortOrderBy"];
            }
            else
            {
                entity.SortBy = "DisplaySequence";
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

        private void LoadCategoryHead(BrandCategoryInfo brandcategory)
        {
            if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
            {
                MetaTags.AddMetaKeywords(brandcategory.MetaKeywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(brandcategory.MetaKeywords))
            {
                MetaTags.AddMetaDescription(brandcategory.MetaDescription, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(brandcategory.BrandName))
            {
                PageTitle.AddSiteNameTitle(brandcategory.BrandName, HiContext.Current.Context);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-BrandDetails.html";
            }
            base.OnInit(e);
        }

        private void ReloadBrand(string sortOrder, string sortOrderBy)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("brandId", this.brandId.ToString());
            queryStrings.Add("TagIds", Globals.UrlEncode(this.cutdownSearch.Item.TagIds));
            queryStrings.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
            queryStrings.Add("minSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString()));
            queryStrings.Add("maxSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString()));
            queryStrings.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            queryStrings.Add("sortOrderBy", sortOrderBy);
            queryStrings.Add("sortOrder", sortOrder);
            base.ReloadPage(queryStrings);
        }
    }
}

