namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyProductUnclassified : DistributorPage
    {
        protected ImageLinkButton btnDelete;
        protected Button btnMove;
        protected Button btnSearch;
        private int? classId;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected DistributorProductCategoriesDropDownList dropMoveToCategories;
        protected Grid grdProducts;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string productcode;
        private string searchkey;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = this.searchkey;
            query.PageSize = this.pager.PageSize;
            query.ProductCode = this.productcode;
            if (this.dropCategories.SelectedValue.HasValue)
            {
                query.CategoryId = new int?(this.dropCategories.SelectedValue.Value);
                if (this.dropCategories.SelectedValue.Value != 0)
                {
                    query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(query.CategoryId.Value).Path;
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult unclassifiedProducts = SubSiteProducthelper.GetUnclassifiedProducts(query);
            this.grdProducts.DataSource = unclassifiedProducts.Data;
            this.grdProducts.DataBind();
            this.pager.TotalRecords = unclassifiedProducts.TotalRecords;
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
                int num = SubSiteProducthelper.DeleteProducts(str);
                if (num > 0)
                {
                    this.ShowMsg(string.Format("成功删除了选择的{0}件商品", num), true);
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
                    SubSiteProducthelper.UpdateProductCategory(Convert.ToInt32(str2), newCategoryId);
                }
                this.dropCategories.SelectedValue = new int?(newCategoryId);
                this.BindProducts();
                this.ShowMsg("转移商品类型成功", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("productCode", this.txtSKU.Text);
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("classId", this.dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        private void dropAddToCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList) sender;
            GridViewRow namingContainer = (GridViewRow) list.NamingContainer;
            if (list.SelectedValue.HasValue)
            {
                SubsiteCatalogHelper.SetProductExtendCategory((int) this.grdProducts.DataKeys[namingContainer.RowIndex].Value, SubsiteCatalogHelper.GetCategory(list.SelectedValue.Value).Path + "|");
                this.ReBind();
            }
            else
            {
                SubsiteCatalogHelper.SetProductExtendCategory((int) this.grdProducts.DataKeys[namingContainer.RowIndex].Value, null);
                this.ReBind();
            }
        }

        private void grdProducts_ReBindData(object sender)
        {
            this.ReBind();
        }

        private void grdProducts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList) e.Row.FindControl("dropAddToCategories");
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
                    literal.Text = SubsiteCatalogHelper.GetFullCategory((int) obj2);
                }
                DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList) e.Row.FindControl("dropAddToCategories");
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
                        literal2.Text = SubsiteCatalogHelper.GetFullCategory(int.Parse(s));
                        list.SelectedValue = new int?(int.Parse(s));
                    }
                }
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropMoveToCategories.DataBind();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["classId"]))
                {
                    int result = 0;
                    if (int.TryParse(this.Page.Request.QueryString["classId"], out result))
                    {
                        this.classId = new int?(result);
                    }
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchkey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productcode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                this.dropCategories.SelectedValue = this.classId;
                this.txtSearchText.Text = this.searchkey;
                this.txtSKU.Text = this.productcode;
            }
            else
            {
                this.searchkey = this.txtSearchText.Text;
                this.classId = this.dropCategories.SelectedValue;
                this.productcode = this.txtSKU.Text;
            }
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReBind()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("productCode", this.txtSKU.Text);
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("classId", this.dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }
    }
}

