namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class ArticleDetails : HtmlTemplatedWebControl
    {
        private HtmlAnchor aFront;
        private HtmlAnchor aNext;
        private Common_ArticleRelative ariticlative;
        private int articleId;
        private Label lblFront;
        private Label lblFrontTitle;
        private Label lblNext;
        private Label lblNextTitle;
        private FormatedTimeLabel litArticleAddedDate;
        private Literal litArticleContent;
        private Literal litArticleDescription;
        private Literal litArticleTitle;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
            {
                base.GotoResourceNotFound();
            }
            this.litArticleAddedDate = (FormatedTimeLabel) this.FindControl("litArticleAddedDate");
            this.litArticleContent = (Literal) this.FindControl("litArticleContent");
            this.litArticleDescription = (Literal) this.FindControl("litArticleDescription");
            this.litArticleTitle = (Literal) this.FindControl("litArticleTitle");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            this.ariticlative = (Common_ArticleRelative) this.FindControl("list_Common_ArticleRelative");
            if (!this.Page.IsPostBack)
            {
                ArticleInfo article = CommentBrowser.GetArticle(this.articleId);
                if ((article != null) && article.IsRelease)
                {
                    PageTitle.AddSiteNameTitle(article.Title, HiContext.Current.Context);
                    if (!string.IsNullOrEmpty(article.MetaKeywords))
                    {
                        MetaTags.AddMetaKeywords(article.MetaKeywords, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(article.MetaDescription))
                    {
                        MetaTags.AddMetaDescription(article.MetaDescription, HiContext.Current.Context);
                    }
                    this.litArticleTitle.Text = article.Title;
                    this.litArticleDescription.Text = article.Description;
                    string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { this.articleId });
                    this.litArticleContent.Text = article.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
                    this.litArticleAddedDate.Time = article.AddedDate;
                    ArticleInfo info2 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Front", article.CategoryId);
                    if ((info2 != null) && (info2.ArticleId > 0))
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { info2.ArticleId });
                            this.lblFrontTitle.Text = info2.Title;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    ArticleInfo info3 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Next", article.CategoryId);
                    if ((info3 != null) && (info3.ArticleId > 0))
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { info3.ArticleId });
                            this.lblNextTitle.Text = info3.Title;
                        }
                    }
                    else if (this.lblNext != null)
                    {
                        this.lblNext.Visible = false;
                    }
                    this.ariticlative.DataSource = CommentBrowser.GetArticlProductList(this.articleId);
                    this.ariticlative.DataBind();
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ArticleDetails.html";
            }
            base.OnInit(e);
        }
    }
}

