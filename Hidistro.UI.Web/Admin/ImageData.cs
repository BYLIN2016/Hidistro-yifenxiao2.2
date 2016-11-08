namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PictureMange)]
    public class ImageData : AdminPage
    {
        protected ImageLinkButton btnDelete1;
        protected ImageLinkButton btnDelete2;
        protected Button btnImagetSearch;
        protected Button btnMoveImageData;
        protected Button btnSaveImageDataName;
        protected ImageDataGradeDropDownList dropImageFtp;
        private int? enumOrder;
        protected System.Web.UI.WebControls.FileUpload FileUpload;
        public string GlobalsPath = Globals.ApplicationPath;
        protected ImageOrderDropDownList ImageOrder;
        protected ImageTypeLabel ImageTypeID;
        private string keyOrder;
        private string keyWordIName = string.Empty;
        protected Label lblImageData;
        private int pageIndex;
        protected Pager pager;
        protected DataList photoDataList;
        protected TextBox ReImageDataName;
        protected HiddenField ReImageDataNameId;
        protected HiddenField RePlaceId;
        protected HiddenField RePlaceImg;
        protected TextBox txtWordName;
        private int? typeId = null;

        private void BindImageData()
        {
            this.pageIndex = this.pager.PageIndex;
            PhotoListOrder uploadTimeDesc = PhotoListOrder.UploadTimeDesc;
            if (this.enumOrder.HasValue)
            {
                uploadTimeDesc = (PhotoListOrder) Enum.ToObject(typeof(PhotoListOrder), this.enumOrder.Value);
            }
            DbQueryResult result = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, uploadTimeDesc);
            this.photoDataList.DataSource = result.Data;
            this.photoDataList.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            this.lblImageData.Text = "共" + this.pager.TotalRecords + "张";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = true;
            bool flag2 = true;
            foreach (DataListItem item in this.photoDataList.Controls)
            {
                CheckBox box = (CheckBox) item.FindControl("checkboxCol");
                HiddenField field = (HiddenField) item.FindControl("HiddenFieldImag");
                if ((box != null) && box.Checked)
                {
                    flag2 = false;
                    try
                    {
                        int photoId = (int) this.photoDataList.DataKeys[item.ItemIndex];
                        StoreHelper.DeleteImage(field.Value);
                        if (!GalleryHelper.DeletePhoto(photoId))
                        {
                            flag = false;
                        }
                        continue;
                    }
                    catch
                    {
                        this.ShowMsg("删除文件错误", false);
                        this.BindImageData();
                        continue;
                    }
                }
            }
            if (flag2)
            {
                this.ShowMsg("未选择删除的图片", false);
            }
            else
            {
                if (flag)
                {
                    this.ShowMsg("删除图片成功", true);
                }
                this.BindImageData();
            }
        }

        private void btnImagetSearch_Click(object sender, EventArgs e)
        {
            this.keyWordIName = this.txtWordName.Text;
            this.BindImageData();
        }

        private void btnMoveImageData_Click(object sender, EventArgs e)
        {
            List<int> pList = new List<int>();
            int pTypeId = Convert.ToInt32(this.dropImageFtp.SelectedItem.Value);
            foreach (DataListItem item in this.photoDataList.Controls)
            {
                CheckBox box = (CheckBox) item.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    int num2 = (int) this.photoDataList.DataKeys[item.ItemIndex];
                    pList.Add(num2);
                }
            }
            if (GalleryHelper.MovePhotoType(pList, pTypeId) > 0)
            {
                this.ShowMsg("图片移动成功！", true);
            }
            this.BindImageData();
        }

        private void btnSaveImageDataName_Click(object sender, EventArgs e)
        {
            string text = this.ReImageDataName.Text;
            if (string.IsNullOrEmpty(text) || (text.Length > 30))
            {
                this.ShowMsg("图片名称不能为空长度限制在30个字符以内！", false);
            }
            else
            {
                GalleryHelper.RenamePhoto(Convert.ToInt32(this.ReImageDataNameId.Value), text);
                this.BindImageData();
            }
        }

        public static string Html_ToClient(string Str)
        {
            if (Str == null)
            {
                return null;
            }
            if (Str != string.Empty)
            {
                return HttpContext.Current.Server.HtmlDecode(Str.Trim());
            }
            return string.Empty;
        }

        private void ImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.enumOrder = this.ImageOrder.SelectedValue;
            this.BindImageData();
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordIName"]))
            {
                this.keyWordIName = Globals.UrlDecode(this.Page.Request.QueryString["keyWordIName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordSel"]))
            {
                this.keyOrder = Globals.UrlDecode(this.Page.Request.QueryString["keyWordSel"]);
            }
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["imageTypeId"], out result))
            {
                this.typeId = new int?(result);
            }
            if (this.enumOrder.HasValue)
            {
                this.ImageOrder.SelectedValue = this.enumOrder;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnDelete1.Click += new EventHandler(this.btnDelete_Click);
            this.btnDelete2.Click += new EventHandler(this.btnDelete_Click);
            this.btnSaveImageDataName.Click += new EventHandler(this.btnSaveImageDataName_Click);
            this.btnMoveImageData.Click += new EventHandler(this.btnMoveImageData_Click);
            this.ImageOrder.SelectedIndexChanged += new EventHandler(this.ImageOrder_SelectedIndexChanged);
            this.btnImagetSearch.Click += new EventHandler(this.btnImagetSearch_Click);
            this.photoDataList.ItemCommand += new DataListCommandEventHandler(this.photoDataList_ItemCommand);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.ImageOrder.DataBind();
                this.dropImageFtp.DataBind();
                this.BindImageData();
            }
        }

        private void photoDataList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int photoId = Convert.ToInt32(this.photoDataList.DataKeys[e.Item.ItemIndex]);
            string photoPath = GalleryHelper.GetPhotoPath(photoId);
            if (GalleryHelper.DeletePhoto(photoId))
            {
                StoreHelper.DeleteImage(photoPath);
                this.ShowMsg("删除图片成功", true);
            }
            this.BindImageData();
        }

        public static string TruncStr(string str, int maxSize)
        {
            str = Html_ToClient(str);
            if (!(str != string.Empty))
            {
                return string.Empty;
            }
            int num = 0;
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            for (int i = 0; i <= (bytes.Length - 1); i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num > maxSize)
                {
                    str = str.Substring(0, i);
                    return str;
                }
            }
            return str;
        }
    }
}

