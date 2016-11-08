namespace Hidistro.SaleSystem.Catalog
{
    using System;
    using Hidistro.Core;

    public abstract class CategorySubsiteProvider : CategoryProvider
    {
        private static readonly CategorySubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CategoryData,Hidistro.SaleSystem.DistributionData") as CategorySubsiteProvider);

        protected CategorySubsiteProvider()
        {
        }

        public static CategorySubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

