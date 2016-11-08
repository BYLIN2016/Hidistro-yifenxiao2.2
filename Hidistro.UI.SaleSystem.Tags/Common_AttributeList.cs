namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_AttributeList : WebControl
    {
        private int brandId;
        private int categoryId;
        private string valueStr = string.Empty;

        private string CreateUrl(string paraName, string paraValue)
        {
            string rawUrl = this.Context.Request.RawUrl;
            if (rawUrl.IndexOf("?") >= 0)
            {
                string oldValue = rawUrl.Substring(rawUrl.IndexOf("?") + 1);
                string[] strArray = oldValue.Split(new char[] { Convert.ToChar("&") });
                rawUrl = rawUrl.Replace(oldValue, "");
                foreach (string str3 in strArray)
                {
                    if (!str3.ToLower().StartsWith(paraName + "=") && !str3.ToLower().StartsWith("pageindex="))
                    {
                        rawUrl = rawUrl + str3 + "&";
                    }
                }
                return (rawUrl + paraName + "=" + Globals.UrlEncode(paraValue));
            }
            string str4 = rawUrl;
            return (str4 + "?" + paraName + "=" + Globals.UrlEncode(paraValue));
        }

        protected override void OnInit(EventArgs e)
        {
            int.TryParse(this.Context.Request.QueryString["categoryId"], out this.categoryId);
            int.TryParse(this.Context.Request.QueryString["brand"], out this.brandId);
            this.valueStr = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
            base.OnInit(e);
        }

        private string RemoveAttribute(string paraValue, int attributeId, int valueId)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(paraValue))
            {
                string[] strArray = paraValue.Split(new char[] { '-' });
                if ((strArray != null) && (strArray.Length > 0))
                {
                    foreach (string str2 in strArray)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            string[] strArray2 = str2.Split(new char[] { '_' });
                            if (((strArray2 != null) && (strArray2.Length > 0)) && (strArray2[0] != attributeId.ToString()))
                            {
                                str = str + str2 + "-";
                            }
                        }
                    }
                }
            }
            return string.Concat(new object[] { str, attributeId, "_", valueId });
        }

        private void RendeAttribute(StringBuilder sb)
        {
            IList<AttributeInfo> attributeInfoByCategoryId = CategoryBrowser.GetAttributeInfoByCategoryId(this.categoryId, 0x3e8);
            if ((attributeInfoByCategoryId != null) && (attributeInfoByCategoryId.Count > 0))
            {
                foreach (AttributeInfo info in attributeInfoByCategoryId)
                {
                    sb.AppendLine("<dl class=\"attribute_dl\">");
                    if (info.AttributeValues.Count > 0)
                    {
                        sb.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", info.AttributeName).AppendLine();
                        sb.AppendLine("<dd class=\"attribute_val\">");
                        sb.AppendLine("<div class=\"h_chooselist\">");
                        string str = "all";
                        string paraValue = this.RemoveAttribute(this.valueStr, info.AttributeId, 0);
                        string str3 = "all select";
                        if (!string.IsNullOrEmpty(this.valueStr) && new Regex(string.Format("{0}_[1-9]+", info.AttributeId)).IsMatch(this.valueStr))
                        {
                            str3 = "all";
                        }
                        sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", str3, this.CreateUrl("valuestr", paraValue)).AppendLine();
                        foreach (AttributeValueInfo info2 in info.AttributeValues)
                        {
                            str = string.Empty;
                            paraValue = this.RemoveAttribute(this.valueStr, info.AttributeId, info2.ValueId);
                            if (!string.IsNullOrEmpty(this.valueStr) && this.valueStr.Split(new char[] { '-' }).Contains<string>((info.AttributeId + "_" + info2.ValueId)))
                            {
                                str = "select";
                            }
                            sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", str, this.CreateUrl("valuestr", paraValue), info2.ValueStr).AppendLine();
                        }
                        sb.AppendLine("</div>");
                        sb.AppendLine("</dd>");
                    }
                    sb.AppendLine("</dl>");
                }
            }
        }

        private void RendeBrand(StringBuilder sb)
        {
            DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, 0x3e8);
            if ((brandCategories != null) && (brandCategories.Rows.Count > 0))
            {
                sb.AppendLine("<dl class=\"attribute_dl\">");
                sb.AppendLine("<dt class=\"attribute_name\">品牌：</dt>");
                sb.AppendLine("<dd class=\"attribute_val\">");
                sb.AppendLine("<div class=\"h_chooselist\">");
                string str = "all";
                if (this.brandId == 0)
                {
                    str = str + " select";
                }
                sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", str, this.CreateUrl("brand", "")).AppendLine();
                foreach (DataRow row in brandCategories.Rows)
                {
                    str = string.Empty;
                    if (this.brandId == ((int) row["BrandId"]))
                    {
                        str = str + " select";
                    }
                    sb.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", str, this.CreateUrl("brand", row["BrandId"].ToString()), row["BrandName"]).AppendLine();
                }
                sb.AppendLine("</div>");
                sb.AppendLine("</dd>");
                sb.AppendLine("</dl>");
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"attribute_bd\">");
            this.RendeBrand(sb);
            this.RendeAttribute(sb);
            sb.AppendLine("</div>");
            writer.Write(sb.ToString());
        }
    }
}

