namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Text;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class SiteMap : AdminPage
    {
        protected Button btnSaveMapSettings;
        protected HyperLink Hysitemap;
        protected TextBox tbsitemapnum;
        protected TextBox tbsitemaptime;

        protected void BindSiteMap()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!string.IsNullOrEmpty(masterSettings.SiteMapTime))
            {
                this.tbsitemaptime.Text = masterSettings.SiteMapTime;
            }
            if (!string.IsNullOrEmpty(masterSettings.SiteMapNum))
            {
                this.tbsitemapnum.Text = masterSettings.SiteMapNum;
            }
        }

        protected void btnSaveMapSettings_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (!string.IsNullOrEmpty(this.tbsitemaptime.Text) && !string.IsNullOrEmpty(this.tbsitemapnum.Text))
            {
                masterSettings.SiteMapNum = this.tbsitemapnum.Text;
                masterSettings.SiteMapTime = this.tbsitemaptime.Text;
                SettingsManager.Save(masterSettings);
                this.BindSiteMap();
                this.ShowMsg("保存成功。", true);
            }
            else
            {
                this.ShowMsg("参数错误。", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindSiteMap();
            }
            string str = HiContext.Current.HostPath + Globals.ApplicationPath + "/sitemapindex.xml";
            this.Hysitemap.Text = str;
            this.Hysitemap.NavigateUrl = str;
            this.Hysitemap.Target = "_blank";
            StreamReader reader = new StreamReader(base.Server.MapPath(Globals.ApplicationPath + "/robots.txt"), Encoding.Default);
            string str2 = reader.ReadToEnd();
            reader.Close();
            if (str2.Contains("Sitemap"))
            {
                str2 = str2.Substring(0, str2.IndexOf("Sitemap"));
            }
            FileStream stream = new FileStream(base.Server.MapPath(Globals.ApplicationPath + "/robots.txt"), FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
                {
                    writer.Flush();
                    writer.Write(str2);
                    writer.WriteLine("Sitemap: " + str);
                    writer.Flush();
                    writer.Dispose();
                    writer.Close();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                stream.Dispose();
                stream.Close();
            }
        }
    }
}

