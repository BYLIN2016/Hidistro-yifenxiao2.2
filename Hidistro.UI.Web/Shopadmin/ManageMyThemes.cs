namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Entities;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ManageMyThemes : DistributorPage
    {
        protected Button btnUpload2;
        protected DataList dtManageThemes;
        protected FileUpload fileTemplate;
        protected HtmlInputHidden hdtempname;
        protected Image Image1;
        protected Image imgThemeImgUrl;
        protected Literal litThemeName;

        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            string str = this.hdtempname.Value.Trim();
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("无法获取对应模板名称,请重新上传", false);
            }
            else if ((this.fileTemplate.PostedFile.ContentLength == 0) || (((this.fileTemplate.PostedFile.ContentType != "application/x-zip-compressed") && (this.fileTemplate.PostedFile.ContentType != "application/zip")) && (this.fileTemplate.PostedFile.ContentType != "application/octet-stream")))
            {
                this.ShowMsg("请上传正确的数据包文件", false);
            }
            else
            {
                string fileName = Path.GetFileName(this.fileTemplate.PostedFile.FileName);
                if (!fileName.Equals(str + ".zip"))
                {
                    this.ShowMsg("上传的模板压缩名与原模板名不一致", false);
                }
                else
                {
                    string str3 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/" + HiContext.Current.User.UserId.ToString());
                    string filename = Path.Combine(str3, fileName);
                    this.fileTemplate.PostedFile.SaveAs(filename);
                    this.PrepareDataFiles(str3, new object[] { filename });
                    File.Delete(filename);
                    this.UserTemplate(str);
                    this.GetThemes(SettingsManager.GetSiteSettings(HiContext.Current.User.UserId).Theme);
                    this.ShowMsg("上传成功！", true);
                    this.hdtempname.Value = "";
                }
            }
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

        private void dtManageThemes_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DisplayThemesImages images = (DisplayThemesImages) e.Item.FindControl("themeImg");
                string path = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/" + images.ThemeName);
                string directoryName = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/" + HiContext.Current.User.UserId.ToString() + "/" + images.ThemeName);
                if (e.CommandName == "btnUse")
                {
                    this.UserTemplate(images.ThemeName);
                    this.ShowMsg("成功修改了店铺模板", true);
                    this.GetThemes(images.ThemeName);
                }
                if (e.CommandName == "download")
                {
                    new DirectoryInfo(path);
                    Encoding encoding = Encoding.UTF8;
                    using (ZipFile file = new ZipFile())
                    {
                        file.CompressionLevel = CompressionLevel.Default;
                        file.AddDirectory(directoryName);
                        HttpResponse response = HttpContext.Current.Response;
                        response.ContentType = "application/zip";
                        response.ContentEncoding = encoding;
                        response.AddHeader("Content-Disposition", "attachment;filename=" + images.ThemeName + ".zip");
                        response.Clear();
                        file.Save(response.OutputStream);
                        response.Flush();
                        response.Close();
                    }
                }
                if (e.CommandName == "back")
                {
                    try
                    {
                        if (!Directory.Exists(path))
                        {
                            this.ShowMsg("主站不存在该模板", false);
                        }
                        else
                        {
                            this.CopyDir(path, directoryName);
                            this.ShowMsg("恢复店铺模板成功", true);
                        }
                    }
                    catch
                    {
                        this.ShowMsg("恢复店铺模板失败", false);
                    }
                }
            }
        }

        public void GetThemes(string currentThemeName)
        {
            this.dtManageThemes.DataSource = this.LoadThemes(currentThemeName);
            this.dtManageThemes.DataBind();
        }

        protected IList<ManageThemeInfo> LoadThemes(string currentThemeName)
        {
            HttpContext context = HiContext.Current.Context;
            XmlDocument document = new XmlDocument();
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string path = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\sites\" + HiContext.Current.User.UserId.ToString();
            string[] strArray = Directory.Exists(path) ? Directory.GetDirectories(path) : null;
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
                        if (string.Compare(item.ThemeName, currentThemeName) == 0)
                        {
                            this.litThemeName.Text = item.ThemeName;
                            this.imgThemeImgUrl.ImageUrl = string.Concat(new object[] { Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", str2, "/", document.SelectSingleNode("ManageTheme/ImageUrl").InnerText });
                            this.Image1.ImageUrl = string.Concat(new object[] { Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", str2, "/", document.SelectSingleNode("ManageTheme/BigImageUrl").InnerText });
                        }
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dtManageThemes.ItemCommand += new DataListCommandEventHandler(this.dtManageThemes_ItemCommand);
            this.btnUpload2.Click += new EventHandler(this.btnUpload2_Click);
            if (!this.Page.IsPostBack)
            {
                string theme = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId).Theme;
                this.GetThemes(theme);
            }
        }

        private string PrepareDataFiles(string _datapath, params object[] initParams)
        {
            string path = (string) initParams[0];
            DirectoryInfo info2 = new DirectoryInfo(_datapath);
            DirectoryInfo info = info2.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(info2.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(info.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return info.FullName;
        }

        protected void UserTemplate(string name)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            siteSettings.Theme = name;
            SettingsManager.Save(siteSettings);
            HiCache.Remove(string.Format("AdsFileCache-{0}", HiContext.Current.User.UserId));
            HiCache.Remove(string.Format("ArticleSubjectFileCache-{0}", HiContext.Current.User.UserId));
            HiCache.Remove(string.Format("ProductSubjectFileCache-{0}", HiContext.Current.User.UserId));
        }
    }
}

