namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Order_ShippingAddress : UserControl
    {
        protected Button btnupdatepost;
        protected string edit = "";
        protected HtmlInputHidden hdtagId;
        protected Literal lblShipAddress;
        protected Literal litCompanyName;
        protected Literal litModeName;
        protected Label litRemark;
        protected Literal litShipToDate;
        protected Label lkBtnEditShippingAddress;
        private OrderInfo order;
        protected Panel plExpress;
        protected HtmlAnchor power;
        protected HtmlTableRow tr_company;
        protected TextBox txtpost;

        private void btnupdatepost_Click(object sender, EventArgs e)
        {
            this.order.ShipOrderNumber = this.txtpost.Text;
            OrderHelper.SetOrderShipNumber(this.order.OrderId, this.order.ShipOrderNumber);
            this.ShowMsg("修改发货单号成功", true);
            this.LoadControl();
        }

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
            if ((this.order.OrderStatus == OrderStatus.WaitBuyerPay) || (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid))
            {
                this.lkBtnEditShippingAddress.Visible = true;
            }
            else
            {
                this.lkBtnEditShippingAddress.Visible = false;
            }
            this.litShipToDate.Text = this.order.ShipToDate;
            if ((this.order.OrderStatus == OrderStatus.Finished) || (this.order.OrderStatus == OrderStatus.SellerAlreadySent))
            {
                this.litModeName.Text = this.order.RealModeName + " 发货单号：" + this.order.ShipOrderNumber;
                this.txtpost.Text = this.order.ShipOrderNumber;
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
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadControl();
            }
            if (this.order.OrderStatus == OrderStatus.SellerAlreadySent)
            {
                this.edit = "&nbsp;<input type=\"button\" class=\"submit_btnbianji\" id=\"btnedit\" value=\"修改发货单号\" onclick=\"ShowPurchaseOrder();\"/>";
            }
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

