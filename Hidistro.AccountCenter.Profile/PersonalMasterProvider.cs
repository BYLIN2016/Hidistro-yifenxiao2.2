namespace Hidistro.AccountCenter.Profile
{
    using System;
    using Hidistro.Core;

    public abstract class PersonalMasterProvider : PersonalProvider
    {
        private static readonly PersonalMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.ProfileData,Hidistro.AccountCenter.Data") as PersonalMasterProvider);

        protected PersonalMasterProvider()
        {
        }

        public static PersonalMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

