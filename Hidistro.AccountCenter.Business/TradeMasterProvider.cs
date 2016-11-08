namespace Hidistro.AccountCenter.Business
{
    using System;
    using Hidistro.Core;

    public abstract class TradeMasterProvider : TradeProvider
    {
        private static readonly TradeMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.BusinessData,Hidistro.AccountCenter.Data") as TradeMasterProvider);

        protected TradeMasterProvider()
        {
        }

        public static TradeMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

