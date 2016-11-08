namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class SetCategoryTemplate : AdminPage
    {
        protected ImageLinkButton btnDelete;
        protected Button btnSaveAll;
        protected Button btnUpload;
        protected DropdownColumn dropTheme;
        protected DropDownList dropThmes;
        protected FileUpload fileThame;
        protected Grid grdCategries;

        private void BindData()
        {
            DropdownColumn column = (DropdownColumn) this.grdCategries.Columns[1];
            column.DataSource = this.GetThemes();
            this.grdCategries.DataSource = CatalogHelper.GetMainCategories();
            this.grdCategries.DataBind();
        }

        private void BindDorpDown()
        {
            this.dropThmes.Items.Clear();
            this.dropThmes.Items.Add(new ListItem("请选择分类模板文件", ""));
            foreach (ManageThemeInfo info in this.GetThemes())
            {
                this.dropThmes.Items.Add(new ListItem(info.Name, info.ThemeName));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.dropThmes.SelectedValue))
            {
                this.ShowMsg("请选择您要删除的模板", false);
            }
            else if (!this.validata(this.dropThmes.SelectedItem.Text))
            {
                this.ShowMsg("您要删除的模板正在被使用，不能删除", false);
            }
            else
            {
                string virtualPath = HiContext.Current.GetSkinPath() + "/categorythemes/" + this.dropThmes.SelectedValue;
                virtualPath = HiContext.Current.Context.Request.MapPath(virtualPath);
                if (!File.Exists(virtualPath))
                {
                    this.ShowMsg(string.Format("删除失败!模板{0}已经不存在", this.dropThmes.SelectedValue), false);
                }
                else
                {
                    File.Delete(virtualPath);
                    this.BindDorpDown();
                    this.BindData();
                    this.ShowMsg("删除模板成功", true);
                }
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            this.SaveAll();
            this.BindDorpDown();
            this.BindData();
            this.ShowMsg("批量保存分类模板成功", true);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.fileThame.HasFile)
            {
                if (!this.fileThame.PostedFile.FileName.EndsWith(".htm") && !this.fileThame.PostedFile.FileName.EndsWith(".html"))
                {
                    this.ShowMsg("请检查您上传文件的格式是否为html或htm", false);
                }
                else
                {
                    string virtualPath = HiContext.Current.GetSkinPath() + "/categorythemes/" + GetFilename(Path.GetFileName(this.fileThame.PostedFile.FileName), Path.GetExtension(this.fileThame.PostedFile.FileName));
                    this.fileThame.PostedFile.SaveAs(HiContext.Current.Context.Request.MapPath(virtualPath));
                    this.BindDorpDown();
                    this.BindData();
                    this.ShowMsg("上传成功", true);
                }
            }
            else
            {
                this.ShowMsg("上传失败！", false);
            }
        }

        private static string GetFilename(string filename, string extension)
        {
            return (filename.Substring(0, filename.IndexOf(".")) + extension);
        }

        protected IList<ManageThemeInfo> GetThemes()
        {
            HttpContext context = HiContext.Current.Context;
            IList<ManageThemeInfo> list = new List<ManageThemeInfo>();
            string path = context.Request.MapPath(HiContext.Current.GetSkinPath() + "/categorythemes");
            string[] strArray = Directory.Exists(path) ? Directory.GetFiles(path) : null;
            ManageThemeInfo item = null;
            foreach (string str2 in strArray)
            {
                if (str2.EndsWith(".html"))
                {
                    item = new ManageThemeInfo();
                    item.ThemeName = item.Name = Path.GetFileName(str2);
                    list.Add(item);
                }
            }
            return list;
        }

        private void grdCategries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Save")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int categoryId = (int) this.grdCategries.DataKeys[rowIndex].Value;
                DropdownColumn column = (DropdownColumn) this.grdCategries.Columns[1];
                string themeName = column.SelectedValues[rowIndex];
                if (CatalogHelper.SetCategoryThemes(categoryId, themeName))
                {
                    this.BindData();
                    this.ShowMsg("保存分类模板成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdCategries.RowCommand += new GridViewCommandEventHandler(this.grdCategries_RowCommand);
            this.btnUpload.Click += new EventHandler(this.btnUpload_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnSaveAll.Click += new EventHandler(this.btnSaveAll_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindDorpDown();
                this.BindData();
            }
        }

        private void SaveAll()
        {
            DropdownColumn column = (DropdownColumn) this.grdCategries.Columns[1];
            foreach (GridViewRow row in this.grdCategries.Rows)
            {
                string themeName = column.SelectedValues[row.RowIndex];
                int categoryId = (int) this.grdCategries.DataKeys[row.RowIndex].Value;
                CatalogHelper.SetCategoryThemes(categoryId, themeName);
            }
        }

        private bool validata(string theme)
        {
            DropdownColumn column = (DropdownColumn) this.grdCategries.Columns[1];
            foreach (GridViewRow row in this.grdCategries.Rows)
            {
                string str = column.SelectedValues[row.RowIndex];
                if (str == theme)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

