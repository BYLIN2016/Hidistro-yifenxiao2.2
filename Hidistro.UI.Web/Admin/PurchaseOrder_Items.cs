namespace Hidistro.UI.Web.Admin
{
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PurchaseOrder_Items : UserControl
    {
        protected DataList dlstOrderItems;
        protected HtmlGenericControl giftsList;
        protected DataList grdOrderGift;
        protected FormatedMoneyLabel lblGoodsAmount;
        protected Literal lblWeight;
        private PurchaseOrderInfo purchaseOrder;
        protected PurchaseOrderItemUpdateHyperLink purchaseOrderItemUpdateHyperLink;

        protected override void OnLoad(EventArgs e)
        {
            this.dlstOrderItems.DataSource = this.purchaseOrder.PurchaseOrderItems;
            this.dlstOrderItems.DataBind();
            if (this.purchaseOrder.PurchaseOrderGifts.Count <= 0)
            {
                this.giftsList.Visible = false;
            }
            else
            {
                this.grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;
                this.grdOrderGift.DataBind();
            }
            this.lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();
            this.lblWeight.Text = this.purchaseOrder.Weight.ToString(CultureInfo.InvariantCulture);
            this.purchaseOrderItemUpdateHyperLink.PurchaseOrderId = this.purchaseOrder.PurchaseOrderId;
            this.purchaseOrderItemUpdateHyperLink.PurchaseStatusCode = this.purchaseOrder.PurchaseStatus;
            this.purchaseOrderItemUpdateHyperLink.DistorUserId = this.purchaseOrder.DistributorId;
            this.purchaseOrderItemUpdateHyperLink.DataBind();
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

