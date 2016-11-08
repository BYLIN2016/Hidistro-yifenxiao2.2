namespace Hidistro.SaleSystem.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Web.Caching;
    using System.Xml;

    public static class CommentBrowser
    {
        public static AfficheInfo GetAffiche(int afficheId)
        {
            return CommentProvider.Instance().GetAffiche(afficheId);
        }

        public static List<AfficheInfo> GetAfficheList()
        {
            return CommentProvider.Instance().GetAfficheList();
        }

        public static DataSet GetAllHotKeywords()
        {
            return CommentProvider.Instance().GetAllHotKeywords();
        }

        public static ArticleInfo GetArticle(int articleId)
        {
            return CommentProvider.Instance().GetArticle(articleId);
        }

        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return CommentProvider.Instance().GetArticleCategory(categoryId);
        }

        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return CommentProvider.Instance().GetArticleList(articleQuery);
        }

        public static IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
        {
            return CommentProvider.Instance().GetArticleList(categoryId, maxNum);
        }

        public static IList<ArticleCategoryInfo> GetArticleMainCategories()
        {
            return CommentProvider.Instance().GetArticleMainCategories();
        }

        public static XmlDocument GetArticleSubjectDocument()
        {
            string key = "ArticleSubjectFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("ArticleSubjectFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/ArticleSubjects.xml");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        public static DataTable GetArticlProductList(int arctid)
        {
            return CommentProvider.Instance().GetArticlProductList(arctid);
        }

        public static IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? num)
        {
            return CommentProvider.Instance().GetFriendlyLinksIsVisible(num);
        }

        public static AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
        {
            return CommentProvider.Instance().GetFrontOrNextAffiche(afficheId, type);
        }

        public static ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
        {
            return CommentProvider.Instance().GetFrontOrNextArticle(articleId, type, categoryId);
        }

        public static HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
        {
            return CommentProvider.Instance().GetFrontOrNextHelp(helpId, categoryId, type);
        }

        public static PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
        {
            return CommentProvider.Instance().GetFrontOrNextPromoteInfo(promote, type);
        }

        public static HelpInfo GetHelp(int helpId)
        {
            return CommentProvider.Instance().GetHelp(helpId);
        }

        public static HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            return CommentProvider.Instance().GetHelpCategory(categoryId);
        }

        public static IList<HelpCategoryInfo> GetHelpCategorys()
        {
            return CommentProvider.Instance().GetHelpCategorys();
        }

        public static DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            return CommentProvider.Instance().GetHelpList(helpQuery);
        }

        public static DataSet GetHelps()
        {
            return CommentProvider.Instance().GetHelps();
        }

        public static DataSet GetHelpTitleList()
        {
            return CommentProvider.Instance().GetHelpTitleList();
        }

        public static DataTable GetHotKeywords(int categoryId, int hotKeywordsNum)
        {
            return CommentProvider.Instance().GetHotKeywords(categoryId, hotKeywordsNum);
        }

        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return CommentProvider.Instance().GetLeaveComments(query);
        }

        public static PromotionInfo GetPromote(int activityId)
        {
            return CommentProvider.Instance().GetPromote(activityId);
        }

        public static DataTable GetPromotes(Pagination pagination, int promotiontype, out int totalPromotes)
        {
            return CommentProvider.Instance().GetPromotes(pagination, promotiontype, out totalPromotes);
        }

        public static VoteInfo GetVoteById(long voteId)
        {
            return CommentProvider.Instance().GetVoteById(voteId);
        }

        public static DataSet GetVoteByIsShow()
        {
            return CommentProvider.Instance().GetVoteByIsShow();
        }

        public static VoteItemInfo GetVoteItemById(long voteItemId)
        {
            return CommentProvider.Instance().GetVoteItemById(voteItemId);
        }

        public static IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            return CommentProvider.Instance().GetVoteItems(voteId);
        }

        public static int Vote(long voteItemId)
        {
            return CommentProvider.Instance().Vote(voteItemId);
        }
    }
}

