namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PurchaseOrder_ShippingAddress : UserControl
    {
        protected Button btnSaveRemark;
        protected FormatedTimeLabel lblPurchaseDate;
        protected Literal lblShipAddress;
        protected Literal litCompanyName;
        protected Literal litModeName;
        protected Literal litShipToDate;
        protected Literal ltrShipNum;
        protected Panel plExpress;
        protected HtmlAnchor power;
        private PurchaseOrderInfo purchaseOrder;
        protected HtmlTableRow tr_company;
        protected TextBox txtRemark;

        private void btnSaveRemark_Click(object sender, EventArgs e)
        {
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                SubsiteSalesHelper.SavePurchaseOrderRemark(this.purchaseOrder.PurchaseOrderId, Globals.HtmlEncode(this.txtRemark.Text));
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        public void LoadControl()
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(this.PurchaseOrder.ShippingRegion))
            {
                shippingRegion = this.PurchaseOrder.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.Address))
            {
                shippingRegion = shippingRegion + this.PurchaseOrder.Address;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.ZipCode))
            {
                shippingRegion = shippingRegion + "," + this.PurchaseOrder.ZipCode;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.ShipTo))
            {
                shippingRegion = shippingRegion + "," + this.PurchaseOrder.ShipTo;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.TelPhone))
            {
                shippingRegion = shippingRegion + "," + this.PurchaseOrder.TelPhone;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.CellPhone))
            {
                shippingRegion = shippingRegion + "," + this.PurchaseOrder.CellPhone;
            }
            this.lblShipAddress.Text = shippingRegion;
            this.litShipToDate.Text = this.PurchaseOrder.ShipToDate;
            if ((this.PurchaseOrder.PurchaseStatus == OrderStatus.Finished) || (this.PurchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                this.litModeName.Text = this.PurchaseOrder.RealModeName;
                this.ltrShipNum.Text = "  物流单号：" + this.PurchaseOrder.ShipOrderNumber;
            }
            else
            {
                this.litModeName.Text = this.PurchaseOrder.ModeName;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyName))
            {
                this.litCompanyName.Text = this.purchaseOrder.ExpressCompanyName;
                this.tr_company.Visible = true;
            }
            this.txtRemark.Text = Globals.HtmlDecode(this.PurchaseOrder.Remark);
            this.lblPurchaseDate.Time = this.PurchaseOrder.PurchaseDate;
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                this.btnSaveRemark.Enabled = true;
            }
            else
            {
                this.btnSaveRemark.Enabled = false;
            }
            if (((this.PurchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent) || (this.PurchaseOrder.PurchaseStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(this.PurchaseOrder.ExpressCompanyAbb))
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveRemark.Click += new EventHandler(this.btnSaveRemark_Click);
            if (!this.Page.IsPostBack)
            {
                this.LoadControl();
            }
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

