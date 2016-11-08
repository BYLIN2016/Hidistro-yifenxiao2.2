namespace Hidistro.UI.Web.API
{
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class MessageInfo
    {
        public static string GetOrderSkuContent(string skucontent)
        {
            string str = "";
            skucontent = skucontent.Replace("；", "");
            if (!string.IsNullOrEmpty(skucontent) && (skucontent.IndexOf("：") >= 0))
            {
                str = skucontent.Substring(0, skucontent.IndexOf("："));
            }
            if (!(str == ""))
            {
                return str;
            }
            return "不存在";
        }

        public static Dictionary<string, string> GetShippingRegion(string regionstr)
        {
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            dictionary2.Add("Province", "");
            dictionary2.Add("City", "");
            dictionary2.Add("District", "");
            Dictionary<string, string> dictionary = dictionary2;
            string[] strArray = regionstr.Split(new char[] { '，' });
            if (strArray.Length >= 1)
            {
                dictionary["Province"] = strArray[0].ToString();
            }
            if (strArray.Length >= 2)
            {
                dictionary["City"] = strArray[1].ToString();
            }
            if (strArray.Length >= 3)
            {
                dictionary["District"] = strArray[2].ToString();
            }
            return dictionary;
        }

        public static string GetSkuContent(string skuId)
        {
            string str = "";
            if (!string.IsNullOrEmpty(skuId.Trim()))
            {
                foreach (DataRow row in ControlProvider.Instance().GetSkuContentBySku(skuId).Rows)
                {
                    if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                    {
                        object obj2 = str;
                        str = string.Concat(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], "; " });
                    }
                }
            }
            if (!(str == ""))
            {
                return str;
            }
            return "不存在";
        }

        public static string ShowMessageInfo(ApiErrorCode messageenum, string field)
        {
            string format = "<error_response><code>{0}</code><msg>" + field + " {1}</msg></error_response>";
            switch (messageenum)
            {
                case ApiErrorCode.Paramter_Error:
                    return string.Format(format, 0x65, "is error");

                case ApiErrorCode.Format_Eroor:
                    return string.Format(format, 0x66, "format is error");

                case ApiErrorCode.Signature_Error:
                    return string.Format(format, 0x67, "signature is error");

                case ApiErrorCode.Empty_Error:
                    return string.Format(format, 0x68, "is empty");

                case ApiErrorCode.NoExists_Error:
                    return string.Format(format, 0x69, "is not exists");

                case ApiErrorCode.Exists_Error:
                    return string.Format(format, 0x69, "is exists");

                case ApiErrorCode.Paramter_Diffrent:
                    return string.Format(format, 0x6b, "is diffrent");

                case ApiErrorCode.Group_Error:
                    return string.Format(format, 0x6c, "is not the end grouporder");

                case ApiErrorCode.NoPay_Error:
                    return string.Format(format, 0x6d, "is not pay money");

                case ApiErrorCode.NoShippingMode:
                    return string.Format(format, 110, "is not shippingmodel");

                case ApiErrorCode.ShipingOrderNumber_Error:
                    return string.Format(format, 0x6f, "is shippingordernumer error");

                case ApiErrorCode.Session_Empty:
                    return string.Format(format, 200, "sessionId is no exist");

                case ApiErrorCode.Session_Error:
                    return string.Format(format, 0xc9, "sessionId is no Invalid");

                case ApiErrorCode.Session_TimeOut:
                    return string.Format(format, 0xca, "is timeout");

                case ApiErrorCode.Username_Exist:
                    return string.Format(format, 0xcb, "account is Exist");

                case ApiErrorCode.Ban_Register:
                    return string.Format(format, 0xcc, "Prohibition on registration");

                case ApiErrorCode.SaleState_Error:
                    return string.Format(format, 300, "cant not buy product");

                case ApiErrorCode.Stock_Error:
                    return string.Format(format, 0x12d, "stock is lack");
            }
            return string.Format(format, 0x3e7, "unknown_Error");
        }
    }
}

