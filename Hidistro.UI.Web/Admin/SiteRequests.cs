namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.DistributorSiteRequests)]
    public class SiteRequests : AdminPage
    {
        protected Button btnRefuse;
        protected Button btnSave;
        protected Button btnSearch;
        protected HtmlGenericControl domainName1;
        protected Grid grdDistributorDomainRequests;
        protected HtmlInputHidden hidRequestId;
        protected PageSize hrefPageSize;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlGenericControl spanDistributorName;
        protected HtmlGenericControl spanUserName;
        protected HtmlGenericControl spanUserNameForRefuse;
        protected TextBox txtDistributorName;
        protected TextBox txtReason;
        private string userName;

        private void BindRequests()
        {
            Pagination pagination = new Pagination();
            pagination.PageIndex = this.pager.PageIndex;
            pagination.PageSize = this.pager.PageSize;
            int total = 0;
            DataTable table = DistributorHelper.GetDomainRequests(pagination, this.userName, out total);
            this.grdDistributorDomainRequests.DataSource = table;
            this.grdDistributorDomainRequests.DataBind();
            this.pager.TotalRecords = total;
            this.pager1.TotalRecords = total;
        }

        private void btnRefuse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtReason.Text.Trim()))
            {
                this.ShowMsg("拒绝原因不能为空", false);
            }
            else if (DistributorHelper.RefuseSiteRequest(int.Parse(this.hidRequestId.Value), this.txtReason.Text.Trim()))
            {
                this.BindRequests();
                this.ShowMsg("您拒绝了该分销商的站点开通申请", true);
            }
            else
            {
                this.ShowMsg("拒绝该该分销商的站点开通申请失败", false);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(int.Parse(this.hidRequestId.Value));
            if (siteRequestInfo == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                int num;
                bool flag;
                bool flag2;
                SiteSettings siteSettings = new SiteSettings(siteRequestInfo.FirstSiteUrl, new int?(siteRequestInfo.UserId));
                siteSettings.Disabled = false;
                siteSettings.CreateDate = new DateTime?(DateTime.Now);
                siteSettings.RequestDate = new DateTime?(siteRequestInfo.RequestTime);
                siteSettings.LogoUrl = "/utility/pics/agentlogo.jpg";
                LicenseChecker.Check(out flag, out flag2, out num);
                if (!DistributorHelper.AddSiteSettings(siteSettings, siteRequestInfo.RequestId, num))
                {
                    this.ShowMsg("开通分销商站点失败，可能是您能够开启的数量已经达到了授权的上限或是授权已过有效期！", false);
                }
                else
                {
                    IList<ManageThemeInfo> list = this.LoadThemes();
                    string path = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + siteSettings.UserId.ToString();
                    string str2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString() + @"\" + list[0].ThemeName;
                    string srcPath = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + list[0].ThemeName;
                    if (!Directory.Exists(path))
                    {
                        try
                        {
                            Directory.CreateDirectory(path);
                            Directory.CreateDirectory(path + "/article");
                            Directory.CreateDirectory(path + "/brand");
                            Directory.CreateDirectory(path + "/fckfiles");
                            Directory.CreateDirectory(path + "/help");
                            Directory.CreateDirectory(path + "/link");
                        }
                        catch
                        {
                            this.ShowMsg("开通分销商站点失败", false);
                            return;
                        }
                    }
                    if (!Directory.Exists(str2))
                    {
                        try
                        {
                            this.CopyDir(srcPath, str2);
                            siteSettings.Theme = list[0].ThemeName;
                            SettingsManager.Save(siteSettings);
                        }
                        catch
                        {
                            this.ShowMsg("开通分销商站点失败", false);
                            return;
                        }
                    }
                    this.BindRequests();
                    this.ShowMsg("成功开通了分销商的站点", true);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
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

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = this.Page.Request.QueryString["userName"];
                }
                this.txtDistributorName.Text = this.userName;
            }
            else
            {
                this.userName = this.txtDistributorName.Text.Trim();
            }
        }

        protected IList<ManageThemeInfo> LoadThemes()
        {
            HttpContext context = HiContext.Current.Context;
            XmlDocument document = new XmlDocument();
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string path = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + @"\Templates\library";
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
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["showMessage"]) && (base.Request["showMessage"] == "true"))
            {
                int result = 0;
                if (string.IsNullOrEmpty(base.Request["requestId"]) || !int.TryParse(base.Request["requestId"], out result))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(result);
                if (siteRequestInfo == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(siteRequestInfo.UserId);
                if (distributor == null)
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                builder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
                builder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
                builder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
                builder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
                builder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
                builder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
                builder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
                builder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
                builder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
                builder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
                builder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
                builder.AppendFormat(",\"Domain1\":\"{0}\"", siteRequestInfo.FirstSiteUrl);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnRefuse.Click += new EventHandler(this.btnRefuse_Click);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindRequests();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtDistributorName.Text.Trim());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

