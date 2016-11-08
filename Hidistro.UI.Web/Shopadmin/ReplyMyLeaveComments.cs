namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class ReplyMyLeaveComments : DistributorPage
    {
        protected Button btnReplyLeaveComments;
        protected KindeditorControl fckReplyContent;
        private long LeaveId;
        protected Label litContent;
        protected Literal litTitle;
        protected Literal litUserName;

        protected void btnReplyLeaveComments_Click(object sender, EventArgs e)
        {
            LeaveCommentReplyInfo target = new LeaveCommentReplyInfo();
            target.LeaveId = this.LeaveId;
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
                if (SubsiteCommentsHelper.ReplyLeaveComment(target) > 0)
                {
                    base.Response.Redirect(Globals.ApplicationPath + string.Format("/Shopadmin/comment/ReplyedLeaveCommentsSuccsed.aspx?leaveId={0}", this.LeaveId), true);
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
            if (!long.TryParse(this.Page.Request.QueryString["LeaveId"], out this.LeaveId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyLeaveComments.Click += new EventHandler(this.btnReplyLeaveComments_Click);
                if (!this.Page.IsPostBack)
                {
                    LeaveCommentInfo leaveComment = SubsiteCommentsHelper.GetLeaveComment(this.LeaveId);
                    if (leaveComment == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.litTitle.Text = Globals.HtmlDecode(leaveComment.Title);
                        this.litContent.Text = Globals.HtmlDecode(leaveComment.PublishContent);
                        this.litUserName.Text = Globals.HtmlDecode(leaveComment.UserName);
                    }
                }
            }
        }
    }
}

