namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class PurchaseOrder_ShippingAddress : UserControl
    {
        protected Button btnupdatepost;
        protected string edit = "";
        protected HtmlInputHidden hdtagId;
        protected FormatedTimeLabel lblPurchaseDate;
        protected Literal lblShipAddress;
        protected Literal litCompanyName;
        protected Literal litModeName;
        protected Label litRemark;
        protected Literal litShipToDate;
        protected Label lkBtnEditShippingAddress;
        protected Panel plExpress;
        protected HtmlAnchor power;
        private PurchaseOrderInfo purchaseOrder;
        protected HtmlTableRow tr_company;
        protected TextBox txtpost;

        private void btnupdatepost_Click(object sender, EventArgs e)
        {
            this.purchaseOrder.ShipOrderNumber = this.txtpost.Text;
            OrderHelper.EditPurchaseOrderShipNumber(this.purchaseOrder.PurchaseOrderId, this.purchaseOrder.OrderId, this.purchaseOrder.ShipOrderNumber);
            this.ShowMsg("修改发货单号成功", true);
            this.LoadControl();
        }

        public void LoadControl()
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(this.purchaseOrder.ShippingRegion))
            {
                shippingRegion = this.PurchaseOrder.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.Address))
            {
                shippingRegion = shippingRegion + this.PurchaseOrder.Address;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.ZipCode))
            {
                shippingRegion = shippingRegion + "，" + this.purchaseOrder.ZipCode;
            }
            if (!string.IsNullOrEmpty(this.PurchaseOrder.ShipTo))
            {
                shippingRegion = shippingRegion + "，" + this.purchaseOrder.ShipTo;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.TelPhone))
            {
                shippingRegion = shippingRegion + "，" + this.purchaseOrder.TelPhone;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.CellPhone))
            {
                shippingRegion = shippingRegion + "，" + this.purchaseOrder.CellPhone;
            }
            this.lblShipAddress.Text = shippingRegion;
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
            {
                this.lkBtnEditShippingAddress.Visible = true;
            }
            this.litShipToDate.Text = this.purchaseOrder.ShipToDate;
            if ((this.purchaseOrder.PurchaseStatus == OrderStatus.Finished) || (this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent))
            {
                this.txtpost.Text = this.purchaseOrder.ShipOrderNumber;
                this.litModeName.Text = this.purchaseOrder.RealModeName + " 发货单号：" + this.purchaseOrder.ShipOrderNumber;
            }
            else
            {
                this.litModeName.Text = this.purchaseOrder.ModeName;
            }
            if (!string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyName))
            {
                this.litCompanyName.Text = this.purchaseOrder.ExpressCompanyName;
                this.tr_company.Visible = true;
            }
            this.litRemark.Text = this.purchaseOrder.Remark;
            this.lblPurchaseDate.Time = this.purchaseOrder.PurchaseDate;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadControl();
            }
            if (this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent)
            {
                this.edit = "&nbsp;<input type=\"button\" class=\"submit_btnbianji\" id=\"btnedit\" value=\"修改发货单号\" onclick=\"ShowPurchaseOrder();\"/>";
            }
            if (((this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent) || (this.purchaseOrder.PurchaseStatus == OrderStatus.Finished)) && !string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyAbb))
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
            this.btnupdatepost.Click += new EventHandler(this.btnupdatepost_Click);
        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
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

