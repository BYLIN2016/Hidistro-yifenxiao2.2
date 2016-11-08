namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UnderlingBalanceDrawRequest : DistributorPage
    {
        protected Button btnQueryBalanceDrawRequest;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dateEnd;
        private DateTime? dateStart;
        protected Grid grdBalanceDrawRequest;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlGenericControl spanaccountName;
        protected HtmlGenericControl spanBankName;
        protected HtmlGenericControl spanmerchantCode;
        protected HtmlGenericControl spanRemark;
        protected TextBox txtUserName;
        private int userId;
        private string userName;

        public void BindBalanceDrawRequest()
        {
            BalanceDrawRequestQuery query = new BalanceDrawRequestQuery();
            if (this.userId > 0)
            {
                query.UserId = new int?(this.userId);
            }
            query.UserName = this.userName;
            query.FromDate = this.dateStart;
            query.ToDate = this.dateEnd;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            DbQueryResult balanceDrawRequests = UnderlingHelper.GetBalanceDrawRequests(query);
            this.grdBalanceDrawRequest.DataSource = balanceDrawRequests.Data;
            this.grdBalanceDrawRequest.DataBind();
            this.pager.TotalRecords = balanceDrawRequests.TotalRecords;
            this.pager1.TotalRecords = balanceDrawRequests.TotalRecords;
        }

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            this.ReloadUnderlingBalanceDrawRequest(true);
        }

        private void GetBalanceDrawRequestQuery()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
                {
                    int.TryParse(this.Page.Request.QueryString["userId"], out this.userId);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
                {
                    this.dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
                {
                    this.dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
                }
                this.txtUserName.Text = this.userName;
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
            }
            else
            {
                this.userName = this.txtUserName.Text.Trim();
            }
        }

        private void grdBalanceDrawRequest_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
            int userId = (int) this.grdBalanceDrawRequest.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "UnLineReCharge")
            {
                if (UnderlingHelper.DealBalanceDrawRequest(userId, true))
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
                if (UnderlingHelper.DealBalanceDrawRequest(userId, false))
                {
                    this.BindBalanceDrawRequest();
                }
                else
                {
                    this.ShowMsg("预付款提现申请操作失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnQueryBalanceDrawRequest.Click += new EventHandler(this.btnQueryBalanceDrawRequest_Click);
            this.grdBalanceDrawRequest.RowCommand += new GridViewCommandEventHandler(this.grdBalanceDrawRequest_RowCommand);
            this.GetBalanceDrawRequestQuery();
            if (!this.Page.IsPostBack)
            {
                if (int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
                {
                    Member member = UnderlingHelper.GetMember(this.userId);
                    if (member != null)
                    {
                        this.txtUserName.Text = member.Username;
                    }
                }
                this.BindBalanceDrawRequest();
            }
        }

        private void ReloadUnderlingBalanceDrawRequest(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtUserName.Text);
            queryStrings.Add("pageSize", this.hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("dataStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

