namespace Hidistro.Jobs
{
    using Hidistro.Core;
    using Hidistro.Core.Jobs;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Xml;

    public class SiteMapJob : IJob
    {
        private Database database = DatabaseFactory.CreateDatabase();
        private string indexxml;
        private string prourl;
        private List<string> sitemaps = new List<string>();
        private SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
        private string webroot;
        private string weburl;

        public SiteMapJob()
        {
            string home = Globals.GetSiteUrls().Home;
            if (home == "/")
            {
                home = "";
            }
            else
            {
                home = "/" + home.Replace("/", "");
            }
            this.prourl = "http://" + this.siteSettings.SiteUrl;
            this.weburl = "http://" + this.siteSettings.SiteUrl + home;
            this.sitemaps.Add(this.weburl + "/sitemap1.xml");
            this.sitemaps.Add(this.weburl + "/sitemap2.xml");
            this.sitemaps.Add(this.weburl + "/sitemap3.xml");
            this.indexxml = this.weburl + "/sitemapindex.xml";
            this.webroot = Globals.MapPath("/" + Globals.ApplicationPath);
        }

        public void CreateArticleXml()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            document.AppendChild(element);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ArticleId from dbo.Hishop_Articles where IsRelease='1'");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    XmlElement element2 = document.CreateElement("url", element.NamespaceURI);
                    XmlElement element3 = document.CreateElement("loc", element2.NamespaceURI);
                    XmlText text = document.CreateTextNode(this.prourl + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { Convert.ToInt32(reader["ArticleId"]) }));
                    element3.AppendChild(text);
                    XmlElement element4 = document.CreateElement("lastmod", element2.NamespaceURI);
                    XmlText text2 = document.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
                    element4.AppendChild(text2);
                    XmlElement element5 = document.CreateElement("changefreq", element2.NamespaceURI);
                    element5.InnerText = "daily";
                    XmlElement element6 = document.CreateElement("priority", element2.NamespaceURI);
                    element6.InnerText = "1.0";
                    element2.AppendChild(element3);
                    element2.AppendChild(element4);
                    element2.AppendChild(element5);
                    element2.AppendChild(element6);
                    element.AppendChild(element2);
                }
            }
            if (File.Exists(this.webroot + "/sitemap3.xml"))
            {
                File.Delete(this.webroot + "/sitemap3.xml");
            }
            document.Save(this.webroot + "/sitemap3.xml");
        }

        public void CreateCateXml()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            document.AppendChild(element);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select CategoryId,RewriteName from Hishop_Categories");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    XmlElement element2 = document.CreateElement("url", element.NamespaceURI);
                    XmlElement element3 = document.CreateElement("loc", element2.NamespaceURI);
                    XmlText text = document.CreateTextNode(this.prourl + Globals.GetSiteUrls().SubCategory(Convert.ToInt32(reader["CategoryId"]), reader["RewriteName"]));
                    element3.AppendChild(text);
                    XmlElement element4 = document.CreateElement("lastmod", element2.NamespaceURI);
                    XmlText text2 = document.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
                    element4.AppendChild(text2);
                    XmlElement element5 = document.CreateElement("changefreq", element2.NamespaceURI);
                    element5.InnerText = "daily";
                    XmlElement element6 = document.CreateElement("priority", element2.NamespaceURI);
                    element6.InnerText = "1.0";
                    element2.AppendChild(element3);
                    element2.AppendChild(element4);
                    element2.AppendChild(element5);
                    element2.AppendChild(element6);
                    element.AppendChild(element2);
                }
            }
            if (File.Exists(this.webroot + "/sitemap1.xml"))
            {
                File.Delete(this.webroot + "/sitemap1.xml");
            }
            document.Save(this.webroot + "/sitemap1.xml");
        }

        public void CreateIndexXml()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("", "sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
            document.AppendChild(element);
            foreach (string str in this.sitemaps)
            {
                XmlElement element2 = document.CreateElement("sitemap", element.NamespaceURI);
                XmlElement element3 = document.CreateElement("loc", element2.NamespaceURI);
                XmlText text = document.CreateTextNode(str);
                element3.AppendChild(text);
                XmlElement element4 = document.CreateElement("lastmod", element2.NamespaceURI);
                XmlText text2 = document.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
                element4.AppendChild(text2);
                element2.AppendChild(element3);
                element2.AppendChild(element4);
                element.AppendChild(element2);
            }
            if (File.Exists(this.webroot + "/sitemapindex.xml"))
            {
                File.Delete(this.webroot + "/sitemapindex.xml");
            }
            document.Save(this.webroot + "/sitemapindex.xml");
        }

        public void CreateProductXml()
        {
            DbCommand sqlStringCommand;
            int num;
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(newChild);
            XmlElement element = document.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            document.AppendChild(element);
            if (int.TryParse(this.siteSettings.SiteMapNum, out num) && (num > 0))
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select top " + num + " productid from dbo.Hishop_Products where salestatus=1  order by productid desc");
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("select top  1000 productid from dbo.Hishop_Products where salestatus=1 order by productid desc");
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    XmlElement element2 = document.CreateElement("url", element.NamespaceURI);
                    XmlElement element3 = document.CreateElement("loc", element2.NamespaceURI);
                    XmlText text = document.CreateTextNode(this.prourl + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { Convert.ToInt32(reader["productid"]) }));
                    element3.AppendChild(text);
                    XmlElement element4 = document.CreateElement("lastmod", element2.NamespaceURI);
                    XmlText text2 = document.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
                    element4.AppendChild(text2);
                    XmlElement element5 = document.CreateElement("changefreq", element2.NamespaceURI);
                    element5.InnerText = "daily";
                    XmlElement element6 = document.CreateElement("priority", element2.NamespaceURI);
                    element6.InnerText = "1.0";
                    element2.AppendChild(element3);
                    element2.AppendChild(element4);
                    element2.AppendChild(element5);
                    element2.AppendChild(element6);
                    element.AppendChild(element2);
                }
            }
            if (File.Exists(this.webroot + "/sitemap2.xml"))
            {
                File.Delete(this.webroot + "/sitemap2.xml");
            }
            document.Save(this.webroot + "/sitemap2.xml");
        }

        public void Execute(XmlNode node)
        {
            this.CreateCateXml();
            this.CreateProductXml();
            this.CreateArticleXml();
            this.CreateIndexXml();
        }
    }
}

