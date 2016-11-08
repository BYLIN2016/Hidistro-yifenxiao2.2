namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
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

    [PrivilegeCheck(Privilege.PurchaseOrderStatistics)]
    public class PurchaseOrderStatistics : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdPurchaseOrderStatistics;
        protected Label lblPageCount;
        protected Label lblSearchCount;
        protected Pager pager;
        protected TextBox txtUserName;
        private string userName;

        private void BindPurchaseOrderStatistics()
        {
            UserOrderQuery order = new UserOrderQuery();
            order.UserName = this.userName;
            order.StartDate = this.dateStart;
            order.EndDate = this.dateEnd;
            order.PageSize = 10;
            order.PageIndex = this.pager.PageIndex;
            order.SortBy = "PurchaseDate";
            order.SortOrder = SortAction.Desc;
            OrderStatisticsInfo purchaseOrders = DistributorHelper.GetPurchaseOrders(order);
            this.grdPurchaseOrderStatistics.DataSource = purchaseOrders.OrderTbl;
            this.grdPurchaseOrderStatistics.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalCount;
            this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", purchaseOrders.OrderTbl.Rows.Count, Globals.FormatMoney(purchaseOrders.TotalOfPage), Globals.FormatMoney(purchaseOrders.ProfitsOfPage));
            this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", purchaseOrders.TotalCount, Globals.FormatMoney(purchaseOrders.TotalOfSearch), Globals.FormatMoney(purchaseOrders.ProfitsOfSearch));
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            UserOrderQuery order = new UserOrderQuery();
            order.UserName = this.userName;
            order.StartDate = this.dateStart;
            order.EndDate = this.dateEnd;
            order.PageIndex = this.pager.PageIndex;
            order.SortBy = "PurchaseDate";
            order.SortOrder = SortAction.Desc;
            OrderStatisticsInfo purchaseOrdersNoPage = DistributorHelper.GetPurchaseOrdersNoPage(order);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            builder.AppendLine("<td>采购单号</td>");
            builder.AppendLine("<td>订单号</td>");
            builder.AppendLine("<td>下单时间</td>");
            builder.AppendLine("<td>分销商名称</td>");
            builder.AppendLine("<td>采购单金额</td>");
            builder.AppendLine("<td>利润</td>");
            builder.AppendLine("</tr>");
            foreach (DataRow row in purchaseOrdersNoPage.OrderTbl.Rows)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["PurchaseOrderId"].ToString() + "</td>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseDate"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Distributorname"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseTotal"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["PurchaseProfit"].ToString() + "</td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("<tr>");
            builder.AppendLine("<td>当前查询结果共计," + purchaseOrdersNoPage.TotalCount + "</td>");
            builder.AppendLine("<td>采购单金额共计," + purchaseOrdersNoPage.TotalOfSearch + "</td>");
            builder.AppendLine("<td>采购单毛利润共计," + purchaseOrdersNoPage.ProfitsOfSearch + "</td>");
            builder.AppendLine("<td></td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "UTF-8";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=PurchaseOrderStatistics.xls");
            this.Page.Response.ContentEncoding = Encoding.UTF8;
            this.Page.Response.ContentType = "application/ms-excel";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdPurchaseOrderStatistics_ReBindData(object sender)
        {
            this.ReBind(false);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["orderId"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
                }
                this.txtUserName.Text = this.userName;
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
            }
            else
            {
                this.userName = this.txtUserName.Text;
                this.dateStart = this.calendarStartDate.SelectedDate;
                this.dateEnd = this.calendarEndDate.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.grdPurchaseOrderStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdPurchaseOrderStatistics_ReBindData);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindPurchaseOrderStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtUserName.Text);
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

