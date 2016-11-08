namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CategoryData : CategoryMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum)
        {
            IList<AttributeInfo> list = new List<AttributeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Hishop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2) AND ValueId IN (SELECT ValueId FROM Hishop_ProductAttributes) ORDER BY DisplaySequence DESC;" + string.Format(" SELECT TOP {0} * FROM Hishop_Attributes WHERE TypeId=(SELECT AssociatedProductType FROM Hishop_Categories WHERE CategoryId=@CategoryId) AND UsageMode <> 2", maxNum) + " AND AttributeId IN (SELECT AttributeId FROM Hishop_ProductAttributes) ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<AttributeValueInfo> list2 = new List<AttributeValueInfo>();
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
                    list2.Add(item);
                }
                if (!reader.NextResult())
                {
                    return list;
                }
                while (reader.Read())
                {
                    AttributeInfo info2 = new AttributeInfo();
                    info2.AttributeId = (int) reader["AttributeId"];
                    info2.AttributeName = (string) reader["AttributeName"];
                    info2.DisplaySequence = (int) reader["DisplaySequence"];
                    info2.TypeId = (int) reader["TypeId"];
                    info2.UsageMode = (AttributeUseageMode) ((int) reader["UsageMode"]);
                    info2.UseAttributeImage = (bool) reader["UseAttributeImage"];
                    foreach (AttributeValueInfo info3 in list2)
                    {
                        if (info2.AttributeId == info3.AttributeId)
                        {
                            info2.AttributeValues.Add(info3);
                        }
                    }
                    list.Add(info2);
                }
            }
            return list;
        }

        public override DataTable GetBrandCategories(int categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT TOP {0} BrandId, BrandName, Logo, RewriteName FROM Hishop_BrandCategories", maxNum);
            CategoryInfo category = CategoryBrowser.GetCategory(categoryId);
            if (category != null)
            {
                builder.AppendFormat(" WHERE BrandId IN (SELECT BrandId FROM Hishop_Products WHERE MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", category.Path);
            }
            builder.Append(" ORDER BY DisplaySequence DESC");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override BrandCategoryInfo GetBrandCategory(int brandId)
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBrandCategory(reader);
                }
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

        public override DataSet GetThreeLayerCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name,CategoryId,RewriteName FROM Hishop_Categories WHERE ParentCategoryId=0 AND Depth = 1 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM Hishop_Categories WHERE Depth = 2 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM Hishop_Categories WHERE Depth = 3 ORDER BY DisplaySequence;");
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            set.Relations.Add("One", set.Tables[0].Columns["CategoryId"], set.Tables[1].Columns["ParentCategoryId"], false);
            set.Relations.Add("Two", set.Tables[1].Columns["CategoryId"], set.Tables[2].Columns["ParentCategoryId"], false);
            return set;
        }
    }
}

