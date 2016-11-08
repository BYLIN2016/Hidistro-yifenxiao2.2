namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Subsites.Commodities;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductSKUDisplay : WebControl
    {
        
        private string _ColumnClass;
        
        private string _CssClass;
        
        private string _HeadColumnClass;
        
        private string _HeadRowClass;
        
        private string _RowClass;

        protected override void Render(HtmlTextWriter writer)
        {
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["ProductId"], out result))
            {
                DataTable productSKU = SubSiteProducthelper.GetProductSKU(result);
                if ((productSKU != null) && (productSKU.Rows.Count > 0))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<input type=\"hidden\" id=\"skuContent\" value=\"1\" />");
                    builder.AppendFormat("<table id=\"tbSkuContent\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"{0}\" style=\"display:inline;\">", this.CssClass);
                    builder.AppendFormat("<tr class=\"{0}\">", this.HeadRowClass);
                    for (int i = productSKU.Columns.Count - 1; i > 0; i--)
                    {
                        builder.AppendFormat("<td class=\"{0}\">{1}</td>", this.HeadColumnClass, productSKU.Columns[i].ColumnName);
                    }
                    builder.Append("</tr>");
                    foreach (DataRow row in productSKU.Rows)
                    {
                        builder.AppendFormat("<tr class=\"{0}\">", this.RowClass);
                        for (int j = productSKU.Columns.Count - 1; j > 0; j--)
                        {
                            string columnName = productSKU.Columns[j].ColumnName;
                            string s = row[columnName].ToString();
                            if (columnName.Equals("一口价"))
                            {
                                builder.AppendFormat("<td class=\"{0}\"><input type=\"text\" style=\"width:80px\" class=\"skuPriceItem\" id=\"{1}\" value=\"{2}\" /></td>", this.RowClass, row["SkuId"], decimal.Parse(s).ToString("F2"));
                            }
                            else if (s.StartsWith("/Storage/master/sku/") && ((s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".gif")) || ((s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".ico")) || s.ToLower().EndsWith(".bmp"))))
                            {
                                builder.AppendFormat("<td class=\"{0}\"><img src=\"{1}\" /></td>", this.RowClass, Globals.ApplicationPath + s);
                            }
                            else
                            {
                                decimal num4 = 0M;
                                int num5 = 0;
                                if (decimal.TryParse(s, out num4) && !int.TryParse(s, out num5))
                                {
                                    s = num4.ToString("F2");
                                }
                                builder.AppendFormat("<td class=\"{0}\">{1}</td>", this.RowClass, s);
                            }
                        }
                        builder.Append("</tr>");
                    }
                    builder.Append("</table>");
                    writer.Write(builder.ToString());
                    return;
                }
            }
            writer.Write("<input type=\"hidden\" id=\"skuContent\" value=\"0\" />");
        }

        public string ColumnClass
        {
            
            get
            {
                return _ColumnClass;
            }
            
            set
            {
                _ColumnClass = value;
            }
        }

        public string CssClass
        {
            
            get
            {
                return _CssClass;
            }
            
            set
            {
                _CssClass = value;
            }
        }

        public string HeadColumnClass
        {
            
            get
            {
                return _HeadColumnClass;
            }
            
            set
            {
                _HeadColumnClass = value;
            }
        }

        public string HeadRowClass
        {
            
            get
            {
                return _HeadRowClass;
            }
            
            set
            {
                _HeadRowClass = value;
            }
        }

        public string RowClass
        {
            
            get
            {
                return _RowClass;
            }
            
            set
            {
                _RowClass = value;
            }
        }
    }
}

