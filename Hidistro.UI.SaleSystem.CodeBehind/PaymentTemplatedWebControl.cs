namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class PaymentTemplatedWebControl : HtmlTemplatedWebControl
    {
        protected decimal Amount;
        protected string Gateway;
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        protected OrderInfo Order;
        protected string OrderId;

        public PaymentTemplatedWebControl(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!this.isBackRequest)
            {
                if (!base.LoadHtmlThemedControl())
                {
                    throw new SkinNotFoundException(this.SkinPath);
                }
                this.AttachChildControls();
            }
            this.DoValidate();
        }

        protected abstract void DisplayMessage(string status);
        private void DoValidate()
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(this.Page.Request.Form);
            values2.Add(this.Page.Request.QueryString);
            NameValueCollection parameters = values2;
            this.Gateway = this.Page.Request.QueryString["HIGW"];
            this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { this.Gateway })) + "?" + this.Page.Request.Url.Query;
            }
            this.OrderId = this.Notify.GetOrderId();
            this.Order = TradeHelper.GetOrderInfo(this.OrderId);
            if (this.Order == null)
            {
                this.ResponseStatus(true, "ordernotfound");
            }
            else
            {
                this.Amount = this.Notify.GetOrderAmount();
                if (this.Amount <= 0M)
                {
                    this.Amount = this.Order.GetTotal();
                }
                this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
                PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.Order.PaymentTypeId);
                if (paymentMode == null)
                {
                    this.ResponseStatus(true, "gatewaynotfound");
                }
                else
                {
                    this.Notify.Finished += new EventHandler<FinishedEventArgs>(this.Notify_Finished);
                    this.Notify.NotifyVerifyFaild += new EventHandler(this.Notify_NotifyVerifyFaild);
                    this.Notify.Payment += new EventHandler(this.Notify_Payment);
                    this.Notify.VerifyNotify(0x7530, HiCryptographer.Decrypt(paymentMode.Settings));
                }
            }
        }

        private void FinishOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.Finished)
            {
                this.ResponseStatus(true, "success");
            }
            else if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                this.ResponseStatus(false, "fail");
            }
        }

        private void Notify_Finished(object sender, FinishedEventArgs e)
        {
            if (e.IsMedTrade)
            {
                this.FinishOrder();
            }
            else
            {
                this.UserPayOrder();
            }
        }

        private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
            this.ResponseStatus(false, "verifyfaild");
        }

        private void Notify_Payment(object sender, EventArgs e)
        {
            this.UserPayOrder();
        }

        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(HiContext.Current.Context, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }

        private void UserPayOrder()
        {
            if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                int maxCount = 0;
                int orderCount = 0;
                int groupBuyOerderNumber = 0;
                if (this.Order.GroupBuyId > 0)
                {
                    GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
                    if ((groupBuy == null) || (groupBuy.Status != GroupBuyStatus.UnderWay))
                    {
                        this.ResponseStatus(false, "groupbuyalreadyfinished");
                        return;
                    }
                    orderCount = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
                    groupBuyOerderNumber = this.Order.GetGroupBuyOerderNumber();
                    maxCount = groupBuy.MaxCount;
                    if (maxCount < (orderCount + groupBuyOerderNumber))
                    {
                        this.ResponseStatus(false, "exceedordermax");
                        return;
                    }
                }
                if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
                {
                    DebitNote note = new DebitNote();
                    note.NoteId = Globals.GetGenerateId();
                    note.OrderId = this.Order.OrderId;
                    note.Operator = this.Order.Username;
                    note.Remark = "客户订单在线支付成功";
                    TradeHelper.SaveDebitNote(note);
                    if ((this.Order.GroupBuyId > 0) && (maxCount == (orderCount + groupBuyOerderNumber)))
                    {
                        TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
                    }
                    if ((this.Order.UserId != 0) && (this.Order.UserId != 0x44c))
                    {
                        IUser user = Users.GetUser(this.Order.UserId);
                        if ((user != null) && ((user.UserRole == UserRole.Member) || (user.UserRole == UserRole.Underling)))
                        {
                            Messenger.OrderPayment(user, this.Order.OrderId, this.Order.GetTotal());
                        }
                    }
                    this.Order.OnPayment();
                    this.ResponseStatus(true, "success");
                }
                else
                {
                    this.ResponseStatus(false, "fail");
                }
            }
        }
    }
}

