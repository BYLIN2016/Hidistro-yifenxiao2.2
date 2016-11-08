namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ReplayMyReceivedMessageToAdmin : DistributorPage
    {
        protected Button btnReplyReplyReceivedMessages;
        protected DataList dtlistReceivedMessagesReplyed;
        protected Label litAddresser;
        protected Literal litContent;
        protected FormatedTimeLabel litDate;
        protected Literal litTitle;
        private long messageId;
        protected TextBox txtContes;
        protected HtmlGenericControl txtContesTip;
        protected TextBox txtReplyTitle;
        protected HtmlGenericControl txtReplyTitleTip;

        protected void btnReplyReplyReceivedMessages_Click(object sender, EventArgs e)
        {
            MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
            messageBoxInfo.Title = this.txtReplyTitle.Text;
            messageBoxInfo.Content = this.txtContes.Text.Trim();
            messageBoxInfo.Sernder = HiContext.Current.User.Username;
            messageBoxInfo.Accepter = "admin";
            if (SubsiteCommentsHelper.SendMessageToManager(messageBoxInfo))
            {
                this.ShowMsg("成功的回复了管理员的消息", true);
                this.txtReplyTitle.Text = string.Empty;
                this.txtContes.Text = string.Empty;
            }
            else
            {
                this.ShowMsg("回复管理员的消息失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(base.Request.QueryString["MessageId"], out this.messageId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyReplyReceivedMessages.Click += new EventHandler(this.btnReplyReplyReceivedMessages_Click);
                if (!this.Page.IsPostBack)
                {
                    SubsiteCommentsHelper.PostMessageIsRead(this.messageId);
                    MessageBoxInfo message = SubsiteCommentsHelper.GetMessage(this.messageId);
                    this.litTitle.Text = message.Title;
                    this.litContent.Text = message.Content;
                }
            }
        }
    }
}

