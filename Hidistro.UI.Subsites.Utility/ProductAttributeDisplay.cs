namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Subsites.Commodities;
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductAttributeDisplay : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["ProductId"], out result))
            {
                DataTable productAttribute = SubSiteProducthelper.GetProductAttribute(result);
                if ((productAttribute != null) && (productAttribute.Rows.Count > 0))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<input type=\"hidden\" id=\"attributeContent\" value=\"1\" />");
                    builder.Append("<span  class=\"Property\">");
                    builder.Append("<table width=\"0\" border=\"0\" cellspacing=\"0\">");
                    foreach (DataRow row in productAttribute.Rows)
                    {
                        builder.AppendFormat("<tr><td width=\"20%\" align=\"right\">{0}：</td><td>{1}</td>", row["AttributeName"], row["ValueStr"]);
                    }
                    builder.Append("</table>");
                    builder.Append("</span>");
                    writer.Write(builder.ToString());
                    return;
                }
            }
            writer.Write("<input type=\"hidden\" id=\"attributeContent\" value=\"0\" />");
        }
    }
}

