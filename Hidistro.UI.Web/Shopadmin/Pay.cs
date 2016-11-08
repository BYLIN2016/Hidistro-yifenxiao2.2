namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using System;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    public class Pay : DistributorPage
    {
        protected HtmlGenericControl abbrInfo;
        protected Button btnBack;
        protected Button btnBack1;
        protected Button btnConfirmPay;
        protected ImageButton imgBtnBack;
        protected FormatedTimeLabel lblPurchaseDate;
        protected FormatedMoneyLabel lblTotalPrice;
        protected FormatedMoneyLabel lblUseableBalance;
        protected Literal litorder;
        protected Literal litOrderId;
        protected Literal litPurchaseOrderId;
        protected HtmlGenericControl PaySucceess;
        private PurchaseOrderInfo purchaseOrder;
        private string purchaseOrderId;
        protected TextBox txtTradePassword;

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!this.purchaseOrder.IsManualPurchaseOrder)
            {
                base.Response.Redirect("MyPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
            }
            else
            {
                base.Response.Redirect("ManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
            }
        }

        private void btnConfirmPay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTradePassword.Text))
            {
                this.ShowMsg("请输入交易密码", false);
            }
            else
            {
                int num;
                int.TryParse(base.Request["PayMode"], out num);
                SubsiteStoreHelper.GetPaymentMode(num);
                if (((decimal) this.lblUseableBalance.Money) < ((decimal) this.lblTotalPrice.Money))
                {
                    this.ShowMsg("您的预付款金额不足", false);
                }
                else
                {
                    Distributor user = SubsiteStoreHelper.GetDistributor();
                    if ((user.Balance - user.RequestBalance) < this.purchaseOrder.GetPurchaseTotal())
                    {
                        this.ShowMsg("您的预付款金额不足", false);
                    }
                    else
                    {
                        BalanceDetailInfo balance = new BalanceDetailInfo();
                        balance.UserId = user.UserId;
                        balance.UserName = user.Username;
                        balance.TradeType = TradeTypes.Consume;
                        balance.TradeDate = DateTime.Now;
                        balance.Expenses = new decimal?(this.purchaseOrder.GetPurchaseTotal());
                        balance.Balance = user.Balance - this.purchaseOrder.GetPurchaseTotal();
                        balance.Remark = string.Format("采购单{0}的付款", this.purchaseOrder.PurchaseOrderId);
                        user.TradePassword = this.txtTradePassword.Text;
                        if (Users.ValidTradePassword(user))
                        {
                            if (!SubsiteSalesHelper.ConfirmPay(balance, this.purchaseOrder))
                            {
                                this.ShowMsg("付款失败", false);
                            }
                            else
                            {
                                PurchaseDebitNote note = new PurchaseDebitNote();
                                note.NoteId = Globals.GetGenerateId();
                                note.PurchaseOrderId = this.purchaseOrderId;
                                note.Operator = HiContext.Current.User.Username;
                                note.Remark = "分销商采购单预付款支付成功";
                                SubsiteSalesHelper.SavePurchaseDebitNote(note);
                                this.ShowMsg("采购单预付款支付成功", true);
                            }
                        }
                        else
                        {
                            this.ShowMsg("交易密码错误", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnConfirmPay.Click += new EventHandler(this.btnConfirmPay_Click);
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnBack1.Click += new EventHandler(this.btnBack_Click);
            this.imgBtnBack.Click += new ImageClickEventHandler(this.btnBack_Click);
            if (string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseOrderId = base.Request.QueryString["PurchaseOrderId"];
                this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                if (!base.IsPostBack)
                {
                    if (this.purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        int num;
                        int.TryParse(base.Request["PayMode"], out num);
                        PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(num);
                        if ((num > 0) && (paymentMode.Gateway != "hishop.plugins.payment.advancerequest"))
                        {
                            SubsiteStoreHelper.GetDistributor();
                            string showUrl = Globals.FullPath(Globals.GetSiteUrls().Home);
                            if (paymentMode.Gateway.ToLower() != "hishop.plugins.payment.podrequest")
                            {
                                showUrl = base.Server.UrlEncode(string.Format("http://{0}/shopadmin/purchaseorder/MyPurchaseOrderDetails.aspx?purchaseOrderId={1}", base.Request.Url.Host, this.purchaseOrder.PurchaseOrderId));
                            }
                            if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
                            {
                                showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("bank_pay", new object[] { this.purchaseOrder.PurchaseOrderId }));
                            }
                            if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
                            {
                                showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("advance_pay", new object[] { this.purchaseOrder.PurchaseOrderId }));
                            }
                            string attach = "";
                            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                            {
                                attach = cookie.Value;
                            }
                            PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.purchaseOrder.PurchaseOrderId, this.purchaseOrder.GetPurchaseTotal(), "采购单支付", "采购单号-" + this.purchaseOrder.PurchaseOrderId, "", this.purchaseOrder.PurchaseDate, showUrl, Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentNotify_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
                        }
                        if (this.purchaseOrder.IsManualPurchaseOrder)
                        {
                            this.litorder.Visible = false;
                            this.litOrderId.Visible = false;
                        }
                        else
                        {
                            this.litOrderId.Text = this.purchaseOrder.OrderId;
                        }
                        this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
                        this.lblPurchaseDate.Time = this.purchaseOrder.PurchaseDate;
                        this.lblTotalPrice.Money = this.purchaseOrder.GetPurchaseTotal();
                        AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                        this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
                    }
                }
            }
        }
    }
}

