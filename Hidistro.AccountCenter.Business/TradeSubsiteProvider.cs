namespace Hidistro.AccountCenter.Business
{
    using System;
    using Hidistro.Core;

    public abstract class TradeSubsiteProvider : TradeProvider
    {
        private static readonly TradeSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.BusinessData,Hidistro.AccountCenter.DistributionData") as TradeSubsiteProvider);

        protected TradeSubsiteProvider()
        {
        }

        public static TradeSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

