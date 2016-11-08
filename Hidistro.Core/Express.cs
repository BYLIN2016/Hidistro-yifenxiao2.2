namespace Hidistro.Core
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Xml;

    public static class Express
    {
        public static string GetDataByKuaidi100(string computer, string expressNo)
        {
            HttpWebResponse response;
            string innerText = "29833628d495d7a5";
            string str2 = null;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str2 = current.Request.MapPath("~/Express.xml");
            }
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(str2))
            {
                document.Load(str2);
                XmlNode node = document.SelectSingleNode("expressapi");
                if ((node != null) && (node.ChildNodes.Count > 0))
                {
                    foreach (XmlNode node2 in node)
                    {
                        if (node2.Name == "kuaidi100")
                        {
                            innerText = node2.Attributes["key"].InnerText;
                        }
                    }
                }
            }
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, innerText));
            request.Timeout = 0x1f40;
            string str3 = "暂时没有此快递单号的信息";
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch
            {
                return str3;
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                str3 = reader.ReadToEnd().Replace("&amp;", "").Replace("&nbsp;", "").Replace("&", "");
            }
            return str3;
        }

        public static string GetDataByTaobaoTop(string computer, string expressNo)
        {
            return "暂时没有此快递单号的信息";
        }

        public static string GetExpressData(string computer, string expressNo)
        {
            if (GetExpressType() == "kuaidi100")
            {
                return GetDataByKuaidi100(computer, expressNo);
            }
            return GetDataByTaobaoTop(computer, expressNo);
        }

        public static string GetExpressType()
        {
            string innerText = "kuaidi100";
            string str2 = null;
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                str2 = current.Request.MapPath("~/Express.xml");
            }
            XmlDocument document = new XmlDocument();
            if (!string.IsNullOrEmpty(str2))
            {
                document.Load(str2);
                XmlNode node = document.SelectSingleNode("expressapi");
                if (node != null)
                {
                    innerText = node.Attributes["usetype"].InnerText;
                }
            }
            return innerText;
        }
    }
}

