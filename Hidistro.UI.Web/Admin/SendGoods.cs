namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.OrderSendGoods)]
    public class SendGoods : AdminPage
    {
        protected Button btnSendGoods;
        protected ExpressRadioButtonList expressRadioButtonList;
        protected Order_ItemsList itemsList;
        protected Label lblOrderId;
        protected FormatedTimeLabel lblOrderTime;
        protected Literal litReceivingInfo;
        protected Label litRemark;
        protected Literal litShippingModeName;
        protected Label litShipToDate;
        private string orderId;
        protected ShippingModeRadioButtonList radioShippingMode;
        protected TextBox txtShipOrderNumber;
        protected HtmlGenericControl txtShipOrderNumberTip;

        private void BindExpressCompany(int modeId)
        {
            this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
            this.expressRadioButtonList.DataBind();
        }

        private void BindOrderItems(OrderInfo order)
        {
            this.lblOrderId.Text = order.OrderId;
            this.lblOrderTime.Time = order.OrderDate;
            this.itemsList.Order = order;
        }

        private void BindShippingAddress(OrderInfo order)
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(order.ShippingRegion))
            {
                shippingRegion = order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(order.Address))
            {
                shippingRegion = shippingRegion + order.Address;
            }
            if (!string.IsNullOrEmpty(order.ShipTo))
            {
                shippingRegion = shippingRegion + "  " + order.ShipTo;
            }
            if (!string.IsNullOrEmpty(order.ZipCode))
            {
                shippingRegion = shippingRegion + "  " + order.ZipCode;
            }
            if (!string.IsNullOrEmpty(order.TelPhone))
            {
                shippingRegion = shippingRegion + "  " + order.TelPhone;
            }
            if (!string.IsNullOrEmpty(order.CellPhone))
            {
                shippingRegion = shippingRegion + "  " + order.CellPhone;
            }
            this.litReceivingInfo.Text = shippingRegion;
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
            if (orderInfo != null)
            {
                if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Success))
                {
                    this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
                }
                else if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
                {
                    this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                }
                else if (!this.radioShippingMode.SelectedValue.HasValue)
                {
                    this.ShowMsg("请选择配送方式", false);
                }
                else if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || (this.txtShipOrderNumber.Text.Trim().Length > 20))
                {
                    this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
                }
                else if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
                {
                    this.ShowMsg("请选择物流公司", false);
                }
                else
                {
                    ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
                    orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                    orderInfo.RealModeName = shippingMode.Name;
                    ExpressCompanyInfo info3 = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
                    if (info3 != null)
                    {
                        orderInfo.ExpressCompanyAbb = info3.Kuaidi100Code;
                        orderInfo.ExpressCompanyName = info3.Name;
                    }
                    orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
                    if (OrderHelper.SendGoods(orderInfo))
                    {
                        SendNote note = new SendNote();
                        note.NoteId = Globals.GetGenerateId();
                        note.OrderId = this.orderId;
                        note.Operator = HiContext.Current.User.Username;
                        note.Remark = "后台" + note.Operator + "发货成功";
                        OrderHelper.SaveSendNote(note);
                        if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                        {
                            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                            if (paymentMode != null)
                            {
                                PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                            }
                        }
                        int userId = orderInfo.UserId;
                        if (userId == 0x44c)
                        {
                            userId = 0;
                        }
                        IUser user = Users.GetUser(userId);
                        Messenger.OrderShipping(orderInfo, user);
                        orderInfo.OnDeliver();
                        this.ShowMsg("发货成功", true);
                    }
                    else
                    {
                        this.ShowMsg("发货失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.orderId = this.Page.Request.QueryString["OrderId"];
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                this.BindOrderItems(orderInfo);
                this.btnSendGoods.Click += new EventHandler(this.btnSendGoods_Click);
                this.radioShippingMode.SelectedIndexChanged += new EventHandler(this.radioShippingMode_SelectedIndexChanged);
                if (!this.Page.IsPostBack)
                {
                    if (orderInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.radioShippingMode.DataBind();
                        this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
                        this.BindExpressCompany(orderInfo.ShippingModeId);
                        this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
                        this.BindShippingAddress(orderInfo);
                        this.litShippingModeName.Text = orderInfo.ModeName;
                        this.litShipToDate.Text = orderInfo.ShipToDate;
                        this.litRemark.Text = orderInfo.Remark;
                        this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
                    }
                }
            }
        }

        private void radioShippingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioShippingMode.SelectedValue.HasValue)
            {
                this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
            }
        }
    }
}

