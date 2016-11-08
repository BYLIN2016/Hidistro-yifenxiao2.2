namespace Hidistro.UI.Web.Admin
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
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductUnclassified)]
    public class ProductUnclassified : AdminPage
    {
        protected ImageLinkButton btnDelete;
        protected Button btnMove;
        protected Button btnSearch;
        protected Button btnSetCategories;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private int? categoryId;
        protected ProductCategoriesDropDownList dropAddToAllCategories;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        protected ProductLineDropDownList dropLines;
        protected ProductCategoriesDropDownList dropMoveToCategories;
        protected ProductTypeDownList dropType;
        private DateTime? endDate;
        protected Grid grdProducts;
        protected PageSize hrefPageSize;
        private int? lineId;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        private string searchkey;
        private DateTime? startDate;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;
        private int? typeId;

        private void BindProducts()
        {
            this.LoadParameters();
            ProductQuery query2 = new ProductQuery();
            query2.Keywords = this.searchkey;
            query2.ProductCode = this.productCode;
            query2.ProductLineId = this.lineId;
            query2.PageSize = this.pager.PageSize;
            query2.CategoryId = this.categoryId;
            query2.StartDate = this.startDate;
            query2.EndDate = this.endDate;
            query2.PageIndex = this.pager.PageIndex;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "DisplaySequence";
            query2.BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null;
            query2.TypeId = this.typeId;
            ProductQuery query = query2;
            if (this.categoryId.HasValue && (this.categoryId.Value != 0))
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(query.CategoryId.Value).Path;
            }
            DbQueryResult unclassifiedProducts = ProductHelper.GetUnclassifiedProducts(query);
            this.grdProducts.DataSource = unclassifiedProducts.Data;
            this.grdProducts.DataBind();
            this.dropType.SelectedValue = query.TypeId;
            this.pager1.TotalRecords = this.pager.TotalRecords = unclassifiedProducts.TotalRecords;
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
                int num = ProductHelper.RemoveProduct(str);
                if (num > 0)
                {
                    this.ShowMsg(string.Format("成功删除了选择的商品", num), true);
                    this.BindProducts();
                }
                else
                {
                    this.ShowMsg("删除商品失败，未知错误", false);
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            int newCategoryId = this.dropMoveToCategories.SelectedValue.HasValue ? this.dropMoveToCategories.SelectedValue.Value : 0;
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请选择要转移的商品", false);
            }
            else
            {
                string[] strArray;
                if (!str.Contains(","))
                {
                    strArray = new string[] { str };
                }
                else
                {
                    strArray = str.Split(new char[] { ',' });
                }
                foreach (string str2 in strArray)
                {
                    ProductHelper.UpdateProductCategory(Convert.ToInt32(str2), newCategoryId);
                }
                this.dropCategories.SelectedValue = new int?(newCategoryId);
                this.ReBind(false);
                this.ShowMsg("转移商品类型成功", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void btnSetCategories_Click(object sender, EventArgs e)
        {
            if (this.dropMoveToCategories.SelectedValue.HasValue)
            {
                int local1 = this.dropMoveToCategories.SelectedValue.Value;
            }
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请选择要设置的商品", false);
            }
            else
            {
                string[] strArray;
                if (!str.Contains(","))
                {
                    strArray = new string[] { str };
                }
                else
                {
                    strArray = str.Split(new char[] { ',' });
                }
                foreach (string str2 in strArray)
                {
                    if (this.dropAddToAllCategories.SelectedValue.HasValue)
                    {
                        CatalogHelper.SetProductExtendCategory(Convert.ToInt32(str2), CatalogHelper.GetCategory(this.dropAddToAllCategories.SelectedValue.Value).Path + "|");
                    }
                    else
                    {
                        CatalogHelper.SetProductExtendCategory(Convert.ToInt32(str2), null);
                    }
                }
                this.ReBind(false);
                this.ShowMsg("批量设置扩展分类成功", true);
            }
        }

        private void dropAddToCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductCategoriesDropDownList list = (ProductCategoriesDropDownList) sender;
            GridViewRow namingContainer = (GridViewRow) list.NamingContainer;
            if (list.SelectedValue.HasValue)
            {
                CatalogHelper.SetProductExtendCategory((int) this.grdProducts.DataKeys[namingContainer.RowIndex].Value, CatalogHelper.GetCategory(list.SelectedValue.Value).Path + "|");
                this.ReBind(false);
            }
            else
            {
                CatalogHelper.SetProductExtendCategory((int) this.grdProducts.DataKeys[namingContainer.RowIndex].Value, null);
                this.ReBind(false);
            }
        }

        private void grdProducts_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void grdProducts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductCategoriesDropDownList list = (ProductCategoriesDropDownList) e.Row.FindControl("dropAddToCategories");
                list.SelectedIndexChanged += new EventHandler(this.dropAddToCategories_SelectedIndexChanged);
            }
        }

        private void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal) e.Row.FindControl("litMainCategory");
                literal.Text = "-";
                object obj2 = DataBinder.Eval(e.Row.DataItem, "CategoryId");
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    literal.Text = CatalogHelper.GetFullCategory((int) obj2);
                }
                ProductCategoriesDropDownList list = (ProductCategoriesDropDownList) e.Row.FindControl("dropAddToCategories");
                list.DataBind();
                Literal literal2 = (Literal) e.Row.FindControl("litExtendCategory");
                literal2.Text = "-";
                object obj3 = DataBinder.Eval(e.Row.DataItem, "ExtendCategoryPath");
                if ((obj3 != null) && (obj3 != DBNull.Value))
                {
                    string s = (string) obj3;
                    if (s.Length > 0)
                    {
                        s = s.Substring(0, s.Length - 1);
                        if (s.Contains("|"))
                        {
                            s = s.Substring(s.LastIndexOf('|') + 1);
                        }
                        literal2.Text = CatalogHelper.GetFullCategory(int.Parse(s));
                        list.SelectedValue = new int?(int.Parse(s));
                    }
                }
            }
        }

        private void LoadParameters()
        {
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
            {
                this.categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(this.Page.Request.QueryString["typeId"], out num2))
            {
                this.typeId = new int?(num2);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            int num3 = 0;
            if (int.TryParse(this.Page.Request.QueryString["brandId"], out num3))
            {
                this.dropBrandList.SelectedValue = new int?(num3);
            }
            int num4 = 0;
            if (int.TryParse(this.Page.Request.QueryString["lineId"], out num4))
            {
                this.lineId = new int?(num4);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
            {
                this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                this.startDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                this.endDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            this.dropCategories.DataBind();
            this.dropCategories.SelectedValue = this.categoryId;
            this.dropType.DataBind();
            this.dropType.SelectedValue = this.typeId;
            this.txtSearchText.Text = this.searchkey;
            this.txtSKU.Text = this.productCode;
            this.dropLines.DataBind();
            this.dropLines.SelectedValue = this.lineId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
            this.grdProducts.RowCreated += new GridViewRowEventHandler(this.grdProducts_RowCreated);
            this.grdProducts.RowDataBound += new GridViewRowEventHandler(this.grdProducts_RowDataBound);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnMove.Click += new EventHandler(this.btnMove_Click);
            this.btnSetCategories.Click += new EventHandler(this.btnSetCategories_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.dropBrandList.DataBind();
                this.dropLines.DataBind();
                this.dropMoveToCategories.DataBind();
                this.BindProducts();
                this.dropAddToAllCategories.DataBind();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", this.dropCategories.SelectedValue.ToString());
            }
            if (this.dropType.SelectedValue.HasValue)
            {
                queryStrings.Add("typeId", this.dropType.SelectedValue.ToString());
            }
            if (this.dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", this.dropLines.SelectedValue.ToString());
            }
            if (this.dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", this.dropBrandList.SelectedValue.ToString());
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
            {
                queryStrings.Add("productCode", this.txtSKU.Text.Trim());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
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

