namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.UI.Subsites.Utility;
    using LitJson;
    using System;
    using System.Collections;

    public class FileCategoryJson : DistributorPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable hashtable = new Hashtable();
            base.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
            base.Response.Write(JsonMapper.ToJson(hashtable));
            base.Response.End();
        }
    }
}

