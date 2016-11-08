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

    public class EditMyFriendlyLink : DistributorPage
    {
        protected ImageLinkButton btnPicDelete;
        protected Button btnSubmitLinks;
        protected HiImage imgPic;
        private int linkId;
        protected YesNoRadioButtonList radioShowLinks;
        protected TextBox txtaddLinkUrl;
        protected HtmlGenericControl txtaddLinkUrlTip;
        protected TextBox txtaddTitle;
        protected HtmlGenericControl txtaddTitleTip;
        protected FileUpload uploadImageUrl;

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            FriendlyLinksInfo friendlyLink = SubsiteStoreHelper.GetFriendlyLink(this.linkId);
            try
            {
                SubsiteStoreHelper.DeleteImage(friendlyLink.ImageUrl);
            }
            catch
            {
            }
            friendlyLink.ImageUrl = this.imgPic.ImageUrl = string.Empty;
            if (SubsiteStoreHelper.UpdateFriendlyLink(friendlyLink))
            {
                this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
            }
        }

        private void btnSubmitLinks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
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
            FriendlyLinksInfo friendlyLink = SubsiteStoreHelper.GetFriendlyLink(this.linkId);
            friendlyLink.ImageUrl = this.uploadImageUrl.HasFile ? str : friendlyLink.ImageUrl;
            friendlyLink.LinkUrl = this.txtaddLinkUrl.Text;
            friendlyLink.Title = Globals.HtmlEncode(this.txtaddTitle.Text.Trim());
            friendlyLink.Visible = this.radioShowLinks.SelectedValue;
            if (!string.IsNullOrEmpty(friendlyLink.ImageUrl) || !string.IsNullOrEmpty(friendlyLink.Title))
            {
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<FriendlyLinksInfo>(friendlyLink, new string[] { "ValFriendlyLinksInfo" });
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
                    this.UpdateFriendlyLink(friendlyLink);
                }
            }
            else
            {
                this.ShowMsg("友情链接Logo和网站名称不能同时为空", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitLinks.Click += new EventHandler(this.btnSubmitLinks_Click);
            this.btnPicDelete.Click += new EventHandler(this.btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["linkId"], out this.linkId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                FriendlyLinksInfo friendlyLink = SubsiteStoreHelper.GetFriendlyLink(this.linkId);
                if (friendlyLink == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.txtaddTitle.Text = Globals.HtmlDecode(friendlyLink.Title);
                    this.txtaddLinkUrl.Text = friendlyLink.LinkUrl;
                    this.radioShowLinks.SelectedValue = friendlyLink.Visible;
                    this.imgPic.ImageUrl = friendlyLink.ImageUrl;
                    this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                    this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
                }
            }
        }

        private void UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (SubsiteStoreHelper.UpdateFriendlyLink(friendlyLink))
            {
                this.imgPic.ImageUrl = string.Empty;
                this.ShowMsg("修改友情链接信息成功", true);
            }
            else
            {
                this.ShowMsg("修改友情链接信息失败", false);
            }
        }
    }
}

