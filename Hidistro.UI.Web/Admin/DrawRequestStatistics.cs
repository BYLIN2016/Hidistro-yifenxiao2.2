namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
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

    [PrivilegeCheck(Privilege.MemberDrawRequestStatistics)]
    public class DrawRequestStatistics : AdminPage
    {
        protected LinkButton btnCreateReport;
        protected Button btnQueryBalanceDrawRequest;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdBalanceDrawRequest;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtUserName;
        private string userName;

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.UserName = this.userName;
            query.FromDate = this.dateStart;
            query.ToDate = this.dateEnd;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.TradeType = TradeTypes.DrawRequest;
            DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(query);
            string s = ((string.Empty + "用户名") + ",交易时间" + ",业务摘要") + ",转出金额" + ",当前余额\r\n";
            foreach (DataRow row in ((DataTable) balanceDetailsNoPage.Data).Rows)
            {
                s = s + row["UserName"];
                s = s + "," + row["TradeDate"];
                s = s + ",提现";
                s = s + "," + row["Expenses"];
                object obj2 = s;
                s = string.Concat(new object[] { obj2, ",", row["Balance"], "\r\n" });
            }
            this.Page.Response.Clear();
            this.Page.Response.Buffer = false;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.EnableViewState = false;
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        public void GetBalanceDrawRequest()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.UserName = this.userName;
            query.FromDate = this.dateStart;
            query.ToDate = this.dateEnd;
            query.PageIndex = this.pager.PageIndex;
            query.TradeType = TradeTypes.DrawRequest;
            DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
            this.grdBalanceDrawRequest.DataSource = balanceDetails.Data;
            this.grdBalanceDrawRequest.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = balanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
                {
                    this.dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
                {
                    this.dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
                }
                this.txtUserName.Text = this.userName;
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
            }
            else
            {
                this.userName = this.txtUserName.Text;
                this.dateStart = this.calendarStart.SelectedDate;
                this.dateEnd = this.calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDrawRequest.Click += new EventHandler(this.btnQueryBalanceDrawRequest_Click);
            this.btnCreateReport.Click += new EventHandler(this.btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.GetBalanceDrawRequest();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtUserName.Text);
            queryStrings.Add("dateStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

