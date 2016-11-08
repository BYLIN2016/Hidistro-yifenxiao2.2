namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text.RegularExpressions;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ForgotPassword : HtmlTemplatedWebControl
    {
        private IButton btnCheckAnswer;
        private IButton btnCheckEmail;
        private IButton btnCheckMobile;
        private IButton btnCheckUserName;
        private IButton btnPrev;
        private IButton btnPrev2;
        private IButton btnPrev3;
        private IButton btnSendEmail;
        private IButton btnSendMobile;
        private IButton btnSetPassword;
        private DropDownList dropType;
        private static string emailCode = "";
        private HtmlGenericControl htmDivCellPhone;
        private HtmlGenericControl htmDivEmail;
        private HtmlGenericControl htmDivPassword;
        private HtmlGenericControl htmDivQuestionAndAnswer;
        private HtmlGenericControl htmDivUserName;
        private Literal litAnswerMessage;
        private Literal litEmail;
        private Literal litMobile;
        private Literal litUserQuestion;
        private static string mobileCode = "";
        private TextBox txtEmailValid;
        private TextBox txtMobileValid;
        private TextBox txtPassword;
        private TextBox txtRePassword;
        private TextBox txtUserAnswer;
        private TextBox txtUserName;

        protected override void AttachChildControls()
        {
            this.htmDivUserName = (HtmlGenericControl) this.FindControl("htmDivUserName");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.btnCheckUserName = ButtonManager.Create(this.FindControl("btnCheckUserName"));
            this.btnCheckMobile = ButtonManager.Create(this.FindControl("btnCheckMobile"));
            this.btnCheckEmail = ButtonManager.Create(this.FindControl("btnCheckEmail"));
            this.htmDivQuestionAndAnswer = (HtmlGenericControl) this.FindControl("htmDivQuestionAndAnswer");
            this.litUserQuestion = (Literal) this.FindControl("litUserQuestion");
            this.litMobile = (Literal) this.FindControl("litMobile");
            this.litEmail = (Literal) this.FindControl("litEmail");
            this.txtUserAnswer = (TextBox) this.FindControl("txtUserAnswer");
            this.txtMobileValid = (TextBox) this.FindControl("txtMobileValid");
            this.txtEmailValid = (TextBox) this.FindControl("txtEmailValid");
            this.litAnswerMessage = (Literal) this.FindControl("litAnswerMessage");
            this.btnCheckAnswer = ButtonManager.Create(this.FindControl("btnCheckAnswer"));
            this.btnSendMobile = ButtonManager.Create(this.FindControl("btnSendMobile"));
            this.btnSendMobile.Click += new EventHandler(this.btnSendMobile_Click);
            this.btnSendEmail = ButtonManager.Create(this.FindControl("btnSendEmail"));
            this.btnSendEmail.Click += new EventHandler(this.btnSendEmail_Click);
            this.btnPrev = ButtonManager.Create(this.FindControl("btnPrev"));
            this.btnPrev2 = ButtonManager.Create(this.FindControl("btnPrev2"));
            this.btnPrev3 = ButtonManager.Create(this.FindControl("btnPrev3"));
            this.btnPrev.Click += new EventHandler(this.btnPrev_Click);
            this.btnPrev2.Click += new EventHandler(this.btnPrev_Click);
            this.btnPrev3.Click += new EventHandler(this.btnPrev_Click);
            this.htmDivPassword = (HtmlGenericControl) this.FindControl("htmDivPassword");
            this.htmDivCellPhone = (HtmlGenericControl) this.FindControl("htmDivCellPhone");
            this.htmDivEmail = (HtmlGenericControl) this.FindControl("htmDivEmail");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.txtRePassword = (TextBox) this.FindControl("txtRePassword");
            this.dropType = (DropDownList) this.FindControl("dropType");
            this.btnSetPassword = ButtonManager.Create(this.FindControl("btnSetPassword"));
            PageTitle.AddSiteNameTitle("找回密码", HiContext.Current.Context);
            this.btnCheckUserName.Click += new EventHandler(this.btnCheckUserName_Click);
            this.btnCheckMobile.Click += new EventHandler(this.btnCheckMobile_Click);
            this.btnCheckEmail.Click += new EventHandler(this.btnCheckEmail_Click);
            this.btnCheckAnswer.Click += new EventHandler(this.btnCheckAnswer_Click);
            this.btnSetPassword.Click += new EventHandler(this.btnSetPassword_Click);
            if (!this.Page.IsPostBack)
            {
                this.LoadType();
                this.panelShow("InputUserName");
                mobileCode = "";
                emailCode = "";
            }
        }

        private void btnCheckAnswer_Click(object sender, EventArgs e)
        {
            if (Users.FindUserByUsername(this.txtUserName.Text.Trim()).MembershipUser.ValidatePasswordAnswer(this.txtUserAnswer.Text.Trim()))
            {
                this.panelShow("InputPassword");
            }
            else
            {
                this.litAnswerMessage.Visible = true;
            }
        }

        private void btnCheckEmail_Click(object sender, EventArgs e)
        {
            if (this.txtEmailValid.Text.Length == 0)
            {
                this.ShowMessage("请输入验证码", false);
            }
            else if (!this.txtEmailValid.Text.ToLower().Equals(emailCode.ToLower()))
            {
                this.ShowMessage("验证码输入错误，请重新输入", false);
            }
            else
            {
                this.panelShow("InputPassword");
                emailCode = "";
            }
        }

        private void btnCheckMobile_Click(object sender, EventArgs e)
        {
            if (this.txtMobileValid.Text.Length == 0)
            {
                this.ShowMessage("请输入验证码", false);
            }
            else if (!this.txtMobileValid.Text.ToLower().Equals(mobileCode.ToLower()))
            {
                this.ShowMessage("验证码输入错误，请重新输入", false);
            }
            else
            {
                this.panelShow("InputPassword");
                mobileCode = "";
            }
        }

        private void btnCheckUserName_Click(object sender, EventArgs e)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z0-9]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if ((!regex.IsMatch(this.txtUserName.Text.Trim()) || (this.txtUserName.Text.Trim().Length < 2)) || (this.txtUserName.Text.Trim().Length > 20))
            {
                this.ShowMessage("用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间", false);
                return;
            }
            if (this.txtUserName.Text.Contains(","))
            {
                this.ShowMessage("用户名不能包含逗号", false);
                return;
            }
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            if (((user == null) || (user.UserRole == UserRole.SiteManager)) || (user.UserRole == UserRole.Anonymous))
            {
                this.ShowMessage("该用户不存在", false);
                return;
            }
            IUser user2 = Users.GetUser(0, this.txtUserName.Text.Trim(), false, true);
            if (user.UserRole == UserRole.Distributor)
            {
                if (HiContext.Current.SiteSettings.IsDistributorSettings && (user2.UserId != HiContext.Current.SiteSettings.UserId.Value))
                {
                    this.ShowMessage("分销商只能在自己的站点或主站上登录", false);
                    return;
                }
            }
            else
            {
                if (HiContext.Current.SiteSettings.IsDistributorSettings)
                {
                    Member member = user2 as Member;
                    if (member.ParentUserId.HasValue)
                    {
                        if (member.ParentUserId.Value == HiContext.Current.SiteSettings.UserId)
                        {
                            goto Label_01CE;
                        }
                    }
                    this.ShowMessage("您不是本站会员，请您进行注册", false);
                    return;
                }
                Member member2 = user2 as Member;
                if (member2.ParentUserId.HasValue && (member2.ParentUserId.Value != 0))
                {
                    this.ShowMessage("您不是本站会员，请您进行注册", false);
                    return;
                }
            }
        Label_01CE:
            if (this.dropType.SelectedIndex == 0)
            {
                if (!string.IsNullOrEmpty(user.PasswordQuestion))
                {
                    if (this.litUserQuestion != null)
                    {
                        this.litUserQuestion.Text = user.PasswordQuestion.ToString();
                    }
                    this.panelShow("InputAnswer");
                    return;
                }
                this.ShowMessage("您没有设置密保问题，无法找回密码，请自行联系管理员修改密码", false);
            }
            else if (this.dropType.SelectedIndex == 1)
            {
                if (user is Member)
                {
                    Member member3 = user as Member;
                    if (!string.IsNullOrEmpty(member3.CellPhone))
                    {
                        if (this.litMobile != null)
                        {
                            this.litMobile.Text = member3.CellPhone.Substring(0, 3) + "****" + member3.CellPhone.Substring(7);
                        }
                        this.panelShow("CellPhone");
                        return;
                    }
                    this.ShowMessage("没有设置手机号码", false);
                    return;
                }
                if (user is Distributor)
                {
                    Distributor distributor = user as Distributor;
                    if (!string.IsNullOrEmpty(distributor.CellPhone))
                    {
                        if (this.litMobile != null)
                        {
                            this.litMobile.Text = distributor.CellPhone.Substring(0, 3) + "****" + distributor.CellPhone.Substring(7);
                        }
                        this.panelShow("CellPhone");
                    }
                    else
                    {
                        this.ShowMessage("没有设置手机号码", false);
                    }
                }
            }
            else if (this.dropType.SelectedIndex == 2)
            {
                if (!string.IsNullOrEmpty(user.Email))
                {
                    if (this.litEmail != null)
                    {
                        this.litEmail.Text = user.Email;
                    }
                    this.panelShow("Email");
                }
                else
                {
                    this.ShowMessage("没有设置电子邮箱", false);
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.LoadType();
            this.panelShow("InputUserName");
            mobileCode = "";
            emailCode = "";
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            string str;
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            emailCode = HiContext.Current.CreateVerifyCode(6);
            string body = string.Format("亲爱的{0}：<br>您好！感谢您使用{1}。<br>您正在进行账户基础信息维护，请在校验码输入框中输入：{2}，以完成操作。 <br>注意：此操作可能会修改您的密码、登录邮箱或绑定手机。如非本人操作，请及时登录并修改密码以保证账户安全。（工作人员不会向您索取此校验码，请勿泄漏！） ", user.Username, siteSettings.SiteName, emailCode);
            SendStatus status = Messenger.SendMail("验证码", body, user.Email, siteSettings, out str);
            if ((status == SendStatus.NoProvider) || (status == SendStatus.ConfigError))
            {
                this.ShowMessage("后台设置错误，请自行联系后台管理员", false);
            }
            else
            {
                switch (status)
                {
                    case SendStatus.Fail:
                        this.ShowMessage("发送失败", false);
                        return;

                    case SendStatus.Success:
                        this.ShowMessage("发送成功", true);
                        break;
                }
            }
        }

        private void btnSendMobile_Click(object sender, EventArgs e)
        {
            string str;
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            if (user is Member)
            {
                Member member = user as Member;
                mobileCode = HiContext.Current.CreateVerifyCode(6);
                switch (Messenger.SendSMS(member.CellPhone, "您本次的验证码是：" + mobileCode, siteSettings, out str))
                {
                    case SendStatus.NoProvider:
                    case SendStatus.ConfigError:
                        this.ShowMessage("后台设置错误，请自行联系后台管理员", false);
                        return;

                    case SendStatus.Fail:
                        this.ShowMessage("发送失败", false);
                        return;

                    case SendStatus.Success:
                        this.ShowMessage("发送成功", true);
                        return;
                }
            }
            else if (user is Distributor)
            {
                Distributor distributor = user as Distributor;
                mobileCode = HiContext.Current.CreateVerifyCode(6);
                switch (Messenger.SendSMS(distributor.CellPhone, "您本次的验证码是：" + mobileCode, siteSettings, out str))
                {
                    case SendStatus.NoProvider:
                    case SendStatus.ConfigError:
                        this.ShowMessage("后台设置错误，请自行联系后台管理员", false);
                        return;

                    case SendStatus.Fail:
                        this.ShowMessage("发送失败", false);
                        return;

                    case SendStatus.Success:
                        this.ShowMessage("发送成功", true);
                        return;
                }
            }
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            bool flag = false;
            if (string.IsNullOrEmpty(this.txtPassword.Text.Trim()) || string.IsNullOrEmpty(this.txtRePassword.Text.Trim()))
            {
                this.ShowMessage("密码不允许为空！", false);
            }
            else if (this.txtPassword.Text.Trim() != this.txtRePassword.Text.Trim())
            {
                this.ShowMessage("两次输入的密码需一致", false);
            }
            else if ((this.txtPassword.Text.Length < Membership.Provider.MinRequiredPasswordLength) || (this.txtPassword.Text.Length > HiConfiguration.GetConfig().PasswordMaxLength))
            {
                this.ShowMessage(string.Format("密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, HiConfiguration.GetConfig().PasswordMaxLength), false);
            }
            else
            {
                if (this.dropType.SelectedIndex == 0)
                {
                    flag = user.ChangePasswordWithAnswer(this.txtUserAnswer.Text, this.txtPassword.Text);
                }
                else if (user is Member)
                {
                    flag = (user as Member).ChangePasswordWithoutAnswer(this.txtPassword.Text);
                }
                else if (user is Distributor)
                {
                    flag = (user as Distributor).ChangePasswordWithoutAnswer(this.txtPassword.Text);
                }
                else
                {
                    flag = user.ChangePassword(this.txtPassword.Text);
                }
                if (flag)
                {
                    Messenger.UserPasswordForgotten(user, this.txtPassword.Text);
                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("ForgotPasswordSuccess") + string.Format("?UserName={0}", user.Username));
                }
                else
                {
                    this.ShowMessage("登录密码修改失败，请重试", false);
                }
            }
        }

        private void LoadType()
        {
            this.dropType.Items.Clear();
            this.dropType.Items.Add(new ListItem("通过密保问题", "0"));
            this.dropType.Items.Add(new ListItem("通过手机号码", "1"));
            this.dropType.Items.Add(new ListItem("通过电子邮箱", "2"));
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ForgotPassword.html";
            }
            base.OnInit(e);
        }

        private void panelShow(string type)
        {
            this.litAnswerMessage.Visible = false;
            if (type == "InputUserName")
            {
                this.htmDivUserName.Visible = true;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = false;
                this.htmDivCellPhone.Visible = false;
                this.htmDivEmail.Visible = false;
            }
            else if (type == "CellPhone")
            {
                this.htmDivCellPhone.Visible = true;
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = false;
                this.htmDivEmail.Visible = false;
            }
            else if (type == "Email")
            {
                this.htmDivEmail.Visible = true;
                this.htmDivCellPhone.Visible = false;
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = false;
            }
            else if (type == "InputAnswer")
            {
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = true;
                this.htmDivPassword.Visible = false;
                this.htmDivCellPhone.Visible = false;
                this.htmDivEmail.Visible = false;
            }
            else if (type == "InputPassword")
            {
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = true;
                this.htmDivCellPhone.Visible = false;
                this.htmDivEmail.Visible = false;
            }
        }
    }
}

