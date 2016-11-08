namespace Hidistro.UI.Web.ShopAdmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SetMyShipper : DistributorPage
    {
        protected Button btnEditShipper;
        protected RegionSelector ddlReggion;
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
                    else if (SubsiteSalesHelper.SetMyShipper(shipper))
                    {
                        this.ShowMsg("成功保存了发货信息", true);
                    }
                    else
                    {
                        this.ShowMsg("保存发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnEditShipper.Click += new EventHandler(this.btnEditShipper_Click);
            if (!this.Page.IsPostBack)
            {
                ShippersInfo myShipper = SubsiteSalesHelper.GetMyShipper();
                if (myShipper != null)
                {
                    Globals.EntityCoding(myShipper, false);
                    this.txtShipperTag.Text = myShipper.ShipperTag;
                    this.txtShipperName.Text = myShipper.ShipperName;
                    this.ddlReggion.SetSelectedRegionId(new int?(myShipper.RegionId));
                    this.txtAddress.Text = myShipper.Address;
                    this.txtCellPhone.Text = myShipper.CellPhone;
                    this.txtTelPhone.Text = myShipper.TelPhone;
                    this.txtZipcode.Text = myShipper.Zipcode;
                    this.txtRemark.Text = myShipper.Remark;
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

