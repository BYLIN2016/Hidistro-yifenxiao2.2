namespace Hidistro.Core
{
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Urls;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class Globals
    {
        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false);
        }

        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded)
                {
                    str = "&";
                }
                else
                {
                    str = "&amp;";
                }
            }
            return (url + str + querystring);
        }

        public static int[] BubbleSort(int[] r)
        {
            for (int i = 0; i < r.Length; i++)
            {
                bool flag = false;
                for (int j = r.Length - 2; j >= i; j--)
                {
                    if (r[j + 1] > r[j])
                    {
                        int num3 = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = num3;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    return r;
                }
            }
            return r;
        }

        public static void EntityCoding(object entity, bool encode)
        {
            if (entity != null)
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    if (info.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
                    {
                        if (!(info.CanWrite && info.CanRead))
                        {
                            throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
                        }
                        if (!info.PropertyType.Equals(typeof(string)))
                        {
                            throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
                        }
                        string str = info.GetValue(entity, null) as string;
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (encode)
                            {
                                info.SetValue(entity, HtmlEncode(str), null);
                            }
                            else
                            {
                                info.SetValue(entity, HtmlDecode(str), null);
                            }
                        }
                    }
                }
            }
        }

        public static string FormatMoney(decimal money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { money.ToString("F", CultureInfo.InvariantCulture) });
        }

        public static string FullPath(string local)
        {
            if (string.IsNullOrEmpty(local))
            {
                return local;
            }
            if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
            {
                return local;
            }
            if (HttpContext.Current == null)
            {
                return local;
            }
            return FullPath(HostPath(HttpContext.Current.Request.Url), local);
        }

        public static string FullPath(string hostPath, string local)
        {
            return (hostPath + local);
        }

        public static string GetAdminAbsolutePath(string path)
        {
            if (path.StartsWith("/"))
            {
                return (ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + path);
            }
            return (ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + "/" + path);
        }

        public static string GetGenerateId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        public static SiteUrls GetSiteUrls()
        {
            return SiteUrls.Instance();
        }

        public static string HostPath(Uri uri)
        {
            if (uri == null)
            {
                return string.Empty;
            }
            string str = (uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString(CultureInfo.InvariantCulture));
            return string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { uri.Scheme, uri.Host, str });
        }

        public static string HtmlDecode(string textToFormat)
        {
            if (string.IsNullOrEmpty(textToFormat))
            {
                return textToFormat;
            }
            return HttpUtility.HtmlDecode(textToFormat);
        }

        public static string HtmlEncode(string textToFormat)
        {
            if (string.IsNullOrEmpty(textToFormat))
            {
                return textToFormat;
            }
            return HttpUtility.HtmlEncode(textToFormat);
        }

        public static string MapPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                return current.Request.MapPath(path);
            }
            return PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
        }

        public static string PhysicalPath(string path)
        {
            if (null == path)
            {
                return string.Empty;
            }
            return (RootPath().TrimEnd(new char[] { Path.DirectorySeparatorChar }) + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[] { Path.DirectorySeparatorChar }));
        }

        public static void RedirectToSSL(HttpContext context)
        {
            if ((null != context) && !context.Request.IsSecureConnection)
            {
                Uri url = context.Request.Url;
                context.Response.Redirect("https://" + url.ToString().Substring(7));
            }
        }

        private static string RootPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string newValue = Path.DirectorySeparatorChar.ToString();
            baseDirectory = baseDirectory.Replace("/", newValue);
            string filesPath = HiConfiguration.GetConfig().FilesPath;
            if (filesPath == null)
            {
                return baseDirectory;
            }
            filesPath = filesPath.Replace("/", newValue);
            if (((filesPath.Length > 0) && filesPath.StartsWith(newValue)) && baseDirectory.EndsWith(newValue))
            {
                return (baseDirectory + filesPath.Substring(1));
            }
            return (baseDirectory + filesPath);
        }

        public static string StripAllTags(string strToStrip)
        {
            strToStrip = Regex.Replace(strToStrip, @"</p(?:\s*)>(?:\s*)<p(?:\s*)>", "\n\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, @"<br(?:\s*)/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            strToStrip = StripHtmlXmlTags(strToStrip);
            return strToStrip;
        }

        public static string StripForPreview(string content)
        {
            content = Regex.Replace(content, "<br>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br/>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<br />", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<p>", "\n", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            content = content.Replace("'", "&#39;");
            return StripHtmlXmlTags(content);
        }

        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static string StripScriptTags(string content)
        {
            content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "'javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return Regex.Replace(content, "\"javascript:", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        }

        public static string ToDelimitedString(ICollection collection, string delimiter)
        {
            if (collection == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if (collection is Hashtable)
            {
                foreach (object obj2 in ((Hashtable) collection).Keys)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is ArrayList)
            {
                foreach (object obj2 in (ArrayList) collection)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is string[])
            {
                foreach (string str in (string[]) collection)
                {
                    builder.Append(str + delimiter);
                }
            }
            if (collection is MailAddressCollection)
            {
                foreach (MailAddress address in (MailAddressCollection) collection)
                {
                    builder.Append(address.Address + delimiter);
                }
            }
            return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
        }

        public static string UnHtmlEncode(string formattedPost)
        {
            RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
            formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", options);
            formattedPost = Regex.Replace(formattedPost, "&lt;", "<", options);
            formattedPost = Regex.Replace(formattedPost, "&gt;", ">", options);
            return formattedPost;
        }

        public static string UrlDecode(string urlToDecode)
        {
            if (string.IsNullOrEmpty(urlToDecode))
            {
                return urlToDecode;
            }
            return HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
        }

        public static string UrlEncode(string urlToEncode)
        {
            if (string.IsNullOrEmpty(urlToEncode))
            {
                return urlToEncode;
            }
            return HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
        }

        public static void ValidateSecureConnection(SSLSettings ssl, HttpContext context)
        {
            if (HiConfiguration.GetConfig().SSL == ssl)
            {
                RedirectToSSL(context);
            }
        }

        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }

        public static string DomainName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return string.Empty;
                }
                return HttpContext.Current.Request.Url.Host;
            }
        }

        public static string IPAddress
        {
            get
            {
                string userHostAddress;
                HttpRequest request = HttpContext.Current.Request;
                if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    userHostAddress = request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                if (string.IsNullOrEmpty(userHostAddress))
                {
                    userHostAddress = request.UserHostAddress;
                }
                return userHostAddress;
            }
        }
    }
}

