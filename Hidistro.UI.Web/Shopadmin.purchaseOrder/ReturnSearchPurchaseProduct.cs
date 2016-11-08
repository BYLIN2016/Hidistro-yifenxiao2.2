namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Subsites.Commodities;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Data;
    using System.Text;

    public class ReturnSearchPurchaseProduct : DistributorPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            string str = "";
            string str2 = "";
            string str3 = "1";
            if (base.Request.Params["serachName"] != null)
            {
                str = base.Request.Params["serachName"].Trim();
            }
            if (base.Request.Params["serachSKU"] != null)
            {
                str2 = base.Request.Params["serachSKU"].Trim();
            }
            if (base.Request.Params["page"] != null)
            {
                str3 = base.Request.Params["page"].Trim();
            }
            ProductQuery query = new ProductQuery();
            query.PageSize = 8;
            query.PageIndex = Convert.ToInt32(str3);
            query.Keywords = str;
            query.ProductCode = str2;
            int count = 0;
            builder.Append("{");
            builder.Append("\"data\":[");
            DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(query, out count);
            int num2 = puchaseProducts.Rows.Count;
            for (int i = 0; i < num2; i++)
            {
                DataRow row = puchaseProducts.Rows[i];
                builder.Append("{");
                builder.AppendFormat("\"skuId\":\"{0}\"", row["SkuId"]);
                builder.AppendFormat(",\"sku\":\"{0}\"", row["SKU"]);
                builder.AppendFormat(",\"productId\":\"{0}\"", row["ProductId"].ToString().Trim());
                string str4 = row["ProductName"].ToString().Trim();
                builder.AppendFormat(",\"Name\":\"{0}\"", str4);
                builder.AppendFormat(",\"Price\":\"{0}\"", row["PurchasePrice"]);
                builder.AppendFormat(",\"Stock\":\"{0}\"", row["Stock"]);
                if (i == (num2 - 1))
                {
                    builder.Append("}");
                }
                else
                {
                    builder.Append("},");
                }
            }
            builder.AppendFormat("],\"recCount\":\"{0}\"", count);
            builder.Append("}");
            base.Response.ContentType = "application/json";
            string s = builder.ToString();
            base.Response.Write(s);
            base.Response.End();
        }
    }
}

