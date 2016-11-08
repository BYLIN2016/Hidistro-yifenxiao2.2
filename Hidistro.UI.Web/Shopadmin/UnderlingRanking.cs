namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;

    public class UnderlingRanking : DistributorPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected DropDownList ddlSort;
        protected Grid grdProductSaleStatistics;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string sortBy = "SaleTotals";

        private void BindUnderlingRanking()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = this.dateStart;
            query.EndDate = this.dateEnd;
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            query.SortBy = this.sortBy;
            query.SortOrder = SortAction.Desc;
            int total = 0;
            DataTable underlingStatistics = UnderlingHelper.GetUnderlingStatistics(query, out total);
            this.grdProductSaleStatistics.DataSource = underlingStatistics;
            this.grdProductSaleStatistics.DataBind();
            this.calendarStartDate.SelectedDate = query.StartDate;
            this.calendarEndDate.SelectedDate = query.EndDate;
            this.pager.TotalRecords = total;
            this.pager1.TotalRecords = total;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.StartDate = this.dateStart;
            query.EndDate = this.dateEnd;
            query.SortBy = this.sortBy;
            query.SortOrder = SortAction.Desc;
            DataTable underlingStatisticsNoPage = UnderlingHelper.GetUnderlingStatisticsNoPage(query);
            string s = (string.Empty + "会员") + ",订单数" + ",消费金额\r\n";
            foreach (DataRow row in underlingStatisticsNoPage.Rows)
            {
                s = s + row["UserName"].ToString();
                s = s + "," + row["OrderCount"].ToString();
                s = s + "," + row["SaleTotals"].ToString() + "\r\n";
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberRanking.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadUnderlingRanking(true);
        }

        private void grdProductSaleStatistics_ReBindData(object sender)
        {
            this.ReloadUnderlingRanking(false);
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
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
                {
                    this.sortBy = base.Server.UrlDecode(this.Page.Request.QueryString["sortBy"]);
                }
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
                this.ddlSort.SelectedValue = this.sortBy;
            }
            else
            {
                this.dateStart = this.calendarStartDate.SelectedDate;
                this.dateEnd = this.calendarEndDate.SelectedDate;
                this.sortBy = this.ddlSort.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
            this.ddlSort.Items.Add(new ListItem("消费金额", "SaleTotals"));
            this.ddlSort.Items.Add(new ListItem("订单数", "OrderCount"));
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindUnderlingRanking();
            }
        }

        private void ReloadUnderlingRanking(bool isSeach)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("sortBy", this.ddlSort.SelectedValue);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSeach)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

