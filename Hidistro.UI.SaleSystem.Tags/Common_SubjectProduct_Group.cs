namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Comments;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectProduct_Group : WebControl
    {
        
        private int _SubjectId;
        private int categoryId;

        private DataTable GetProductList(XmlNode node)
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            query.CategoryIds = node.Attributes["CategoryId"].Value;
            query.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
            return ProductBrowser.GetSubjectList(query);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        private void RenderBrand(XmlNode node, StringBuilder sb)
        {
            DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, int.Parse(node.Attributes["BrandNum"].Value));
            if ((brandCategories != null) && (brandCategories.Rows.Count > 0))
            {
                sb.AppendLine("<div class=\"bd_brand\">");
                sb.AppendLine("<ul>");
                foreach (DataRow row in brandCategories.Rows)
                {
                    sb.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" /></a></li>", Globals.GetSiteUrls().SubBrandDetails((int) row["BrandId"], row["RewriteName"]), row["Logo"]).AppendLine();
                }
                sb.AppendLine("</ul>");
                sb.AppendLine("</div>");
            }
        }

        private void RenderHeader(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"group_hd\">");
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
            DataTable hotKeywords = CommentBrowser.GetHotKeywords(this.categoryId, int.Parse(node.Attributes["HotKeywordNum"].Value));
            if ((hotKeywords != null) && (hotKeywords.Rows.Count > 0))
            {
                sb.AppendLine("<ul>");
                foreach (DataRow row in hotKeywords.Rows)
                {
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory((int) row["CategoryId"], null) + "?keywords=" + Globals.UrlEncode((string) row["Keywords"]), row["Keywords"]).AppendLine("");
                }
                sb.AppendLine("</ul>");
            }
            if (node.Attributes["IsShowMoreLink"].Value == "true")
            {
                sb.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(this.categoryId, null)).AppendLine();
            }
            sb.AppendLine("</div>");
        }

        private void RenderLift(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"bd_left\">");
            IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, int.Parse(node.Attributes["SubCategoryNum"].Value));
            if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
            {
                sb.AppendLine("<ul>");
                foreach (CategoryInfo info in maxSubCategories)
                {
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(info.CategoryId, info.RewriteName), info.Name).AppendLine("");
                }
                sb.AppendLine("</ul>");
            }
            if (!string.IsNullOrEmpty(node.Attributes["AdImageLeft"].Value))
            {
                sb.AppendFormat("<div class=\"bd_left_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageLeft"].Value);
            }
            sb.AppendLine("</div>");
        }

        private void RenderMiddle(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"bd_middle\">");
            if (!string.IsNullOrEmpty(node.Attributes["AdImageRight"].Value))
            {
                sb.AppendFormat("<div class=\"bd_right_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageRight"].Value);
            }
            DataTable productList = this.GetProductList(node);
            if ((productList != null) && (productList.Rows.Count > 0))
            {
                sb.AppendLine("<ul>");
                foreach (DataRow row in productList.Rows)
                {
                    string defaultProductImage;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    if (row["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
                    {
                        defaultProductImage = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
                    }
                    else
                    {
                        defaultProductImage = masterSettings.DefaultProductImage;
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
        }

        private void RenderRight(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"bd_right\">");
            this.RenderBrand(node, sb);
            this.RenderSaleTop(node, sb);
            sb.AppendLine("</div>");
        }

        private void RenderSaleTop(XmlNode node, StringBuilder sb)
        {
            DataTable saleProductRanking = ProductBrowser.GetSaleProductRanking(new int?(this.categoryId), int.Parse(node.Attributes["SaleTopNum"].Value));
            if ((saleProductRanking != null) && (saleProductRanking.Rows.Count > 0))
            {
                int result = 0;
                int.TryParse(node.Attributes["ImageNum"].Value, out result);
                bool flag = false;
                bool.TryParse(node.Attributes["IsShowPrice"].Value, out flag);
                bool flag2 = false;
                bool.TryParse(node.Attributes["IsShowSaleCounts"].Value, out flag2);
                bool flag3 = false;
                bool.TryParse(node.Attributes["IsImgShowPrice"].Value, out flag3);
                bool flag4 = false;
                bool.TryParse(node.Attributes["IsImgShowSaleCounts"].Value, out flag4);
                sb.AppendLine("<div class=\"bd_saletop\">");
                sb.AppendLine("<ul>");
                int num2 = 0;
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                foreach (DataRow row in saleProductRanking.Rows)
                {
                    string defaultProductImage = siteSettings.DefaultProductImage;
                    if (row["ThumbnailUrl" + node.Attributes["TopImageSize"].Value] != DBNull.Value)
                    {
                        defaultProductImage = row["ThumbnailUrl" + node.Attributes["TopImageSize"].Value].ToString();
                    }
                    num2++;
                    sb.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
                    if (num2 <= result)
                    {
                        sb.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), Globals.ApplicationPath + defaultProductImage).AppendLine();
                    }
                    sb.AppendLine("<div class=\"info\">");
                    sb.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), row["ProductName"]).AppendLine();
                    if ((flag && (num2 > result)) || (flag3 && (num2 <= result)))
                    {
                        string str2 = string.Empty;
                        if (row["MarketPrice"] != DBNull.Value)
                        {
                            str2 = Globals.FormatMoney((decimal) row["MarketPrice"]);
                        }
                        sb.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal) row["SalePrice"]), str2).AppendLine();
                    }
                    if ((flag2 && (num2 > result)) || (flag4 && (num2 <= result)))
                    {
                        sb.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", row["SaleCounts"]).AppendLine();
                    }
                    sb.Append("</div>");
                    sb.AppendLine("</li>");
                }
                sb.AppendLine("</ul>");
                sb.AppendLine("</div>");
            }
        }

        public string RendHtml()
        {
            StringBuilder sb = new StringBuilder();
            XmlNode node = TagsHelper.FindProductNode(this.SubjectId, "group");
            if (node != null)
            {
                int.TryParse(node.Attributes["CategoryId"].Value, out this.categoryId);
                sb.AppendFormat("<div class=\"group{0} cssEdite\" type=\"group\" id=\"products_{1}\" >", node.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
                this.RenderHeader(node, sb);
                sb.AppendLine("<div class=\"group_bd\">");
                this.RenderLift(node, sb);
                this.RenderMiddle(node, sb);
                this.RenderRight(node, sb);
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

