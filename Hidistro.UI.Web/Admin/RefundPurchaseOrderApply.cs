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

    [PrivilegeCheck(Privilege.PurchaseOrderRefundApply)]
    public class RefundPurchaseOrderApply : AdminPage
    {
        protected Button btnAcceptRefund;
        protected Button btnRefuseRefund;
        protected Button btnSearchButton;
        protected DropDownList ddlHandleStatus;
        protected DataList dlstRefund;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected PageSize hrefPageSize;
        protected Label lblAddress;
        protected Label lblContacts;
        protected Label lblEmail;
        protected Label lblPurchaseOrderTotal;
        protected Label lblRefundRemark;
        protected Label lblRefundType;
        protected Label lblStatus;
        protected Label lblTelephone;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected Label refund_lblPurchaseOrderId;
        protected TextBox txtAdminRemark;
        protected TextBox txtOrderId;

        private void BindRefund()
        {
            RefundApplyQuery refundQuery = this.GetRefundQuery();
            DbQueryResult purchaseRefundApplys = SalesHelper.GetPurchaseRefundApplys(refundQuery);
            this.dlstRefund.DataSource = purchaseRefundApplys.Data;
            this.dlstRefund.DataBind();
            this.pager.TotalRecords = purchaseRefundApplys.TotalRecords;
            this.pager1.TotalRecords = purchaseRefundApplys.TotalRecords;
            this.txtOrderId.Text = refundQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            if (refundQuery.HandleStatus.HasValue && (refundQuery.HandleStatus.Value > -1))
            {
                this.ddlHandleStatus.SelectedValue = refundQuery.HandleStatus.Value.ToString();
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseRefund(SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value), HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
            this.BindRefund();
            this.ShowMsg("成功的确认了采购单退款", true);
        }

        private void btnRefuseRefund_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseRefund(SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value), HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindRefund();
            this.ShowMsg("成功的拒绝了采购单退款", true);
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadRefunds(true);
        }

        private void dlstRefund_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnCheckRefund");
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

        private RefundApplyQuery GetRefundQuery()
        {
            RefundApplyQuery query = new RefundApplyQuery();
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
                this.ShowMsg("请选要删除的退款申请单", false);
            }
            else
            {
                int num;
                SalesHelper.DelPurchaseRefundApply(str.Split(new char[] { ',' }), out num);
                this.BindRefund();
                string msg = string.Format("成功删除了{0}个退款申请单", num);
                if (str.Split(new char[] { ',' }).Length != num)
                {
                    msg = msg + ",待处理退款申请单不能删除";
                }
                this.ShowMsg(msg, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstRefund.ItemDataBound += new DataListItemEventHandler(this.dlstRefund_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            if (!base.IsPostBack)
            {
                this.BindRefund();
            }
        }

        private void ReloadRefunds(bool isSearch)
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

