namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;

    public class Promotes : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptPromoteSales;

        protected override void AttachChildControls()
        {
            this.rptPromoteSales = (ThemedTemplatedRepeater) this.FindControl("rptPromoteSales");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("优惠活动中心", HiContext.Current.Context);
                this.BindPromoteSales();
            }
        }

        private void BindPromoteSales()
        {
            int num2;
            int promotiontype = 0;
            if (int.TryParse(this.Page.Request.QueryString["promoteType"], out num2))
            {
                promotiontype = num2;
            }
            Pagination pagination = new Pagination();
            pagination.PageIndex = this.pager.PageIndex;
            pagination.PageSize = this.pager.PageSize;
            int totalPromotes = 0;
            DataTable table = CommentBrowser.GetPromotes(pagination, promotiontype, out totalPromotes);
            if ((table != null) && (table.Rows.Count > 0))
            {
                this.rptPromoteSales.DataSource = table;
                this.rptPromoteSales.DataBind();
            }
            this.pager.TotalRecords = totalPromotes;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Promotes.html";
            }
            base.OnInit(e);
        }
    }
}

