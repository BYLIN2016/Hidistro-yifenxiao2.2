namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectBrand : WebControl
    {
        
        private int _CommentId;

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindCommentNode(this.CommentId, "brand");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"brand cssEdite\" type=\"brand\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
                int result = 0;
                int num2 = 0;
                bool flag = true;
                bool flag2 = true;
                string str = "";
                int.TryParse(node.Attributes["CategoryId"].Value, out result);
                int.TryParse(node.Attributes["MaxNum"].Value, out num2);
                bool.TryParse(node.Attributes["IsShowLogo"].Value, out flag);
                bool.TryParse(node.Attributes["IsShowTitle"].Value, out flag2);
                str = node.Attributes["ImageSize"].Value;
                DataTable brandCategories = CategoryBrowser.GetBrandCategories(result, num2);
                if ((brandCategories != null) && (brandCategories.Rows.Count > 0))
                {
                    builder.AppendLine("<ul>");
                    foreach (DataRow row in brandCategories.Rows)
                    {
                        builder.AppendLine("<li>");
                        if (flag)
                        {
                            builder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" width=\"{2}\"></a></div>", Globals.GetSiteUrls().SubBrandDetails((int) row["BrandId"], row["RewriteName"]), row["Logo"], str.Split(new char[] { '*' })[0]).AppendLine();
                        }
                        if (flag2)
                        {
                            builder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().SubBrandDetails((int) row["BrandId"], row["RewriteName"]), row["BrandName"]).AppendLine();
                        }
                        builder.AppendLine("</li>");
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

