namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_SubjectProduct_Simple : WebControl
    {
        
        private int _SubjectId;

        private DataTable GetProductList(XmlNode node)
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            query.CategoryIds = node.Attributes["CategoryId"].Value;
            if (!string.IsNullOrEmpty(node.Attributes["TagId"].Value))
            {
                query.TagId = int.Parse(node.Attributes["TagId"].Value);
            }
            if (!string.IsNullOrEmpty(node.Attributes["BrandId"].Value))
            {
                query.BrandCategoryId = new int?(int.Parse(node.Attributes["BrandId"].Value));
            }
            if (!string.IsNullOrEmpty(node.Attributes["TypeId"].Value))
            {
                query.ProductTypeId = new int?(int.Parse(node.Attributes["TypeId"].Value));
            }
            string str = node.Attributes["AttributeString"].Value;
            if (!string.IsNullOrEmpty(str))
            {
                IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
                string[] strArray = str.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    string[] strArray2 = strArray[i].Split(new char[] { '_' });
                    AttributeValueInfo item = new AttributeValueInfo();
                    item.AttributeId = Convert.ToInt32(strArray2[0]);
                    item.ValueId = Convert.ToInt32(strArray2[1]);
                    list.Add(item);
                }
                query.AttributeValues = list;
            }
            query.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
            return ProductBrowser.GetSubjectList(query);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            XmlNode node = TagsHelper.FindProductNode(this.SubjectId, "simple");
            StringBuilder builder = new StringBuilder();
            if (node != null)
            {
                builder.AppendFormat("<div class=\"pro_simple{0} cssEdite\" type=\"simple\" id=\"products_{1}\" >", node.Attributes["ImageSize"].Value, this.SubjectId);
                DataTable productList = this.GetProductList(node);
                if ((productList != null) && (productList.Rows.Count > 0))
                {
                    builder.AppendLine("<ul>");
                    foreach (DataRow row in productList.Rows)
                    {
                        string defaultProductImage = SettingsManager.GetMasterSettings(true).DefaultProductImage;
                        if (row["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
                        {
                            defaultProductImage = row["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
                        }
                        builder.AppendLine("<li>");
                        builder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), Globals.ApplicationPath + defaultProductImage).AppendLine();
                        builder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), row["ProductName"]).AppendLine();
                        string str2 = string.Empty;
                        if (row["MarketPrice"] != DBNull.Value)
                        {
                            str2 = Globals.FormatMoney((decimal) row["MarketPrice"]);
                        }
                        builder.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal) row["RankPrice"]), str2).AppendLine();
                        builder.AppendLine("</li>");
                    }
                    builder.AppendLine("</ul>");
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public int SubjectId
        {
            
            get
            {
                return _SubjectId;
            }
            
            set
            {
                _SubjectId = value;
            }
        }
    }
}

