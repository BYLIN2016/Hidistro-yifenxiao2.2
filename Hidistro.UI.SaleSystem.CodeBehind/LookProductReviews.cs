namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;

    public class LookProductReviews : HtmlTemplatedWebControl
    {
        private Pager pager;
        private int productId;
        private ThemedTemplatedRepeater rptRecords;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.rptRecords = (ThemedTemplatedRepeater) this.FindControl("rptRecords");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("商品评论", HiContext.Current.Context);
                this.BindData();
            }
        }

        private void BindData()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            page.PageSize = this.pager.PageSize;
            DbQueryResult productReviews = ProductBrowser.GetProductReviews(page, this.productId);
            this.rptRecords.DataSource = productReviews.Data;
            this.rptRecords.DataBind();
            this.pager.TotalRecords = productReviews.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-LookProductReviews.html";
            }
            base.OnInit(e);
        }

        private void ReBind()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }
    }
}

