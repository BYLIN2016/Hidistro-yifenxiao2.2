namespace Hidistro.AccountCenter.Profile
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class PersonalHelper
    {
        public static bool AddInpourBlance(InpourRequestInfo inpourRequest)
        {
            return PersonalProvider.Instance().AddInpourBlance(inpourRequest);
        }

        public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            if (null == shippingAddress)
            {
                return 0;
            }
            Globals.EntityCoding(shippingAddress, true);
            return PersonalProvider.Instance().AddShippingAddress(shippingAddress);
        }

        public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            Globals.EntityCoding(balanceDrawRequest, true);
            bool flag = PersonalProvider.Instance().BalanceDrawRequest(balanceDrawRequest);
            if (flag)
            {
                Users.ClearUserCache(HiContext.Current.User);
            }
            return flag;
        }

        public static bool CreateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            if (null == shippingAddress)
            {
                return false;
            }
            Globals.EntityCoding(shippingAddress, true);
            return PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddress, DataProviderAction.Create);
        }

        public static bool DeleteShippingAddress(int shippingId)
        {
            ShippingAddressInfo shippingAddress = new ShippingAddressInfo {
                ShippingId = shippingId
            };
            return PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddress, DataProviderAction.Delete);
        }

        public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            return PersonalProvider.Instance().GetBalanceDetails(query);
        }

        public static InpourRequestInfo GetInpourBlance(string inpourId)
        {
            return PersonalProvider.Instance().GetInpourBlance(inpourId);
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return PersonalProvider.Instance().GetMemberGrade(gradeId);
        }

        public static IList<MemberGradeInfo> GetMemberGrades()
        {
            return PersonalProvider.Instance().GetMemberGrades();
        }

        public static DbQueryResult GetMyReferralMembers(MemberQuery query)
        {
            return PersonalProvider.Instance().GetMyReferralMembers(query);
        }

        public static IList<ShippingAddressInfo> GetShippingAddress(int userId)
        {
            return PersonalProvider.Instance().GetShippingAddress(userId);
        }

        public static int GetShippingAddressCount(int userId)
        {
            return PersonalProvider.Instance().GetShippingAddressCount(userId);
        }

        public static void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
        {
            PersonalProvider.Instance().GetStatisticsNum(out noPayOrderNum, out noReadMessageNum, out noReplyLeaveCommentNum);
        }

        public static ShippingAddressInfo GetUserShippingAddress(int shippingId)
        {
            return PersonalProvider.Instance().GetUserShippingAddress(shippingId);
        }

        public static bool Recharge(BalanceDetailInfo balanceDetails, string InpourId)
        {
            bool flag = PersonalProvider.Instance().AddBalanceDetail(balanceDetails);
            if (flag)
            {
                PersonalProvider.Instance().RemoveInpourRequest(InpourId);
            }
            return flag;
        }

        public static void RemoveInpourRequest(string inpourId)
        {
            PersonalProvider.Instance().RemoveInpourRequest(inpourId);
        }

        public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
        {
            if (null == shippingAddress)
            {
                return false;
            }
            Globals.EntityCoding(shippingAddress, true);
            return PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddress, DataProviderAction.Update);
        }

        public static bool ViewProductConsultations()
        {
            return PersonalProvider.Instance().ViewProductConsultations();
        }
    }
}

