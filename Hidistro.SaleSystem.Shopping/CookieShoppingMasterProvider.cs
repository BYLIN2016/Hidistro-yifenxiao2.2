namespace Hidistro.SaleSystem.Shopping
{
    using System;
    using Hidistro.Core;

    public abstract class CookieShoppingMasterProvider : CookieShoppingProvider
    {
        private static readonly CookieShoppingMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CookieShoppingData,Hidistro.SaleSystem.Data") as CookieShoppingMasterProvider);

        protected CookieShoppingMasterProvider()
        {
        }

        public static CookieShoppingMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

