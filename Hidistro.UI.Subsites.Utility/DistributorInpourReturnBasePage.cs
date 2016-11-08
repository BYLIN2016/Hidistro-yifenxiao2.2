namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class DistributorInpourReturnBasePage : DistributorPage
    {
        protected decimal Amount;
        protected string Gateway;
        protected string InpourId;
        protected InpourRequestInfo InpourRequest;
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        private PaymentModeInfo paymode;

        public DistributorInpourReturnBasePage(bool _isBackRequest)
        {
            this.isBackRequest = _isBackRequest;
        }

        protected override void CreateChildControls()
        {
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
                this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[] { this.Gateway }));
                this.Notify.ReturnUrl = this.Notify.ReturnUrl + "?" + this.Page.Request.Url.Query;
            }
            this.InpourId = this.Notify.GetOrderId();
            this.Amount = this.Notify.GetOrderAmount();
            this.InpourRequest = SubsiteStoreHelper.GetInpouRequest(this.InpourId);
            if (this.InpourRequest == null)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                this.Amount = this.InpourRequest.InpourBlance;
                this.paymode = SubsiteStoreHelper.GetPaymentMode(this.InpourRequest.PaymentId);
                if (this.paymode == null)
                {
                    this.ResponseStatus(true, "gatewaynotfound");
                }
                else
                {
                    this.Notify.Finished += new EventHandler<FinishedEventArgs>(this.Notify_Finished);
                    this.Notify.NotifyVerifyFaild += new EventHandler(this.Notify_NotifyVerifyFaild);
                    this.Notify.Payment += new EventHandler(this.Notify_Payment);
                    this.Notify.VerifyNotify(0x7530, HiCryptographer.Decrypt(this.paymode.Settings));
                }
            }
        }

        private void Notify_Finished(object sender, FinishedEventArgs e)
        {
            DateTime now = DateTime.Now;
            TradeTypes selfhelpInpour = TradeTypes.SelfhelpInpour;
            Distributor user = Users.GetUser(this.InpourRequest.UserId, false) as Distributor;
            decimal num = user.Balance + this.InpourRequest.InpourBlance;
            BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
            balanceDetails.UserId = this.InpourRequest.UserId;
            balanceDetails.UserName = user.Username;
            balanceDetails.TradeDate = now;
            balanceDetails.TradeType = selfhelpInpour;
            balanceDetails.Income = new decimal?(this.InpourRequest.InpourBlance);
            balanceDetails.Balance = num;
            balanceDetails.InpourId = this.InpourRequest.InpourId;
            if (this.paymode != null)
            {
                balanceDetails.Remark = "充值支付方式：" + this.paymode.Name;
            }
            if (SubsiteStoreHelper.Recharge(balanceDetails))
            {
                Users.ClearUserCache(user);
                this.ResponseStatus(true, "success");
            }
            else
            {
                SubsiteStoreHelper.RemoveInpourRequest(this.InpourId);
                this.ResponseStatus(false, "fail");
            }
        }

        private void Notify_NotifyVerifyFaild(object sender, EventArgs e)
        {
            this.ResponseStatus(false, "verifyfaild");
        }

        private void Notify_Payment(object sender, EventArgs e)
        {
            this.ResponseStatus(false, "waitconfirm");
        }

        private void ResponseStatus(bool success, string status)
        {
            if (this.isBackRequest)
            {
                this.Notify.WriteBack(this.Context, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }
    }
}

