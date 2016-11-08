namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
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

    [PrivilegeCheck(Privilege.BalanceDrawRequest)]
    public class BalanceDrawRequest : AdminPage
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
        protected HtmlGenericControl spanRemark;
        protected TextBox txtUserName;

        public void BindBalanceDrawRequest()
        {
            BalanceDrawRequestQuery query = new BalanceDrawRequestQuery();
            query.FromDate = this.dataStart;
            query.ToDate = this.dataEnd;
            query.UserName = this.txtUserName.Text;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            DbQueryResult balanceDrawRequests = MemberHelper.GetBalanceDrawRequests(query);
            this.grdBalanceDrawRequest.DataSource = balanceDrawRequests.Data;
            this.grdBalanceDrawRequest.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = balanceDrawRequests.TotalRecords;
            this.pager.TotalRecords = this.pager.TotalRecords = balanceDrawRequests.TotalRecords;
        }

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void grdBalanceDrawRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
            int userId = (int) this.grdBalanceDrawRequest.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "UnLineReCharge")
            {
                if (MemberHelper.DealBalanceDrawRequest(userId, true))
                {
                    this.BindBalanceDrawRequest();
                }
                else
                {
                    this.ShowMsg("预付款提现申请操作失败", false);
                }
            }
            if (e.CommandName == "RefuseRequest")
            {
                if (MemberHelper.DealBalanceDrawRequest(userId, false))
                {
                    this.BindBalanceDrawRequest();
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
                this.txtUserName.Text = this.searchKey;
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
                int num;
                if (int.TryParse(this.Page.Request.QueryString["userId"], out num))
                {
                    Member member = MemberHelper.GetMember(num);
                    if (member != null)
                    {
                        this.txtUserName.Text = member.Username;
                    }
                }
                this.BindBalanceDrawRequest();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", this.txtUserName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
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

