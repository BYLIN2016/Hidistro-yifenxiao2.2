namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;

    public class LookLineItems : HtmlTemplatedWebControl
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
                PageTitle.AddSiteNameTitle("商品成交记录", HiContext.Current.Context);
                this.BindData();
            }
        }

        private void BindData()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            page.PageSize = this.pager.PageSize;
            DbQueryResult lineItems = ProductBrowser.GetLineItems(page, this.productId);
            DataTable data = lineItems.Data as DataTable;
            foreach (DataRow row in data.Rows)
            {
                string str = (string) row["Username"];
                if (str.ToLower() == "anonymous")
                {
                    row["Username"] = "匿名用户";
                }
                else
                {
                    row["Username"] = str.Substring(0, 1) + "**" + str.Substring(str.Length - 1);
                }
            }
            this.rptRecords.DataSource = data;
            this.rptRecords.DataBind();
            this.pager.TotalRecords = lineItems.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-LookLineItems.html";
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

