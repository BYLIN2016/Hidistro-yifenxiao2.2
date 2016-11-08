namespace Hidistro.SaleSystem.Shopping
{
    using System;
    using Hidistro.Core;

    public abstract class ShoppingMasterProvider : ShoppingProvider
    {
        private static readonly ShoppingMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.ShoppingData,Hidistro.SaleSystem.Data") as ShoppingMasterProvider);

        protected ShoppingMasterProvider()
        {
        }

        public static ShoppingMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

