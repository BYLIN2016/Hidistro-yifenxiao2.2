namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.MessageTemplets)]
    public class EditCellPhoneMessageTemplet : AdminPage
    {
        protected Button btnSaveCellPhoneMessageTemplet;
        protected Label litEmailType;
        protected Literal litTagDescription;
        private string messageType;
        protected TextBox txtContent;
        protected HtmlGenericControl txtContentTip;

        private void btnSaveCellPhoneMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
            if (messageTemplate != null)
            {
                if (string.IsNullOrEmpty(this.txtContent.Text))
                {
                    this.ShowMsg("短信内容不能为空", false);
                }
                else if ((this.txtContent.Text.Trim().Length < 1) || (this.txtContent.Text.Trim().Length > 300))
                {
                    this.ShowMsg("长度限制在1-300个字符之间", false);
                }
                else
                {
                    messageTemplate.SMSBody = this.txtContent.Text.Trim();
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
                    this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.messageType = this.Page.Request.QueryString["MessageType"];
            this.btnSaveCellPhoneMessageTemplet.Click += new EventHandler(this.btnSaveCellPhoneMessageTemplet_Click);
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
                        this.txtContent.Text = messageTemplate.SMSBody;
                    }
                }
            }
        }
    }
}

