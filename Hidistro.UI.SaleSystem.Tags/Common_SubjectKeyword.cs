namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Comments;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectKeyword : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "keyword");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                int categoryId = 0;
                int result = 0;
                int num3 = 0;
                int.TryParse(node.Attributes["CategoryId"].Value, out result);
                int.TryParse(node.Attributes["MaxNum"].Value, out num3);
                CategoryInfo category = CategoryBrowser.GetCategory(result);
                if (category != null)
                {
                    categoryId = category.TopCategoryId;
                }
                DataTable hotKeywords = CommentBrowser.GetHotKeywords(categoryId, num3);
                builder.AppendFormat("<ul class=\"keyword cssEdite\" type=\"keyword\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                if ((hotKeywords != null) && (hotKeywords.Rows.Count > 0))
                {
                    foreach (DataRow row in hotKeywords.Rows)
                    {
                        builder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory((int) row["CategoryId"], null) + "?keywords=" + Globals.UrlEncode((string) row["Keywords"]), row["Keywords"]).AppendLine();
                    }
                }
                builder.AppendLine("</ul>");
            }
            return builder.ToString();
        }

        public int CommentId
        {
            
            get
            {
                return _CommentId;
            }
            
            set
            {
                _CommentId = value;
            }
        }
    }
}

