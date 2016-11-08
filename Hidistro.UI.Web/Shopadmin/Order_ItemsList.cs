namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Order_ItemsList : UserControl
    {
        protected DataList dlstOrderItems;
        protected DataList grdOrderGift;
        protected HyperLink hlkReducedPromotion;
        protected Literal lblAmoutPrice;
        protected Literal lblBundlingPrice;
        protected Label lblOrderGifts;
        protected FormatedMoneyLabel lblTotalPrice;
        protected Literal litWeight;
        private OrderInfo order;

        private void dlstOrderItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HyperLink link1 = (HyperLink) e.Item.FindControl("hpkBuyToSend");
                HyperLink link2 = (HyperLink) e.Item.FindControl("hpkBuyDiscount");
                Literal literal1 = (Literal) e.Item.FindControl("litPurchaseGiftId");
                Literal literal2 = (Literal) e.Item.FindControl("litWholesaleDiscountId");
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.dlstOrderItems.ItemDataBound += new DataListItemEventHandler(this.dlstOrderItems_ItemDataBound);
            this.dlstOrderItems.DataSource = this.order.LineItems.Values;
            this.dlstOrderItems.DataBind();
            if (this.order.Gifts.Count == 0)
            {
                this.grdOrderGift.Visible = false;
                this.lblOrderGifts.Visible = false;
            }
            else
            {
                this.grdOrderGift.DataSource = this.order.Gifts;
                this.grdOrderGift.DataBind();
            }
            this.litWeight.Text = this.order.Weight.ToString(CultureInfo.InvariantCulture);
            if (this.order.IsReduced)
            {
                this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
                this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
                this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { this.order.ReducedPromotionId });
            }
            if (this.order.BundlingID > 0)
            {
                this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
            }
            this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
        }

        public OrderInfo Order
        {
            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }
        }
    }
}

