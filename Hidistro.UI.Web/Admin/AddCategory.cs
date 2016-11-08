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
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddProductCategory)]
    public class AddCategory : AdminPage
    {
        protected Button btnSaveAddCategory;
        protected Button btnSaveCategory;
        protected ProductCategoriesDropDownList dropCategories;
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
        protected TextBox txtSKUPrefix;

        private void btnSaveAddCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = this.GetCategory();
            if (category != null)
            {
                if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    this.ShowMsg("成功添加了商品分类", true);
                    this.dropCategories.DataBind();
                    this.dropProductTypes.DataBind();
                    this.txtCategoryName.Text = string.Empty;
                    this.txtSKUPrefix.Text = string.Empty;
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
                if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
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
            target.SKUPrefix = this.txtSKUPrefix.Text.Trim();
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
                CategoryInfo category = CatalogHelper.GetCategory(target.ParentCategoryId.Value);
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
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int result = 0;
                int.TryParse(base.Request["parentCategoryId"], out result);
                CategoryInfo category = CatalogHelper.GetCategory(result);
                if (category != null)
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{ ");
                    base.Response.Write(string.Format("\"SKUPrefix\":\"{0}\"", category.SKUPrefix));
                    base.Response.Write("}");
                    base.Response.End();
                }
            }
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropProductTypes.DataBind();
            }
        }
    }
}

