namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductDetailsLink : HyperLink
    {
        
        private bool _ImageLink;
        
        private bool _IsCountDownProduct;
        
        private bool _IsGroupBuyProduct;
        
        private bool _IsUnSale;
        
        private object _ProductId;
        
        private object _ProductName;
        
        private int? _StringLenth;
        public const string TagID = "ProductDetailsLink";

        public ProductDetailsLink()
        {
            base.ID = "ProductDetailsLink";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.ProductId != null) && (this.ProductId != DBNull.Value))
            {
                if (this.IsGroupBuyProduct)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[] { this.ProductId });
                }
                else if (this.IsCountDownProduct)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[] { this.ProductId });
                }
                else if (this.IsUnSale)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[] { this.ProductId });
                }
                else
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.ProductId });
                }
            }
            if ((!this.ImageLink && (this.ProductId != null)) && (this.ProductId != DBNull.Value))
            {
                if (this.StringLenth.HasValue && (this.ProductName.ToString().Length > this.StringLenth.Value))
                {
                    base.Text = this.ProductName.ToString().Substring(0, this.StringLenth.Value) + "...";
                }
                else
                {
                    base.Text = this.ProductName.ToString();
                }
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public bool ImageLink
        {
            
            get
            {
                return _ImageLink;
            }
            
            set
            {
                _ImageLink = value;
            }
        }

        public bool IsCountDownProduct
        {
            
            get
            {
                return _IsCountDownProduct;
            }
            
            set
            {
                _IsCountDownProduct = value;
            }
        }

        public bool IsGroupBuyProduct
        {
            
            get
            {
                return _IsGroupBuyProduct;
            }
            
            set
            {
                _IsGroupBuyProduct = value;
            }
        }

        public bool IsUnSale
        {
            
            get
            {
                return _IsUnSale;
            }
            
            set
            {
                _IsUnSale = value;
            }
        }

        public object ProductId
        {
            
            get
            {
                return _ProductId;
            }
            
            set
            {
                _ProductId = value;
            }
        }

        public object ProductName
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

        public int? StringLenth
        {
            
            get
            {
                return _StringLenth;
            }
            
            set
            {
                _StringLenth = value;
            }
        }
    }
}

