namespace Hidistro.Entities
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;
    using System.Xml;

    public static class TagsHelper
    {
        public static System.Xml.XmlNode FindAdNode(int id, string type)
        {
            return GetAdDocument().SelectSingleNode(string.Format("//Ad[@Id='{0}' and @Type='{1}']", id, type));
        }

        public static System.Xml.XmlNode FindCommentNode(int id, string type)
        {
            return GetCommentDocument().SelectSingleNode(string.Format("//Comment[@Id='{0}' and @Type='{1}']", id, type));
        }

        public static System.Xml.XmlNode FindHeadMenuNode(int id)
        {
            return GetHeadMenuDocument().SelectSingleNode(string.Format("//Menu[@Id='{0}']", id));
        }

        public static System.Xml.XmlNode FindProductNode(int subjectId, string type)
        {
            return GetProductDocument().SelectSingleNode(string.Format("//Subject[@SubjectId='{0}' and @Type='{1}']", subjectId, type));
        }

        private static XmlDocument GetAdDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
            string key = "AdFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                HttpContext context = HiContext.Current.Context;
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        private static XmlDocument GetCommentDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
            string key = "CommentFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                HttpContext context = HiContext.Current.Context;
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        private static XmlDocument GetHeadMenuDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
            string key = "HeadMenuCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                HttpContext context = HiContext.Current.Context;
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        private static XmlDocument GetProductDocument()
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
            string key = "ProductFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                HttpContext context = HiContext.Current.Context;
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        private static void RemoveAdNodeCache()
        {
            string key = "AdFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }

        private static void RemoveCommentNodeCache()
        {
            string key = "CommentFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }

        private static void RemoveHeadMenuCache()
        {
            string key = "HeadMenuCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }

        private static void RemoveProductNodeCache()
        {
            string key = "ProductFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            HiCache.Remove(key);
        }

        public static bool UpdateAdNode(int aId, string type, Dictionary<string, string> adnode)
        {
            bool flag = false;
            XmlDocument adDocument = GetAdDocument();
            System.Xml.XmlNode node = FindAdNode(aId, type);
            if (node == null)
            {
                return flag;
            }
            foreach (KeyValuePair<string, string> pair in adnode)
            {
                node.Attributes[pair.Key].Value = pair.Value;
            }
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
            adDocument.Save(filename);
            RemoveAdNodeCache();
            return true;
        }

        public static bool UpdateCommentNode(int commentId, string type, Dictionary<string, string> commentnode)
        {
            bool flag = false;
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
            XmlDocument commentDocument = GetCommentDocument();
            System.Xml.XmlNode node = FindCommentNode(commentId, type);
            if (node == null)
            {
                return flag;
            }
            foreach (KeyValuePair<string, string> pair in commentnode)
            {
                node.Attributes[pair.Key].Value = pair.Value;
            }
            commentDocument.Save(filename);
            RemoveCommentNodeCache();
            return true;
        }

        public static bool UpdateHeadMenuNode(int menuId, Dictionary<string, string> headmenunode)
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
            bool flag = false;
            XmlDocument commentDocument = GetCommentDocument();
            System.Xml.XmlNode node = FindHeadMenuNode(menuId);
            if (node == null)
            {
                return flag;
            }
            foreach (KeyValuePair<string, string> pair in headmenunode)
            {
                node.Attributes[pair.Key].Value = pair.Value;
            }
            commentDocument.Save(filename);
            RemoveHeadMenuCache();
            return true;
        }

        public static bool UpdateProductNode(int subjectId, string type, Dictionary<string, string> simplenode)
        {
            string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
            bool flag = false;
            XmlDocument productDocument = GetProductDocument();
            System.Xml.XmlNode node = FindProductNode(subjectId, type);
            if (node == null)
            {
                return flag;
            }
            foreach (KeyValuePair<string, string> pair in simplenode)
            {
                node.Attributes[pair.Key].Value = pair.Value;
            }
            productDocument.Save(filename);
            RemoveProductNodeCache();
            return true;
        }
    }
}

