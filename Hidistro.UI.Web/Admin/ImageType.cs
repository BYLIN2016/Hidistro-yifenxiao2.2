namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PictureMange)]
    public class ImageType : AdminPage
    {
        protected TextBox AddImageTypeName;
        protected Button btnSaveImageType;
        protected Button ImageTypeAdd;
        protected Button ImageTypeEdit;
        protected ImageTypeLabel ImageTypeID;
        protected Grid ImageTypeList;

        private void btnSaveImageType_Click(object sender, EventArgs e)
        {
            string text = this.AddImageTypeName.Text;
            if (text.Length == 0)
            {
                this.ShowMsg("分类名称不能为空", false);
            }
            else if (text.Length > 20)
            {
                this.ShowMsg("分类名称长度限在20个字符以内", false);
            }
            else if (GalleryHelper.AddPhotoCategory(Globals.HtmlEncode(text)))
            {
                this.ShowMsg("添加成功！", true);
                this.GetImageType();
            }
            else
            {
                this.ShowMsg("添加失败", false);
            }
        }

        private void GetImageType()
        {
            this.ImageTypeList.DataSource = GalleryHelper.GetPhotoCategories();
            this.ImageTypeList.DataBind();
        }

        private void ImageTypeEdit_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> photoCategorys = new Dictionary<int, string>();
            for (int i = 0; i < this.ImageTypeList.Rows.Count; i++)
            {
                GridViewRow row = this.ImageTypeList.Rows[i];
                string text = ((TextBox) row.Cells[1].FindControl("ImageTypeName")).Text;
                if (text.Length > 20)
                {
                    this.ShowMsg("分类长度限在20个字符以内", false);
                    return;
                }
                int key = Convert.ToInt32(this.ImageTypeList.DataKeys[i].Value);
                photoCategorys.Add(key, Globals.HtmlEncode(text.ToString()));
            }
            try
            {
                if (GalleryHelper.UpdatePhotoCategories(photoCategorys) > 0)
                {
                    this.ShowMsg("修改成功！", true);
                }
                else
                {
                    this.ShowMsg("修改失败！", false);
                }
            }
            catch
            {
                this.ShowMsg("修改失败！", false);
            }
        }

        protected void ImageTypeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int num2 = (int) this.ImageTypeList.DataKeys[rowIndex].Value;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.ImageTypeList.Rows.Count - 1))
                {
                    GalleryHelper.SwapSequence(num2, (int) this.ImageTypeList.DataKeys[rowIndex + 1].Value);
                    this.GetImageType();
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                GalleryHelper.SwapSequence(num2, (int) this.ImageTypeList.DataKeys[rowIndex - 1].Value);
                this.GetImageType();
            }
        }

        protected void ImageTypeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int categoryId = (int) this.ImageTypeList.DataKeys[e.RowIndex].Value;
            if (GalleryHelper.DeletePhotoCategory(categoryId))
            {
                this.ShowMsg("删除成功!", true);
                this.GetImageType();
            }
            else
            {
                this.ShowMsg("删除分类失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.ImageTypeList.RowDeleting += new GridViewDeleteEventHandler(this.ImageTypeList_RowDeleting);
            this.ImageTypeList.RowCommand += new GridViewCommandEventHandler(this.ImageTypeList_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveImageType.Click += new EventHandler(this.btnSaveImageType_Click);
            this.ImageTypeEdit.Click += new EventHandler(this.ImageTypeEdit_Click);
            if (!this.Page.IsPostBack)
            {
                this.GetImageType();
            }
        }
    }
}

