namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberBalanceStatistics)]
    public class DistributionBalanceStatistics : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnQueryBalanceDetails;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdBalanceDetails;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtUserName;
        private string userName;

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = this.dateStart;
            query.ToDate = this.dateEnd;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.UserName = this.userName;
            DbQueryResult distributorBalanceDetailsNoPage = DistributorHelper.GetDistributorBalanceDetailsNoPage(query);
            string s = ((string.Empty + "用户名" + ",交易时间") + ",业务摘要" + ",转入金额") + ",转出金额" + ",当前余额\r\n";
            foreach (DataRow row in ((DataTable) distributorBalanceDetailsNoPage.Data).Rows)
            {
                string str2 = string.Empty;
                switch (Convert.ToInt32(row["TradeType"]))
                {
                    case 1:
                        str2 = "自助充值";
                        break;

                    case 2:
                        str2 = "后台加款";
                        break;

                    case 3:
                        str2 = "消费";
                        break;

                    case 4:
                        str2 = "提现";
                        break;

                    case 5:
                        str2 = "订单退款";
                        break;

                    default:
                        str2 = "其他";
                        break;
                }
                s = s + row["UserName"];
                s = s + "," + row["TradeDate"];
                s = s + "," + str2;
                s = s + "," + row["Income"];
                s = s + "," + row["Expenses"];
                object obj2 = s;
                s = string.Concat(new object[] { obj2, ",", row["Balance"], "\r\n" });
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributionBalanceDetailsStatistics.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void GetDistributionBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = this.dateStart;
            query.ToDate = this.dateEnd;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.UserName = this.userName;
            DbQueryResult distributorBalanceDetails = DistributorHelper.GetDistributorBalanceDetails(query);
            this.grdBalanceDetails.DataSource = distributorBalanceDetails.Data;
            this.grdBalanceDetails.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = distributorBalanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = this.Page.Request.QueryString["userName"].ToString();
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
                }
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
                this.txtUserName.Text = this.userName;
            }
            else
            {
                this.dateStart = this.calendarStart.SelectedDate;
                this.dateEnd = this.calendarEnd.SelectedDate;
                this.userName = this.txtUserName.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDetails.Click += new EventHandler(this.btnQueryBalanceDetails_Click);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.GetDistributionBalanceDetails();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.userName);
            queryStrings.Add("dateStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

