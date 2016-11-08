namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SKUSelector : WebControl
    {
        
        private DataTable _DataSource;
        
        private int _ProductId;
        public const string TagID = "SKUSelector";

        public SKUSelector()
        {
            base.ID = "SKUSelector";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            DataTable dataSource = this.DataSource;
            if ((dataSource != null) && (dataSource.Rows.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                IList<string> list = new List<string>();
                builder.AppendFormat("<input type=\"hidden\" id=\"{0}\" value=\"{1}\" />", "hiddenProductId", this.ProductId).AppendLine();
                builder.AppendFormat("<div id=\"productSkuSelector\" class=\"{0}\">", this.CssClass).AppendLine();
                string str = string.Empty;
                foreach (DataRow row in dataSource.Rows)
                {
                    if (!list.Contains((string) row["AttributeName"]))
                    {
                        list.Add((string) row["AttributeName"]);
                        str = str + "\"" + ((string) row["AttributeName"]) + "\" ";
                        builder.AppendLine("<div class=\"SKURowClass\">");
                        builder.AppendFormat("<span>{0}：</span><input type=\"hidden\" name=\"skuCountname\" AttributeName=\"{0}\" id=\"skuContent_{1}\" />", row["AttributeName"], row["AttributeId"]);
                        builder.AppendFormat("<dl id=\"skuRow_{0}\">", row["AttributeId"]);
                        IList<string> list2 = new List<string>();
                        foreach (DataRow row2 in dataSource.Rows)
                        {
                            if ((string.Compare((string) row["AttributeName"], (string) row2["AttributeName"]) == 0) && !list2.Contains((string) row2["ValueStr"]))
                            {
                                string str2 = string.Concat(new object[] { "skuValueId_", row["AttributeId"], "_", row2["ValueId"] });
                                list2.Add((string) row2["ValueStr"]);
                                if ((bool) row["UseAttributeImage"])
                                {
                                    builder.AppendFormat("<dd><img class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" src=\"{4}\" /></dd> ", new object[] { str2, row["AttributeId"], row2["ValueId"], row2["ValueStr"], Globals.ApplicationPath + ((string) row2["ImageUrl"]) });
                                }
                                else
                                {
                                    builder.AppendFormat("<dd><input type=\"button\" class=\"SKUValueClass\" id=\"{0}\" AttributeId=\"{1}\" ValueId=\"{2}\" value=\"{3}\" /></dd> ", new object[] { str2, row["AttributeId"], row2["ValueId"], row2["ValueStr"] });
                                }
                            }
                        }
                        builder.AppendLine("</dl></div>");
                    }
                }
                builder.AppendFormat("<div id=\"showSelectSKU\"  class=\"SKUShowSelectClass\">请选择：{0} </div>", str);
                builder.AppendLine("</div>");
                writer.Write(builder.ToString());
            }
        }

        public DataTable DataSource
        {
            
            get
            {
                return _DataSource;
            }
            
            set
            {
                _DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public int ProductId
        {
            
            get
            {
                return _ProductId;
            }
            
            set
            {
                _ProductId = value;
            }
        }
    }
}

