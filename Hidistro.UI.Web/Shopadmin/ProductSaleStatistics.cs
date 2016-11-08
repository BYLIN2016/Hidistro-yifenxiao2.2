namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.HtmlControls;

    public class ProductSaleStatistics : DistributorPage
    {
        protected Grid grdProductSaleStatistics;
        protected HtmlInputHidden hidPageIndex;
        protected HtmlInputHidden hidPageSize;
        protected Pager pager;

        private void BindProductSaleStatistics()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            query.SortBy = "BuyPercentage";
            query.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productVisitAndBuyStatistics = SubsiteSalesHelper.GetProductVisitAndBuyStatistics(query, out totalProductSales);
            this.grdProductSaleStatistics.DataSource = productVisitAndBuyStatistics;
            this.grdProductSaleStatistics.DataBind();
            this.pager.TotalRecords = totalProductSales;
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindProductSaleStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

