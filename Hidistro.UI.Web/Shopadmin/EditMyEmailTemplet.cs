namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using kindeditor.Net;
    using System;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditMyEmailTemplet : DistributorPage
    {
        protected Button btnSaveEmailTemplet;
        private string emailType;
        protected KindeditorControl fcContent;
        protected Literal litEmailDescription;
        protected Label litEmailType;
        protected TextBox txtEmailSubject;
        protected HtmlGenericControl txtEmailSubjectTip;

        private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.emailType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
            {
                string msg = string.Empty;
                bool flag = true;
                if (string.IsNullOrEmpty(this.txtEmailSubject.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件标题不能为空");
                    flag = false;
                }
                else if ((this.txtEmailSubject.Text.Trim().Length < 1) || (this.txtEmailSubject.Text.Trim().Length > 60))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件标题长度限制在1-60个字符之间");
                    flag = false;
                }
                if (string.IsNullOrEmpty(this.fcContent.Text) || (this.fcContent.Text.Trim().Length == 0))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件内容不能为空");
                    flag = false;
                }
                if (!flag)
                {
                    this.ShowMsg(msg, false);
                }
                else
                {
                    string text = this.fcContent.Text;
                    Regex regex = new Regex("<img\\b[^>]*?\\bsrc[\\s]*=[\\s]*[\"']?[\\s]*(?<imgUrl>[^\"'>]*)[^>]*?/?[\\s]*>", RegexOptions.IgnoreCase);
                    foreach (Match match in regex.Matches(text))
                    {
                        string oldValue = match.Groups["imgUrl"].Value;
                        if (oldValue.StartsWith("/"))
                        {
                            text = text.Replace(oldValue, string.Format("http://{0}{1}", base.Request.Url.Host, oldValue));
                        }
                    }
                    distributorMessageTemplate.EmailBody = text;
                    distributorMessageTemplate.EmailSubject = this.txtEmailSubject.Text.Trim();
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveEmailTemplet.Click += new EventHandler(this.btnSaveEmailTemplet_Click);
            this.emailType = this.Page.Request.QueryString["MessageType"];
            if (!this.Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.emailType))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.emailType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litEmailType.Text = distributorMessageTemplate.Name;
                        this.litEmailDescription.Text = distributorMessageTemplate.TagDescription;
                        this.txtEmailSubject.Text = distributorMessageTemplate.EmailSubject;
                        this.fcContent.Text = distributorMessageTemplate.EmailBody;
                    }
                }
            }
        }
    }
}

