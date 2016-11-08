namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.HOP;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ProductData : ProductProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((attribute.AttributeValues.Count != 0) && (obj2 != null))
            {
                int num = Convert.ToInt32(obj2);
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    this.database.AddInParameter(command, "AttributeId", DbType.Int32, num);
                    this.database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    this.database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.ExecuteNonQuery(command);
                }
            }
            return (obj2 != null);
        }

        public override int AddAttributeName(AttributeInfo attribute)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2);
            }
            return num;
        }

        public override int AddAttributeValue(AttributeValueInfo attributeValue)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeValue.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public override int AddBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BrandCategories;INSERT INTO Hishop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override void AddBrandProductTypes(int brandId, IList<int> productTypes)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            foreach (int num in productTypes)
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductTypeId", num);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override int AddProduct(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Create");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            this.database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            this.database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            this.database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            this.database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            this.database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            this.database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int) product.SaleStatus);
            this.database.AddInParameter(storedProcCommand, "AddedDate", DbType.DateTime, product.AddedDate);
            this.database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            this.database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            this.database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            this.database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            this.database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            this.database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, product.LineId);
            this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            this.database.AddInParameter(storedProcCommand, "LowestSalePrice", DbType.Currency, product.LowestSalePrice);
            this.database.AddInParameter(storedProcCommand, "PenetrationStatus", DbType.Int16, (int) product.PenetrationStatus);
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            this.database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            this.database.AddInParameter(storedProcCommand, "TaobaoProductId", DbType.Int64, product.TaobaoProductId);
            this.database.AddOutParameter(storedProcCommand, "ProductId", DbType.Int32, 4);
            this.database.ExecuteNonQuery(storedProcCommand, dbTran);
            return (int) this.database.GetParameterValue(storedProcCommand, "ProductId");
        }

        public override bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, DbTransaction dbTran)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM Hishop_ProductAttributes WHERE ProductId = {0};", productId);
            int num = 0;
            if (attributes != null)
            {
                foreach (int num2 in attributes.Keys)
                {
                    foreach (int num3 in attributes[num2])
                    {
                        num++;
                        builder.AppendFormat(" INSERT INTO Hishop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, num2, num3);
                    }
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (dbTran == null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
        }

        public override bool AddProductLine(ProductLineInfo productLine)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductLines(Name, SupplierName) VALUES(@Name, @SupplierName)");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, productLine.Name);
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, productLine.SupplierName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUs(SkuId, ProductId, SKU, Weight, Stock, AlertStock, CostPrice, SalePrice, PurchasePrice) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock, @AlertStock, @CostPrice, @SalePrice, @PurchasePrice)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String);
            this.database.AddInParameter(sqlStringCommand, "Weight", DbType.Decimal);
            this.database.AddInParameter(sqlStringCommand, "Stock", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "AlertStock", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "CostPrice", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand, "SalePrice", DbType.Currency);
            this.database.AddInParameter(sqlStringCommand, "PurchasePrice", DbType.Currency);
            DbCommand command = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
            this.database.AddInParameter(command, "SkuId", DbType.String);
            this.database.AddInParameter(command, "AttributeId", DbType.Int32);
            this.database.AddInParameter(command, "ValueId", DbType.Int32);
            DbCommand command3 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
            this.database.AddInParameter(command3, "SkuId", DbType.String);
            this.database.AddInParameter(command3, "GradeId", DbType.Int32);
            this.database.AddInParameter(command3, "MemberSalePrice", DbType.Currency);
            DbCommand command4 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUDistributorPrice(SkuId, GradeId, DistributorPurchasePrice) VALUES(@SkuId, @GradeId, @DistributorPurchasePrice)");
            this.database.AddInParameter(command4, "SkuId", DbType.String);
            this.database.AddInParameter(command4, "GradeId", DbType.Int32);
            this.database.AddInParameter(command4, "DistributorPurchasePrice", DbType.Currency);
            try
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
                foreach (SKUItem item in skus.Values)
                {
                    string str = productId.ToString(CultureInfo.InvariantCulture) + "_" + item.SkuId;
                    this.database.SetParameterValue(sqlStringCommand, "SkuId", str);
                    this.database.SetParameterValue(sqlStringCommand, "SKU", item.SKU);
                    this.database.SetParameterValue(sqlStringCommand, "Weight", item.Weight);
                    this.database.SetParameterValue(sqlStringCommand, "Stock", item.Stock);
                    this.database.SetParameterValue(sqlStringCommand, "AlertStock", item.AlertStock);
                    this.database.SetParameterValue(sqlStringCommand, "CostPrice", item.CostPrice);
                    this.database.SetParameterValue(sqlStringCommand, "SalePrice", Math.Round(item.SalePrice, HiContext.Current.SiteSettings.DecimalLength));
                    this.database.SetParameterValue(sqlStringCommand, "PurchasePrice", item.PurchasePrice);
                    if (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
                    {
                        return false;
                    }
                    this.database.SetParameterValue(command, "SkuId", str);
                    foreach (int num in item.SkuItems.Keys)
                    {
                        this.database.SetParameterValue(command, "AttributeId", num);
                        this.database.SetParameterValue(command, "ValueId", item.SkuItems[num]);
                        this.database.ExecuteNonQuery(command, dbTran);
                    }
                    this.database.SetParameterValue(command3, "SkuId", str);
                    foreach (int num2 in item.MemberPrices.Keys)
                    {
                        this.database.SetParameterValue(command3, "GradeId", num2);
                        this.database.SetParameterValue(command3, "MemberSalePrice", item.MemberPrices[num2]);
                        this.database.ExecuteNonQuery(command3, dbTran);
                    }
                    this.database.SetParameterValue(command4, "SkuId", str);
                    foreach (int num2 in item.DistributorPrices.Keys)
                    {
                        this.database.SetParameterValue(command4, "GradeId", num2);
                        this.database.SetParameterValue(command4, "DistributorPurchasePrice", item.DistributorPrices[num2]);
                        this.database.ExecuteNonQuery(command4, dbTran);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool AddProductTags(int productId, IList<int> tagIds, DbTransaction tran)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag VALUES(@TagId,@ProductId)");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            foreach (int num in tagIds)
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
                this.database.SetParameterValue(sqlStringCommand, "TagId", num);
                if (tran != null)
                {
                    flag = this.database.ExecuteNonQuery(sqlStringCommand, tran) > 0;
                }
                else
                {
                    flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
                if (!flag)
                {
                    return flag;
                }
            }
            return flag;
        }

        public override int AddProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return Convert.ToInt32(obj2);
            }
            return 0;
        }

        public override void AddProductTypeBrands(int typeId, IList<int> brands)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32);
            foreach (int num in brands)
            {
                this.database.SetParameterValue(sqlStringCommand, "BrandId", num);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override bool AddRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedProducts(ProductId, RelatedProductId) VALUES (@ProductId, @RelatedProductId)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            try
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch
            {
                return false;
            }
        }

        public override bool AddSkuStock(string productIds, int addStock)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddSubjectProducts(int tagId, IList<int> productIds)
        {
            if (productIds.Count <= 0)
            {
                return false;
            }
            foreach (int num in productIds)
            {
                this.RemoveSubjectProduct(tagId, num);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag(TagId, ProductId) VALUES (@TagId, @ProductId)");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, tagId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            try
            {
                foreach (int num2 in productIds)
                {
                    this.database.SetParameterValue(sqlStringCommand, "ProductId", num2);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool AddSupplier(string supplierName, string remark)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF (SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)) = 0 INSERT INTO Hishop_Suppliers(SupplierName, Remark) VALUES(@SupplierName, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int AddTags(string tagname)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Tags VALUES(@TagName);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagname);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public override bool BrandHvaeProducts(int brandId)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    flag = reader.GetInt32(0) > 0;
                }
            }
            return flag;
        }

        private static string BuildProductQuery(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT p.ProductId FROM Hishop_Products p WHERE p.SaleStatus = {0}", (int) query.SaleStatus);
            if (!(string.IsNullOrEmpty(query.ProductCode) || (query.ProductCode.Length <= 0)))
            {
                builder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                builder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            }
            if (query.CategoryId.HasValue)
            {
                builder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY p.{0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public override int CanclePenetrationProducts(string productIds, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET PenetrationStatus = 2 WHERE ProductId IN (" + productIds + ") ;delete from Hishop_PurchaseShoppingCarts where productid in (" + productIds + ")");
            if (dbTran != null)
            {
                return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
            }
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
        {
            StringBuilder builder = new StringBuilder(" ");
            if (baseGradeId == -2)
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND CostPrice - {1} < 0", productIds, checkPrice);
            }
            else if (baseGradeId == -3)
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND SalePrice - {1} < 0", productIds, checkPrice);
            }
            else if (baseGradeId == -4)
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND PurchasePrice - {1} < 0", productIds, checkPrice);
            }
            else if (isMember)
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE MemberSalePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
            }
            else
            {
                builder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE DistributorPurchasePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ClearAttributeValue(int attributeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearRelatedProducts(int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ClearSubjectProducts(int tagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagId = @TagId");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, tagId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int CreateCategory(CategoryInfo category)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
            this.database.AddOutParameter(storedProcCommand, "CategoryId", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(storedProcCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Int32, category.DisplaySequence);
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Title", DbType.String, category.MetaTitle);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, category.MetaDescription);
            }
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            }
            if (!string.IsNullOrEmpty(category.Notes1))
            {
                this.database.AddInParameter(storedProcCommand, "Notes1", DbType.String, category.Notes1);
            }
            if (!string.IsNullOrEmpty(category.Notes2))
            {
                this.database.AddInParameter(storedProcCommand, "Notes2", DbType.String, category.Notes2);
            }
            if (!string.IsNullOrEmpty(category.Notes3))
            {
                this.database.AddInParameter(storedProcCommand, "Notes3", DbType.String, category.Notes3);
            }
            if (!string.IsNullOrEmpty(category.Notes4))
            {
                this.database.AddInParameter(storedProcCommand, "Notes4", DbType.String, category.Notes4);
            }
            if (!string.IsNullOrEmpty(category.Notes5))
            {
                this.database.AddInParameter(storedProcCommand, "Notes5", DbType.String, category.Notes5);
            }
            if (category.ParentCategoryId.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, category.ParentCategoryId.Value);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "ParentCategoryId", DbType.Int32, 0);
            }
            if (category.AssociatedProductType.HasValue)
            {
                this.database.AddInParameter(storedProcCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType.Value);
            }
            if (!string.IsNullOrEmpty(category.RewriteName))
            {
                this.database.AddInParameter(storedProcCommand, "RewriteName", DbType.String, category.RewriteName);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (int) this.database.GetParameterValue(storedProcCommand, "CategoryId");
        }

        public override bool DeleteAttribute(int attributeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteAttribute(int attributeId, int valueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId=@ValueId AND AttributeId=@AttributeId;DELETE FROM Hishop_ProductAttributes WHERE AttributeId=@AttributeId AND ValueId=@ValueId");
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteAttributeValue(int attributeValueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId");
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValueId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteBrandCategory(int brandId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DeleteBrandProductTypes(int brandId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteCanclePenetrationProducts(string productIds, DbTransaction dbTran)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Products WHERE ProductId IN (" + productIds + ")");
                if (dbTran != null)
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                }
                else
                {
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteCategory(int categoryId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override void DeleteNotinProductLines(int distributorUserId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Products WHERE DistributorUserId=@DistributorUserId AND LineId NOT IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId=@DistributorUserId)");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, distributorUserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteProduct(string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Products WHERE ProductId IN ({0}); DELETE FROM Hishop_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0});DELETE FROM Hishop_ProductTag WHERE ProductId IN ({0})", productIds) + string.Format(" DELETE FROM distro_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0})", productIds) + string.Format(" DELETE FROM distro_SKUMemberPrice WHERE SkuId NOT IN (SELECT SkuId FROM Hishop_Skus); DELETE FROM Hishop_BundlingProductItems WHERE ProductID IN ({0})", productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteProductLine(int lineId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductLine_Delete");
            this.database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, lineId);
            return (this.database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override bool DeleteProductSKUS(int productId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_SKUs WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            try
            {
                if (dbTran == null)
                {
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
                else
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteProductTags(int productId, DbTransaction tran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            if (tran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
        }

        public override bool DeleteProductTypeBrands(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool DeleteProducType(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void DeleteSkuUnderlingPrice()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_SKUMemberPrice WHERE SkuId NOT IN (SELECT SkuId FROM Hishop_SKUs)");
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeleteSupplier(string supplierName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteTags(int tagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagID=@TagID;DELETE FROM distro_ProductTag WHERE TagId=@TagID;DELETE FROM Hishop_Tags WHERE TagID=@TagID;");
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int DisplaceCategory(int oldCategoryId, int newCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
            this.database.AddInParameter(sqlStringCommand, "oldCategoryId", DbType.Int32, oldCategoryId);
            this.database.AddInParameter(sqlStringCommand, "newCategory", DbType.Int32, newCategory);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void EnsureMapping(DataSet mappingSet)
        {
            using (DbCommand command = this.database.GetSqlStringCommand("INSERT INTO  Hishop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(command, "TypeName", DbType.String);
                this.database.AddInParameter(command, "Remark", DbType.String);
                DataRow[] rowArray = mappingSet.Tables["types"].Select("SelectedTypeId=0");
                foreach (DataRow row in rowArray)
                {
                    this.database.SetParameterValue(command, "TypeName", row["TypeName"]);
                    this.database.SetParameterValue(command, "Remark", row["Remark"]);
                    row["SelectedTypeId"] = this.database.ExecuteScalar(command);
                }
            }
            using (DbCommand command2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(command2, "AttributeName", DbType.String);
                this.database.AddInParameter(command2, "TypeId", DbType.Int32);
                this.database.AddInParameter(command2, "UsageMode", DbType.Int32);
                this.database.AddInParameter(command2, "UseAttributeImage", DbType.Boolean);
                DataRow[] rowArray2 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
                foreach (DataRow row2 in rowArray2)
                {
                    int num = (int) mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", row2["MappedTypeId"]))[0]["SelectedTypeId"];
                    this.database.SetParameterValue(command2, "AttributeName", row2["AttributeName"]);
                    this.database.SetParameterValue(command2, "TypeId", num);
                    this.database.SetParameterValue(command2, "UsageMode", int.Parse(row2["UsageMode"].ToString()));
                    this.database.SetParameterValue(command2, "UseAttributeImage", bool.Parse(row2["UseAttributeImage"].ToString()));
                    row2["SelectedAttributeId"] = this.database.ExecuteScalar(command2);
                }
            }
            using (DbCommand command3 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues;INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY;"))
            {
                this.database.AddInParameter(command3, "AttributeId", DbType.Int32);
                this.database.AddInParameter(command3, "ValueStr", DbType.String);
                this.database.AddInParameter(command3, "ImageUrl", DbType.String);
                DataRow[] rowArray3 = mappingSet.Tables["values"].Select("SelectedValueId=0");
                foreach (DataRow row3 in rowArray3)
                {
                    int num2 = (int) mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", row3["MappedAttributeId"]))[0]["SelectedAttributeId"];
                    this.database.SetParameterValue(command3, "AttributeId", num2);
                    this.database.SetParameterValue(command3, "ValueStr", row3["ValueStr"]);
                    this.database.SetParameterValue(command3, "ImageUrl", row3["ImageUrl"]);
                    row3["SelectedValueId"] = this.database.ExecuteScalar(command3);
                }
            }
            mappingSet.AcceptChanges();
        }

        public override AttributeInfo GetAttribute(int attributeId)
        {
            AttributeInfo info = new AttributeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
                while (reader.Read())
                {
                    AttributeValueInfo item = new AttributeValueInfo();
                    item.ValueId = (int) reader["ValueId"];
                    item.AttributeId = (int) reader["AttributeId"];
                    item.DisplaySequence = (int) reader["DisplaySequence"];
                    item.ValueStr = (string) reader["ValueStr"];
                    if (reader["ImageUrl"] != DBNull.Value)
                    {
                        item.ImageUrl = (string) reader["ImageUrl"];
                    }
                    list.Add(item);
                }
                if (!reader.NextResult())
                {
                    return info;
                }
                if (reader.Read())
                {
                    info.AttributeId = (int) reader["AttributeId"];
                    info.AttributeName = (string) reader["AttributeName"];
                    info.DisplaySequence = (int) reader["DisplaySequence"];
                    info.TypeId = (int) reader["TypeId"];
                    info.UsageMode = (AttributeUseageMode) ((int) reader["UsageMode"]);
                    info.UseAttributeImage = (bool) reader["UseAttributeImage"];
                    info.AttributeValues = list;
                }
            }
            return info;
        }

        public override IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
        {
            string str;
            IList<AttributeInfo> list = new List<AttributeInfo>();
            if (attributeUseageMode == AttributeUseageMode.Choose)
            {
                str = "UsageMode = 2";
            }
            else
            {
                str = "UsageMode <> 2";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE " + str + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes Where  " + str + " ) ORDER BY DisplaySequence Desc");
            using (DataSet set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo();
                    item.AttributeId = (int) row["AttributeId"];
                    item.AttributeName = (string) row["AttributeName"];
                    item.DisplaySequence = (int) row["DisplaySequence"];
                    item.TypeId = (int) row["TypeId"];
                    item.UsageMode = (AttributeUseageMode) ((int) row["UsageMode"]);
                    item.UseAttributeImage = (bool) row["UseAttributeImage"];
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString(CultureInfo.InvariantCulture));
                        foreach (DataRow row2 in rowArray)
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo();
                            info2.ValueId = (int) row2["ValueId"];
                            info2.AttributeId = item.AttributeId;
                            if (row2["ImageUrl"] != DBNull.Value)
                            {
                                info2.ImageUrl = (string) row2["ImageUrl"];
                            }
                            info2.ValueStr = (string) row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<AttributeInfo> GetAttributes(int typeId)
        {
            IList<AttributeInfo> list = new List<AttributeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo();
                    item.AttributeId = (int) row["AttributeId"];
                    item.AttributeName = (string) row["AttributeName"];
                    item.DisplaySequence = (int) row["DisplaySequence"];
                    item.TypeId = (int) row["TypeId"];
                    item.UsageMode = (AttributeUseageMode) ((int) row["UsageMode"]);
                    item.UseAttributeImage = (bool) row["UseAttributeImage"];
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString(CultureInfo.InvariantCulture));
                        foreach (DataRow row2 in rowArray)
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo();
                            info2.ValueId = (int) row2["ValueId"];
                            info2.AttributeId = item.AttributeId;
                            info2.ValueStr = (string) row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
        {
            string str;
            IList<AttributeInfo> list = new List<AttributeInfo>();
            if (attributeUseageMode == AttributeUseageMode.Choose)
            {
                str = "UsageMode = 2";
            }
            else
            {
                str = "UsageMode <> 2";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId AND " + str + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId AND  " + str + " ) ORDER BY DisplaySequence Desc");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo();
                    item.AttributeId = (int) row["AttributeId"];
                    item.AttributeName = (string) row["AttributeName"];
                    item.DisplaySequence = (int) row["DisplaySequence"];
                    item.TypeId = (int) row["TypeId"];
                    item.UsageMode = (AttributeUseageMode) ((int) row["UsageMode"]);
                    item.UseAttributeImage = (bool) row["UseAttributeImage"];
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] rowArray = set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString(CultureInfo.InvariantCulture));
                        foreach (DataRow row2 in rowArray)
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo();
                            info2.ValueId = (int) row2["ValueId"];
                            info2.AttributeId = item.AttributeId;
                            if (row2["ImageUrl"] != DBNull.Value)
                            {
                                info2.ImageUrl = (string) row2["ImageUrl"];
                            }
                            info2.ValueStr = (string) row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public override AttributeValueInfo GetAttributeValueInfo(int valueId)
        {
            AttributeValueInfo info = new AttributeValueInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE ValueId=@ValueId");
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateAttributeValue(reader);
                    info.ImageUrl = reader["ImageUrl"].ToString();
                    info.DisplaySequence = (int) reader["DisplaySequence"];
                }
            }
            return info;
        }

        public override DbQueryResult GetBindProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("  SaleStatus! = {0}", 0);
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Products", "ProductId", builder.ToString(), "ProductId, ProductCode, ProductName,ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, MarketPrice,DisplaySequence,LowestSalePrice, PenetrationStatus");
        }

        public override DataTable GetBrandCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategories(string brandName)
        {
            string str = "1=1";
            if (!string.IsNullOrEmpty(brandName))
            {
                str = str + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories  WHERE " + str + " ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override BrandCategoryInfo GetBrandCategory(int brandId)
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBrandCategory(reader);
                }
                IList<int> list = new List<int>();
                reader.NextResult();
                while (reader.Read())
                {
                    list.Add((int) reader["ProductTypeId"]);
                }
                info.ProductTypes = list;
            }
            return info;
        }

        public override DataTable GetCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,Name,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,Theme,HasChildren FROM Hishop_Categories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CategoryInfo GetCategory(int categoryId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE CategoryId =@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            CategoryInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductCategory(reader);
                }
            }
            return info;
        }

        private int GetDistributorDiscount(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
        {
            int? nullable;
            StringBuilder builder = new StringBuilder();
            builder.Append("(");
            if (query.IncludeOnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            builder.Remove(builder.Length - 4, 4);
            builder.Append(")");
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.IsMakeTaobao.HasValue && (((nullable = query.IsMakeTaobao).GetValueOrDefault() != -1) || !nullable.HasValue))
            {
                builder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int) query.PenetrationStatus);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                builder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
            }
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
        {
            int? nullable;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [LowestSalePrice], [PenetrationStatus], [HasSKU] ").Append("FROM Hishop_Products a  left join Taobao_Products b on a.productid=b.productid WHERE ");
            builder.Append("(");
            if (query.IncludeOnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 1);
            }
            if (query.IncludeUnSales)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 2);
            }
            if (query.IncludeInStock)
            {
                builder.AppendFormat("SaleStatus = {0} OR ", 3);
            }
            builder.Remove(builder.Length - 4, 4);
            builder.Append(")");
            if (query.IsMakeTaobao.HasValue && (((nullable = query.IsMakeTaobao).GetValueOrDefault() != -1) || !nullable.HasValue))
            {
                if (query.IsMakeTaobao == 1)
                {
                    builder.AppendFormat(" AND a.ProductId IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
                }
                else
                {
                    builder.AppendFormat(" AND a.ProductId NOT IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
                }
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int) query.PenetrationStatus);
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(removeProductIds))
            {
                builder.AppendFormat(" AND a.ProductId NOT IN ({0})", removeProductIds);
            }
            builder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_GetExportList");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, builder.ToString());
            return this.database.ExecuteDataSet(storedProcCommand);
        }

        public override DataTable GetGroupBuyProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" WHERE SaleStatus = {0}", 1);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override int GetMaxSequence()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT MAX(DisplaySequence) FROM Hishop_Products");
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            return ((obj2 == DBNull.Value) ? 0 : ((int) obj2));
        }

        public override DataTable GetProductBaseInfo(string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, LowestSalePrice,ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM Hishop_Products WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds, out IList<int> tagsId)
        {
            ProductInfo info = null;
            attrs = null;
            distributorUserIds = new List<int>();
            tagsId = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.PurchasePrice, skus.Stock, skus.AlertStock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Hishop_SKUMemberPrice smp INNER JOIN Hishop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT s.SkuId, sdp.GradeId, sdp.DistributorPurchasePrice FROM Hishop_SKUDistributorPrice sdp INNER JOIN Hishop_SKUs s ON sdp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId; SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId = (SELECT LineId FROM Hishop_Products WHERE ProductId = @ProductId);SELECT * FROM Hishop_ProductTag WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                string str;
                if (reader.Read())
                {
                    info = DataMapper.PopulateProduct(reader);
                }
                if (info == null)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string) reader["SkuId"];
                    if (!info.Skus.ContainsKey(str))
                    {
                        info.Skus.Add(str, DataMapper.PopulateSKU(reader));
                    }
                    if ((reader["AttributeId"] != DBNull.Value) && (reader["ValueId"] != DBNull.Value))
                    {
                        info.Skus[str].SkuItems.Add((int) reader["AttributeId"], (int) reader["ValueId"]);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string) reader["SkuId"];
                    info.Skus[str].MemberPrices.Add((int) reader["GradeId"], (decimal) reader["MemberSalePrice"]);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    str = (string) reader["SkuId"];
                    info.Skus[str].DistributorPrices.Add((int) reader["GradeId"], (decimal) reader["DistributorPurchasePrice"]);
                }
                reader.NextResult();
                attrs = new Dictionary<int, IList<int>>();
                while (reader.Read())
                {
                    int key = (int) reader["AttributeId"];
                    int item = (int) reader["ValueId"];
                    if (!attrs.ContainsKey(key))
                    {
                        IList<int> list = new List<int>();
                        list.Add(item);
                        attrs.Add(key, list);
                    }
                    else
                    {
                        attrs[key].Add(item);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    distributorUserIds.Add((int) reader["UserId"]);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    tagsId.Add((int) reader["TagId"]);
                }
            }
            return info;
        }

        public override IList<int> GetProductIds(ProductQuery query)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(BuildProductQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["ProductId"]);
                }
            }
            return list;
        }

        public override ProductLineInfo GetProductLine(int lineId)
        {
            ProductLineInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines WHERE LineId=@LineId");
            this.database.AddInParameter(sqlStringCommand, "LineId", DbType.Int32, lineId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateProductLine(reader);
                }
            }
            return info;
        }

        public override IList<ProductLineInfo> GetProductLineList()
        {
            IList<ProductLineInfo> list = new List<ProductLineInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateProductLine(reader));
                }
            }
            return list;
        }

        public override DataTable GetProductLines()
        {
            DataTable table;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT LineId,Name,SupplierName,(SELECT count(*) From Hishop_Products WHERE LineId=pl.LineId AND SaleStatus<>0) AS ProductCount FROM Hishop_ProductLines pl");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override string GetProductNameByProductIds(string productIds, out int sumcount)
        {
            int num = 0;
            string str = "";
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT ProductName FROM Hishop_Products WHERE PenetrationStatus=1", new object[0]);
            builder.AppendFormat(" AND SaleStatus!={0}", 0);
            builder.AppendFormat(" AND ProductId IN ({0})", productIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    str = str + ((string) reader["ProductName"]) + ",";
                    num++;
                }
            }
            if (str != "")
            {
                str = str.Substring(0, str.Length - 1);
            }
            sumcount = num;
            return str;
        }

        public override string GetProductNamesByLineId(int lineId, out int count)
        {
            int num = 0;
            string str = "";
            try
            {
                StringBuilder builder = new StringBuilder("select ProductName from Hishop_Products where PenetrationStatus=1");
                builder.AppendFormat(" and SaleStatus!={0}", 0);
                builder.AppendFormat(" and LineId={0}", lineId);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        str = str + reader["ProductName"].ToString() + ",";
                        num++;
                    }
                }
                if (str != "")
                {
                    str = str.Substring(0, str.Length);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            count = num;
            return str;
        }

        public override DbQueryResult GetProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                builder.AppendFormat(" AND SaleStatus = {0}", (int) query.SaleStatus);
            }
            else
            {
                builder.AppendFormat(" AND SaleStatus <> ({0})", 0);
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.TypeId.HasValue)
            {
                builder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
            }
            if (query.TagId.HasValue)
            {
                builder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue)
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.PenetrationStatus != PenetrationStatus.NotSet)
            {
                builder.AppendFormat(" AND PenetrationStatus={0}", (int) query.PenetrationStatus);
            }
            if (query.IsMakeTaobao.HasValue && (query.IsMakeTaobao.Value >= 0))
            {
                builder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
            {
                builder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts)");
            }
            if (!(!query.IsIncludeBundlingProduct.HasValue ? true : query.IsIncludeBundlingProduct.Value))
            {
                builder.Append(" AND ProductId NOT IN (SELECT ProductID FROM Hishop_BundlingProductItems)");
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.IsAlert)
            {
                builder.Append(" AND ProductId IN (SELECT DISTINCT ProductId FROM Hishop_SKUs WHERE Stock <= AlertStock)");
            }
            string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,SaleStatus,PenetrationStatus";
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override IList<ProductInfo> GetProducts(IList<int> productIds)
        {
            IList<ProductInfo> list = new List<ProductInfo>();
            string str = "(";
            foreach (int num in productIds)
            {
                str = str + num + ",";
            }
            if (str.Length > 1)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId IN " + (str.Substring(0, str.Length - 1) + ")"));
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(DataMapper.PopulateProduct(reader));
                    }
                }
            }
            return list;
        }

        public override DataSet GetProductsByQuery(ProductQuery query, out int totalrecord)
        {
            DataSet set = new DataSet();
            StringBuilder builder = new StringBuilder();
            builder.Append(" 1=1 ");
            if (query.SaleStatus != ProductSaleStatus.All)
            {
                builder.AppendFormat(" AND SaleStatus = {0}", (int) query.SaleStatus);
            }
            else
            {
                builder.AppendFormat(" AND SaleStatus not in ({0})", 0);
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
            }
            if (query.TagId.HasValue)
            {
                builder.AppendFormat("AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
            }
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.CategoryId.HasValue && (query.CategoryId.Value > 0))
            {
                builder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
            }
            if (query.IsMakeTaobao.HasValue && (query.IsMakeTaobao.Value >= 0))
            {
                builder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
            }
            if (query.PublishStatus != PublishStatus.NotSet)
            {
                if (query.PublishStatus == PublishStatus.Notyet)
                {
                    builder.Append(" AND TaobaoProductId = 0");
                }
                else
                {
                    builder.Append(" AND TaobaoProductId <> 0");
                }
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            string str = string.Concat(new object[] { "SELECT TOP ", query.PageSize, " ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE ", builder.ToString(), ";" });
            if (query.PageIndex > 1)
            {
                str = string.Concat(new object[] { "SELECT TOP ", query.PageSize, " ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE (ProductId>(SELECT max(ProductId) from (SELECT TOP ", (query.PageIndex - 1) * query.PageSize, " ProductId FROM vw_Hishop_BrowseProductList WHERE ", builder.ToString(), " order by ProductId) as T)) AND ", builder.ToString(), " order by ProductId;" });
            }
            str = (str + "select ProductId,SkuId,SKU,Stock,SalePrice from dbo.Hishop_SKUs;") + "SELECT COUNT(*) AS SumRecord FROM vw_Hishop_BrowseProductList WHERE " + builder.ToString();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str);
            using (set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                set.Relations.Add("ProductRealation", set.Tables[0].Columns["ProductId"], set.Tables[1].Columns["ProductId"], false);
            }
            totalrecord = Convert.ToInt32(set.Tables[2].Rows[0]["SumRecord"].ToString());
            return set;
        }

        public override DataSet GetProductSkuDetials(int productId)
        {
            DataSet set = new DataSet();
            string query = "";
            if (!(string.IsNullOrEmpty(productId.ToString()) || (Convert.ToInt32(productId) <= 0)))
            {
                query = string.Concat(new object[] { "SELECT ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE ProductId=", productId, ";SELECT ProductId,SkuId,SKU,Stock,SalePrice from dbo.Hishop_SKUs WHERE ProductId=", productId });
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                set = this.database.ExecuteDataSet(sqlStringCommand);
            }
            return set;
        }

        public override ProductTypeInfo GetProductType(int typeId)
        {
            ProductTypeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateType(reader);
                }
                IList<int> list = new List<int>();
                reader.NextResult();
                while (reader.Read())
                {
                    list.Add((int) reader["BrandId"]);
                }
                info.Brands = list;
            }
            return info;
        }

        public override IList<ProductTypeInfo> GetProductTypes()
        {
            IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateType(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetProductTypes(ProductTypeQuery query)
        {
            return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
        }

        public override DbQueryResult GetRelatedProducts(Pagination page, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" SaleStatus = {0}", 1);
            builder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
            string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", builder.ToString(), selectFields);
        }

        public override DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId)
        {
            IUser user = Users.GetUser(distorUserId, false);
            if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            if (distributor == null)
            {
                return null;
            }
            int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT s.SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice,", distributor.GradeId, distributorDiscount);
            builder.Append(" (SELECT ProductName FROM Hishop_Products WHERE ProductId = s.ProductId) AS ProductName,");
            builder.Append(" (SELECT ThumbnailUrl40 FROM Hishop_Products WHERE ProductId = s.ProductId) AS ThumbnailUrl40,AttributeName, ValueStr");
            builder.Append(" FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId");
            builder.Append(" left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSkuDistributorPrices(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductName, SKU, MarketPrice, CostPrice, PurchasePrice");
            builder.AppendFormat(" FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            builder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS DistributorGradeName,Discount FROM aspnet_DistributorGrades");
            builder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_DistributorGrades WHERE GradeId = sd.GradeId) AS DistributorGradeName,  DistributorPurchasePrice");
            builder.AppendFormat(" FROM Hishop_SKUDistributorPrice sd WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            DataTable table = null;
            DataTable table2 = null;
            DataTable table3 = null;
            DataTable table4 = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    table.Columns.Add("SKUContent");
                    reader.NextResult();
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                    reader.NextResult();
                    table4 = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table4 != null) && (table4.Rows.Count > 0))
                    {
                        foreach (DataRow row in table4.Rows)
                        {
                            table.Columns.Add((string) row["DistributorGradeName"]);
                        }
                    }
                    while (reader.Read())
                    {
                        table.Columns.Add((string) reader["DistributorGradeName"]);
                    }
                    reader.NextResult();
                    table3 = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    string str = string.Empty;
                    foreach (DataRow row3 in table2.Rows)
                    {
                        if (((string) row2["SkuId"]) == ((string) row3["SkuId"]))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, row3["AttributeName"], "：", row3["ValueStr"], "; " });
                        }
                    }
                    row2["SKUContent"] = str;
                }
            }
            if ((table3 != null) && (table3.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    foreach (DataRow row4 in table3.Rows)
                    {
                        if (((string) row2["SkuId"]) == ((string) row4["SkuId"]))
                        {
                            row2[(string) row4["DistributorGradeName"]] = (decimal) row4["DistributorPurchasePrice"];
                        }
                    }
                }
            }
            if ((table4 != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    decimal num = decimal.Parse(row2["PurchasePrice"].ToString());
                    foreach (DataRow row4 in table4.Rows)
                    {
                        decimal num2 = decimal.Parse(row4["Discount"].ToString());
                        string str2 = (num * (num2 / 100M)).ToString("F2");
                        row2[(string) row4["DistributorGradeName"]] = row2[(string) row4["DistributorGradeName"]] + "|" + str2;
                    }
                }
            }
            return table;
        }

        public override DataTable GetSkuMemberPrices(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            builder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
            builder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
            builder.AppendFormat(" FROM Hishop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            DataTable table = null;
            DataTable table2 = null;
            DataTable table3 = null;
            DataTable table4 = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                if ((table != null) && (table.Rows.Count > 0))
                {
                    table.Columns.Add("SKUContent");
                    reader.NextResult();
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                    reader.NextResult();
                    table4 = DataHelper.ConverDataReaderToDataTable(reader);
                    if ((table4 != null) && (table4.Rows.Count > 0))
                    {
                        foreach (DataRow row in table4.Rows)
                        {
                            table.Columns.Add((string) row["MemberGradeName"]);
                        }
                    }
                    reader.NextResult();
                    table3 = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    string str = string.Empty;
                    foreach (DataRow row3 in table2.Rows)
                    {
                        if (((string) row2["SkuId"]) == ((string) row3["SkuId"]))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, row3["AttributeName"], "：", row3["ValueStr"], "; " });
                        }
                    }
                    row2["SKUContent"] = str;
                }
            }
            if ((table3 != null) && (table3.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    foreach (DataRow row4 in table3.Rows)
                    {
                        if (((string) row2["SkuId"]) == ((string) row4["SkuId"]))
                        {
                            row2[(string) row4["MemberGradeName"]] = row4["MemberSalePrice"];
                        }
                    }
                }
            }
            if ((table4 != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row2 in table.Rows)
                {
                    decimal num = decimal.Parse(row2["SalePrice"].ToString());
                    foreach (DataRow row5 in table4.Rows)
                    {
                        decimal num2 = decimal.Parse(row5["Discount"].ToString());
                        string str2 = (num * (num2 / 100M)).ToString("F2");
                        row2[(string) row5["MemberGradeName"]] = row2[(string) row5["MemberGradeName"]] + "|" + str2;
                    }
                }
            }
            return table;
        }

        public override DataTable GetSkusByProductId(int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,saleprice");
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId)
        {
            IUser user = Users.GetUser(distorUserId, false);
            if (((user == null) || user.IsAnonymous) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            if (distributor == null)
            {
                return null;
            }
            int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
            builder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
            builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
            builder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", distributor.GradeId, distributorDiscount);
            builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetSkuStocks(string productIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock, AlertStock,ThumbnailUrl40 FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
            builder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
            builder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            DataTable table = null;
            DataTable table2 = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            table.Columns.Add("SKUContent");
            if ((((table != null) && (table.Rows.Count > 0)) && (table2 != null)) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    string str = string.Empty;
                    foreach (DataRow row2 in table2.Rows)
                    {
                        if (((string) row["SkuId"]) == ((string) row2["SkuId"]))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, row2["AttributeName"], "：", row2["ValueStr"], "; " });
                        }
                    }
                    row["SKUContent"] = str;
                }
            }
            return table;
        }

        public override int GetSpecificationId(int typeId, string specificationName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId FROM Hishop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND AttributeName = @AttributeName");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, specificationName);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if (obj2 != null)
            {
                num = (int) obj2;
            }
            return num;
        }

        public override int GetSpecificationValueId(int attributeId, string ValueStr)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ValueId FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, ValueStr);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2);
            }
            return num;
        }

        public override IList<int> GetSubjectProductIds(int tagId)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId FROM Hishop_ProductTag WHERE TagId=@TagId");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, tagId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["ProductId"]);
                }
            }
            return list;
        }

        public override DbQueryResult GetSubjectProducts(int tagId, Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList", "ProductId", string.Format("SaleStatus!={0} and ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId = {1})", 0, tagId), "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
        }

        public override DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) AND SaleStatus<>{1} ", distorUserId, 0);
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Keywords = DataHelper.CleanSearchString(query.Keywords);
                string[] strArray = Regex.Split(query.Keywords.Trim(), @"\s+");
                builder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[0]));
                for (int i = 1; (i < strArray.Length) && (i <= 4); i++)
                {
                    builder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(strArray[i]));
                }
            }
            if (query.ProductLineId.HasValue && (query.ProductLineId.Value > 0))
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Products", "ProductId", builder.ToString(), "ProductId, ProductCode, ProductName,ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, MarketPrice,DisplaySequence,LowestSalePrice, PenetrationStatus");
        }

        public override string GetSupplierRemark(string supplierName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Remark FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, supplierName);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (string) obj2;
            }
            return string.Empty;
        }

        public override IList<string> GetSuppliers()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SupplierName FROM Hishop_Suppliers");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader.GetString(0));
                }
            }
            return list;
        }

        public override DbQueryResult GetSuppliers(Pagination page)
        {
            return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, true, "Hishop_Suppliers", "SupplierName", "", "SupplierName,Remark");
        }

        public override string GetTagName(int tagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TagName  FROM  Hishop_Tags WHERE TagID = {0}", tagId));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return obj2.ToString();
            }
            return string.Empty;
        }

        public override DataTable GetTags()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override int GetTags(string tagName)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TagID  FROM  Hishop_Tags WHERE TagName=@TagName");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                num = Convert.ToInt32(reader["TagID"].ToString());
            }
            return num;
        }

        public override DataSet GetTaobaoProductDetails(int productId)
        {
            DataTable table;
            DataTable table2;
            DataTable table3;
            DataTable table4;
            DataTable table5;
            DataSet set = new DataSet();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT [Name] FROM Hishop_ProductLines WHERE LineId = p.LineId) AS ProductLineName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT MIN(PurchasePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS PurchasePrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', PurchasePrice AS '采购价', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table2 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table3 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table4 = DataHelper.ConverDataReaderToDataTable(reader);
                reader.NextResult();
                table5 = DataHelper.ConverDataReaderToDataTable(reader);
            }
            if (((table3 != null) && (table3.Rows.Count > 0)) && ((table4 != null) && (table4.Rows.Count > 0)))
            {
                foreach (DataRow row in table4.Rows)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = (string) row["AttributeName"];
                    if (!table3.Columns.Contains(column.ColumnName))
                    {
                        table3.Columns.Add(column);
                    }
                }
                foreach (DataRow row2 in table3.Rows)
                {
                    foreach (DataRow row in table4.Rows)
                    {
                        if (string.Compare((string) row2["商家编码"], (string) row["商家编码"]) == 0)
                        {
                            row2[(string) row["AttributeName"]] = row["ValueStr"];
                        }
                    }
                }
            }
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            set.Tables.Add(table5);
            return set;
        }

        public override int GetTypeId(string typeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TypeId FROM Hishop_ProductTypes where TypeName = @TypeName");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, typeName);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return 0;
        }

        public override DbQueryResult GetUnclassifiedProducts(ProductQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
            if (!string.IsNullOrEmpty(query.ProductCode))
            {
                builder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
            }
            if (query.ProductLineId.HasValue)
            {
                builder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
            }
            if (query.BrandId.HasValue)
            {
                builder.AppendFormat(" AND BrandId={0}", Convert.ToInt32(query.BrandId.Value));
            }
            if (query.TypeId.HasValue)
            {
                builder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
            }
            if (query.CategoryId.HasValue)
            {
                if (query.CategoryId.Value > 0)
                {
                    builder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
                }
                else
                {
                    builder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
                }
            }
            builder.AppendFormat(" AND SaleStatus!={0}", (int) query.SaleStatus);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList", "ProductId", builder.ToString(), "CategoryId,MainCategoryPath,ExtendCategoryPath, ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
        }

        public override IList<string> GetUserIdByLineId(int lineId)
        {
            List<string> list = new List<string>();
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId=" + lineId);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    while (reader.Read())
                    {
                        list.Add(reader["UserId"].ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return list;
        }

        public override IList<string> GetUserNameByProductId(string productIds)
        {
            List<string> list = new List<string>();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserName FROM aspnet_Users WHERE UserId IN (SELECT UserId from Hishop_DistributorProductLines WHERE LineId IN ");
            builder.AppendFormat(" (SELECT LineId from Hishop_Products WHERE SaleStatus!={0} AND PenetrationStatus=1 AND ProductId in ({1})))", 0, productIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader["UserName"].ToString());
                }
            }
            return list;
        }

        public override bool IsExitTaobaoProduct(long taobaoProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products WHERE TaobaoProductId = {0}", taobaoProductId));
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool OffShelfProductExcludedSalePrice(int productId, decimal lowestSalePrice, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Products  SET SaleStatus=3 WHERE ProductId = @ProductId AND (SELECT MIN(SalePrice) FROM vw_distro_SkuPrices WHERE DistributoruserId = distro_Products.DistributoruserId AND SkuId IN (SELECT SkuId FROM Hishop_Skus WHERE ProductId = @ProductId)) < @LowestSalePrice");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "LowestSalePrice", DbType.Currency, lowestSalePrice);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
        }

        private decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
        {
            decimal num = 0M;
            string str = operation;
            if (str == null)
            {
                return num;
            }
            if (!(str == "+"))
            {
                if (str != "-")
                {
                    if ((str != "*") && (str != "/"))
                    {
                        return num;
                    }
                    return (opreation1 * opreation2);
                }
            }
            else
            {
                return (opreation1 + opreation2);
            }
            return (opreation1 - opreation2);
        }

        public override int PenetrationProducts(string productIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET PenetrationStatus = 1 WHERE ProductId IN (" + productIds + ")");
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool RemoveRelatedProduct(int productId, int relatedProductId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "RelatedProductId", DbType.Int32, relatedProductId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool RemoveSubjectProduct(int tagId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagId = @TagId AND ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32, tagId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool ReplaceProductLine(int fromlineId, int replacelineId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductLine_Replace");
            this.database.AddInParameter(storedProcCommand, "OldLineId", DbType.Int32, fromlineId);
            this.database.AddInParameter(storedProcCommand, "NewLineId", DbType.Int32, replacelineId);
            return (this.database.ExecuteNonQuery(storedProcCommand) > 0);
        }

        public override bool ReplaceProductNames(string productIds, string oldWord, string newWord)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE ProductId IN ({2})", DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), productIds));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool SetBrandCategoryThemes(int brandid, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandid);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetCategoryThemes(int categoryId, string themeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, themeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool SetProductExtendCategory(int productId, string extendCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", DbType.String, extendCategoryPath);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
        }

        public override bool SwapCategorySequence(int categoryId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@CategoryId", DbType.Int32, categoryId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, TypeId = @TypeId, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId; DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag && (attribute.AttributeValues.Count != 0))
            {
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    this.database.AddInParameter(command, "AttributeId", DbType.Int32, attribute.AttributeId);
                    this.database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    this.database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.ExecuteNonQuery(command);
                }
            }
            return flag;
        }

        public override bool UpdateAttributeName(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UsageMode = @UsageMode WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateAttributeValue(int attributeId, int valueId, string newValue)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET ValueStr=@ValueStr WHERE ValueId=@valueId AND AttributeId=@AttributeId");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, newValue);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BrandCategories SET BrandName = @BrandName, Logo = @Logo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandCategory.BrandId);
            this.database.AddInParameter(sqlStringCommand, "BrandName", DbType.String, brandCategory.BrandName);
            this.database.AddInParameter(sqlStringCommand, "Logo", DbType.String, brandCategory.Logo);
            this.database.AddInParameter(sqlStringCommand, "CompanyUrl", DbType.String, brandCategory.CompanyUrl);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, brandCategory.RewriteName);
            this.database.AddInParameter(sqlStringCommand, "MetaKeywords", DbType.String, brandCategory.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "MetaDescription", DbType.String, brandCategory.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, brandCategory.Description);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, brandId);
            this.database.AddInParameter(storedProcCommand, "Sort", DbType.Int32, action);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
            this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", DbType.Int32, displaysequence);
            this.database.AddInParameter(sqlStringCommand, "@BrandId", DbType.Int32, brandId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override CategoryActionStatus UpdateCategory(CategoryInfo category)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName WHERE CategoryId = @CategoryId;UPDATE Hishop_Categories SET RewriteName = @RewriteName WHERE ParentCategoryId = @CategoryId");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, category.CategoryId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, category.Name);
            this.database.AddInParameter(sqlStringCommand, "SKUPrefix", DbType.String, category.SKUPrefix);
            this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", DbType.Int32, category.AssociatedProductType);
            this.database.AddInParameter(sqlStringCommand, "Meta_Title", DbType.String, category.MetaTitle);
            this.database.AddInParameter(sqlStringCommand, "Meta_Description", DbType.String, category.MetaDescription);
            this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", DbType.String, category.MetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "Notes1", DbType.String, category.Notes1);
            this.database.AddInParameter(sqlStringCommand, "Notes2", DbType.String, category.Notes2);
            this.database.AddInParameter(sqlStringCommand, "Notes3", DbType.String, category.Notes3);
            this.database.AddInParameter(sqlStringCommand, "Notes4", DbType.String, category.Notes4);
            this.database.AddInParameter(sqlStringCommand, "Notes5", DbType.String, category.Notes5);
            this.database.AddInParameter(sqlStringCommand, "RewriteName", DbType.String, category.RewriteName);
            return ((this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError);
        }

        public override bool UpdateProduct(ProductInfo product, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Update");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, product.CategoryId);
            this.database.AddInParameter(storedProcCommand, "MainCategoryPath", DbType.String, product.MainCategoryPath);
            this.database.AddInParameter(storedProcCommand, "TypeId", DbType.Int32, product.TypeId);
            this.database.AddInParameter(storedProcCommand, "ProductName", DbType.String, product.ProductName);
            this.database.AddInParameter(storedProcCommand, "ProductCode", DbType.String, product.ProductCode);
            this.database.AddInParameter(storedProcCommand, "ShortDescription", DbType.String, product.ShortDescription);
            this.database.AddInParameter(storedProcCommand, "Unit", DbType.String, product.Unit);
            this.database.AddInParameter(storedProcCommand, "Description", DbType.String, product.Description);
            this.database.AddInParameter(storedProcCommand, "Title", DbType.String, product.Title);
            this.database.AddInParameter(storedProcCommand, "Meta_Description", DbType.String, product.MetaDescription);
            this.database.AddInParameter(storedProcCommand, "Meta_Keywords", DbType.String, product.MetaKeywords);
            this.database.AddInParameter(storedProcCommand, "SaleStatus", DbType.Int32, (int) product.SaleStatus);
            this.database.AddInParameter(storedProcCommand, "DisplaySequence", DbType.Currency, product.DisplaySequence);
            this.database.AddInParameter(storedProcCommand, "ImageUrl1", DbType.String, product.ImageUrl1);
            this.database.AddInParameter(storedProcCommand, "ImageUrl2", DbType.String, product.ImageUrl2);
            this.database.AddInParameter(storedProcCommand, "ImageUrl3", DbType.String, product.ImageUrl3);
            this.database.AddInParameter(storedProcCommand, "ImageUrl4", DbType.String, product.ImageUrl4);
            this.database.AddInParameter(storedProcCommand, "ImageUrl5", DbType.String, product.ImageUrl5);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", DbType.String, product.ThumbnailUrl40);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", DbType.String, product.ThumbnailUrl60);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", DbType.String, product.ThumbnailUrl100);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", DbType.String, product.ThumbnailUrl160);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", DbType.String, product.ThumbnailUrl180);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", DbType.String, product.ThumbnailUrl220);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", DbType.String, product.ThumbnailUrl310);
            this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", DbType.String, product.ThumbnailUrl410);
            this.database.AddInParameter(storedProcCommand, "LineId", DbType.Int32, product.LineId);
            this.database.AddInParameter(storedProcCommand, "MarketPrice", DbType.Currency, product.MarketPrice);
            this.database.AddInParameter(storedProcCommand, "LowestSalePrice", DbType.Currency, product.LowestSalePrice);
            this.database.AddInParameter(storedProcCommand, "PenetrationStatus", DbType.Int16, (int) product.PenetrationStatus);
            this.database.AddInParameter(storedProcCommand, "BrandId", DbType.Int32, product.BrandId);
            this.database.AddInParameter(storedProcCommand, "HasSKU", DbType.Boolean, product.HasSKU);
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, product.ProductId);
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
        }

        public override bool UpdateProductBaseInfo(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            foreach (DataRow row in dt.Rows)
            {
                num++;
                string str = num.ToString();
                builder.AppendFormat(" UPDATE Hishop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0}", str);
                builder.AppendFormat(", LowestSalePrice = {0} WHERE ProductId = {1}", row["LowestSalePrice"], row["ProductId"]);
                builder.AppendFormat(" UPDATE distro_Products SET ProductCode = @ProductCode{0} WHERE ProductId = {1}", str, row["ProductId"]);
                this.database.AddInParameter(sqlStringCommand, "ProductName" + str, DbType.String, row["ProductName"]);
                this.database.AddInParameter(sqlStringCommand, "ProductCode" + str, DbType.String, row["ProductCode"]);
                this.database.AddInParameter(sqlStringCommand, "MarketPrice" + str, DbType.String, row["MarketPrice"]);
            }
            sqlStringCommand.CommandText = builder.ToString();
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, newCategoryId);
            this.database.AddInParameter(sqlStringCommand, "MainCategoryPath", DbType.String, mainCategoryPath);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateProductLine(ProductLineInfo productLine)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductLines SET Name=@Name, SupplierName=@SupplierName WHERE LineId=@LineId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, productLine.Name);
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, productLine.SupplierName);
            this.database.AddInParameter(sqlStringCommand, "LineId", DbType.Int32, productLine.LineId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateProductLine(int lineId, int productId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("delete from distro_Products where ProductId=@productId and DistributorUserId ");
            builder.Append(" in (select UserId from Hishop_DistributorProductLines where LineId in ");
            builder.Append("(select LineId from Hishop_Products where ProductId=@productId))");
            builder.Append("update Hishop_Products set LineId=@lineId where ProductId=@productId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "@productId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "@lineId", DbType.Int32, lineId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateProductNames(string productIds, string prefix, string suffix)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE ProductId IN ({2})", DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), productIds));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET SaleStatus = {0} WHERE ProductId IN ({1})", (int) saleStatus, productIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateProductType(ProductTypeInfo productType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
            this.database.AddInParameter(sqlStringCommand, "TypeName", DbType.String, productType.TypeName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, productType.Remark);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, productType.TypeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateShowSaleCounts(DataTable dt)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                builder.AppendFormat(" UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId = {1}", row["ShowSaleCounts"], row["ProductId"]);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId IN ({1})", showSaleCounts, productIds));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = SaleCounts {0} {1} WHERE ProductId IN ({2})", operation, showSaleCounts, productIds));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSku(AttributeValueInfo attributeValue)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValue.ValueId);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(DataSet ds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable table = ds.Tables["skuPriceTable"];
            DataTable table2 = ds.Tables["skuDistributorPriceTable"];
            string str = string.Empty;
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "'", row["skuId"], "'," });
                    builder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, PurchasePrice = {1} WHERE SkuId = '{2}'", row["costPrice"], row["purchasePrice"], row["skuId"]);
                }
            }
            if (str.Length > 1)
            {
                builder.AppendFormat(" DELETE FROM Hishop_SKUDistributorPrice WHERE SkuId IN ({0}) ", str.Remove(str.Length - 1));
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table2.Rows)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId, GradeId, DistributorPurchasePrice) VALUES ('{0}', {1}, {2})", row["skuId"], row["gradeId"], row["distributorPrice"]);
                }
            }
            if (builder.Length <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else if (gradeId == -4)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice) SELECT SkuId, {0} AS GradeId, {1} AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            StringBuilder builder = new StringBuilder(" ");
            if (gradeId == -2)
            {
                if (baseGradeId == -2)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
                else if (baseGradeId == -4)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = PurchasePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
            }
            else if (gradeId == -4)
            {
                if (baseGradeId == -2)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
                else if (baseGradeId == -4)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = PurchasePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                if (baseGradeId == -2)
                {
                    builder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
                    builder.AppendFormat("  SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[] { gradeId, operation, price, DataHelper.CleanSearchString(productIds) });
                }
                else if (baseGradeId == -4)
                {
                    builder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
                    builder.AppendFormat("  SELECT SkuId, {0} AS GradeId, PurchasePrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[] { gradeId, operation, price, DataHelper.CleanSearchString(productIds) });
                }
                else
                {
                    builder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
                    builder.AppendFormat(" SELECT SkuId, {0} AS GradeId, DistributorPurchasePrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUDistributorPrice", gradeId, operation, price);
                    builder.AppendFormat(" WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(DataSet ds)
        {
            StringBuilder builder = new StringBuilder();
            DataTable table = ds.Tables["skuPriceTable"];
            DataTable table2 = ds.Tables["skuMemberPriceTable"];
            string str = string.Empty;
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, "'", row["skuId"], "'," });
                    builder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", row["costPrice"], row["salePrice"], row["skuId"]);
                }
            }
            if (str.Length > 1)
            {
                builder.AppendFormat(" DELETE FROM Hishop_SKUMemberPrice WHERE SkuId IN ({0}) ", str.Remove(str.Length - 1));
            }
            if ((table2 != null) && (table2.Rows.Count > 0))
            {
                foreach (DataRow row in table2.Rows)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", row["skuId"], row["gradeId"], row["memberPrice"]);
                }
            }
            if (builder.Length <= 0)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
        {
            StringBuilder builder = new StringBuilder();
            if (gradeId == -2)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else if (gradeId == -3)
            {
                builder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
        {
            StringBuilder builder = new StringBuilder(" ");
            if (gradeId == -2)
            {
                if (baseGradeId == -2)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
                else if (baseGradeId == -3)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
            }
            else if (gradeId == -3)
            {
                if (baseGradeId == -2)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
                else if (baseGradeId == -3)
                {
                    builder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
                }
            }
            else
            {
                builder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
                if (baseGradeId == -2)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[] { gradeId, operation, price, DataHelper.CleanSearchString(productIds) });
                }
                else if (baseGradeId == -3)
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, SalePrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[] { gradeId, operation, price, DataHelper.CleanSearchString(productIds) });
                }
                else
                {
                    builder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, MemberSalePrice {1} ({2}) AS MemberSalePrice", gradeId, operation, price);
                    builder.AppendFormat(" FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
                }
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuStock(Dictionary<string, int> skuStocks)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string str in skuStocks.Keys)
            {
                builder.AppendFormat(" UPDATE Hishop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[str], DataHelper.CleanSearchString(str));
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSkuStock(string productIds, int stock)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = {0} WHERE ProductId IN ({1})", stock, DataHelper.CleanSearchString(productIds)));
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSpecification(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateSupplier(string oldName, string newName, string remark)
        {
            if (!oldName.Equals(newName))
            {
                DbCommand command = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
                this.database.AddInParameter(command, "SupplierName", DbType.String, newName);
                if (((int) this.database.ExecuteScalar(command)) == 0)
                {
                    DbCommand command2 = this.database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET SupplierName=@SupplierName,Remark=@Remark WHERE LOWER(SupplierName) = LOWER(@OldSupplierName)");
                    this.database.AddInParameter(command2, "SupplierName", DbType.String, newName);
                    this.database.AddInParameter(command2, "Remark", DbType.String, remark);
                    this.database.AddInParameter(command2, "OldSupplierName", DbType.String, oldName);
                    return (this.database.ExecuteNonQuery(command2) >= 1);
                }
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET Remark=@Remark WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
            this.database.AddInParameter(sqlStringCommand, "SupplierName", DbType.String, newName);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool UpdateTags(int tagId, string tagName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Tags SET TagName=@TagName WHERE TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds);update Taobao_DistroProducts set  updatestatus=1 where  ProductId = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, taobaoProduct.ProductId);
            this.database.AddInParameter(sqlStringCommand, "Cid", DbType.Int64, taobaoProduct.Cid);
            this.database.AddInParameter(sqlStringCommand, "StuffStatus", DbType.String, taobaoProduct.StuffStatus);
            this.database.AddInParameter(sqlStringCommand, "ProTitle", DbType.String, taobaoProduct.ProTitle);
            this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int64, taobaoProduct.Num);
            this.database.AddInParameter(sqlStringCommand, "LocationState", DbType.String, taobaoProduct.LocationState);
            this.database.AddInParameter(sqlStringCommand, "LocationCity", DbType.String, taobaoProduct.LocationCity);
            this.database.AddInParameter(sqlStringCommand, "FreightPayer", DbType.String, taobaoProduct.FreightPayer);
            this.database.AddInParameter(sqlStringCommand, "PostFee", DbType.Currency, taobaoProduct.PostFee);
            this.database.AddInParameter(sqlStringCommand, "ExpressFee", DbType.Currency, taobaoProduct.ExpressFee);
            this.database.AddInParameter(sqlStringCommand, "EMSFee", DbType.Currency, taobaoProduct.EMSFee);
            this.database.AddInParameter(sqlStringCommand, "HasInvoice", DbType.Boolean, taobaoProduct.HasInvoice);
            this.database.AddInParameter(sqlStringCommand, "HasWarranty", DbType.Boolean, taobaoProduct.HasWarranty);
            this.database.AddInParameter(sqlStringCommand, "HasDiscount", DbType.Boolean, taobaoProduct.HasDiscount);
            this.database.AddInParameter(sqlStringCommand, "ValidThru", DbType.Int64, taobaoProduct.ValidThru);
            this.database.AddInParameter(sqlStringCommand, "ListTime", DbType.DateTime, taobaoProduct.ListTime);
            this.database.AddInParameter(sqlStringCommand, "PropertyAlias", DbType.String, taobaoProduct.PropertyAlias);
            this.database.AddInParameter(sqlStringCommand, "InputPids", DbType.String, taobaoProduct.InputPids);
            this.database.AddInParameter(sqlStringCommand, "InputStr", DbType.String, taobaoProduct.InputStr);
            this.database.AddInParameter(sqlStringCommand, "SkuProperties", DbType.String, taobaoProduct.SkuProperties);
            this.database.AddInParameter(sqlStringCommand, "SkuQuantities", DbType.String, taobaoProduct.SkuQuantities);
            this.database.AddInParameter(sqlStringCommand, "SkuPrices", DbType.String, taobaoProduct.SkuPrices);
            this.database.AddInParameter(sqlStringCommand, "SkuOuterIds", DbType.String, taobaoProduct.SkuOuterIds);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

