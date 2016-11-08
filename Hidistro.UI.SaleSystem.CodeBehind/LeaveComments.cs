namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.SaleSystem.Member;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class LeaveComments : HtmlTemplatedWebControl
    {
        private IButton btnRefer;
        private Pager pager;
        private ThemedTemplatedRepeater rptLeaveComments;
        private HtmlControl spLeavePsw;
        private HtmlControl spLeaveReg;
        private HtmlControl spLeaveUserName;
        private TextBox txtContent;
        private HtmlInputText txtLeaveCode;
        private HtmlInputText txtLeavePsw;
        private HtmlInputText txtLeaveUserName;
        private TextBox txtTitle;
        private TextBox txtUserName;
        private string verifyCodeKey = "VerifyCode";

        protected override void AttachChildControls()
        {
            this.rptLeaveComments = (ThemedTemplatedRepeater) this.FindControl("rptLeaveComments");
            this.pager = (Pager) this.FindControl("pager");
            this.txtTitle = (TextBox) this.FindControl("txtTitle");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtContent = (TextBox) this.FindControl("txtContent");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.spLeaveUserName = (HtmlControl) this.FindControl("spLeaveUserName");
            this.spLeavePsw = (HtmlControl) this.FindControl("spLeavePsw");
            this.spLeaveReg = (HtmlControl) this.FindControl("spLeaveReg");
            this.txtLeaveUserName = (HtmlInputText) this.FindControl("txtLeaveUserName");
            this.txtLeavePsw = (HtmlInputText) this.FindControl("txtLeavePsw");
            this.txtLeaveCode = (HtmlInputText) this.FindControl("txtLeaveCode");
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            PageTitle.AddSiteNameTitle("客户留言", HiContext.Current.Context);
            if ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling))
            {
                this.txtUserName.Text = HiContext.Current.User.Username;
                this.txtLeaveUserName.Value = string.Empty;
                this.txtLeavePsw.Value = string.Empty;
                this.spLeaveUserName.Visible = false;
                this.spLeavePsw.Visible = false;
                this.spLeaveReg.Visible = false;
                this.btnRefer.Text = "发表";
            }
            else
            {
                this.spLeaveUserName.Visible = true;
                this.spLeavePsw.Visible = true;
                this.spLeaveReg.Visible = true;
                this.btnRefer.Text = "登录并留言";
            }
            this.txtLeaveCode.Value = string.Empty;
            this.BindList();
        }

        private void BindList()
        {
            LeaveCommentQuery query = new LeaveCommentQuery();
            query.PageSize = 3;
            query.PageIndex = this.pager.PageIndex;
            DbQueryResult leaveComments = CommentBrowser.GetLeaveComments(query);
            this.rptLeaveComments.DataSource = leaveComments.Data;
            this.rptLeaveComments.DataBind();
            this.pager.TotalRecords = (int) (leaveComments.TotalRecords * (Convert.ToDouble(this.pager.PageSize) / 3.0));
        }

        public void btnRefer_Click(object sender, EventArgs e)
        {
            if (!HiContext.Current.CheckVerifyCode(this.txtLeaveCode.Value))
            {
                this.ShowMessage("验证码不正确", false);
            }
            else if (this.ValidateConvert() && (((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)) || this.userRegion(this.txtLeaveUserName.Value, this.txtLeavePsw.Value)))
            {
                LeaveCommentInfo target = new LeaveCommentInfo();
                target.UserName = Globals.HtmlEncode(this.txtUserName.Text);
                target.UserId = new int?(HiContext.Current.User.UserId);
                target.Title = Globals.HtmlEncode(this.txtTitle.Text);
                target.PublishContent = Globals.HtmlEncode(this.txtContent.Text);
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<LeaveCommentInfo>(target, new string[] { "Refer" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    this.ShowMessage(msg, false);
                }
                else
                {
                    if (CommentProcessor.InsertLeaveComment(target))
                    {
                        this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "留言成功，管理员回复后即可显示", Globals.GetSiteUrls().UrlData.FormatUrl("LeaveComments")));
                    }
                    else
                    {
                        this.ShowMessage("留言失败", false);
                    }
                    this.txtTitle.Text = string.Empty;
                    this.txtContent.Text = string.Empty;
                }
            }
        }

        private bool CheckVerifyCode(string verifyCode)
        {
            if (HttpContext.Current.Request.Cookies[this.verifyCodeKey] == null)
            {
                return false;
            }
            return (string.Compare(HiCryptographer.Decrypt(HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value), verifyCode, true, CultureInfo.InvariantCulture) == 0);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-LeaveComments.html";
            }
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["isCallback"]) && (HttpContext.Current.Request["isCallback"] == "true"))
            {
                string verifyCode = HttpContext.Current.Request["code"];
                string str2 = "";
                if (!this.CheckVerifyCode(verifyCode))
                {
                    str2 = "0";
                }
                else
                {
                    str2 = "1";
                }
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write("{ ");
                HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", str2));
                HttpContext.Current.Response.Write("}");
                HttpContext.Current.Response.End();
            }
            base.OnInit(e);
        }

        private bool userRegion(string username, string password)
        {
            HiContext current = HiContext.Current;
            Member member = Users.GetUser(0, username, false, true) as Member;
            if ((member == null) || member.IsAnonymous)
            {
                this.ShowMessage("用户名或密码错误", false);
                return false;
            }
            member.Password = password;
            switch (MemberProcessor.ValidLogin(member))
            {
                case LoginUserStatus.Success:
                {
                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
                    member.GetUserCookie().WriteCookie(authCookie, 30, false);
                    current.User = member;
                    return true;
                }
                case LoginUserStatus.AccountPending:
                    this.ShowMessage("用户账号还没有通过审核", false);
                    return false;

                case LoginUserStatus.InvalidCredentials:
                    this.ShowMessage("用户名或密码错误", false);
                    return false;
            }
            this.ShowMessage("未知错误", false);
            return false;
        }

        private bool ValidateConvert()
        {
            string str = string.Empty;
            if (((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling)) && (string.IsNullOrEmpty(this.txtLeaveUserName.Value) || string.IsNullOrEmpty(this.txtLeavePsw.Value)))
            {
                str = str + Formatter.FormatErrorMessage("请填写用户名和密码");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
                return false;
            }
            return true;
        }
    }
}

