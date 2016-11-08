namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ManageLeaveComments)]
    public class ReplyLeaveComments : AdminPage
    {
        protected Button btnReplyLeaveComments;
        protected KindeditorControl fckReplyContent;
        private long leaveId;
        protected Label litContent;
        protected Literal litTitle;
        protected Literal litUserName;

        protected void btnReplyLeaveComments_Click(object sender, EventArgs e)
        {
            LeaveCommentReplyInfo target = new LeaveCommentReplyInfo();
            target.LeaveId = this.leaveId;
            if (string.IsNullOrEmpty(this.fckReplyContent.Text))
            {
                target.ReplyContent = null;
            }
            else
            {
                target.ReplyContent = this.fckReplyContent.Text;
            }
            target.UserId = HiContext.Current.User.UserId;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<LeaveCommentReplyInfo>(target, new string[] { "ValLeaveCommentReply" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else
            {
                if (NoticeHelper.ReplyLeaveComment(target) > 0)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/comment/ReplyedLeaveCommentsSuccsed.aspx?leaveId={0}", this.leaveId)), true);
                }
                else
                {
                    this.ShowMsg("回复客户留言失败", false);
                }
                this.fckReplyContent.Text = string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["LeaveId"], out this.leaveId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyLeaveComments.Click += new EventHandler(this.btnReplyLeaveComments_Click);
                if (!this.Page.IsPostBack)
                {
                    LeaveCommentInfo leaveComment = NoticeHelper.GetLeaveComment(this.leaveId);
                    this.litTitle.Text = Globals.HtmlDecode(leaveComment.Title);
                    this.litContent.Text = Globals.HtmlDecode(leaveComment.PublishContent);
                    this.litUserName.Text = Globals.HtmlDecode(leaveComment.UserName);
                }
            }
        }
    }
}

