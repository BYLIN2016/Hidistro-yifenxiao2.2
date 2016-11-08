namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;

    public static class CategoryBrowser
    {
        private const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";
        private const string MainCategoriesCachekey = "DataCache-Categories";

        public static IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum = 0x3e8)
        {
            return CategoryProvider.Instance().GetAttributeInfoByCategoryId(categoryId, maxNum);
        }

        public static DataTable GetBrandCategories(int categoryId, int maxNum = 0x3e8)
        {
            return CategoryProvider.Instance().GetBrandCategories(categoryId, maxNum);
        }

        public static BrandCategoryInfo GetBrandCategory(int brandId)
        {
            return CategoryProvider.Instance().GetBrandCategory(brandId);
        }

        public static DataTable GetCategories()
        {
            DataTable categories = null;
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                categories = HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value)) as DataTable;
            }
            else
            {
                categories = HiCache.Get("DataCache-Categories") as DataTable;
            }
            if (categories == null)
            {
                categories = CategoryProvider.Instance().GetCategories();
                if (HiContext.Current.SiteSettings.IsDistributorSettings)
                {
                    HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value), categories, 360, CacheItemPriority.Normal);
                    return categories;
                }
                HiCache.Insert("DataCache-Categories", categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            return CategoryProvider.Instance().GetCategory(categoryId);
        }

        public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 0x3e8)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("ParentCategoryId = " + parentCategoryId);
            for (int i = 0; (i < maxNum) && (i < rowArray.Length); i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static DataSet GetThreeLayerCategories()
        {
            return CategoryProvider.Instance().GetThreeLayerCategories();
        }
    }
}

