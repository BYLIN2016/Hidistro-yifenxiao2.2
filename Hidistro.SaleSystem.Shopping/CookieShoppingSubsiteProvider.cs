namespace Hidistro.SaleSystem.Shopping
{
    using System;
    using Hidistro.Core;

    public abstract class CookieShoppingSubsiteProvider : CookieShoppingProvider
    {
        private static readonly CookieShoppingSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CookieShoppingData,Hidistro.SaleSystem.DistributionData") as CookieShoppingSubsiteProvider);

        protected CookieShoppingSubsiteProvider()
        {
        }

        public static CookieShoppingSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

