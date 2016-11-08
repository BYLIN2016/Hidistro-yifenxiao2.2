namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class MakeTaobaoProductData : Page
    {
        protected HtmlForm form1;
        protected HtmlHead Head1;
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out this.productId);
            DataSet taobaoProductDetails = ProductHelper.GetTaobaoProductDetails(this.productId);
            DataTable table = taobaoProductDetails.Tables[0];
            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();
            dicArrayPre.Add("SiteUrl", HiContext.Current.SiteUrl);
            dicArrayPre.Add("_input_charset", "utf-8");
            dicArrayPre.Add("return_url", Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("MakeTaobaoProductData_url")));
            dicArrayPre.Add("ProductId", this.productId.ToString());
            dicArrayPre.Add("HasSKU", table.Rows[0]["HasSKU"].ToString());
            dicArrayPre.Add("ProductName", (string) table.Rows[0]["ProductName"]);
            dicArrayPre.Add("ProductCode", (string) table.Rows[0]["ProductCode"]);
            dicArrayPre.Add("CategoryName", (table.Rows[0]["CategoryName"] == DBNull.Value) ? "" : ((string) table.Rows[0]["CategoryName"]));
            dicArrayPre.Add("ProductLineName", (table.Rows[0]["ProductLineName"] == DBNull.Value) ? "" : ((string) table.Rows[0]["ProductLineName"]));
            dicArrayPre.Add("BrandName", (table.Rows[0]["BrandName"] == DBNull.Value) ? "" : ((string) table.Rows[0]["BrandName"]));
            dicArrayPre.Add("SalePrice", Convert.ToDecimal(table.Rows[0]["SalePrice"]).ToString("F2"));
            dicArrayPre.Add("MarketPrice", (table.Rows[0]["MarketPrice"] == DBNull.Value) ? "0" : Convert.ToDecimal(table.Rows[0]["MarketPrice"]).ToString("F2"));
            dicArrayPre.Add("CostPrice", Convert.ToDecimal(table.Rows[0]["CostPrice"]).ToString("F2"));
            dicArrayPre.Add("PurchasePrice", Convert.ToDecimal(table.Rows[0]["PurchasePrice"]).ToString("F2"));
            dicArrayPre.Add("Stock", table.Rows[0]["Stock"].ToString());
            DataTable table2 = taobaoProductDetails.Tables[1];
            if (table2.Rows.Count > 0)
            {
                string str = string.Empty;
                foreach (DataRow row in table2.Rows)
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], ";" });
                }
                str = str.Remove(str.Length - 1);
                dicArrayPre.Add("Attributes", str);
            }
            DataTable table3 = taobaoProductDetails.Tables[2];
            if (table3.Rows.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                for (int i = table3.Columns.Count - 1; i >= 0; i--)
                {
                    builder2.Append(table3.Columns[i].ColumnName).Append(";");
                }
                foreach (DataRow row2 in table3.Rows)
                {
                    for (int j = table3.Columns.Count - 1; j >= 0; j--)
                    {
                        builder.Append(row2[table3.Columns[j].ColumnName]).Append(";");
                    }
                    builder.Remove(builder.Length - 1, 1).Append(",");
                }
                builder2.Remove(builder2.Length - 1, 1).Append(",").Append(builder.Remove(builder.Length - 1, 1));
                dicArrayPre.Add("skus", builder2.ToString());
            }
            DataTable table4 = taobaoProductDetails.Tables[3];
            if (table4.Rows.Count > 0)
            {
                dicArrayPre.Add("Cid", table4.Rows[0]["Cid"].ToString());
                if (table4.Rows[0]["StuffStatus"] != DBNull.Value)
                {
                    dicArrayPre.Add("StuffStatus", (string) table4.Rows[0]["StuffStatus"]);
                }
                dicArrayPre.Add("ProTitle", (string) table4.Rows[0]["ProTitle"]);
                dicArrayPre.Add("Num", table4.Rows[0]["Num"].ToString());
                dicArrayPre.Add("LocationState", (string) table4.Rows[0]["LocationState"]);
                dicArrayPre.Add("LocationCity", (string) table4.Rows[0]["LocationCity"]);
                dicArrayPre.Add("FreightPayer", (string) table4.Rows[0]["FreightPayer"]);
                if (table4.Rows[0]["PostFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("PostFee", table4.Rows[0]["PostFee"].ToString());
                }
                if (table4.Rows[0]["ExpressFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("ExpressFee", table4.Rows[0]["ExpressFee"].ToString());
                }
                if (table4.Rows[0]["EMSFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("EMSFee", table4.Rows[0]["EMSFee"].ToString());
                }
                dicArrayPre.Add("HasInvoice", table4.Rows[0]["HasInvoice"].ToString());
                dicArrayPre.Add("HasWarranty", table4.Rows[0]["HasWarranty"].ToString());
                dicArrayPre.Add("HasDiscount", table4.Rows[0]["HasDiscount"].ToString());
                if (table4.Rows[0]["PropertyAlias"] != DBNull.Value)
                {
                    dicArrayPre.Add("PropertyAlias", (string) table4.Rows[0]["PropertyAlias"]);
                }
                if (table4.Rows[0]["SkuProperties"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuProperties", (string) table4.Rows[0]["SkuProperties"]);
                }
                if (table4.Rows[0]["SkuQuantities"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuQuantities", (string) table4.Rows[0]["SkuQuantities"]);
                }
                if (table4.Rows[0]["SkuPrices"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuPrices", (string) table4.Rows[0]["SkuPrices"]);
                }
                if (table4.Rows[0]["SkuOuterIds"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuOuterIds", (string) table4.Rows[0]["SkuOuterIds"]);
                }
                if (table4.Rows[0]["inputpids"] != DBNull.Value)
                {
                    dicArrayPre.Add("inputpids", (string) table4.Rows[0]["inputpids"]);
                }
                if (table4.Rows[0]["inputstr"] != DBNull.Value)
                {
                    dicArrayPre.Add("inputstr", (string) table4.Rows[0]["inputstr"]);
                }
            }
            Dictionary<string, string> dictionary2 = OpenIdFunction.FilterPara(dicArrayPre);
            StringBuilder builder3 = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                builder3.Append(OpenIdFunction.CreateField(pair.Key, pair.Value));
            }
            dicArrayPre.Clear();
            dictionary2.Clear();
            string action = "http://order1.kuaidiangtong.com/MakeTaoBaoData.aspx";
            if (table4.Rows.Count > 0)
            {
                action = "http://order1.kuaidiangtong.com/UpdateTaoBaoData.aspx";
            }
            OpenIdFunction.Submit(OpenIdFunction.CreateForm(builder3.ToString(), action));
        }
    }
}

