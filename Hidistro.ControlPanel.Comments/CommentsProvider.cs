namespace Hidistro.ControlPanel.Comments
{
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using Hidistro.Membership.Context;
    using Hidistro.Core;

    public abstract class CommentsProvider
    {
        private static readonly CommentsProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.CommentData,Hidistro.ControlPanel.Data") as CommentsProvider);

        protected CommentsProvider()
        {
        }

        public abstract bool AddAffiche(AfficheInfo affiche);
        public abstract bool AddArticle(ArticleInfo article);
        public abstract bool AddHelp(HelpInfo help);
        public abstract bool AddMessage(MessageBoxInfo messageBoxInfo, UserRole toRole);
        public abstract bool AddReleatesProdcutByArticId(int articId, int prodcutId);
        public abstract bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action);
        public abstract bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action);
        public abstract bool DeleteAffiche(int afficheId);
        public abstract int DeleteAffiches(List<int> afficheIds);
        public abstract bool DeleteArticle(int articleId);
        public abstract int DeleteArticles(IList<int> articles);
        public abstract bool DeleteHelp(int helpId);
        public abstract int DeleteHelpCategorys(List<int> categoryIds);
        public abstract int DeleteHelps(IList<int> helps);
        public abstract bool DeleteLeaveComment(long leaveId);
        public abstract bool DeleteLeaveCommentReply(long leaveReplyId);
        public abstract int DeleteLeaveComments(IList<long> leaveIds);
        public abstract int DeleteManagerMessages(IList<long> messageList);
        public abstract int DeleteProductConsultation(int consultationId);
        public abstract int DeleteProductReview(long reviewId);
        public abstract int DeleteReview(IList<int> reviews);
        public abstract AfficheInfo GetAffiche(int afficheId);
        public abstract List<AfficheInfo> GetAfficheList();
        public abstract ArticleInfo GetArticle(int articleId);
        public abstract ArticleCategoryInfo GetArticleCategory(int categoryId);
        public abstract DbQueryResult GetArticleList(ArticleQuery articleQuery);
        public abstract DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery);
        public abstract IList<Distributor> GetDistributorsByRank(int? gradeId);
        public abstract HelpInfo GetHelp(int helpId);
        public abstract HelpCategoryInfo GetHelpCategory(int categoryId);
        public abstract IList<HelpCategoryInfo> GetHelpCategorys();
        public abstract DbQueryResult GetHelpList(HelpQuery helpQuery);
        public abstract LeaveCommentInfo GetLeaveComment(long leaveId);
        public abstract DbQueryResult GetLeaveComments(LeaveCommentQuery query);
        public abstract IList<ArticleCategoryInfo> GetMainArticleCategories();
        public abstract MessageBoxInfo GetManagerMessage(long messageId);
        public abstract DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role);
        public abstract DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role);
        public abstract IList<Member> GetMembersByRank(int? gradeId);
        public abstract int GetMemberUnReadMessageNum();
        public abstract ProductConsultationInfo GetProductConsultation(int consultationId);
        public abstract ProductReviewInfo GetProductReview(int reviewId);
        public abstract DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery);
        public abstract DbQueryResult GetRelatedArticsProducts(Pagination page, int articId);
        public abstract DataTable GetReplyLeaveComments(long leaveId);
        public abstract bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole);
        public static CommentsProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool PostManagerMessageIsRead(long messageId);
        public abstract bool RemoveReleatesProductByArticId(int articId);
        public abstract bool RemoveReleatesProductByArticId(int articId, int productId);
        public abstract int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply);
        public abstract bool ReplyProductConsultation(ProductConsultationInfo productConsultation);
        public abstract void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence);
        public abstract void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence);
        public abstract bool UpdateAffiche(AfficheInfo affiche);
        public abstract bool UpdateArticle(ArticleInfo article);
        public abstract bool UpdateHelp(HelpInfo help);
        public abstract bool UpdateRelease(int articId, bool release);
    }
}

