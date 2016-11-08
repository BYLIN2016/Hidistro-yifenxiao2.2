namespace Hidistro.SaleSystem.Member
{
    using System;
    using Hidistro.Core;

    public abstract class MemberSubsiteProvider : MemberProvider
    {
        private static readonly MemberSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.MemberData,Hidistro.SaleSystem.DistributionData") as MemberSubsiteProvider);

        protected MemberSubsiteProvider()
        {
        }

        public static MemberSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

