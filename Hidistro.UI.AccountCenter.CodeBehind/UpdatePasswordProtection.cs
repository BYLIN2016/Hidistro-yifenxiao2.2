namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class UpdatePasswordProtection : MemberTemplatedWebControl
    {
        private IButton btnOK3;
        private Literal litOldQuestion;
        private HtmlGenericControl LkUpdateTradePassword;
        private SmallStatusMessage StatusPasswordProtection;
        private HtmlTableRow tblrOldAnswer;
        private HtmlTableRow tblrOldQuestion;
        private TextBox txtAnswer;
        private TextBox txtOdeAnswer;
        private TextBox txtQuestion;

        protected override void AttachChildControls()
        {
            this.litOldQuestion = (Literal) this.FindControl("litOldQuestion");
            this.txtOdeAnswer = (TextBox) this.FindControl("txtOdeAnswer");
            this.txtQuestion = (TextBox) this.FindControl("txtQuestion");
            this.txtAnswer = (TextBox) this.FindControl("txtAnswer");
            this.LkUpdateTradePassword = (HtmlGenericControl) this.FindControl("one2");
            this.btnOK3 = ButtonManager.Create(this.FindControl("btnOK3"));
            this.StatusPasswordProtection = (SmallStatusMessage) this.FindControl("StatusPasswordProtection");
            this.tblrOldQuestion = (HtmlTableRow) this.FindControl("tblrOldQuestion");
            this.tblrOldAnswer = (HtmlTableRow) this.FindControl("tblrOldAnswer");
            PageTitle.AddSiteNameTitle("修改密码保护", HiContext.Current.Context);
            this.btnOK3.Click += new EventHandler(this.btnOK3_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindAnswerAndQuestion();
                Member user = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
                if (!user.IsOpenBalance)
                {
                    this.LkUpdateTradePassword.Visible = false;
                }
            }
        }

        private void BindAnswerAndQuestion()
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
            if (user != null)
            {
                this.tblrOldQuestion.Visible = this.tblrOldAnswer.Visible = !string.IsNullOrEmpty(user.PasswordQuestion);
                this.litOldQuestion.Text = user.PasswordQuestion;
            }
        }

        private void btnOK3_Click(object sender, EventArgs e)
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
            if ((user.MembershipUser != null) && user.MembershipUser.IsLockedOut)
            {
                this.ShowMessage(this.StatusPasswordProtection, "你已经被管理员锁定", false);
            }
            else if (string.IsNullOrEmpty(this.txtQuestion.Text) || string.IsNullOrEmpty(this.txtAnswer.Text))
            {
                this.ShowMessage(this.StatusPasswordProtection, "问题和答案为必填项", false);
            }
            else if (!string.IsNullOrEmpty(user.PasswordQuestion))
            {
                if (user.ChangePasswordQuestionAndAnswer(Globals.HtmlEncode(this.txtOdeAnswer.Text), Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text)))
                {
                    Users.ClearUserCache(user);
                    this.BindAnswerAndQuestion();
                    this.ShowMessage(this.StatusPasswordProtection, "成功修改了密码答案", true);
                }
                else
                {
                    this.ShowMessage(this.StatusPasswordProtection, "修改密码答案失败", false);
                }
            }
            else if (user.ChangePasswordQuestionAndAnswer(Globals.HtmlEncode(this.txtQuestion.Text), Globals.HtmlEncode(this.txtAnswer.Text)))
            {
                Users.ClearUserCache(user);
                this.BindAnswerAndQuestion();
                this.ShowMessage(this.StatusPasswordProtection, "成功修改了密码答案", true);
            }
            else
            {
                this.ShowMessage(this.StatusPasswordProtection, "修改密码答案失败", false);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UpdatePasswordProtection.html";
            }
            base.OnInit(e);
        }

        protected virtual void ShowMessage(SmallStatusMessage state, string msg, bool success)
        {
            if (state != null)
            {
                state.Success = success;
                state.Text = msg;
                state.Visible = true;
            }
        }
    }
}

