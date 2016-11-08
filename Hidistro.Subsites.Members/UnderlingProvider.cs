namespace Hidistro.Subsites.Members
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using Hidistro.Core;

    public abstract class UnderlingProvider
    {
        private static readonly UnderlingProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.UnderlingData,Hidistro.Subsites.Data") as UnderlingProvider);

        protected UnderlingProvider()
        {
        }

        public abstract bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails);
        public abstract bool CreateUnderlingGrade(MemberGradeInfo underlingGrade);
        public abstract bool DealBalanceDrawRequest(int userId, bool agree);
        public abstract bool DeleteMember(int userId);
        public abstract void DeleteSKUMemberPrice(int gradeId);
        public abstract bool DeleteUnderlingGrade(int gradeId);
        public abstract DbQueryResult GetBalanceDetails(BalanceDetailQuery query);
        public abstract DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query);
        public abstract MemberGradeInfo GetMemberGrade(int gradeId);
        public abstract DbQueryResult GetMembers(MemberQuery query);
        public abstract DataTable GetMembersNopage(MemberQuery query, IList<string> fields);
        public abstract DbQueryResult GetUnderlingBlanceList(MemberQuery query);
        public abstract IList<MemberGradeInfo> GetUnderlingGrades();
        public abstract DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total);
        public abstract DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query);
        public abstract IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days);
        public abstract bool HasSamePointMemberGrade(MemberGradeInfo memberGrade);
        public static UnderlingProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract void SetDefalutUnderlingGrade(int gradeId);
        public abstract bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade);
    }
}

