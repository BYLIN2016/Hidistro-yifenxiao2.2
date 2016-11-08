namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;

    public class UserShippingAddresses : MemberTemplatedWebControl
    {
        private IButton btnAddAddress;
        private IButton btnCancel;
        private IButton btnEditAddress;
        private RegionSelector dropRegionsSelect;
        private Common_Address_AddressList dtlstRegionsSelect;
        private Literal lblAddressCount;
        private static int shippingId;
        private TextBox txtAddress;
        private TextBox txtCellPhone;
        private TextBox txtShipTo;
        private TextBox txtTelPhone;
        private TextBox txtZipcode;

        protected override void AttachChildControls()
        {
            this.txtShipTo = (TextBox) this.FindControl("txtShipTo");
            this.txtAddress = (TextBox) this.FindControl("txtAddress");
            this.txtZipcode = (TextBox) this.FindControl("txtZipcode");
            this.txtTelPhone = (TextBox) this.FindControl("txtTelPhone");
            this.txtCellPhone = (TextBox) this.FindControl("txtCellPhone");
            this.dropRegionsSelect = (RegionSelector) this.FindControl("dropRegions");
            this.btnAddAddress = ButtonManager.Create(this.FindControl("btnAddAddress"));
            this.btnCancel = ButtonManager.Create(this.FindControl("btnCancel"));
            this.btnEditAddress = ButtonManager.Create(this.FindControl("btnEditAddress"));
            this.dtlstRegionsSelect = (Common_Address_AddressList) this.FindControl("list_Common_Consignee_ConsigneeList");
            this.lblAddressCount = (Literal) this.FindControl("lblAddressCount");
            this.btnAddAddress.Click += new EventHandler(this.btnAddAddress_Click);
            this.btnEditAddress.Click += new EventHandler(this.btnEditAddress_Click);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.dtlstRegionsSelect._ItemCommand += new Common_Address_AddressList.CommandEventHandler(this.dtlstRegionsSelect_ItemCommand);
            PageTitle.AddSiteNameTitle("我的收货地址", HiContext.Current.Context);
            if (!this.Page.IsPostBack)
            {
                this.lblAddressCount.Text = HiContext.Current.Config.ShippingAddressQuantity.ToString();
                this.dropRegionsSelect.DataBind();
                this.Reset();
                this.btnEditAddress.Visible = false;
                this.BindList();
            }
        }

        private void BindList()
        {
            IList<ShippingAddressInfo> shippingAddress = PersonalHelper.GetShippingAddress(HiContext.Current.User.UserId);
            this.dtlstRegionsSelect.DataSource = shippingAddress;
            this.dtlstRegionsSelect.DataBind();
        }

        protected void btnAddAddress_Click(object sender, EventArgs e)
        {
            if (this.ValShippingAddress())
            {
                if (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) >= HiContext.Current.Config.ShippingAddressQuantity)
                {
                    this.ShowMessage(string.Format("最多只能添加{0}个收货地址", HiContext.Current.Config.ShippingAddressQuantity), false);
                    this.Reset();
                }
                else
                {
                    if (PersonalHelper.CreateShippingAddress(this.GetShippingAddressInfo()))
                    {
                        this.ShowMessage("成功的添加了一个收货地址", true);
                        this.Reset();
                    }
                    else
                    {
                        this.ShowMessage("地址已经在，请重新输入一次再试", false);
                    }
                    this.BindList();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        protected void btnEditAddress_Click(object sender, EventArgs e)
        {
            if (this.ValShippingAddress())
            {
                ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo();
                shippingAddressInfo.ShippingId = Convert.ToInt32(this.ViewState["shippingId"]);
                if (PersonalHelper.UpdateShippingAddress(shippingAddressInfo))
                {
                    this.ShowMessage("成功的修改了一个收货地址", true);
                    this.Reset();
                }
                else
                {
                    this.ShowMessage("地址已经在，请重新输入一次再试", false);
                }
                this.btnEditAddress.Visible = false;
                this.btnAddAddress.Visible = true;
                this.BindList();
            }
        }

        protected void dtlstRegionsSelect_ItemCommand(object source, DataListCommandEventArgs e)
        {
            shippingId = (int) this.dtlstRegionsSelect.DataKeys[e.Item.ItemIndex];
            this.ViewState["shippingId"] = shippingId;
            if (e.CommandName == "Edit")
            {
                ShippingAddressInfo userShippingAddress = PersonalHelper.GetUserShippingAddress(shippingId);
                if (userShippingAddress != null)
                {
                    this.txtShipTo.Text = userShippingAddress.ShipTo;
                    this.dropRegionsSelect.SetSelectedRegionId(new int?(userShippingAddress.RegionId));
                    this.txtAddress.Text = userShippingAddress.Address;
                    this.txtZipcode.Text = userShippingAddress.Zipcode;
                    this.txtTelPhone.Text = userShippingAddress.TelPhone;
                    this.txtCellPhone.Text = userShippingAddress.CellPhone;
                    this.btnCancel.Visible = true;
                    this.btnAddAddress.Visible = false;
                    this.btnEditAddress.Visible = true;
                }
            }
            if (e.CommandName == "Delete")
            {
                if (PersonalHelper.DeleteShippingAddress(shippingId))
                {
                    this.ShowMessage("成功的删除了你要删除的记录", true);
                    this.BindList();
                }
                else
                {
                    this.ShowMessage("删除失败", false);
                }
                shippingId = 0;
            }
        }

        private ShippingAddressInfo GetShippingAddressInfo()
        {
            ShippingAddressInfo info = new ShippingAddressInfo();
            info.UserId = HiContext.Current.User.UserId;
            info.ShipTo = this.txtShipTo.Text;
            info.RegionId = this.dropRegionsSelect.GetSelectedRegionId().Value;
            info.Address = this.txtAddress.Text;
            info.Zipcode = this.txtZipcode.Text;
            info.CellPhone = this.txtCellPhone.Text;
            info.TelPhone = this.txtTelPhone.Text;
            return info;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserShippingAddresses.html";
            }
            base.OnInit(e);
        }

        private void Reset()
        {
            this.txtShipTo.Text = string.Empty;
            this.dropRegionsSelect.SetSelectedRegionId(null);
            this.txtAddress.Text = string.Empty;
            this.txtZipcode.Text = string.Empty;
            this.txtTelPhone.Text = string.Empty;
            this.txtCellPhone.Text = string.Empty;
            shippingId = 0;
            this.btnAddAddress.Visible = true;
        }

        private bool ValShippingAddress()
        {
            Regex regex = new Regex(@"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*");
            if (string.IsNullOrEmpty(this.txtShipTo.Text.Trim()) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
            {
                this.ShowMessage("收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()))
            {
                this.ShowMessage("详细地址不能为空", false);
                return false;
            }
            if ((this.txtAddress.Text.Trim().Length < 3) || (this.txtAddress.Text.Trim().Length > 60))
            {
                this.ShowMessage("详细地址长度在3-60个字符之间", false);
                return false;
            }
            if (!this.dropRegionsSelect.GetSelectedRegionId().HasValue || (this.dropRegionsSelect.GetSelectedRegionId().Value == 0))
            {
                this.ShowMessage("请选择收货地址", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()))
            {
                this.ShowMessage("电话号码和手机二者必填其一", false);
                this.Reset();
                return false;
            }
            if (!string.IsNullOrEmpty(this.txtTelPhone.Text.Trim()) && ((this.txtTelPhone.Text.Trim().Length < 3) || (this.txtTelPhone.Text.Trim().Length > 20)))
            {
                this.ShowMessage("电话号码长度限制在3-20个字符之间", false);
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCellPhone.Text.Trim()) || ((this.txtCellPhone.Text.Trim().Length >= 3) && (this.txtCellPhone.Text.Trim().Length <= 20)))
            {
                return true;
            }
            this.ShowMessage("手机号码长度限制在3-20个字符之间", false);
            return false;
        }
    }
}

