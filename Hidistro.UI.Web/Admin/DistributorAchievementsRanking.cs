namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorAchievementsRanking)]
    public class DistributorAchievementsRanking : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdDistributorStatistics;
        protected FormatedMoneyLabel lblProfitTotal;
        protected FormatedMoneyLabel lblTotal;
        protected Pager pager;

        private void BindDistributorRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = this.dateStart;
            query.EndDate = this.dateEnd;
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            query.SortBy = "SaleTotals";
            query.SortOrder = SortAction.Desc;
            int totalDistributors = 0;
            OrderStatisticsInfo distributorStatistics = DistributorHelper.GetDistributorStatistics(query, out totalDistributors);
            this.grdDistributorStatistics.DataSource = distributorStatistics.OrderTbl;
            this.grdDistributorStatistics.DataBind();
            this.lblTotal.Money = distributorStatistics.TotalOfSearch;
            this.lblProfitTotal.Money = distributorStatistics.ProfitsOfSearch;
            this.pager.TotalRecords = totalDistributors;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = this.dateStart;
            query.EndDate = this.dateEnd;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "SaleTotals";
            query.SortOrder = SortAction.Desc;
            DataTable orderTbl = DistributorHelper.GetDistributorStatisticsNoPage(query).OrderTbl;
            string s = string.Empty + "排行,分销商名称,交易量,交易金额,利润\r\n";
            foreach (DataRow row in orderTbl.Rows)
            {
                if (Convert.ToDecimal(row["SaleTotals"]) > 0M)
                {
                    s = s + row["IndexId"].ToString();
                }
                else
                {
                    s = s ?? "";
                }
                s = s + "," + row["UserName"].ToString();
                s = s + "," + row["PurchaseOrderCount"].ToString();
                s = s + "," + row["SaleTotals"].ToString();
                s = s + "," + row["Profits"].ToString();
                s = s + "\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorRanking.CSV");
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

        private void grdDistributorStatistics_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["dateEnd"]));
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
            this.grdDistributorStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdDistributorStatistics_ReBindData);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindDistributorRanking();
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

