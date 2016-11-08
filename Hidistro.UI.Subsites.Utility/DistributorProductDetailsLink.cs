namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DistributorProductDetailsLink : HyperLink
    {
        
        private bool _IsCountDownProduct;
        
        private bool _IsGroupBuyProduct;
        private bool imageLink;
        public const string TagID = "DistributorProductDetailsLink";
        private bool unSale;

        public DistributorProductDetailsLink()
        {
            base.ID = "DistributorProductDetailsLink";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.ProductId != null) && (this.ProductId != DBNull.Value))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (this.IsGroupBuyProduct)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[] { this.ProductId });
                }
                else if (this.IsCountDownProduct)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[] { this.ProductId });
                }
                else if (this.unSale)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[] { this.ProductId });
                }
                else
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.ProductId });
                }
            }
            if ((!this.imageLink && (this.ProductId != null)) && (this.ProductId != DBNull.Value))
            {
                base.Text = this.ProductName.ToString();
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public bool ImageLink
        {
            get
            {
                return this.imageLink;
            }
            set
            {
                this.imageLink = value;
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

        public object ProductId
        {
            get
            {
                if (this.ViewState["ProductId"] != null)
                {
                    return this.ViewState["ProductId"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["ProductId"] = value;
                }
            }
        }

        public object ProductName
        {
            get
            {
                if (this.ViewState["ProductName"] != null)
                {
                    return this.ViewState["ProductName"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["ProductName"] = value;
                }
            }
        }

        public bool UnSale
        {
            get
            {
                return this.unSale;
            }
            set
            {
                this.unSale = value;
            }
        }
    }
}

