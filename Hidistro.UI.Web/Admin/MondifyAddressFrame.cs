namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    public class MondifyAddressFrame : AdminPage
    {
        private string action = "";
        protected Button btnMondifyAddress;
        protected RegionSelector dropRegions;
        protected PurchaseOrderInfo purchaseOrder;
        private string purchaseOrderId;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtShipTo;
        protected TextBox txtTelPhone;
        protected TextBox txtZipcode;

        private void BindUpdateSippingAddress()
        {
            this.txtShipTo.Text = Globals.HtmlDecode(this.purchaseOrder.ShipTo);
            this.dropRegions.SetSelectedRegionId(new int?(this.purchaseOrder.RegionId));
            this.txtAddress.Text = Globals.HtmlDecode(this.purchaseOrder.Address);
            this.txtZipcode.Text = this.purchaseOrder.ZipCode;
            this.txtTelPhone.Text = this.purchaseOrder.TelPhone;
            this.txtCellPhone.Text = this.purchaseOrder.CellPhone;
        }

        protected void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            this.purchaseOrder.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text.Trim());
            if (!this.dropRegions.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("收货人地址必选", false);
            }
            else
            {
                this.purchaseOrder.RegionId = this.dropRegions.GetSelectedRegionId().Value;
                this.purchaseOrder.Address = Globals.HtmlEncode(this.txtAddress.Text.Trim());
                this.purchaseOrder.TelPhone = this.txtTelPhone.Text.Trim();
                this.purchaseOrder.CellPhone = this.txtCellPhone.Text.Trim();
                this.purchaseOrder.ZipCode = this.txtZipcode.Text.Trim();
                this.purchaseOrder.ShippingRegion = this.dropRegions.SelectedRegions;
                if (SalesHelper.SavePurchaseOrderShippingAddress(this.purchaseOrder))
                {
                    this.ShowMsg("修改成功", true);
                }
                else
                {
                    this.ShowMsg("修改失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"]))
            {
                base.GotoResourceNotFound();
            }
            else if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
                this.purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                this.action = this.Page.Request.QueryString["action"];
                if ((this.action == "update") && !base.IsPostBack)
                {
                    this.BindUpdateSippingAddress();
                }
                this.btnMondifyAddress.Click += new EventHandler(this.btnMondifyAddress_Click);
            }
        }
    }
}

