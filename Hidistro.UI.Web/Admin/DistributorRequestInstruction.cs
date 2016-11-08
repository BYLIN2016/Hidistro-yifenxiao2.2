namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorRequestInstruction)]
    public class DistributorRequestInstruction : AdminPage
    {
        protected Button btnOK;
        protected KindeditorControl fkFooter;
        protected KindeditorControl fkProtocols;

        private void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.DistributorRequestInstruction = this.fkFooter.Text;
            masterSettings.DistributorRequestProtocols = this.fkProtocols.Text;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(masterSettings, new string[] { "ValRequestProtocols" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else
            {
                SettingsManager.Save(masterSettings);
                this.ShowMsg("保存成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.fkFooter.Text = masterSettings.DistributorRequestInstruction;
                this.fkProtocols.Text = masterSettings.DistributorRequestProtocols;
            }
        }
    }
}

