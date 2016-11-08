namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web;

    public sealed class ProductTypeHelper
    {
        private ProductTypeHelper()
        {
        }

        public static bool AddAttribute(AttributeInfo attribute)
        {
            return ProductProvider.Instance().AddAttribute(attribute);
        }

        public static bool AddAttributeName(AttributeInfo attribute)
        {
            return (ProductProvider.Instance().AddAttributeName(attribute) > 0);
        }

        public static int AddAttributeValue(AttributeValueInfo attributeValue)
        {
            return ProductProvider.Instance().AddAttributeValue(attributeValue);
        }

        public static int AddProductType(ProductTypeInfo productType)
        {
            if (productType == null)
            {
                return 0;
            }
            Globals.EntityCoding(productType, true);
            int typeId = ProductProvider.Instance().AddProductType(productType);
            if (typeId > 0)
            {
                if (productType.Brands.Count > 0)
                {
                    ProductProvider.Instance().AddProductTypeBrands(typeId, productType.Brands);
                }
                EventLogs.WriteOperationLog(Privilege.AddProductType, string.Format(CultureInfo.InvariantCulture, "创建了一个新的商品类型:”{0}”", new object[] { productType.TypeName }));
            }
            return typeId;
        }

        public static bool ClearAttributeValue(int attributeId)
        {
            return ProductProvider.Instance().ClearAttributeValue(attributeId);
        }

        public static bool DeleteAttribute(int attriubteId)
        {
            return ProductProvider.Instance().DeleteAttribute(attriubteId);
        }

        public static bool DeleteAttribute(int attributeId, int valueId)
        {
            return ProductProvider.Instance().DeleteAttribute(attributeId, valueId);
        }

        public static bool DeleteAttributeValue(int attributeValueId)
        {
            return ProductProvider.Instance().DeleteAttributeValue(attributeValueId);
        }

        public static bool DeleteProductType(int typeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductType);
            bool flag = ProductProvider.Instance().DeleteProducType(typeId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductType, string.Format(CultureInfo.InvariantCulture, "删除了编号为”{0}”的商品类型", new object[] { typeId }));
            }
            return flag;
        }

        public static AttributeInfo GetAttribute(int attributeId)
        {
            return ProductProvider.Instance().GetAttribute(attributeId);
        }

        public static IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
        {
            return ProductProvider.Instance().GetAttributes(attributeUseageMode);
        }

        public static IList<AttributeInfo> GetAttributes(int typeId)
        {
            return ProductProvider.Instance().GetAttributes(typeId);
        }

        public static IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
        {
            return ProductProvider.Instance().GetAttributes(typeId, attributeUseageMode);
        }

        public static AttributeValueInfo GetAttributeValueInfo(int valueId)
        {
            return ProductProvider.Instance().GetAttributeValueInfo(valueId);
        }

        public static DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            return ProductProvider.Instance().GetBrandCategoriesByTypeId(typeId);
        }

        public static ProductTypeInfo GetProductType(int typeId)
        {
            return ProductProvider.Instance().GetProductType(typeId);
        }

        public static IList<ProductTypeInfo> GetProductTypes()
        {
            return ProductProvider.Instance().GetProductTypes();
        }

        public static DbQueryResult GetProductTypes(ProductTypeQuery query)
        {
            return ProductProvider.Instance().GetProductTypes(query);
        }

        public static int GetSpecificationId(int typeId, string specificationName)
        {
            int specificationId = ProductProvider.Instance().GetSpecificationId(typeId, specificationName);
            if (specificationId > 0)
            {
                return specificationId;
            }
            AttributeInfo attribute = new AttributeInfo();
            attribute.TypeId = typeId;
            attribute.UsageMode = AttributeUseageMode.Choose;
            attribute.UseAttributeImage = false;
            attribute.AttributeName = specificationName;
            return ProductProvider.Instance().AddAttributeName(attribute);
        }

        public static int GetSpecificationValueId(int attributeId, string valueStr)
        {
            int specificationValueId = ProductProvider.Instance().GetSpecificationValueId(attributeId, valueStr);
            if (specificationValueId > 0)
            {
                return specificationValueId;
            }
            AttributeValueInfo attributeValue = new AttributeValueInfo();
            attributeValue.AttributeId = attributeId;
            attributeValue.ValueStr = valueStr;
            return ProductProvider.Instance().AddAttributeValue(attributeValue);
        }

        public static int GetTypeId(string typeName)
        {
            int typeId = ProductProvider.Instance().GetTypeId(typeName);
            if (typeId > 0)
            {
                return typeId;
            }
            ProductTypeInfo productType = new ProductTypeInfo();
            productType.TypeName = typeName;
            return ProductProvider.Instance().AddProductType(productType);
        }

        public static void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
        {
            ProductProvider.Instance().SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
        {
            ProductProvider.Instance().SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateAttribute(AttributeInfo attribute)
        {
            return ProductProvider.Instance().UpdateAttribute(attribute);
        }

        public static bool UpdateAttributeName(AttributeInfo attribute)
        {
            return ProductProvider.Instance().UpdateAttributeName(attribute);
        }

        public static bool UpdateAttributeValue(int attributeId, int valueId, string newValue)
        {
            return ProductProvider.Instance().UpdateAttributeValue(attributeId, valueId, newValue);
        }

        public static bool UpdateProductType(ProductTypeInfo productType)
        {
            if (productType == null)
            {
                return false;
            }
            Globals.EntityCoding(productType, true);
            bool flag = ProductProvider.Instance().UpdateProductType(productType);
            if (flag)
            {
                if (ProductProvider.Instance().DeleteProductTypeBrands(productType.TypeId))
                {
                    ProductProvider.Instance().AddProductTypeBrands(productType.TypeId, productType.Brands);
                }
                EventLogs.WriteOperationLog(Privilege.EditProductType, string.Format(CultureInfo.InvariantCulture, "修改了编号为”{0}”的商品类型", new object[] { productType.TypeId }));
            }
            return flag;
        }

        public static bool UpdateSku(AttributeValueInfo attributeValue)
        {
            return ProductProvider.Instance().UpdateSku(attributeValue);
        }

        public static bool UpdateSpecification(AttributeInfo attribute)
        {
            return ProductProvider.Instance().UpdateSpecification(attribute);
        }

        public static string UploadSKUImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/sku/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

