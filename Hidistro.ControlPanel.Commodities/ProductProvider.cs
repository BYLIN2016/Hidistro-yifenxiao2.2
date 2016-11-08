namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.HOP;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using Hidistro.Core;

    public abstract class ProductProvider
    {
        private static readonly ProductProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.ProductData,Hidistro.ControlPanel.Data") as ProductProvider);

        protected ProductProvider()
        {
        }

        public abstract bool AddAttribute(AttributeInfo attribute);
        public abstract int AddAttributeName(AttributeInfo attribute);
        public abstract int AddAttributeValue(AttributeValueInfo attributeValue);
        public abstract int AddBrandCategory(BrandCategoryInfo brandCategory);
        public abstract void AddBrandProductTypes(int brandId, IList<int> productTypes);
        public abstract int AddProduct(ProductInfo product, DbTransaction dbTran);
        public abstract bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, DbTransaction dbTran);
        public abstract bool AddProductLine(ProductLineInfo productLine);
        public abstract bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, DbTransaction dbTran);
        public abstract bool AddProductTags(int productId, IList<int> tagIds, DbTransaction tran);
        public abstract int AddProductType(ProductTypeInfo productType);
        public abstract void AddProductTypeBrands(int typeId, IList<int> brands);
        public abstract bool AddRelatedProduct(int productId, int relatedProductId);
        public abstract bool AddSkuStock(string productIds, int addStock);
        public abstract bool AddSubjectProducts(int tagId, IList<int> productIds);
        public abstract bool AddSupplier(string supplierName, string remark);
        public abstract int AddTags(string tagname);
        public abstract bool BrandHvaeProducts(int brandId);
        public abstract int CanclePenetrationProducts(string productIds, DbTransaction dbTran);
        public abstract bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember);
        public abstract bool ClearAttributeValue(int attributeId);
        public abstract bool ClearRelatedProducts(int productId);
        public abstract bool ClearSubjectProducts(int tagId);
        public abstract int CreateCategory(CategoryInfo category);
        public abstract bool DeleteAttribute(int attributeId);
        public abstract bool DeleteAttribute(int attributeId, int valueId);
        public abstract bool DeleteAttributeValue(int attributeValueId);
        public abstract bool DeleteBrandCategory(int brandId);
        public abstract bool DeleteBrandProductTypes(int brandId);
        public abstract bool DeleteCanclePenetrationProducts(string productIds, DbTransaction dbTran);
        public abstract bool DeleteCategory(int categoryId);
        public abstract void DeleteNotinProductLines(int distributorUserId);
        public abstract int DeleteProduct(string productIds);
        public abstract bool DeleteProductLine(int lineId);
        public abstract bool DeleteProductSKUS(int productId, DbTransaction dbTran);
        public abstract bool DeleteProductTags(int productId, DbTransaction tran);
        public abstract bool DeleteProductTypeBrands(int typeId);
        public abstract bool DeleteProducType(int typeId);
        public abstract void DeleteSkuUnderlingPrice();
        public abstract void DeleteSupplier(string supplierName);
        public abstract bool DeleteTags(int tagId);
        public abstract int DisplaceCategory(int oldCategoryId, int newCategory);
        public abstract void EnsureMapping(DataSet mappingSet);
        public abstract AttributeInfo GetAttribute(int attributeId);
        public abstract IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode);
        public abstract IList<AttributeInfo> GetAttributes(int typeId);
        public abstract IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode);
        public abstract AttributeValueInfo GetAttributeValueInfo(int valueId);
        public abstract DbQueryResult GetBindProducts(ProductQuery query);
        public abstract DataTable GetBrandCategories();
        public abstract DataTable GetBrandCategories(string brandName);
        public abstract DataTable GetBrandCategoriesByTypeId(int typeId);
        public abstract BrandCategoryInfo GetBrandCategory(int brandId);
        public abstract DataTable GetCategories();
        public abstract CategoryInfo GetCategory(int categoryId);
        public abstract DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds);
        public abstract DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds);
        public abstract DataTable GetGroupBuyProducts(ProductQuery query);
        public abstract int GetMaxSequence();
        public abstract DataTable GetProductBaseInfo(string productIds);
        public abstract ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds, out IList<int> tagsId);
        public abstract IList<int> GetProductIds(ProductQuery query);
        public abstract ProductLineInfo GetProductLine(int lineId);
        public abstract IList<ProductLineInfo> GetProductLineList();
        public abstract DataTable GetProductLines();
        public abstract string GetProductNameByProductIds(string productIds, out int sumcount);
        public abstract string GetProductNamesByLineId(int lineId, out int count);
        public abstract DbQueryResult GetProducts(ProductQuery query);
        public abstract IList<ProductInfo> GetProducts(IList<int> productIds);
        public abstract DataSet GetProductsByQuery(ProductQuery query, out int totalrecord);
        public abstract DataSet GetProductSkuDetials(int productId);
        public abstract ProductTypeInfo GetProductType(int typeId);
        public abstract IList<ProductTypeInfo> GetProductTypes();
        public abstract DbQueryResult GetProductTypes(ProductTypeQuery query);
        public abstract DbQueryResult GetRelatedProducts(Pagination page, int productId);
        public abstract DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId);
        public abstract DataTable GetSkuDistributorPrices(string productIds);
        public abstract DataTable GetSkuMemberPrices(string productIds);
        public abstract DataTable GetSkusByProductId(int productId);
        public abstract DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId);
        public abstract DataTable GetSkuStocks(string productIds);
        public abstract int GetSpecificationId(int typeId, string specificationName);
        public abstract int GetSpecificationValueId(int attributeId, string valueStr);
        public abstract IList<int> GetSubjectProductIds(int tagId);
        public abstract DbQueryResult GetSubjectProducts(int tagId, Pagination page);
        public abstract DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId);
        public abstract string GetSupplierRemark(string supplierName);
        public abstract IList<string> GetSuppliers();
        public abstract DbQueryResult GetSuppliers(Pagination page);
        public abstract string GetTagName(int tagId);
        public abstract DataTable GetTags();
        public abstract int GetTags(string tagName);
        public abstract DataSet GetTaobaoProductDetails(int productId);
        public abstract int GetTypeId(string typeName);
        public abstract DbQueryResult GetUnclassifiedProducts(ProductQuery query);
        public abstract IList<string> GetUserIdByLineId(int lineId);
        public abstract IList<string> GetUserNameByProductId(string productIds);
        public static ProductProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool IsExitTaobaoProduct(long taobaoProductId);
        public abstract bool OffShelfProductExcludedSalePrice(int productId, decimal lowestSalePrice, DbTransaction dbTran);
        public abstract int PenetrationProducts(string productIds);
        public abstract bool RemoveRelatedProduct(int productId, int relatedProductId);
        public abstract bool RemoveSubjectProduct(int tagId, int productId);
        public abstract bool ReplaceProductLine(int fromlineId, int replacelineId);
        public abstract bool ReplaceProductNames(string productIds, string oldWord, string newWord);
        public abstract bool SetBrandCategoryThemes(int brandid, string themeName);
        public abstract bool SetCategoryThemes(int categoryId, string themeName);
        public abstract bool SetProductExtendCategory(int productId, string extendCategoryPath);
        public abstract void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence);
        public abstract bool SwapCategorySequence(int categoryId, int displaysequence);
        public abstract bool UpdateAttribute(AttributeInfo attribute);
        public abstract bool UpdateAttributeName(AttributeInfo attribute);
        public abstract bool UpdateAttributeValue(int attributeId, int valueId, string newValue);
        public abstract bool UpdateBrandCategory(BrandCategoryInfo brandCategory);
        public abstract void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action);
        public abstract bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence);
        public abstract CategoryActionStatus UpdateCategory(CategoryInfo category);
        public abstract bool UpdateProduct(ProductInfo product, DbTransaction dbTran);
        public abstract bool UpdateProductBaseInfo(DataTable dt);
        public abstract bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath);
        public abstract bool UpdateProductLine(ProductLineInfo productLine);
        public abstract bool UpdateProductLine(int replacelineId, int productId);
        public abstract bool UpdateProductNames(string productIds, string prefix, string suffix);
        public abstract int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus);
        public abstract bool UpdateProductType(ProductTypeInfo productType);
        public abstract bool UpdateShowSaleCounts(DataTable dt);
        public abstract bool UpdateShowSaleCounts(string productIds, int showSaleCounts);
        public abstract bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation);
        public abstract bool UpdateSku(AttributeValueInfo attributeValue);
        public abstract bool UpdateSkuDistributorPrices(DataSet ds);
        public abstract bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price);
        public abstract bool UpdateSkuDistributorPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price);
        public abstract bool UpdateSkuMemberPrices(DataSet ds);
        public abstract bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price);
        public abstract bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price);
        public abstract bool UpdateSkuStock(Dictionary<string, int> skuStocks);
        public abstract bool UpdateSkuStock(string productIds, int stock);
        public abstract bool UpdateSpecification(AttributeInfo attribute);
        public abstract bool UpdateSupplier(string oldName, string newName, string remark);
        public abstract bool UpdateTags(int tagId, string tagName);
        public abstract bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct);
    }
}

