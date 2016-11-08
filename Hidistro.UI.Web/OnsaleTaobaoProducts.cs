namespace Hidistro.UI.Web.Admin.TB
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Products)]
    public partial class OnsaleTaobaoProducts : AdminPage
    {
        protected Button btnCancle;
        protected ImageLinkButton btnDelete;
        protected HyperLink btnDownTaobao;
        protected Button btnInStock;
        protected Button btnPenetration;
        protected Button btnSearch;
        protected Button btnUnSale;
        protected Button btnUpdateLine;
        protected Button btnUpdateProductTags;
        protected Button btnUpSale;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private int? categoryId;
        protected CheckBox chkDeleteImage;
        protected CheckBox chkInstock;
        protected CheckBox chkIsAlert;
        private int? distributorId;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        protected DistributorDropDownList dropDistributor;
        protected ProductLineDropDownList dropLines;
        protected PenetrationStatusDropDownList dropPenetrationStatus;
        protected ProductLineDropDownList dropProductLines;
        protected SaleStatusDropDownList dropSaleStatus;
        protected ProductTagsDropDownList dropTagList;
        protected ProductTypeDownList dropType;
        private DateTime? endDate;
        protected Grid grdProducts;
        protected HtmlInputHidden hdPenetrationStatus;
        protected HtmlInputHidden hdProductLine;
        protected PageSize hrefPageSize;
        private bool isAlert;
        private int? lineId;
        protected ProductTagsLiteral litralProductTag;
        protected Pager pager;
        protected Pager pager1;
        private PenetrationStatus penetrationStatus;
        private string productCode;
        private string productName;
        private ProductSaleStatus saleStatus = ProductSaleStatus.All;
        private DateTime? startDate;
        private int? tagId;
        protected TrimTextBox txtProductTag;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;
        private int? typeId;

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery query2 = new ProductQuery();
            query2.Keywords = this.productName;
            query2.ProductCode = this.productCode;
            query2.CategoryId = this.categoryId;
            query2.ProductLineId = this.lineId;
            query2.PageSize = this.pager.PageSize;
            query2.PageIndex = this.pager.PageIndex;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "DisplaySequence";
            query2.StartDate = this.startDate;
            query2.BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null;
            query2.TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null;
            query2.TypeId = this.typeId;
            query2.UserId = this.distributorId;
            query2.IsAlert = this.isAlert;
            query2.SaleStatus = this.saleStatus;
            query2.PenetrationStatus = this.penetrationStatus;
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
            this.dropType.SelectedValue = entity.TypeId;
            this.dropDistributor.SelectedValue = entity.UserId;
            this.chkIsAlert.Checked = entity.IsAlert;
            this.pager1.TotalRecords = this.pager.TotalRecords = products.TotalRecords;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else
            {
                SendMessageHelper.SendMessageToDistributors(str, 5);
                if (ProductHelper.CanclePenetrationProducts(str) > 0)
                {
                    this.ShowMsg("取消铺货成功", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("取消铺货失败，未知错误", false);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要删除的商品", false);
            }
            else
            {
                List<int> list = new List<int>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add(Convert.ToInt32(str2));
                }
                SendMessageHelper.SendMessageToDistributors(str, 3);
                if (ProductHelper.RemoveProduct(str) > 0)
                {
                    this.ShowMsg("成功删除了选择的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("删除商品失败，未知错误", false);
                }
                if (this.hdPenetrationStatus.Value.Equals("1"))
                {
                    ProductHelper.CanclePenetrationProducts(str);
                }
            }
        }

        private void btnInStock_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要入库的商品", false);
            }
            else
            {
                if (this.hdPenetrationStatus.Value.Equals("1"))
                {
                    SendMessageHelper.SendMessageToDistributors(str, 2);
                    if (ProductHelper.CanclePenetrationProducts(str) == 0)
                    {
                        this.ShowMsg("取消铺货失败！", false);
                        return;
                    }
                }
                if (ProductHelper.InStock(str) > 0)
                {
                    this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("入库商品失败，未知错误", false);
                }
            }
        }

        private void btnPenetration_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else if (ProductHelper.PenetrationProducts(str) > 0)
            {
                this.ShowMsg("铺货成功", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("铺货失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void btnUnSale_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要下架的商品", false);
            }
            else
            {
                if (this.hdPenetrationStatus.Value.Equals("1"))
                {
                    SendMessageHelper.SendMessageToDistributors(str, 1);
                    if (ProductHelper.CanclePenetrationProducts(str) == 0)
                    {
                        this.ShowMsg("取消铺货失败！", false);
                        return;
                    }
                }
                if (ProductHelper.OffShelf(str) > 0)
                {
                    this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("下架商品失败，未知错误", false);
                }
            }
        }

        private void btnUpdateLine_Click(object sender, EventArgs e)
        {
            int num = 0;
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要转移的商品", false);
            }
            else
            {
                SendMessageHelper.SendMessageToDistributors(str + "|" + this.dropProductLines.SelectedItem.Text, 6);
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    if (ProductLineHelper.UpdateProductLine(Convert.ToInt32(this.hdProductLine.Value), int.Parse(str2)))
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    this.BindProducts();
                    this.ShowMsg(string.Format("成功转移了{0}件商品", num), true);
                }
                else
                {
                    this.ShowMsg("转移商品失败", false);
                }
            }
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
                    ProductHelper.DeleteProductTags(Convert.ToInt32(str4), null);
                    if ((tagsId.Count > 0) && ProductHelper.AddProductTags(Convert.ToInt32(str4), tagsId, null))
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
                    this.ShowMsg("已成功取消了商品的关联商品标签", true);
                }
                this.txtProductTag.Text = "";
            }
        }

        private void btnUpSale_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择要上架的商品", false);
            }
            else if (ProductHelper.UpShelf(str) > 0)
            {
                this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                this.BindProducts();
            }
            else
            {
                this.ShowMsg("上架商品失败，未知错误", false);
            }
        }

        private void dropPenetrationStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void dropSaleStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadProductOnSales(true);
        }

        private void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal)e.Row.FindControl("litSaleStatus");
                Literal literal2 = (Literal)e.Row.FindControl("litMarketPrice");
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
                if (string.IsNullOrEmpty(literal2.Text))
                {
                    literal2.Text = "-";
                }
            }
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<int> list = new List<int>();
            string str = this.grdProducts.DataKeys[e.RowIndex].Value.ToString();
            if (str != "")
            {
                list.Add(Convert.ToInt32(str));
            }
            SendMessageHelper.SendMessageToDistributors(str, 3);
            if (this.hdPenetrationStatus.Value.Equals("1"))
            {
                ProductHelper.CanclePenetrationProducts(str);
            }
            if (ProductHelper.RemoveProduct(str) > 0)
            {
                this.ShowMsg("删除商品成功", true);
                this.ReloadProductOnSales(false);
            }
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
                this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out num2))
            {
                this.dropBrandList.SelectedValue = new int?(num2);
            }
            int num3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["lineId"], out num3))
            {
                this.lineId = new int?(num3);
            }
            int num4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["tagId"], out num4))
            {
                this.tagId = new int?(num4);
            }
            int num5 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out num5))
            {
                this.typeId = new int?(num5);
            }
            int num6 = 0;
            if (int.TryParse(this.Page.Request.QueryString["distributorId"], out num6))
            {
                this.distributorId = new int?(num6);
            }
            bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
            {
                this.saleStatus = (ProductSaleStatus)Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PenetrationStatus"]))
            {
                this.penetrationStatus = (PenetrationStatus)Enum.Parse(typeof(PenetrationStatus), this.Page.Request.QueryString["PenetrationStatus"]);
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
            this.dropTagList.DataBind();
            this.dropTagList.SelectedValue = this.tagId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
            this.dropType.SelectedValue = this.typeId;
            this.dropDistributor.SelectedValue = this.distributorId;
            this.chkIsAlert.Checked = this.isAlert;
            this.dropPenetrationStatus.SelectedValue = this.penetrationStatus;
            this.dropSaleStatus.SelectedValue = this.saleStatus;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnUpSale.Click += new EventHandler(this.btnUpSale_Click);
            this.btnUnSale.Click += new EventHandler(this.btnUnSale_Click);
            this.btnInStock.Click += new EventHandler(this.btnInStock_Click);
            this.btnCancle.Click += new EventHandler(this.btnCancle_Click);
            this.btnPenetration.Click += new EventHandler(this.btnPenetration_Click);
            this.btnUpdateProductTags.Click += new EventHandler(this.btnUpdateProductTags_Click);
            this.btnUpdateLine.Click += new EventHandler(this.btnUpdateLine_Click);
            this.grdProducts.RowDataBound += new GridViewRowEventHandler(this.grdProducts_RowDataBound);
            this.grdProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
            this.dropSaleStatus.SelectedIndexChanged += new EventHandler(this.dropSaleStatus_SelectedIndexChanged);
            this.dropPenetrationStatus.SelectedIndexChanged += new EventHandler(this.dropPenetrationStatus_SelectedIndexChanged);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropLines.DataBind();
                this.dropBrandList.DataBind();
                this.dropTagList.DataBind();
                this.dropType.DataBind();
                this.dropDistributor.DataBind();
                this.dropPenetrationStatus.DataBind();
                this.dropSaleStatus.DataBind();
                this.dropProductLines.DataBind();
                this.btnDownTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", HiContext.Current.SiteSettings.SiteUrl, Globals.ApplicationPath);
                this.BindProducts();
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
            if (this.dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", this.dropLines.SelectedValue.ToString());
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
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            if (this.dropTagList.SelectedValue.HasValue)
            {
                queryStrings.Add("tagId", this.dropTagList.SelectedValue.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                queryStrings.Add("typeId", this.dropType.SelectedValue.ToString());
            }
            if (this.dropDistributor.SelectedValue.HasValue)
            {
                queryStrings.Add("distributorId", this.dropDistributor.SelectedValue.ToString());
            }
            queryStrings.Add("isAlert", this.chkIsAlert.Checked.ToString());
            queryStrings.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
            queryStrings.Add("PenetrationStatus", this.dropPenetrationStatus.SelectedValue.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

