namespace Hidistro.Entities
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Web;

    public static class ExpressHelper
    {
        private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");

        public static void AddExpress(string name, string kuaidi100Code, string taobaoCode)
        {
            XmlDocument xmlNode = GetXmlNode();
            System.Xml.XmlNode node = xmlNode.SelectSingleNode("companys");
            XmlElement newChild = xmlNode.CreateElement("company");
            newChild.SetAttribute("name", name);
            newChild.SetAttribute("Kuaidi100Code", kuaidi100Code);
            newChild.SetAttribute("TaobaoCode", taobaoCode);
            node.AppendChild(newChild);
            xmlNode.Save(path);
        }

        public static void DeleteExpress(string name)
        {
            XmlDocument xmlNode = GetXmlNode();
            System.Xml.XmlNode node = xmlNode.SelectSingleNode("companys");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                if (node2.Attributes["name"].Value == name)
                {
                    node.RemoveChild(node2);
                    break;
                }
            }
            xmlNode.Save(path);
        }

        public static ExpressCompanyInfo FindNode(string company)
        {
            ExpressCompanyInfo info = null;
            XmlDocument xmlNode = GetXmlNode();
            string xpath = string.Format("//company[@name='{0}']", company);
            System.Xml.XmlNode node = xmlNode.SelectSingleNode(xpath);
            if (node != null)
            {
                info = new ExpressCompanyInfo();
                info.Name = company;
                info.Kuaidi100Code = node.Attributes["Kuaidi100Code"].Value;
                info.TaobaoCode = node.Attributes["TaobaoCode"].Value;
            }
            return info;
        }

        public static ExpressCompanyInfo FindNodeByCode(string code)
        {
            ExpressCompanyInfo info = null;
            XmlDocument xmlNode = GetXmlNode();
            string xpath = string.Format("//company[@TaobaoCode='{0}']", code);
            System.Xml.XmlNode node = xmlNode.SelectSingleNode(xpath);
            if (node != null)
            {
                info = new ExpressCompanyInfo();
                info.Name = node.Attributes["name"].Value;
                info.Kuaidi100Code = node.Attributes["Kuaidi100Code"].Value;
                info.TaobaoCode = code;
            }
            return info;
        }

        public static IList<ExpressCompanyInfo> GetAllExpress()
        {
            IList<ExpressCompanyInfo> list = new List<ExpressCompanyInfo>();
            System.Xml.XmlNode node = GetXmlNode().SelectSingleNode("companys");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                ExpressCompanyInfo item = new ExpressCompanyInfo();
                item.Name = node2.Attributes["name"].Value;
                item.Kuaidi100Code = node2.Attributes["Kuaidi100Code"].Value;
                item.TaobaoCode = node2.Attributes["TaobaoCode"].Value;
                list.Add(item);
            }
            return list;
        }

        public static IList<string> GetAllExpressName()
        {
            IList<string> list = new List<string>();
            System.Xml.XmlNode node = GetXmlNode().SelectSingleNode("companys");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                list.Add(node2.Attributes["name"].Value);
            }
            return list;
        }

        public static string GetDataByKuaidi100(string computer, string expressNo)
        {
            HttpWebResponse response;
            string str = "29833628d495d7a5";
            System.Xml.XmlNode node = GetXmlNode().SelectSingleNode("companys");
            if (node != null)
            {
                str = node.Attributes["Kuaidi100NewKey"].Value;
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, str));
            request.Timeout = 0x1f40;
            string str2 = "暂时没有此快递单号的信息";
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch
            {
                return str2;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                str2 = reader.ReadToEnd().Replace("&amp;", "").Replace("&nbsp;", "").Replace("&", "");
            }
            return str2;
        }

        public static string GetExpressData(string computer, string expressNo)
        {
            return GetDataByKuaidi100(computer, expressNo);
        }

        public static DataTable GetExpressTable()
        {
            DataTable table = new DataTable();
            System.Xml.XmlNode node = GetXmlNode().SelectSingleNode("companys");
            table.Columns.Add("Name");
            table.Columns.Add("Kuaidi100Code");
            table.Columns.Add("TaobaoCode");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                DataRow row = table.NewRow();
                row["Name"] = node2.Attributes["name"].Value;
                row["Kuaidi100Code"] = node2.Attributes["Kuaidi100Code"].Value;
                row["TaobaoCode"] = node2.Attributes["TaobaoCode"].Value;
                table.Rows.Add(row);
            }
            return table;
        }

        private static XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(path))
            {
                document.Load(path);
            }
            return document;
        }

        public static bool IsExitExpress(string name)
        {
            System.Xml.XmlNode node = GetXmlNode().SelectSingleNode("companys");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                if (node2.Attributes["name"].Value == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode)
        {
            XmlDocument xmlNode = GetXmlNode();
            System.Xml.XmlNode node = xmlNode.SelectSingleNode("companys");
            foreach (System.Xml.XmlNode node2 in node.ChildNodes)
            {
                if (node2.Attributes["name"].Value == oldcompanyname)
                {
                    node2.Attributes["name"].Value = name;
                    node2.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
                    node2.Attributes["TaobaoCode"].Value = taobaoCode;
                    break;
                }
            }
            xmlNode.Save(path);
        }
    }
}

