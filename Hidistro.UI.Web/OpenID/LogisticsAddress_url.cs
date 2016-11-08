namespace Hidistro.UI.Web.OpenID
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Xml;

    public class LogisticsAddress_url : Page
    {
        protected HtmlForm form1;

        private SortedDictionary<string, string> GetRequestPost()
        {
            int index = 0;
            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();
            string[] allKeys = base.Request.Form.AllKeys;
            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], base.Request.Form[allKeys[index]]);
            }
            return dictionary;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int num = 0;
            SortedDictionary<string, string> requestPost = this.GetRequestPost();
            if (requestPost.Count > 0)
            {
                string openIdType = "hishop.plugins.openid.alipay.alipayservice";
                OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);
                if (openIdSettings == null)
                {
                    base.Response.Write("登录失败，没有找到对应的插件配置信息。");
                    return;
                }
                XmlDocument document = new XmlDocument();
                document.LoadXml(HiCryptographer.Decrypt(openIdSettings.Settings));
                AliPayNotify notify = new AliPayNotify(requestPost, base.Request.Form["notify_id"], document.FirstChild.SelectSingleNode("Partner").InnerText, document.FirstChild.SelectSingleNode("Key").InnerText);
                string responseTxt = notify.ResponseTxt;
                string str3 = base.Request.Form["sign"];
                string mysign = notify.Mysign;
                if ((responseTxt == "true") && (str3 == mysign))
                {
                    string str5 = base.Request.Form["receive_address"];
                    if (!string.IsNullOrEmpty(str5))
                    {
                        XmlDocument document2 = new XmlDocument();
                        document2.LoadXml(str5);
                        ShippingAddressInfo shippingAddress = new ShippingAddressInfo();
                        shippingAddress.UserId = HiContext.Current.User.UserId;
                        if ((document2.SelectSingleNode("/receiveAddress/address") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/address").InnerText))
                        {
                            shippingAddress.Address = Globals.HtmlEncode(document2.SelectSingleNode("/receiveAddress/address").InnerText);
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/fullname") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/fullname").InnerText))
                        {
                            shippingAddress.ShipTo = Globals.HtmlEncode(document2.SelectSingleNode("/receiveAddress/fullname").InnerText);
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/post") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/post").InnerText))
                        {
                            shippingAddress.Zipcode = document2.SelectSingleNode("/receiveAddress/post").InnerText;
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/mobile_phone") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/mobile_phone").InnerText))
                        {
                            shippingAddress.CellPhone = document2.SelectSingleNode("/receiveAddress/mobile_phone").InnerText;
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/phone") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/phone").InnerText))
                        {
                            shippingAddress.TelPhone = document2.SelectSingleNode("/receiveAddress/phone").InnerText;
                        }
                        string innerText = string.Empty;
                        string str7 = string.Empty;
                        string str8 = string.Empty;
                        if ((document2.SelectSingleNode("/receiveAddress/area") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/area").InnerText))
                        {
                            innerText = document2.SelectSingleNode("/receiveAddress/area").InnerText;
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/city") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/city").InnerText))
                        {
                            str7 = document2.SelectSingleNode("/receiveAddress/city").InnerText;
                        }
                        if ((document2.SelectSingleNode("/receiveAddress/prov") != null) && !string.IsNullOrEmpty(document2.SelectSingleNode("/receiveAddress/prov").InnerText))
                        {
                            str8 = document2.SelectSingleNode("/receiveAddress/prov").InnerText;
                        }
                        if ((string.IsNullOrEmpty(innerText) && string.IsNullOrEmpty(str7)) && string.IsNullOrEmpty(str8))
                        {
                            shippingAddress.RegionId = 0;
                        }
                        else
                        {
                            shippingAddress.RegionId = RegionHelper.GetRegionId(innerText, str7, str8);
                        }
                        SiteSettings siteSettings = HiContext.Current.SiteSettings;
                        if (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) < HiContext.Current.Config.ShippingAddressQuantity)
                        {
                            num = PersonalHelper.AddShippingAddress(shippingAddress);
                        }
                    }
                }
            }
            this.Page.Response.Redirect(Globals.ApplicationPath + "/SubmmitOrder.aspx?shippingId=" + num);
        }
    }
}

