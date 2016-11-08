namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;

    public static class OpenIdFunction
    {
        private const string FormFormat = "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>";
        private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";

        public static string BuildMysign(Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
        {
            return Sign(CreateLinkString(dicArray) + key, sign_type, _input_charset);
        }

        public static string CreateField(string name, string strValue)
        {
            return string.Format(CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[] { name, strValue });
        }

        public static string CreateForm(string content, string action)
        {
            content = content + "<input type=\"submit\" value=\"信任登录\" style=\"display:none;\">";
            return string.Format(CultureInfo.InvariantCulture, "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[] { action, content });
        }

        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dicArray)
            {
                builder.Append(pair.Key + "=" + pair.Value + "&");
            }
            int length = builder.Length;
            builder.Remove(length - 1, 1);
            return builder.ToString();
        }

        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in dicArrayPre)
            {
                if (((pair.Key.ToLower() != "sign") && (pair.Key.ToLower() != "sign_type")) && ((pair.Value != "") && (pair.Value != null)))
                {
                    dictionary.Add(pair.Key.ToLower(), pair.Value);
                }
            }
            return dictionary;
        }

        public static string Sign(string prestr, string sign_type, string _input_charset)
        {
            StringBuilder builder = new StringBuilder(0x20);
            if (sign_type.ToUpper() == "MD5")
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return builder.ToString();
        }

        public static void Submit(string formContent)
        {
            string s = formContent + "<script>document.forms['openidform'].submit();</script>";
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }
    }
}

