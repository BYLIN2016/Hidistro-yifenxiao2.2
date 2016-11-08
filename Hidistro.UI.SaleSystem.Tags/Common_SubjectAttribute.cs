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

    public class Common_SubjectAttribute : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            StringBuilder builder = new StringBuilder();
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "attribute");
            if (node != null)
            {
                builder.AppendFormat("<div class=\"attribute_bd cssEdite\" type=\"attribute\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                int result = 0;
                int num2 = 0;
                int.TryParse(node.Attributes["CategoryId"].Value, out result);
                int.TryParse(node.Attributes["MaxNum"].Value, out num2);
                string rewriteName = null;
                CategoryInfo category = CategoryBrowser.GetCategory(result);
                if (category != null)
                {
                    rewriteName = category.RewriteName;
                }
                IList<AttributeInfo> attributeInfoByCategoryId = CategoryBrowser.GetAttributeInfoByCategoryId(result, 0x3e8);
                if ((attributeInfoByCategoryId != null) && (attributeInfoByCategoryId.Count > 0))
                {
                    foreach (AttributeInfo info2 in attributeInfoByCategoryId)
                    {
                        builder.AppendLine("<dl class=\"attribute_dl\">");
                        builder.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", info2.AttributeName).AppendLine();
                        builder.AppendLine("<dd class=\"attribute_val\">");
                        builder.AppendLine("<div class=\"h_chooselist\">");
                        foreach (AttributeValueInfo info3 in info2.AttributeValues)
                        {
                            builder.AppendFormat("<a href=\"{0}\" >{1}</a>", string.Concat(new object[] { Globals.GetSiteUrls().SubCategory(result, rewriteName), "?valueStr=", info3.AttributeId, "_", info3.ValueId }), info3.ValueStr).AppendLine();
                        }
                        builder.AppendLine("</div>");
                        builder.AppendLine("</dd>");
                        builder.AppendLine("</dl>");
                    }
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

