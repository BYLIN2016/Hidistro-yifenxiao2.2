namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
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

    [PrivilegeCheck(Privilege.OrderStatistics)]
    public class OrderStatistics : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Label lblPageCount;
        protected Label lblSearchCount;
        private string orderId;
        protected Pager pager;
        protected Repeater rp_orderStatistics;
        private string shipTo;
        protected TextBox txtOrderId;
        protected TextBox txtShipTo;
        protected TextBox txtUserName;
        private string userName;

        private void BindUserOrderStatistics()
        {
            UserOrderQuery userOrder = new UserOrderQuery();
            userOrder.UserName = this.userName;
            userOrder.ShipTo = this.shipTo;
            userOrder.StartDate = this.dateStart;
            userOrder.EndDate = this.dateEnd;
            userOrder.OrderId = this.orderId;
            userOrder.PageSize = this.pager.PageSize;
            userOrder.PageIndex = this.pager.PageIndex;
            userOrder.SortBy = "OrderDate";
            userOrder.SortOrder = SortAction.Desc;
            OrderStatisticsInfo userOrders = SalesHelper.GetUserOrders(userOrder);
            this.rp_orderStatistics.DataSource = userOrders.OrderTbl;
            this.rp_orderStatistics.DataBind();
            this.pager.TotalRecords = userOrders.TotalCount;
            this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
            this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            UserOrderQuery userOrder = new UserOrderQuery();
            userOrder.UserName = this.userName;
            userOrder.ShipTo = this.shipTo;
            userOrder.StartDate = this.dateStart;
            userOrder.EndDate = this.dateEnd;
            userOrder.OrderId = this.orderId;
            userOrder.PageSize = this.pager.PageSize;
            userOrder.PageIndex = this.pager.PageIndex;
            userOrder.SortBy = "OrderDate";
            userOrder.SortOrder = SortAction.Desc;
            OrderStatisticsInfo userOrdersNoPage = SalesHelper.GetUserOrdersNoPage(userOrder);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            builder.AppendLine("<td>订单号</td>");
            builder.AppendLine("<td>下单时间</td>");
            builder.AppendLine("<td>总订单金额</td>");
            builder.AppendLine("<td>用户名</td>");
            builder.AppendLine("<td>收货人</td>");
            builder.AppendLine("<td>利润</td>");
            builder.AppendLine("</tr>");
            foreach (DataRow row in userOrdersNoPage.OrderTbl.Rows)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["OrderDate"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Total"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["UserName"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["ShipTo"].ToString() + "</td>");
                builder.AppendLine("<td>" + row["Profits"].ToString() + "</td>");
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("<tr>");
            builder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
            builder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
            builder.AppendLine("<td>订单毛利润共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
            builder.AppendLine("<td></td>");
            builder.AppendLine("</tr>");
            builder.AppendLine("</table>");
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=UserOrderStatistics.xls");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/ms-excel";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(builder.ToString());
            this.Page.Response.End();
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
                {
                    this.shipTo = base.Server.UrlDecode(this.Page.Request.QueryString["shipTo"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
                {
                    this.orderId = Globals.UrlDecode(this.Page.Request.QueryString["orderId"]);
                }
                this.txtUserName.Text = this.userName;
                this.txtShipTo.Text = this.shipTo;
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
                this.txtOrderId.Text = this.orderId;
            }
            else
            {
                this.userName = this.txtUserName.Text;
                this.shipTo = this.txtShipTo.Text;
                this.dateStart = this.calendarStartDate.SelectedDate;
                this.dateEnd = this.calendarEndDate.SelectedDate;
                this.orderId = this.txtOrderId.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindUserOrderStatistics();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtUserName.Text);
            queryStrings.Add("shipTo", this.txtShipTo.Text);
            queryStrings.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("orderId", this.txtOrderId.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected void rp_orderStatistics_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater repeater = (Repeater) e.Item.FindControl("rp_orderitem");
            string orderId = ((DataRowView) e.Item.DataItem)["OrderId"].ToString();
            if (orderId != "")
            {
                repeater.DataSource = OrderHelper.GetLineItemInfo(orderId);
                repeater.DataBind();
            }
        }
    }
}

