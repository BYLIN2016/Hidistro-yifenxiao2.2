namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class SetPurchaseOrderOption : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtClosePurchaseOrderDays;
        protected HtmlGenericControl txtClosePurchaseOrderDaysTip;
        protected TextBox txtFinishPurchaseOrderDays;
        protected HtmlGenericControl txtFinishPurchaseOrderDaysTip;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            if (this.ValidateValues(out num, out num2))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.ClosePurchaseOrderDays = num;
                masterSettings.FinishPurchaseOrderDays = num2;
                if (this.ValidationSettings(masterSettings))
                {
                    Globals.EntityCoding(masterSettings, true);
                    SettingsManager.Save(masterSettings);
                    this.ShowMsg("保存成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtClosePurchaseOrderDays.Text = masterSettings.ClosePurchaseOrderDays.ToString(CultureInfo.InvariantCulture);
                this.txtFinishPurchaseOrderDays.Text = masterSettings.FinishPurchaseOrderDays.ToString(CultureInfo.InvariantCulture);
            }
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }

        private bool ValidateValues(out int closePurchaseOrderDays, out int finishPurchaseOrderDays)
        {
            string str = string.Empty;
            if (!int.TryParse(this.txtClosePurchaseOrderDays.Text, out closePurchaseOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("过期几天自动关闭采购单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(this.txtFinishPurchaseOrderDays.Text, out finishPurchaseOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("发货几天自动完成采购单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private bool ValidationSettings(SiteSettings setting)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(setting, new string[] { "ValMasterSettings" });
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

