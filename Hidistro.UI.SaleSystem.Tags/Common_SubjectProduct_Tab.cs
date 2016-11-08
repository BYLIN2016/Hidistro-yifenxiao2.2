namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectProduct_Tab : WebControl
    {
        
        private int _SubjectId;

        private DataTable GetTabProductList(XmlNode node, string whereName)
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            string str = node.Attributes[whereName].Value;
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(new char[] { ',' });
                query.CategoryIds = strArray[0];
                if (!string.IsNullOrEmpty(strArray[1]))
                {
                    query.TagId = int.Parse(strArray[1]);
                }
                if (!string.IsNullOrEmpty(strArray[2]))
                {
                    query.BrandCategoryId = new int?(int.Parse(strArray[2]));
                }
            }
            query.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
            return ProductBrowser.GetSubjectList(query);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        private void RenderHeader(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"tab_hd\">");
            sb.AppendLine("<ul>");
            sb.AppendFormat("<li class=\"select\" onmouseover=\"changeTab(this, 'products_{0}', '_item_1')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle1"].Value).AppendLine();
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle2"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_2')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle2"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle3"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_3')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle3"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle4"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_4')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle4"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle5"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_5')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle5"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle6"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_6')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle6"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle7"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_7')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle7"].Value).AppendLine();
            }
            if (!string.IsNullOrEmpty(node.Attributes["TabTitle8"].Value))
            {
                sb.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_8')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle8"].Value).AppendLine();
            }
            sb.AppendLine("</ul>");
            sb.AppendLine("</div>");
        }

        private void RenderProdcutItem(XmlNode node, StringBuilder sb, string whereName)
        {
            DataTable tabProductList = this.GetTabProductList(node, whereName);
            if ((tabProductList != null) && (tabProductList.Rows.Count > 0))
            {
                sb.AppendLine("<ul>");
                foreach (DataRow row in tabProductList.Rows)
                {
                    string defaultProductImage = SettingsManager.GetMasterSettings(false).DefaultProductImage;
                    if (row["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
                    {
                        defaultProductImage = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
                    }
                    sb.AppendLine("<li>");
                    sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), Globals.ApplicationPath + defaultProductImage).AppendLine();
                    sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), row["ProductName"]).AppendLine();
                    string str2 = string.Empty;
                    if (row["MarketPrice"] != DBNull.Value)
                    {
                        str2 = Globals.FormatMoney((decimal) row["MarketPrice"]);
                    }
                    sb.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal) row["RankPrice"]), str2).AppendLine();
                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ul>");
            }
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindProductNode(this.SubjectId, "tab");
            StringBuilder sb = new StringBuilder();
            if (node != null)
            {
                sb.AppendFormat("<div class=\"pro_tab{0} cssEdite\" type=\"tab\" id=\"products_{1}\" >", node.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
                this.RenderHeader(node, sb);
                sb.AppendFormat("<div class=\"tab_item\" id=\"products_{0}_item_1\">", this.SubjectId);
                this.RenderProdcutItem(node, sb, "Where1");
                sb.AppendLine("</div>");
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle2"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_2\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where2");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle3"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_3\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where3");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle4"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_4\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where4");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle5"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_5\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where5");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle6"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_6\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where6");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle7"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_7\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where7");
                    sb.AppendLine("</div>");
                }
                if (!string.IsNullOrEmpty(node.Attributes["TabTitle8"].Value))
                {
                    sb.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_8\">", this.SubjectId);
                    this.RenderProdcutItem(node, sb, "Where8");
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div>");
            }
            return sb.ToString();
        }

        public int SubjectId
        {
            
            get
            {
                return _SubjectId;
            }
            
            set
            {
                _SubjectId = value;
            }
        }
    }
}

