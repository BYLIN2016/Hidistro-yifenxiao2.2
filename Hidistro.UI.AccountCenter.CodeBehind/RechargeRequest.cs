namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class RechargeRequest : MemberTemplatedWebControl
    {
        private IButton btnReCharge;
        private FormatedMoneyLabel litUseableBalance;
        private Literal litUserName;
        private RadioButtonList rbtnPaymentMode;
        private TextBox txtReChargeBalance;

        protected override void AttachChildControls()
        {
            this.litUserName = (Literal) this.FindControl("litUserName");
            this.rbtnPaymentMode = (RadioButtonList) this.FindControl("rbtnPaymentMode");
            this.txtReChargeBalance = (TextBox) this.FindControl("txtReChargeBalance");
            this.btnReCharge = ButtonManager.Create(this.FindControl("btnReCharge"));
            this.litUseableBalance = (FormatedMoneyLabel) this.FindControl("litUseableBalance");
            PageTitle.AddSiteNameTitle("预付款充值", HiContext.Current.Context);
            this.btnReCharge.Click += new EventHandler(this.btnReCharge_Click);
            if (!this.Page.IsPostBack)
            {
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", HttpContext.Current.Request.Url));
                }
                this.BindPaymentMode();
                this.litUserName.Text = HiContext.Current.User.Username;
                this.litUseableBalance.Money = user.Balance - user.RequestBalance;
            }
        }

        private void BindPaymentMode()
        {
            IList<PaymentModeInfo> paymentModes = TradeHelper.GetPaymentModes();
            if (paymentModes.Count > 0)
            {
                foreach (PaymentModeInfo info in paymentModes)
                {
                    string str = info.Gateway.ToLower();
                    if ((info.IsUseInpour && !str.Equals("hishop.plugins.payment.advancerequest")) && !str.Equals("hishop.plugins.payment.bankrequest"))
                    {
                        if (str.Equals("hishop.plugins.payment.alipay_shortcut.shortcutrequest"))
                        {
                            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                            {
                                this.rbtnPaymentMode.Items.Add(new ListItem(info.Name, info.ModeId.ToString()));
                            }
                        }
                        else
                        {
                            this.rbtnPaymentMode.Items.Add(new ListItem(info.Name, info.ModeId.ToString()));
                        }
                    }
                }
                this.rbtnPaymentMode.SelectedIndex = 0;
            }
        }

        protected void btnReCharge_Click(object sender, EventArgs e)
        {
            if (this.rbtnPaymentMode.Items.Count == 0)
            {
                this.ShowMessage("无法充值,因为后台没有添加支付方式", false);
            }
            else if (this.rbtnPaymentMode.SelectedValue == null)
            {
                this.ShowMessage("选择要充值使用的支付方式", false);
            }
            else
            {
                decimal num;
                int length = 0;
                if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
                {
                    length = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
                }
                if ((!decimal.TryParse(this.txtReChargeBalance.Text, out num) || (decimal.Parse(this.txtReChargeBalance.Text) <= 0M)) || (length > 2))
                {
                    this.ShowMessage("请输入大于0的充值金额且金额整数位数在1到10之间,且不能超过2位小数", false);
                }
                else
                {
                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_RechargeConfirm", new object[] { this.Page.Server.UrlEncode(this.rbtnPaymentMode.SelectedValue), this.Page.Server.UrlEncode(this.txtReChargeBalance.Text) }));
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-RechargeRequest.html";
            }
            base.OnInit(e);
        }
    }
}

