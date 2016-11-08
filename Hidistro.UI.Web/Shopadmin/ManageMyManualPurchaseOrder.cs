namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageMyManualPurchaseOrder : DistributorPage
    {
        protected Button btnClosePurchaseOrder;
        protected Button btnOk;
        protected Button btnReplace;
        protected Button btnReturn;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected DataList dlstPurchaseOrders;
        protected DropDownList dropRefundType;
        protected DropDownList dropReturnRefundType;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HyperLink hlinkAllOrder;
        protected HyperLink hlinkClose;
        protected HyperLink hlinkFinish;
        protected HyperLink hlinkHistory;
        protected HyperLink hlinkNotPay;
        protected HyperLink hlinkSendGoods;
        protected HyperLink hlinkYetPay;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtPurchaseOrderId;
        protected TextBox txtRemark;
        protected TextBox txtReplaceRemark;
        protected TextBox txtReturnRemark;
        protected TextBox txtShipTo;

        private void BindPurchaseOrders()
        {
            PurchaseOrderQuery purchaseOrderQuery = this.GetPurchaseOrderQuery();
            purchaseOrderQuery.IsManualPurchaseOrder = true;
            DbQueryResult purchaseOrders = SubsiteSalesHelper.GetPurchaseOrders(purchaseOrderQuery);
            this.dlstPurchaseOrders.DataSource = purchaseOrders.Data;
            this.dlstPurchaseOrders.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalRecords;
            this.pager1.TotalRecords = purchaseOrders.TotalRecords;
            this.txtShipTo.Text = purchaseOrderQuery.ShipTo;
            this.txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
            this.calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            this.calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
            this.lblStatus.Text = ((int) purchaseOrderQuery.PurchaseStatus).ToString();
        }

        private void btnBatchPayMoney_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要付款的采购单", false);
            }
            else
            {
                this.Page.Response.Redirect("BatchPay.aspx?PurchaseOrderIds=" + str);
            }
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
            purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
            if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
            {
                this.ShowMsg("取消采购成功", true);
            }
            else
            {
                this.ShowMsg("取消采购失败", false);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!SubsiteSalesHelper.CanPurchaseRefund(this.hidPurchaseOrderId.Value))
            {
                this.ShowMsg("已有待确认的申请！", false);
            }
            else if (SubsiteSalesHelper.ApplyForPurchaseRefund(this.hidPurchaseOrderId.Value, this.txtRemark.Text, int.Parse(this.dropRefundType.SelectedValue)))
            {
                this.BindPurchaseOrders();
                this.ShowMsg("成功的申请了退款", true);
            }
            else
            {
                this.ShowMsg("申请退款失败", false);
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (!SubsiteSalesHelper.CanPurchaseReplace(this.hidPurchaseOrderId.Value))
            {
                this.ShowMsg("已有待确认的申请！", false);
            }
            else if (SubsiteSalesHelper.ApplyForPurchaseReplace(this.hidPurchaseOrderId.Value, this.txtReplaceRemark.Text))
            {
                this.BindPurchaseOrders();
                this.ShowMsg("成功的申请了换货", true);
            }
            else
            {
                this.ShowMsg("申请换货失败", false);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!SubsiteSalesHelper.CanPurchaseReturn(this.hidPurchaseOrderId.Value))
            {
                this.ShowMsg("已有待确认的申请！", false);
            }
            else if (SubsiteSalesHelper.ApplyForPurchaseReturn(this.hidPurchaseOrderId.Value, this.txtReturnRemark.Text, int.Parse(this.dropReturnRefundType.SelectedValue)))
            {
                this.BindPurchaseOrders();
                this.ShowMsg("成功的申请了退货", true);
            }
            else
            {
                this.ShowMsg("申请退货失败", false);
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBinderPurchaseOrders(true);
        }

        private void dlstPurchaseOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
            if (((purchaseOrder != null) && (e.CommandName == "FINISH_TRADE")) && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
            {
                if (SubsiteSalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
                {
                    this.BindPurchaseOrders();
                    this.ShowMsg("成功的完成了该采购单", true);
                }
                else
                {
                    this.ShowMsg("完成采购单失败", false);
                }
            }
        }

        private void dlstPurchaseOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlGenericControl control = (HtmlGenericControl) e.Item.FindControl("lkbtnClosePurchaseOrder");
                HyperLink link = (HyperLink) e.Item.FindControl("lkbtnPay");
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("lkbtnConfirmPurchaseOrder");
                Literal literal = (Literal) e.Item.FindControl("litTbOrderDetailLink");
                Literal literal2 = (Literal) e.Item.FindControl("litPayment");
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseRefund");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseReturn");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseReplace");
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
                if (status == OrderStatus.WaitBuyerPay)
                {
                    control.Visible = true;
                    if ((DataBinder.Eval(e.Item.DataItem, "Gateway") == DBNull.Value) || ("hishop.plugins.payment.podrequest" != ((string) DataBinder.Eval(e.Item.DataItem, "Gateway"))))
                    {
                        link.Visible = true;
                    }
                }
                switch (status)
                {
                    case OrderStatus.BuyerAlreadyPaid:
                        anchor.Visible = true;
                        break;

                    case OrderStatus.SellerAlreadySent:
                        anchor2.Visible = true;
                        anchor3.Visible = true;
                        break;
                }
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.dlstPurchaseOrders.DataKeys[e.Item.ItemIndex].ToString());
                if (string.IsNullOrEmpty(purchaseOrder.PaymentType))
                {
                    if (status == OrderStatus.BuyerAlreadyPaid)
                    {
                        literal2.Text = "<br>支付方式：预付款";
                    }
                }
                else
                {
                    literal2.Text = "<br>支付方式：" + purchaseOrder.PaymentType;
                }
                button.Visible = status == OrderStatus.SellerAlreadySent;
                object obj2 = DataBinder.Eval(e.Item.DataItem, "TaobaoOrderId");
                if (((obj2 != null) && (obj2 != DBNull.Value)) && (obj2.ToString().Length > 0))
                {
                    literal.Text = string.Format("<a target=\"_blank\" href=\"http://trade.taobao.com/trade/detail/trade_item_detail.htm?bizOrderId={0}\"><span>来自淘宝</span></a>", obj2);
                }
            }
        }

        private PurchaseOrderQuery GetPurchaseOrderQuery()
        {
            PurchaseOrderQuery query = new PurchaseOrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.PurchaseOrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["PurchaseStatus"], out result))
                {
                    query.PurchaseStatus = (OrderStatus) result;
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "PurchaseDate";
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstPurchaseOrders.ItemDataBound += new DataListItemEventHandler(this.dlstPurchaseOrders_ItemDataBound);
            this.dlstPurchaseOrders.ItemCommand += new DataListCommandEventHandler(this.dlstPurchaseOrders_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnReturn.Click += new EventHandler(this.btnReturn_Click);
            this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
            if (!this.Page.IsPostBack)
            {
                this.SetPurchaseOrderStatusLink();
                this.BindPurchaseOrders();
            }
        }

        private void ReBinderPurchaseOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text.Trim());
            queryStrings.Add("ShipTo", this.txtShipTo.Text.Trim());
            queryStrings.Add("PurchaseStatus", this.lblStatus.Text);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetPurchaseOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/ShopAdmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx?PurchaseStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkHistory.NavigateUrl = string.Format(format, 0x63);
            this.hlinkFinish.NavigateUrl = string.Format(format, 5);
        }
    }
}

