namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorProfile : DistributorPage
    {
        protected Button btnSave;
        protected RegionSelector rsddlRegion;
        protected TextBox txtAddress;
        protected HtmlGenericControl txtAddressTip;
        protected TextBox txtCellPhone;
        protected HtmlGenericControl txtCellPhoneTip;
        protected TextBox txtCompanyName;
        protected HtmlGenericControl txtCompanyNameTip;
        protected TextBox txtMSN;
        protected HtmlGenericControl txtMSNTip;
        protected TextBox txtprivateEmail;
        protected HtmlGenericControl txtprivateEmailTip;
        protected TextBox txtQQ;
        protected HtmlGenericControl txtQQTip;
        protected TextBox txtRealName;
        protected TextBox txtTel;
        protected HtmlGenericControl txtTelTip;
        protected TextBox txtWangwang;
        protected HtmlGenericControl txtWangwangTip;
        protected TextBox txtZipcode;
        protected HtmlGenericControl txtZipcodeTip;

        private void BindData(Distributor distributor)
        {
            this.txtRealName.Text = distributor.RealName;
            this.txtCompanyName.Text = distributor.CompanyName;
            this.txtprivateEmail.Text = distributor.Email;
            this.txtAddress.Text = distributor.Address;
            this.txtZipcode.Text = distributor.Zipcode;
            this.txtQQ.Text = distributor.QQ;
            this.txtWangwang.Text = distributor.Wangwang;
            this.txtMSN.Text = distributor.MSN;
            this.txtTel.Text = distributor.TelPhone;
            this.txtCellPhone.Text = distributor.CellPhone;
            this.rsddlRegion.SetSelectedRegionId(new int?(distributor.RegionId));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidationInput())
            {
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                distributor.RealName = this.txtRealName.Text.Trim().Replace('"', '“');
                distributor.CompanyName = this.txtCompanyName.Text.Trim();
                if (this.rsddlRegion.GetSelectedRegionId().HasValue)
                {
                    distributor.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                    distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
                }
                distributor.Email = this.txtprivateEmail.Text.Trim();
                distributor.Address = this.txtAddress.Text.Trim();
                distributor.Zipcode = this.txtZipcode.Text.Trim();
                distributor.QQ = this.txtQQ.Text.Trim();
                distributor.Wangwang = this.txtWangwang.Text.Trim();
                distributor.MSN = this.txtMSN.Text.Trim();
                distributor.TelPhone = this.txtTel.Text.Trim();
                distributor.CellPhone = this.txtCellPhone.Text.Trim();
                distributor.IsCreate = false;
                if (this.ValidationDistributorRequest(distributor))
                {
                    if (SubsiteStoreHelper.UpdateDistributor(distributor))
                    {
                        this.ShowMsg("成功的修改了信息", true);
                    }
                    else
                    {
                        this.ShowMsg("修改失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                if ((distributor != null) && (distributor.UserRole == UserRole.Distributor))
                {
                    this.BindData(distributor);
                }
            }
        }

        private bool ValidationDistributorRequest(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
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

        public bool ValidationInput()
        {
            string str = string.Empty;
            if ((string.IsNullOrEmpty(this.txtQQ.Text) && string.IsNullOrEmpty(this.txtWangwang.Text)) && string.IsNullOrEmpty(this.txtMSN.Text))
            {
                str = str + "QQ,旺旺,MSN,三者必填其一";
            }
            if (string.IsNullOrEmpty(this.txtTel.Text) && string.IsNullOrEmpty(this.txtCellPhone.Text))
            {
                str = str + "<br/>固定电话和手机,二者必填其一";
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

