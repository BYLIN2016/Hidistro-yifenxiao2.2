namespace Hidistro.AccountCenter.Profile
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public abstract class PersonalProvider
    {
        protected PersonalProvider()
        {
        }

        public abstract bool AddBalanceDetail(BalanceDetailInfo balanceDetails);
        public abstract bool AddInpourBlance(InpourRequestInfo inpourRequest);
        public abstract int AddShippingAddress(ShippingAddressInfo shippingAddress);
        public abstract bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest);
        public abstract bool CreateUpdateDeleteShippingAddress(ShippingAddressInfo shippingAddress, DataProviderAction action);
        public abstract DbQueryResult GetBalanceDetails(BalanceDetailQuery query);
        public abstract InpourRequestInfo GetInpourBlance(string inpourId);
        public abstract MemberGradeInfo GetMemberGrade(int gradeId);
        public abstract IList<MemberGradeInfo> GetMemberGrades();
        public abstract DbQueryResult GetMyReferralMembers(MemberQuery query);
        public abstract IList<ShippingAddressInfo> GetShippingAddress(int userId);
        public abstract int GetShippingAddressCount(int userId);
        public abstract void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum);
        public abstract ShippingAddressInfo GetUserShippingAddress(int shippingId);
        public static PersonalProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return PersonalSubsiteProvider.CreateInstance();
            }
            return PersonalMasterProvider.CreateInstance();
        }

        public abstract void RemoveInpourRequest(string inpourId);
        public abstract bool ViewProductConsultations();
    }
}

