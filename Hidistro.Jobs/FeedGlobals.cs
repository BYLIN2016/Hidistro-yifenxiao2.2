namespace Hidistro.Jobs
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class FeedGlobals
    {
        internal static XmlElement CreateXMlNode(XmlDocument doc, XmlElement rootContent, string nodeName)
        {
            XmlElement newChild = doc.CreateElement(nodeName);
            rootContent.AppendChild(newChild);
            return newChild;
        }

        internal static void CreateXMlNodeAttr(XmlDocument doc, XmlElement rootContent, string nodeName, string attrName, string attrValue, string nodeValue)
        {
            XmlElement newChild = doc.CreateElement(nodeName);
            XmlAttribute node = doc.CreateAttribute(attrName);
            node.Value = attrValue;
            newChild.Attributes.Append(node);
            XmlText text = doc.CreateTextNode(nodeValue);
            newChild.AppendChild(text);
            rootContent.AppendChild(newChild);
        }

        internal static void CreateXMlNodeValue(XmlDocument doc, XmlElement rootContent, string nodeName, string nodeValue)
        {
            XmlElement newChild = doc.CreateElement(nodeName);
            XmlText text = doc.CreateTextNode(nodeValue);
            newChild.AppendChild(text);
            rootContent.AppendChild(newChild);
        }

        internal static string GetCategoryIds(string MainCategoryPath)
        {
            string[] strArray = MainCategoryPath.Split(new char[] { '|' });
            if ((strArray.Length != 1) && (strArray[1] != ""))
            {
                return (strArray[0] + "," + strArray[1]);
            }
            return strArray[0];
        }

        internal static DataTable GetDistributorFeed()
        {
            Database database = DatabaseFactory.CreateDatabase();
            string query = "SELECT [UserId],[SiteUrl] ,[IsOpenEtao],[EtaoID] ,[EtaoStatus] FROM [distro_Settings]";
            DbCommand sqlStringCommand = database.GetSqlStringCommand(query);
            return database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        internal static DataSet GetETaoFeedProducts(int DistributorUserId)
        {
            Database database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("select top 500 p.DistributorUserId,p2.*,brandname,  (SELECT     MIN(SalePrice) AS Expr1 FROM    dbo.Hishop_SKUs WHERE    (ProductId = p.ProductId)) AS SalePrice from distro_Products p inner join Hishop_Products p2 on p.ProductId=p2.ProductId   left join Hishop_BrandCategories  a on  p.brandId=a.brandId where p.DistributorUserId=" + DistributorUserId + " and p.salestatus=1 and p.categoryId!=0; ");
            builder.Append("select * from Hishop_Categories;");
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            return database.ExecuteDataSet(sqlStringCommand);
        }

        internal static string GetEtaoSku(int productid)
        {
            Database database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            List<Hidistro.Jobs.Attribute> list = new List<Hidistro.Jobs.Attribute>();
            List<string> list2 = new List<string>();
            builder.AppendFormat("select AttributeName,valuestr from (SELECT   AttributeName, ValueStr FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = {0})) s   group by  AttributeName,valuestr", productid);
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    Hidistro.Jobs.Attribute item = new Hidistro.Jobs.Attribute();
                    item.AttrName = reader["AttributeName"].ToString();
                    item.AttrValue = reader["valuestr"].ToString();
                    list.Add(item);
                    if (!list2.Contains(reader["AttributeName"].ToString()))
                    {
                        list2.Add(reader["AttributeName"].ToString());
                    }
                }
            }
            string str = "";
            foreach (string str2 in list2)
            {
                string str3 = "";
                foreach (Hidistro.Jobs.Attribute attribute2 in list)
                {
                    if (str2 == attribute2.AttrName)
                    {
                        str3 = str3 + attribute2.AttrValue + ",";
                    }
                }
                string str5 = str;
                str = str5 + str2 + ":" + str3.Substring(0, str3.Length - 1) + ";";
            }
            return str;
        }

        public static void MakeSellerCats(DataSet ds, string prefixRootPath, string seller_ID, string sellerCatsVersion)
        {
            DataTable table = ds.Tables[1];
            if (((table != null) && (table.Rows.Count > 0)) && !(prefixRootPath.Trim() == ""))
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration newChild = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(newChild);
                XmlElement element = doc.CreateElement("", "root", "");
                doc.AppendChild(element);
                CreateXMlNodeValue(doc, element, "version", sellerCatsVersion);
                CreateXMlNodeValue(doc, element, "modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                CreateXMlNodeValue(doc, element, "seller_id", seller_ID);
                XmlElement element2 = CreateXMlNode(doc, element, "seller_cats");
                DataRow[] rowArray = table.Select("depth=1");
                foreach (DataRow row in rowArray)
                {
                    XmlElement element3 = doc.CreateElement("cat");
                    element2.AppendChild(element3);
                    CreateXMlNodeValue(doc, element3, "scid", row["CategoryId"].ToString());
                    CreateXMlNodeValue(doc, element3, "name", row["Name"].ToString());
                    DataRow[] rowArray2 = table.Select("ParentCategoryId=" + row["categoryId"]);
                    if ((rowArray2 != null) && (rowArray2.Length > 0))
                    {
                        XmlElement element4 = CreateXMlNode(doc, element3, "cats");
                        foreach (DataRow row2 in rowArray2)
                        {
                            XmlElement element5 = doc.CreateElement("cat");
                            element4.AppendChild(element5);
                            CreateXMlNodeValue(doc, element5, "scid", row2["CategoryId"].ToString());
                            CreateXMlNodeValue(doc, element5, "name", row2["Name"].ToString());
                        }
                    }
                }
                if (File.Exists(prefixRootPath + "SellerCats.xml"))
                {
                    File.Delete(prefixRootPath + "SellerCats.xml");
                }
                doc.Save(prefixRootPath + "SellerCats.xml");
            }
        }
    }
}

