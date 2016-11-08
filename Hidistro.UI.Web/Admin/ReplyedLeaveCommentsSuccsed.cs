namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ReplyedLeaveCommentsSuccsed : AdminPage
    {
        protected HtmlAnchor a_user;
        protected DataList dtlistLeaveCommentsReply;
        protected HyperLink hlReply;
        protected Label lblUserName;
        private long leaveId;
        protected FormatedTimeLabel litLeaveDate;
        protected Literal litPublishContent;
        protected Literal litTitle;

        private void BindList()
        {
            this.dtlistLeaveCommentsReply.DataSource = NoticeHelper.GetReplyLeaveComments(this.leaveId);
            this.dtlistLeaveCommentsReply.DataBind();
        }

        private void dtlistLeaveCommentsReply_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            if (NoticeHelper.DeleteLeaveCommentReply(Convert.ToInt64(this.dtlistLeaveCommentsReply.DataKeys[e.Item.ItemIndex])))
            {
                this.ShowMsg("删除成功", true);
                this.BindList();
            }
            else
            {
                this.ShowMsg("删除失败，请重试", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(this.Page.Request.QueryString["leaveId"], out this.leaveId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.dtlistLeaveCommentsReply.DeleteCommand += new DataListCommandEventHandler(this.dtlistLeaveCommentsReply_DeleteCommand);
                if (!base.IsPostBack)
                {
                    this.SetControl(this.leaveId);
                    this.hlReply.NavigateUrl = Globals.ApplicationPath + "/admin/comment/ReplyLeaveComments.aspx?LeaveId=" + this.leaveId;
                    this.BindList();
                }
            }
        }

        private void SetControl(long leaveId)
        {
            LeaveCommentInfo leaveComment = NoticeHelper.GetLeaveComment(leaveId);
            Globals.EntityCoding(leaveComment, false);
            this.litTitle.Text = leaveComment.Title;
            this.lblUserName.Text = leaveComment.UserName;
            this.litLeaveDate.Time = leaveComment.PublishDate;
            this.litPublishContent.Text = leaveComment.PublishContent;
            this.a_user.HRef = string.Concat(new object[] { "javascript:DialogFrame('", Globals.ApplicationPath, "/admin/member/MemberDetails.aspx?userId=", leaveComment.UserId, "','查看会员详细信息',null,null)" });
        }
    }
}

