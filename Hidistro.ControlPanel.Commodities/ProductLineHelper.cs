namespace Hidistro.ControlPanel.Commodities
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    public class ProductLineHelper
    {
        public static bool AddProductLine(ProductLineInfo productLine)
        {
            Globals.EntityCoding(productLine, true);
            bool flag = ProductProvider.Instance().AddProductLine(productLine);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddProductLine, string.Format(CultureInfo.InvariantCulture, "成功的添加了一条产品线", new object[0]));
            }
            return flag;
        }

        public static bool AddSupplier(string supplierName, string remark)
        {
            return ProductProvider.Instance().AddSupplier(supplierName, remark);
        }

        public static bool DeleteProductLine(int lineId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteProductLine);
            bool flag = ProductProvider.Instance().DeleteProductLine(lineId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteProductLine, string.Format(CultureInfo.InvariantCulture, "删除了一条产品线", new object[0]));
            }
            return flag;
        }

        public static void DeleteSupplier(string supplierName)
        {
            ProductProvider.Instance().DeleteSupplier(supplierName);
        }

        public static ProductLineInfo GetProductLine(int lineId)
        {
            return ProductProvider.Instance().GetProductLine(lineId);
        }

        public static IList<ProductLineInfo> GetProductLineList()
        {
            return ProductProvider.Instance().GetProductLineList();
        }

        public static DataTable GetProductLines()
        {
            return ProductProvider.Instance().GetProductLines();
        }

        public static string GetSupplierRemark(string supplierName)
        {
            return ProductProvider.Instance().GetSupplierRemark(supplierName);
        }

        public static IList<string> GetSuppliers()
        {
            return ProductProvider.Instance().GetSuppliers();
        }

        public static DbQueryResult GetSuppliers(Pagination page)
        {
            return ProductProvider.Instance().GetSuppliers(page);
        }

        public static bool ReplaceProductLine(int fromlineId, int replacelineId)
        {
            return ProductProvider.Instance().ReplaceProductLine(fromlineId, replacelineId);
        }

        public static bool UpdateProductLine(ProductLineInfo productLine)
        {
            Globals.EntityCoding(productLine, true);
            bool flag = ProductProvider.Instance().UpdateProductLine(productLine);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditProductLine, string.Format(CultureInfo.InvariantCulture, "修改了产品线", new object[0]));
            }
            return flag;
        }

        public static bool UpdateProductLine(int replacelineId, int productId)
        {
            return ProductProvider.Instance().UpdateProductLine(replacelineId, productId);
        }

        public static bool UpdateSupplier(string oldName, string newName, string remark)
        {
            return ProductProvider.Instance().UpdateSupplier(oldName, newName, remark);
        }
    }
}

