namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.SaleSystem.Comments;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectArticle : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        private void RenderHeader(XmlNode node, StringBuilder sb)
        {
            sb.AppendLine("<div class=\"article_hd\">");
            sb.AppendLine("<h2>");
            if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
            {
                sb.AppendFormat("<img src=\"{0}\" />", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
            }
            if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
            {
                sb.Append(node.Attributes["Title"].Value);
            }
            sb.AppendLine("</h2>");
            sb.AppendLine("</div>");
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "article");
            StringBuilder sb = new StringBuilder();
            if (node != null)
            {
                sb.AppendFormat("<div class=\"article cssEdite\" type=\"article\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                this.RenderHeader(node, sb);
                sb.AppendLine("<div class=\"article_bd\">");
                if (!string.IsNullOrEmpty(node.Attributes["AdImage"].Value))
                {
                    sb.AppendFormat("<div class=\"article_ad\"><img src=\"{0}\" /></div>", node.Attributes["AdImage"].Value).AppendLine();
                }
                int result = 0;
                int num2 = 0;
                int.TryParse(node.Attributes["CategoryId"].Value, out result);
                int.TryParse(node.Attributes["MaxNum"].Value, out num2);
                IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(result, num2);
                if ((articleList != null) && (articleList.Count > 0))
                {
                    sb.AppendLine("<div class=\"article_list\">");
                    sb.AppendLine("<ul>");
                    foreach (ArticleInfo info in articleList)
                    {
                        sb.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { info.ArticleId }), info.Title).AppendLine();
                    }
                    sb.AppendLine("</ul>");
                    sb.AppendLine("</div>");
                }
                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
            }
            return sb.ToString();
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

