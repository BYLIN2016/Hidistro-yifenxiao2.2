namespace Hidistro.SaleSystem.Shopping
{
    using System;
    using Hidistro.Core;

    public abstract class ShoppingSubsiteProvider : ShoppingProvider
    {
        private static readonly ShoppingSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.ShoppingData,Hidistro.SaleSystem.DistributionData") as ShoppingSubsiteProvider);

        protected ShoppingSubsiteProvider()
        {
        }

        public static ShoppingSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

