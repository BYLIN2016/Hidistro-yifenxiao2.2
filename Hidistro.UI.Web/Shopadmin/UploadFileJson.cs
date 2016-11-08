namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Subsites.Utility;
    using LitJson;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public class UploadFileJson : DistributorPage
    {
        private string savePath;
        private string saveUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            if ((user.UserRole != UserRole.Distributor) && (user.UserRole != UserRole.SiteManager))
            {
                this.showError("您没有权限执行此操作！");
            }
            else
            {
                string str = "false";
                if (base.Request.Form["isAdvPositions"] != null)
                {
                    str = base.Request.Form["isAdvPositions"].ToString().ToLower().Trim();
                }
                if (user.UserRole == UserRole.SiteManager)
                {
                    if (str == "false")
                    {
                        this.savePath = "~/Storage/master/gallery/";
                        this.saveUrl = "/Storage/master/gallery/";
                    }
                    else
                    {
                        this.savePath = string.Format("{0}/fckfiles/Files/Image/", HiContext.Current.GetSkinPath());
                        if (base.Request.ApplicationPath != "/")
                        {
                            this.saveUrl = this.savePath.Substring(base.Request.ApplicationPath.Length);
                        }
                        else
                        {
                            this.saveUrl = this.savePath;
                        }
                    }
                }
                else if (str == "false")
                {
                    this.savePath = string.Format("~/Storage/sites/{0}/fckfiles/", user.UserId);
                    this.saveUrl = string.Format("/Storage/sites/{0}/fckfiles/", user.UserId);
                }
                else
                {
                    SiteSettings siteSettings = SettingsManager.GetSiteSettings(user.UserId);
                    this.savePath = string.Format("~/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
                    this.saveUrl = string.Format("/Templates/sites/{0}/{1}/fckfiles/Files/Image/", user.UserId, siteSettings.Theme);
                }
                int result = 0;
                if (base.Request.Form["fileCategory"] != null)
                {
                    int.TryParse(base.Request.Form["fileCategory"], out result);
                }
                string str2 = string.Empty;
                if (base.Request.Form["imgTitle"] != null)
                {
                    str2 = base.Request.Form["imgTitle"];
                }
                HttpPostedFile postedFile = base.Request.Files["imgFile"];
                if (postedFile == null)
                {
                    this.showError("请先选择文件！");
                }
                if (!ResourcesHelper.CheckPostedFile(postedFile))
                {
                    this.showError("不能上传空文件，且必须是有效的图片文件！");
                }
                else
                {
                    string path = base.Server.MapPath(this.savePath);
                    if (!Directory.Exists(path))
                    {
                        this.showError("上传目录不存在。");
                    }
                    if ((str == "false") && (user.UserRole == UserRole.SiteManager))
                    {
                        path = path + string.Format("{0}/", DateTime.Now.ToString("yyyyMM"));
                        this.saveUrl = this.saveUrl + string.Format("{0}/", DateTime.Now.ToString("yyyyMM"));
                    }
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = postedFile.FileName;
                    if (str2.Length == 0)
                    {
                        str2 = fileName;
                    }
                    string str5 = Path.GetExtension(fileName).ToLower();
                    string str6 = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + str5;
                    string filename = path + str6;
                    string str8 = this.saveUrl + str6;
                    try
                    {
                        postedFile.SaveAs(filename);
                        if ((user.UserRole == UserRole.SiteManager) && (str == "false"))
                        {
                            Database database = DatabaseFactory.CreateDatabase();
                            DbCommand sqlStringCommand = database.GetSqlStringCommand("insert into Hishop_PhotoGallery(CategoryId,PhotoName,PhotoPath,FileSize,UploadTime,LastUpdateTime)values(@cid,@name,@path,@size,@time,@time1)");
                            database.AddInParameter(sqlStringCommand, "cid", DbType.Int32, result);
                            database.AddInParameter(sqlStringCommand, "name", DbType.String, str2);
                            database.AddInParameter(sqlStringCommand, "path", DbType.String, str8);
                            database.AddInParameter(sqlStringCommand, "size", DbType.Int32, postedFile.ContentLength);
                            database.AddInParameter(sqlStringCommand, "time", DbType.DateTime, DateTime.Now);
                            database.AddInParameter(sqlStringCommand, "time1", DbType.DateTime, DateTime.Now);
                            database.ExecuteNonQuery(sqlStringCommand);
                        }
                    }
                    catch
                    {
                        this.showError("保存文件出错！");
                    }
                    Hashtable hashtable = new Hashtable();
                    hashtable["error"] = 0;
                    hashtable["url"] = Globals.ApplicationPath + str8;
                    base.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                    base.Response.Write(JsonMapper.ToJson(hashtable));
                    base.Response.End();
                }
            }
        }

        private void showError(string message)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["error"] = 1;
            hashtable["message"] = message;
            base.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            base.Response.Write(JsonMapper.ToJson(hashtable));
            base.Response.End();
        }
    }
}

