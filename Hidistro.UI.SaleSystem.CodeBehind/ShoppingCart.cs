namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ShoppingCart : HtmlTemplatedWebControl
    {
        private Button btnCheckout;
        private ImageLinkButton btnClearCart;
        private Button btnSKU;
        private HiddenField hfdIsLogin;
        private HyperLink hlkReducedPromotion;
        private Literal lblAmoutPrice;
        private FormatedMoneyLabel lblTotalPrice;
        private Literal litNoProduct;
        private Panel pnlPromoGift;
        private Panel pnlShopCart;
        private Common_ShoppingCart_GiftList shoppingCartGiftList;
        private Common_ShoppingCart_ProductList shoppingCartProductList;
        private Common_ShoppingCart_PromoGiftList shoppingCartPromoGiftList;
        private TextBox txtSKU;

        protected override void AttachChildControls()
        {
            this.txtSKU = (TextBox) this.FindControl("txtSKU");
            this.btnSKU = (Button) this.FindControl("btnSKU");
            this.btnClearCart = (ImageLinkButton) this.FindControl("btnClearCart");
            this.shoppingCartProductList = (Common_ShoppingCart_ProductList) this.FindControl("Common_ShoppingCart_ProductList");
            this.shoppingCartGiftList = (Common_ShoppingCart_GiftList) this.FindControl("Common_ShoppingCart_GiftList");
            this.shoppingCartPromoGiftList = (Common_ShoppingCart_PromoGiftList) this.FindControl("Common_ShoppingCart_PromoGiftList");
            this.lblTotalPrice = (FormatedMoneyLabel) this.FindControl("lblTotalPrice");
            this.lblAmoutPrice = (Literal) this.FindControl("lblAmoutPrice");
            this.hlkReducedPromotion = (HyperLink) this.FindControl("hlkReducedPromotion");
            this.btnCheckout = (Button) this.FindControl("btnCheckout");
            this.pnlShopCart = (Panel) this.FindControl("pnlShopCart");
            this.pnlPromoGift = (Panel) this.FindControl("pnlPromoGift");
            this.litNoProduct = (Literal) this.FindControl("litNoProduct");
            this.hfdIsLogin = (HiddenField) this.FindControl("hfdIsLogin");
            this.btnSKU.Click += new EventHandler(this.btnSKU_Click);
            this.btnClearCart.Click += new EventHandler(this.btnClearCart_Click);
            this.shoppingCartProductList.ItemCommand += new DataListCommandEventHandler(this.shoppingCartProductList_ItemCommand);
            this.shoppingCartGiftList.ItemCommand += new DataListCommandEventHandler(this.shoppingCartGiftList_ItemCommand);
            this.shoppingCartGiftList.FreeItemCommand += new DataListCommandEventHandler(this.shoppingCartGiftList_FreeItemCommand);
            this.shoppingCartPromoGiftList.ItemCommand += new RepeaterCommandEventHandler(this.shoppingCartPromoGiftList_ItemCommand);
            this.btnCheckout.Click += new EventHandler(this.btnCheckout_Click);
            if (!HiContext.Current.SiteSettings.IsOpenSiteSale && !HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.btnSKU.Visible = false;
                this.btnCheckout.Visible = false;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindShoppingCart();
            }
            if (!HiContext.Current.User.IsAnonymous)
            {
                this.hfdIsLogin.Value = "logined";
            }
        }

        private void BindShoppingCart()
        {
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart == null)
            {
                this.pnlShopCart.Visible = false;
                this.litNoProduct.Visible = true;
                ShoppingCartProcessor.ClearShoppingCart();
            }
            else
            {
                this.pnlShopCart.Visible = true;
                this.litNoProduct.Visible = false;
                if (shoppingCart.LineItems.Values.Count > 0)
                {
                    this.shoppingCartProductList.DataSource = shoppingCart.LineItems.Values;
                    this.shoppingCartProductList.DataBind();
                    this.shoppingCartProductList.ShowProductCart();
                }
                if (shoppingCart.LineGifts.Count > 0)
                {
                    IEnumerable<ShoppingCartGiftInfo> source = shoppingCart.LineGifts.Where<ShoppingCartGiftInfo>(delegate (ShoppingCartGiftInfo s) {
                        return s.PromoType == 0;
                    });
                    IEnumerable<ShoppingCartGiftInfo> enumerable2 = shoppingCart.LineGifts.Where<ShoppingCartGiftInfo>(delegate (ShoppingCartGiftInfo s) {
                        return s.PromoType == 5;
                    });
                    this.shoppingCartGiftList.DataSource = source;
                    this.shoppingCartGiftList.FreeDataSource = enumerable2;
                    this.shoppingCartGiftList.DataBind();
                    this.shoppingCartGiftList.ShowGiftCart(source.Count<ShoppingCartGiftInfo>() > 0, enumerable2.Count<ShoppingCartGiftInfo>() > 0);
                }
                if (shoppingCart.IsSendGift)
                {
                    int sumnum = 1;
                    int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
                    IList<GiftInfo> onlinePromotionGifts = ProductBrowser.GetOnlinePromotionGifts();
                    if ((onlinePromotionGifts != null) && (onlinePromotionGifts.Count > 0))
                    {
                        this.shoppingCartPromoGiftList.DataSource = onlinePromotionGifts;
                        this.shoppingCartPromoGiftList.DataBind();
                        this.shoppingCartPromoGiftList.ShowPromoGift(sumnum - giftItemQuantity, sumnum);
                    }
                }
                if (shoppingCart.IsReduced)
                {
                    this.lblAmoutPrice.Text = string.Format("商品金额：{0}", shoppingCart.GetAmount().ToString("F2"));
                    this.hlkReducedPromotion.Text = shoppingCart.ReducedPromotionName + string.Format(" 优惠：{0}", shoppingCart.ReducedPromotionAmount.ToString("F2"));
                    this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { shoppingCart.ReducedPromotionId });
                }
                this.lblTotalPrice.Money = shoppingCart.GetTotal();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            HiContext.Current.Context.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            string str = this.Page.Request.Form["ck_productId"];
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMessage("请选择要清除的商品", false);
            }
            else
            {
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    ShoppingCartProcessor.RemoveLineItem(str2);
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

        protected void btnSKU_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
            {
                this.ShowMessage("请输入货号", false);
            }
            else
            {
                IList<string> skuIdsBysku = ShoppingProcessor.GetSkuIdsBysku(this.txtSKU.Text.Trim());
                if ((skuIdsBysku == null) || (skuIdsBysku.Count == 0))
                {
                    this.ShowMessage("货号无效，请确认后重试", false);
                }
                else
                {
                    foreach (string str in skuIdsBysku)
                    {
                        ShoppingCartProcessor.AddLineItem(str, 1);
                    }
                    this.BindShoppingCart();
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ShoppingCart.html";
            }
            base.OnInit(e);
        }

        protected void shoppingCartGiftList_FreeItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                string str = e.CommandArgument.ToString();
                int result = 0;
                if (!string.IsNullOrEmpty(str) && int.TryParse(str, out result))
                {
                    ShoppingCartProcessor.RemoveGiftItem(result, PromoteType.SentGift);
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

        protected void shoppingCartGiftList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int num;
            Control control = e.Item.Controls[0];
            TextBox box = (TextBox) control.FindControl("txtBuyNum");
            Literal literal = (Literal) control.FindControl("litGiftId");
            if (!int.TryParse(box.Text, out num) || (box.Text.IndexOf(".") != -1))
            {
                this.ShowMessage("兑换数量必须为整数", false);
            }
            else if (num <= 0)
            {
                this.ShowMessage("兑换数量必须为大于0的整数", false);
            }
            else
            {
                if (e.CommandName == "updateBuyNum")
                {
                    ShoppingCartProcessor.UpdateGiftItemQuantity(Convert.ToInt32(literal.Text), num, PromoteType.NotSet);
                }
                if (e.CommandName == "delete")
                {
                    ShoppingCartProcessor.RemoveGiftItem(Convert.ToInt32(literal.Text), PromoteType.NotSet);
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

        protected void shoppingCartProductList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int num;
            Control control = e.Item.Controls[0];
            TextBox box = (TextBox) control.FindControl("txtBuyNum");
            Literal literal = (Literal) control.FindControl("litSkuId");
            if (!int.TryParse(box.Text, out num) || (box.Text.IndexOf(".") != -1))
            {
                this.ShowMessage("购买数量必须为整数", false);
            }
            else if (num <= 0)
            {
                this.ShowMessage("购买数量必须为大于0的整数", false);
            }
            else
            {
                if (e.CommandName == "updateBuyNum")
                {
                    if (ShoppingCartProcessor.GetSkuStock(literal.Text.Trim()) < num)
                    {
                        this.ShowMessage("该商品库存不够", false);
                        return;
                    }
                    ShoppingCartProcessor.UpdateLineItemQuantity(literal.Text, num);
                }
                if (e.CommandName == "delete")
                {
                    ShoppingCartProcessor.RemoveLineItem(literal.Text);
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

        protected void shoppingCartPromoGiftList_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("change"))
            {
                int giftId = Convert.ToInt32(e.CommandArgument.ToString());
                if (giftId > 0)
                {
                    int giftItemQuantity = ShoppingCartProcessor.GetGiftItemQuantity(PromoteType.SentGift);
                    if (this.shoppingCartPromoGiftList.SumNum > giftItemQuantity)
                    {
                        ShoppingCartProcessor.AddGiftItem(giftId, 1, PromoteType.SentGift);
                        this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
                    }
                    else
                    {
                        this.ShowMessage("礼品兑换失败，您不能超过最多兑换数", false);
                    }
                }
            }
        }
    }
}

