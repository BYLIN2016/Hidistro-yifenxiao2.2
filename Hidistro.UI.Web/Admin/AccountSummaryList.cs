namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MemberAccount)]
    public class AccountSummaryList : AdminPage
    {
        protected Button btnAddBalance;
        protected Button btnQuery;
        protected HtmlInputHidden curentBalance;
        protected HtmlInputHidden currentUserId;
        protected Grid grdMemberAccountList;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        private string realName;
        private string searchKey;
        protected TextBox txtRealName;
        protected TextBox txtReCharge;
        protected TextBox txtRemark;
        protected TextBox txtUserName;

        private void btnAddBalance_Click(object sender, EventArgs e)
        {
            if (!RoleHelper.GetUserPrivileges(HiContext.Current.User.Username).Contains(0x232a) && (HiContext.Current.User.UserRole != UserRole.SiteManager))
            {
                this.ShowMsg("权限不够", false);
            }
            else
            {
                decimal num;
                int length = 0;
                if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
                {
                    length = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
                }
                int userId = int.Parse(this.currentUserId.Value);
                if (!decimal.TryParse(this.txtReCharge.Text.Trim(), out num) || (length > 2))
                {
                    this.ShowMsg("本次充值要给当前客户加款的金额只能是数值，且不能超过2位小数", false);
                }
                else if ((num < -10000000M) || (num > 10000000M))
                {
                    this.ShowMsg("金额大小必须在正负1000万之间", false);
                }
                else
                {
                    Member user = Users.GetUser(userId, false) as Member;
                    if ((user == null) || !user.IsOpenBalance)
                    {
                        this.ShowMsg("本次充值已失败，该用户不存在或预付款还没有开通", false);
                    }
                    else
                    {
                        decimal num4 = num + user.Balance;
                        BalanceDetailInfo target = new BalanceDetailInfo();
                        target.UserId = userId;
                        target.UserName = user.Username;
                        target.TradeDate = DateTime.Now;
                        target.TradeType = TradeTypes.BackgroundAddmoney;
                        target.Income = new decimal?(num);
                        target.Balance = num4;
                        target.Remark = Globals.HtmlEncode(this.txtRemark.Text.Trim());
                        ValidationResults results = Hishop.Components.Validation.Validation.Validate<BalanceDetailInfo>(target, new string[] { "ValBalanceDetail" });
                        string msg = string.Empty;
                        if (!results.IsValid)
                        {
                            foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                            {
                                msg = msg + Formatter.FormatErrorMessage(result.Message);
                            }
                            this.ShowMsg(msg, false);
                        }
                        else if (MemberHelper.AddBalance(target, num))
                        {
                            this.txtReCharge.Text = "";
                            this.ReBind(false);
                        }
                    }
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        public void GetBalance()
        {
            MemberQuery query = new MemberQuery();
            query.Username = this.searchKey;
            query.Realname = this.realName;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            DbQueryResult memberBlanceList = MemberHelper.GetMemberBlanceList(query);
            this.grdMemberAccountList.DataSource = memberBlanceList.Data;
            this.grdMemberAccountList.DataBind();
            this.pager.TotalRecords = this.pager.TotalRecords = memberBlanceList.TotalRecords;
            this.pager1.TotalRecords = this.pager.TotalRecords = memberBlanceList.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
                }
                this.txtUserName.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
            }
            else
            {
                this.searchKey = this.txtUserName.Text.Trim();
                this.realName = this.txtRealName.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnQuery.Click += new EventHandler(this.btnQuery_Click);
            this.btnAddBalance.Click += new EventHandler(this.btnAddBalance_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.GetBalance();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", base.Server.UrlEncode(this.txtUserName.Text));
            queryStrings.Add("realName", base.Server.UrlEncode(this.txtRealName.Text));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }
    }
}

