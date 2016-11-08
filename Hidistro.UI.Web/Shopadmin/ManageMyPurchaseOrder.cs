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

    public class ManageMyPurchaseOrder : DistributorPage
    {
        protected ImageLinkButton btnBatchPayMoney;
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
        protected TextBox txtOrderId;
        protected TextBox txtProductName;
        protected TextBox txtPurchaseOrderId;
        protected TextBox txtRemark;
        protected TextBox txtReplaceRemark;
        protected TextBox txtReturnRemark;

        private void BindPurchaseOrders()
        {
            PurchaseOrderQuery purchaseOrderQuery = this.GetPurchaseOrderQuery();
            purchaseOrderQuery.SortOrder = SortAction.Desc;
            purchaseOrderQuery.SortBy = "PurchaseDate";
            purchaseOrderQuery.IsManualPurchaseOrder = false;
            DbQueryResult purchaseOrders = SubsiteSalesHelper.GetPurchaseOrders(purchaseOrderQuery);
            this.dlstPurchaseOrders.DataSource = purchaseOrders.Data;
            this.dlstPurchaseOrders.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalRecords;
            this.pager1.TotalRecords = purchaseOrders.TotalRecords;
            this.txtOrderId.Text = purchaseOrderQuery.OrderId;
            this.txtProductName.Text = purchaseOrderQuery.ProductName;
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
            if (!string.IsNullOrEmpty(this.hidPurchaseOrderId.Value))
            {
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
                purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
                if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
                {
                    this.BindPurchaseOrders();
                    this.ShowMsg("取消采购成功", true);
                }
                else
                {
                    this.ShowMsg("取消采购失败", false);
                }
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
            this.ReBinderPurchaseOrders(false);
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
                HyperLink link = (HyperLink) e.Item.FindControl("lkbtnPay");
                HyperLink link2 = (HyperLink) e.Item.FindControl("lkbtnSendGoods");
                HtmlGenericControl control = (HtmlGenericControl) e.Item.FindControl("lkBtnCancelPurchaseOrder");
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("lkbtnConfirmPurchaseOrder");
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseRefund");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseReturn");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Item.FindControl("lkbtnApplyForPurchaseReplace");
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
                string orderId = (string) DataBinder.Eval(e.Item.DataItem, "OrderId");
                Literal literal = (Literal) e.Item.FindControl("litPayment");
                if (status == OrderStatus.SellerAlreadySent)
                {
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
                    if ((orderInfo != null) && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                    {
                        link2.Visible = true;
                    }
                    else
                    {
                        button.Visible = true;
                    }
                    anchor2.Visible = true;
                    anchor3.Visible = true;
                }
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        link.Visible = true;
                        control.Visible = true;
                        if ((DataBinder.Eval(e.Item.DataItem, "Gateway") != DBNull.Value) && ("hishop.plugins.payment.podrequest" == ((string) DataBinder.Eval(e.Item.DataItem, "Gateway"))))
                        {
                            link.Visible = false;
                        }
                        break;

                    case OrderStatus.BuyerAlreadyPaid:
                        anchor.Visible = true;
                        break;
                }
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.dlstPurchaseOrders.DataKeys[e.Item.ItemIndex].ToString());
                if (string.IsNullOrEmpty(purchaseOrder.PaymentType))
                {
                    if (status == OrderStatus.BuyerAlreadyPaid)
                    {
                        literal.Text = "<br>支付方式：预付款";
                    }
                }
                else
                {
                    literal.Text = "<br>支付方式：" + purchaseOrder.PaymentType;
                }
            }
        }

        private PurchaseOrderQuery GetPurchaseOrderQuery()
        {
            PurchaseOrderQuery query = new PurchaseOrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.PurchaseOrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
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
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstPurchaseOrders.ItemDataBound += new DataListItemEventHandler(this.dlstPurchaseOrders_ItemDataBound);
            this.dlstPurchaseOrders.ItemCommand += new DataListCommandEventHandler(this.dlstPurchaseOrders_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnBatchPayMoney.Click += new EventHandler(this.btnBatchPayMoney_Click);
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnReturn.Click += new EventHandler(this.btnReturn_Click);
            this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
            this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
            if (!this.Page.IsPostBack)
            {
                this.SetPurchaseOrderStatusLink();
                this.BindPurchaseOrders();
            }
        }

        private void ReBinderPurchaseOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", this.txtOrderId.Text.Trim());
            queryStrings.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text.Trim());
            queryStrings.Add("ProductName", this.txtProductName.Text.Trim());
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
            string format = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx?PurchaseStatus={0}";
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

