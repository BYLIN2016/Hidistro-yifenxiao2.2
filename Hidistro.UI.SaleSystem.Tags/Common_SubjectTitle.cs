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

    public class Common_SubjectTitle : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "title");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"title cssEdite\" type=\"title\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                builder.AppendLine("<div>");
                if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
                {
                    builder.AppendFormat("<span class=\"icon\"><img src=\"{0}\" /></span>", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
                }
                if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
                {
                    builder.AppendFormat("<span class=\"title\">{0}</span>", node.Attributes["Title"].Value);
                }
                builder.AppendLine("</div>");
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

