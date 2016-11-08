namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectProduct_Floor : WebControl
    {
        
        private int _SubjectId;

        private DataTable GetProductList(XmlNode node)
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            query.CategoryIds = node.Attributes["CategoryId"].Value;
            if (!string.IsNullOrEmpty(node.Attributes["TagId"].Value))
            {
                query.TagId = int.Parse(node.Attributes["TagId"].Value);
            }
            if (!string.IsNullOrEmpty(node.Attributes["BrandId"].Value))
            {
                query.BrandCategoryId = new int?(int.Parse(node.Attributes["BrandId"].Value));
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
            sb.AppendLine("<div class=\"floor_hd\">");
            sb.AppendLine("<div>");
            if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
            {
                sb.AppendFormat("<span class=\"icon\"><img src=\"{0}\" /></span>", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
            }
            if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
            {
                sb.AppendFormat("<span class=\"title\">{0}</span>", node.Attributes["Title"].Value);
            }
            sb.AppendLine("</div>");
            int result = 0;
            if (int.TryParse(node.Attributes["CategoryId"].Value, out result))
            {
                IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(result, int.Parse(node.Attributes["SubCategoryNum"].Value));
                if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
                {
                    sb.AppendLine("<ul>");
                    foreach (CategoryInfo info in maxSubCategories)
                    {
                        sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(info.CategoryId, info.RewriteName), info.Name).AppendLine("");
                    }
                    sb.AppendLine("</ul>");
                }
                if (node.Attributes["IsShowMoreLink"].Value == "true")
                {
                    sb.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(result, null)).AppendLine();
                }
            }
            sb.AppendLine("</div>");
        }

        public string RendHtml()
        {
            StringBuilder sb = new StringBuilder();
            XmlNode node = TagsHelper.FindProductNode(this.SubjectId, "floor");
            if (node != null)
            {
                sb.AppendFormat("<div class=\"floor{0} cssEdite\" type=\"floor\" id=\"products_{1}\" >", node.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
                this.RenderHeader(node, sb);
                sb.AppendLine("<div class=\"floor_bd\">");
                if (!string.IsNullOrEmpty(node.Attributes["AdImage"].Value))
                {
                    sb.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImage"].Value).AppendLine();
                }
                else
                {
                    sb.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", SettingsManager.GetMasterSettings(true).DefaultProductImage).AppendLine();
                }
                sb.AppendLine("<div class=\"floor_pro\">");
                DataTable productList = this.GetProductList(node);
                if ((productList != null) && (productList.Rows.Count > 0))
                {
                    sb.AppendLine("<ul>");
                    foreach (DataRow row in productList.Rows)
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
                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
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

