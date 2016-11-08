namespace Hidistro.AccountCenter.Comments
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class CommentsHelper
    {
        public static bool AddProductToFavorite(int productId)
        {
            return CommentDataProvider.Instance().AddProductToFavorite(productId);
        }

        public static int DeleteFavorite(int favoriteId)
        {
            return CommentDataProvider.Instance().DeleteFavorite(favoriteId);
        }

        public static bool DeleteFavorites(string ids)
        {
            return CommentDataProvider.Instance().DeleteFavorites(ids);
        }

        public static int DeleteMemberMessages(IList<long> messageList)
        {
            return CommentDataProvider.Instance().DeleteMemberMessages(messageList);
        }

        public static bool ExistsProduct(int productId)
        {
            return CommentDataProvider.Instance().ExistsProduct(productId);
        }

        public static DbQueryResult GetBatchBuyProducts(ProductQuery query)
        {
            return CommentDataProvider.Instance().GetBatchBuyProducts(query);
        }

        public static DbQueryResult GetFavorites(string tags, Pagination page)
        {
            return CommentDataProvider.Instance().GetFavorites(HiContext.Current.User.UserId, tags, page);
        }

        public static MessageBoxInfo GetMemberMessage(long messageId)
        {
            return CommentDataProvider.Instance().GetMemberMessage(messageId);
        }

        public static DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
        {
            return CommentDataProvider.Instance().GetMemberReceivedMessages(query);
        }

        public static DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
        {
            return CommentDataProvider.Instance().GetMemberSendedMessages(query);
        }

        public static DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
        {
            return CommentDataProvider.Instance().GetProductConsultationsAndReplys(query, out total);
        }

        public static ProductInfo GetProductDetails(int productId)
        {
            return CommentDataProvider.Instance().GetProductDetails(productId);
        }

        public static DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
        {
            return CommentDataProvider.Instance().GetUserProductReviewsAndReplys(query, out total);
        }

        public static int GetUserProductReviewsCount()
        {
            return CommentDataProvider.Instance().GetUserProductReviewsCount();
        }

        public static bool PostMemberMessageIsRead(long messageId)
        {
            return CommentDataProvider.Instance().PostMemberMessageIsRead(messageId);
        }

        public static bool SendMessage(MessageBoxInfo messageBoxInfo)
        {
            return CommentDataProvider.Instance().InsertMessage(messageBoxInfo);
        }

        public static int UpdateFavorite(int favoriteId, string tags, string remark)
        {
            return CommentDataProvider.Instance().UpdateFavorite(favoriteId, tags, remark);
        }
    }
}

