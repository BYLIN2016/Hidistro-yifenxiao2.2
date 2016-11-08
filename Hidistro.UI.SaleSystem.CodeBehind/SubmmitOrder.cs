namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SubmmitOrder : HtmlTemplatedWebControl
    {
        private IButton btnCreateOrder;
        private int bundlingid;
        private decimal bundlingprice;
        private int buyAmount;
        private string buytype = "";
        private Common_SubmmintOrder_GiftList cartGiftList;
        private Common_SubmmintOrder_ProductList cartProductList;
        private HtmlInputCheckBox chkTax;
        private HtmlSelect CmbCoupCode;
        private RegionSelector dropRegions;
        private HtmlSelect drpShipToDate;
        private HtmlInputHidden hdbuytype;
        private HyperLink hlkFeeFreight;
        private HyperLink hlkReducedPromotion;
        private HyperLink hlkSentTimesPoint;
        private HtmlInputHidden htmlCouponCode;
        private HtmlInputHidden inputPaymentModeId;
        private HtmlInputHidden inputShippingModeId;
        private bool isBundling;
        private bool isCountDown;
        private bool isGroupBuy;
        private bool isSignBuy;
        private FormatedMoneyLabel lblOrderTotal;
        private FormatedMoneyLabel lblPaymentPrice;
        private FormatedMoneyLabel lblShippModePrice;
        private FormatedMoneyLabel lblTotalPrice;
        private Label litAllWeight;
        private FormatedMoneyLabel litCouponAmout;
        private Label litPoint;
        private Literal litProductAmount;
        private Literal litProductBundling;
        private Label litTaxRate;
        private Panel pannel_useraddress;
        private Common_PaymentModeList paymentModeList;
        private string productSku;
        private Common_ShippingModeList shippingModeList;
        private ShoppingCartInfo shoppingCart;
        private HtmlTable tbCoupon;
        private TextBox txtAddress;
        private TextBox txtCellPhone;
        private TextBox txtInvoiceTitle;
        private TextBox txtMessage;
        private TextBox txtShipTo;
        private TextBox txtTelPhone;
        private TextBox txtZipcode;

        protected override void AttachChildControls()
        {
            this.dropRegions = (RegionSelector) this.FindControl("dropRegions");
            this.txtShipTo = (TextBox) this.FindControl("txtShipTo");
            this.txtAddress = (TextBox) this.FindControl("txtAddress");
            this.txtZipcode = (TextBox) this.FindControl("txtZipcode");
            this.txtCellPhone = (TextBox) this.FindControl("txtCellPhone");
            this.txtTelPhone = (TextBox) this.FindControl("txtTelPhone");
            this.txtInvoiceTitle = (TextBox) this.FindControl("txtInvoiceTitle");
            this.drpShipToDate = (HtmlSelect) this.FindControl("drpShipToDate");
            this.litTaxRate = (Label) this.FindControl("litTaxRate");
            this.shippingModeList = (Common_ShippingModeList) this.FindControl("Common_ShippingModeList");
            this.paymentModeList = (Common_PaymentModeList) this.FindControl("grd_Common_PaymentModeList");
            this.inputPaymentModeId = (HtmlInputHidden) this.FindControl("inputPaymentModeId");
            this.inputShippingModeId = (HtmlInputHidden) this.FindControl("inputShippingModeId");
            this.hdbuytype = (HtmlInputHidden) this.FindControl("hdbuytype");
            this.pannel_useraddress = (Panel) this.FindControl("pannel_useraddress");
            this.lblPaymentPrice = (FormatedMoneyLabel) this.FindControl("lblPaymentPrice");
            this.lblShippModePrice = (FormatedMoneyLabel) this.FindControl("lblShippModePrice");
            this.chkTax = (HtmlInputCheckBox) this.FindControl("chkTax");
            this.cartProductList = (Common_SubmmintOrder_ProductList) this.FindControl("Common_SubmmintOrder_ProductList");
            this.cartGiftList = (Common_SubmmintOrder_GiftList) this.FindControl("Common_SubmmintOrder_GiftList");
            this.litProductAmount = (Literal) this.FindControl("litProductAmount");
            this.litProductBundling = (Literal) this.FindControl("litProductBundling");
            this.litAllWeight = (Label) this.FindControl("litAllWeight");
            this.litPoint = (Label) this.FindControl("litPoint");
            this.hlkSentTimesPoint = (HyperLink) this.FindControl("hlkSentTimesPoint");
            this.lblOrderTotal = (FormatedMoneyLabel) this.FindControl("lblOrderTotal");
            this.txtMessage = (TextBox) this.FindControl("txtMessage");
            this.hlkFeeFreight = (HyperLink) this.FindControl("hlkFeeFreight");
            this.hlkReducedPromotion = (HyperLink) this.FindControl("hlkReducedPromotion");
            this.lblTotalPrice = (FormatedMoneyLabel) this.FindControl("lblTotalPrice");
            this.htmlCouponCode = (HtmlInputHidden) this.FindControl("htmlCouponCode");
            this.CmbCoupCode = (HtmlSelect) this.FindControl("CmbCoupCode");
            this.tbCoupon = (HtmlTable) this.FindControl("tbCoupon");
            this.litCouponAmout = (FormatedMoneyLabel) this.FindControl("litCouponAmout");
            this.btnCreateOrder = ButtonManager.Create(this.FindControl("btnCreateOrder"));
            this.btnCreateOrder.Click += new EventHandler(this.btnCreateOrder_Click);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!this.Page.IsPostBack)
            {
                this.BindUserAddress();
                this.shippingModeList.DataSource = ShoppingProcessor.GetShippingModes();
                this.shippingModeList.DataBind();
                this.ReBindPayment();
                if (this.shoppingCart != null)
                {
                    this.litTaxRate.Text = masterSettings.TaxRate.ToString(CultureInfo.InvariantCulture);
                    this.BindShoppingCartInfo(this.shoppingCart);
                    if ((this.isGroupBuy || this.isCountDown) || (this.isBundling || (this.shoppingCart.LineItems.Count == 0)))
                    {
                        this.tbCoupon.Visible = false;
                    }
                    this.CmbCoupCode.DataTextField = "DisplayText";
                    this.CmbCoupCode.DataValueField = "ClaimCode";
                    this.CmbCoupCode.DataSource = ShoppingProcessor.GetCoupon(this.shoppingCart.GetTotal());
                    this.CmbCoupCode.DataBind();
                    ListItem item = new ListItem("", "0");
                    this.CmbCoupCode.Items.Insert(0, item);
                    this.hdbuytype.Value = this.buytype;
                    this.pannel_useraddress.Visible = !HiContext.Current.User.IsAnonymous && (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) > 0);
                }
            }
        }

        private void BindShoppingCartInfo(ShoppingCartInfo shoppingCart)
        {
            if (shoppingCart.LineItems.Values.Count > 0)
            {
                this.cartProductList.DataSource = shoppingCart.LineItems.Values;
                this.cartProductList.DataBind();
                this.cartProductList.ShowProductCart();
            }
            if (shoppingCart.LineGifts.Count > 0)
            {
                IEnumerable<ShoppingCartGiftInfo> source = shoppingCart.LineGifts.Where<ShoppingCartGiftInfo>(delegate (ShoppingCartGiftInfo s) {
                    return s.PromoType == 0;
                });
                IEnumerable<ShoppingCartGiftInfo> enumerable2 = shoppingCart.LineGifts.Where<ShoppingCartGiftInfo>(delegate (ShoppingCartGiftInfo s) {
                    return s.PromoType == 5;
                });
                this.cartGiftList.DataSource = source;
                this.cartGiftList.FreeDataSource = enumerable2;
                this.cartGiftList.DataBind();
                this.cartGiftList.ShowGiftCart(source.Count<ShoppingCartGiftInfo>() > 0, enumerable2.Count<ShoppingCartGiftInfo>() > 0);
            }
            if (shoppingCart.IsReduced)
            {
                this.litProductAmount.Text = string.Format("商品金额：{0}", Globals.FormatMoney(shoppingCart.GetAmount()));
                this.hlkReducedPromotion.Text = shoppingCart.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCart.ReducedPromotionAmount.ToString("F2"));
                this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { shoppingCart.ReducedPromotionId });
            }
            if (shoppingCart.IsFreightFree)
            {
                this.hlkFeeFreight.Text = string.Format("（{0}）", shoppingCart.FreightFreePromotionName);
                this.hlkFeeFreight.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { shoppingCart.FreightFreePromotionId });
            }
            this.lblTotalPrice.Money = shoppingCart.GetTotal();
            this.lblOrderTotal.Money = shoppingCart.GetTotal();
            this.litPoint.Text = shoppingCart.GetPoint().ToString();
            if (this.isBundling)
            {
                BundlingInfo bundlingInfo = ProductBrowser.GetBundlingInfo(this.bundlingid);
                this.lblTotalPrice.Money = bundlingInfo.Price;
                this.lblOrderTotal.Money = bundlingInfo.Price;
                this.litPoint.Text = shoppingCart.GetPoint(bundlingInfo.Price).ToString();
                this.litProductBundling.Text = "（捆绑价）";
            }
            this.litAllWeight.Text = shoppingCart.Weight.ToString();
            if (shoppingCart.IsSendTimesPoint)
            {
                this.hlkSentTimesPoint.Text = string.Format("（{0}；送{1}倍）", shoppingCart.SentTimesPointPromotionName, shoppingCart.TimesPoint.ToString("F2"));
                this.hlkSentTimesPoint.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { shoppingCart.SentTimesPointPromotionId });
            }
        }

        private void BindUserAddress()
        {
            if (!HiContext.Current.User.IsAnonymous)
            {
                Member user = HiContext.Current.User as Member;
                this.txtShipTo.Text = user.RealName;
                this.dropRegions.SetSelectedRegionId(new int?(user.RegionId));
                this.dropRegions.DataBind();
                this.txtAddress.Text = user.Address;
                this.txtTelPhone.Text = user.TelPhone;
                this.txtCellPhone.Text = user.CellPhone;
            }
        }

        public void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (this.ValidateCreateOrder())
            {
                OrderInfo orderInfo = this.GetOrderInfo(this.shoppingCart);
                if (this.shoppingCart.GetQuantity() > 1)
                {
                    this.isSignBuy = false;
                }
                if (orderInfo == null)
                {
                    this.ShowMessage("购物车中已经没有任何商品", false);
                }
                else if (orderInfo.GetTotal() < 0M)
                {
                    this.ShowMessage("订单金额不能为负", false);
                }
                else
                {
                    if (!HiContext.Current.User.IsAnonymous)
                    {
                        int totalNeedPoint = this.shoppingCart.GetTotalNeedPoint();
                        int points = ((Member) HiContext.Current.User).Points;
                        if ((points >= 0) && (totalNeedPoint > points))
                        {
                            this.ShowMessage("您当前的积分不够兑换所需积分！", false);
                            return;
                        }
                    }
                    if (this.isCountDown)
                    {
                        CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(this.shoppingCart.LineItems[this.productSku].ProductId);
                        if (countDownInfo.EndDate < DateTime.Now)
                        {
                            this.ShowMessage("此订单为抢购订单，但抢购时间已到！", false);
                            return;
                        }
                        if (this.shoppingCart.LineItems[this.productSku].Quantity > countDownInfo.MaxCount)
                        {
                            this.ShowMessage("你购买的数量超过限购数量:" + countDownInfo.MaxCount.ToString(), false);
                            return;
                        }
                        int num3 = ShoppingProcessor.CountDownOrderCount(this.shoppingCart.LineItems[this.productSku].ProductId);
                        if ((num3 + this.shoppingCart.LineItems[this.productSku].Quantity) > countDownInfo.MaxCount)
                        {
                            this.ShowMessage(string.Format("你已经抢购过该商品{0}件，每个用户只允许抢购{1}件,如果你有未付款的抢购单，请及时支付！", num3, countDownInfo.MaxCount), false);
                            return;
                        }
                    }
                    try
                    {
                        if (ShoppingProcessor.CreatOrder(orderInfo))
                        {
                            Messenger.OrderCreated(orderInfo, HiContext.Current.User);
                            orderInfo.OnCreated();
                            if (this.shoppingCart.GetTotalNeedPoint() > 0)
                            {
                                ShoppingProcessor.CutNeedPoint(this.shoppingCart.GetTotalNeedPoint(), orderInfo.OrderId);
                            }
                            if ((!this.isCountDown && !this.isGroupBuy) && (!this.isSignBuy && !this.isBundling))
                            {
                                ShoppingCartProcessor.ClearShoppingCart();
                            }
                            if (!HiContext.Current.User.IsAnonymous && (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) == 0))
                            {
                                ShippingAddressInfo shippingAddress = new ShippingAddressInfo();
                                shippingAddress.UserId = HiContext.Current.User.UserId;
                                shippingAddress.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);
                                shippingAddress.RegionId = this.dropRegions.GetSelectedRegionId().Value;
                                shippingAddress.Address = Globals.HtmlEncode(this.txtAddress.Text);
                                shippingAddress.Zipcode = this.txtZipcode.Text;
                                shippingAddress.CellPhone = this.txtCellPhone.Text;
                                shippingAddress.TelPhone = this.txtTelPhone.Text;
                                PersonalHelper.CreateShippingAddress(shippingAddress);
                            }
                            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("FinishOrder", new object[] { orderInfo.OrderId }));
                        }
                    }
                    catch (Exception exception)
                    {
                        this.ShowMessage(exception.ToString(), false);
                    }
                }
            }
        }

        private void FillOrderCoupon(OrderInfo orderInfo)
        {
            if (!string.IsNullOrEmpty(this.htmlCouponCode.Value))
            {
                CouponInfo info = ShoppingProcessor.UseCoupon(Convert.ToDecimal(this.lblOrderTotal.Money), this.htmlCouponCode.Value);
                orderInfo.CouponName = info.Name;
                if (info.Amount.HasValue)
                {
                    orderInfo.CouponAmount = info.Amount.Value;
                }
                orderInfo.CouponCode = this.htmlCouponCode.Value;
                orderInfo.CouponValue = info.DiscountValue;
            }
        }

        private void FillOrderPaymentMode(OrderInfo orderInfo)
        {
            orderInfo.PaymentTypeId = int.Parse(this.inputPaymentModeId.Value);
            PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.PaymentTypeId);
            if (paymentMode != null)
            {
                orderInfo.PaymentType = Globals.HtmlEncode(paymentMode.Name);
                orderInfo.PayCharge = ShoppingProcessor.CalcPayCharge(orderInfo.GetTotal(), paymentMode);
                orderInfo.Gateway = paymentMode.Gateway;
            }
        }

        private void FillOrderShippingMode(OrderInfo orderInfo, ShoppingCartInfo shoppingCartInfo)
        {
            orderInfo.ShippingRegion = this.dropRegions.SelectedRegions;
            orderInfo.Address = Globals.HtmlEncode(this.txtAddress.Text);
            orderInfo.ZipCode = this.txtZipcode.Text;
            orderInfo.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text);
            orderInfo.TelPhone = this.txtTelPhone.Text;
            orderInfo.CellPhone = this.txtCellPhone.Text;
            if (!string.IsNullOrEmpty(this.inputShippingModeId.Value))
            {
                orderInfo.ShippingModeId = int.Parse(this.inputShippingModeId.Value, NumberStyles.None);
            }
            if (this.dropRegions.GetSelectedRegionId().HasValue)
            {
                orderInfo.RegionId = this.dropRegions.GetSelectedRegionId().Value;
            }
            orderInfo.ShipToDate = this.drpShipToDate.Value;
            ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(orderInfo.ShippingModeId, true);
            if (shippingMode != null)
            {
                orderInfo.ModeName = shippingMode.Name;
                orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode);
                if (!orderInfo.IsFreightFree)
                {
                    orderInfo.AdjustedFreight = orderInfo.Freight;
                }
            }
        }

        private string GenerateOrderId()
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

        private OrderInfo GetOrderInfo(ShoppingCartInfo shoppingCartInfo)
        {
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, this.isGroupBuy, this.isCountDown, this.isSignBuy);
            if (orderInfo == null)
            {
                return null;
            }
            if (this.chkTax.Checked)
            {
                orderInfo.Tax = (orderInfo.GetTotal() * decimal.Parse(this.litTaxRate.Text)) / 100M;
                if (this.isBundling)
                {
                    BundlingInfo bundlingInfo = ProductBrowser.GetBundlingInfo(this.bundlingid);
                    orderInfo.Tax = (bundlingInfo.Price * decimal.Parse(this.litTaxRate.Text)) / 100M;
                }
            }
            orderInfo.InvoiceTitle = this.txtInvoiceTitle.Text;
            if (this.isGroupBuy)
            {
                GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(shoppingCartInfo.LineItems[this.productSku].ProductId);
                orderInfo.GroupBuyId = productGroupBuyInfo.GroupBuyId;
                orderInfo.NeedPrice = productGroupBuyInfo.NeedPrice;
            }
            if (this.isCountDown)
            {
                CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(this.shoppingCart.LineItems[this.productSku].ProductId);
                orderInfo.CountDownBuyId = countDownInfo.CountDownId;
            }
            if (this.isBundling)
            {
                BundlingInfo info5 = ProductBrowser.GetBundlingInfo(this.bundlingid);
                orderInfo.BundlingID = info5.BundlingID;
                orderInfo.BundlingPrice = info5.Price;
                orderInfo.Points = this.shoppingCart.GetPoint(info5.Price);
            }
            orderInfo.OrderId = this.GenerateOrderId();
            orderInfo.OrderDate = DateTime.Now;
            IUser user = HiContext.Current.User;
            orderInfo.UserId = user.UserId;
            orderInfo.Username = user.Username;
            if (!user.IsAnonymous)
            {
                Member member = user as Member;
                orderInfo.EmailAddress = member.Email;
                orderInfo.RealName = member.RealName;
                orderInfo.QQ = member.QQ;
                orderInfo.Wangwang = member.Wangwang;
                orderInfo.MSN = member.MSN;
            }
            orderInfo.Remark = Globals.HtmlEncode(this.txtMessage.Text);
            orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
            orderInfo.RefundStatus = RefundStatus.None;
            this.FillOrderCoupon(orderInfo);
            this.FillOrderShippingMode(orderInfo, shoppingCartInfo);
            this.FillOrderPaymentMode(orderInfo);
            return orderInfo;
        }

        protected override void OnInit(EventArgs e)
        {
            if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "groupBuy")))
            {
                this.productSku = this.Page.Request.QueryString["productSku"];
                this.isGroupBuy = true;
                this.shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(this.productSku, this.buyAmount);
                this.buytype = "GroupBuy";
            }
            else if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "countDown")))
            {
                this.productSku = this.Page.Request.QueryString["productSku"];
                this.isCountDown = true;
                this.shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(this.productSku, this.buyAmount);
                this.buytype = "CountDown";
            }
            else if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "signBuy")))
            {
                this.productSku = this.Page.Request.QueryString["productSku"];
                this.isSignBuy = true;
                this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.productSku, this.buyAmount);
            }
            else if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out this.buyAmount) && !string.IsNullOrEmpty(this.Page.Request.QueryString["Bundlingid"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "Bundling")))
            {
                this.productSku = this.Page.Request.QueryString["Bundlingid"];
                if (int.TryParse(this.productSku, out this.bundlingid))
                {
                    this.shoppingCart = ShoppingCartProcessor.GetShoppingCart(this.bundlingid, this.buyAmount);
                    this.isBundling = true;
                    this.buytype = "Bundling";
                }
            }
            else
            {
                this.shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                if ((this.shoppingCart != null) && (this.shoppingCart.GetQuantity() == 0))
                {
                    this.buytype = "0";
                }
            }
            if (this.shoppingCart == null)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
            }
            else
            {
                if (this.SkinName == null)
                {
                    this.SkinName = "Skin-SubmmitOrder.html";
                }
                base.OnInit(e);
            }
        }

        private void ReBindPayment()
        {
            IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
            IList<PaymentModeInfo> list2 = new List<PaymentModeInfo>();
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                foreach (PaymentModeInfo info in paymentModes)
                {
                    if (((string.Compare(info.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) == 0) || (string.Compare(info.Gateway, "hishop.plugins.payment.alipaydirect.directrequest", true) == 0)) || (((string.Compare(info.Gateway, "hishop.plugins.payment.alipayassure.assurerequest", true) == 0) || (string.Compare(info.Gateway, "hishop.plugins.payment.alipay.standardrequest", true) == 0)) || ((string.Compare(info.Gateway, "hishop.plugins.payment.advancerequest", true) == 0) && !HiContext.Current.User.IsAnonymous)))
                    {
                        list2.Add(info);
                    }
                }
            }
            else
            {
                foreach (PaymentModeInfo info2 in paymentModes)
                {
                    if (string.Compare(info2.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) != 0)
                    {
                        list2.Add(info2);
                    }
                    if ((string.Compare(info2.Gateway, "hishop.plugins.payment.advancerequest", true) == 0) && HiContext.Current.User.IsAnonymous)
                    {
                        list2.Remove(info2);
                    }
                }
            }
            this.paymentModeList.DataSource = list2;
            this.paymentModeList.DataBind();
        }

        private bool ValidateCreateOrder()
        {
            if (!this.dropRegions.GetSelectedRegionId().HasValue || (this.dropRegions.GetSelectedRegionId().Value == 0))
            {
                this.ShowMessage("请选择收货地址", false);
                return false;
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
            {
                this.ShowMessage("请输入正确的收货人姓名", false);
                return false;
            }
            if ((this.txtShipTo.Text.Length < 2) || (this.txtShipTo.Text.Length > 20))
            {
                this.ShowMessage("收货人姓名长度应在2-20个字符之间", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtAddress.Text))
            {
                this.ShowMessage("请输入收货人详细地址", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.inputShippingModeId.Value))
            {
                this.ShowMessage("请选择配送方式", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.inputPaymentModeId.Value))
            {
                this.ShowMessage("请选择支付方式", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
            {
                this.ShowMessage("电话号码和手机号码必填其一", false);
                return false;
            }
            return true;
        }
    }
}

