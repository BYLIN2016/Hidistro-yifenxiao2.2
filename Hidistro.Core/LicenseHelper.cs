namespace Hidistro.Core
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Caching;

    public static class LicenseHelper
    {
        private const string PublicKeyCache = "FileCache-Publickey";

        public static string GetPublicKey()
        {
            string str = HiCache.Get("FileCache-Publickey") as string;
            if (string.IsNullOrEmpty(str))
            {
                string path = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    path = current.Request.MapPath("~/config/publickey.xml");
                }
                else
                {
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\publickey.xml");
                }
                str = File.ReadAllText(path);
                HiCache.Max("FileCache-Publickey", str, new CacheDependency(path));
            }
            return str;
        }
    }
}

