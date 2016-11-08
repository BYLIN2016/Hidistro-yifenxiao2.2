namespace Hidistro.SaleSystem.Member
{
    using System;
    using Hidistro.Core;

    public abstract class MemberMasterProvider : MemberProvider
    {
        private static readonly MemberMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.MemberData,Hidistro.SaleSystem.Data") as MemberMasterProvider);

        protected MemberMasterProvider()
        {
        }

        public static MemberMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

