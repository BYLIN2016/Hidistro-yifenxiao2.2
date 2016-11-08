namespace Hidistro.SaleSystem.Member
{
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;

    public static class MemberProcessor
    {
        public static CreateUserStatus CreateMember(Member member)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return Users.CreateUser(member, HiContext.Current.Config.RolesConfiguration.Underling);
            }
            return Users.CreateUser(member, HiContext.Current.Config.RolesConfiguration.Member);
        }

        public static IList<OpenIdSettingsInfo> GetConfigedItems()
        {
            return MemberProvider.Instance().GetConfigedItems();
        }

        public static int GetDefaultMemberGrade()
        {
            return MemberProvider.Instance().GetDefaultMemberGrade();
        }

        public static OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
        {
            return MemberProvider.Instance().GetOpenIdSettings(openIdType);
        }

        public static ShippingAddressInfo GetShippingAddress(int shippingId)
        {
            return MemberProvider.Instance().GetShippingAddress(shippingId);
        }

        public static IList<ShippingAddressInfo> GetShippingAddresses(int userId)
        {
            return MemberProvider.Instance().GetShippingAddresses(userId);
        }

        public static LoginUserStatus ValidLogin(Member member)
        {
            if (member == null)
            {
                return LoginUserStatus.InvalidCredentials;
            }
            return Users.ValidateUser(member);
        }
    }
}

