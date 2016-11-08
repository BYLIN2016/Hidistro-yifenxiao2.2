namespace Hidistro.UI.Web.DialogTemplates
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using LitJson;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public class UploadFileJson : IHttpHandler
    {
        private string message = "";
        private string savePath;
        private string saveUrl;

        private bool CheckUploadFile(HttpPostedFile imgFile, ref string dirPath)
        {
            if (imgFile == null)
            {
                this.showError("请选择上传文件");
                return false;
            }
            if (!ResourcesHelper.CheckPostedFile(imgFile))
            {
                this.showError("不能上传空文件，且必须是有效的图片文件！");
                return false;
            }
            dirPath = Globals.MapPath(this.savePath);
            if (!Directory.Exists(dirPath))
            {
                this.showError("上传目录不存在。");
                return false;
            }
            return true;
        }

        public void ProcessRequest(HttpContext context)
        {
            IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
            string str = "AdvertImg";
            if ((!HiContext.Current.Context.User.IsInRole("manager") && !HiContext.Current.Context.User.IsInRole("systemadministrator")) && ((user.UserRole != UserRole.Distributor) && (user.UserRole != UserRole.SiteManager)))
            {
                this.showError("您没有权限执行此操作");
            }
            else
            {
                if (context.Request.Form["fileCategory"] != null)
                {
                    str = context.Request.Form["fileCategory"];
                }
                string str2 = string.Empty;
                if (context.Request.Form["imgTitle"] != null)
                {
                    str2 = context.Request.Form["imgTitle"];
                }
                this.savePath = string.Format("{0}/UploadImage/" + str + "/", HiContext.Current.GetSkinPath());
                if (context.Request.ApplicationPath != "/")
                {
                    this.saveUrl = this.savePath.Substring(context.Request.ApplicationPath.Length);
                }
                else
                {
                    this.saveUrl = this.savePath;
                }
                HttpPostedFile imgFile = context.Request.Files["imgFile"];
                string dirPath = "";
                if (this.CheckUploadFile(imgFile, ref dirPath))
                {
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string fileName = imgFile.FileName;
                    if (str2.Length == 0)
                    {
                        str2 = fileName;
                    }
                    string str5 = Path.GetExtension(fileName).ToLower();
                    string str6 = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + str5;
                    string filename = dirPath + str6;
                    string str8 = this.saveUrl + str6;
                    try
                    {
                        imgFile.SaveAs(filename);
                        Hashtable hashtable = new Hashtable();
                        hashtable["error"] = 0;
                        hashtable["url"] = Globals.ApplicationPath + str8;
                        this.message = JsonMapper.ToJson(hashtable);
                    }
                    catch
                    {
                        this.showError("保存文件出错");
                    }
                }
            }
            context.Response.ContentType = "text/html";
            context.Response.Write(this.message);
        }

        private void showError(string message)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["error"] = 1;
            hashtable["message"] = message;
            message = JavaScriptConvert.SerializeObject(hashtable);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

