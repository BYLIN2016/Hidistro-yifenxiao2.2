namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Urls;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.SiteSettings)]
    public class SiteContent : AdminPage
    {
        protected Label Label1;
        protected ImageLinkButton btnDeleteLogo;
        protected Button btnkey;
        protected Button btnOK;
        protected Button btnUpoad;
        protected DecimalLengthDropDownList dropBitNumber;
        protected KindeditorControl fckRegisterAgreement;
        protected KindeditorControl fcOnLineServer;
        protected FileUpload fileUpload;
        protected KindeditorControl fkFooter;
        protected HiImage imgLogo;
        protected HtmlGenericControl P1;
        protected YesNoRadioButtonList radEnableHtmRewrite;
        protected YesNoRadioButtonList radiIsOpenSiteSale;
        protected YesNoRadioButtonList radiIsShowCountDown;
        protected YesNoRadioButtonList radiIsShowGroupBuy;
        protected YesNoRadioButtonList radiIsShowOnlineGift;
        protected TextBox txtDomainName;
        protected HtmlGenericControl txtDomainNameTip;
        protected Literal txtkeycode;
        protected TextBox txtNamePrice;
        protected HtmlGenericControl txtNamePriceTip;
        protected HtmlGenericControl txtProductPointSetTip;
        protected TextBox txtSiteName;
        protected HtmlGenericControl txtSiteNameTip;
        protected ImageUploader uploader1;

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            try
            {
                StoreHelper.DeleteImage(masterSettings.LogoUrl);
            }
            catch
            {
            }
            masterSettings.LogoUrl = string.Empty;
            if (this.ValidationSettings(masterSettings))
            {
                SettingsManager.Save(masterSettings);
                this.LoadSiteContent(masterSettings);
            }
        }

        protected void btnkey_Click(object sender, EventArgs e)
        {
            this.txtkeycode.Text = this.CreateKey(20);
            this.btnOK_Click(sender, e);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.SiteName = this.txtSiteName.Text.Trim();
            masterSettings.SiteUrl = this.txtDomainName.Text.Trim();
            masterSettings.HtmlOnlineServiceCode = this.fcOnLineServer.Text;
            masterSettings.Footer = this.fkFooter.Text;
            masterSettings.RegisterAgreement = this.fckRegisterAgreement.Text;
            masterSettings.DefaultProductImage = this.uploader1.UploadedImageUrl;
            masterSettings.DecimalLength = this.dropBitNumber.SelectedValue;
            if (this.txtNamePrice.Text.Length <= 20)
            {
                masterSettings.YourPriceName = this.txtNamePrice.Text;
            }
            masterSettings.DefaultProductImage = this.uploader1.UploadedImageUrl;
            masterSettings.DefaultProductThumbnail1 = this.uploader1.ThumbnailUrl40;
            masterSettings.DefaultProductThumbnail2 = this.uploader1.ThumbnailUrl60;
            masterSettings.DefaultProductThumbnail3 = this.uploader1.ThumbnailUrl100;
            masterSettings.DefaultProductThumbnail4 = this.uploader1.ThumbnailUrl160;
            masterSettings.DefaultProductThumbnail5 = this.uploader1.ThumbnailUrl180;
            masterSettings.DefaultProductThumbnail6 = this.uploader1.ThumbnailUrl220;
            masterSettings.DefaultProductThumbnail7 = this.uploader1.ThumbnailUrl310;
            masterSettings.DefaultProductThumbnail8 = this.uploader1.ThumbnailUrl410;
            masterSettings.CheckCode = this.txtkeycode.Text;
            masterSettings.IsOpenSiteSale = this.radiIsOpenSiteSale.SelectedValue;
            masterSettings.CheckCode = this.txtkeycode.Text.Trim();
            if (this.ValidationSettings(masterSettings))
            {
                Globals.EntityCoding(masterSettings, true);
                SettingsManager.Save(masterSettings);
                if (this.radEnableHtmRewrite.SelectedValue != SiteUrls.GetEnableHtmRewrite())
                {
                    if (this.radEnableHtmRewrite.SelectedValue)
                    {
                        SiteUrls.EnableHtmRewrite();
                    }
                    else
                    {
                        SiteUrls.CloseHtmRewrite();
                    }
                }
                this.ShowMsg("成功修改了店铺基本信息", true);
                this.LoadSiteContent(masterSettings);
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (this.fileUpload.HasFile)
            {
                try
                {
                    masterSettings.LogoUrl = StoreHelper.UploadLogo(this.fileUpload.PostedFile);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            SettingsManager.Save(masterSettings);
            this.LoadSiteContent(masterSettings);
        }

        private string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        private void LoadSiteContent(SiteSettings siteSettings)
        {
            this.txtSiteName.Text = siteSettings.SiteName;
            this.txtDomainName.Text = siteSettings.SiteUrl;
            this.imgLogo.ImageUrl = siteSettings.LogoUrl;
            if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
            {
                this.btnDeleteLogo.Visible = true;
            }
            else
            {
                this.btnDeleteLogo.Visible = false;
            }
            this.fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode;
            this.fkFooter.Text = siteSettings.Footer;
            this.fckRegisterAgreement.Text = siteSettings.RegisterAgreement;
            this.txtkeycode.Text = siteSettings.CheckCode;
            this.dropBitNumber.SelectedValue = siteSettings.DecimalLength;
            this.txtNamePrice.Text = siteSettings.YourPriceName;
            this.uploader1.UploadedImageUrl = siteSettings.DefaultProductImage;
            this.radiIsOpenSiteSale.SelectedValue = siteSettings.IsOpenSiteSale;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label1.Text = "易通网络销售！！！！！！！！！！！！！！！！！！";
                this.btnUpoad.Click += new EventHandler(this.btnUpoad_Click);
                this.btnDeleteLogo.Click += new EventHandler(this.btnDeleteLogo_Click);
                this.btnkey.Click += new EventHandler(this.btnkey_Click);
                this.btnOK.Click += new EventHandler(this.btnOK_Click);
                if (!this.Page.IsPostBack)
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    this.LoadSiteContent(masterSettings);
                    this.radEnableHtmRewrite.SelectedValue = SiteUrls.GetEnableHtmRewrite();
                }
            }
            catch
            { }
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

