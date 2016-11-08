namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MessageTemplets)]
    public class EditInnerMessageTemplet : AdminPage
    {
        protected Button btnSaveMessageTemplet;
        protected Label litEmailType;
        protected Literal litTagDescription;
        private string messageType;
        protected TextBox txtContent;
        protected HtmlGenericControl txtContentTip;
        protected TextBox txtMessageSubject;
        protected HtmlGenericControl txtMessageSubjectTip;

        private void btnSaveMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
            if (messageTemplate != null)
            {
                string msg = string.Empty;
                bool flag = true;
                if (string.IsNullOrEmpty(this.txtMessageSubject.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息标题不能为空");
                    flag = false;
                }
                if ((this.txtMessageSubject.Text.Trim().Length < 1) || (this.txtMessageSubject.Text.Trim().Length > 60))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息标题长度限制在1-60个字符之间");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this.txtContent.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息内容不能为空");
                    flag = false;
                }
                if ((this.txtContent.Text.Trim().Length < 1) || (this.txtContent.Text.Trim().Length > 300))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息长度限制在300个字符以内");
                    flag = false;
                }
                if (!flag)
                {
                    this.ShowMsg(msg, false);
                }
                else
                {
                    messageTemplate.InnerMessageSubject = this.txtMessageSubject.Text.Trim();
                    messageTemplate.InnerMessageBody = this.txtContent.Text;
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
                    this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.messageType = this.Page.Request.QueryString["MessageType"];
            this.btnSaveMessageTemplet.Click += new EventHandler(this.btnSaveMessageTemplet_Click);
            if (!base.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.messageType))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
                    if (messageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litEmailType.Text = messageTemplate.Name;
                        this.litTagDescription.Text = messageTemplate.TagDescription;
                        this.txtMessageSubject.Text = messageTemplate.InnerMessageSubject;
                        this.txtContent.Text = messageTemplate.InnerMessageBody;
                    }
                }
            }
        }
    }
}

