namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_GoodsList_CurrentTop : WebControl
    {
        
        private int _ImageNum;
        
        private bool _IsShowPrice;
        
        private bool _IsShowSaleCounts;
        
        private int _MaxNum;
        private int imageSize = 60;

        private DataTable GetProductList()
        {
            int num;
            SubjectListQuery entity = new SubjectListQuery();
            entity.CategoryIds = this.Page.Request.QueryString["categoryId"];
            if (int.TryParse(this.Page.Request.QueryString["brand"], out num))
            {
                entity.BrandCategoryId = new int?(num);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
            {
                IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
                string str = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]));
                string[] strArray = str.Split(new char[] { '-' });
                if (!string.IsNullOrEmpty(str))
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string[] strArray2 = strArray[i].Split(new char[] { '_' });
                        if (((strArray2.Length > 0) && !string.IsNullOrEmpty(strArray2[1])) && (strArray2[1] != "0"))
                        {
                            AttributeValueInfo item = new AttributeValueInfo();
                            item.AttributeId = Convert.ToInt32(strArray2[0]);
                            item.ValueId = Convert.ToInt32(strArray2[1]);
                            list.Add(item);
                        }
                    }
                }
                entity.AttributeValues = list;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                entity.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal result = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out result))
                {
                    entity.MinPrice = new decimal?(result);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal num4 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out num4))
                {
                    entity.MaxPrice = new decimal?(num4);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
            {
                int num5 = 0;
                if (int.TryParse(this.Page.Request.QueryString["TagIds"], out num5))
                {
                    entity.TagId = num5;
                }
            }
            entity.MaxNum = this.MaxNum;
            entity.SortBy = "ShowSaleCounts";
            entity.SortOrder = SortAction.Desc;
            Globals.EntityCoding(entity, true);
            return ProductBrowser.GetSubjectList(entity);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(this.RendHtml());
        }

        public string RendHtml()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<div class=\"sale_top\" >", new object[0]);
            DataTable productList = this.GetProductList();
            if ((productList != null) && (productList.Rows.Count > 0))
            {
                int num = 0;
                builder.AppendLine("<ul>");
                foreach (DataRow row in productList.Rows)
                {
                    num++;
                    builder.AppendFormat("<li class=\"saleitem{0}\">", num).AppendLine();
                    builder.AppendFormat("<em>{0}</em>", num).AppendLine();
                    if (num <= this.ImageNum)
                    {
                        builder.AppendFormat("<div class=\"img\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), Globals.ApplicationPath + row["ThumbnailUrl" + this.ImageSize]).AppendLine();
                    }
                    builder.AppendLine("<div class=\"info\">");
                    builder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }), row["ProductName"]).AppendLine();
                    if (this.IsShowPrice)
                    {
                        string str = string.Empty;
                        if (row["MarketPrice"] != DBNull.Value)
                        {
                            str = Globals.FormatMoney((decimal) row["MarketPrice"]);
                        }
                        builder.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal) row["RankPrice"]), str).AppendLine();
                    }
                    if (this.IsShowSaleCounts)
                    {
                        builder.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", row["SaleCounts"]).AppendLine();
                    }
                    builder.Append("</div>");
                    builder.AppendLine("</li>");
                }
                builder.AppendLine("</ul>");
            }
            builder.Append("</div>");
            return builder.ToString();
        }

        public int ImageNum
        {
            
            get
            {
                return _ImageNum;
            }
            
            set
            {
                _ImageNum = value;
            }
        }

        public int ImageSize
        {
            get
            {
                return this.imageSize;
            }
            set
            {
                this.imageSize = value;
            }
        }

        public bool IsShowPrice
        {
            
            get
            {
                return _IsShowPrice;
            }
            
            set
            {
                _IsShowPrice = value;
            }
        }

        public bool IsShowSaleCounts
        {
            
            get
            {
                return _IsShowSaleCounts;
            }
            
            set
            {
                _IsShowSaleCounts = value;
            }
        }

        public int MaxNum
        {
            
            get
            {
                return _MaxNum;
            }
            
            set
            {
                _MaxNum = value;
            }
        }
    }
}

