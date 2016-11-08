namespace Hidistro.UI.Web.DialogTemplates
{
    using Hidistro.Membership.Context;
    using LitJson;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;

    public class FileCategoryJson : IHttpHandler
    {
        private string message = "";

        public void ProcessRequest(HttpContext context)
        {
            Hashtable hashtable = new Hashtable();
            if (HiContext.Current.Context.User.IsInRole("manager") || HiContext.Current.Context.User.IsInRole("systemadministrator"))
            {
                List<Hashtable> list = new List<Hashtable>();
                hashtable["category_list"] = list;
                Hashtable item = new Hashtable();
                item["cId"] = "AdvertImg";
                item["cName"] = "广告位图片";
                list.Add(item);
                item = new Hashtable();
                item["cId"] = "TitleImg";
                item["cName"] = "标题图片";
                list.Add(item);
            }
            this.message = JsonMapper.ToJson(hashtable);
            context.Response.ContentType = "text/json";
            context.Response.Write(this.message);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

