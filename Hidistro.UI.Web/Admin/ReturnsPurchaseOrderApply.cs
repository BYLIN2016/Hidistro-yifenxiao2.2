namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PurchaseOrderReturnsApply)]
    public class ReturnsPurchaseOrderApply : AdminPage
    {
        protected Button btnAcceptReturn;
        protected Button btnRefuseReturn;
        protected Button btnSearchButton;
        protected DropDownList ddlHandleStatus;
        protected DataList dlstReturns;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected Label return_lblAddress;
        protected Label return_lblContacts;
        protected Label return_lblEmail;
        protected Label return_lblPurchaseOrderId;
        protected Label return_lblPurchaseOrderTotal;
        protected Label return_lblRefundType;
        protected Label return_lblReturnRemark;
        protected Label return_lblTelephone;
        protected TextBox return_txtAdminRemark;
        protected TextBox return_txtRefundMoney;
        protected TextBox txtOrderId;

        private void BindReturns()
        {
            ReturnsApplyQuery returnsQuery = this.GetReturnsQuery();
            DbQueryResult purchaseReturnsApplys = SalesHelper.GetPurchaseReturnsApplys(returnsQuery);
            this.dlstReturns.DataSource = purchaseReturnsApplys.Data;
            this.dlstReturns.DataBind();
            this.pager.TotalRecords = purchaseReturnsApplys.TotalRecords;
            this.pager1.TotalRecords = purchaseReturnsApplys.TotalRecords;
            this.txtOrderId.Text = returnsQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            if (returnsQuery.HandleStatus.HasValue && (returnsQuery.HandleStatus.Value > -1))
            {
                this.ddlHandleStatus.SelectedValue = returnsQuery.HandleStatus.Value.ToString();
            }
        }

        protected void btnAcceptReturns_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.hidRefundMoney.Value, out num))
            {
                this.ShowMsg("退款金额需为数字格式！", false);
            }
            else
            {
                decimal num2;
                decimal.TryParse(this.hidOrderTotal.Value, out num2);
                if (num > num2)
                {
                    this.ShowMsg("退款金额不能大于订单金额！", false);
                }
                else
                {
                    SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, HiContext.Current.User.Username, num, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
                    this.BindReturns();
                    this.ShowMsg("成功的确认了采购单退货", true);
                }
            }
        }

        private void btnRefuseReturns_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, HiContext.Current.User.Username, 0M, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindReturns();
            this.ShowMsg("成功的拒绝了采购单退货", true);
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadReturnss(true);
        }

        private void dlstReturns_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReturns");
                Label label = (Label) e.Item.FindControl("lblHandleStatus");
                if (label.Text == "0")
                {
                    anchor.Visible = true;
                    label.Text = "待处理";
                }
                else if (label.Text == "1")
                {
                    label.Text = "已处理";
                }
                else
                {
                    label.Text = "已拒绝";
                }
            }
        }

        private ReturnsApplyQuery GetReturnsQuery()
        {
            ReturnsApplyQuery query = new ReturnsApplyQuery();
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

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的退货申请单", false);
            }
            else
            {
                int num;
                string format = "成功删除了{0}个退货申请单";
                if (SalesHelper.DelPurchaseReturnsApply(str.Split(new char[] { ',' }), out num))
                {
                    format = string.Format(format, num);
                }
                else
                {
                    format = string.Format(format, num) + ",待处理的申请不能删除";
                }
                this.BindReturns();
                this.ShowMsg(format, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstReturns.ItemDataBound += new DataListItemEventHandler(this.dlstReturns_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnAcceptReturn.Click += new EventHandler(this.btnAcceptReturns_Click);
            this.btnRefuseReturn.Click += new EventHandler(this.btnRefuseReturns_Click);
            if (!base.IsPostBack)
            {
                this.BindReturns();
            }
        }

        private void ReloadReturnss(bool isSearch)
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

