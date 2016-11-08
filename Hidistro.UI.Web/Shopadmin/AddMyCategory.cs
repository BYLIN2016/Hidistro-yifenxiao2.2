namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddMyCategory : DistributorPage
    {
        protected Button btnSaveAddCategory;
        protected Button btnSaveCategory;
        protected DistributorProductCategoriesDropDownList dropCategories;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckNotes1;
        protected KindeditorControl fckNotes2;
        protected KindeditorControl fckNotes3;
        protected HtmlGenericControl liURL;
        protected TextBox txtCategoryName;
        protected TextBox txtPageDesc;
        protected TextBox txtPageKeyTitle;
        protected TextBox txtPageKeyWords;
        protected TextBox txtRewriteName;

        private void btnSaveAddCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = this.GetCategory();
            if (category != null)
            {
                if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    this.ShowMsg("成功添加了商品分类", true);
                    this.dropCategories.DataBind();
                    this.dropProductTypes.DataBind();
                    this.txtCategoryName.Text = string.Empty;
                    this.txtRewriteName.Text = string.Empty;
                    this.txtPageKeyTitle.Text = string.Empty;
                    this.txtPageKeyWords.Text = string.Empty;
                    this.txtPageDesc.Text = string.Empty;
                    this.fckNotes1.Text = string.Empty;
                    this.fckNotes2.Text = string.Empty;
                    this.fckNotes3.Text = string.Empty;
                }
                else
                {
                    this.ShowMsg("添加商品分类失败,未知错误", false);
                }
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = this.GetCategory();
            if (category != null)
            {
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(category, new string[] { "ValCategory" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMsg(msg, false);
                }
                else if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    base.Response.Redirect("ManageMyCategories.aspx", true);
                }
                else
                {
                    this.ShowMsg("添加商品分类失败,未知错误", false);
                }
            }
        }

        private CategoryInfo GetCategory()
        {
            CategoryInfo target = new CategoryInfo();
            target.Name = this.txtCategoryName.Text.Trim();
            target.ParentCategoryId = this.dropCategories.SelectedValue;
            target.AssociatedProductType = this.dropProductTypes.SelectedValue;
            if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
            {
                target.RewriteName = this.txtRewriteName.Text.Trim();
            }
            else
            {
                target.RewriteName = null;
            }
            target.MetaTitle = this.txtPageKeyTitle.Text.Trim();
            target.MetaKeywords = this.txtPageKeyWords.Text.Trim();
            target.MetaDescription = this.txtPageDesc.Text.Trim();
            target.Notes1 = this.fckNotes1.Text;
            target.Notes2 = this.fckNotes2.Text;
            target.Notes3 = this.fckNotes3.Text;
            target.DisplaySequence = 0;
            if (target.ParentCategoryId.HasValue)
            {
                CategoryInfo category = SubsiteCatalogHelper.GetCategory(target.ParentCategoryId.Value);
                if ((category == null) || (category.Depth >= 5))
                {
                    this.ShowMsg(string.Format("您选择的上级分类有误，商品分类最多只支持{0}级分类", 5), false);
                    return null;
                }
                if (string.IsNullOrEmpty(target.Notes1))
                {
                    target.Notes1 = category.Notes1;
                }
                if (string.IsNullOrEmpty(target.Notes2))
                {
                    target.Notes2 = category.Notes2;
                }
                if (string.IsNullOrEmpty(target.Notes3))
                {
                    target.Notes3 = category.Notes3;
                }
                if (string.IsNullOrEmpty(target.RewriteName))
                {
                    target.RewriteName = category.RewriteName;
                }
            }
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(target, new string[] { "ValCategory" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                return target;
            }
            foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
            {
                msg = msg + Formatter.FormatErrorMessage(result.Message);
            }
            this.ShowMsg(msg, false);
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveCategory.Click += new EventHandler(this.btnSaveCategory_Click);
            this.btnSaveAddCategory.Click += new EventHandler(this.btnSaveAddCategory_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropProductTypes.DataBind();
            }
        }
    }
}

