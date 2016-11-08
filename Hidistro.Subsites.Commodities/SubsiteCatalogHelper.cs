namespace Hidistro.Subsites.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Caching;

    public class SubsiteCatalogHelper
    {
        private const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";

        private SubsiteCatalogHelper()
        {
        }

        public static CategoryActionStatus AddCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            if (SubsiteProductProvider.Instance().CreateCategory(category) > 0)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return CategoryActionStatus.Success;
        }

        public static bool DeleteCategory(int categoryId)
        {
            bool flag = SubsiteProductProvider.Instance().DeleteCategory(categoryId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return flag;
        }

        public static int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            return SubsiteProductProvider.Instance().DisplaceCategory(oldCategoryId, newCategory);
        }

        public static int DownloadCategory()
        {
            int num = SubsiteProductProvider.Instance().DownloadCategory();
            if (num > 0)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return num;
        }

        private static DataTable GetCategories()
        {
            DataTable categories = new DataTable();
            if (HiContext.Current.User.UserRole != UserRole.Anonymous)
            {
                categories = HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId)) as DataTable;
            }
            else
            {
                categories = HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value)) as DataTable;
            }
            if (categories == null)
            {
                categories = SubsiteProductProvider.Instance().GetCategories();
                if (HiContext.Current.User.UserRole != UserRole.Anonymous)
                {
                    HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId), categories, 360, CacheItemPriority.Normal);
                    return categories;
                }
                HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value), categories, 360, CacheItemPriority.Normal);
            }
            return categories;
        }

        public static CategoryInfo GetCategory(int categoryId)
        {
            return SubsiteProductProvider.Instance().GetCategory(categoryId);
        }

        public static string GetFullCategory(int categoryId)
        {
            CategoryInfo category = GetCategory(categoryId);
            if (category == null)
            {
                return null;
            }
            string name = category.Name;
            while ((category != null) && category.ParentCategoryId.HasValue)
            {
                category = GetCategory(category.ParentCategoryId.Value);
                if (category != null)
                {
                    name = category.Name + " >> " + name;
                }
            }
            return name;
        }

        public static IList<CategoryInfo> GetMainCategories()
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("Depth = 1");
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        public static IList<CategoryInfo> GetSequenceCategories()
        {
            IList<CategoryInfo> categories = new List<CategoryInfo>();
            IList<CategoryInfo> mainCategories = GetMainCategories();
            foreach (CategoryInfo info in mainCategories)
            {
                categories.Add(info);
                LoadSubCategorys(info.CategoryId, categories);
            }
            return categories;
        }

        public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
        {
            IList<CategoryInfo> list = new List<CategoryInfo>();
            DataRow[] rowArray = GetCategories().Select("ParentCategoryId = " + parentCategoryId.ToString());
            for (int i = 0; i < rowArray.Length; i++)
            {
                list.Add(DataMapper.ConvertDataRowToProductCategory(rowArray[i]));
            }
            return list;
        }

        private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
        {
            IList<CategoryInfo> subCategories = GetSubCategories(parentCategoryId);
            if ((subCategories != null) && (subCategories.Count > 0))
            {
                foreach (CategoryInfo info in subCategories)
                {
                    categories.Add(info);
                    LoadSubCategorys(info.CategoryId, categories);
                }
            }
        }

        public static bool SetCategoryThemes(int categoryId, string themeName)
        {
            if (SubsiteProductProvider.Instance().SetCategoryThemes(categoryId, themeName))
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return false;
        }

        public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            return SubsiteProductProvider.Instance().SetProductExtendCategory(productId, extendCategoryPath);
        }

        public static void SwapCategorySequence(int categoryId, int displaysequence)
        {
            if (categoryId > 0)
            {
                SubsiteProductProvider.Instance().SwapCategorySequence(categoryId, displaysequence);
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
        }

        public static CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            if (null == category)
            {
                return CategoryActionStatus.UnknowError;
            }
            Globals.EntityCoding(category, true);
            CategoryActionStatus unknowError = CategoryActionStatus.UnknowError;
            unknowError = SubsiteProductProvider.Instance().UpdateCategory(category);
            if (unknowError == CategoryActionStatus.Success)
            {
                HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
            }
            return unknowError;
        }
    }
}

