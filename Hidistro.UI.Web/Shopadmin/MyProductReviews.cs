namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyProductReviews : DistributorPage
    {
        protected Button btnSearch;
        private int? categoryId;
        protected DataList dlstPtReviews;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected PageSize hrefPageSize;
        private string keywords = string.Empty;
        protected Pager pager;
        protected Pager pager1;
        private string productCode;
        protected TextBox txtSearchText;
        protected TextBox txtSKU;

        private void BindPtReview()
        {
            ProductReviewQuery entity = new ProductReviewQuery();
            entity.Keywords = this.keywords;
            entity.CategoryId = this.categoryId;
            entity.ProductCode = this.productCode;
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "ReviewDate";
            int total = 0;
            Globals.EntityCoding(entity, true);
            DataSet productReviews = SubsiteCommentsHelper.GetProductReviews(out total, entity);
            this.dlstPtReviews.DataSource = productReviews.Tables[0].DefaultView;
            this.dlstPtReviews.DataBind();
            this.pager.TotalRecords = total;
            this.pager1.TotalRecords = total;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductReviews(true);
        }

        private void dlstPtReviews_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (SubsiteCommentsHelper.DeleteProductReview((long) Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture)) > 0)
            {
                this.ShowMsg("成功删除了选择的商品评论回复", true);
                this.BindPtReview();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetSearchControl();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.dlstPtReviews.DeleteCommand += new DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
        }

        private void ReloadProductReviews(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", this.txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                this.txtSearchText.Text = this.keywords;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
                this.BindPtReview();
            }
            else
            {
                this.keywords = this.txtSearchText.Text;
                this.productCode = this.txtSKU.Text;
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }
    }
}

