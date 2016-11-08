namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public class BrowsedProductQueue
    {
        private static string browedProductList = "BrowedProductList-Admin";

        static BrowsedProductQueue()
        {
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                browedProductList = string.Format("BrowedProductList-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
        }

        public static void ClearQueue()
        {
            SaveCookie(null);
        }

        public static void EnQueue(int productId)
        {
            IList<int> browedProductList = GetBrowedProductList();
            int index = 0;
            foreach (int num2 in browedProductList)
            {
                if (productId == num2)
                {
                    browedProductList.RemoveAt(index);
                    break;
                }
                index++;
            }
            if (browedProductList.Count <= 20)
            {
                browedProductList.Add(productId);
            }
            else
            {
                browedProductList.RemoveAt(0);
                browedProductList.Add(productId);
            }
            SaveCookie(browedProductList);
        }

        public static IList<int> GetBrowedProductList()
        {
            IList<int> list = new List<int>();
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies[browedProductList];
            if (!((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
            {
                list = Serializer.ConvertToObject(HiContext.Current.Context.Server.UrlDecode(cookie.Value), typeof(List<int>)) as List<int>;
            }
            return list;
        }

        public static IList<int> GetBrowedProductList(int maxNum)
        {
            IList<int> browedProductList = GetBrowedProductList();
            int count = browedProductList.Count;
            if (browedProductList.Count > maxNum)
            {
                for (int i = 0; i < (count - maxNum); i++)
                {
                    browedProductList.RemoveAt(0);
                }
            }
            return browedProductList;
        }

        private static void SaveCookie(IList<int> productIdList)
        {
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies[browedProductList];
            if (cookie == null)
            {
                cookie = new HttpCookie(browedProductList);
            }
            cookie.Expires = DateTime.Now.AddDays(7.0);
            cookie.Value = Globals.UrlEncode(Serializer.ConvertToString(productIdList));
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}

