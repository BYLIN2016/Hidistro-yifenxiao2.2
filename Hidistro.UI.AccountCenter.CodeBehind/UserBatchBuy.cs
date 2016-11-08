namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class UserBatchBuy : MemberTemplatedWebControl
    {
        private Common_BatchBuy_ProductList batchbuys;
        private IButton btnBatchBuy;
        private IButton btnSearch;
        private Common_CategoriesDropDownList ddlCategories;
        private BrandCategoriesDropDownList dropBrandCategories;
        private Pager pager;
        private TextBox txtProductCode;
        private TextBox txtProductName;

        protected override void AttachChildControls()
        {
            this.batchbuys = (Common_BatchBuy_ProductList) this.FindControl("Common_BatchBuy_ProductList");
            this.btnBatchBuy = ButtonManager.Create(this.FindControl("btnBatchBuy"));
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.dropBrandCategories = (BrandCategoriesDropDownList) this.FindControl("dropBrandCategories");
            this.ddlCategories = (Common_CategoriesDropDownList) this.FindControl("ddlCategories");
            this.pager = (Pager) this.FindControl("pager");
            this.txtProductName = (TextBox) this.FindControl("txtProductName");
            this.txtProductCode = (TextBox) this.FindControl("txtProductCode");
            this.btnBatchBuy.Click += new EventHandler(this.btnBatchBuy_Click);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!HiContext.Current.SiteSettings.IsOpenSiteSale && !HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.btnBatchBuy.Visible = false;
            }
            if (!this.Page.IsPostBack)
            {
                this.dropBrandCategories.DataBind();
                this.ddlCategories.DataBind();
                this.BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery entity = new ProductQuery();
            entity.PageSize = this.pager.PageSize;
            entity.PageIndex = this.pager.PageIndex;
            entity.ProductCode = this.txtProductCode.Text;
            entity.Keywords = this.txtProductName.Text;
            entity.BrandId = this.dropBrandCategories.SelectedValue;
            entity.CategoryId = this.ddlCategories.SelectedValue;
            if (entity.CategoryId.HasValue)
            {
                entity.MaiCategoryPath = CategoryBrowser.GetCategory(entity.CategoryId.Value).Path;
            }
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult batchBuyProducts = CommentsHelper.GetBatchBuyProducts(entity);
            this.batchbuys.DataSource = batchBuyProducts.Data;
            this.batchbuys.DataBind();
            this.pager.TotalRecords = batchBuyProducts.TotalRecords;
        }

        private void btnBatchBuy_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in this.batchbuys.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    string skuId = this.batchbuys.DataKeys[row.RowIndex].Value.ToString();
                    TextBox box2 = (TextBox) row.FindControl("txtBuyNum");
                    if (string.IsNullOrEmpty(box2.Text.Trim()) || (int.Parse(box2.Text.Trim()) <= 0))
                    {
                        this.ShowMessage("购买数量值不存在或为非法值", true);
                        return;
                    }
                    ShoppingCartProcessor.AddLineItem(skuId, int.Parse(box2.Text.Trim()));
                    num++;
                }
            }
            if (num > 0)
            {
                this.ShowMessage("选择的商品已经放入购物车", true);
            }
            else
            {
                this.ShowMessage("请选择要购买的商品！", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProducts(true);
        }

        private void LoadParameters()
        {
            int num;
            int num2;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["prodcutcode"]))
            {
                this.txtProductCode.Text = Globals.UrlDecode(this.Page.Request.QueryString["prodcutcode"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyname"]))
            {
                this.txtProductName.Text = Globals.UrlDecode(this.Page.Request.QueryString["keyname"]);
            }
            if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["brandId"]), out num))
            {
                this.dropBrandCategories.SelectedValue = new int?(num);
            }
            if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["categoryId"]), out num2))
            {
                this.ddlCategories.SelectedValue = new int?(num2);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserBatchBuy.html";
            }
            base.OnInit(e);
        }

        private void ReloadProducts(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            new ProductQuery();
            if (!string.IsNullOrEmpty(this.txtProductCode.Text.Trim()))
            {
                queryStrings.Add("prodcutcode", Globals.UrlEncode(this.txtProductCode.Text.Trim()));
            }
            if (!string.IsNullOrEmpty(this.txtProductName.Text.Trim()))
            {
                queryStrings.Add("keyname", Globals.UrlEncode(this.txtProductName.Text.Trim()));
            }
            if (this.dropBrandCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", Globals.UrlEncode(this.dropBrandCategories.SelectedValue.Value.ToString()));
            }
            if (this.ddlCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", Globals.UrlEncode(this.ddlCategories.SelectedValue.Value.ToString()));
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

