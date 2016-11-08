namespace Hidistro.UI.Web
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hishop.Plugins;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class SendPayment : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string orderId = this.Page.Request.QueryString["orderId"];
            OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
            if (orderInfo == null)
            {
                base.Response.Write("<div><font color='red'>您要付款的订单已经不存在，请联系管理员确定</font></div>");
            }
            else if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
            {
                this.Page.Response.Write("订单当前状态不能支付");
            }
            else
            {
                if (orderInfo.CountDownBuyId > 0)
                {
                    CountDownInfo countDownInfoByCountDownId = ProductBrowser.GetCountDownInfoByCountDownId(orderInfo.CountDownBuyId);
                    if ((countDownInfoByCountDownId == null) || (countDownInfoByCountDownId.EndDate < DateTime.Now))
                    {
                        this.Page.Response.Write("此订单属于限时抢购类型订单，但限时抢购活动已经结束或不存在");
                        return;
                    }
                }
                PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                if (paymentMode == null)
                {
                    base.Response.Write("<div><font color='red'>您之前选择的支付方式已经不存在，请联系管理员修改支付方式</font></div>");
                }
                else
                {
                    foreach (LineItemInfo info4 in orderInfo.LineItems.Values)
                    {
                        ProductBrowseInfo info5 = ProductBrowser.GetProductBrowseInfo(info4.ProductId, 6, 6);
                        if ((info5.Product == null) || (info5.Product.SaleStatus == ProductSaleStatus.Delete))
                        {
                            base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已经被管理员删除"));
                            return;
                        }
                        if (info5.Product.SaleStatus == ProductSaleStatus.OnStock)
                        {
                            base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已入库"));
                            return;
                        }
                        int stock = info5.Product.Stock;
                        if (info4.ShipmentQuantity > stock)
                        {
                            base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品库存不足"));
                            return;
                        }
                    }
                    string showUrl = Globals.GetSiteUrls().UrlData.FormatUrl("user_UserOrders");
                    if (paymentMode.Gateway.ToLower() != "hishop.plugins.payment.podrequest")
                    {
                        showUrl = base.Server.UrlEncode(string.Format("http://{0}/user/OrderDetails.aspx?OrderId={1}", base.Request.Url.Host, orderInfo.OrderId));
                    }
                    if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
                    {
                        showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("bank_pay", new object[] { orderInfo.OrderId }));
                    }
                    if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
                    {
                        showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("advance_pay", new object[] { orderInfo.OrderId }));
                    }
                    string attach = "";
                    HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
                    if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
                    {
                        attach = cookie.Value;
                    }
                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), attach).SendRequest();
                }
            }
        }
    }
}

