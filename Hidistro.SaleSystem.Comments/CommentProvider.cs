namespace Hidistro.SaleSystem.Comments
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public abstract class CommentProvider
    {
        protected CommentProvider()
        {
        }

        public abstract AfficheInfo GetAffiche(int afficheId);
        public abstract List<AfficheInfo> GetAfficheList();
        public abstract DataSet GetAllHotKeywords();
        public abstract ArticleInfo GetArticle(int articleId);
        public abstract ArticleCategoryInfo GetArticleCategory(int categoryId);
        public abstract DbQueryResult GetArticleList(ArticleQuery articleQuery);
        public abstract IList<ArticleInfo> GetArticleList(int categoryId, int maxNum);
        public abstract IList<ArticleCategoryInfo> GetArticleMainCategories();
        public abstract DataTable GetArticlProductList(int articlId);
        public abstract IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? num);
        public abstract AfficheInfo GetFrontOrNextAffiche(int afficheId, string type);
        public abstract ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId);
        public abstract HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type);
        public abstract PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type);
        public abstract HelpInfo GetHelp(int helpId);
        public abstract HelpCategoryInfo GetHelpCategory(int categoryId);
        public abstract IList<HelpCategoryInfo> GetHelpCategorys();
        public abstract DbQueryResult GetHelpList(HelpQuery helpQuery);
        public abstract DataSet GetHelps();
        public abstract DataSet GetHelpTitleList();
        public abstract DataTable GetHotKeywords(int categoryId, int hotKeywordsNum);
        public abstract DbQueryResult GetLeaveComments(LeaveCommentQuery query);
        public abstract PromotionInfo GetPromote(int activityId);
        public abstract DataTable GetPromotes(Pagination pagination, int promotiontype, out int totalPromotes);
        public abstract VoteInfo GetVoteById(long voteId);
        public abstract DataSet GetVoteByIsShow();
        public abstract VoteItemInfo GetVoteItemById(long voteItemId);
        public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
        public abstract bool InsertLeaveComment(LeaveCommentInfo leave);
        public static CommentProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return CommentSubsiteProvider.CreateInstance();
            }
            return CommentMasterProvider.CreateInstance();
        }

        public abstract int Vote(long voteItemId);
    }
}

