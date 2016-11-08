namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorAccount)]
    public class DistributorBalanceDetails : AdminPage
    {
        protected Button btnQueryBalanceDetails;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dataEnd;
        private DateTime? dataStart;
        protected TradeTypeDropDownList ddlTradeType;
        protected Grid grdBalanceDetails;
        protected PageSize hrefPageSize;
        protected Literal litUser;
        protected Pager pager;
        protected Pager pager1;
        private TradeTypes tradeType;
        private int userId;

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void ddlTradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void GetBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = this.dataStart;
            query.ToDate = this.dataEnd;
            query.UserId = new int?(this.userId);
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.TradeType = this.tradeType;
            DbQueryResult distributorBalanceDetails = DistributorHelper.GetDistributorBalanceDetails(query);
            this.grdBalanceDetails.DataSource = distributorBalanceDetails.Data;
            this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = distributorBalanceDetails.TotalRecords;
            this.pager1.TotalRecords = distributorBalanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
                {
                    this.dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
                {
                    this.dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["tradeType"], out result);
                    this.tradeType = (TradeTypes) result;
                }
                this.ddlTradeType.DataBind();
                this.ddlTradeType.SelectedValue = this.tradeType;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
            }
            else
            {
                this.tradeType = this.ddlTradeType.SelectedValue;
                this.dataStart = this.calendarStart.SelectedDate;
                this.dataEnd = this.calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDetails.Click += new EventHandler(this.btnQueryBalanceDetails_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.LoadParameters();
                if (!base.IsPostBack)
                {
                    Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                    if (distributor == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litUser.Text = distributor.Username;
                        this.GetBalanceDetails();
                    }
                }
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userId", this.Page.Request.QueryString["userId"]);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("dataStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
            queryStrings.Add("tradeType", ((int) this.ddlTradeType.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

