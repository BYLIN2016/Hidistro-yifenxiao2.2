namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PrintPackingOrder : AdminPage
    {
        protected HtmlForm form1;
        protected Grid grdOrderGifts;
        protected Grid grdOrderItems;
        protected HeadContainer HeadContainer1;
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
        private string orderId = string.Empty;
        protected PageTitle PageTitle1;
        protected Script Script2;

        private void BindOrderInfo(OrderInfo order)
        {
            this.litAddress.Text = order.ShippingRegion + order.Address;
            this.litCellPhone.Text = order.CellPhone;
            this.litTelPhone.Text = order.TelPhone;
            this.litZipCode.Text = order.ZipCode;
            this.litOrderId.Text = order.OrderId;
            this.litOrderDate.Text = order.OrderDate.ToString();
            this.litPayType.Text = order.PaymentType;
            this.litRemark.Text = order.Remark;
            this.litShipperMode.Text = order.RealModeName;
            this.litShippNo.Text = order.ShipOrderNumber;
            this.litSkipTo.Text = order.ShipTo;
            switch (order.OrderStatus)
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

        private void BindOrderItems(OrderInfo order)
        {
            this.grdOrderItems.DataSource = order.LineItems.Values;
            this.grdOrderItems.DataBind();
            if (order.Gifts.Count > 0)
            {
                this.grdOrderGifts.DataSource = order.Gifts;
                this.grdOrderGifts.DataBind();
            }
            else
            {
                this.grdOrderGifts.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.orderId = this.Page.Request.Params["OrderId"];
            if (!this.Page.IsPostBack && !string.IsNullOrEmpty(this.orderId))
            {
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
                this.BindOrderInfo(orderInfo);
                this.BindOrderItems(orderInfo);
            }
        }
    }
}

