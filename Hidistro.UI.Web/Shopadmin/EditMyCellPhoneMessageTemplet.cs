namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditMyCellPhoneMessageTemplet : DistributorPage
    {
        protected Button btnSaveCellPhoneMessageTemplet;
        protected Label litEmailType;
        protected Literal litTagDescription;
        private string messageType;
        protected TextBox txtContent;
        protected HtmlGenericControl txtContentTip;

        private void btnSaveCellPhoneMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
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
                    distributorMessageTemplate.SMSBody = this.txtContent.Text.Trim();
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
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
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litEmailType.Text = distributorMessageTemplate.Name;
                        this.litTagDescription.Text = distributorMessageTemplate.TagDescription;
                        this.txtContent.Text = distributorMessageTemplate.SMSBody;
                    }
                }
            }
        }
    }
}

