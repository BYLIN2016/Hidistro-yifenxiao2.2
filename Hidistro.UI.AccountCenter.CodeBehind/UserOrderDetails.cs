namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UserOrderDetails : MemberTemplatedWebControl
    {
        private GridView grdOrderGift;
        private FormatedMoneyLabel lblAdjustedDiscount;
        private Literal lblBundlingPrice;
        private FormatedMoneyLabel lblCartMoney;
        private FormatedMoneyLabel lblDiscount;
        private FormatedMoneyLabel lblFreight;
        private OrderStatusLabel lblOrderStatus;
        private FormatedMoneyLabel lblPayCharge;
        private FormatedMoneyLabel lblRefundTotal;
        private FormatedMoneyLabel lblTotalBalance;
        private FormatedMoneyLabel lbltotalPrice;
        private Label lbRefundMoney;
        private FormatedTimeLabel litAddDate;
        private Literal litAddress;
        private Literal litCloseReason;
        private Literal litCouponValue;
        private HyperLink litDiscountName;
        private Literal litEmail;
        private Literal litFree;
        private HyperLink litFreeName;
        private Literal litInvoiceTitle;
        private Literal litModeName;
        private Literal litOrderId;
        private Literal litPaymentType;
        private Literal litPhone;
        private Literal litPoints;
        private Literal litRealModeName;
        private Literal litRefundOrderRemark;
        private Literal litRegion;
        private Literal litRemark;
        private HyperLink litSentTimesPointPromotion;
        private Literal litShippNumber;
        private Literal litShipTo;
        private Literal litShipToDate;
        private FormatedMoneyLabel litTax;
        private Literal litTellPhone;
        private FormatedMoneyLabel litTotalPrice;
        private Literal litUserAddress;
        private Literal litUserEmail;
        private Literal litUserMSN;
        private Literal litUserName;
        private Literal litUserPhone;
        private Literal litUserQQ;
        private Literal litUserTellPhone;
        private Literal litWeight;
        private Literal litZipcode;
        private string orderId;
        private Common_OrderManage_OrderItems orderItems;
        private Panel plExpress;
        private Panel plOrderGift;
        private Panel plOrderSended;
        private Panel plRefund;
        private HtmlAnchor power;

        protected override void AttachChildControls()
        {
            this.orderId = this.Page.Request.QueryString["orderId"];
            this.litOrderId = (Literal) this.FindControl("litOrderId");
            this.lbltotalPrice = (FormatedMoneyLabel) this.FindControl("lbltotalPrice");
            this.litAddDate = (FormatedTimeLabel) this.FindControl("litAddDate");
            this.lblOrderStatus = (OrderStatusLabel) this.FindControl("lblOrderStatus");
            this.litCloseReason = (Literal) this.FindControl("litCloseReason");
            this.litRemark = (Literal) this.FindControl("litRemark");
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litRegion = (Literal) this.FindControl("litRegion");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.litZipcode = (Literal) this.FindControl("litZipcode");
            this.litEmail = (Literal) this.FindControl("litEmail");
            this.litPhone = (Literal) this.FindControl("litPhone");
            this.litTellPhone = (Literal) this.FindControl("litTellPhone");
            this.litShipToDate = (Literal) this.FindControl("litShipToDate");
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.litUserAddress = (Literal) this.FindControl("litUserAddress");
            this.litUserEmail = (Literal) this.FindControl("litUserEmail");
            this.litUserPhone = (Literal) this.FindControl("litUserPhone");
            this.litUserTellPhone = (Literal) this.FindControl("litUserTellPhone");
            this.litUserQQ = (Literal) this.FindControl("litUserQQ");
            this.litUserMSN = (Literal) this.FindControl("litUserMSN");
            this.litPaymentType = (Literal) this.FindControl("litPaymentType");
            this.litModeName = (Literal) this.FindControl("litModeName");
            this.plOrderSended = (Panel) this.FindControl("plOrderSended");
            this.litRealModeName = (Literal) this.FindControl("litRealModeName");
            this.litShippNumber = (Literal) this.FindControl("litShippNumber");
            this.litDiscountName = (HyperLink) this.FindControl("litDiscountName");
            this.lblAdjustedDiscount = (FormatedMoneyLabel) this.FindControl("lblAdjustedDiscount");
            this.litFreeName = (HyperLink) this.FindControl("litFreeName");
            this.plExpress = (Panel) this.FindControl("plExpress");
            this.power = (HtmlAnchor) this.FindControl("power");
            this.orderItems = (Common_OrderManage_OrderItems) this.FindControl("Common_OrderManage_OrderItems");
            this.grdOrderGift = (GridView) this.FindControl("grdOrderGift");
            this.plOrderGift = (Panel) this.FindControl("plOrderGift");
            this.lblCartMoney = (FormatedMoneyLabel) this.FindControl("lblCartMoney");
            this.lblBundlingPrice = (Literal) this.FindControl("lblBundlingPrice");
            this.litPoints = (Literal) this.FindControl("litPoints");
            this.litSentTimesPointPromotion = (HyperLink) this.FindControl("litSentTimesPointPromotion");
            this.litWeight = (Literal) this.FindControl("litWeight");
            this.litFree = (Literal) this.FindControl("litFree");
            this.lblFreight = (FormatedMoneyLabel) this.FindControl("lblFreight");
            this.lblPayCharge = (FormatedMoneyLabel) this.FindControl("lblPayCharge");
            this.litCouponValue = (Literal) this.FindControl("litCouponValue");
            this.lblDiscount = (FormatedMoneyLabel) this.FindControl("lblDiscount");
            this.litTotalPrice = (FormatedMoneyLabel) this.FindControl("litTotalPrice");
            this.lblRefundTotal = (FormatedMoneyLabel) this.FindControl("lblRefundTotal");
            this.lbRefundMoney = (Label) this.FindControl("lbRefundMoney");
            this.plRefund = (Panel) this.FindControl("plRefund");
            this.lblTotalBalance = (FormatedMoneyLabel) this.FindControl("lblTotalBalance");
            this.litRefundOrderRemark = (Literal) this.FindControl("litRefundOrderRemark");
            this.litTax = (FormatedMoneyLabel) this.FindControl("litTax");
            this.litInvoiceTitle = (Literal) this.FindControl("litInvoiceTitle");
            PageTitle.AddTitle("订单详细页", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                if ((orderInfo == null) || (orderInfo.UserId != HiContext.Current.User.UserId))
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
                }
                else
                {
                    this.BindOrderBase(orderInfo);
                    this.BindOrderAddress(orderInfo);
                    this.BindOrderItems(orderInfo);
                    this.BindOrderRefund(orderInfo);
                    this.BindOrderReturn(orderInfo);
                }
            }
        }

        private void BindOrderAddress(OrderInfo order)
        {
            this.litShipTo.Text = order.ShipTo;
            this.litRegion.Text = order.ShippingRegion;
            this.litAddress.Text = order.Address;
            this.litZipcode.Text = order.ZipCode;
            this.litEmail.Text = order.EmailAddress;
            this.litTellPhone.Text = order.TelPhone;
            this.litPhone.Text = order.CellPhone;
            this.litShipToDate.Text = order.ShipToDate;
            Member user = HiContext.Current.User as Member;
            this.litUserName.Text = user.Username;
            this.litUserAddress.Text = RegionHelper.GetFullRegion(user.RegionId, "") + user.Address;
            this.litUserTellPhone.Text = user.TelPhone;
            this.litUserPhone.Text = user.CellPhone;
            this.litUserEmail.Text = user.Email;
            this.litUserQQ.Text = user.QQ;
            this.litUserMSN.Text = user.MSN;
            this.litPaymentType.Text = order.PaymentType + "(" + Globals.FormatMoney(order.PayCharge) + ")";
            this.litModeName.Text = order.ModeName + "(" + Globals.FormatMoney(order.AdjustedFreight) + ")";
            if ((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished))
            {
                this.plOrderSended.Visible = true;
                this.litShippNumber.Text = order.ShipOrderNumber;
                this.litRealModeName.Text = order.ExpressCompanyName;
            }
            if (((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(order.ExpressCompanyAbb))
            {
                if (this.plExpress != null)
                {
                    this.plExpress.Visible = true;
                }
                if ((Express.GetExpressType() == "kuaidi100") && (this.power != null))
                {
                    this.power.Visible = true;
                }
            }
        }

        private void BindOrderBase(OrderInfo order)
        {
            this.litOrderId.Text = order.OrderId;
            this.lbltotalPrice.Money = order.GetTotal();
            this.litAddDate.Time = order.OrderDate;
            this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
            if (order.OrderStatus == OrderStatus.Closed)
            {
                this.litCloseReason.Text = order.CloseReason;
            }
            this.litRemark.Text = order.Remark;
        }

        private void BindOrderItems(OrderInfo order)
        {
            this.orderItems.DataSource = order.LineItems.Values;
            this.orderItems.DataBind();
            if (order.Gifts.Count > 0)
            {
                this.plOrderGift.Visible = true;
                this.grdOrderGift.DataSource = order.Gifts;
                this.grdOrderGift.DataBind();
            }
            if (order.BundlingID > 0)
            {
                this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(order.BundlingPrice));
            }
            this.lblCartMoney.Money = order.GetAmount();
            this.litWeight.Text = order.Weight.ToString();
            this.lblPayCharge.Money = order.PayCharge;
            this.lblFreight.Money = order.AdjustedFreight;
            if (order.IsFreightFree)
            {
                this.litFreeName.Text = order.FreightFreePromotionName;
                this.litFreeName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.FreightFreePromotionId });
            }
            this.litTax.Money = order.Tax;
            this.litInvoiceTitle.Text = order.InvoiceTitle;
            this.lblAdjustedDiscount.Money = order.AdjustedDiscount;
            this.litCouponValue.Text = order.CouponName + " -" + Globals.FormatMoney(order.CouponValue);
            this.lblDiscount.Money = order.ReducedPromotionAmount;
            if (order.IsReduced)
            {
                this.litDiscountName.Text = order.ReducedPromotionName;
                this.litDiscountName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.ReducedPromotionId });
            }
            this.litPoints.Text = order.Points.ToString(CultureInfo.InvariantCulture);
            if (order.IsSendTimesPoint)
            {
                this.litSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
                this.litSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.SentTimesPointPromotionId });
            }
            this.litTotalPrice.Money = order.GetTotal();
        }

        private void BindOrderRefund(OrderInfo order)
        {
            if ((order.RefundStatus == RefundStatus.Refund) || (order.RefundStatus == RefundStatus.Below))
            {
                this.plRefund.Visible = true;
                this.lblTotalBalance.Money = order.RefundAmount;
                this.litRefundOrderRemark.Text = order.RefundRemark;
            }
        }

        private void BindOrderReturn(OrderInfo order)
        {
            if (order.OrderStatus == OrderStatus.Returned)
            {
                decimal num;
                this.lblRefundTotal.Money = TradeHelper.GetRefundMoney(order, out num);
            }
            else
            {
                this.lbRefundMoney.Visible = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserOrderDetails.html";
            }
            base.OnInit(e);
        }
    }
}

