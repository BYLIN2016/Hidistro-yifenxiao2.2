namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditMyInnerMessageTemplet : DistributorPage
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
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
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
                if ((this.txtContent.Text.Trim().Length < 1) || (this.txtContent.Text.Trim().Length > 0xfa0))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息长度限制在4000个字符以内");
                    flag = false;
                }
                if (!flag)
                {
                    this.ShowMsg(msg, false);
                }
                else
                {
                    distributorMessageTemplate.InnerMessageSubject = this.txtMessageSubject.Text.Trim();
                    distributorMessageTemplate.InnerMessageBody = this.txtContent.Text;
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
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
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litEmailType.Text = distributorMessageTemplate.Name;
                        this.litTagDescription.Text = distributorMessageTemplate.TagDescription;
                        this.txtMessageSubject.Text = distributorMessageTemplate.InnerMessageSubject;
                        this.txtContent.Text = distributorMessageTemplate.InnerMessageBody;
                    }
                }
            }
        }
    }
}

