namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Subsites.Commodities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class GridSkuUnderlingPriceTable : WebControl
    {
        private void CreateRow(DataRow row, DataTable dtSkus, StringBuilder sb)
        {
            string str = row["SkuId"].ToString();
            sb.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", str).AppendLine();
            sb.AppendFormat("<td>&nbsp;{0}</td>", row["SKU"] + "&nbsp;").AppendLine();
            sb.AppendFormat("<td style=\"width:300px;\">{0} {1}</td>", row["ProductName"], row["SKUContent"]).AppendLine();
            sb.AppendFormat("<td>&nbsp;{0}</td>", (row["MarketPrice"] != DBNull.Value) ? decimal.Parse(row["MarketPrice"].ToString()).ToString("F2") : "").AppendLine();
            sb.AppendFormat("<td>&nbsp;{0}</td>", (row["PurchasePrice"] != DBNull.Value) ? decimal.Parse(row["PurchasePrice"].ToString()).ToString("F2") : "").AppendLine();
            sb.AppendFormat("<td><input type=\"text\" id=\"tdSalePrice_{0}\" style=\"width:60px\" class=\"forminput\" value=\"{1}\" />", str, decimal.Parse(row["SalePrice"].ToString()).ToString("F2")).AppendLine();
            for (int i = 7; i < dtSkus.Columns.Count; i++)
            {
                string columnName = dtSkus.Columns[i].ColumnName;
                string[] strArray = row[columnName].ToString().Split(new char[] { '|' });
                string str3 = "";
                string str4 = "";
                if (strArray[0].ToString() != "")
                {
                    str3 = decimal.Parse(strArray[0].ToString()).ToString("F2");
                }
                str4 = strArray[1].ToString();
                sb.AppendFormat("<td><input type=\"text\" id=\"{0}_tdMemberPrice_{1}\" name=\"tdMemberPrice_{1}\" style=\"width:50px\" class=\"forminput\" value=\"{2}\" />{3}", new object[] { columnName.Substring(0, columnName.IndexOf("_")), str, str3, str4 }).AppendLine();
            }
            sb.AppendLine("</tr>");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string str = this.Page.Request.QueryString["productIds"];
            if (!string.IsNullOrEmpty(str))
            {
                DataTable skuUnderlingPrices = SubSiteProducthelper.GetSkuUnderlingPrices(str);
                if ((skuUnderlingPrices != null) && (skuUnderlingPrices.Rows.Count > 0))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<table cellspacing=\"0\" border=\"0\" style=\"width:100%;border-collapse:collapse;\">");
                    sb.AppendLine("<tr class=\"table_title\">");
                    sb.AppendLine("<th class=\"td_right td_left\" scope=\"col\">货号</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" scope=\"col\" style=\"width:300px;\">商品</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" scope=\"col\">市场价</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" scope=\"col\">我的采购价</th>");
                    sb.AppendLine("<th class=\"td_right td_left\" scope=\"col\">一口价</th>");
                    for (int i = 7; i < skuUnderlingPrices.Columns.Count; i++)
                    {
                        string columnName = skuUnderlingPrices.Columns[i].ColumnName;
                        columnName = columnName.Substring(columnName.IndexOf("_") + 1) + "价";
                        sb.AppendFormat("<th class=\"td_right td_left\" scope=\"col\">{0}</th>", columnName).AppendLine();
                    }
                    sb.AppendLine("</tr>");
                    foreach (DataRow row in skuUnderlingPrices.Rows)
                    {
                        this.CreateRow(row, skuUnderlingPrices, sb);
                    }
                    sb.AppendLine("</table>");
                    writer.Write(sb.ToString());
                }
            }
        }
    }
}

