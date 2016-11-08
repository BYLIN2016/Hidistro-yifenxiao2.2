namespace Hidistro.ControlPanel.Members
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    public static class MemberHelper
    {
        public static bool AddBalance(BalanceDetailInfo balanceDetails, decimal money)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            bool flag = MemberProvider.Instance().InsertBalanceDetail(balanceDetails);
            if (flag)
            {
                Users.ClearUserCache(Users.GetUser(balanceDetails.UserId));
            }
            EventLogs.WriteOperationLog(Privilege.MemberAccount, string.Format(CultureInfo.InvariantCulture, "给会员\"{0}\"添加预付款\"{1}\"", new object[] { balanceDetails.UserName, money }));
            return flag;
        }

        public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (null == memberGrade)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            bool flag = MemberProvider.Instance().CreateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[] { memberGrade.Name }));
            }
            return flag;
        }

        public static bool DealBalanceDrawRequest(int userId, bool agree)
        {
            bool flag = MemberProvider.Instance().DealBalanceDrawRequest(userId, agree);
            if (flag)
            {
                Users.ClearUserCache(Users.GetUser(userId));
            }
            return flag;
        }

        public static bool Delete(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            IUser user = Users.GetUser(userId);
            bool flag = MemberProvider.Instance().Delete(userId);
            if (flag)
            {
                Users.ClearUserCache(user);
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool DeleteMemberGrade(int gradeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
            bool flag = MemberProvider.Instance().DeleteMemberGrade(gradeId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[] { gradeId }));
            }
            return flag;
        }

        public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            return MemberProvider.Instance().GetBalanceDetails(query);
        }

        public static DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
        {
            return MemberProvider.Instance().GetBalanceDetailsNoPage(query);
        }

        public static DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            return MemberProvider.Instance().GetBalanceDrawRequests(query);
        }

        public static DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query)
        {
            return MemberProvider.Instance().GetBalanceDrawRequestsNoPage(query);
        }

        public static Member GetMember(int userId)
        {
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Member))
            {
                return (user as Member);
            }
            return null;
        }

        public static DbQueryResult GetMemberBlanceList(MemberQuery query)
        {
            return MemberProvider.Instance().GetMemberBlanceList(query);
        }

        public static Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            return MemberProvider.Instance().GetMemberClientSet();
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return MemberProvider.Instance().GetMemberGrade(gradeId);
        }

        public static IList<MemberGradeInfo> GetMemberGrades()
        {
            return MemberProvider.Instance().GetMemberGrades();
        }

        public static DbQueryResult GetMembers(MemberQuery query)
        {
            return MemberProvider.Instance().GetMembers(query);
        }

        public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            return MemberProvider.Instance().GetMembersNopage(query, fields);
        }

        public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            return MemberProvider.Instance().HasSamePointMemberGrade(memberGrade);
        }

        public static bool InsertClientSet(Dictionary<int, MemberClientSet> clientset)
        {
            return MemberProvider.Instance().InsertClientSet(clientset);
        }

        public static void SetDefalutMemberGrade(int gradeId)
        {
            MemberProvider.Instance().SetDefalutMemberGrade(gradeId);
        }

        public static bool Update(Member member)
        {
            bool flag = Users.UpdateUser(member);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[] { member.UserId }));
            }
            return flag;
        }

        public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (null == memberGrade)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            bool flag = MemberProvider.Instance().UpdateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[] { memberGrade.GradeId }));
            }
            return flag;
        }
    }
}

