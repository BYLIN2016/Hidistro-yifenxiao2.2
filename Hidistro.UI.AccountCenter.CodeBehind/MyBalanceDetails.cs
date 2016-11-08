namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web;

    public class MyBalanceDetails : MemberTemplatedWebControl
    {
        private Common_Advance_AccountList accountList;
        private IButton btnSearchBalanceDetails;
        private WebCalendar calendarEnd;
        private WebCalendar calendarStart;
        private TradeTypeDropDownList dropTradeType;
        private Pager pager;

        protected override void AttachChildControls()
        {
            this.accountList = (Common_Advance_AccountList) this.FindControl("Common_Advance_AccountList");
            this.pager = (Pager) this.FindControl("pager");
            this.calendarStart = (WebCalendar) this.FindControl("calendarStart");
            this.calendarEnd = (WebCalendar) this.FindControl("calendarEnd");
            this.dropTradeType = (TradeTypeDropDownList) this.FindControl("dropTradeType");
            this.btnSearchBalanceDetails = ButtonManager.Create(this.FindControl("btnSearchBalanceDetails"));
            PageTitle.AddSiteNameTitle("帐户明细", HiContext.Current.Context);
            this.btnSearchBalanceDetails.Click += new EventHandler(this.btnSearchBalanceDetails_Click);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
                this.BindBalanceDetails();
            }
        }

        private void BindBalanceDetails()
        {
            BalanceDetailQuery balanceDetailQuery = this.GetBalanceDetailQuery();
            DbQueryResult balanceDetails = PersonalHelper.GetBalanceDetails(balanceDetailQuery);
            this.accountList.DataSource = balanceDetails.Data;
            this.accountList.DataBind();
            this.dropTradeType.DataBind();
            this.dropTradeType.SelectedValue = balanceDetailQuery.TradeType;
            this.calendarStart.SelectedDate = balanceDetailQuery.FromDate;
            this.calendarEnd.SelectedDate = balanceDetailQuery.ToDate;
            this.pager.TotalRecords = balanceDetails.TotalRecords;
        }

        private void btnSearchBalanceDetails_Click(object sender, EventArgs e)
        {
            this.ReloadMyBalanceDetails(true);
        }

        private BalanceDetailQuery GetBalanceDetailQuery()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.UserId = new int?(HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataStart"]))
            {
                query.FromDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataStart"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dataEnd"]))
            {
                query.ToDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["dataEnd"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tradeType"]))
            {
                query.TradeType = (TradeTypes) Convert.ToInt32(this.Page.Server.UrlDecode(this.Page.Request.QueryString["tradeType"]));
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-MyBalanceDetails.html";
            }
            base.OnInit(e);
        }

        private void ReloadMyBalanceDetails(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("dataStart", this.calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", this.calendarEnd.SelectedDate.ToString());
            queryStrings.Add("tradeType", ((int) this.dropTradeType.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

