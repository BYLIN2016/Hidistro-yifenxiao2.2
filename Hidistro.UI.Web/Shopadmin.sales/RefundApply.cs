namespace Hidistro.UI.Web.Shopadmin.sales
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class RefundApply : DistributorPage
    {
        protected Button btnAcceptRefund;
        protected Button btnRefuseRefund;
        protected Button btnSearchButton;
        protected DropDownList ddlHandleStatus;
        protected DataList dlstRefund;
        protected HtmlInputHidden hidOrderId;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidRefundType;
        protected PageSize hrefPageSize;
        protected Label lblAddress;
        protected Label lblContacts;
        protected Label lblEmail;
        protected Label lblOrderId;
        protected Label lblOrderTotal;
        protected Label lblRefundRemark;
        protected Label lblRefundType;
        protected Label lblStatus;
        protected Label lblTelephone;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlTextArea txtAdminRemark;
        protected TextBox txtOrderId;

        private void BindRefund()
        {
            RefundApplyQuery refundQuery = this.GetRefundQuery();
            DbQueryResult refundApplys = SubsiteSalesHelper.GetRefundApplys(refundQuery);
            this.dlstRefund.DataSource = refundApplys.Data;
            this.dlstRefund.DataBind();
            this.pager.TotalRecords = refundApplys.TotalRecords;
            this.pager1.TotalRecords = refundApplys.TotalRecords;
            this.txtOrderId.Text = refundQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            if (refundQuery.HandleStatus.HasValue && (refundQuery.HandleStatus.Value > -1))
            {
                this.ddlHandleStatus.SelectedValue = refundQuery.HandleStatus.Value.ToString();
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
            if (SubsiteSalesHelper.CheckRefund(orderInfo, this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindRefund();
                orderInfo.GetTotal();
                if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed))
                {
                    decimal decimal1 = orderInfo.GetTotal() - orderInfo.NeedPrice;
                }
                this.ShowMsg("成功的确认了订单退款", true);
            }
        }

        private void btnRefuseRefund_Click(object sender, EventArgs e)
        {
            SubsiteSalesHelper.CheckRefund(SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value), this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindRefund();
            this.ShowMsg("成功的拒绝了订单退款", true);
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
                string format = "成功删除了{0}个退款申请单";
                if (SubsiteSalesHelper.DelRefundApply(str.Split(new char[] { ',' }), out num))
                {
                    format = string.Format(format, num);
                }
                else
                {
                    format = string.Format(format, num) + ",待处理的申请不能删除";
                }
                this.BindRefund();
                this.ShowMsg(format, true);
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

