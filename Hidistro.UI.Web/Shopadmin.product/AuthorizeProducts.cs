namespace Hidistro.UI.Web.Shopadmin.product
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
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AuthorizeProducts : DistributorPage
    {
        protected Button btnSearch;
        protected Grid grdAuthorizeProducts;
        protected PageSize hrefPageSize;
        protected HtmlInputCheckBox isDownCategory;
        private int? lineId;
        protected LinkButton lkbtnDownloadCheck;
        protected LinkButton lkbtnDownloadCheck1;
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
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(entity, true);
            this.grdAuthorizeProducts.DataSource = authorizeProducts.Data;
            this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = authorizeProducts.TotalRecords;
            this.pager1.TotalRecords = authorizeProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBindData(true);
        }

        private void grdAuthorizeProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
            int productId = (int) this.grdAuthorizeProducts.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "download")
            {
                SubSiteProducthelper.DownloadProduct(productId, this.isDownCategory.Checked);
                this.ReBindData(false);
            }
        }

        private void lkbtnDownloadCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in this.grdAuthorizeProducts.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    int productId = (int) this.grdAuthorizeProducts.DataKeys[row.RowIndex].Value;
                    SubSiteProducthelper.DownloadProduct(productId, this.isDownCategory.Checked);
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要下载的商品", false);
            }
            else
            {
                this.ReBindData(false);
            }
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
            this.grdAuthorizeProducts.RowCommand += new GridViewCommandEventHandler(this.grdAuthorizeProducts_RowCommand);
            this.lkbtnDownloadCheck.Click += new EventHandler(this.lkbtnDownloadCheck_Click);
            this.lkbtnDownloadCheck1.Click += new EventHandler(this.lkbtnDownloadCheck_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

