namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SlideAd : WebControl
    {
        
        private int _AdId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindAdNode(this.AdId, "slide");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                string str = "600";
                string str2 = "300";
                if (!string.IsNullOrEmpty(node.Attributes["AdImageSize"].Value) && node.Attributes["AdImageSize"].Value.Contains("*"))
                {
                    str = node.Attributes["AdImageSize"].Value.Split(new char[] { '*' })[0];
                    str2 = node.Attributes["AdImageSize"].Value.Split(new char[] { '*' })[1];
                }
                builder.AppendFormat("<div class=\"ad_slide cssEdite\" type=\"slide\" id=\"ads_{0}\" >", this.AdId).AppendLine();
                builder.AppendLine("<div class=\"focusWarp\">");
                builder.AppendLine("<ul class=\"imgList\">");
                if (!string.IsNullOrEmpty(node.Attributes["Image1"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url1"].Value.Length == 0) ? "#" : node.Attributes["Url1"].Value, node.Attributes["Image1"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image2"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url2"].Value.Length == 0) ? "#" : node.Attributes["Url2"].Value, node.Attributes["Image2"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image3"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url3"].Value.Length == 0) ? "#" : node.Attributes["Url3"].Value, node.Attributes["Image3"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image4"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url4"].Value.Length == 0) ? "#" : node.Attributes["Url4"].Value, node.Attributes["Image4"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image5"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url5"].Value.Length == 0) ? "#" : node.Attributes["Url5"].Value, node.Attributes["Image5"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image6"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url6"].Value.Length == 0) ? "#" : node.Attributes["Url6"].Value, node.Attributes["Image6"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image7"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url7"].Value.Length == 0) ? "#" : node.Attributes["Url7"].Value, node.Attributes["Image7"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image8"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url8"].Value.Length == 0) ? "#" : node.Attributes["Url8"].Value, node.Attributes["Image8"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image9"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url9"].Value.Length == 0) ? "#" : node.Attributes["Url9"].Value, node.Attributes["Image9"].Value, str, str2 }).AppendLine();
                }
                if (!string.IsNullOrEmpty(node.Attributes["Image10"].Value))
                {
                    builder.AppendFormat("<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[] { (node.Attributes["Url10"].Value.Length == 0) ? "#" : node.Attributes["Url10"].Value, node.Attributes["Image10"].Value, str, str2 }).AppendLine();
                }
                builder.AppendLine("</ul>");
                builder.AppendLine("</div>");
                builder.AppendLine("</div>");
                builder.AppendLine("<script type=\"text/javascript\">");
                builder.Append("$(function(){");
                builder.AppendFormat("$(\"#ads_{0}\").mogFocus({{scrollWidth:" + str + "}}); ", this.AdId).AppendLine();
                builder.Append("});");
                builder.AppendLine("</script>");
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

