namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.Core.Jobs;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Xml;

    public class FullIndexJob : IJob
    {
        private string fullVersion = "1.0";
        private string incVersion = "1.0";
        private string prefixFullPath;
        private string prefixIncPath;
        private string prefixRootPath;
        private IList<string> productsList = new List<string>();
        private string seller_ID = "";
        private string sellerCatsVersion = "1.0";
        private string storgePath = "";
        private string webSite = "";

        public void Execute(XmlNode node)
        {
            DataTable distributorFeed = FeedGlobals.GetDistributorFeed();
            int count = distributorFeed.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                if (Convert.ToBoolean(distributorFeed.Rows[i]["IsOpenEtao"]) && (Convert.ToInt32(distributorFeed.Rows[i]["EtaoStatus"]) == 1))
                {
                    this.seller_ID = Convert.ToString(distributorFeed.Rows[i]["EtaoID"]);
                    this.webSite = "http://" + Convert.ToString(distributorFeed.Rows[i]["SiteUrl"]);
                    this.prefixRootPath = Globals.MapPath("/Storage/Root/" + distributorFeed.Rows[i]["UserId"].ToString() + "/");
                    this.storgePath = "/Storage/Root/" + distributorFeed.Rows[i]["UserId"].ToString() + "/";
                    this.prefixFullPath = "Item_Full";
                    this.prefixIncPath = "Item_Inc";
                    DataSet eTaoFeedProducts = FeedGlobals.GetETaoFeedProducts(Convert.ToInt32(distributorFeed.Rows[i]["UserId"]));
                    this.MakeProductDetail(eTaoFeedProducts);
                    this.MakeFullIndex(eTaoFeedProducts, this.fullVersion, this.prefixFullPath);
                }
            }
        }

        public void MakeFullIndex(DataSet ds, string StrVersion, string StrFileName)
        {
            DataTable table = ds.Tables[0];
            if (((table != null) && (table.Rows.Count > 0)) && !(StrFileName.Trim() == ""))
            {
                string str = this.webSite + this.storgePath;
                XmlDocument doc = new XmlDocument();
                XmlDeclaration newChild = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(newChild);
                XmlElement element = doc.CreateElement("", "root", "");
                doc.AppendChild(element);
                FeedGlobals.CreateXMlNodeValue(doc, element, "version", StrVersion);
                FeedGlobals.CreateXMlNodeValue(doc, element, "modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                FeedGlobals.CreateXMlNodeValue(doc, element, "seller_id", this.seller_ID);
                FeedGlobals.CreateXMlNodeValue(doc, element, "cat_url", str + "SellerCats.xml");
                FeedGlobals.CreateXMlNodeValue(doc, element, "dir", str + this.prefixFullPath + "/");
                XmlElement element2 = doc.CreateElement("item_ids");
                element.AppendChild(element2);
                foreach (DataRow row in table.Rows)
                {
                    if (((row != null) && (row["productId"].ToString().Trim() != "")) && this.productsList.Contains(row["productId"].ToString().Trim()))
                    {
                        XmlElement element3 = doc.CreateElement("outer_id");
                        XmlAttribute node = doc.CreateAttribute("action");
                        node.Value = "upload";
                        element3.Attributes.Append(node);
                        XmlText text = doc.CreateTextNode(row["productId"].ToString().Trim());
                        element3.AppendChild(text);
                        element2.AppendChild(element3);
                    }
                }
                if (File.Exists(this.prefixRootPath + "FullIndex.xml"))
                {
                    File.Delete(this.prefixRootPath + "FullIndex.xml");
                }
                doc.Save(this.prefixRootPath + "FullIndex.xml");
            }
        }

        public void MakeProductDetail(DataSet ds)
        {
            if ((ds != null) && (ds.Tables.Count > 0))
            {
                string path = this.prefixRootPath + this.prefixFullPath + @"\";
                string str2 = this.prefixRootPath + this.prefixFullPath + @"\";
                if (!Directory.Exists(str2))
                {
                    Directory.CreateDirectory(str2);
                }
                DataTable table = ds.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        XmlElement element3;
                        XmlText text;
                        XmlDocument doc = new XmlDocument();
                        XmlDeclaration newChild = doc.CreateXmlDeclaration(this.fullVersion, "utf-8", null);
                        doc.AppendChild(newChild);
                        XmlElement element = doc.CreateElement("", "item", "");
                        doc.AppendChild(element);
                        FeedGlobals.CreateXMlNodeValue(doc, element, "seller_id", this.seller_ID);
                        FeedGlobals.CreateXMlNodeValue(doc, element, "outer_id", row["productId"].ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "title", row["ProductName"].ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "type", "fixed");
                        FeedGlobals.CreateXMlNodeValue(doc, element, "price", Math.Round(Convert.ToDecimal(row["SalePrice"]), 2).ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "discount", "");
                        FeedGlobals.CreateXMlNodeValue(doc, element, "desc", ((row["ShortDescription"] == null) || (row["ShortDescription"] == DBNull.Value)) ? "" : row["ShortDescription"].ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "brand", ((row["brandName"] == null) || (row["brandName"] == DBNull.Value)) ? "" : row["brandName"].ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "tags", ((row["Meta_Keywords"] == DBNull.Value) || (row["Meta_Keywords"] == null)) ? "" : row["Meta_Keywords"].ToString());
                        FeedGlobals.CreateXMlNodeValue(doc, element, "image", ((row["ImageUrl1"] == null) || (row["ImageUrl1"] == DBNull.Value)) ? "" : (this.webSite + row["ImageUrl1"].ToString()));
                        XmlElement element2 = doc.CreateElement("more_images");
                        element.AppendChild(element2);
                        if (!string.IsNullOrEmpty(Convert.ToString(row["ImageUrl2"])))
                        {
                            element3 = doc.CreateElement("img");
                            text = doc.CreateTextNode(this.webSite + row["ImageUrl2"].ToString());
                            element3.AppendChild(text);
                            element2.AppendChild(element3);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(row["ImageUrl3"])))
                        {
                            element3 = doc.CreateElement("img");
                            text = doc.CreateTextNode(this.webSite + row["ImageUrl3"].ToString());
                            element3.AppendChild(text);
                            element2.AppendChild(element3);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(row["ImageUrl4"])))
                        {
                            element3 = doc.CreateElement("img");
                            text = doc.CreateTextNode(this.webSite + row["ImageUrl4"].ToString());
                            element3.AppendChild(text);
                            element2.AppendChild(element3);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(row["ImageUrl5"])))
                        {
                            element3 = doc.CreateElement("img");
                            text = doc.CreateTextNode(this.webSite + row["ImageUrl5"].ToString());
                            element3.AppendChild(text);
                            element2.AppendChild(element3);
                        }
                        FeedGlobals.CreateXMlNodeValue(doc, element, "scids", FeedGlobals.GetCategoryIds((string) row["MainCategoryPath"]));
                        FeedGlobals.CreateXMlNodeValue(doc, element, "post_fee", "0");
                        FeedGlobals.CreateXMlNodeValue(doc, element, "props", FeedGlobals.GetEtaoSku((int) row["productId"]));
                        FeedGlobals.CreateXMlNodeValue(doc, element, "showcase", "0");
                        FeedGlobals.CreateXMlNodeValue(doc, element, "href", this.webSite + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["productId"] }));
                        path = str2 + row["productId"].ToString().Trim() + ".xml";
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        doc.Save(path);
                        this.productsList.Add(row["productId"].ToString().Trim());
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}

