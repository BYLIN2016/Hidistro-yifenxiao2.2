namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UnderlingBalanceDetails : DistributorPage
    {
        protected Button btnQueryBalanceDetails;
        protected WebCalendar calendarEnd;
        protected WebCalendar calendarStart;
        private DateTime? dataEnd;
        private DateTime? dataStart;
        protected TradeTypeDropDownList dropTradeType;
        protected Grid grdBalanceDetails;
        protected PageSize hrefPageSize;
        protected LinkButton lbtnDrawRequest;
        protected Literal litBalance;
        protected Literal litDrawBalance;
        protected Literal litUser;
        protected Literal litUserBalance;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlGenericControl spanRemark;
        private int typeId;
        private int userId;

        private void BindBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = this.dataStart;
            query.ToDate = this.dataEnd;
            query.TradeType = (TradeTypes) this.typeId;
            query.PageIndex = this.pager.PageIndex;
            query.UserId = new int?(this.userId);
            query.PageSize = this.pager.PageSize;
            DbQueryResult balanceDetails = UnderlingHelper.GetBalanceDetails(query);
            this.grdBalanceDetails.DataSource = balanceDetails.Data;
            this.grdBalanceDetails.DataBind();
            this.pager.TotalRecords = balanceDetails.TotalRecords;
            this.pager1.TotalRecords = balanceDetails.TotalRecords;
        }

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void lbtnDrawRequest_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("UnderlingBalanceDrawRequest.aspx?userId=" + this.userId);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userId"]))
                {
                    int.TryParse(this.Page.Request.QueryString["userId"], out this.userId);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
                {
                    int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
                {
                    this.dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
                {
                    this.dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
                }
                this.ViewState["UserId"] = this.userId;
                this.dropTradeType.DataBind();
                this.dropTradeType.SelectedValue = (TradeTypes) this.typeId;
                this.calendarStart.SelectedDate = this.dataStart;
                this.calendarEnd.SelectedDate = this.dataEnd;
            }
            else
            {
                this.userId = (int) this.ViewState["UserId"];
                this.typeId = (int) this.dropTradeType.SelectedValue;
                this.dataStart = this.calendarStart.SelectedDate;
                this.dataEnd = this.calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQueryBalanceDetails.Click += new EventHandler(this.btnQueryBalanceDetails_Click);
            this.lbtnDrawRequest.Click += new EventHandler(this.lbtnDrawRequest_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                if (this.userId != 0)
                {
                    Member member = UnderlingHelper.GetMember(this.userId);
                    if (member != null)
                    {
                        this.litBalance.Text = member.Balance.ToString("F2");
                        this.litDrawBalance.Text = member.RequestBalance.ToString("F2");
                        this.litUserBalance.Text = (member.Balance - member.RequestBalance).ToString("F2");
                        MemberGradeInfo memberGrade = UnderlingHelper.GetMemberGrade(member.GradeId);
                        if (memberGrade != null)
                        {
                            this.litUser.Text = member.Username + "(" + memberGrade.Name + ")";
                        }
                        else
                        {
                            this.litUser.Text = member.Username;
                        }
                    }
                }
                this.BindBalanceDetails();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userId", this.ViewState["UserId"].ToString());
            queryStrings.Add("typeId", ((int) this.dropTradeType.SelectedValue).ToString(CultureInfo.InvariantCulture));
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

