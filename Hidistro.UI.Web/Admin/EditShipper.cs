namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Shippers)]
    public class EditShipper : AdminPage
    {
        protected Button btnEditShipper;
        protected RegionSelector ddlReggion;
        private int shipperId;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtRemark;
        protected TextBox txtShipperName;
        protected TextBox txtShipperTag;
        protected TextBox txtTelPhone;
        protected TextBox txtZipcode;

        private void btnEditShipper_Click(object sender, EventArgs e)
        {
            ShippersInfo shipper = new ShippersInfo();
            shipper.ShipperId = this.shipperId;
            shipper.ShipperTag = this.txtShipperTag.Text.Trim();
            shipper.ShipperName = this.txtShipperName.Text.Trim();
            if (!this.ddlReggion.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择地区", false);
            }
            else
            {
                shipper.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
                shipper.Address = this.txtAddress.Text.Trim();
                shipper.CellPhone = this.txtCellPhone.Text.Trim();
                shipper.TelPhone = this.txtTelPhone.Text.Trim();
                shipper.Zipcode = this.txtZipcode.Text.Trim();
                shipper.Remark = this.txtRemark.Text.Trim();
                if (this.ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        this.ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SalesHelper.UpdateShipper(shipper))
                    {
                        this.ShowMsg("成功修改了一个发货信息", true);
                    }
                    else
                    {
                        this.ShowMsg("修改发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["ShipperId"], out this.shipperId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnEditShipper.Click += new EventHandler(this.btnEditShipper_Click);
                if (!this.Page.IsPostBack)
                {
                    ShippersInfo shipper = SalesHelper.GetShipper(this.shipperId);
                    if (shipper == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(shipper, false);
                        this.txtShipperTag.Text = shipper.ShipperTag;
                        this.txtShipperName.Text = shipper.ShipperName;
                        this.ddlReggion.SetSelectedRegionId(new int?(shipper.RegionId));
                        this.txtAddress.Text = shipper.Address;
                        this.txtCellPhone.Text = shipper.CellPhone;
                        this.txtTelPhone.Text = shipper.TelPhone;
                        this.txtZipcode.Text = shipper.Zipcode;
                        this.txtRemark.Text = shipper.Remark;
                    }
                }
            }
        }

        private bool ValidationShipper(ShippersInfo shipper)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippersInfo>(shipper, new string[] { "Valshipper" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

