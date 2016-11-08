namespace Hidistro.Subsites.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class SubsiteCommentsHelper
    {
        public static bool CreateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return SubsiteCommentsProvider.Instance().AddAffiche(affiche);
        }

        public static bool CreateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return SubsiteCommentsProvider.Instance().AddArticle(article);
        }

        public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
        }

        public static bool CreateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return SubsiteCommentsProvider.Instance().AddHelp(help);
        }

        public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
        }

        public static bool DeleteAffiche(int afficheId)
        {
            return SubsiteCommentsProvider.Instance().DeleteAffiche(afficheId);
        }

        public static int DeleteAffiches(List<int> affiches)
        {
            if ((affiches == null) || (affiches.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteAffiches(affiches);
        }

        public static bool DeleteArticle(int articleId)
        {
            return SubsiteCommentsProvider.Instance().DeleteArticle(articleId);
        }

        public static bool DeleteArticleCategory(int categoryId)
        {
            ArticleCategoryInfo articleCategory = new ArticleCategoryInfo();
            articleCategory.CategoryId = categoryId;
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Delete);
        }

        public static int DeleteArticles(IList<int> articles)
        {
            if ((articles == null) || (articles.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteArticles(articles);
        }

        public static bool DeleteHelp(int helpId)
        {
            return SubsiteCommentsProvider.Instance().DeleteHelp(helpId);
        }

        public static bool DeleteHelpCategory(int categoryId)
        {
            HelpCategoryInfo helpCategory = new HelpCategoryInfo();
            helpCategory.CategoryId = new int?(categoryId);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Delete);
        }

        public static int DeleteHelps(IList<int> helps)
        {
            if ((helps == null) || (helps.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteHelps(helps);
        }

        public static bool DeleteLeaveComment(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveComment(leaveId);
        }

        public static bool DeleteLeaveCommentReply(long leaveReplyId)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveCommentReply(leaveReplyId);
        }

        public static int DeleteLeaveComments(IList<long> leaveIds)
        {
            return SubsiteCommentsProvider.Instance().DeleteLeaveComments(leaveIds);
        }

        public static int DeleteMessages(IList<long> messageList)
        {
            return SubsiteCommentsProvider.Instance().DeleteMessages(messageList);
        }

        public static int DeleteProductConsultation(int consultationId)
        {
            return SubsiteCommentsProvider.Instance().DeleteProductConsultation(consultationId);
        }

        public static int DeleteProductReview(long reviewId)
        {
            return SubsiteCommentsProvider.Instance().DeleteProductReview(reviewId);
        }

        public static int DeleteReview(IList<int> reviews)
        {
            if ((reviews == null) || (reviews.Count == 0))
            {
                return 0;
            }
            return SubsiteCommentsProvider.Instance().DeleteReview(reviews);
        }

        public static AfficheInfo GetAffiche(int afficheId)
        {
            return SubsiteCommentsProvider.Instance().GetAffiche(afficheId);
        }

        public static List<AfficheInfo> GetAfficheList()
        {
            return SubsiteCommentsProvider.Instance().GetAfficheList();
        }

        public static ArticleInfo GetArticle(int articleId)
        {
            return SubsiteCommentsProvider.Instance().GetArticle(articleId);
        }

        public static ArticleCategoryInfo GetArticleCategory(int categoryId)
        {
            return SubsiteCommentsProvider.Instance().GetArticleCategory(categoryId);
        }

        public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
        {
            return SubsiteCommentsProvider.Instance().GetArticleList(articleQuery);
        }

        public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            return SubsiteCommentsProvider.Instance().GetConsultationProducts(consultationQuery);
        }

        public static HelpInfo GetHelp(int helpId)
        {
            return SubsiteCommentsProvider.Instance().GetHelp(helpId);
        }

        public static HelpCategoryInfo GetHelpCategory(int categoryId)
        {
            return SubsiteCommentsProvider.Instance().GetHelpCategory(categoryId);
        }

        public static IList<HelpCategoryInfo> GetHelpCategorys()
        {
            return SubsiteCommentsProvider.Instance().GetHelpCategorys();
        }

        public static DbQueryResult GetHelpList(HelpQuery helpQuery)
        {
            return SubsiteCommentsProvider.Instance().GetHelpList(helpQuery);
        }

        public static int GetIsReadMessageToAdmin()
        {
            return SubsiteCommentsProvider.Instance().GetIsReadMessageToAdmin();
        }

        public static LeaveCommentInfo GetLeaveComment(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().GetLeaveComment(leaveId);
        }

        public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
        {
            return SubsiteCommentsProvider.Instance().GetLeaveComments(query);
        }

        public static IList<ArticleCategoryInfo> GetMainArticleCategories()
        {
            return SubsiteCommentsProvider.Instance().GetMainArticleCategories();
        }

        public static MessageBoxInfo GetMessage(long messageId)
        {
            return SubsiteCommentsProvider.Instance().GetMessage(messageId);
        }

        public static ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            return SubsiteCommentsProvider.Instance().GetProductConsultation(consultationId);
        }

        public static ProductReviewInfo GetProductReview(int reviewId)
        {
            return SubsiteCommentsProvider.Instance().GetProductReview(reviewId);
        }

        public static DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
        {
            return SubsiteCommentsProvider.Instance().GetProductReviews(out total, reviewQuery);
        }

        public static DbQueryResult GetReceivedMessages(MessageBoxQuery query, UserRole role)
        {
            return SubsiteCommentsProvider.Instance().GetReceivedMessages(query, role);
        }

        public static DataTable GetReplyLeaveComments(long leaveId)
        {
            return SubsiteCommentsProvider.Instance().GetReplyLeaveComments(leaveId);
        }

        public static DbQueryResult GetSendedMessages(MessageBoxQuery query, UserRole role)
        {
            return SubsiteCommentsProvider.Instance().GetSendedMessages(query, role);
        }

        public static bool PostMessageIsRead(long messageId)
        {
            return SubsiteCommentsProvider.Instance().PostMessageIsRead(messageId);
        }

        public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
        {
            leaveReply.ReplyDate = DateTime.Now;
            return SubsiteCommentsProvider.Instance().ReplyLeaveComment(leaveReply);
        }

        public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            return SubsiteCommentsProvider.Instance().ReplyProductConsultation(productConsultation);
        }

        public static bool SendMessageToManager(MessageBoxInfo messageBoxInfo)
        {
            return SubsiteCommentsProvider.Instance().InsertMessage(messageBoxInfo, UserRole.SiteManager);
        }

        public static int SendMessageToMember(IList<MessageBoxInfo> messageBoxInfos)
        {
            int num = 0;
            foreach (MessageBoxInfo info in messageBoxInfos)
            {
                if (SubsiteCommentsProvider.Instance().InsertMessage(info, UserRole.Member))
                {
                    num++;
                }
            }
            return num;
        }

        public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteCommentsProvider.Instance().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
        {
            SubsiteCommentsProvider.Instance().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
        }

        public static bool UpdateAffiche(AfficheInfo affiche)
        {
            if (null == affiche)
            {
                return false;
            }
            Globals.EntityCoding(affiche, true);
            return SubsiteCommentsProvider.Instance().UpdateAffiche(affiche);
        }

        public static bool UpdateArticle(ArticleInfo article)
        {
            if (null == article)
            {
                return false;
            }
            Globals.EntityCoding(article, true);
            return SubsiteCommentsProvider.Instance().UpdateArticle(article);
        }

        public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
        {
            if (null == articleCategory)
            {
                return false;
            }
            Globals.EntityCoding(articleCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
        }

        public static bool UpdateHelp(HelpInfo help)
        {
            if (null == help)
            {
                return false;
            }
            Globals.EntityCoding(help, true);
            return SubsiteCommentsProvider.Instance().UpdateHelp(help);
        }

        public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
        {
            if (null == helpCategory)
            {
                return false;
            }
            Globals.EntityCoding(helpCategory, true);
            return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
        }

        public static bool UpdateMyArticRelease(int articlId, bool isrealse)
        {
            return SubsiteCommentsProvider.Instance().UpdateMyArticRelease(articlId, isrealse);
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

