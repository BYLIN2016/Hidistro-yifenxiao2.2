namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorBalanceDrawRequest)]
    public class DistributorBalanceDrawRequest : AdminPage
    {
        protected Button btnQueryBalanceDrawRequest;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dataEnd;
        private DateTime? dataStart;
        protected Grid grdBalanceDrawRequest;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string searchKey;
        protected HtmlGenericControl spanaccountName;
        protected HtmlGenericControl spanBankName;
        protected HtmlGenericControl spanmerchantCode;
        protected TextBox txtUserName;
        private int? userId;

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        public void GetBalanceDrawRequest()
        {
            BalanceDrawRequestQuery query = new BalanceDrawRequestQuery();
            query.FromDate = this.dataStart;
            query.ToDate = this.dataEnd;
            query.UserName = this.txtUserName.Text;
            query.UserId = this.userId;
            query.PageIndex = this.pager.PageIndex;
            DbQueryResult distributorBalanceDrawRequests = DistributorHelper.GetDistributorBalanceDrawRequests(query);
            this.grdBalanceDrawRequest.DataSource = distributorBalanceDrawRequests.Data;
            this.grdBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = distributorBalanceDrawRequests.TotalRecords;
        }

        private void grdBalanceDrawRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
            int userId = (int) this.grdBalanceDrawRequest.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "UnLineReCharge")
            {
                if (DistributorHelper.DealDistributorBalanceDrawRequest(userId, true))
                {
                    this.GetBalanceDrawRequest();
                }
                else
                {
                    this.ShowMsg("预付款提现申请操作失败", false);
                }
            }
            if (e.CommandName == "RefuseRequest")
            {
                if (DistributorHelper.DealDistributorBalanceDrawRequest(userId, false))
                {
                    this.GetBalanceDrawRequest();
                }
                else
                {
                    this.ShowMsg("预付款提现申请操作失败", false);
                }
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                int num;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]) && int.TryParse(this.Page.Request.QueryString["userId"], out num))
                {
                    this.userId = new int?(num);
                    Distributor distributor = DistributorHelper.GetDistributor(this.userId.Value);
                    if (distributor != null)
                    {
                        this.txtUserName.Text = distributor.Username;
                    }
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
                {
                    this.dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
                {
                    this.dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
                }
                if (string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
                {
                    this.txtUserName.Text = this.searchKey;
                }
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
            }
            else
            {
                this.searchKey = this.txtUserName.Text;
                this.dataStart = this.calendarStart.SelectedDate;
                this.dataEnd = this.calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDrawRequest.Click += new EventHandler(this.btnQueryBalanceDrawRequest_Click);
            this.grdBalanceDrawRequest.RowCommand += new GridViewCommandEventHandler(this.grdBalanceDrawRequest_RowCommand);
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
            queryStrings.Add("searchKey", this.txtUserName.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("dataStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

