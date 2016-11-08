namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.ManageDistributorSites)]
    public class ManageTemples : AdminPage
    {
        protected Button btnSave;
        protected DataList dtManageThemes;
        protected Literal litDomain;
        protected Literal litUserName;
        private int userId;

        private void btnManageThemesOK_Click(object sender, EventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(this.userId);
            string path = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (DataListItem item in this.dtManageThemes.Items)
            {
                CheckBox box = item.FindControl("rbCheckThemes") as CheckBox;
                if (box.Checked)
                {
                    DisplayThemesImages images = (DisplayThemesImages) item.FindControl("themeImg");
                    string srcPath = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + images.ThemeName;
                    string str3 = path + @"\" + images.ThemeName;
                    if (!Directory.Exists(str3))
                    {
                        try
                        {
                            this.CopyDir(srcPath, str3);
                            continue;
                        }
                        catch
                        {
                            this.ShowMsg("修改模板失败", false);
                            continue;
                        }
                    }
                }
            }
            this.ShowMsg("成功修改了店铺模板", true);
            this.GetThemes();
        }

        private void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath = aimPath + Path.DirectorySeparatorChar;
                }
                if (!Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                foreach (string str in Directory.GetFileSystemEntries(srcPath))
                {
                    if (Directory.Exists(str))
                    {
                        this.CopyDir(str, aimPath + Path.GetFileName(str));
                    }
                    else
                    {
                        File.Copy(str, aimPath + Path.GetFileName(str), true);
                    }
                }
            }
            catch
            {
                this.ShowMsg("无法复制!", false);
            }
        }

        private void dtManageThemes_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            IList<ManageThemeInfo> list = this.LoadThemes(@"sites\\" + SettingsManager.GetSiteSettings(this.userId).UserId.ToString());
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                CheckBox box = (CheckBox) e.Item.FindControl("rbCheckThemes");
                foreach (ManageThemeInfo info in list)
                {
                    if (info.ThemeName == box.Text)
                    {
                        box.Checked = true;
                    }
                }
            }
        }

        public void GetThemes()
        {
            this.dtManageThemes.DataSource = this.LoadThemes("library");
            this.dtManageThemes.DataBind();
        }

        private void LoadInfo()
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(this.userId);
            if (siteSettings == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.litDomain.Text = siteSettings.SiteUrl;
                Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.litUserName.Text = distributor.Username;
                }
            }
        }

        protected IList<ManageThemeInfo> LoadThemes(string path)
        {
            HttpContext context = HiContext.Current.Context;
            XmlDocument document = new XmlDocument();
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string str = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\" + path;
            string[] strArray = Directory.Exists(str) ? Directory.GetDirectories(str) : null;
            ManageThemeInfo item = null;
            foreach (string str3 in strArray)
            {
                DirectoryInfo info2 = new DirectoryInfo(str3);
                string str2 = info2.Name.ToLower(CultureInfo.InvariantCulture);
                if ((str2.Length > 0) && !str2.StartsWith("_"))
                {
                    foreach (FileInfo info3 in info2.GetFiles(str2 + ".xml"))
                    {
                        item = new ManageThemeInfo();
                        FileStream inStream = info3.OpenRead();
                        document.Load(inStream);
                        inStream.Close();
                        item.Name = document.SelectSingleNode("ManageTheme/Name").InnerText;
                        item.ThemeImgUrl = document.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
                        item.ThemeName = str2;
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnManageThemesOK_Click);
            this.dtManageThemes.ItemDataBound += new DataListItemEventHandler(this.dtManageThemes_ItemDataBound);
            if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                this.LoadInfo();
                this.GetThemes();
            }
        }
    }
}

