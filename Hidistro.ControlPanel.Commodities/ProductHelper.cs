namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.HOP;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class ProductHelper
    {
        public static ProductActionStatus AddProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> tagsId)
        {
            if (null == product)
            {
                return ProductActionStatus.UnknowError;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            product.LowestSalePrice = Math.Round(product.LowestSalePrice, decimalLength);
            ProductActionStatus unknowError = ProductActionStatus.UnknowError;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    ProductProvider provider = ProductProvider.Instance();
                    int productId = provider.AddProduct(product, dbTran);
                    if (productId == 0)
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.DuplicateSKU;
                    }
                    product.ProductId = productId;
                    if (((skus != null) && (skus.Count > 0)) && !provider.AddProductSKUs(productId, skus, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (((attrs != null) && (attrs.Count > 0)) && !provider.AddProductAttributes(productId, attrs, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.AttributeError;
                    }
                    if (((tagsId != null) && (tagsId.Count > 0)) && !provider.AddProductTags(productId, tagsId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    dbTran.Commit();
                    unknowError = ProductActionStatus.Success;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            if (unknowError == ProductActionStatus.Success)
            {
                EventLogs.WriteOperationLog(Privilege.AddProducts, string.Format(CultureInfo.InvariantCulture, "上架了一个新商品:”{0}”", new object[] { product.ProductName }));
            }
            return unknowError;
        }

        public static bool AddProductTags(int productId, IList<int> tagsId, DbTransaction dbtran)
        {
            return ProductProvider.Instance().AddProductTags(productId, tagsId, dbtran);
        }

        public static bool AddRelatedProduct(int productId, int relatedProductId)
        {
            return ProductProvider.Instance().AddRelatedProduct(productId, relatedProductId);
        }

        public static bool AddSkuStock(string productIds, int addStock)
        {
            return ProductProvider.Instance().AddSkuStock(productIds, addStock);
        }

        public static bool AddSubjectProduct(int tagId, int productId)
        {
            IList<int> productIds = new List<int>();
            productIds.Add(productId);
            return ProductProvider.Instance().AddSubjectProducts(tagId, productIds);
        }

        public static bool AddSubjectProducts(int tagId, IList<int> productIds)
        {
            return ProductProvider.Instance().AddSubjectProducts(tagId, productIds);
        }

        public static int CanclePenetrationProducts(string productIds)
        {
            int num;
            ManagerHelper.CheckPrivilege(Privilege.UpPackProduct);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    num = ProductProvider.Instance().CanclePenetrationProducts(productIds, dbTran);
                    if (num <= 0)
                    {
                        dbTran.Rollback();
                        return 0;
                    }
                    if (!ProductProvider.Instance().DeleteCanclePenetrationProducts(productIds, dbTran))
                    {
                        dbTran.Rollback();
                        return 0;
                    }
                    dbTran.Commit();
                }
                catch
                {
                    dbTran.Rollback();
                    return 0;
                }
                finally
                {
                    connection.Close();
                }
                if (num > 0)
                {
                    EventLogs.WriteOperationLog(Privilege.UpPackProduct, string.Format(CultureInfo.InvariantCulture, "对 “{0}” 件商品进行了取消铺货", new object[] { productIds.Split(new char[] { ',' }).Length }));
                }
            }
            return num;
        }

        public static bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
        {
            return ProductProvider.Instance().CheckPrice(productIds, baseGradeId, checkPrice, isMember);
        }

        public static bool ClearRelatedProducts(int productId)
        {
            return ProductProvider.Instance().ClearRelatedProducts(productId);
        }

        public static bool ClearSubjectProducts(int tagId)
        {
            return ProductProvider.Instance().ClearSubjectProducts(tagId);
        }

        private static ProductInfo ConverToProduct(DataRow productRow, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeImages)
        {
            ProductInfo info2 = new ProductInfo();
            info2.CategoryId = categoryId;
            info2.TypeId = new int?((int) productRow["SelectedTypeId"]);
            info2.ProductName = (string) productRow["ProductName"];
            info2.ProductCode = (string) productRow["ProductCode"];
            info2.LineId = lineId;
            info2.BrandId = bandId;
            info2.LowestSalePrice = (decimal) productRow["LowestSalePrice"];
            info2.Unit = (string) productRow["Unit"];
            info2.ShortDescription = (string) productRow["ShortDescription"];
            info2.Description = (string) productRow["Description"];
            info2.PenetrationStatus = PenetrationStatus.Notyet;
            info2.Title = (string) productRow["Title"];
            info2.MetaDescription = (string) productRow["Meta_Description"];
            info2.MetaKeywords = (string) productRow["Meta_Keywords"];
            info2.AddedDate = DateTime.Now;
            info2.SaleStatus = saleStatus;
            info2.HasSKU = (bool) productRow["HasSKU"];
            info2.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
            info2.ImageUrl1 = (string) productRow["ImageUrl1"];
            info2.ImageUrl2 = (string) productRow["ImageUrl2"];
            info2.ImageUrl3 = (string) productRow["ImageUrl3"];
            info2.ImageUrl4 = (string) productRow["ImageUrl4"];
            info2.ImageUrl5 = (string) productRow["ImageUrl5"];
            ProductInfo info = info2;
            if (productRow["MarketPrice"] != DBNull.Value)
            {
                info.MarketPrice = new decimal?((decimal) productRow["MarketPrice"]);
            }
            if (includeImages)
            {
                string[] strArray;
                HttpContext current = HttpContext.Current;
                if (!(string.IsNullOrEmpty(info.ImageUrl1) || (info.ImageUrl1.Length <= 0)))
                {
                    strArray = ProcessImages(current, info.ImageUrl1);
                    info.ThumbnailUrl40 = strArray[0];
                    info.ThumbnailUrl60 = strArray[1];
                    info.ThumbnailUrl100 = strArray[2];
                    info.ThumbnailUrl160 = strArray[3];
                    info.ThumbnailUrl180 = strArray[4];
                    info.ThumbnailUrl220 = strArray[5];
                    info.ThumbnailUrl310 = strArray[6];
                    info.ThumbnailUrl410 = strArray[7];
                }
                if (!(string.IsNullOrEmpty(info.ImageUrl2) || (info.ImageUrl2.Length <= 0)))
                {
                    strArray = ProcessImages(current, info.ImageUrl2);
                }
                if (!(string.IsNullOrEmpty(info.ImageUrl3) || (info.ImageUrl3.Length <= 0)))
                {
                    strArray = ProcessImages(current, info.ImageUrl3);
                }
                if (!(string.IsNullOrEmpty(info.ImageUrl4) || (info.ImageUrl4.Length <= 0)))
                {
                    strArray = ProcessImages(current, info.ImageUrl4);
                }
                if (!(string.IsNullOrEmpty(info.ImageUrl5) || (info.ImageUrl5.Length <= 0)))
                {
                    strArray = ProcessImages(current, info.ImageUrl5);
                }
            }
            return info;
        }

        private static Dictionary<string, SKUItem> ConverToSkus(int mappedProductId, DataSet productData, bool includeCostPrice, bool includeStock)
        {
            DataRow[] rowArray = productData.Tables["skus"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            if (rowArray.Length == 0)
            {
                return null;
            }
            Dictionary<string, SKUItem> dictionary = new Dictionary<string, SKUItem>();
            foreach (DataRow row in rowArray)
            {
                string key = (string) row["NewSkuId"];
                SKUItem item2 = new SKUItem();
                item2.SkuId = key;
                item2.SKU = (string) row["SKU"];
                item2.SalePrice = (decimal) row["SalePrice"];
                item2.PurchasePrice = (decimal) row["PurchasePrice"];
                item2.AlertStock = (int) row["AlertStock"];
                SKUItem item = item2;
                if (row["Weight"] != DBNull.Value)
                {
                    item.Weight = (decimal) row["Weight"];
                }
                if (includeCostPrice && (row["CostPrice"] != DBNull.Value))
                {
                    item.CostPrice = (decimal) row["CostPrice"];
                }
                if (includeStock)
                {
                    item.Stock = (int) row["Stock"];
                }
                DataRow[] rowArray2 = productData.Tables["skuItems"].Select("NewSkuId='" + key + "' AND MappedProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
                foreach (DataRow row2 in rowArray2)
                {
                    item.SkuItems.Add((int) row2["SelectedAttributeId"], (int) row2["SelectedValueId"]);
                }
                dictionary.Add(key, item);
            }
            return dictionary;
        }

        private static Dictionary<int, IList<int>> ConvertToAttributes(int mappedProductId, DataSet productData)
        {
            DataRow[] rowArray = productData.Tables["attributes"].Select("ProductId=" + mappedProductId.ToString(CultureInfo.InvariantCulture));
            if (rowArray.Length == 0)
            {
                return null;
            }
            Dictionary<int, IList<int>> dictionary = new Dictionary<int, IList<int>>();
            foreach (DataRow row in rowArray)
            {
                int key = (int) row["SelectedAttributeId"];
                if (!dictionary.ContainsKey(key))
                {
                    IList<int> list = new List<int>();
                    dictionary.Add(key, list);
                }
                dictionary[key].Add((int) row["SelectedValueId"]);
            }
            return dictionary;
        }

        public static void DeleteNotinProductLines(int distributorUserId)
        {
            ProductProvider.Instance().DeleteNotinProductLines(distributorUserId);
        }

        public static int DeleteProduct(string productIds, bool isDeleteImage)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            string[] strArray = productIds.Split(new char[] { ',' });
            IList<int> list = new List<int>();
            foreach (string str in strArray)
            {
                list.Add(int.Parse(str));
            }
            IList<ProductInfo> products = ProductProvider.Instance().GetProducts(list);
            int num = ProductProvider.Instance().DeleteProduct(productIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProducts, string.Format(CultureInfo.InvariantCulture, "删除了 “{0}” 件商品", new object[] { list.Count }));
                if (isDeleteImage)
                {
                    foreach (ProductInfo info in products)
                    {
                        try
                        {
                            DeleteProductImage(info);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return num;
        }

        private static void DeleteProductImage(ProductInfo product)
        {
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl1))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl1);
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl2))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl2);
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl3))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl3);
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl4))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl4);
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
                if (!string.IsNullOrEmpty(product.ImageUrl5))
                {
                    ResourcesHelper.DeleteImage(product.ImageUrl5);
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs60/60_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs100/100_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs160/160_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs180/180_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs220/220_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs310/310_"));
                    ResourcesHelper.DeleteImage(product.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs410/410_"));
                }
            }
        }

        public static bool DeleteProductTags(int productId, DbTransaction tran)
        {
            return ProductProvider.Instance().DeleteProductTags(productId, tran);
        }

        public static void EnsureMapping(DataSet mappingSet)
        {
            ProductProvider.Instance().EnsureMapping(mappingSet);
        }

        public static DbQueryResult GetBindProducts(ProductQuery query)
        {
            return ProductProvider.Instance().GetBindProducts(query);
        }

        public static DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            return ProductProvider.Instance().GetExportProducts(query, removeProductIds);
        }

        public static DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            DataSet set = ProductProvider.Instance().GetExportProducts(query, includeCostPrice, includeStock, removeProductIds);
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
            return ProductProvider.Instance().GetGroupBuyProducts(query);
        }

        public static int GetMaxSequence()
        {
            return ProductProvider.Instance().GetMaxSequence();
        }

        public static DataTable GetProductBaseInfo(string productIds)
        {
            return ProductProvider.Instance().GetProductBaseInfo(productIds);
        }

        public static ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds, out IList<int> tagsId)
        {
            return ProductProvider.Instance().GetProductDetails(productId, out attrs, out distributorUserIds, out tagsId);
        }

        public static IList<int> GetProductIds(ProductQuery query)
        {
            return ProductProvider.Instance().GetProductIds(query);
        }

        public static string GetProductNameByProductIds(string productId, out int sumcount)
        {
            return ProductProvider.Instance().GetProductNameByProductIds(productId, out sumcount);
        }

        public static string GetProductNamesByLineId(int lineId, out int count)
        {
            return ProductProvider.Instance().GetProductNamesByLineId(lineId, out count);
        }

        public static DbQueryResult GetProducts(ProductQuery query)
        {
            return ProductProvider.Instance().GetProducts(query);
        }

        public static IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            return ProductProvider.Instance().GetProducts(productIds);
        }

        public static DataSet GetProductsByQuery(ProductQuery query, out int totalrecord)
        {
            return ProductProvider.Instance().GetProductsByQuery(query, out totalrecord);
        }

        public static DataSet GetProductSkuDetials(int productId)
        {
            return ProductProvider.Instance().GetProductSkuDetials(productId);
        }

        public static DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetRelatedProducts(page, productId);
        }

        public static DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId)
        {
            return ProductProvider.Instance().GetSkuContentBySkuBuDistorUserId(skuId, distorUserId);
        }

        public static DataTable GetSkuDistributorPrices(string productIds)
        {
            return ProductProvider.Instance().GetSkuDistributorPrices(productIds);
        }

        public static DataTable GetSkuMemberPrices(string productIds)
        {
            return ProductProvider.Instance().GetSkuMemberPrices(productIds);
        }

        public static DataTable GetSkusByProductId(int productId)
        {
            return ProductProvider.Instance().GetSkusByProductId(productId);
        }

        public static DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId)
        {
            return ProductProvider.Instance().GetSkusByProductIdByDistorId(productId, distorUserId);
        }

        public static DataTable GetSkuStocks(string productIds)
        {
            return ProductProvider.Instance().GetSkuStocks(productIds);
        }

        public static IList<int> GetSubjectProductIds(int tagId)
        {
            return ProductProvider.Instance().GetSubjectProductIds(tagId);
        }

        public static DbQueryResult GetSubjectProducts(int tagId, Pagination page)
        {
            return ProductProvider.Instance().GetSubjectProducts(tagId, page);
        }

        public static DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId)
        {
            return ProductProvider.Instance().GetSubmitPuchaseProductsByDistorUserId(query, distorUserId);
        }

        public static DataSet GetTaobaoProductDetails(int productId)
        {
            return ProductProvider.Instance().GetTaobaoProductDetails(productId);
        }

        public static DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            return ProductProvider.Instance().GetUnclassifiedProducts(query);
        }

        public static IList<string> GetUserIdByLineId(int lineId)
        {
            return ProductProvider.Instance().GetUserIdByLineId(lineId);
        }

        public static IList<string> GetUserNameByProductId(string productIds)
        {
            return ProductProvider.Instance().GetUserNameByProductId(productIds);
        }

        public static void ImportProducts(DataTable productData, int categoryId, int lineId, int? brandId, ProductSaleStatus saleStatus, bool isImportFromTaobao)
        {
            if ((productData != null) && (productData.Rows.Count > 0))
            {
                foreach (DataRow row in productData.Rows)
                {
                    string[] strArray;
                    ProductInfo product = new ProductInfo();
                    product.CategoryId = categoryId;
                    product.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
                    product.ProductName = (string) row["ProductName"];
                    product.ProductCode = (string) row["SKU"];
                    product.LineId = lineId;
                    product.BrandId = brandId;
                    if (row["Description"] != DBNull.Value)
                    {
                        product.Description = (string) row["Description"];
                    }
                    product.PenetrationStatus = PenetrationStatus.Notyet;
                    product.AddedDate = DateTime.Now;
                    product.SaleStatus = saleStatus;
                    product.HasSKU = false;
                    HttpContext current = HttpContext.Current;
                    if (row["ImageUrl1"] != DBNull.Value)
                    {
                        product.ImageUrl1 = (string) row["ImageUrl1"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl1) || (product.ImageUrl1.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl1);
                        product.ThumbnailUrl40 = strArray[0];
                        product.ThumbnailUrl60 = strArray[1];
                        product.ThumbnailUrl100 = strArray[2];
                        product.ThumbnailUrl160 = strArray[3];
                        product.ThumbnailUrl180 = strArray[4];
                        product.ThumbnailUrl220 = strArray[5];
                        product.ThumbnailUrl310 = strArray[6];
                        product.ThumbnailUrl410 = strArray[7];
                    }
                    if (row["ImageUrl2"] != DBNull.Value)
                    {
                        product.ImageUrl2 = (string) row["ImageUrl2"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl2) || (product.ImageUrl2.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl2);
                    }
                    if (row["ImageUrl3"] != DBNull.Value)
                    {
                        product.ImageUrl3 = (string) row["ImageUrl3"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl3) || (product.ImageUrl3.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl3);
                    }
                    if (row["ImageUrl4"] != DBNull.Value)
                    {
                        product.ImageUrl4 = (string) row["ImageUrl4"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl4) || (product.ImageUrl4.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl4);
                    }
                    if (row["ImageUrl5"] != DBNull.Value)
                    {
                        product.ImageUrl5 = (string) row["ImageUrl5"];
                    }
                    if (!(string.IsNullOrEmpty(product.ImageUrl5) || (product.ImageUrl5.Length <= 0)))
                    {
                        strArray = ProcessImages(current, product.ImageUrl5);
                    }
                    SKUItem item = new SKUItem();
                    item.SkuId = "_0";
                    item.SKU = (string) row["SKU"];
                    product.LowestSalePrice = item.PurchasePrice = item.SalePrice = (decimal) row["SalePrice"];
                    if (row["Stock"] != DBNull.Value)
                    {
                        item.Stock = (int) row["Stock"];
                    }
                    if (row["Weight"] != DBNull.Value)
                    {
                        item.Weight = (decimal) row["Weight"];
                    }
                    Dictionary<string, SKUItem> skus = new Dictionary<string, SKUItem>();
                    skus.Add(item.SkuId, item);
                    ProductActionStatus status = AddProduct(product, skus, null, null);
                    if (isImportFromTaobao && (status == ProductActionStatus.Success))
                    {
                        TaobaoProductInfo taobaoProduct = new TaobaoProductInfo();
                        taobaoProduct.ProductId = product.ProductId;
                        taobaoProduct.ProTitle = product.ProductName;
                        taobaoProduct.Cid = (long) row["Cid"];
                        if (row["StuffStatus"] != DBNull.Value)
                        {
                            taobaoProduct.StuffStatus = (string) row["StuffStatus"];
                        }
                        taobaoProduct.Num = (long) row["Num"];
                        taobaoProduct.LocationState = (string) row["LocationState"];
                        taobaoProduct.LocationCity = (string) row["LocationCity"];
                        taobaoProduct.FreightPayer = (string) row["FreightPayer"];
                        if (row["PostFee"] != DBNull.Value)
                        {
                            taobaoProduct.PostFee = (decimal) row["PostFee"];
                        }
                        if (row["ExpressFee"] != DBNull.Value)
                        {
                            taobaoProduct.ExpressFee = (decimal) row["ExpressFee"];
                        }
                        if (row["EMSFee"] != DBNull.Value)
                        {
                            taobaoProduct.EMSFee = (decimal) row["EMSFee"];
                        }
                        taobaoProduct.HasInvoice = (bool) row["HasInvoice"];
                        taobaoProduct.HasWarranty = (bool) row["HasWarranty"];
                        taobaoProduct.HasDiscount = (bool) row["HasDiscount"];
                        taobaoProduct.ValidThru = (long) row["ValidThru"];
                        if (row["ListTime"] != DBNull.Value)
                        {
                            taobaoProduct.ListTime = (DateTime) row["ListTime"];
                        }
                        else
                        {
                            taobaoProduct.ListTime = DateTime.Now;
                        }
                        if (row["PropertyAlias"] != DBNull.Value)
                        {
                            taobaoProduct.PropertyAlias = (string) row["PropertyAlias"];
                        }
                        if (row["InputPids"] != DBNull.Value)
                        {
                            taobaoProduct.InputPids = (string) row["InputPids"];
                        }
                        if (row["InputStr"] != DBNull.Value)
                        {
                            taobaoProduct.InputStr = (string) row["InputStr"];
                        }
                        if (row["SkuProperties"] != DBNull.Value)
                        {
                            taobaoProduct.SkuProperties = (string) row["SkuProperties"];
                        }
                        if (row["SkuQuantities"] != DBNull.Value)
                        {
                            taobaoProduct.SkuQuantities = (string) row["SkuQuantities"];
                        }
                        if (row["SkuPrices"] != DBNull.Value)
                        {
                            taobaoProduct.SkuPrices = (string) row["SkuPrices"];
                        }
                        if (row["SkuOuterIds"] != DBNull.Value)
                        {
                            taobaoProduct.SkuOuterIds = (string) row["SkuOuterIds"];
                        }
                        UpdateToaobProduct(taobaoProduct);
                    }
                }
            }
        }

        public static void ImportProducts(DataSet productData, int categoryId, int lineId, int? bandId, ProductSaleStatus saleStatus, bool includeCostPrice, bool includeStock, bool includeImages)
        {
            foreach (DataRow row in productData.Tables["products"].Rows)
            {
                int mappedProductId = (int) row["ProductId"];
                ProductInfo product = ConverToProduct(row, categoryId, lineId, bandId, saleStatus, includeImages);
                Dictionary<string, SKUItem> skus = ConverToSkus(mappedProductId, productData, includeCostPrice, includeStock);
                Dictionary<int, IList<int>> attrs = ConvertToAttributes(mappedProductId, productData);
                ProductActionStatus status = AddProduct(product, skus, attrs, null);
            }
        }

        public static int InStock(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.InStockProduct);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnStock);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量入库了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public static bool IsExitTaobaoProduct(long taobaoProductId)
        {
            return ProductProvider.Instance().IsExitTaobaoProduct(taobaoProductId);
        }

        public static int OffShelf(int productId)
        {
            if (productId <= 0)
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productId.ToString(), ProductSaleStatus.UnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public static int OffShelf(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.OffShelfProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.UnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量下架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public static int PenetrationProducts(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.PackProduct);
            int num = ProductProvider.Instance().PenetrationProducts(productIds);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.PackProduct, string.Format(CultureInfo.InvariantCulture, "对 “{0}” 件商品进行了铺货", new object[] { num }));
            }
            return num;
        }

        private static string[] ProcessImages(HttpContext context, string originalSavePath)
        {
            string fileName = Path.GetFileName(originalSavePath);
            string str2 = "/Storage/master/product/thumbs40/40_" + fileName;
            string str3 = "/Storage/master/product/thumbs60/60_" + fileName;
            string str4 = "/Storage/master/product/thumbs100/100_" + fileName;
            string str5 = "/Storage/master/product/thumbs160/160_" + fileName;
            string str6 = "/Storage/master/product/thumbs180/180_" + fileName;
            string str7 = "/Storage/master/product/thumbs220/220_" + fileName;
            string str8 = "/Storage/master/product/thumbs310/310_" + fileName;
            string str9 = "/Storage/master/product/thumbs410/410_" + fileName;
            string path = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
            if (File.Exists(path))
            {
                try
                {
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str2), 40, 40);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str3), 60, 60);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str4), 100, 100);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str5), 160, 160);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str6), 180, 180);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str7), 220, 220);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str8), 310, 310);
                    ResourcesHelper.CreateThumbnail(path, context.Request.MapPath(Globals.ApplicationPath + str9), 410, 410);
                }
                catch
                {
                }
            }
            return new string[] { str2, str3, str4, str5, str6, str7, str8, str9 };
        }

        public static int RemoveProduct(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.Delete);
            if (num > 0)
            {
                ProductProvider.Instance().CanclePenetrationProducts(productIds, null);
                EventLogs.WriteOperationLog(Privilege.OffShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量删除了 “{0}” 件商品到回收站", new object[] { num }));
            }
            return num;
        }

        public static bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            return ProductProvider.Instance().RemoveRelatedProduct(productId, relatedProductId);
        }

        public static bool RemoveSubjectProduct(int tagId, int productId)
        {
            return ProductProvider.Instance().RemoveSubjectProduct(tagId, productId);
        }

        public static bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            return ProductProvider.Instance().ReplaceProductNames(productIds, oldWord, newWord);
        }

        public static ProductActionStatus UpdateProduct(ProductInfo product, Dictionary<string, SKUItem> skus, Dictionary<int, IList<int>> attrs, IList<int> distributorUserIds, IList<int> tagIds)
        {
            if (null == product)
            {
                return ProductActionStatus.UnknowError;
            }
            Globals.EntityCoding(product, true);
            int decimalLength = HiContext.Current.SiteSettings.DecimalLength;
            if (product.MarketPrice.HasValue)
            {
                product.MarketPrice = new decimal?(Math.Round(product.MarketPrice.Value, decimalLength));
            }
            product.LowestSalePrice = Math.Round(product.LowestSalePrice, decimalLength);
            ProductActionStatus unknowError = ProductActionStatus.UnknowError;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    ProductProvider provider = ProductProvider.Instance();
                    if (!provider.UpdateProduct(product, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.DuplicateSKU;
                    }
                    if (!provider.DeleteProductSKUS(product.ProductId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (((skus != null) && (skus.Count > 0)) && !provider.AddProductSKUs(product.ProductId, skus, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.SKUError;
                    }
                    if (!provider.AddProductAttributes(product.ProductId, attrs, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.AttributeError;
                    }
                    if (!provider.OffShelfProductExcludedSalePrice(product.ProductId, product.LowestSalePrice, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.OffShelfError;
                    }
                    if (!provider.DeleteProductTags(product.ProductId, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    if ((tagIds.Count > 0) && !provider.AddProductTags(product.ProductId, tagIds, dbTran))
                    {
                        dbTran.Rollback();
                        return ProductActionStatus.ProductTagEroor;
                    }
                    dbTran.Commit();
                    unknowError = ProductActionStatus.Success;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            if (unknowError == ProductActionStatus.Success)
            {
                ProductProvider.Instance().DeleteSkuUnderlingPrice();
                if (product.PenetrationStatus == PenetrationStatus.Notyet)
                {
                    ProductProvider.Instance().CanclePenetrationProducts(product.ProductId.ToString(), null);
                }
                if ((distributorUserIds != null) && (distributorUserIds.Count != 0))
                {
                    foreach (int num2 in distributorUserIds)
                    {
                        DeleteNotinProductLines(num2);
                    }
                }
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的商品", new object[] { product.ProductId }));
            }
            return unknowError;
        }

        public static bool UpdateProductBaseInfo(DataTable dt)
        {
            if ((dt == null) || (dt.Rows.Count <= 0))
            {
                return false;
            }
            return ProductProvider.Instance().UpdateProductBaseInfo(dt);
        }

        public static bool UpdateProductCategory(int productId, int newCategoryId)
        {
            bool flag;
            if (newCategoryId != 0)
            {
                flag = ProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, CatalogHelper.GetCategory(newCategoryId).Path + "|");
            }
            else
            {
                flag = ProductProvider.Instance().UpdateProductCategory(productId, newCategoryId, null);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProducts, string.Format(CultureInfo.InvariantCulture, "修改编号 “{0}” 的店铺分类为 “{1}”", new object[] { productId, newCategoryId }));
            }
            return flag;
        }

        public static bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            return ProductProvider.Instance().UpdateProductNames(productIds, prefix, suffix);
        }

        public static ApiErrorCode UpdateProductStock(int productId, string skuId, string sku, int type, int stock)
        {
            DataRow[] rowArray;
            if ((productId <= 0) || ((type == 1) && (stock <= 0)))
            {
                return ApiErrorCode.Format_Eroor;
            }
            skuId = DataHelper.CleanSearchString(skuId);
            sku = DataHelper.CleanSearchString(sku);
            DataTable skuStocks = ProductProvider.Instance().GetSkuStocks(productId.ToString());
            int num = 0;
            string key = "";
            bool flag = false;
            bool flag2 = false;
            if (skuStocks.Rows.Count <= 0)
            {
                return ApiErrorCode.Exists_Error;
            }
            num = Convert.ToInt32(skuStocks.Rows[0]["Stock"]);
            if (!string.IsNullOrEmpty(skuId))
            {
                rowArray = skuStocks.Select("SkuId='" + skuId + "'");
                if (rowArray.Length <= 0)
                {
                    return ApiErrorCode.Exists_Error;
                }
                num = Convert.ToInt32(rowArray[0]["Stock"]);
                key = skuId;
                flag2 = true;
            }
            if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(skuId))
            {
                rowArray = skuStocks.Select("SKU='" + sku + "'");
                if (rowArray.Length <= 0)
                {
                    return ApiErrorCode.Exists_Error;
                }
                num = Convert.ToInt32(rowArray[0]["Stock"]);
                key = rowArray[0]["SkuId"].ToString();
                flag2 = true;
            }
            if (type != 1)
            {
                if ((num + stock) <= 0)
                {
                    stock = 0;
                }
                else
                {
                    stock += num;
                }
            }
            if (!flag2)
            {
                flag = ProductProvider.Instance().UpdateSkuStock(productId.ToString(), stock);
            }
            else
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add(key, stock);
                flag = ProductProvider.Instance().UpdateSkuStock(dictionary);
            }
            if (flag)
            {
                return ApiErrorCode.Success;
            }
            return ApiErrorCode.Unknown_Error;
        }

        public static bool UpdateShowSaleCounts(DataTable dt)
        {
            if ((dt == null) || (dt.Rows.Count <= 0))
            {
                return false;
            }
            return ProductProvider.Instance().UpdateShowSaleCounts(dt);
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            return ProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts);
        }

        public static bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            return ProductProvider.Instance().UpdateShowSaleCounts(productIds, showSaleCounts, operation);
        }

        public static bool UpdateSkuDistributorPrices(DataSet ds)
        {
            return ProductProvider.Instance().UpdateSkuDistributorPrices(ds);
        }

        public static bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price)
        {
            return ProductProvider.Instance().UpdateSkuDistributorPrices(productIds, gradeId, price);
        }

        public static bool UpdateSkuDistributorPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            return ProductProvider.Instance().UpdateSkuDistributorPrices(productIds, gradeId, baseGradeId, operation, price);
        }

        public static bool UpdateSkuMemberPrices(DataSet ds)
        {
            return ProductProvider.Instance().UpdateSkuMemberPrices(ds);
        }

        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            return ProductProvider.Instance().UpdateSkuMemberPrices(productIds, gradeId, price);
        }

        public static bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            return ProductProvider.Instance().UpdateSkuMemberPrices(productIds, gradeId, baseGradeId, operation, price);
        }

        public static bool UpdateSkuStock(Dictionary<string, int> skuStocks)
        {
            return ProductProvider.Instance().UpdateSkuStock(skuStocks);
        }

        public static bool UpdateSkuStock(string productIds, int stock)
        {
            return ProductProvider.Instance().UpdateSkuStock(productIds, stock);
        }

        public static bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            return ProductProvider.Instance().UpdateToaobProduct(taobaoProduct);
        }

        public static string UploadDefaltProductImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static int UpShelf(int productId)
        {
            if (productId <= 0)
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productId.ToString(), ProductSaleStatus.OnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }

        public static int UpShelf(string productIds)
        {
            ManagerHelper.CheckPrivilege(Privilege.UpShelfProducts);
            if (string.IsNullOrEmpty(productIds))
            {
                return 0;
            }
            int num = ProductProvider.Instance().UpdateProductSaleStatus(productIds, ProductSaleStatus.OnSale);
            if (num > 0)
            {
                EventLogs.WriteOperationLog(Privilege.UpShelfProducts, string.Format(CultureInfo.InvariantCulture, "批量上架了 “{0}” 件商品", new object[] { num }));
            }
            return num;
        }
    }
}

