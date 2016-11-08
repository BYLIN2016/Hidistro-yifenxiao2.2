namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class SetSEO : AdminPage
    {
        protected Button btnOK;
        protected TextBox txtSearchMetaDescription;
        protected HtmlGenericControl txtSearchMetaDescriptionTip;
        protected TextBox txtSearchMetaKeywords;
        protected HtmlGenericControl txtSearchMetaKeywordsTip;
        protected TextBox txtSiteDescription;
        protected HtmlGenericControl txtSiteDescriptionTip;

        protected void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.SiteDescription = this.txtSiteDescription.Text.Trim();
            masterSettings.SearchMetaDescription = this.txtSearchMetaDescription.Text.Trim();
            masterSettings.SearchMetaKeywords = this.txtSearchMetaKeywords.Text.Trim();
            Globals.EntityCoding(masterSettings, true);
            SettingsManager.Save(masterSettings);
            this.ShowMsg("保存成功", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this.txtSiteDescription.Text = masterSettings.SiteDescription;
                this.txtSearchMetaDescription.Text = masterSettings.SearchMetaDescription;
                this.txtSearchMetaKeywords.Text = masterSettings.SearchMetaKeywords;
            }
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
        }
    }
}

