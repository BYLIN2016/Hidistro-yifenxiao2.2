namespace Hidistro.UI.AccountCenter.CodeBehind
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
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class RechargeConfirm : MemberTemplatedWebControl
    {
        private decimal balance;
        private IButton btnConfirm;
        private HiImage imgPayment;
        private FormatedMoneyLabel lblBlance;
        private Literal lblPaymentName;
        private Literal litPayCharge;
        private Literal litUserName;
        private int paymentModeId;

        protected override void AttachChildControls()
        {
            int.TryParse(this.Page.Request.QueryString["modeId"], out this.paymentModeId);
            decimal.TryParse(this.Page.Request.QueryString["blance"], out this.balance);
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.lblPaymentName = (Literal) this.FindControl("lblPaymentName");
            this.imgPayment = (HiImage) this.FindControl("imgPayment");
            this.lblBlance = (FormatedMoneyLabel) this.FindControl("lblBlance");
            this.litPayCharge = (Literal) this.FindControl("litPayCharge");
            this.btnConfirm = ButtonManager.Create(this.FindControl("btnConfirm"));
            PageTitle.AddSiteNameTitle("充值确认", HiContext.Current.Context);
            this.btnConfirm.Click += new EventHandler(this.btnConfirm_Click);
            if (!this.Page.IsPostBack)
            {
                if ((this.paymentModeId == 0) || (this.balance == 0M))
                {
                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_InpourRequest"));
                }
                else
                {
                    PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
                    this.litUserName.Text = HiContext.Current.User.Username;
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.paymentModeId);
            InpourRequestInfo info3 = new InpourRequestInfo();
            info3.InpourId = this.GenerateInpourId();
            info3.TradeDate = DateTime.Now;
            info3.InpourBlance = this.balance;
            info3.UserId = HiContext.Current.User.UserId;
            info3.PaymentId = paymentMode.ModeId;
            InpourRequestInfo inpourRequest = info3;
            if (PersonalHelper.AddInpourBlance(inpourRequest))
            {
                string attach = "";
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                {
                    attach = cookie.Value;
                }
                string orderId = inpourRequest.InpourId.ToString(CultureInfo.InvariantCulture);
                PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderId, inpourRequest.InpourBlance + ((decimal) this.ViewState["PayCharge"]), "预付款充值", "操作流水号-" + orderId, HiContext.Current.User.Email, inpourRequest.TradeDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
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

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-RechargeConfirm.html";
            }
            base.OnInit(e);
        }
    }
}

