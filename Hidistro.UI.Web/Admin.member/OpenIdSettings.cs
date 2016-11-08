namespace Hidistro.UI.Web.Admin.member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using kindeditor.Net;
    using System;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.OpenIdSettings)]
    public class OpenIdSettings : AdminPage
    {
        protected Button btnSave;
        protected KindeditorControl fcContent;
        protected Literal lblDisplayName;
        protected Literal lblDisplayName2;
        private string openIdType;
        protected Script Script1;
        protected HiddenField txtConfigData;
        protected TextBox txtName;
        protected HiddenField txtSelectedName;

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfigData data;
            if (this.ValidateValues(out data))
            {
                OpenIdSettingsInfo info2 = new OpenIdSettingsInfo();
                info2.Name = this.txtName.Text.Trim();
                info2.Description = this.fcContent.Text;
                info2.OpenIdType = this.openIdType;
                info2.Settings = HiCryptographer.Encrypt(data.SettingsXml);
                OpenIdSettingsInfo settings = info2;
                OpenIdHelper.SaveSettings(settings);
                base.Response.Redirect("openidservices.aspx");
            }
        }

        private ConfigData LoadConfig()
        {
            this.txtSelectedName.Value = this.openIdType;
            this.txtConfigData.Value = "";
            ConfigablePlugin plugin = OpenIdService.CreateInstance(this.openIdType);
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

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.openIdType = base.Request.QueryString["t"];
            if (string.IsNullOrEmpty(this.openIdType) || (this.openIdType.Trim().Length == 0))
            {
                base.GotoResourceNotFound();
            }
            PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(this.openIdType);
            if (pluginItem == null)
            {
                base.GotoResourceNotFound();
            }
            if (!this.Page.IsPostBack)
            {
                this.txtName.Text = pluginItem.DisplayName;
                this.lblDisplayName.Text = pluginItem.DisplayName;
                this.lblDisplayName2.Text = pluginItem.DisplayName;
                this.txtSelectedName.Value = this.openIdType;
                OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(this.openIdType);
                if (openIdSettings != null)
                {
                    ConfigData data = new ConfigData(HiCryptographer.Decrypt(openIdSettings.Settings));
                    this.txtConfigData.Value = data.SettingsXml;
                    this.txtName.Text = openIdSettings.Name;
                    this.fcContent.Text = openIdSettings.Description;
                }
            }
        }

        private bool ValidateValues(out ConfigData data)
        {
            string str = string.Empty;
            data = this.LoadConfig();
            if (!data.IsValid)
            {
                foreach (string str2 in data.ErrorMsgs)
                {
                    str = str + Formatter.FormatErrorMessage(str2);
                }
            }
            if ((string.IsNullOrEmpty(this.txtName.Text) || (this.txtName.Text.Trim().Length == 0)) || (this.txtName.Text.Length > 50))
            {
                str = str + Formatter.FormatErrorMessage("显示名称不能为空，长度限制在50个字符以内");
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

