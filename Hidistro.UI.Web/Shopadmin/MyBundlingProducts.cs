namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyBundlingProducts : DistributorPage
    {
        protected Button btnSearch;
        protected Grid grdBundlingList;
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        private string productName = string.Empty;
        protected TextBox txtProductName;

        private void BindBundlingProducts()
        {
            BundlingInfoQuery query = new BundlingInfoQuery();
            query.ProductName = this.productName;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult bundlingProducts = SubsitePromoteHelper.GetBundlingProducts(query);
            this.grdBundlingList.DataSource = bundlingProducts.Data;
            this.grdBundlingList.DataBind();
            this.pager.TotalRecords = bundlingProducts.TotalRecords;
            this.pager1.TotalRecords = bundlingProducts.TotalRecords;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadHelpList(true);
        }

        private void grdBundlingList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsitePromoteHelper.DeleteBundlingProduct((int) this.grdBundlingList.DataKeys[e.RowIndex].Value))
            {
                this.BindBundlingProducts();
                this.ShowMsg("成功删除了选择的捆绑商品！", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num2 = 0;
            foreach (GridViewRow row in this.grdBundlingList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    SubsitePromoteHelper.DeleteBundlingProduct(Convert.ToInt32(this.grdBundlingList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture));
                }
            }
            if (num2 != 0)
            {
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"个捆绑商品", new object[] { num2 }), true);
                this.BindBundlingProducts();
            }
            else
            {
                this.ShowMsg("请先选择需要删除的捆绑商品", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                this.txtProductName.Text = this.productName;
            }
            else
            {
                this.productName = this.txtProductName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindBundlingProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
            this.grdBundlingList.RowDeleting += new GridViewDeleteEventHandler(this.grdBundlingList_RowDeleting);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", this.grdBundlingList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

