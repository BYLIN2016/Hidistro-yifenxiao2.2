namespace Hidistro.AccountCenter.Profile
{
    using System;
    using Hidistro.Core;

    public abstract class PersonalSubsiteProvider : PersonalProvider
    {
        private static readonly PersonalSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.ProfileData,Hidistro.AccountCenter.DistributionData") as PersonalSubsiteProvider);

        protected PersonalSubsiteProvider()
        {
        }

        public static PersonalSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

