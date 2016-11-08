namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hishop.Plugins;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;

    [ParseChildren(true), PersistChildren(false)]
    public abstract class InpourTemplatedWebControl : HtmlTemplatedWebControl
    {
        protected decimal Amount;
        protected string Gateway;
        protected string InpourId;
        protected InpourRequestInfo InpourRequest;
        private readonly bool isBackRequest;
        protected PaymentNotify Notify;
        protected PaymentModeInfo paymode;

        public InpourTemplatedWebControl(bool _isBackRequest)
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
                this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourReturn_url", new object[] { this.Gateway })) + "?" + this.Page.Request.Url.Query;
            }
            this.InpourId = this.Notify.GetOrderId();
            this.Amount = this.Notify.GetOrderAmount();
            this.InpourRequest = PersonalHelper.GetInpourBlance(this.InpourId);
            if (this.InpourRequest == null)
            {
                this.ResponseStatus(true, "success");
            }
            else
            {
                this.Amount = this.InpourRequest.InpourBlance;
                this.paymode = TradeHelper.GetPaymentMode(this.InpourRequest.PaymentId);
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
            Member user = Users.GetUser(this.InpourRequest.UserId, false) as Member;
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
            if (PersonalHelper.Recharge(balanceDetails))
            {
                Users.ClearUserCache(user);
                this.ResponseStatus(true, "success");
            }
            else
            {
                PersonalHelper.RemoveInpourRequest(this.InpourId);
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
                this.Notify.WriteBack(HiContext.Current.Context, success);
            }
            else
            {
                this.DisplayMessage(status);
            }
        }
    }
}

