namespace Hidistro.UI.Web.OpenID
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Xml;

    public class LogisticsAddress : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string openIdType = "hishop.plugins.openid.alipay.alipayservice";
            OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);
            if (openIdSettings != null)
            {
                string str2 = base.Request.QueryString["alipaytoken"];
                XmlDocument document = new XmlDocument();
                document.LoadXml(HiCryptographer.Decrypt(openIdSettings.Settings));
                SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
                dicArrayPre.Add("service", "user.logistics.address.query");
                dicArrayPre.Add("partner", document.FirstChild.SelectSingleNode("Partner").InnerText);
                dicArrayPre.Add("_input_charset", "utf-8");
                dicArrayPre.Add("return_url", Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("LogisticsAddress_url")));
                dicArrayPre.Add("token", str2);
                Dictionary<string, string> dicArray = OpenIdFunction.FilterPara(dicArrayPre);
                string str3 = OpenIdFunction.BuildMysign(dicArray, document.FirstChild.SelectSingleNode("Key").InnerText, "MD5", "utf-8");
                dicArray.Add("sign", str3);
                dicArray.Add("sign_type", "MD5");
                StringBuilder builder = new StringBuilder();
                foreach (KeyValuePair<string, string> pair in dicArray)
                {
                    builder.Append(OpenIdFunction.CreateField(pair.Key, pair.Value));
                }
                dicArrayPre.Clear();
                dicArray.Clear();
                OpenIdFunction.Submit(OpenIdFunction.CreateForm(builder.ToString(), "https://mapi.alipay.com/gateway.do?_input_charset=utf-8"));
            }
        }
    }
}

