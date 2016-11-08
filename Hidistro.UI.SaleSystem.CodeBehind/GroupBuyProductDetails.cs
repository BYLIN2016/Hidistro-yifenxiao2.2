namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI.WebControls;

    public class GroupBuyProductDetails : HtmlTemplatedWebControl
    {
        private BuyButton btnOrder;
        private Common_Location common_Location;
        private Common_ProductConsultations consultations;
        private Common_GoodsList_Correlative correlative;
        private HyperLink hpkProductConsultations;
        private HyperLink hpkProductReviews;
        private Common_ProductImages images;
        private FormatedMoneyLabel lblCurrentSalePrice;
        private FormatedTimeLabel lblEndTime;
        private FormatedMoneyLabel lblNeedPrice;
        private FormatedMoneyLabel lblSalePrice;
        private SkuLabel lblSku;
        private FormatedTimeLabel lblStartTime;
        private StockLabel lblStock;
        private TotalLabel lblTotalPrice;
        private Literal litBrand;
        private Literal litBrosedNum;
        private Literal litContent;
        private Label litCount;
        private Literal litDescription;
        private Label litMaxCount;
        private Literal litNeedCount;
        private Literal litProductName;
        private Literal litRemainTime;
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
            this.lblSku = (SkuLabel) this.FindControl("lblSku");
            this.lblStock = (StockLabel) this.FindControl("lblStock");
            this.litUnit = (Literal) this.FindControl("litUnit");
            this.litWeight = (Label) this.FindControl("litWeight");
            this.litBrosedNum = (Literal) this.FindControl("litBrosedNum");
            this.litBrand = (Literal) this.FindControl("litBrand");
            this.lblSalePrice = (FormatedMoneyLabel) this.FindControl("lblSalePrice");
            this.lblTotalPrice = (TotalLabel) this.FindControl("lblTotalPrice");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.btnOrder = (BuyButton) this.FindControl("btnOrder");
            this.hpkProductConsultations = (HyperLink) this.FindControl("hpkProductConsultations");
            this.hpkProductReviews = (HyperLink) this.FindControl("hpkProductReviews");
            this.lblCurrentSalePrice = (FormatedMoneyLabel) this.FindControl("lblCurrentSalePrice");
            this.litCount = (Label) this.FindControl("litCount");
            this.lblNeedPrice = (FormatedMoneyLabel) this.FindControl("lblNeedPrice");
            this.litContent = (Literal) this.FindControl("litContent");
            this.lblEndTime = (FormatedTimeLabel) this.FindControl("lblEndTime");
            this.lblStartTime = (FormatedTimeLabel) this.FindControl("lblStartTime");
            this.litRemainTime = (Literal) this.FindControl("litRemainTime");
            this.litNeedCount = (Literal) this.FindControl("litNeedCount");
            this.litMaxCount = (Label) this.FindControl("litMaxCount");
            this.images = (Common_ProductImages) this.FindControl("common_ProductImages");
            this.rptExpandAttributes = (ThemedTemplatedRepeater) this.FindControl("rptExpandAttributes");
            this.skuSelector = (SKUSelector) this.FindControl("SKUSelector");
            this.reviews = (Common_ProductReview) this.FindControl("list_Common_ProductReview");
            this.consultations = (Common_ProductConsultations) this.FindControl("list_Common_ProductConsultations");
            this.correlative = (Common_GoodsList_Correlative) this.FindControl("list_Common_GoodsList_Correlative");
            if (!this.Page.IsPostBack)
            {
                ProductBrowseInfo info = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(this.reviews.MaxNum), new int?(this.consultations.MaxNum));
                GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(this.productId);
                if ((info.Product == null) || (productGroupBuyInfo == null))
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品参与的团购活动已经结束；或已被管理员删除"));
                }
                else
                {
                    this.LoadPageSearch(info.Product);
                    this.hpkProductReviews.Text = "查看全部" + ProductBrowser.GetProductReviewNumber(this.productId).ToString() + "条评论";
                    this.hpkProductConsultations.Text = "查看全部" + ProductBrowser.GetProductConsultationNumber(this.productId).ToString() + "条咨询";
                    this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
                    this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
                    this.LoadProductInfo(info.Product, info.BrandName);
                    this.LoadProductGroupBuyInfo(productGroupBuyInfo);
                    this.btnOrder.Stock = info.Product.Stock;
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
                    if (info.DBReviews != null)
                    {
                        this.reviews.DataSource = info.DBReviews;
                        this.reviews.DataBind();
                    }
                    if (info.DBConsultations != null)
                    {
                        this.consultations.DataSource = info.DBConsultations;
                        this.consultations.DataBind();
                    }
                    if (info.DbCorrelatives != null)
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

        private void LoadProductGroupBuyInfo(GroupBuyInfo groupBuy)
        {
            int orderCount = ProductBrowser.GetOrderCount(groupBuy.GroupBuyId);
            decimal currentPrice = ProductBrowser.GetCurrentPrice(groupBuy.GroupBuyId, orderCount);
            this.litCount.Text = orderCount.ToString();
            this.lblCurrentSalePrice.Money = currentPrice;
            this.lblNeedPrice.Money = groupBuy.NeedPrice;
            this.litMaxCount.Text = groupBuy.MaxCount.ToString();
            this.litContent.Text = groupBuy.Content;
            this.lblTotalPrice.Value = new decimal?(currentPrice);
            this.lblEndTime.Time = groupBuy.EndDate;
            this.lblStartTime.Time = groupBuy.StartDate;
            this.litRemainTime.Text = "";
            this.litNeedCount.Text = groupBuy.GroupBuyConditions[0].Count.ToString();
        }

        private void LoadProductInfo(ProductInfo productDetails, string brandName)
        {
            if ((this.common_Location != null) && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
            {
                this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
                this.common_Location.ProductName = productDetails.ProductName;
            }
            this.litProductName.Text = productDetails.ProductName;
            this.lblSku.Value = productDetails.SkuId;
            this.lblSku.Text = productDetails.SKU;
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
            this.lblSalePrice.Money = productDetails.MaxSalePrice;
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
                this.SkinName = "Skin-GroupBuyProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

