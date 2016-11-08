namespace Hidistro.UI.Web.Pay
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class DistributorPaymentNotify_url : DistributorPage
    {
        protected decimal Amount;
        protected string Gateway;
        private readonly bool isBackRequest;
        protected Literal litMessage;
        private PaymentNotify Notify;
        private PurchaseOrderInfo PurchaseOrder;
        protected string PurchaseOrderId;

        public DistributorPaymentNotify_url()
        {
            this.isBackRequest = false;
        }

        public DistributorPaymentNotify_url(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected void DisplayMessage(string status)
        {
            switch (status)
            {
                case "ordernotfound":
                    this.litMessage.Text = string.Format("没有找到对应的采购单信息，采购单号：{0}", this.PurchaseOrderId);
                    return;

                case "gatewaynotfound":
                    this.litMessage.Text = "没有找到与此采购单对应的支付方式，系统无法自动完成操作，请联系管理员";
                    return;

                case "verifyfaild":
                    this.litMessage.Text = "支付返回验证失败，操作已停止";
                    return;

                case "success":
                    this.litMessage.Text = string.Format("恭喜您，采购单已成功完成支付：{0}</br>支付金额：{1}", this.PurchaseOrderId, this.Amount.ToString("F"));
                    return;

                case "exceedordermax":
                    this.litMessage.Text = "订单为团购订单，订购数量超过订购总数，支付失败";
                    return;

                case "groupbuyalreadyfinished":
                    this.litMessage.Text = "订单为团购订单，团购活动已结束，支付失败";
                    return;

                case "fail":
                    this.litMessage.Text = string.Format("订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
                    return;
            }
            this.litMessage.Text = "未知错误，操作已停止";
        }

        private void FinishOrder()
        {
            if (this.PurchaseOrder.PurchaseStatus == OrderStatus.Finished)
            {
                this.ResponseStatus(true, "success");
            }
            else if (this.PurchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE) && SubsiteSalesHelper.ConfirmPurchaseOrderFinish(this.PurchaseOrder))
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

        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection values2 = new NameValueCollection();
            values2.Add(this.Page.Request.Form);
            values2.Add(this.Page.Request.QueryString);
            NameValueCollection parameters = values2;
            this.Gateway = this.Page.Request.QueryString["HIGW"];
            this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
            if (this.isBackRequest)
            {
                this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentReturn_url", new object[] { this.Gateway })) + "?" + this.Page.Request.Url.Query;
            }
            this.PurchaseOrderId = this.Notify.GetOrderId();
            this.PurchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.PurchaseOrderId);
            if (this.PurchaseOrder == null)
            {
                this.ResponseStatus(true, "purchaseordernotfound");
            }
            else
            {
                this.Amount = this.Notify.GetOrderAmount();
                if (this.Amount <= 0M)
                {
                    this.Amount = this.PurchaseOrder.GetPurchaseTotal();
                }
                PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.PurchaseOrder.PaymentTypeId);
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

        private void ResponseStatus(bool success, string status)
        {
            this.DisplayMessage(status);
        }

        private void UserPayOrder()
        {
            if (this.PurchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
            {
                this.ResponseStatus(true, "success");
            }
            else if (this.PurchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY) && SubsiteSalesHelper.ConfirmPay(this.PurchaseOrder))
            {
                PurchaseDebitNote note = new PurchaseDebitNote();
                note.NoteId = Globals.GetGenerateId();
                note.PurchaseOrderId = this.PurchaseOrder.PurchaseOrderId;
                note.Operator = this.PurchaseOrder.Distributorname;
                note.Remark = "分销商采购单在线支付成功";
                SubsiteSalesHelper.SavePurchaseDebitNote(note);
                this.ResponseStatus(true, "success");
            }
        }
    }
}

