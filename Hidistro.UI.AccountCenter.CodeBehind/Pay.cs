namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class Pay : MemberTemplatedWebControl
    {
        private IButton btnPay;
        private FormatedMoneyLabel lblOrderAmount;
        private Label lblOrderId;
        private FormatedMoneyLabel litUseableBalance;
        private string orderId;
        private TextBox txtPassword;

        protected override void AttachChildControls()
        {
            this.lblOrderId = (Label) this.FindControl("lblOrderId");
            this.lblOrderAmount = (FormatedMoneyLabel) this.FindControl("lblOrderAmount");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.litUseableBalance = (FormatedMoneyLabel) this.FindControl("litUseableBalance");
            this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
            this.orderId = this.Page.Request.QueryString["orderId"];
            PageTitle.AddSiteNameTitle("订单支付", HiContext.Current.Context);
            this.btnPay.Click += new EventHandler(this.btnPay_Click);
            if (string.IsNullOrEmpty(this.orderId))
            {
                base.GotoResourceNotFound();
            }
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
                OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
                if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
                {
                    this.ShowMessage("当前的订单订单状态不是等待付款，所以不能支付", false);
                    this.btnPay.Visible = false;
                }
                this.lblOrderId.Text = orderInfo.OrderId;
                this.lblOrderAmount.Money = orderInfo.GetTotal();
                this.litUseableBalance.Money = user.Balance - user.RequestBalance;
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
            int maxCount = 0;
            int orderCount = 0;
            int groupBuyOerderNumber = 0;
            if (orderInfo.CountDownBuyId > 0)
            {
                CountDownInfo countDownBuy = TradeHelper.GetCountDownBuy(orderInfo.CountDownBuyId);
                if ((countDownBuy == null) || (countDownBuy.EndDate < DateTime.Now))
                {
                    this.ShowMessage("当前的订单为限时抢购订单，此活动已结束，所以不能支付", false);
                    return;
                }
            }
            if (orderInfo.GroupBuyId > 0)
            {
                GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(orderInfo.GroupBuyId);
                if ((groupBuy == null) || (groupBuy.Status != GroupBuyStatus.UnderWay))
                {
                    this.ShowMessage("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
                    return;
                }
                orderCount = TradeHelper.GetOrderCount(orderInfo.GroupBuyId);
                groupBuyOerderNumber = orderInfo.GetGroupBuyOerderNumber();
                maxCount = groupBuy.MaxCount;
                if (maxCount < (orderCount + groupBuyOerderNumber))
                {
                    this.ShowMessage("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
                    return;
                }
            }
            if (!orderInfo.CheckAction(OrderActions.BUYER_PAY))
            {
                this.ShowMessage("当前的订单订单状态不是等待付款，所以不能支付", false);
            }
            else if (HiContext.Current.User.UserId != orderInfo.UserId)
            {
                this.ShowMessage("预付款只能为自己下的订单付款,查一查该订单是不是你的", false);
            }
            else if (((decimal) this.litUseableBalance.Money) < orderInfo.GetTotal())
            {
                this.ShowMessage("预付款余额不足,支付失败", false);
            }
            else
            {
                IUser user = HiContext.Current.User;
                user.TradePassword = this.txtPassword.Text;
                if (Users.ValidTradePassword(user))
                {
                    foreach (LineItemInfo info4 in orderInfo.LineItems.Values)
                    {
                        if (ShoppingProcessor.GetStock(info4.ProductId, info4.SkuId) < info4.ShipmentQuantity)
                        {
                            this.ShowMessage("订单中商品库存不足，禁止支付！", false);
                            return;
                        }
                    }
                    if (TradeHelper.UserPayOrder(orderInfo, true))
                    {
                        DebitNote note = new DebitNote();
                        note.NoteId = Globals.GetGenerateId();
                        note.OrderId = this.orderId;
                        note.Operator = HiContext.Current.User.Username;
                        note.Remark = "客户预付款订单支付成功";
                        TradeHelper.SaveDebitNote(note);
                        if ((orderInfo.GroupBuyId > 0) && (maxCount == (orderCount + groupBuyOerderNumber)))
                        {
                            TradeHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                        }
                        Messenger.OrderPayment(user, orderInfo.OrderId, orderInfo.GetTotal());
                        orderInfo.OnPayment();
                        this.Page.Response.Redirect(Globals.ApplicationPath + "/user/PaySucceed.aspx?orderId=" + this.orderId);
                    }
                    else
                    {
                        this.ShowMessage(string.Format("对订单{0} 支付失败", orderInfo.OrderId), false);
                    }
                }
                else
                {
                    this.ShowMessage("交易密码有误，请重试", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-Pay.html";
            }
            base.OnInit(e);
        }
    }
}

