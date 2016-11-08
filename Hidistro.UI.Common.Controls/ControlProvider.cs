namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;

    public abstract class ControlProvider
    {
        private static readonly ControlProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.UI.Common.Data.SqlCommonDataProvider, Hidistro.UI.Common.Data") as ControlProvider);

        protected ControlProvider()
        {
        }

        public abstract DataTable GetBrandCategories();
        public abstract DataTable GetBrandCategoriesByTypeId(int typeId);
        public abstract void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum);
        public abstract IList<ProductLineInfo> GetProductLineList();
        public abstract IList<ProductTypeInfo> GetProductTypes();
        public abstract IList<ShippingModeInfo> GetShippingModes();
        public abstract DataTable GetSkuContentBySku(string skuId);
        public abstract DataTable GetTags();
        public static ControlProvider Instance()
        {
            return _defaultInstance;
        }
    }
}

