namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public abstract class ProductProvider
    {
        protected ProductProvider()
        {
        }

        public abstract DbQueryResult GetBrowseProductList(ProductBrowseQuery query);
        public abstract BundlingInfo GetBundlingInfo(int bundlingid);
        public abstract List<BundlingItemInfo> GetBundlingItemsByID(int bundingid);
        public abstract DbQueryResult GetBundlingProductList(BundlingInfoQuery query);
        public abstract DataTable GetCounDownProducList(int maxnum);
        public abstract CountDownInfo GetCountDownInfo(int productId);
        public abstract CountDownInfo GetCountDownInfoByCountDownId(int countDownId);
        public abstract DbQueryResult GetCountDownProductList(ProductBrowseQuery query);
        public abstract decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity);
        public abstract GiftInfo GetGift(int giftId);
        public abstract DataTable GetGroupByProductList(int maxnum);
        public abstract DataSet GetGroupByProductList(ProductBrowseQuery query, out int count);
        public abstract int GetLineItemNumber(int productId);
        public abstract DbQueryResult GetLineItems(Pagination page, int productId);
        public abstract DataTable GetLineItems(int productId, int maxNum);
        public abstract DbQueryResult GetOnlineGifts(Pagination page);
        public abstract IList<GiftInfo> GetOnlinePromotionGifts();
        public abstract int GetOrderCount(int groupBuyId);
        public abstract ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum);
        public abstract int GetProductConsultationNumber(int productId);
        public abstract DbQueryResult GetProductConsultations(Pagination page, int productId);
        public abstract GroupBuyInfo GetProductGroupBuyInfo(int productId);
        public abstract int GetProductReviewNumber(int productId);
        public abstract DataTable GetProductReviews(int maxNum);
        public abstract DbQueryResult GetProductReviews(Pagination page, int productId);
        public abstract ProductInfo GetProductSimpleInfo(int productId);
        public abstract DataTable GetSaleProductRanking(int? categoryId, int maxNum);
        public abstract DataTable GetSubjectList(SubjectListQuery query);
        public abstract DbQueryResult GetUnSaleProductList(ProductBrowseQuery query);
        public abstract DataTable GetVistiedProducts(IList<int> productIds);
        public abstract bool InsertProductConsultation(ProductConsultationInfo productConsultation);
        public abstract bool InsertProductReview(ProductReviewInfo review);
        public static ProductProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return ProductSubsiteProvider.CreateInstance();
            }
            return ProductMasterProvider.CreateInstance();
        }

        public abstract bool IsBuyProduct(int productId);
        public abstract void LoadProductReview(int productId, out int buyNum, out int reviewNum);
        public abstract bool ProductExists(int productId);
    }
}

