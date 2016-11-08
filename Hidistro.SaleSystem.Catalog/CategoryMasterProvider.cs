namespace Hidistro.SaleSystem.Catalog
{
    using System;
    using Hidistro.Core;

    public abstract class CategoryMasterProvider : CategoryProvider
    {
        private static readonly CategoryMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CategoryData,Hidistro.SaleSystem.Data") as CategoryMasterProvider);

        protected CategoryMasterProvider()
        {
        }

        public static CategoryMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

