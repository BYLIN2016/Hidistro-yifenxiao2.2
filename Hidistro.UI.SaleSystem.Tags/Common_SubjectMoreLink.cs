namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectMoreLink : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "morelink");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"morelink cssEdite\" type=\"morelink\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                int result = 0;
                if (int.TryParse(node.Attributes["CategoryId"].Value, out result))
                {
                    builder.AppendFormat("<em><a href=\"{0}\">{1}</a></em>", Globals.GetSiteUrls().SubCategory(result, null), node.Attributes["Title"].Value).AppendLine();
                }
                else
                {
                    builder.AppendFormat("<em><a href=\"{0}/SubCategory.aspx\">{1}</a></em>", Globals.ApplicationPath, node.Attributes["Title"].Value).AppendLine();
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

