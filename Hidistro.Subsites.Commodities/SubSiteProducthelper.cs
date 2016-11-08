namespace Hidistro.Subsites.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.HOP;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;

    public static class SubSiteProducthelper
    {
        public static bool AddProductTags(int productId, IList<int> tagsId, DbTransaction tran)
        {
            return SubsiteProductProvider.Instance().AddProductTags(productId, tagsId, tran);
        }

        public static bool AddRelatedProduct(int productId, int relatedProductId)
        {
            return SubsiteProductProvider.Instance().AddRelatedProduct(productId, relatedProductId);
        }

        public static bool AddSubjectProduct(int tagId, int productId)
        {
            IList<int> productIds = new List<int>();
            productIds.Add(productId);
            return SubsiteProductProvider.Instance().AddSubjectProducts(tagId, productIds);
        }

        public static bool AddSubjectProducts(int tagId, IList<int> productIds)
        {
            return SubsiteProductProvider.Instance().AddSubjectProducts(tagId, productIds);
        }

        public static bool AddTaobaoProductId(int productId, long taobaoProductId, int distributorId)
        {
            return SubsiteProductProvider.Instance().AddTaobaoProductId(productId, taobaoProductId, distributorId);
        }

        public static bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
        {
            return SubsiteProductProvider.Instance().CheckPrice(productIds, basePriceName, checkPrice);
        }

        public static bool CheckPrice(string productIds, string basePriceName, decimal checkPrice, string operation)
        {
            return SubsiteProductProvider.Instance().CheckPrice(productIds, basePriceName, checkPrice, operation);
        }

        public static bool ClearRelatedProducts(int productId)
        {
            return SubsiteProductProvider.Instance().ClearRelatedProducts(productId);
        }

        public static bool ClearSubjectProducts(int tagId)
        {
            return SubsiteProductProvider.Instance().ClearSubjectProducts(tagId);
        }

        public static int DeleteProducts(string productIds)
        {
            return SubsiteProductProvider.Instance().DeleteProducts(productIds);
        }

        public static bool DeleteProductTags(int productId, DbTransaction tran)
        {
            return SubsiteProductProvider.Instance().DeleteProductTags(productId, tran);
        }

        public static bool DownloadProduct(int productId, bool isDownCategory)
        {
            return SubsiteProductProvider.Instance().DownloadProduct(productId, isDownCategory);
        }

        public static IList<ProductLineInfo> GetAuthorizeProductLineList()
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProductLineList();
        }

        public static DataTable GetAuthorizeProductLines()
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProductLines();
        }

        public static DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload)
        {
            return SubsiteProductProvider.Instance().GetAuthorizeProducts(query, onlyNotDownload);
        }

        public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            return SubsiteProductProvider.Instance().GetExportProducts(query, removeProductIds);
        }

        public static DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            DataSet set = SubsiteProductProvider.Instance().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
            set.Tables[0].TableName = "types";
            set.Tables[1].TableName = "attributes";
            set.Tables[2].TableName = "values";
            set.Tables[3].TableName = "products";
            set.Tables[4].TableName = "skus";
            set.Tables[5].TableName = "skuItems";
            set.Tables[6].TableName = "productAttributes";
            set.Tables[7].TableName = "taobaosku";
            return set;
        }

        public static DataTable GetGroupBuyProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetGroupBuyProducts(query);
        }

        public static ProductInfo GetProduct(int productId)
        {
            return SubsiteProductProvider.Instance().GetProduct(productId);
        }

        public static DataTable GetProductAttribute(int productId)
        {
            return SubsiteProductProvider.Instance().GetProductAttribute(productId);
        }

        public static DataTable GetProductBaseInfo(string productIds)
        {
            return SubsiteProductProvider.Instance().GetProductBaseInfo(productIds);
        }

        public static IList<int> GetProductIds(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetProductIds(query);
        }

        public static DbQueryResult GetProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetProducts(query);
        }

        public static IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            return SubsiteProductProvider.Instance().GetProducts(productIds);
        }

        public static DataTable GetProductSKU(int productId)
        {
            return SubsiteProductProvider.Instance().GetProductSKU(productId);
        }

        public static IList<int> GetProductTags(int productId)
        {
            return SubsiteProductProvider.Instance().GetProductTags(productId);
        }

        public static DataTable GetPuchaseProduct(string skuId)
        {
            return SubsiteProductProvider.Instance().GetPuchaseProduct(skuId);
        }

        public static DataTable GetPuchaseProducts(ProductQuery query, out int count)
        {
            return SubsiteProductProvider.Instance().GetPuchaseProducts(query, out count);
        }

        public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            return SubsiteProductProvider.Instance().GetRelatedProducts(page, productId);
        }

        public static DataTable GetSkuContent(long taobaoProductId, string taobaoSkuId, int distributorId)
        {
            string skuId = SubsiteProductProvider.Instance().GetSkuIdByTaobao(taobaoProductId, taobaoSkuId, distributorId);
            return SubsiteProductProvider.Instance().GetSkuContentBySku(skuId, distributorId);
        }

        public static DataTable GetSkuContentBySku(string skuId)
        {
            return SubsiteProductProvider.Instance().GetSkuContentBySku(skuId);
        }

        public static IList<SKUItem> GetSkus(string productIds)
        {
            return SubsiteProductProvider.Instance().GetSkus(productIds);
        }

        public static DataTable GetSkusByProductId(int productId)
        {
            return SubsiteProductProvider.Instance().GetSkusByProductId(productId);
        }

        public static DataTable GetSkuUnderlingPrices(string productIds)
        {
            return SubsiteProductProvider.Instance().GetSkuUnderlingPrices(productIds);
        }

        public static DbQueryResult GetSubjectProducts(int tagId, Pagination page)
        {
            return SubsiteProductProvider.Instance().GetSubjectProducts(tagId, page);
        }

        public static DbQueryResult GetSubmitPuchaseProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetSubmitPuchaseProducts(query);
        }

        public static DataTable GetTags()
        {
            return SubsiteProductProvider.Instance().GetTags();
        }

        public static PublishToTaobaoProductInfo GetTaobaoProduct(int productId, int distributorId)
        {
            return SubsiteProductProvider.Instance().GetTaobaoProduct(productId, distributorId);
        }

        public static DbQueryResult GetToTaobaoProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetToTaobaoProducts(query);
        }

        public static DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            return SubsiteProductProvider.Instance().GetUnclassifiedProducts(query);
        }

        public static int GetUpProducts()
        {
            return SubsiteProductProvider.Instance().GetUpProducts();
        }

        public static bool IsOnSale(string productIds)
        {
            return SubsiteProductProvider.Instance().IsOnSale(productIds);
        }

        public static bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            return SubsiteProductProvider.Instance().RemoveRelatedProduct(productId, relatedProductId);
        }

        public static bool RemoveSubjectProduct(int tagId, int productId)
        {
            return SubsiteProductProvider.Instance().RemoveSubjectProduct(tagId, productId);
        }

        public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            return SubsiteProductProvider.Instance().ReplaceProductNames(productIds, oldWord, newWord);
        }

        public static bool UpdateProduct(ProductInfo product, Dictionary<string, decimal> skuSalePrice, IList<int> tagIdList)
        {
            bool flag;
            if (null == product)
            {
                return false;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsiteProductProvider.Instance().UpdateProduct(product, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsiteProductProvider.Instance().AddSkuSalePrice(product.ProductId, skuSalePrice, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!DeleteProductTags(product.ProductId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (((tagIdList != null) && (tagIdList.Count > 0)) && !AddProductTags(product.ProductId, tagIdList, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
            }
            return flag;
        }

        public static bool UpdateProductCategory(int productId, int newCategoryId)
        {
            if (newCategoryId != 0)
            {
                return SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, SubsiteCatalogHelper.GetCategory(newCategoryId).Path + "|");
            }
            return SubsiteProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, null);
        }

        public static bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            return SubsiteProductProvider.Instance().UpdateProductNames(productIds, prefix, suffix);
        }

        public static int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            return SubsiteProductProvider.Instance().UpdateProductSaleStatus(productIds, saleStatus);
        }

        public static bool UpdateShowSaleCounts(DataTable dt)
        {
            if ((dt == null) || (dt.Rows.Count <= 0))
            {
                return false;
            }
            return SubsiteProductProvider.Instance().UpdateShowSaleCounts(dt);
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            return SubsiteProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts);
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            return SubsiteProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
        }

        public static bool UpdateSkuUnderlingPrices(DataSet ds, string skuIds)
        {
            if ((ds == null) || string.IsNullOrEmpty(skuIds))
            {
                return false;
            }
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(ds, skuIds);
        }

        public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price)
        {
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, price);
        }

        public static bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
        {
            return SubsiteProductProvider.Instance().UpdateSkuUnderlingPrices(productIds, gradeId, basePriceName, operation, price);
        }
    }
}

