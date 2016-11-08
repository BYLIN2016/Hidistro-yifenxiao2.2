namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectCategory : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "category");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"category cssEdite\" type=\"category\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                int result = 0;
                int num2 = 0;
                int.TryParse(node.Attributes["CategoryId"].Value, out result);
                int.TryParse(node.Attributes["MaxNum"].Value, out num2);
                IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(result, num2);
                if ((maxSubCategories != null) && (maxSubCategories.Count > 0))
                {
                    builder.AppendLine("<ul>");
                    foreach (CategoryInfo info in maxSubCategories)
                    {
                        builder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(info.CategoryId, info.RewriteName), info.Name).AppendLine();
                    }
                    builder.AppendLine("</ul>");
                }
                builder.AppendLine("</div>");
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

