namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class Common_ShoppingCart_Info : AscxTemplatedWebControl
    {
        private FormatedMoneyLabel cartMoney;
        private Literal cartNum;

        protected override void AttachChildControls()
        {
            this.cartMoney = (FormatedMoneyLabel) this.FindControl("cartMoney");
            this.cartNum = (Literal) this.FindControl("cartNum");
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if ((shoppingCart != null) && ((shoppingCart.LineGifts.Count > 0) || (shoppingCart.LineItems.Count > 0)))
            {
                this.cartMoney.Money = shoppingCart.GetTotal();
                this.cartNum.Text = shoppingCart.GetQuantity().ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_Info.ascx";
            }
            base.OnInit(e);
        }
    }
}

