namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_ImageAd : WebControl
    {
        
        private int _AdId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindAdNode(this.AdId, "image");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"adv_image cssEdite\" type=\"image\" id=\"ads_{0}\" >", this.AdId).AppendLine();
                builder.AppendFormat("<a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a>", string.IsNullOrEmpty(node.Attributes["Url"].Value) ? "#" : node.Attributes["Url"].Value, node.Attributes["Image"].Value).AppendLine();
                builder.AppendLine("</div>");
            }
            return builder.ToString();
        }

        public int AdId
        {
            
            get
            {
                return _AdId;
            }
            
            set
            {
                _AdId = value;
            }
        }
    }
}

