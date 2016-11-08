namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UnderlingAccountSummaryList : DistributorPage
    {
        protected Button btnAddBalance;
        protected Button btnQuery;
        protected HtmlInputHidden curentBalance;
        protected HtmlInputHidden currentUserId;
        protected Grid grdUnderlingAccountList;
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
            decimal num;
            int length = 0;
            if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
            {
                length = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
            }
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
                int userId = int.Parse(this.currentUserId.Value);
                Member user = Users.GetUser(userId, false) as Member;
                if ((user == null) || !user.IsOpenBalance)
                {
                    this.ShowMsg("本次充值已失败，该用户的预付款还没有开通", false);
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
                    else if (UnderlingHelper.AddUnderlingBalanceDetail(target))
                    {
                        this.txtReCharge.Text = "";
                        this.ReBind(false);
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
            DbQueryResult underlingBlanceList = UnderlingHelper.GetUnderlingBlanceList(query);
            this.grdUnderlingAccountList.DataSource = underlingBlanceList.Data;
            this.grdUnderlingAccountList.DataBind();
            this.pager.TotalRecords = underlingBlanceList.TotalRecords;
            this.pager1.TotalRecords = underlingBlanceList.TotalRecords;
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
                this.txtRealName.Text = this.realName;
                this.txtUserName.Text = this.searchKey;
            }
            else
            {
                this.searchKey = this.txtUserName.Text;
                this.realName = this.txtRealName.Text;
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
            queryStrings.Add("searchKey", this.txtUserName.Text);
            queryStrings.Add("realName", this.txtRealName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

