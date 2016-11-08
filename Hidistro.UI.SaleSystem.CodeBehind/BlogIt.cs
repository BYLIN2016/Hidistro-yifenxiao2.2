namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI.WebControls;

    public class BlogIt : HtmlTemplatedWebControl
    {
        private HyperLink hlinkProductContent;
        private HyperLink hlinkProductTitle;
        private HiImage imgUrl;
        private Label lblImgUrl;
        private Label lblProductNameLinkText;
        private Label lblUrl;
        private Label lblUrl2;
        private int productId;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productID"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.lblProductNameLinkText = (Label) this.FindControl("lblProductNameLinkText");
            this.hlinkProductTitle = (HyperLink) this.FindControl("hlinkProductTitle");
            this.hlinkProductContent = (HyperLink) this.FindControl("hlinkProductContent");
            this.lblImgUrl = (Label) this.FindControl("lblImgUrl");
            this.lblUrl = (Label) this.FindControl("lblUrl");
            this.lblUrl2 = (Label) this.FindControl("lblUrl2");
            this.imgUrl = (HiImage) this.FindControl("imgUrl");
            if (!this.Page.IsPostBack)
            {
                ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
                if (productSimpleInfo == null)
                {
                    base.GotoResourceNotFound();
                }
                PageTitle.AddSiteNameTitle(productSimpleInfo.ProductName + " 推荐到博客", HiContext.Current.Context);
                string name = "productDetails";
                if (productSimpleInfo.SaleStatus == ProductSaleStatus.UnSale)
                {
                    name = "unproductdetails";
                }
                string str2 = Globals.GetSiteUrls().UrlData.FormatUrl(name, new object[] { this.productId });
                this.hlinkProductTitle.Text = this.hlinkProductContent.Text = productSimpleInfo.ProductName;
                this.hlinkProductTitle.NavigateUrl = this.hlinkProductContent.NavigateUrl = str2;
                this.lblProductNameLinkText.Text = string.Format("插入这段代码，可以在你的博客中显示“{0}”的文字链接", string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", str2, "Text"));
                this.imgUrl.ImageUrl = productSimpleInfo.ImageUrl1;
                if (!string.IsNullOrEmpty(productSimpleInfo.ImageUrl1))
                {
                    this.lblImgUrl.Text = Globals.FullPath(Globals.ApplicationPath + this.imgUrl.ImageUrl);
                }
                IUser user = HiContext.Current.User;
                if ((user.UserRole == UserRole.Member) || (user.UserRole == UserRole.Underling))
                {
                    this.lblUrl.Text = this.lblUrl2.Text = Globals.FullPath(HttpContext.Current.Request.Url.PathAndQuery).Replace("BlogIt", name) + "&ReferralUserId=" + user.UserId;
                }
                else
                {
                    this.lblUrl.Text = this.lblUrl2.Text = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl(name, new object[] { this.productId }));
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-BlogIt.html";
            }
            base.OnInit(e);
        }
    }
}

