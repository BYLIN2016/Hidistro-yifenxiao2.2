namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ArticleCategories)]
    public class EditArticleCategory : AdminPage
    {
        protected ImageLinkButton btnPicDelete;
        protected Button btnSubmitArticleCategory;
        private int categoryId;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
        protected TextBox txtArticleCategoryiesDesc;
        protected TextBox txtArticleCategoryiesName;

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
            try
            {
                ResourcesHelper.DeleteImage(articleCategory.IconUrl);
            }
            catch
            {
            }
            articleCategory.IconUrl = (string) (this.imgPic.ImageUrl = null);
            if (ArticleHelper.UpdateArticleCategory(articleCategory))
            {
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            }
        }

        private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
        {
            ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
            if (articleCategory != null)
            {
                if (this.fileUpload.HasFile)
                {
                    try
                    {
                        ResourcesHelper.DeleteImage(articleCategory.IconUrl);
                        articleCategory.IconUrl = ArticleHelper.UploadArticleImage(this.fileUpload.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                articleCategory.Name = this.txtArticleCategoryiesName.Text.Trim();
                articleCategory.Description = this.txtArticleCategoryiesDesc.Text.Trim();
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleCategoryInfo>(articleCategory, new string[] { "ValArticleCategoryInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    this.UpdateCategory(articleCategory);
                }
                else
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMsg(msg, false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitArticleCategory.Click += new EventHandler(this.btnSubmitArticleCategory_Click);
            this.btnPicDelete.Click += new EventHandler(this.btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
                if (articleCategory == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(articleCategory, false);
                    this.txtArticleCategoryiesName.Text = articleCategory.Name;
                    this.txtArticleCategoryiesDesc.Text = articleCategory.Description;
                    this.imgPic.ImageUrl = articleCategory.IconUrl;
                    this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                    this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                }
            }
        }

        private void UpdateCategory(ArticleCategoryInfo category)
        {
            if (ArticleHelper.UpdateArticleCategory(category))
            {
                this.imgPic.ImageUrl = null;
                this.CloseWindow();
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
            this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
        }
    }
}

