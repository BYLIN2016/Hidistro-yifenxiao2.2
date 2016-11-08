namespace Hidistro.ControlPanel.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;

    public static class ArticleHelper
    {
        public static bool AddReleatesProdcutByArticId(int articId, int productId)
        {
            return CommentsProvider.Instance().AddReleatesProdcutByArticId(articId, productId);
        }

        public static bool CreateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return CommentsProvider.Instance().AddArticle(article);
        }

        public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
        }

        public static bool CreateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return CommentsProvider.Instance().AddHelp(help);
        }

        public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
        }

        public static bool DeleteArticle(int articleId)
        {
            return CommentsProvider.Instance().DeleteArticle(articleId);
        }

        public static bool DeleteArticleCategory(int categoryId)
        {
            ArticleCategoryInfo articleCategory = new ArticleCategoryInfo();
            articleCategory.CategoryId = categoryId;
            return CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Delete);
        }

        public static int DeleteArticles(IList<int> articles)
        {
            if ((articles == null) || (articles.Count == 0))
            {
                return 0;
            }
            return CommentsProvider.Instance().DeleteArticles(articles);
        }

        public static bool DeleteHelp(int helpId)
        {
            return CommentsProvider.Instance().DeleteHelp(helpId);
        }

        public static bool DeleteHelpCategory(int categoryId)
        {
            HelpCategoryInfo helpCategory = new HelpCategoryInfo();
            helpCategory.CategoryId = new int?(categoryId);
            return CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Delete);
        }

        public static int DeleteHelpCategorys(List<int> categoryIds)
        {
            if ((categoryIds == null) || (categoryIds.Count == 0))
            {
                return 0;
            }
            return CommentsProvider.Instance().DeleteHelpCategorys(categoryIds);
        }

        public static int DeleteHelps(IList<int> helps)
        {
            if ((helps == null) || (helps.Count == 0))
            {
                return 0;
            }
            return CommentsProvider.Instance().DeleteHelps(helps);
        }

        public static ArticleInfo GetArticle(int articleId)
        {
            return CommentsProvider.Instance().GetArticle(articleId);
        }

        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return CommentsProvider.Instance().GetArticleCategory(categoryId);
        }

        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return CommentsProvider.Instance().GetArticleList(articleQuery);
        }

        public static HelpInfo GetHelp(int helpId)
        {
            return CommentsProvider.Instance().GetHelp(helpId);
        }

        public static HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            return CommentsProvider.Instance().GetHelpCategory(categoryId);
        }

        public static IList<HelpCategoryInfo> GetHelpCategorys()
        {
            return CommentsProvider.Instance().GetHelpCategorys();
        }

        public static DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            return CommentsProvider.Instance().GetHelpList(helpQuery);
        }

        public static IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            return CommentsProvider.Instance().GetMainArticleCategories();
        }

        public static DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
        {
            return CommentsProvider.Instance().GetRelatedArticsProducts(page, articId);
        }

        public static bool RemoveReleatesProductByArticId(int articId)
        {
            return CommentsProvider.Instance().RemoveReleatesProductByArticId(articId);
        }

        public static bool RemoveReleatesProductByArticId(int articId, int productId)
        {
            return CommentsProvider.Instance().RemoveReleatesProductByArticId(articId, productId);
        }

        public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            CommentsProvider.Instance().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            CommentsProvider.Instance().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return CommentsProvider.Instance().UpdateArticle(article);
        }

        public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
        }

        public static bool UpdateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return CommentsProvider.Instance().UpdateHelp(help);
        }

        public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
        }

        public static bool UpdateRelease(int articId, bool release)
        {
            return CommentsProvider.Instance().UpdateRelease(articId, release);
        }

        public static string UploadArticleImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }

        public static string UploadHelpImage(HttpPostedFile postedFile)
        {
            if (!ResourcesHelper.CheckPostedFile(postedFile))
            {
                return string.Empty;
            }
            string str = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
            postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + str));
            return str;
        }
    }
}

