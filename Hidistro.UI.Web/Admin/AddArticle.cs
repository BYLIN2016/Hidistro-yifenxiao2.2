namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Articles)]
    public class AddArticle : AdminPage
    {
        protected Button btnAddArticle;
        protected HtmlInputCheckBox ckrrelease;
        protected ArticleCategoryDropDownList dropArticleCategory;
        protected KindeditorControl fcContent;
        protected FileUpload fileUpload;
        protected TextBox txtArticleTitle;
        protected TrimTextBox txtMetaDescription;
        protected TrimTextBox txtMetaKeywords;
        protected TextBox txtShortDesc;

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (this.fileUpload.HasFile)
            {
                try
                {
                    str = ArticleHelper.UploadArticleImage(this.fileUpload.PostedFile);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            ArticleInfo target = new ArticleInfo();
            if (this.dropArticleCategory.SelectedValue.HasValue)
            {
                target.CategoryId = this.dropArticleCategory.SelectedValue.Value;
                target.Title = this.txtArticleTitle.Text.Trim();
                target.MetaDescription = this.txtMetaDescription.Text.Trim();
                target.MetaKeywords = this.txtMetaKeywords.Text.Trim();
                target.IconUrl = str;
                target.Description = this.txtShortDesc.Text.Trim();
                target.Content = this.fcContent.Text;
                target.AddedDate = DateTime.Now;
                target.IsRelease = this.ckrrelease.Checked;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleInfo>(target, new string[] { "ValArticleInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMsg(msg, false);
                }
                else if (ArticleHelper.CreateArticle(target))
                {
                    this.txtArticleTitle.Text = string.Empty;
                    this.txtShortDesc.Text = string.Empty;
                    this.fcContent.Text = string.Empty;
                    this.ShowMsg("成功添加了一篇文章", true);
                }
                else
                {
                    this.ShowMsg("添加文章错误", false);
                }
            }
            else
            {
                this.ShowMsg("请选择文章分类", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddArticle.Click += new EventHandler(this.btnAddArticle_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropArticleCategory.DataBind();
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["categoryId"], out result);
                    this.dropArticleCategory.SelectedValue = new int?(result);
                }
            }
        }
    }
}

