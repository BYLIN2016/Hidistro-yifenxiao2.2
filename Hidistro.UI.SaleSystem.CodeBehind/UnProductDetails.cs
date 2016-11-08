namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI.WebControls;

    public class UnProductDetails : HtmlTemplatedWebControl
    {
        private Common_Location common_Location;
        private Common_ProductConsultations consultations;
        private Common_GoodsList_Correlative correlative;
        private HyperLink hpkProductConsultations;
        private HyperLink hpkProductReviews;
        private Common_ProductImages images;
        private Label lblBuyPrice;
        private FormatedMoneyLabel lblMarkerPrice;
        private Literal lblProductCode;
        private SkuLabel lblSku;
        private StockLabel lblStock;
        private TotalLabel lblTotalPrice;
        private Literal litBrand;
        private Literal litBrosedNum;
        private Literal litDescription;
        private Literal litProductName;
        private Literal litShortDescription;
        private Literal litUnit;
        private Label litWeight;
        private int productId;
        private Common_ProductReview reviews;
        private ThemedTemplatedRepeater rptExpandAttributes;
        private SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.common_Location = (Common_Location) this.FindControl("common_Location");
            this.litProductName = (Literal) this.FindControl("litProductName");
            this.lblProductCode = (Literal) this.FindControl("lblProductCode");
            this.lblSku = (SkuLabel) this.FindControl("lblSku");
            this.lblStock = (StockLabel) this.FindControl("lblStock");
            this.litUnit = (Literal) this.FindControl("litUnit");
            this.litWeight = (Label) this.FindControl("litWeight");
            this.litBrosedNum = (Literal) this.FindControl("litBrosedNum");
            this.litBrand = (Literal) this.FindControl("litBrand");
            this.lblMarkerPrice = (FormatedMoneyLabel) this.FindControl("lblMarkerPrice");
            this.lblBuyPrice = (Label) this.FindControl("lblBuyPrice");
            this.lblTotalPrice = (TotalLabel) this.FindControl("lblTotalPrice");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.hpkProductConsultations = (HyperLink) this.FindControl("hpkProductConsultations");
            this.hpkProductReviews = (HyperLink) this.FindControl("hpkProductReviews");
            this.images = (Common_ProductImages) this.FindControl("common_ProductImages");
            this.rptExpandAttributes = (ThemedTemplatedRepeater) this.FindControl("rptExpandAttributes");
            this.skuSelector = (SKUSelector) this.FindControl("SKUSelector");
            this.reviews = (Common_ProductReview) this.FindControl("list_Common_ProductReview");
            this.consultations = (Common_ProductConsultations) this.FindControl("list_Common_ProductConsultations");
            this.correlative = (Common_GoodsList_Correlative) this.FindControl("list_Common_GoodsList_Correlative");
            if (!this.Page.IsPostBack)
            {
                int maxNum = 0;
                if (this.reviews != null)
                {
                    maxNum = this.reviews.MaxNum;
                }
                int num2 = 0;
                if (this.consultations != null)
                {
                    num2 = this.consultations.MaxNum;
                }
                ProductBrowseInfo info = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(maxNum), new int?(num2));
                if ((info.Product == null) || (info.Product.SaleStatus == ProductSaleStatus.Delete))
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
                }
                else if (info.Product.SaleStatus == ProductSaleStatus.OnSale)
                {
                    Globals.GetSiteUrls().UrlData.FormatUrl("productdetails", new object[] { this.Page.Request.QueryString["productId"] });
                }
                else
                {
                    this.LoadPageSearch(info.Product);
                    if (this.hpkProductReviews != null)
                    {
                        this.hpkProductReviews.Text = "查看全部" + ProductBrowser.GetProductReviewNumber(this.productId).ToString() + "条评论";
                        this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
                    }
                    if (this.hpkProductConsultations != null)
                    {
                        this.hpkProductConsultations.Text = "查看全部" + ProductBrowser.GetProductConsultationNumber(this.productId).ToString() + "条咨询";
                        this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
                    }
                    this.LoadProductInfo(info.Product, info.BrandName);
                    BrowsedProductQueue.EnQueue(this.productId);
                    this.images.ImageInfo = info.Product;
                    if (info.DbAttribute != null)
                    {
                        this.rptExpandAttributes.DataSource = info.DbAttribute;
                        this.rptExpandAttributes.DataBind();
                    }
                    if (info.DbSKUs != null)
                    {
                        this.skuSelector.ProductId = this.productId;
                        this.skuSelector.DataSource = info.DbSKUs;
                    }
                    if ((this.reviews != null) && (info.DBReviews != null))
                    {
                        this.reviews.DataSource = info.DBReviews;
                        this.reviews.DataBind();
                    }
                    if ((this.consultations != null) && (info.DBConsultations != null))
                    {
                        this.consultations.DataSource = info.DBConsultations;
                        this.consultations.DataBind();
                    }
                    if ((this.correlative != null) && (info.DbCorrelatives != null))
                    {
                        this.correlative.DataSource = info.DbCorrelatives;
                        this.correlative.DataBind();
                    }
                }
            }
        }

        private void LoadPageSearch(ProductInfo productDetails)
        {
            if (!string.IsNullOrEmpty(productDetails.MetaKeywords))
            {
                MetaTags.AddMetaKeywords(productDetails.MetaKeywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.MetaDescription))
            {
                MetaTags.AddMetaDescription(productDetails.MetaDescription, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.Title))
            {
                PageTitle.AddSiteNameTitle(productDetails.Title, HiContext.Current.Context);
            }
            else
            {
                PageTitle.AddSiteNameTitle(productDetails.ProductName, HiContext.Current.Context);
            }
        }

        private void LoadProductInfo(ProductInfo productDetails, string brandName)
        {
            if ((this.common_Location != null) && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
            {
                this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
                this.common_Location.ProductName = productDetails.ProductName;
            }
            this.litProductName.Text = productDetails.ProductName;
            this.lblProductCode.Text = productDetails.ProductCode;
            this.lblSku.Text = productDetails.SKU;
            this.lblSku.Value = productDetails.SkuId;
            this.lblStock.Stock = productDetails.Stock;
            this.litUnit.Text = productDetails.Unit;
            if (productDetails.Weight > 0M)
            {
                this.litWeight.Text = string.Format("{0} g", productDetails.Weight);
            }
            else
            {
                this.litWeight.Text = "无";
            }
            this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
            this.litBrand.Text = brandName;
            if (productDetails.MinSalePrice == productDetails.MaxSalePrice)
            {
                this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2");
            }
            else
            {
                this.lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2") + " - " + productDetails.MaxSalePrice.ToString("F2");
            }
            this.lblMarkerPrice.Money = productDetails.MarketPrice;
            this.litDescription.Text = productDetails.Description;
            if (this.litShortDescription != null)
            {
                this.litShortDescription.Text = productDetails.ShortDescription;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-UnProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

