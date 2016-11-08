namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;

    public class ProductConsultationsAndReplay : HtmlTemplatedWebControl
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
                PageTitle.AddSiteNameTitle("商品咨询", HiContext.Current.Context);
                this.BindData();
            }
        }

        private void BindData()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            page.PageSize = page.PageSize;
            DbQueryResult productConsultations = ProductBrowser.GetProductConsultations(page, this.productId);
            this.rptRecords.DataSource = productConsultations.Data;
            this.rptRecords.DataBind();
            this.pager.TotalRecords = productConsultations.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductConsultationsAndReplay.html";
            }
            base.OnInit(e);
        }
    }
}

