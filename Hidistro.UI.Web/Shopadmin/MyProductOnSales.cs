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

    public class MyProductOnSales : DistributorPage
    {
        protected Button btnAddOK;
        protected ImageLinkButton btnDelete;
        protected LinkButton btnInStock;
        protected LinkButton btnOffShelf;
        protected Button btnReplaceOK;
        protected Button btnSearch;
        protected Button btnUpdateProductTags;
        protected LinkButton btnUpShelf;
        private int? categoryId;
        protected CheckBox chkIsAlert;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected SaleStatusDropDownList dropSaleStatus;
        protected ProductTagsDropDownList dropTagList;
        protected Grid grdProducts;
        protected PageSize hrefPageSize;
        private bool isAlert;
        protected ProductTagsLiteral litralProductTag;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        private string productName;
        private ProductSaleStatus saleStatus = ProductSaleStatus.All;
        private int? tagId;
        protected TextBox txtNewWord;
        protected TextBox txtOleWord;
        protected TextBox txtPrefix;
        protected TrimTextBox txtProductTag;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;
        protected TextBox txtSuffix;

        private void BindProducts()
        {
            ProductQuery entity = new ProductQuery();
            entity.Keywords = this.productName;
            entity.ProductCode = this.productCode;
            entity.CategoryId = this.categoryId;
            entity.TagId = this.tagId;
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            entity.PageSize = this.pager.PageSize;
            entity.PageIndex = this.pager.PageIndex;
            entity.IsAlert = this.isAlert;
            entity.SaleStatus = this.saleStatus;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult products = SubSiteProducthelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
            this.pager1.TotalRecords = products.TotalRecords;
        }

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
            {
                this.ShowMsg("前后缀不能同时为空", false);
            }
            else
            {
                if (SubSiteProducthelper.UpdateProductNames(str, Globals.HtmlEncode(this.txtPrefix.Text.Trim()), Globals.HtmlEncode(this.txtSuffix.Text.Trim())))
                {
                    this.ShowMsg("为商品名称添加前后缀成功", true);
                }
                else
                {
                    this.ShowMsg("为商品名称添加前后缀失败", false);
                }
                this.BindProducts();
            }
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
                this.ReBindProducts(false);
            }
            else
            {
                this.ShowMsg("删除商品失败，未知错误", false);
            }
        }

        private void btnInStock_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要入库的商品", false);
            }
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.OnStock) > 0)
            {
                this.ShowMsg("成功入库了选择的商品，您可以在仓库里的商品里面找到入库以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("入库商品失败，未知错误", false);
            }
        }

        private void btnOffShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.UnSale) > 0)
            {
                this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("下架商品失败，未知错误", false);
            }
        }

        private void btnReplaceOK_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
            {
                this.ShowMsg("查找字符串不能为空", false);
            }
            else
            {
                if (SubSiteProducthelper.ReplaceProductNames(str, Globals.HtmlEncode(this.txtOleWord.Text.Trim()), Globals.HtmlEncode(this.txtNewWord.Text.Trim())))
                {
                    this.ShowMsg("为商品名称替换字符串缀成功", true);
                }
                else
                {
                    this.ShowMsg("为商品名称替换字符串缀失败", false);
                }
                this.BindProducts();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBindProducts(true);
        }

        private void btnUpdateProductTags_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要关联的商品", false);
            }
            else
            {
                IList<int> tagsId = new List<int>();
                if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
                {
                    string str2 = this.txtProductTag.Text.Trim();
                    string[] strArray = null;
                    if (str2.Contains(","))
                    {
                        strArray = str2.Split(new char[] { ',' });
                    }
                    else
                    {
                        strArray = new string[] { str2 };
                    }
                    foreach (string str3 in strArray)
                    {
                        tagsId.Add(Convert.ToInt32(str3));
                    }
                }
                string[] strArray2 = null;
                if (str.Contains(","))
                {
                    strArray2 = str.Split(new char[] { ',' });
                }
                else
                {
                    strArray2 = new string[] { str };
                }
                int num = 0;
                foreach (string str4 in strArray2)
                {
                    SubSiteProducthelper.DeleteProductTags(Convert.ToInt32(str4), null);
                    if ((tagsId.Count > 0) && SubSiteProducthelper.AddProductTags(Convert.ToInt32(str4), tagsId, null))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
                }
                else
                {
                    this.ShowMsg("已成功取消了商品关联的商品标签", true);
                }
                this.txtProductTag.Text = "";
            }
        }

        private void btnUpShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要上架的商品", false);
            }
            else if (!SubSiteProducthelper.IsOnSale(str))
            {
                this.ShowMsg("选择要上架的商品中有一口价低于最低零售价的情况", false);
            }
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.OnSale) > 0)
            {
                this.ShowMsg("成功上架了选择的商品,您可以到出售中的商品中找到上架的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("上架商品失败，未知错误", false);
            }
        }

        private void dropSaleStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReBindProducts(true);
        }

        private void grdProducts_ReBindData(object sender)
        {
            this.BindProducts();
        }

        private void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal) e.Row.FindControl("litSaleStatus");
                if (literal.Text == "1")
                {
                    literal.Text = "出售中";
                }
                else if (literal.Text == "2")
                {
                    literal.Text = "下架区";
                }
                else
                {
                    literal.Text = "仓库中";
                }
            }
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tagId"]))
                {
                    int num2 = 0;
                    if (int.TryParse(this.Page.Request.QueryString["tagId"], out num2))
                    {
                        this.tagId = new int?(num2);
                    }
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
                {
                    this.saleStatus = (ProductSaleStatus) Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
                }
                bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
                this.txtSearchText.Text = this.productName;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
                this.dropTagList.DataBind();
                this.dropTagList.SelectedValue = this.tagId;
                this.chkIsAlert.Checked = this.isAlert;
                this.dropSaleStatus.SelectedValue = this.saleStatus;
            }
            else
            {
                this.productName = this.txtSearchText.Text;
                this.productCode = this.txtSKU.Text;
                this.categoryId = this.dropCategories.SelectedValue;
                this.tagId = this.dropTagList.SelectedValue;
                this.saleStatus = this.dropSaleStatus.SelectedValue;
                this.isAlert = this.chkIsAlert.Checked;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
            this.grdProducts.RowDataBound += new GridViewRowEventHandler(this.grdProducts_RowDataBound);
            this.btnUpShelf.Click += new EventHandler(this.btnUpShelf_Click);
            this.btnOffShelf.Click += new EventHandler(this.btnOffShelf_Click);
            this.btnInStock.Click += new EventHandler(this.btnInStock_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnAddOK.Click += new EventHandler(this.btnAddOK_Click);
            this.btnReplaceOK.Click += new EventHandler(this.btnReplaceOK_Click);
            this.btnUpdateProductTags.Click += new EventHandler(this.btnUpdateProductTags_Click);
            this.grdProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
            this.dropSaleStatus.SelectedIndexChanged += new EventHandler(this.dropSaleStatus_SelectedIndexChanged);
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
                this.dropSaleStatus.DataBind();
                this.BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBindProducts(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtSearchText.Text));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (this.dropTagList.SelectedValue.HasValue)
            {
                queryStrings.Add("tagId", this.dropTagList.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", Globals.UrlEncode(this.txtSKU.Text));
            if (isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("isAlert", this.chkIsAlert.Checked.ToString());
            queryStrings.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

