namespace Hidistro.ControlPanel.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Hidistro.Membership.Context;

    public sealed class NoticeHelper
    {
        private NoticeHelper()
        {
        }

        public static int AddMessageToDistributor(IList<MessageBoxInfo> messageBoxInfos)
        {
            int num = 0;
            foreach (MessageBoxInfo info in messageBoxInfos)
            {
                if (CommentsProvider.Instance().AddMessage(info, UserRole.Distributor))
                {
                    num++;
                }
            }
            return num;
        }

        public static bool CreateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return CommentsProvider.Instance().AddAffiche(affiche);
        }

        public static bool DeleteAffiche(int afficheId)
        {
            return CommentsProvider.Instance().DeleteAffiche(afficheId);
        }

        public static int DeleteAffiches(List<int> affiches)
        {
            if ((affiches == null) || (affiches.Count == 0))
            {
                return 0;
            }
            return CommentsProvider.Instance().DeleteAffiches(affiches);
        }

        public static bool DeleteLeaveComment(long leaveId)
        {
            return CommentsProvider.Instance().DeleteLeaveComment(leaveId);
        }

        public static bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            return CommentsProvider.Instance().DeleteLeaveCommentReply(leaveReplyId);
        }

        public static int DeleteLeaveComments(IList<long> leaveIds)
        {
            return CommentsProvider.Instance().DeleteLeaveComments(leaveIds);
        }

        public static int DeleteManagerMessages(IList<long> messageList)
        {
            return CommentsProvider.Instance().DeleteManagerMessages(messageList);
        }

        public static AfficheInfo GetAffiche(int afficheId)
        {
            return CommentsProvider.Instance().GetAffiche(afficheId);
        }

        public static List<AfficheInfo> GetAfficheList()
        {
            return CommentsProvider.Instance().GetAfficheList();
        }

        public static IList<Distributor> GetDistributorsByRank(int? gradeId)
        {
            return CommentsProvider.Instance().GetDistributorsByRank(gradeId);
        }

        public static LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            return CommentsProvider.Instance().GetLeaveComment(leaveId);
        }

        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return CommentsProvider.Instance().GetLeaveComments(query);
        }

        public static MessageBoxInfo GetManagerMessage(long messageId)
        {
            return CommentsProvider.Instance().GetManagerMessage(messageId);
        }

        public static DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role)
        {
            return CommentsProvider.Instance().GetManagerReceivedMessages(query, role);
        }

        public static DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role)
        {
            return CommentsProvider.Instance().GetManagerSendedMessages(query, role);
        }

        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return CommentsProvider.Instance().GetMembersByRank(gradeId);
        }

        public static int GetMemberUnReadMessageNum()
        {
            return CommentsProvider.Instance().GetMemberUnReadMessageNum();
        }

        public static DataTable GetReplyLeaveComments(long leaveId)
        {
            return CommentsProvider.Instance().GetReplyLeaveComments(leaveId);
        }

        public static bool PostManagerMessageIsRead(long messageId)
        {
            return CommentsProvider.Instance().PostManagerMessageIsRead(messageId);
        }

        public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            leaveReply.ReplyDate = DateTime.Now;
            return CommentsProvider.Instance().ReplyLeaveComment(leaveReply);
        }

        public static int SendMessageToDistributor(IList<MessageBoxInfo> messageBoxInfos)
        {
            int num = 0;
            foreach (MessageBoxInfo info in messageBoxInfos)
            {
                if (CommentsProvider.Instance().InsertMessage(info, UserRole.Distributor))
                {
                    num++;
                }
            }
            return num;
        }

        public static int SendMessageToMember(IList<MessageBoxInfo> messageBoxInfos)
        {
            int num = 0;
            foreach (MessageBoxInfo info in messageBoxInfos)
            {
                if (CommentsProvider.Instance().InsertMessage(info, UserRole.Member))
                {
                    num++;
                }
            }
            return num;
        }

        public static bool UpdateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return CommentsProvider.Instance().UpdateAffiche(affiche);
        }
    }
}

