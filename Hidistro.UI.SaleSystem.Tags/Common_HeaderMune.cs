namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_HeaderMune : WebControl
    {
        private DataTable GetHeaderMune()
        {
            string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/master/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Templates/sites/" + HiContext.Current.SiteSettings.UserId + "/{0}/config/HeaderMenu.xml", HiContext.Current.SiteSettings.Theme));
            }
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            DataTable table = new DataTable();
            table.Columns.Add("Title");
            table.Columns.Add("DisplaySequence", typeof(int));
            table.Columns.Add("Category");
            table.Columns.Add("Url");
            table.Columns.Add("Where");
            table.Columns.Add("Visible");
            foreach (XmlNode node in document.SelectSingleNode("root").ChildNodes)
            {
                if (node.Attributes["Visible"].Value.ToLower() == "true")
                {
                    DataRow row = table.NewRow();
                    row["Title"] = node.Attributes["Title"].Value;
                    row["DisplaySequence"] = int.Parse(node.Attributes["DisplaySequence"].Value);
                    row["Category"] = node.Attributes["Category"].Value;
                    row["Url"] = node.Attributes["Url"].Value;
                    row["Where"] = node.Attributes["Where"].Value;
                    row["Visible"] = node.Attributes["Visible"].Value;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        private string GetUrl(string category, string url, string where)
        {
            string str = url;
            if (category == "1")
            {
                return Globals.GetSiteUrls().UrlData.FormatUrl(url);
            }
            if (category == "2")
            {
                string[] strArray = where.Split(new char[] { ',' });
                str = Globals.ApplicationPath + string.Format("/SubCategory.aspx?keywords={0}&minSalePrice={1}&maxSalePrice={2}", strArray[5], strArray[3], strArray[4]);
                if (strArray[0] != "0")
                {
                    str = str + "&categoryId=" + strArray[0];
                }
                if (strArray[1] != "0")
                {
                    str = str + "&brand=" + strArray[1];
                }
                if (strArray[2] != "0")
                {
                    str = str + "&TagIds=" + strArray[2];
                }
            }
            return str;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            DataTable headerMune = this.GetHeaderMune();
            if (headerMune.Rows.Count > 0)
            {
                foreach (DataRow row in headerMune.Select("", "DisplaySequence ASC"))
                {
                    builder.AppendFormat("<li> <a href=\"{0}\"><span>{1}</span></a></li>", this.GetUrl((string) row["Category"], (string) row["Url"], (string) row["Where"]), row["Title"]);
                }
                writer.Write(builder.ToString());
            }
        }
    }
}

