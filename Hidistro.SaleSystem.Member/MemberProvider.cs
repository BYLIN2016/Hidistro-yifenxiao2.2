namespace Hidistro.SaleSystem.Member
{
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public abstract class MemberProvider
    {
        protected MemberProvider()
        {
        }

        public abstract IList<OpenIdSettingsInfo> GetConfigedItems();
        public abstract int GetDefaultMemberGrade();
        public abstract int GetMemberDiscount(int gradeId);
        public abstract OpenIdSettingsInfo GetOpenIdSettings(string openIdType);
        public abstract ShippingAddressInfo GetShippingAddress(int shippingId);
        public abstract IList<ShippingAddressInfo> GetShippingAddresses(int userId);
        public static MemberProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return MemberSubsiteProvider.CreateInstance();
            }
            return MemberMasterProvider.CreateInstance();
        }

        public static OpenIdSettingsInfo PopulateOpenIdSettings(IDataReader reader)
        {
            OpenIdSettingsInfo info2 = new OpenIdSettingsInfo();
            info2.OpenIdType = (string) reader["OpenIdType"];
            info2.Name = (string) reader["Name"];
            info2.Settings = (string) reader["Settings"];
            OpenIdSettingsInfo info = info2;
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            return info;
        }
    }
}

