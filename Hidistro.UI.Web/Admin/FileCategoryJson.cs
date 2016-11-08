namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.ControlPanel.Utility;
    using LitJson;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public class FileCategoryJson : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable hashtable = new Hashtable();
            base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            if (Users.GetUser(0, Users.GetLoggedOnUsername(), true, true).UserRole != UserRole.SiteManager)
            {
                base.Response.Write(JsonMapper.ToJson(hashtable));
                base.Response.End();
            }
            else
            {
                List<Hashtable> list = new List<Hashtable>();
                hashtable["category_list"] = list;
                foreach (DataRow row in GalleryHelper.GetPhotoCategories().Rows)
                {
                    Hashtable item = new Hashtable();
                    item["cId"] = row["CategoryId"];
                    item["cName"] = row["CategoryName"];
                    list.Add(item);
                }
                base.Response.Write(JsonMapper.ToJson(hashtable));
                base.Response.End();
            }
        }
    }
}

