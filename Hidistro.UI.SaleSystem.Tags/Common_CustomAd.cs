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

    public class Common_CustomAd : WebControl
    {
        
        private int _AdId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindAdNode(this.AdId, "custom");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"ad_custom cssEdite\" type=\"custom\" id=\"ads_{0}\" >{1}</div>", this.AdId, Globals.HtmlDecode(node.Attributes["Html"].Value)).AppendLine();
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

