namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditPasswordProtection : DistributorPage
    {
        protected Button btnEditProtection;
        protected Literal litOldQuestion;
        protected TextBox txtNewAnswer;
        protected HtmlGenericControl txtNewAnswerTip;
        protected TextBox txtNewQuestion;
        protected HtmlGenericControl txtNewQuestionTip;
        protected TextBox txtOldAnswer;
        protected HtmlGenericControl ulOld;

        private void btnEditProtection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNewQuestion.Text))
            {
                this.ShowMsg("请输入新密保问题", false);
            }
            else if (string.IsNullOrEmpty(this.txtNewAnswer.Text))
            {
                this.ShowMsg("请输入新密保答案", false);
            }
            else
            {
                Distributor user = SubsiteStoreHelper.GetDistributor();
                if (string.IsNullOrEmpty(user.PasswordQuestion))
                {
                    if (user.ChangePasswordQuestionAndAnswer(this.txtNewQuestion.Text.Trim(), this.txtNewAnswer.Text.Trim()))
                    {
                        Users.ClearUserCache(user);
                        this.LoadOldControl();
                        this.ShowMsg("成功修改了密码答案", true);
                    }
                    else
                    {
                        this.ShowMsg("修改密码答案失败", false);
                    }
                }
                else if (user.ChangePasswordQuestionAndAnswer(this.txtOldAnswer.Text.Trim(), this.txtNewQuestion.Text.Trim(), this.txtNewAnswer.Text.Trim()))
                {
                    Users.ClearUserCache(user);
                    this.LoadOldControl();
                    this.ShowMsg("成功修改了密码答案", true);
                }
                else
                {
                    this.ShowMsg("修改密码答案失败，可能是您原来的问题答案输入错误", false);
                }
            }
        }

        private void LoadOldControl()
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
            if (user != null)
            {
                this.ulOld.Visible = !string.IsNullOrEmpty(user.PasswordQuestion);
                this.litOldQuestion.Text = user.PasswordQuestion;
                this.txtOldAnswer.Text = string.Empty;
                this.txtNewQuestion.Text = string.Empty;
                this.txtNewAnswer.Text = string.Empty;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.btnEditProtection.Click += new EventHandler(this.btnEditProtection_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadOldControl();
            }
        }
    }
}

