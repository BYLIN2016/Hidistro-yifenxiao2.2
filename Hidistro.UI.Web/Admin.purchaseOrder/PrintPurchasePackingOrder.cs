namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PrintPurchasePackingOrder : AdminPage
    {
        protected HtmlForm form1;
        protected Grid grdOrderGifts;
        protected Grid grdOrderItems;
        protected Literal litAddress;
        protected Literal litCellPhone;
        protected Literal litOrderDate;
        protected Literal litOrderId;
        protected Literal litOrderStatus;
        protected Literal litPayType;
        protected Literal litRemark;
        protected Literal litShipperMode;
        protected Literal litShippNo;
        protected Literal litSkipTo;
        protected Literal litTelPhone;
        protected Literal litZipCode;
        protected PageTitle PageTitle1;
        private string purchaseOrderId = string.Empty;
        protected Script Script2;

        private void BindOrderInfo(PurchaseOrderInfo order)
        {
            this.litAddress.Text = order.ShippingRegion + order.Address;
            this.litCellPhone.Text = order.CellPhone;
            this.litTelPhone.Text = order.TelPhone;
            this.litZipCode.Text = order.ZipCode;
            this.litOrderId.Text = order.OrderId;
            this.litOrderDate.Text = order.PurchaseDate.ToString();
            this.litRemark.Text = order.Remark;
            this.litShipperMode.Text = order.RealModeName;
            this.litShippNo.Text = order.ShipOrderNumber;
            this.litSkipTo.Text = order.ShipTo;
            switch (order.PurchaseStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    this.litOrderStatus.Text = "等待付款";
                    return;

                case OrderStatus.BuyerAlreadyPaid:
                    this.litOrderStatus.Text = "已付款等待发货";
                    return;

                case OrderStatus.SellerAlreadySent:
                    this.litOrderStatus.Text = "已发货";
                    return;

                case OrderStatus.Closed:
                    this.litOrderStatus.Text = "已关闭";
                    return;

                case OrderStatus.Finished:
                    this.litOrderStatus.Text = "已完成";
                    return;
            }
        }

        private void BindOrderItems(PurchaseOrderInfo order)
        {
            this.grdOrderItems.DataSource = order.PurchaseOrderItems;
            this.grdOrderItems.DataBind();
            if (order.PurchaseOrderGifts.Count > 0)
            {
                this.grdOrderGifts.DataSource = order.PurchaseOrderGifts;
                this.grdOrderGifts.DataBind();
            }
            else
            {
                this.grdOrderGifts.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.purchaseOrderId = this.Page.Request.Params["PurchaseOrderId"];
            if (!this.Page.IsPostBack && !string.IsNullOrEmpty(this.purchaseOrderId))
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                this.BindOrderInfo(purchaseOrder);
                this.BindOrderItems(purchaseOrder);
            }
        }
    }
}

