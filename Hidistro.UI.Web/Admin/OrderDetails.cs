namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Orders)]
    public class OrderDetails : AdminPage
    {
        protected Button btnCloseOrder;
        protected Button btnMondifyPay;
        protected Button btnMondifyShip;
        protected Button btnRemark;
        protected Order_ChargesList chargesList;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected PaymentDropDownList ddlpayment;
        protected ShippingModeDropDownList ddlshippingMode;
        protected HyperLink hlkOrderGifts;
        protected Order_ItemsList itemsList;
        protected Label lbCloseReason;
        protected FormatedTimeLabel lblorderDateForRemark;
        protected OrderStatusLabel lblOrderStatus;
        protected FormatedMoneyLabel lblorderTotalForRemark;
        protected Label lbReason;
        protected HtmlAnchor lbtnClocsOrder;
        protected Literal litFinishTime;
        protected Literal litOrderId;
        protected Literal litPayTime;
        protected Literal litRealName;
        protected Literal litSendGoodTime;
        protected Literal litUserEmail;
        protected Literal litUserName;
        protected Literal litUserTel;
        protected HyperLink lkbtnEditPrice;
        protected HyperLink lkbtnSendGoods;
        private OrderInfo order;
        private string orderId;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected Order_ShippingAddress shippingAddress;
        protected Literal spanOrderId;
        protected TextBox txtRemark;

        private void BindRemark(OrderInfo order)
        {
            this.spanOrderId.Text = order.OrderId;
            this.lblorderDateForRemark.Time = order.OrderDate;
            this.lblorderTotalForRemark.Money = order.GetTotal();
            this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
            this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            this.order.CloseReason = this.ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(this.order))
            {
                Messenger.OrderClosed(Users.GetUser(this.order.UserId), this.order.OrderId, this.order.CloseReason);
                this.order.OnClosed();
                this.ShowMsg("关闭订单成功", true);
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        private void btnMondifyPay_Click(object sender, EventArgs e)
        {
            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
            this.order.PaymentTypeId = paymentMode.ModeId;
            this.order.PaymentType = paymentMode.Name;
            this.order.Gateway = paymentMode.Gateway;
            if (OrderHelper.UpdateOrderPaymentType(this.order))
            {
                this.chargesList.LoadControls();
                this.ShowMsg("修改支付方式成功", true);
            }
            else
            {
                this.ShowMsg("修改支付方式失败", false);
            }
        }

        private void btnMondifyShip_Click(object sender, EventArgs e)
        {
            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.ddlshippingMode.SelectedValue.Value, false);
            this.order.ShippingModeId = shippingMode.ModeId;
            this.order.ModeName = shippingMode.Name;
            if (OrderHelper.UpdateOrderShippingMode(this.order))
            {
                this.chargesList.LoadControls();
                this.shippingAddress.LoadControl();
                this.ShowMsg("修改配送方式成功", true);
            }
            else
            {
                this.ShowMsg("修改配送方式失败", false);
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtRemark.Text))
                {
                    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else
                {
                    this.order.OrderId = this.spanOrderId.Text;
                    if (this.orderRemarkImageForRemark.SelectedItem != null)
                    {
                        this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                    }
                    this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                    if (OrderHelper.SaveRemark(this.order))
                    {
                        this.BindRemark(this.order);
                        this.ShowMsg("保存备忘录成功", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败", false);
                    }
                }
            }
        }

        private void LoadUserControl(OrderInfo order)
        {
            this.itemsList.Order = order;
            this.chargesList.Order = order;
            this.shippingAddress.Order = order;
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
                this.btnMondifyPay.Click += new EventHandler(this.btnMondifyPay_Click);
                this.btnMondifyShip.Click += new EventHandler(this.btnMondifyShip_Click);
                this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
                this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
                this.order = OrderHelper.GetOrderInfo(this.orderId);
                this.LoadUserControl(this.order);
                if (!this.Page.IsPostBack)
                {
                    this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
                    this.litOrderId.Text = this.order.OrderId;
                    this.litUserName.Text = this.order.Username;
                    this.litRealName.Text = this.order.RealName;
                    this.litUserTel.Text = this.order.TelPhone;
                    this.litUserEmail.Text = this.order.EmailAddress;
                    if (((int) this.lblOrderStatus.OrderStatusCode) != 4)
                    {
                        this.lbCloseReason.Visible = false;
                    }
                    else
                    {
                        this.lbReason.Text = this.order.CloseReason;
                    }
                    if (((this.order.OrderStatus != OrderStatus.WaitBuyerPay) && (this.order.OrderStatus != OrderStatus.Closed)) && (this.order.Gateway != "hishop.plugins.payment.podrequest"))
                    {
                        this.litPayTime.Text = "付款时间：" + this.order.PayDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (((this.order.OrderStatus == OrderStatus.SellerAlreadySent) || (this.order.OrderStatus == OrderStatus.Finished)) || (((this.order.OrderStatus == OrderStatus.Returned) || (this.order.OrderStatus == OrderStatus.ApplyForReturns)) || (this.order.OrderStatus == OrderStatus.ApplyForReplacement)))
                    {
                        this.litSendGoodTime.Text = "发货时间：" + this.order.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (this.order.OrderStatus == OrderStatus.Finished)
                    {
                        this.litFinishTime.Text = "完成时间：" + this.order.FinishDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if ((this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid) || ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) && (this.order.Gateway == "hishop.plugins.payment.podrequest")))
                    {
                        this.lkbtnSendGoods.Visible = true;
                    }
                    else
                    {
                        this.lkbtnSendGoods.Visible = false;
                    }
                    if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
                    {
                        this.lbtnClocsOrder.Visible = true;
                        this.lkbtnEditPrice.Visible = true;
                    }
                    else
                    {
                        this.lbtnClocsOrder.Visible = false;
                        this.lkbtnEditPrice.Visible = false;
                    }
                    this.lkbtnEditPrice.NavigateUrl = "javascript:DialogFrame('" + Globals.ApplicationPath + "/Admin/sales/EditOrder.aspx?OrderId=" + this.orderId + "','修改订单价格',null,null)";
                    this.BindRemark(this.order);
                    this.ddlshippingMode.DataBind();
                    this.ddlshippingMode.SelectedValue = new int?(this.order.ShippingModeId);
                    this.ddlpayment.DataBind();
                    this.ddlpayment.SelectedValue = new int?(this.order.PaymentTypeId);
                    if ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) || (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                    {
                        this.hlkOrderGifts.Visible = true;
                        if (this.order.Gifts.Count > 0)
                        {
                            this.hlkOrderGifts.Text = "编辑订单礼品";
                        }
                        this.hlkOrderGifts.NavigateUrl = "javascript:DialogFrameClose('" + Globals.ApplicationPath + "/Admin/sales/OrderGifts.aspx?OrderId=" + this.order.OrderId + "','编辑订单礼品',null,null);";
                    }
                    else
                    {
                        this.hlkOrderGifts.Visible = false;
                    }
                }
            }
        }
    }
}

