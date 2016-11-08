namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using System;
    using System.Net.Mail;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI.WebControls;

    public class MyEmailSettings : DistributorPage
    {
        protected Button btnChangeEmailSettings;
        protected Button btnTestEmailSettings;
        protected Script Script1;
        protected HiddenField txtConfigData;
        protected HiddenField txtSelectedName;
        protected TextBox txtTestEmail;

        private void btnChangeEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                siteSettings.EmailSender = string.Empty;
                siteSettings.EmailSettings = string.Empty;
            }
            else
            {
                if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                    return;
                }
                siteSettings.EmailSender = str;
                siteSettings.EmailSettings = HiCryptographer.Encrypt(data.SettingsXml);
            }
            SettingsManager.Save(siteSettings);
            this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
        }

        private void btnTestEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = this.LoadConfig(out str);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                this.ShowMsg("请先选择发送方式并填写配置信息", false);
            }
            else if (!data.IsValid)
            {
                string msg = "";
                foreach (string str3 in data.ErrorMsgs)
                {
                    msg = msg + Formatter.FormatErrorMessage(str3);
                }
                this.ShowMsg(msg, false);
            }
            else if (string.IsNullOrEmpty(this.txtTestEmail.Text) || (this.txtTestEmail.Text.Trim().Length == 0))
            {
                this.ShowMsg("请填写接收测试邮件的邮箱地址", false);
            }
            else if (!Regex.IsMatch(this.txtTestEmail.Text.Trim(), @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                this.ShowMsg("请填写正确的邮箱地址", false);
            }
            else
            {
                MailMessage message2 = new MailMessage();
                message2.IsBodyHtml = true;
                message2.Priority = MailPriority.High;
                message2.Body = "Success";
                message2.Subject = "This is a test mail";
                MailMessage mail = message2;
                mail.To.Add(this.txtTestEmail.Text.Trim());
                EmailSender sender2 = EmailSender.CreateInstance(str, data.SettingsXml);
                try
                {
                    if (sender2.Send(mail, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                    {
                        this.ShowMsg("发送测试邮件成功", true);
                    }
                    else
                    {
                        this.ShowMsg("发送测试邮件失败", false);
                    }
                }
                catch
                {
                    this.ShowMsg("邮件配置错误", false);
                }
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlEmails"];
            this.txtSelectedName.Value = selectedName;
            this.txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = EmailSender.CreateInstance(selectedName);
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
            this.btnChangeEmailSettings.Click += new EventHandler(this.btnChangeEmailSettings_Click);
            this.btnTestEmailSettings.Click += new EventHandler(this.btnTestEmailSettings_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings.EmailEnabled)
                {
                    this.txtSelectedName.Value = siteSettings.EmailSender.ToLower();
                    ConfigData data = new ConfigData(HiCryptographer.Decrypt(siteSettings.EmailSettings));
                    this.txtConfigData.Value = data.SettingsXml;
                }
            }
        }
    }
}

