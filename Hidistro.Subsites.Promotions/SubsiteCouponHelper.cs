namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class SubsiteCouponHelper
    {
        public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
        {
            Globals.EntityCoding(coupon, true);
            return SubsitePromotionsProvider.Instance().CreateCoupon(coupon, count, out lotNumber);
        }

        public static bool DeleteCoupon(int couponId)
        {
            return SubsitePromotionsProvider.Instance().DeleteCoupon(couponId);
        }

        public static CouponInfo GetCoupon(int couponId)
        {
            return SubsitePromotionsProvider.Instance().GetCouponDetails(couponId);
        }

        public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
        {
            return SubsitePromotionsProvider.Instance().GetCouponItemInfos(lotNumber);
        }

        public static DbQueryResult GetNewCoupons(Pagination page)
        {
            return SubsitePromotionsProvider.Instance().GetNewCoupons(page);
        }

        public static List<int> GetUderlingIds(int? gradeId, string userName)
        {
            return SubsitePromotionsProvider.Instance().GetUderlingIds(gradeId, userName);
        }

        public static void SendClaimCodes(int couponId, IList<CouponItemInfo> listCouponItem)
        {
            foreach (CouponItemInfo info in listCouponItem)
            {
                SubsitePromotionsProvider.Instance().SendClaimCodes(couponId, info);
            }
        }

        public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
        {
            Globals.EntityCoding(coupon, true);
            return SubsitePromotionsProvider.Instance().UpdateCoupon(coupon);
        }
    }
}

