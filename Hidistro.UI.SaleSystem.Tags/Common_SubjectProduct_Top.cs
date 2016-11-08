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

    public class Common_SubjectProduct_Top : WebControl
    {
        
        private int _SubjectId;

        private DataTable GetProductList(XmlNode node)
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "ShowSaleCounts";
            query.SortOrder = SortAction.Desc;
            query.CategoryIds = node.Attributes["CategoryId"].Value;
            query.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
            return ProductBrowser.GetSubjectList(query);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindProductNode(this.SubjectId, "top");
            StringBuilder builder = new StringBuilder();
            if (node != null)
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
                builder.AppendFormat("<div class=\"sale_top{0} cssEdite\" type=\"top\" id=\"products_{1}\" >", node.Attributes["ImageSize"].Value, this.SubjectId);
                DataTable productList = this.GetProductList(node);
                if ((productList != null) && (productList.Rows.Count > 0))
                {
                    int num2 = 0;
                    builder.AppendLine("<ul>");
                    foreach (DataRow row in productList.Rows)
                    {
                        string defaultProductImage = SettingsManager.GetMasterSettings(false).DefaultProductImage;
                        if (row["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
                        {
                            defaultProductImage = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
                        }
                        num2++;
                        builder.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
                        builder.AppendFormat("<em>{0}</em>", num2).AppendLine();
                        if (num2 <= result)
                        {
                            builder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), Globals.ApplicationPath + defaultProductImage).AppendLine();
                        }
                        builder.AppendLine("<div class=\"info\">");
                        builder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), row["ProductName"]).AppendLine();
                        if ((flag && (num2 > result)) || (flag3 && (num2 <= result)))
                        {
                            string str2 = string.Empty;
                            if (row["MarketPrice"] != DBNull.Value)
                            {
                                str2 = Globals.FormatMoney((decimal) row["MarketPrice"]);
                            }
                            builder.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal) row["RankPrice"]), str2).AppendLine();
                        }
                        if ((flag2 && (num2 > result)) || (flag4 && (num2 <= result)))
                        {
                            builder.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", row["SaleCounts"]).AppendLine();
                        }
                        builder.Append("</div>");
                        builder.AppendLine("</li>");
                    }
                    builder.AppendLine("</ul>");
                }
                builder.Append("</div>");
            }
            return builder.ToString();
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

