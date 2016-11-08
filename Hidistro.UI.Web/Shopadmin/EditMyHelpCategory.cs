namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class EditMyHelpCategory : DistributorPage
    {
        protected ImageLinkButton btnPicDelete;
        protected Button btnSubmitHelpCategory;
        private int categoryId;
        protected FileUpload fileUpload;
        protected HiImage imgPic;
        protected YesNoRadioButtonList radioShowFooter;
        protected TextBox txtHelpCategoryDesc;
        protected TextBox txtHelpCategoryName;

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            HelpCategoryInfo helpCategory = SubsiteCommentsHelper.GetHelpCategory(this.categoryId);
            try
            {
                ResourcesHelper.DeleteImage(helpCategory.IconUrl);
            }
            catch
            {
            }
            helpCategory.IconUrl = this.imgPic.ImageUrl = string.Empty;
            if (SubsiteCommentsHelper.UpdateHelpCategory(helpCategory))
            {
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            }
        }

        private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
        {
            string iconUrl = string.Empty;
            iconUrl = SubsiteCommentsHelper.GetHelpCategory(this.categoryId).IconUrl;
            if (this.fileUpload.HasFile)
            {
                try
                {
                    if (!string.IsNullOrEmpty(iconUrl))
                    {
                        ResourcesHelper.DeleteImage(iconUrl);
                    }
                    iconUrl = SubsiteCommentsHelper.UploadHelpImage(this.fileUpload.PostedFile);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            HelpCategoryInfo target = new HelpCategoryInfo();
            target.CategoryId = new int?(this.categoryId);
            target.Name = this.txtHelpCategoryName.Text.Trim();
            target.IconUrl = iconUrl;
            target.Description = this.txtHelpCategoryDesc.Text.Trim();
            target.IsShowFooter = this.radioShowFooter.SelectedValue;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpCategoryInfo>(target, new string[] { "ValHelpCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                this.UpdateCategory(target);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitHelpCategory.Click += new EventHandler(this.btnSubmitHelpCategory_Click);
            this.btnPicDelete.Click += new EventHandler(this.btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                HelpCategoryInfo helpCategory = SubsiteCommentsHelper.GetHelpCategory(this.categoryId);
                if (helpCategory == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(helpCategory, false);
                    this.txtHelpCategoryName.Text = helpCategory.Name;
                    this.txtHelpCategoryDesc.Text = helpCategory.Description;
                    this.radioShowFooter.SelectedValue = helpCategory.IsShowFooter;
                    this.imgPic.ImageUrl = helpCategory.IconUrl;
                    this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                    this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                }
            }
        }

        private void UpdateCategory(HelpCategoryInfo category)
        {
            if (SubsiteCommentsHelper.UpdateHelpCategory(category))
            {
                this.imgPic.ImageUrl = null;
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.ShowMsg("成功修改了帮助分类", true);
            }
            else
            {
                this.ShowMsg("操作失败,未知错误", false);
            }
        }
    }
}

