namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ImageReplace : AdminPage
    {
        protected Button btnSaveImageData;
        protected FileUpload FileUpload1;
        protected HtmlInputHidden Hidden1;
        protected HiddenField RePlaceId;
        protected HiddenField RePlaceImg;

        protected void btnSaveImageData_Click(object sender, EventArgs e)
        {
            string str = this.RePlaceImg.Value;
            int photoId = Convert.ToInt32(this.RePlaceId.Value);
            string photoPath = GalleryHelper.GetPhotoPath(photoId);
            string str3 = photoPath.Substring(photoPath.LastIndexOf("."));
            string extension = string.Empty;
            string str5 = string.Empty;
            try
            {
                HttpPostedFile postedFile = base.Request.Files[0];
                extension = Path.GetExtension(postedFile.FileName);
                if (str3 != extension)
                {
                    this.ShowMsg("上传图片类型与原文件类型不一致！", false);
                }
                else
                {
                    string str6 = Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/gallery";
                    str5 = photoPath.Substring(photoPath.LastIndexOf("/") + 1);
                    string str7 = str.Substring(str.LastIndexOf("/") - 6, 6);
                    string virtualPath = str6 + "/" + str7 + "/";
                    int contentLength = postedFile.ContentLength;
                    string path = base.Request.MapPath(virtualPath);
                    string text1 = str7 + "/" + str5;
                    DirectoryInfo info = new DirectoryInfo(path);
                    if (!info.Exists)
                    {
                        info.Create();
                    }
                    if (!ResourcesHelper.CheckPostedFile(postedFile))
                    {
                        this.ShowMsg("文件上传的类型不正确！", false);
                    }
                    else if (contentLength >= 0x1f4000)
                    {
                        this.ShowMsg("图片文件已超过网站限制大小！", false);
                    }
                    else
                    {
                        postedFile.SaveAs(base.Request.MapPath(virtualPath + str5));
                        GalleryHelper.ReplacePhoto(photoId, contentLength);
                        this.CloseWindow();
                    }
                }
            }
            catch
            {
                this.ShowMsg("替换文件错误!", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!base.IsPostBack && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgsrc"])) && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgId"]))
            {
                string str = Globals.HtmlDecode(this.Page.Request.QueryString["imgsrc"]);
                string str2 = Globals.HtmlDecode(this.Page.Request.QueryString["imgId"]);
                this.RePlaceImg.Value = str;
                this.RePlaceId.Value = str2;
            }
            this.btnSaveImageData.Click += new EventHandler(this.btnSaveImageData_Click);
        }
    }
}

