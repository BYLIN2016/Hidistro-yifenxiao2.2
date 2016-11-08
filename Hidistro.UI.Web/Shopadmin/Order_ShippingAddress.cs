namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Order_ShippingAddress : UserControl
    {
        protected Literal lblShipAddress;
        protected Literal litCompanyName;
        protected Literal litModeName;
        protected Label litRemark;
        protected Literal litShipToDate;
        protected LinkButton lkBtnEditShippingAddress;
        protected Literal ltrShipNum;
        private OrderInfo order;
        protected Panel plExpress;
        protected HtmlAnchor power;
        protected HtmlTableRow tr_company;

        public void LoadControl()
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(this.order.ShippingRegion))
            {
                shippingRegion = this.order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(this.order.Address))
            {
                shippingRegion = shippingRegion + this.order.Address;
            }
            if (!string.IsNullOrEmpty(this.order.ShipTo))
            {
                shippingRegion = shippingRegion + "   " + this.order.ShipTo;
            }
            if (!string.IsNullOrEmpty(this.order.ZipCode))
            {
                shippingRegion = shippingRegion + "   " + this.order.ZipCode;
            }
            if (!string.IsNullOrEmpty(this.order.TelPhone))
            {
                shippingRegion = shippingRegion + "   " + this.order.TelPhone;
            }
            if (!string.IsNullOrEmpty(this.order.CellPhone))
            {
                shippingRegion = shippingRegion + "   " + this.order.CellPhone;
            }
            this.lblShipAddress.Text = shippingRegion;
            if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                this.lkBtnEditShippingAddress.Visible = true;
            }
            this.litShipToDate.Text = this.order.ShipToDate;
            if ((this.order.OrderStatus == OrderStatus.Finished) || (this.order.OrderStatus == OrderStatus.SellerAlreadySent))
            {
                this.litModeName.Text = this.order.RealModeName;
                this.ltrShipNum.Text = "  物流单号：" + this.order.ShipOrderNumber;
            }
            else
            {
                this.litModeName.Text = this.order.ModeName;
            }
            if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
            {
                this.litCompanyName.Text = this.order.ExpressCompanyName;
                this.tr_company.Visible = true;
            }
            this.litRemark.Text = this.order.Remark;
            if (((this.order.OrderStatus == OrderStatus.SellerAlreadySent) || (this.order.OrderStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb))
            {
                if (this.plExpress != null)
                {
                    this.plExpress.Visible = true;
                }
                if ((Express.GetExpressType() == "kuaidi100") && (this.power != null))
                {
                    this.power.Visible = true;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.LoadControl();
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

