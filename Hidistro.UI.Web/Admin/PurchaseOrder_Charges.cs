namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PurchaseOrder_Charges : UserControl
    {
        protected Literal lblModeName;
        protected Literal litDiscount;
        protected Literal litFreight;
        protected Literal litInvoiceTitle;
        protected Literal litTax;
        protected Literal litTotalPrice;
        protected HtmlAnchor lkBtnEditshipingMode;
        private PurchaseOrderInfo purchaseOrder;

        public void LoadControl()
        {
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                this.lkBtnEditshipingMode.Visible = true;
            }
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.Finished) || (this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                this.lblModeName.Text = this.purchaseOrder.RealModeName;
            }
            else
            {
                this.lblModeName.Text = this.purchaseOrder.ModeName;
            }
            this.litFreight.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedFreight);
            this.litDiscount.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedDiscount);
            this.litTotalPrice.Text = Globals.FormatMoney(this.purchaseOrder.GetPurchaseTotal());
            if (this.purchaseOrder.Tax > 0M)
            {
                this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.purchaseOrder.Tax);
                this.litTax.Text = this.litTax.Text + "</span></td></tr>";
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.InvoiceTitle))
            {
                this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.purchaseOrder.InvoiceTitle;
                this.litInvoiceTitle.Text = this.litInvoiceTitle.Text + "</span></td></tr>";
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadControl();
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

