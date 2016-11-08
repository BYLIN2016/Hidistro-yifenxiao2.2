namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class UserReplaceApply : MemberTemplatedWebControl
    {
        private IButton btnSearch;
        private DropDownList ddlHandleStatus;
        private Common_OrderManage_ReplaceApply listReplace;
        private Pager pager;
        private TextBox txtOrderId;

        protected override void AttachChildControls()
        {
            this.txtOrderId = (TextBox) this.FindControl("txtOrderId");
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.listReplace = (Common_OrderManage_ReplaceApply) this.FindControl("Common_OrderManage_ReplaceApply");
            this.ddlHandleStatus = (DropDownList) this.FindControl("ddlHandleStatus");
            this.pager = (Pager) this.FindControl("pager");
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindRefund();
            }
        }

        private void BindRefund()
        {
            ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
            DbQueryResult replaceApplys = TradeHelper.GetReplaceApplys(replaceQuery, HiContext.Current.User.UserId);
            this.listReplace.DataSource = replaceApplys.Data;
            this.listReplace.DataBind();
            this.pager.TotalRecords = replaceApplys.TotalRecords;
            this.txtOrderId.Text = replaceQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            if (replaceQuery.HandleStatus.HasValue && (replaceQuery.HandleStatus.Value > -1))
            {
                this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value.ToString();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadReplace(true);
        }

        private ReplaceApplyQuery GetReplaceQuery()
        {
            ReplaceApplyQuery query = new ReplaceApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out result) && (result > -1))
                {
                    query.HandleStatus = new int?(result);
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "ApplyForTime";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserReplaceApply.html";
            }
            base.OnInit(e);
        }

        private void ReloadReplace(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
            {
                queryStrings.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

