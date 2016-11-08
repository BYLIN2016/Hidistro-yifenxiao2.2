namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class ReplyReceivedMessage : MemberTemplatedWebControl
    {
        private Button btnReplyReceivedMessage;
        private Literal litAddresser;
        private Literal litContent;
        private FormatedTimeLabel litDate;
        private Literal litTitle;
        private long messageId;
        private TextBox txtReplyContent;
        private TextBox txtReplyTitle;

        protected override void AttachChildControls()
        {
            this.litAddresser = (Literal) this.FindControl("litAddresser");
            this.litTitle = (Literal) this.FindControl("litTitle");
            this.litDate = (FormatedTimeLabel) this.FindControl("litDate");
            this.litContent = (Literal) this.FindControl("litContent");
            this.txtReplyTitle = (TextBox) this.FindControl("txtReplyTitle");
            this.txtReplyContent = (TextBox) this.FindControl("txtReplyContent");
            this.btnReplyReceivedMessage = (Button) this.FindControl("btnReplyReceivedMessage");
            this.btnReplyReceivedMessage.Click += new EventHandler(this.btnReplyReceivedMessage_Click);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MessageId"]))
            {
                this.messageId = long.Parse(this.Page.Request.QueryString["MessageId"]);
            }
            if (!this.Page.IsPostBack)
            {
                CommentsHelper.PostMemberMessageIsRead(this.messageId);
                MessageBoxInfo memberMessage = CommentsHelper.GetMemberMessage(this.messageId);
                this.litAddresser.Text = "管理员";
                this.litTitle.Text = memberMessage.Title;
                this.litContent.Text = memberMessage.Content;
                this.litDate.Time = memberMessage.Date;
            }
        }

        private void btnReplyReceivedMessage_Click(object sender, EventArgs e)
        {
            string str = "";
            if (string.IsNullOrEmpty(this.txtReplyTitle.Text) || (this.txtReplyTitle.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtReplyContent.Text) || (this.txtReplyContent.Text.Length > 300))
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
                messageBoxInfo.Title = this.txtReplyTitle.Text.Trim();
                messageBoxInfo.Content = this.txtReplyContent.Text.Trim();
                if (CommentsHelper.SendMessage(messageBoxInfo))
                {
                    this.ShowMessage("回复成功", true);
                }
                else
                {
                    this.ShowMessage("回复失败", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-ReplyReceivedMessage.html";
            }
            base.OnInit(e);
        }
    }
}

