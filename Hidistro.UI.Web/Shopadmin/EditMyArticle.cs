namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditMyArticle : DistributorPage
    {
        private int articleId;
        protected Button btnAddArticle;
        protected ImageLinkButton btnPicDelete;
        protected HtmlInputCheckBox ckrrelease;
        protected DistributorArticleCategoryDropDownList dropArticleCategory;
        protected KindeditorControl fcContent;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
        protected TextBox txtArticleTitle;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected TextBox txtShortDesc;

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            if (!this.dropArticleCategory.SelectedValue.HasValue)
            {
                this.ShowMsg("请选择文章分类", false);
            }
            else
            {
                ArticleInfo article = SubsiteCommentsHelper.GetArticle(this.articleId);
                if (this.fileUpload.HasFile)
                {
                    try
                    {
                        ResourcesHelper.DeleteImage(article.IconUrl);
                        article.IconUrl = SubsiteCommentsHelper.UploadArticleImage(this.fileUpload.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                article.ArticleId = this.articleId;
                article.CategoryId = this.dropArticleCategory.SelectedValue.Value;
                article.Title = this.txtArticleTitle.Text.Trim();
                article.MetaDescription = this.txtMetaDescription.Text.Trim();
                article.MetaKeywords = this.txtMetaKeywords.Text.Trim();
                article.Description = this.txtShortDesc.Text.Trim();
                article.Content = this.fcContent.Text;
                article.AddedDate = DateTime.Now;
                article.IsRelease = this.ckrrelease.Checked;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleInfo>(article, new string[] { "ValArticleInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    if (SubsiteCommentsHelper.UpdateArticle(article))
                    {
                        this.ShowMsg("已经成功修改当前文章", true);
                    }
                    else
                    {
                        this.ShowMsg("修改文章失败", false);
                    }
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

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            ArticleInfo article = SubsiteCommentsHelper.GetArticle(this.articleId);
            try
            {
                ResourcesHelper.DeleteImage(article.IconUrl);
            }
            catch
            {
            }
            article.IconUrl = (string) (this.imgPic.ImageUrl = null);
            if (SubsiteCommentsHelper.UpdateArticle(article))
            {
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnAddArticle.Click += new EventHandler(this.btnAddArticle_Click);
                this.btnPicDelete.Click += new EventHandler(this.btnPicDelete_Click);
                if (!this.Page.IsPostBack)
                {
                    this.dropArticleCategory.DataBind();
                    this.SetControlValue();
                }
            }
        }

        private void SetControlValue()
        {
            ArticleInfo article = SubsiteCommentsHelper.GetArticle(this.articleId);
            if (article == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(article, false);
                this.txtArticleTitle.Text = article.Title;
                this.txtMetaDescription.Text = article.MetaDescription;
                this.txtMetaKeywords.Text = article.MetaKeywords;
                this.imgPic.ImageUrl = article.IconUrl;
                this.txtShortDesc.Text = article.Description;
                this.fcContent.Text = article.Content;
                this.dropArticleCategory.SelectedValue = new int?(article.CategoryId);
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.ckrrelease.Checked = article.IsRelease;
            }
        }
    }
}

