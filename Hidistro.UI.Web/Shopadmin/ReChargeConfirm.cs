namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;

    public class ReChargeConfirm : DistributorPage
    {
        private decimal balance;
        protected Button btnConfirm;
        protected FormatedMoneyLabel lblBlance;
        protected Literal lblPaymentName;
        protected Literal litPayCharge;
        protected Literal litRealName;
        private int paymentModeId;

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.paymentModeId);
            InpourRequestInfo info3 = new InpourRequestInfo();
            info3.InpourId = this.GenerateInpourId();
            info3.TradeDate = DateTime.Now;
            info3.InpourBlance = this.balance;
            info3.UserId = HiContext.Current.User.UserId;
            info3.PaymentId = paymentMode.ModeId;
            InpourRequestInfo inpourRequest = info3;
            if (SubsiteStoreHelper.AddInpourBalance(inpourRequest))
            {
                string attach = "";
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    attach = cookie.Value;
                }
                string orderId = inpourRequest.InpourId.ToString(CultureInfo.InvariantCulture);
                PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderId, inpourRequest.InpourBlance + paymentMode.CalcPayCharge(inpourRequest.InpourBlance), "预付款充值", "操作流水号-" + orderId, HiContext.Current.User.Email, inpourRequest.TradeDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
            }
        }

        private string GenerateInpourId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(this.Page.Request.QueryString["modeId"], out this.paymentModeId);
            decimal.TryParse(this.Page.Request.QueryString["blance"], out this.balance);
            this.btnConfirm.Click += new EventHandler(this.btnConfirm_Click);
            if ((!this.Page.IsPostBack && (this.paymentModeId > 0)) && (this.balance > 0M))
            {
                PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.paymentModeId);
                this.litRealName.Text = HiContext.Current.User.Username;
                if (paymentMode != null)
                {
                    this.lblPaymentName.Text = paymentMode.Name;
                    this.lblBlance.Money = this.balance;
                    this.ViewState["PayCharge"] = paymentMode.CalcPayCharge(this.balance);
                    this.litPayCharge.Text = Globals.FormatMoney(paymentMode.CalcPayCharge(this.balance));
                }
            }
        }
    }
}

