namespace Hidistro.Entities
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Caching;
    using System.Xml;

    public static class RegionHelper
    {
        private static System.Xml.XmlNode FindNode(int id)
        {
            string str = id.ToString(CultureInfo.InvariantCulture);
            string xpath = string.Format("//county[@id='{0}']", str);
            XmlDocument regionDocument = GetRegionDocument();
            System.Xml.XmlNode node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            xpath = string.Format("//city[@id='{0}']", str);
            node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            xpath = string.Format("//province[@id='{0}']", str);
            node = regionDocument.SelectSingleNode(xpath);
            if (node != null)
            {
                return node;
            }
            return null;
        }

        public static Dictionary<int, string> GetAllProvinces()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            XmlNodeList list = GetRegionDocument().SelectNodes("//province");
            foreach (System.Xml.XmlNode node in list)
            {
                dictionary.Add(int.Parse(node.Attributes["id"].Value), node.Attributes["name"].Value);
            }
            return dictionary;
        }

        private static Dictionary<int, string> GetChildList(string xpath)
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            System.Xml.XmlNode node = GetRegionDocument().SelectSingleNode(xpath);
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                dictionary.Add(int.Parse(node2.Attributes["id"].Value), node2.Attributes["name"].Value);
            }
            return dictionary;
        }

        public static Dictionary<int, string> GetCitys(int provinceId)
        {
            return GetChildList(string.Format("root/region/province[@id='{0}']", provinceId.ToString(CultureInfo.InvariantCulture)));
        }

        public static Dictionary<int, string> GetCountys(int cityId)
        {
            return GetChildList(string.Format("root/region/province/city[@id='{0}']", cityId.ToString(CultureInfo.InvariantCulture)));
        }

        public static string GetFullPath(int currentRegionId)
        {
            System.Xml.XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return string.Empty;
            }
            string str = node.Attributes["id"].Value;
            for (System.Xml.XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                str = node2.Attributes["id"].Value + "," + str;
            }
            return str;
        }

        public static string GetFullRegion(int currentRegionId, string separator)
        {
            System.Xml.XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return string.Empty;
            }
            string str = node.Attributes["name"].Value;
            for (System.Xml.XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                str = node2.Attributes["name"].Value + separator + str;
            }
            return str;
        }

        public static Dictionary<int, string> GetProvinces(int regionId)
        {
            return GetChildList(string.Format("root/region[@id='{0}']", regionId.ToString(CultureInfo.InvariantCulture)));
        }

        public static System.Xml.XmlNode GetRegion(int regionId)
        {
            return FindNode(regionId);
        }

        private static XmlDocument GetRegionDocument()
        {
            XmlDocument document = HiCache.Get("FileCache-Regions") as XmlDocument;
            if (document == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + "/config/region.config");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max("FileCache-Regions", document, new CacheDependency(filename));
            }
            return document;
        }

        public static int GetRegionId(string county, string city, string province)
        {
            string xpath = string.Format("//province[@name='{0}']", province);
            System.Xml.XmlNode node = GetRegionDocument().SelectSingleNode(xpath);
            if (node != null)
            {
                int num = int.Parse(node.Attributes["id"].Value);
                xpath = string.Format("//province[@id='{0}']/city[@name='{1}']", num, city);
                node = node.SelectSingleNode(xpath);
                if (node != null)
                {
                    num = int.Parse(node.Attributes["id"].Value);
                    xpath = string.Format("//city[@id='{0}']/county[@name='{1}']", num, county);
                    node = node.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        num = int.Parse(node.Attributes["id"].Value);
                    }
                }
                return num;
            }
            return 0;
        }

        public static Dictionary<int, string> GetRegions()
        {
            return GetChildList("root");
        }

        public static int GetTopRegionId(int currentRegionId)
        {
            System.Xml.XmlNode node = FindNode(currentRegionId);
            if (node == null)
            {
                return 0;
            }
            int num = currentRegionId;
            for (System.Xml.XmlNode node2 = node.ParentNode; node2.Name != "region"; node2 = node2.ParentNode)
            {
                num = int.Parse(node2.Attributes["id"].Value);
            }
            return num;
        }
    }
}

