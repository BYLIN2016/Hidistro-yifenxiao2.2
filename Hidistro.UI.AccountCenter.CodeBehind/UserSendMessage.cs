namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class UserSendMessage : MemberTemplatedWebControl
    {
        private IButton btnRefer;
        private RadioButton radioAdminSelect;
        private TextBox txtContent;
        private TextBox txtTitle;

        protected override void AttachChildControls()
        {
            this.radioAdminSelect = (RadioButton) this.FindControl("radioAdminSelect");
            this.txtTitle = (TextBox) this.FindControl("txtTitle");
            this.txtContent = (TextBox) this.FindControl("txtContent");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack)
            {
                this.radioAdminSelect.Enabled = false;
                this.radioAdminSelect.Checked = true;
                this.txtTitle.Text = this.txtTitle.Text.Trim();
                this.txtContent.Text = this.txtContent.Text.Trim();
            }
        }

        private void btnRefer_Click(object sender, EventArgs e)
        {
            string str = "";
            if (string.IsNullOrEmpty(this.txtTitle.Text) || (this.txtTitle.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtContent.Text) || (this.txtContent.Text.Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
            }
            else
            {
                MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
                messageBoxInfo.Sernder = HiContext.Current.User.Username;
                messageBoxInfo.Accepter = HiContext.Current.SiteSettings.IsDistributorSettings ? Users.GetUser(HiContext.Current.SiteSettings.UserId.Value).Username : "admin";
                messageBoxInfo.Title = Globals.HtmlEncode(this.txtTitle.Text.Replace("~", ""));
                messageBoxInfo.Content = Globals.HtmlEncode(this.txtContent.Text.Replace("~", ""));
                this.txtTitle.Text = string.Empty;
                this.txtContent.Text = string.Empty;
                if (CommentsHelper.SendMessage(messageBoxInfo))
                {
                    this.ShowMessage("发送信息成功", true);
                }
                else
                {
                    this.ShowMessage("发送信息失败", true);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserSendMessage.html";
            }
            base.OnInit(e);
        }
    }
}

