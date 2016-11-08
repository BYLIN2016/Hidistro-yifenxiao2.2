namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductConsultationsManage)]
    public class ProductConsultations : AdminPage
    {
        protected Button btnSearch;
        private int? categoryId;
        protected ProductCategoriesDropDownList dropCategories;
        protected Grid grdConsultation;
        protected PageSize hrefPageSize;
        private string keywords = string.Empty;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindConsultation()
        {
            ProductConsultationAndReplyQuery entity = new ProductConsultationAndReplyQuery();
            entity.Keywords = this.keywords;
            entity.CategoryId = this.categoryId;
            entity.ProductCode = this.productCode;
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "ReplyDate";
            entity.Type = ConsultationReplyType.NoReply;
            Globals.EntityCoding(entity, true);
            DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(entity);
            this.grdConsultation.DataSource = consultationProducts.Data;
            this.grdConsultation.DataBind();
            this.pager.TotalRecords = consultationProducts.TotalRecords;
            this.pager1.TotalRecords = consultationProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductConsultations(true);
        }

        private void grdConsultation_ReBindData(object sender)
        {
            this.BindConsultation();
        }

        private void grdConsultation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int consultationId = (int) this.grdConsultation.DataKeys[e.RowIndex].Value;
            if (ProductCommentHelper.DeleteProductConsultation(consultationId) > 0)
            {
                this.ShowMsg("成功删除了选择的商品咨询", true);
                this.BindConsultation();
            }
            else
            {
                this.ShowMsg("删除商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetSearchControl();
            this.grdConsultation.RowDeleting += new GridViewDeleteEventHandler(this.grdConsultation_RowDeleting);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdConsultation.ReBindData += new Grid.ReBindDataEventHandler(this.grdConsultation_ReBindData);
        }

        private void ReloadProductConsultations(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", this.txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = this.Page.Request.QueryString["Keywords"];
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = this.Page.Request.QueryString["productCode"];
                }
                this.txtSearchText.Text = this.keywords;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
                this.BindConsultation();
            }
            else
            {
                this.keywords = this.txtSearchText.Text.Trim();
                this.productCode = this.txtSKU.Text.Trim();
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }
    }
}

