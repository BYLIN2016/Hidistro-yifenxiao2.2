namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Articles : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptArticles;

        protected override void AttachChildControls()
        {
            this.rptArticles = (ThemedTemplatedRepeater) this.FindControl("rptArticles");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["CategoryId"], out result);
                    ArticleCategoryInfo articleCategory = CommentBrowser.GetArticleCategory(result);
                    if (articleCategory != null)
                    {
                        PageTitle.AddSiteNameTitle(articleCategory.Name, HiContext.Current.Context);
                    }
                }
                else
                {
                    PageTitle.AddSiteNameTitle("文章中心", HiContext.Current.Context);
                }
                this.BindList();
            }
        }

        private void BindList()
        {
            ArticleQuery articleQuery = new ArticleQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    articleQuery.CategoryId = new int?(result);
                }
            }
            articleQuery.PageIndex = this.pager.PageIndex;
            articleQuery.PageSize = this.pager.PageSize;
            articleQuery.SortBy = "AddedDate";
            articleQuery.SortOrder = SortAction.Desc;
            DbQueryResult articleList = CommentBrowser.GetArticleList(articleQuery);
            this.rptArticles.DataSource = articleList.Data;
            this.rptArticles.DataBind();
            this.pager.TotalRecords = articleList.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Articles.html";
            }
            base.OnInit(e);
        }
    }
}

