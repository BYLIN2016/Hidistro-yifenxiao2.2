namespace Hidistro.SaleSystem.Catalog
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;

    public static class ProductBrowser
    {
        public static DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetBrowseProductList(query);
        }

        public static BundlingInfo GetBundlingInfo(int Bundlingid)
        {
            return ProductProvider.Instance().GetBundlingInfo(Bundlingid);
        }

        public static List<BundlingItemInfo> GetBundlingItemsByID(int BundlingID)
        {
            return ProductProvider.Instance().GetBundlingItemsByID(BundlingID);
        }

        public static DbQueryResult GetBundlingProductList(BundlingInfoQuery query)
        {
            return ProductProvider.Instance().GetBundlingProductList(query);
        }

        public static DataTable GetCounDownProducList(int maxnum)
        {
            return ProductProvider.Instance().GetCounDownProducList(maxnum);
        }

        public static CountDownInfo GetCountDownInfo(int productId)
        {
            return ProductProvider.Instance().GetCountDownInfo(productId);
        }

        public static CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
        {
            return ProductProvider.Instance().GetCountDownInfoByCountDownId(countDownId);
        }

        public static DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetCountDownProductList(query);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return ProductProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static GiftInfo GetGift(int giftId)
        {
            return ProductProvider.Instance().GetGift(giftId);
        }

        public static DataTable GetGroupByProductList(int maxnum)
        {
            return ProductProvider.Instance().GetGroupByProductList(maxnum);
        }

        public static DataSet GetGroupByProductList(ProductBrowseQuery query, out int count)
        {
            return ProductProvider.Instance().GetGroupByProductList(query, out count);
        }

        public static int GetLineItemNumber(int productId)
        {
            return ProductProvider.Instance().GetLineItemNumber(productId);
        }

        public static DbQueryResult GetLineItems(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetLineItems(page, productId);
        }

        public static DataTable GetLineItems(int productId, int maxNum)
        {
            return ProductProvider.Instance().GetLineItems(productId, maxNum);
        }

        public static DbQueryResult GetOnlineGifts(Pagination page)
        {
            return ProductProvider.Instance().GetOnlineGifts(page);
        }

        public static IList<GiftInfo> GetOnlinePromotionGifts()
        {
            return ProductProvider.Instance().GetOnlinePromotionGifts();
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return ProductProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
        {
            return ProductProvider.Instance().GetProductBrowseInfo(productId, maxReviewNum, maxConsultationNum);
        }

        public static int GetProductConsultationNumber(int productId)
        {
            return ProductProvider.Instance().GetProductConsultationNumber(productId);
        }

        public static DbQueryResult GetProductConsultations(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetProductConsultations(page, productId);
        }

        public static GroupBuyInfo GetProductGroupBuyInfo(int productId)
        {
            return ProductProvider.Instance().GetProductGroupBuyInfo(productId);
        }

        public static int GetProductReviewNumber(int productId)
        {
            return ProductProvider.Instance().GetProductReviewNumber(productId);
        }

        public static DataTable GetProductReviews(int maxNum)
        {
            return ProductProvider.Instance().GetProductReviews(maxNum);
        }

        public static DbQueryResult GetProductReviews(Pagination page, int productId)
        {
            return ProductProvider.Instance().GetProductReviews(page, productId);
        }

        public static ProductInfo GetProductSimpleInfo(int productId)
        {
            return ProductProvider.Instance().GetProductSimpleInfo(productId);
        }

        public static DataTable GetSaleProductRanking(int? categoryId, int maxNum)
        {
            return ProductProvider.Instance().GetSaleProductRanking(categoryId, maxNum);
        }

        public static DataTable GetSubjectList(SubjectListQuery query)
        {
            return ProductProvider.Instance().GetSubjectList(query);
        }

        public static DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
        {
            return ProductProvider.Instance().GetUnSaleProductList(query);
        }

        public static DataTable GetVistiedProducts(IList<int> productIds)
        {
            return ProductProvider.Instance().GetVistiedProducts(productIds);
        }

        public static bool IsBuyProduct(int productId)
        {
            return ProductProvider.Instance().IsBuyProduct(productId);
        }

        public static void LoadProductReview(int productId, out int buyNum, out int reviewNum)
        {
            ProductProvider.Instance().LoadProductReview(productId, out buyNum, out reviewNum);
        }
    }
}

