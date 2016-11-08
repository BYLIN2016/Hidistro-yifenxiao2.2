namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductSaleRanking)]
    public class ProductSaleRanking : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdProductSaleStatistics;
        protected Pager pager;

        private void BindProductSaleRanking()
        {
            SaleStatisticsQuery productSale = new SaleStatisticsQuery();
            productSale.StartDate = this.dateStart;
            productSale.EndDate = this.dateEnd;
            productSale.PageSize = this.pager.PageSize;
            productSale.PageIndex = this.pager.PageIndex;
            productSale.SortBy = "ProductSaleCounts";
            productSale.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productSales = SalesHelper.GetProductSales(productSale, out totalProductSales);
            this.grdProductSaleStatistics.DataSource = productSales;
            this.grdProductSaleStatistics.DataBind();
            this.pager.TotalRecords = totalProductSales;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery productSale = new SaleStatisticsQuery();
            productSale.StartDate = this.dateStart;
            productSale.EndDate = this.dateEnd;
            productSale.PageSize = this.pager.PageSize;
            productSale.SortBy = "ProductSaleCounts";
            productSale.SortOrder = SortAction.Desc;
            int totalProductSales = 0;
            DataTable productSalesNoPage = SalesHelper.GetProductSalesNoPage(productSale, out totalProductSales);
            string s = ((string.Empty + "排行" + ",商品名称") + ",商家编码" + ",销售量") + ",销售额" + ",利润\r\n";
            foreach (DataRow row in productSalesNoPage.Rows)
            {
                s = s + row["IDOfSaleTotals"].ToString();
                s = s + "," + row["ProductName"].ToString();
                s = s + "," + row["SKU"].ToString();
                s = s + "," + row["ProductSaleCounts"].ToString();
                s = s + "," + row["ProductSaleTotals"].ToString();
                s = s + "," + row["ProductProfitsTotals"].ToString() + "\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ProductSaleRanking.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
                }
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
            }
            else
            {
                this.dateStart = this.calendarStartDate.SelectedDate;
                this.dateEnd = this.calendarEndDate.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindProductSaleRanking();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

