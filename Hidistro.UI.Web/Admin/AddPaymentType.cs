namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using kindeditor.Net;
    using System;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PaymentModes)]
    public class AddPaymentType : AdminPage
    {
        protected Button btnCreate;
        protected CheckBox chkIsPercent;
        protected KindeditorControl fcContent;
        protected YesNoRadioButtonList radiIsUseInDistributor;
        protected YesNoRadioButtonList radiIsUseInpour;
        protected Script Script1;
        protected TextBox txtCharge;
        protected HiddenField txtConfigData;
        protected TextBox txtName;
        protected HiddenField txtSelectedName;

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data;
            decimal num;
            if (this.ValidateValues(out str, out data, out num))
            {
                PaymentModeInfo info2 = new PaymentModeInfo();
                info2.Name = this.txtName.Text;
                info2.Description = this.fcContent.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                info2.Gateway = str;
                info2.IsUseInpour = this.radiIsUseInpour.SelectedValue;
                info2.IsUseInDistributor = this.radiIsUseInDistributor.SelectedValue;
                info2.Charge = num;
                info2.IsPercent = this.chkIsPercent.Checked;
                info2.Settings = HiCryptographer.Encrypt(data.SettingsXml);
                PaymentModeInfo paymentMode = info2;
                switch (SalesHelper.CreatePaymentMode(paymentMode))
                {
                    case PaymentModeActionStatus.Success:
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("sales/PaymentTypes.aspx"));
                        return;

                    case PaymentModeActionStatus.DuplicateGateway:
                        this.ShowMsg("已经添加了一个相同类型的支付接口", false);
                        return;

                    case PaymentModeActionStatus.DuplicateName:
                        this.ShowMsg("已经存在一个相同的支付方式名称", false);
                        return;

                    case PaymentModeActionStatus.OutofNumber:
                        this.ShowMsg("支付方式的数目已经超出系统设置的数目", false);
                        return;
                }
                this.ShowMsg("未知错误", false);
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlPayments"];
            this.txtSelectedName.Value = selectedName;
            this.txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = PaymentRequest.CreateInstance(selectedName);
            if (plugin == null)
            {
                return null;
            }
            ConfigData configData = plugin.GetConfigData(base.Request.Form);
            if (configData != null)
            {
                this.txtConfigData.Value = configData.SettingsXml;
            }
            return configData;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                this.txtCharge.Text = "0";
            }
        }

        private bool ValidateValues(out string selectedPlugin, out ConfigData data, out decimal payCharge)
        {
            string str = string.Empty;
            data = this.LoadConfig(out selectedPlugin);
            payCharge = 0M;
            if (string.IsNullOrEmpty(selectedPlugin))
            {
                this.ShowMsg("请先选择一个支付接口类型", false);
                return false;
            }
            if (!data.IsValid)
            {
                foreach (string str2 in data.ErrorMsgs)
                {
                    str = str + Formatter.FormatErrorMessage(str2);
                }
            }
            if (!decimal.TryParse(this.txtCharge.Text, out payCharge))
            {
                str = str + Formatter.FormatErrorMessage("支付手续费无效,大小在0-10000000之间");
            }
            if ((payCharge < 0M) || (payCharge > 10000000M))
            {
                str = str + Formatter.FormatErrorMessage("支付手续费大小1-10000000之间");
            }
            if (string.IsNullOrEmpty(this.txtName.Text) || (this.txtName.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("支付方式名称不能为空，长度限制在1-60个字符之间");
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

