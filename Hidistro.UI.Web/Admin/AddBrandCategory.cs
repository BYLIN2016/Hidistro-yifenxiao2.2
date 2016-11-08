namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.BrandCategories)]
    public class AddBrandCategory : AdminPage
    {
        protected Button btnAddBrandCategory;
        protected Button btnSave;
        protected ProductTypesCheckBoxList chlistProductTypes;
        protected KindeditorControl fckDescription;
        protected FileUpload fileUpload;
        protected TextBox txtBrandName;
        protected TextBox txtCompanyUrl;
        protected TextBox txtkeyword;
        protected TextBox txtMetaDescription;
        protected TextBox txtReUrl;

        protected void btnAddBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (this.fileUpload.HasFile)
            {
                try
                {
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
                    if (this.ValidationBrandCategory(brandCategoryInfo))
                    {
                        if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
                        {
                            this.ShowMsg("成功添加品牌分类", true);
                        }
                        else
                        {
                            this.ShowMsg("添加品牌分类失败", true);
                        }
                    }
                    return;
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            this.ShowMsg("请上传一张品牌logo图片", false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
            if (this.fileUpload.HasFile)
            {
                try
                {
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
                    if (this.ValidationBrandCategory(brandCategoryInfo))
                    {
                        if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
                        {
                            base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
                        }
                        else
                        {
                            this.ShowMsg("添加品牌分类失败", true);
                        }
                    }
                    return;
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            this.ShowMsg("请上传一张品牌logo图片", false);
        }

        private BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            info.BrandName = Globals.HtmlEncode(this.txtBrandName.Text.Trim());
            if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
            {
                info.CompanyUrl = this.txtCompanyUrl.Text.Trim();
            }
            else
            {
                info.CompanyUrl = null;
            }
            info.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
            info.MetaKeywords = Globals.HtmlEncode(this.txtkeyword.Text.Trim());
            info.MetaDescription = Globals.HtmlEncode(this.txtMetaDescription.Text.Trim());
            IList<int> list = new List<int>();
            foreach (ListItem item in this.chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            info.Description = (!string.IsNullOrEmpty(this.fckDescription.Text) && (this.fckDescription.Text.Length > 0)) ? this.fckDescription.Text : null;
            return info;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            this.btnAddBrandCategory.Click += new EventHandler(this.btnAddBrandCategory_Click);
            if (!base.IsPostBack)
            {
                this.chlistProductTypes.DataBind();
            }
        }

        private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<BrandCategoryInfo>(brandCategory, new string[] { "ValBrandCategory" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

