namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;

    public class OnlineGifts : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptGifts;

        protected override void AttachChildControls()
        {
            this.rptGifts = (ThemedTemplatedRepeater) this.FindControl("rptGifts");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindGift();
            }
        }

        private void BindGift()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            page.PageSize = this.pager.PageSize;
            DbQueryResult onlineGifts = ProductBrowser.GetOnlineGifts(page);
            this.rptGifts.DataSource = onlineGifts.Data;
            this.rptGifts.DataBind();
            this.pager.TotalRecords = onlineGifts.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-OnlineGifts.html";
            }
            base.OnInit(e);
        }
    }
}

