namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductReviewsManage)]
    public class ReplyReceivedMessages : AdminPage
    {
        protected Button btnReplyReplyReceivedMessages;
        protected DataList dtlistMessageReply;
        protected Label litAddresser;
        protected Literal litContent;
        protected FormatedTimeLabel litDate;
        protected Literal litTitle;
        private long messageId;
        protected TextBox txtContes;
        protected HtmlGenericControl txtContesTip;
        protected TextBox txtTitle;
        protected HtmlGenericControl txtTitleTip;

        protected void btnReplyReplyReceivedMessages_Click(object sender, EventArgs e)
        {
            IList<MessageBoxInfo> messageBoxInfos = new List<MessageBoxInfo>();
            MessageBoxInfo item = new MessageBoxInfo();
            item.Accepter = (string) this.ViewState["Sernder"];
            item.Sernder = "admin";
            item.Title = this.txtTitle.Text.Trim();
            item.Content = this.txtContes.Text.Trim();
            messageBoxInfos.Add(item);
            if (NoticeHelper.SendMessageToMember(messageBoxInfos) > 0)
            {
                this.ShowMsg("成功回复了会员的站内信.", true);
            }
            else
            {
                this.ShowMsg("回复会员的站内信失败.", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["MessageId"], out this.messageId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyReplyReceivedMessages.Click += new EventHandler(this.btnReplyReplyReceivedMessages_Click);
                if (!this.Page.IsPostBack)
                {
                    NoticeHelper.PostManagerMessageIsRead(this.messageId);
                    MessageBoxInfo managerMessage = NoticeHelper.GetManagerMessage(this.messageId);
                    this.litTitle.Text = managerMessage.Title;
                    this.litContent.Text = managerMessage.Content;
                    this.ViewState["Sernder"] = managerMessage.Sernder;
                }
            }
        }
    }
}

