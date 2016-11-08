namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Location : WebControl
    {
        
        private string _CateGoryPath;
        
        private string _ProductName;
        private string separatorString = ">>";
        public const string TagID = "common_Location";

        public Common_Location()
        {
            base.ID = "common_Location";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(this.CateGoryPath))
            {
                foreach (string str in this.CateGoryPath.Split(new char[] { '|' }))
                {
                    CategoryInfo category = CategoryBrowser.GetCategory(int.Parse(str));
                    if (category != null)
                    {
                        builder.AppendFormat("<a href ='{0}'>{1}</a>{2}", Globals.GetSiteUrls().SubCategory(category.CategoryId, category.RewriteName), category.Name, this.SeparatorString);
                    }
                }
                string str2 = builder.ToString();
                if (!string.IsNullOrEmpty(this.ProductName))
                {
                    str2 = str2 + this.ProductName;
                }
                else if (str2.Length > this.SeparatorString.Length)
                {
                    str2 = str2.Remove(str2.Length - this.SeparatorString.Length);
                }
                writer.Write(str2);
            }
        }

        public string CateGoryPath
        {
            
            get
            {
                return _CateGoryPath;
            }
            
            set
            {
                _CateGoryPath = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public string ProductName
        {
            
            get
            {
                return _ProductName;
            }
            
            set
            {
                _ProductName = value;
            }
        }

        public string SeparatorString
        {
            get
            {
                return this.separatorString;
            }
            set
            {
                this.separatorString = value;
            }
        }
    }
}

