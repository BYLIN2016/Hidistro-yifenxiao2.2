namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PurchaseOrder_Items : UserControl
    {
        protected DataList dlstOrderItems;
        protected DataList grdOrderGift;
        protected FormatedMoneyLabel lblGoodsAmount;
        protected Literal lblWeight;
        private PurchaseOrderInfo purchaseOrder;

        protected override void OnLoad(EventArgs e)
        {
            this.dlstOrderItems.DataSource = this.purchaseOrder.PurchaseOrderItems;
            this.dlstOrderItems.DataBind();
            this.grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;
            this.grdOrderGift.DataBind();
            this.lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();
            this.lblWeight.Text = this.purchaseOrder.Weight.ToString(CultureInfo.InvariantCulture);
        }

        public PurchaseOrderInfo PurchaseOrder
        {
            get
            {
                return this.purchaseOrder;
            }
            set
            {
                this.purchaseOrder = value;
            }
        }
    }
}

