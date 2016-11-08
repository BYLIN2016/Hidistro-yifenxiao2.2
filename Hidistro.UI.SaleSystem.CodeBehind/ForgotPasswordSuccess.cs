namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ForgotPasswordSuccess : HtmlTemplatedWebControl
    {
        private HtmlGenericControl htmDivAnswerMessage;
        private HtmlGenericControl htmDivEmailMessage;
        private Literal litEmail;
        private Literal litUserNameAnswer;
        private Literal litUserNameEmail;

        protected override void AttachChildControls()
        {
            this.htmDivEmailMessage = (HtmlGenericControl) this.FindControl("htmDivEmailMessage");
            this.litUserNameEmail = (Literal) this.FindControl("litUserNameEmail");
            this.litEmail = (Literal) this.FindControl("litEmail");
            this.htmDivAnswerMessage = (HtmlGenericControl) this.FindControl("htmDivAnswerMessage");
            this.litUserNameAnswer = (Literal) this.FindControl("litUserNameAnswer");
            string str = string.Empty;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                str = this.Page.Request.QueryString["UserName"];
            }
            string str2 = string.Empty;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Email"]))
            {
                str2 = this.Page.Request.QueryString["Email"];
            }
            PageTitle.AddSiteNameTitle("找回密码", HiContext.Current.Context);
            this.htmDivEmailMessage.Visible = false;
            this.htmDivAnswerMessage.Visible = false;
            if (!string.IsNullOrEmpty(str2))
            {
                this.htmDivEmailMessage.Visible = true;
                this.litUserNameEmail.Text = str;
                this.litEmail.Text = str2;
            }
            else if (!string.IsNullOrEmpty(str))
            {
                this.htmDivAnswerMessage.Visible = true;
                this.litUserNameAnswer.Text = str;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ForgotPasswordSuccess.html";
            }
            base.OnInit(e);
        }
    }
}

