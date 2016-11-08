namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Shippers)]
    public class AddShipper : AdminPage
    {
        protected Button btnAddShipper;
        protected YesNoRadioButtonList chkIsDefault;
        protected RegionSelector ddlReggion;
        protected TextBox txtAddress;
        protected TextBox txtCellPhone;
        protected TextBox txtRemark;
        protected TextBox txtShipperName;
        protected TextBox txtShipperTag;
        protected TextBox txtTelPhone;
        protected TextBox txtZipcode;

        private void btnAddShipper_Click(object sender, EventArgs e)
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
                shipper.IsDefault = this.chkIsDefault.SelectedValue;
                shipper.Remark = this.txtRemark.Text.Trim();
                if (this.ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        this.ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SalesHelper.AddShipper(shipper))
                    {
                        this.ShowMsg("成功添加了一个发货信息", true);
                    }
                    else
                    {
                        this.ShowMsg("添加发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnAddShipper.Click += new EventHandler(this.btnAddShipper_Click);
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

