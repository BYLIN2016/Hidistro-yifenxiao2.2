namespace Hidistro.Subsites.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class UnderlingHelper
    {
        public static bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            bool flag = UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails);
            if (flag)
            {
                Users.ClearUserCache(Users.GetUser(balanceDetails.UserId));
            }
            return flag;
        }

        public static bool CreateUnderlingGrade(MemberGradeInfo underlingGrade)
        {
            Globals.EntityCoding(underlingGrade, true);
            return UnderlingProvider.Instance().CreateUnderlingGrade(underlingGrade);
        }

        public static bool DealBalanceDrawRequest(int userId, bool agree)
        {
            bool flag = UnderlingProvider.Instance().DealBalanceDrawRequest(userId, agree);
            if (flag)
            {
                Users.ClearUserCache(Users.GetUser(userId));
            }
            return flag;
        }

        public static bool DeleteMember(int userId)
        {
            IUser user = Users.GetUser(userId);
            bool flag = UnderlingProvider.Instance().DeleteMember(userId);
            if (flag)
            {
                Users.ClearUserCache(user);
            }
            return flag;
        }

        public static bool DeleteUnderlingGrade(int gradeId)
        {
            bool flag = UnderlingProvider.Instance().DeleteUnderlingGrade(gradeId);
            if (flag)
            {
                UnderlingProvider.Instance().DeleteSKUMemberPrice(gradeId);
            }
            return flag;
        }

        public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            return UnderlingProvider.Instance().GetBalanceDetails(query);
        }

        public static DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            return UnderlingProvider.Instance().GetBalanceDrawRequests(query);
        }

        public static Member GetMember(int userId)
        {
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Underling))
            {
                return (user as Member);
            }
            return null;
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return UnderlingProvider.Instance().GetMemberGrade(gradeId);
        }

        public static DbQueryResult GetMembers(MemberQuery query)
        {
            return UnderlingProvider.Instance().GetMembers(query);
        }

        public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            return UnderlingProvider.Instance().GetMembersNopage(query, fields);
        }

        public static DbQueryResult GetUnderlingBlanceList(MemberQuery query)
        {
            return UnderlingProvider.Instance().GetUnderlingBlanceList(query);
        }

        public static IList<MemberGradeInfo> GetUnderlingGrades()
        {
            return UnderlingProvider.Instance().GetUnderlingGrades();
        }

        public static DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total)
        {
            return UnderlingProvider.Instance().GetUnderlingStatistics(query, out total);
        }

        public static DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query)
        {
            return UnderlingProvider.Instance().GetUnderlingStatisticsNoPage(query);
        }

        public static IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days)
        {
            return UnderlingProvider.Instance().GetUserIncrease(year, month, days);
        }

        public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            return UnderlingProvider.Instance().HasSamePointMemberGrade(memberGrade);
        }

        public static void SetDefalutUnderlingGrade(int gradeId)
        {
            UnderlingProvider.Instance().SetDefalutUnderlingGrade(gradeId);
        }

        public static bool Update(Member underling)
        {
            return Users.UpdateUser(underling);
        }

        public static bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade)
        {
            Globals.EntityCoding(underlingGrade, true);
            return UnderlingProvider.Instance().UpdateUnderlingGrade(underlingGrade);
        }
    }
}

