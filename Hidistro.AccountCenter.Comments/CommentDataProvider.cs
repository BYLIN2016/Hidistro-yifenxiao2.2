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

    public abstract class CommentDataProvider
    {
        protected CommentDataProvider()
        {
        }

        public abstract bool AddProductToFavorite(int productId);
        public abstract int DeleteFavorite(int favoriteId);
        public abstract bool DeleteFavorites(string ids);
        public abstract int DeleteMemberMessages(IList<long> messageList);
        public abstract bool ExistsProduct(int productId);
        public abstract DbQueryResult GetBatchBuyProducts(ProductQuery query);
        public abstract DbQueryResult GetFavorites(int userId, string tags, Pagination page);
        public abstract MessageBoxInfo GetMemberMessage(long messageId);
        public abstract DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query);
        public abstract DbQueryResult GetMemberSendedMessages(MessageBoxQuery query);
        public abstract DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total);
        public abstract ProductInfo GetProductDetails(int productId);
        public abstract DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total);
        public abstract int GetUserProductReviewsCount();
        public abstract bool InsertMessage(MessageBoxInfo messageBoxInfos);
        public static CommentDataProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return CommentSubsiteDataProvider.CreateInstance();
            }
            return CommentMasterDataProvider.CreateInstance();
        }

        public abstract bool PostMemberMessageIsRead(long messageId);
        public abstract int UpdateFavorite(int favoriteId, string tags, string remark);
    }
}

