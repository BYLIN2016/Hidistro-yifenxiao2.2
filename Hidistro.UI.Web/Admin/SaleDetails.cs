namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.SaleDetails)]
    public class SaleDetails : AdminPage
    {
        protected Button btnQuery;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? endTime;
        protected GridView grdOrderLineItem;
        protected Pager pager;
        private DateTime? startTime;

        private void BindList()
        {
            SaleStatisticsQuery query = new SaleStatisticsQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.StartDate = this.startTime;
            query.EndDate = this.endTime;
            DbQueryResult saleOrderLineItemsStatistics = SalesHelper.GetSaleOrderLineItemsStatistics(query);
            this.grdOrderLineItem.DataSource = saleOrderLineItemsStatistics.Data;
            this.grdOrderLineItem.DataBind();
            this.pager.TotalRecords = saleOrderLineItemsStatistics.TotalRecords;
            this.grdOrderLineItem.PageSize = query.PageSize;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
                {
                    this.startTime = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startTime"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
                {
                    this.endTime = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endTime"]));
                }
                this.calendarStart.SelectedDate = this.startTime;
                this.calendarEnd.SelectedDate = this.endTime;
            }
            else
            {
                this.startTime = this.calendarStart.SelectedDate;
                this.endTime = this.calendarEnd.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindList();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("startTime", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("endTime", this.calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

