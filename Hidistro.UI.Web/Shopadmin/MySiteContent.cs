namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MySiteContent : DistributorPage
    {
        protected ImageLinkButton btnDeleteLogo;
        protected Button btnOK;
        protected DecimalLengthDropDownList dropBitNumber;
        protected KindeditorControl fckRegisterAgreement;
        protected KindeditorControl fcOnLineServer;
        protected FileUpload fileUpload;
        protected KindeditorControl fkFooter;
        protected HiImage imgLogo;
        protected TextBox txtNamePrice;
        protected HtmlGenericControl txtNamePriceTip;
        protected TextBox txtSearchMetaDescription;
        protected HtmlGenericControl txtSearchMetaDescriptionTip;
        protected TextBox txtSearchMetaKeywords;
        protected HtmlGenericControl txtSearchMetaKeywordsTip;
        protected TextBox txtShowDays;
        protected HtmlGenericControl txtShowDaysTip;
        protected TextBox txtSiteDescription;
        protected HtmlGenericControl txtSiteDescriptionTip;
        protected TextBox txtSiteName;
        protected HtmlGenericControl txtSiteNameTip;

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            try
            {
                SubsiteStoreHelper.DeleteImage(siteSettings.LogoUrl);
            }
            catch
            {
            }
            siteSettings.LogoUrl = string.Empty;
            SettingsManager.Save(siteSettings);
            this.LoadSiteContent(siteSettings);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int num;
            if (this.ValidateValues(out num))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (this.fileUpload.HasFile)
                {
                    try
                    {
                        siteSettings.LogoUrl = SubsiteStoreHelper.UploadLogo(this.fileUpload.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                siteSettings.SiteName = this.txtSiteName.Text.Trim();
                siteSettings.SiteDescription = this.txtSiteDescription.Text.Trim();
                siteSettings.RegisterAgreement = this.fckRegisterAgreement.Text.Trim();
                siteSettings.SearchMetaDescription = this.txtSearchMetaDescription.Text.Trim();
                siteSettings.SearchMetaKeywords = this.txtSearchMetaKeywords.Text.Trim();
                if (!string.IsNullOrEmpty(this.fcOnLineServer.Text))
                {
                    siteSettings.HtmlOnlineServiceCode = this.fcOnLineServer.Text.Trim().Replace("'", @"\");
                }
                else
                {
                    siteSettings.HtmlOnlineServiceCode = string.Empty;
                }
                siteSettings.Footer = this.fkFooter.Text;
                siteSettings.DecimalLength = this.dropBitNumber.SelectedValue;
                if (this.txtNamePrice.Text.Length <= 20)
                {
                    siteSettings.YourPriceName = this.txtNamePrice.Text;
                }
                siteSettings.OrderShowDays = num;
                Globals.EntityCoding(siteSettings, true);
                if (this.ValidationSettings(siteSettings))
                {
                    SettingsManager.Save(siteSettings);
                    this.ShowMsg("成功修改了店铺基本信息", true);
                    this.LoadSiteContent(siteSettings);
                }
            }
        }

        private void LoadSiteContent(SiteSettings siteSettings)
        {
            this.txtSiteName.Text = siteSettings.SiteName;
            this.imgLogo.ImageUrl = siteSettings.LogoUrl;
            if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
            {
                this.btnDeleteLogo.Visible = true;
            }
            else
            {
                this.btnDeleteLogo.Visible = false;
            }
            this.txtSiteDescription.Text = siteSettings.SiteDescription;
            this.txtSearchMetaDescription.Text = siteSettings.SearchMetaDescription;
            this.txtSearchMetaKeywords.Text = siteSettings.SearchMetaKeywords;
            if (!string.IsNullOrEmpty(siteSettings.HtmlOnlineServiceCode))
            {
                this.fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode.Replace(@"\", "'");
            }
            this.fkFooter.Text = siteSettings.Footer;
            this.fckRegisterAgreement.Text = siteSettings.RegisterAgreement;
            this.dropBitNumber.SelectedValue = siteSettings.DecimalLength;
            this.txtNamePrice.Text = siteSettings.YourPriceName;
            this.txtShowDays.Text = siteSettings.OrderShowDays.ToString(CultureInfo.InvariantCulture);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnDeleteLogo.Click += new EventHandler(this.btnDeleteLogo_Click);
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            if (!this.Page.IsPostBack)
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings == null)
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/store/SiteRequest.aspx");
                }
                this.LoadSiteContent(siteSettings);
            }
        }

        private bool ValidateValues(out int showDays)
        {
            string str = string.Empty;
            if (!int.TryParse(this.txtShowDays.Text, out showDays))
            {
                str = str + Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
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

