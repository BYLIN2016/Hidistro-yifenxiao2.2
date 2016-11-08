namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddMyFriendlyLink : DistributorPage
    {
        protected Button btnSubmitLinks;
        protected YesNoRadioButtonList radioShowLinks;
        protected TextBox txtaddLinkUrl;
        protected HtmlGenericControl txtaddLinkUrlTip;
        protected TextBox txtaddTitle;
        protected HtmlGenericControl txtaddTitleTip;
        protected FileUpload uploadImageUrl;

        private void AddNewFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (SubsiteStoreHelper.CreateFriendlyLink(friendlyLink))
            {
                this.ShowMsg("成功添加了一个友情链接", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        private void btnSubmitLinks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (!this.uploadImageUrl.HasFile && string.IsNullOrEmpty(this.txtaddTitle.Text.Trim()))
            {
                this.ShowMsg("友情链接Logo和网站名称不能同时为空", false);
            }
            else
            {
                FriendlyLinksInfo target = new FriendlyLinksInfo();
                if (this.uploadImageUrl.HasFile)
                {
                    try
                    {
                        str = SubsiteStoreHelper.UploadLinkImage(this.uploadImageUrl.PostedFile);
                    }
                    catch
                    {
                        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                target.ImageUrl = str;
                target.LinkUrl = this.txtaddLinkUrl.Text;
                target.Title = Globals.HtmlEncode(this.txtaddTitle.Text.Trim());
                target.Visible = this.radioShowLinks.SelectedValue;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<FriendlyLinksInfo>(target, new string[] { "ValFriendlyLinksInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    this.AddNewFriendlyLink(target);
                    this.Reset();
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
            this.btnSubmitLinks.Click += new EventHandler(this.btnSubmitLinks_Click);
        }

        private void Reset()
        {
            this.txtaddTitle.Text = string.Empty;
            this.radioShowLinks.SelectedValue = true;
            this.txtaddLinkUrl.Text = string.Empty;
        }
    }
}

