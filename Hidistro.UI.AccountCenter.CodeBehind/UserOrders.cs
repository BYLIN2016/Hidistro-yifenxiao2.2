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
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserOrders : MemberTemplatedWebControl
    {
        private IButton btnOk;
        private IButton btnPay;
        private IButton btnReplace;
        private IButton btnReturn;
        private IButton btnSearch;
        private WebCalendar calendarEndDate;
        private WebCalendar calendarStartDate;
        private OrderStautsDropDownList dropOrderStatus;
        private DropDownList dropPayType;
        private DropDownList dropRefundType;
        private DropDownList dropReturnRefundType;
        private HtmlInputHidden hdorderId;
        private Common_OrderManage_OrderList listOrders;
        private Literal litOrderTotal;
        private Pager pager;
        private TextBox txtOrderId;
        private TextBox txtRemark;
        private TextBox txtReplaceRemark;
        private TextBox txtReturnRemark;
        private TextBox txtShipId;
        private TextBox txtShipTo;

        protected override void AttachChildControls()
        {
            this.calendarStartDate = (WebCalendar) this.FindControl("calendarStartDate");
            this.calendarEndDate = (WebCalendar) this.FindControl("calendarEndDate");
            this.hdorderId = (HtmlInputHidden) this.FindControl("hdorderId");
            this.txtOrderId = (TextBox) this.FindControl("txtOrderId");
            this.txtShipId = (TextBox) this.FindControl("txtShipId");
            this.txtShipTo = (TextBox) this.FindControl("txtShipTo");
            this.txtRemark = (TextBox) this.FindControl("txtRemark");
            this.txtReturnRemark = (TextBox) this.FindControl("txtReturnRemark");
            this.txtReplaceRemark = (TextBox) this.FindControl("txtReplaceRemark");
            this.dropOrderStatus = (OrderStautsDropDownList) this.FindControl("dropOrderStatus");
            this.dropPayType = (DropDownList) this.FindControl("dropPayType");
            this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.btnOk = ButtonManager.Create(this.FindControl("btnOk"));
            this.btnReturn = ButtonManager.Create(this.FindControl("btnReturn"));
            this.btnReplace = ButtonManager.Create(this.FindControl("btnReplace"));
            this.litOrderTotal = (Literal) this.FindControl("litOrderTotal");
            this.dropRefundType = (DropDownList) this.FindControl("dropRefundType");
            this.dropReturnRefundType = (DropDownList) this.FindControl("dropReturnRefundType");
            this.listOrders = (Common_OrderManage_OrderList) this.FindControl("Common_OrderManage_OrderList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnSearch.Click += new EventHandler(this.lbtnSearch_Click);
            this.btnPay.Click += new EventHandler(this.btnPay_Click);
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnReturn.Click += new EventHandler(this.btnReturn_Click);
            this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
            this.listOrders._ItemDataBound += new Common_OrderManage_OrderList.DataBindEventHandler(this.listOrders_ItemDataBound);
            this.listOrders._ItemCommand += new Common_OrderManage_OrderList.CommandEventHandler(this.listOrders_ItemCommand);
            this.listOrders._ReBindData += new Common_OrderManage_OrderList.ReBindDataEventHandler(this.listOrders_ReBindData);
            PageTitle.AddSiteNameTitle("我的订单", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                this.dropPayType.DataSource = TradeHelper.GetPaymentModes();
                this.dropPayType.DataTextField = "Name";
                this.dropPayType.DataValueField = "ModeId";
                this.dropPayType.DataBind();
                this.BindOrders();
            }
        }

        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
            this.listOrders.DataSource = userOrder.Data;
            this.listOrders.DataBind();
            this.txtOrderId.Text = orderQuery.OrderId;
            this.txtShipId.Text = orderQuery.ShipId;
            this.txtShipTo.Text = orderQuery.ShipTo;
            this.dropOrderStatus.SelectedValue = orderQuery.Status;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.pager.TotalRecords = userOrder.TotalRecords;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!TradeHelper.CanRefund(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
            }
            else if (!this.CanRefundBalance())
            {
                this.ShowMessage("请先开通预付款账户", false);
            }
            else if (TradeHelper.ApplyForRefund(this.hdorderId.Value, this.txtRemark.Text, int.Parse(this.dropRefundType.SelectedValue)))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了退款", true);
            }
            else
            {
                this.ShowMessage("申请退款失败", false);
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            string orderId = this.hdorderId.Value;
            int result = 0;
            int.TryParse(this.dropPayType.SelectedValue, out result);
            PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(result);
            if (paymentMode != null)
            {
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
                orderInfo.PaymentTypeId = paymentMode.ModeId;
                orderInfo.PaymentType = paymentMode.Name;
                orderInfo.Gateway = paymentMode.Gateway;
                TradeHelper.UpdateOrderPaymentType(orderInfo);
            }
            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[] { orderId }));
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (!TradeHelper.CanReplace(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
            }
            else if (TradeHelper.ApplyForReplace(this.hdorderId.Value, this.txtReplaceRemark.Text))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了换货", true);
            }
            else
            {
                this.ShowMessage("申请换货失败", false);
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!TradeHelper.CanReturn(this.hdorderId.Value))
            {
                this.ShowMessage("已有待确认的申请！", false);
            }
            else if (!this.CanReturnBalance())
            {
                this.ShowMessage("请先开通预付款账户", false);
            }
            else if (TradeHelper.ApplyForReturn(this.hdorderId.Value, this.txtReturnRemark.Text, int.Parse(this.dropReturnRefundType.SelectedValue)))
            {
                this.BindOrders();
                this.ShowMessage("成功的申请了退货", true);
            }
            else
            {
                this.ShowMessage("申请退货失败", false);
            }
        }

        private bool CanRefundBalance()
        {
            if (Convert.ToInt32(this.dropRefundType.SelectedValue) != 1)
            {
                return true;
            }
            Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            return user.IsOpenBalance;
        }

        private bool CanReturnBalance()
        {
            if (Convert.ToInt32(this.dropReturnRefundType.SelectedValue) != 1)
            {
                return true;
            }
            Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
            return user.IsOpenBalance;
        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                query.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipId"]))
            {
                query.ShipId = this.Page.Request.QueryString["shipId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
            {
                query.ShipTo = this.Page.Request.QueryString["shipTo"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
            {
                query.OrderId = this.Page.Request.QueryString["orderId"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["startDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["endDate"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out result))
                {
                    query.Status = (OrderStatus) result;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
            {
                query.SortBy = this.Page.Request.QueryString["sortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out num2))
                {
                    query.SortOrder = (SortAction) num2;
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            return query;
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadUserOrders(true);
        }

        protected void listOrders_ItemCommand(object sender, GridViewCommandEventArgs e)
        {
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if ((e.CommandName == "FINISH_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (TradeHelper.ConfirmOrderFinish(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMessage("成功的完成了该订单", true);
                    }
                    else
                    {
                        this.ShowMessage("完成订单失败", false);
                    }
                }
                if ((e.CommandName == "CLOSE_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
                {
                    if (TradeHelper.CloseOrder(orderInfo.OrderId))
                    {
                        this.BindOrders();
                        this.ShowMessage("成功的关闭了该订单", true);
                    }
                    else
                    {
                        this.ShowMessage("关闭订单失败", false);
                    }
                }
            }
        }

        protected void listOrders_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Row.DataItem, "OrderStatus");
                string str = "";
                if ((DataBinder.Eval(e.Row.DataItem, "Gateway") != null) && !(DataBinder.Eval(e.Row.DataItem, "Gateway") is DBNull))
                {
                    str = (string) DataBinder.Eval(e.Row.DataItem, "Gateway");
                }
                HyperLink link = (HyperLink) e.Row.FindControl("hplinkorderreview");
                HtmlAnchor anchor = (HtmlAnchor) e.Row.FindControl("hlinkPay");
                ImageLinkButton button = (ImageLinkButton) e.Row.FindControl("lkbtnConfirmOrder");
                ImageLinkButton button2 = (ImageLinkButton) e.Row.FindControl("lkbtnCloseOrder");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Row.FindControl("lkbtnApplyForRefund");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Row.FindControl("lkbtnApplyForReturn");
                HtmlAnchor anchor4 = (HtmlAnchor) e.Row.FindControl("lkbtnApplyForReplace");
                if (link != null)
                {
                    link.Visible = status == OrderStatus.Finished;
                }
                anchor.Visible = (status == OrderStatus.WaitBuyerPay) && (str != "hishop.plugins.payment.podrequest");
                button.Visible = status == OrderStatus.SellerAlreadySent;
                button2.Visible = status == OrderStatus.WaitBuyerPay;
                anchor2.Visible = status == OrderStatus.BuyerAlreadyPaid;
                anchor3.Visible = status == OrderStatus.SellerAlreadySent;
                anchor4.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        protected void listOrders_ReBindData(object sender)
        {
            this.ReloadUserOrders(false);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserOrders.html";
            }
            base.OnInit(e);
        }

        private void ReloadUserOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("orderId", this.txtOrderId.Text.Trim());
            queryStrings.Add("shipId", this.txtShipId.Text.Trim());
            queryStrings.Add("shipTo", this.txtShipTo.Text.Trim());
            queryStrings.Add("startDate", this.calendarStartDate.SelectedDate.ToString());
            queryStrings.Add("endDate", this.calendarEndDate.SelectedDate.ToString());
            queryStrings.Add("orderStatus", ((int) this.dropOrderStatus.SelectedValue).ToString());
            queryStrings.Add("sortBy", this.listOrders.SortOrderBy);
            queryStrings.Add("sortOrder", ((int) this.listOrders.SortOrder).ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

