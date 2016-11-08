namespace Hidistro.UI.Web.DialogTemplates
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SelectImage : Page
    {
        protected HtmlForm form1;
        protected HtmlGenericControl imagesize;
        public int pageindex = 1;
        private int pagesize = 30;
        public int pagetotal;
        protected Repeater rp_img;
        protected HtmlSelect slsbannerposition;
        public int sum;
        private string type = "";

        private void DataBindImages()
        {
            IOrderedEnumerable<FileInfo> source = new DirectoryInfo(Globals.MapPath(HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value)).GetFiles().OrderByDescending<FileInfo, DateTime>(delegate (FileInfo file) {
                return file.CreationTime;
            });
            this.sum = source.Count<FileInfo>();
            this.pagetotal = this.sum / this.pagesize;
            if ((this.sum % this.pagesize) != 0)
            {
                this.pagetotal++;
            }
            if ((this.pageindex < 1) || (this.pageindex > this.pagetotal))
            {
                this.pageindex = 1;
            }
            this.rp_img.DataSource = source.Skip<FileInfo>(((this.pageindex - 1) * this.pagesize)).Take<FileInfo>(this.pagesize);
            this.rp_img.DataBind();
        }

        private void loadParamQuery()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["size"]))
            {
                this.imagesize.InnerText = Globals.HtmlEncode(this.Page.Request.QueryString["size"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["type"]))
            {
                this.slsbannerposition.Value = this.Page.Request.QueryString["type"];
                this.slsbannerposition.Disabled = true;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageindex"]) && (Convert.ToInt32(this.Page.Request.QueryString["pageindex"]) > 0))
            {
                this.pageindex = Convert.ToInt32(this.Page.Request.QueryString["pageindex"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.loadParamQuery();
            if (!string.IsNullOrEmpty(base.Request.QueryString["iscallback"]) && Convert.ToBoolean(base.Request.QueryString["iscallback"]))
            {
                this.UploadImage();
            }
            if (!string.IsNullOrEmpty(base.Request.QueryString["del"]))
            {
                string path = base.Request.QueryString["del"];
                string str2 = Globals.PhysicalPath(path);
                if (File.Exists(str2))
                {
                    File.Delete(str2);
                }
            }
            if (!base.IsPostBack)
            {
                this.DataBindImages();
            }
        }

        protected string ShowImage(string filename)
        {
            filename = Globals.ApplicationPath + HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/" + filename;
            return filename;
        }

        private void UploadImage()
        {
            System.Drawing.Image image = null;
            System.Drawing.Image image2 = null;
            Bitmap bitmap = null;
            Graphics graphics = null;
            MemoryStream stream = null;
            try
            {
                HttpPostedFile file = base.Request.Files["Filedata"];
                string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
                string str2 = HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/";
                string str3 = str + Path.GetExtension(file.FileName);
                file.SaveAs(Globals.MapPath(str2 + str3));
                base.Response.StatusCode = 200;
                base.Response.Write(Globals.ApplicationPath + HiContext.Current.GetSkinPath() + "/UploadImage/" + this.slsbannerposition.Value + "/" + str3);
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("服务器错误");
                base.Response.End();
            }
            finally
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (image2 != null)
                {
                    image2.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                base.Response.End();
            }
        }
    }
}

