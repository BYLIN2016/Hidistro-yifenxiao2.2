namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
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

    public class EditMyCategory : DistributorPage
    {
        protected Button btnSaveCategory;
        private int categoryId;
        protected ProductTypeDownList dropProductTypes;
        protected KindeditorControl fckNotes1;
        protected KindeditorControl fckNotes2;
        protected KindeditorControl fckNotes3;
        protected HtmlGenericControl liRewriteName;
        protected TextBox txtCategoryName;
        protected TextBox txtPageDesc;
        protected TextBox txtPageKeyTitle;
        protected TextBox txtPageKeyWords;
        protected TextBox txtRewriteName;

        private void BindCategoryInfo(CategoryInfo categoryInfo)
        {
            if (categoryInfo != null)
            {
                this.txtCategoryName.Text = categoryInfo.Name;
                this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
                this.txtRewriteName.Text = categoryInfo.RewriteName;
                this.txtPageKeyTitle.Text = categoryInfo.MetaTitle;
                this.txtPageKeyWords.Text = categoryInfo.MetaKeywords;
                this.txtPageDesc.Text = categoryInfo.MetaDescription;
                this.fckNotes1.Text = categoryInfo.Notes1;
                this.fckNotes2.Text = categoryInfo.Notes2;
                this.fckNotes3.Text = categoryInfo.Notes3;
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = SubsiteCatalogHelper.GetCategory(this.categoryId);
            if (category == null)
            {
                this.ShowMsg("编缉商品分类错误,未知", false);
            }
            else
            {
                category.AssociatedProductType = this.dropProductTypes.SelectedValue;
                category.Name = this.txtCategoryName.Text;
                category.RewriteName = this.txtRewriteName.Text;
                category.MetaTitle = this.txtPageKeyTitle.Text;
                category.MetaKeywords = this.txtPageKeyWords.Text;
                category.MetaDescription = this.txtPageDesc.Text;
                category.Notes1 = this.fckNotes1.Text;
                category.Notes2 = this.fckNotes2.Text;
                category.Notes3 = this.fckNotes3.Text;
                if (category.Depth > 1)
                {
                    CategoryInfo info2 = SubsiteCatalogHelper.GetCategory(category.ParentCategoryId.Value);
                    if (string.IsNullOrEmpty(category.Notes1))
                    {
                        category.Notes1 = info2.Notes1;
                    }
                    if (string.IsNullOrEmpty(category.Notes2))
                    {
                        category.Notes2 = info2.Notes2;
                    }
                    if (string.IsNullOrEmpty(category.Notes3))
                    {
                        category.Notes3 = info2.Notes3;
                    }
                    if (string.IsNullOrEmpty(category.Notes4))
                    {
                        category.Notes4 = info2.Notes4;
                    }
                    if (string.IsNullOrEmpty(category.Notes5))
                    {
                        category.Notes5 = info2.Notes5;
                    }
                }
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
                else
                {
                    switch (SubsiteCatalogHelper.UpdateCategory(category))
                    {
                        case CategoryActionStatus.Success:
                            this.ShowMsg("成功修改了当前商品分类", true);
                            this.BindCategoryInfo(category);
                            return;

                        case CategoryActionStatus.UpdateParentError:
                            this.ShowMsg("不能自己成为自己的上级分类", false);
                            return;
                    }
                    this.ShowMsg("编缉商品分类错误,未知", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSaveCategory.Click += new EventHandler(this.btnSaveCategory_Click);
                if (!this.Page.IsPostBack)
                {
                    CategoryInfo category = SubsiteCatalogHelper.GetCategory(this.categoryId);
                    this.dropProductTypes.DataBind();
                    this.dropProductTypes.SelectedValue = category.AssociatedProductType;
                    if (category == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(category, false);
                        this.BindCategoryInfo(category);
                        if (category.Depth > 1)
                        {
                            this.liRewriteName.Style.Add("display", "none");
                        }
                    }
                }
            }
        }
    }
}

