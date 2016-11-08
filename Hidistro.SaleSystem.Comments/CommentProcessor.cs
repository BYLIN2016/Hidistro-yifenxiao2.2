namespace Hidistro.SaleSystem.Comments
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using System;

    public static class CommentProcessor
    {
        public static bool InsertLeaveComment(LeaveCommentInfo leave)
        {
            Globals.EntityCoding(leave, true);
            return CommentProvider.Instance().InsertLeaveComment(leave);
        }
    }
}

