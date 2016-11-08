namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public class ProductBasePage : AdminPage
    {
        protected void DoCallback()
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            string str = base.Request.QueryString["action"];
            if (str.Equals("getPrepareData"))
            {
                int typeId = int.Parse(base.Request.QueryString["typeId"]);
                IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId);
                DataTable brandCategoriesByTypeId = ProductTypeHelper.GetBrandCategoriesByTypeId(typeId);
                if (brandCategoriesByTypeId.Rows.Count == 0)
                {
                    brandCategoriesByTypeId = ControlProvider.Instance().GetBrandCategories();
                }
                base.Response.Write(this.GenerateJsonString(attributes, brandCategoriesByTypeId));
                attributes.Clear();
            }
            else if (str.Equals("getMemberGradeList"))
            {
                IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
                if ((memberGrades == null) || (memberGrades.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{\"Status\":\"OK\",\"MemberGrades\":[");
                    foreach (MemberGradeInfo info in memberGrades)
                    {
                        builder.Append("{");
                        builder.AppendFormat("\"GradeId\":\"{0}\",", info.GradeId);
                        builder.AppendFormat("\"Name\":\"{0}\",", info.Name);
                        builder.AppendFormat("\"Discount\":\"{0}\"", info.Discount);
                        builder.Append("},");
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.Append("]}");
                    base.Response.Write(builder.ToString());
                }
            }
            else if (str.Equals("getDistributorGradeList"))
            {
                DataTable distributorGrades = DistributorHelper.GetDistributorGrades();
                if ((distributorGrades == null) || (distributorGrades.Rows.Count == 0))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                }
                else
                {
                    StringBuilder builder2 = new StringBuilder();
                    builder2.Append("{\"Status\":\"OK\",\"DistributorGrades\":[");
                    foreach (DataRow row in distributorGrades.Rows)
                    {
                        builder2.Append("{");
                        builder2.AppendFormat("\"GradeId\":\"{0}\",", (int) row["GradeId"]);
                        builder2.AppendFormat("\"Name\":\"{0}\",", (string) row["Name"]);
                        builder2.AppendFormat("\"Discount\":\"{0}\"", (int) row["Discount"]);
                        builder2.Append("},");
                    }
                    builder2.Remove(builder2.Length - 1, 1);
                    builder2.Append("]}");
                    base.Response.Write(builder2.ToString());
                }
            }
            base.Response.End();
        }

        protected string DownRemotePic(string productDescrip)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            string str = string.Format("/Storage/master/gallery/{0}/", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString());
            string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            IList<string> outsiteLinkImgs = this.GetOutsiteLinkImgs(productDescrip);
            if (outsiteLinkImgs.Count > 0)
            {
                foreach (string str3 in outsiteLinkImgs)
                {
                    WebClient client = new WebClient();
                    string str4 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);
                    string str5 = str3.Substring(str3.LastIndexOf('.'));
                    try
                    {
                        client.DownloadFile(str3, path + str4 + str5);
                        productDescrip = productDescrip.Replace(str3, Globals.ApplicationPath + str + str4 + str5);
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return productDescrip;
        }

        private string GenerateBrandString(DataTable tb)
        {
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in tb.Rows)
            {
                builder.Append("{");
                builder.AppendFormat("\"BrandId\":\"{0}\",\"BrandName\":\"{1}\"", row["BrandID"], row["BrandName"]);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        private string GenerateJsonString(IList<AttributeInfo> attributes, DataTable tbBrandCategories)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            StringBuilder builder3 = new StringBuilder();
            StringBuilder builder4 = new StringBuilder();
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if ((attributes != null) && (attributes.Count > 0))
            {
                builder2.Append("\"Attributes\":[");
                builder3.Append("\"SKUs\":[");
                foreach (AttributeInfo info in attributes)
                {
                    if (info.UsageMode == AttributeUseageMode.Choose)
                    {
                        flag2 = true;
                        builder3.Append("{");
                        builder3.AppendFormat("\"Name\":\"{0}\",", info.AttributeName);
                        builder3.AppendFormat("\"AttributeId\":\"{0}\",", info.AttributeId.ToString(CultureInfo.InvariantCulture));
                        builder3.AppendFormat("\"UseAttributeImage\":\"{0}\",", info.UseAttributeImage ? 1 : 0);
                        builder3.AppendFormat("\"SKUValues\":[{0}]", this.GenerateValueItems(info.AttributeValues));
                        builder3.Append("},");
                    }
                    else if ((info.UsageMode == AttributeUseageMode.View) || (info.UsageMode == AttributeUseageMode.MultiView))
                    {
                        flag = true;
                        builder2.Append("{");
                        builder2.AppendFormat("\"Name\":\"{0}\",", info.AttributeName);
                        builder2.AppendFormat("\"AttributeId\":\"{0}\",", info.AttributeId.ToString(CultureInfo.InvariantCulture));
                        builder2.AppendFormat("\"UsageMode\":\"{0}\",", ((int) info.UsageMode).ToString());
                        builder2.AppendFormat("\"AttributeValues\":[{0}]", this.GenerateValueItems(info.AttributeValues));
                        builder2.Append("},");
                    }
                }
                if (builder2.Length > 14)
                {
                    builder2.Remove(builder2.Length - 1, 1);
                }
                if (builder3.Length > 8)
                {
                    builder3.Remove(builder3.Length - 1, 1);
                }
                builder2.Append("]");
                builder3.Append("]");
            }
            if ((tbBrandCategories != null) && (tbBrandCategories.Rows.Count > 0))
            {
                flag3 = true;
                builder4.AppendFormat("\"BrandCategories\":[{0}]", this.GenerateBrandString(tbBrandCategories));
            }
            builder.Append("{\"HasAttribute\":\"" + flag.ToString() + "\",");
            builder.Append("\"HasSKU\":\"" + flag2.ToString() + "\",");
            builder.Append("\"HasBrandCategory\":\"" + flag3.ToString() + "\",");
            if (flag)
            {
                builder.Append(builder2.ToString()).Append(",");
            }
            if (flag2)
            {
                builder.Append(builder3.ToString()).Append(",");
            }
            if (flag3)
            {
                builder.Append(builder4.ToString()).Append(",");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("}");
            return builder.ToString();
        }

        private string GenerateValueItems(IList<AttributeValueInfo> values)
        {
            if ((values == null) || (values.Count == 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (AttributeValueInfo info in values)
            {
                builder.Append("{");
                builder.AppendFormat("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"", info.ValueId.ToString(CultureInfo.InvariantCulture), info.ValueStr);
                builder.Append("},");
            }
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected Dictionary<int, IList<int>> GetAttributes(string attributesXml)
        {
            XmlDocument document = new XmlDocument();
            Dictionary<int, IList<int>> dictionary = null;
            try
            {
                document.LoadXml(attributesXml);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                dictionary = new Dictionary<int, IList<int>>();
                foreach (XmlNode node in list)
                {
                    int key = int.Parse(node.Attributes["attributeId"].Value);
                    IList<int> list2 = new List<int>();
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        if (node2.Attributes["valueId"].Value != "")
                        {
                            list2.Add(int.Parse(node2.Attributes["valueId"].Value));
                        }
                    }
                    if (list2.Count > 0)
                    {
                        dictionary.Add(key, list2);
                    }
                }
            }
            catch
            {
            }
            return dictionary;
        }

        protected void GetDistributorPrices(SKUItem sku, string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                foreach (XmlNode node in document.DocumentElement.SelectNodes("//grande"))
                {
                    if (!string.IsNullOrEmpty(node.Attributes["price"].Value) && (node.Attributes["price"].Value.Trim().Length != 0))
                    {
                        sku.DistributorPrices.Add(int.Parse(node.Attributes["id"].Value), decimal.Parse(node.Attributes["price"].Value.Trim()));
                    }
                }
            }
        }

        protected void GetMemberPrices(SKUItem sku, string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                foreach (XmlNode node in document.DocumentElement.SelectNodes("//grande"))
                {
                    if (!string.IsNullOrEmpty(node.Attributes["price"].Value) && (node.Attributes["price"].Value.Trim().Length != 0))
                    {
                        sku.MemberPrices.Add(int.Parse(node.Attributes["id"].Value), decimal.Parse(node.Attributes["price"].Value.Trim()));
                    }
                }
            }
        }

        private IList<string> GetOutsiteLinkImgs(string html)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            IList<string> list = new List<string>();
            MatchCollection matchs = new Regex("(src)[^>]*[^/].(?:jpg|bmp|gif|png)(?:\"|') ", RegexOptions.IgnoreCase).Matches(html);
            string item = "";
            for (int i = 0; i < matchs.Count; i++)
            {
                item = matchs[i].Value.Replace(@"\", "").Replace("\"", "").Replace("'", "").Trim().Substring(4);
                if ((item.ToLower(CultureInfo.InvariantCulture).IndexOf(siteSettings.SiteUrl.ToLower(CultureInfo.InvariantCulture)) == -1) && (item.ToLower(CultureInfo.InvariantCulture).IndexOf(masterSettings.SiteUrl.ToLower(CultureInfo.InvariantCulture)) == -1))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        protected Dictionary<string, SKUItem> GetSkus(string skusXml)
        {
            XmlDocument document = new XmlDocument();
            Dictionary<string, SKUItem> dictionary = null;
            try
            {
                document.LoadXml(skusXml);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                dictionary = new Dictionary<string, SKUItem>();
                foreach (XmlNode node in list)
                {
                    SKUItem item2 = new SKUItem();
                    item2.SKU = node.Attributes["skuCode"].Value;
                    item2.SalePrice = decimal.Parse(node.Attributes["salePrice"].Value);
                    item2.CostPrice = (node.Attributes["costPrice"].Value.Length > 0) ? decimal.Parse(node.Attributes["costPrice"].Value) : 0M;
                    item2.PurchasePrice = decimal.Parse(node.Attributes["purchasePrice"].Value);
                    item2.Stock = int.Parse(node.Attributes["qty"].Value);
                    item2.AlertStock = int.Parse(node.Attributes["alertQty"].Value);
                    item2.Weight = (node.Attributes["weight"].Value.Length > 0) ? decimal.Parse(node.Attributes["weight"].Value) : 0M;
                    SKUItem item = item2;
                    string str = "";
                    foreach (XmlNode node2 in node.SelectSingleNode("skuFields").ChildNodes)
                    {
                        str = str + node2.Attributes["valueId"].Value + "_";
                        item.SkuItems.Add(int.Parse(node2.Attributes["attributeId"].Value), int.Parse(node2.Attributes["valueId"].Value));
                    }
                    XmlNode node3 = node.SelectSingleNode("memberPrices");
                    if ((node3 != null) && (node3.ChildNodes.Count > 0))
                    {
                        foreach (XmlNode node4 in node3.ChildNodes)
                        {
                            if (!string.IsNullOrEmpty(node4.Attributes["price"].Value) && (node4.Attributes["price"].Value.Trim().Length != 0))
                            {
                                item.MemberPrices.Add(int.Parse(node4.Attributes["id"].Value), decimal.Parse(node4.Attributes["price"].Value.Trim()));
                            }
                        }
                    }
                    XmlNode node5 = node.SelectSingleNode("distributorPrices");
                    if ((node5 != null) && (node5.ChildNodes.Count > 0))
                    {
                        foreach (XmlNode node6 in node5.ChildNodes)
                        {
                            if (!string.IsNullOrEmpty(node6.Attributes["price"].Value) && (node6.Attributes["price"].Value.Trim().Length != 0))
                            {
                                item.DistributorPrices.Add(int.Parse(node6.Attributes["id"].Value), decimal.Parse(node6.Attributes["price"].Value.Trim()));
                            }
                        }
                    }
                    item.SkuId = str.Substring(0, str.Length - 1);
                    dictionary.Add(item.SkuId, item);
                }
                return dictionary;
            }
            catch
            {
                return null;
            }
        }
    }
}

