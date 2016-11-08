namespace Hidistro.AccountCenter.Business
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public abstract class TradeProvider
    {
        protected TradeProvider()
        {
        }

        public abstract int AddClaimCodeToUser(string claimCode, int userId);
        public abstract bool AddMemberPoint(UserPointInfo point);
        public abstract bool ChangeMemberGrade(int userId, int gradId, int points);
        public abstract bool CloseOrder(string orderId);
        public abstract bool ConfirmOrderFinish(OrderInfo order);
        public abstract CountDownInfo CountDownBuy(int CountDownId);
        public abstract bool ExitCouponClaimCode(string claimCode);
        public abstract DataTable GetChangeCoupons();
        public abstract GroupBuyInfo GetGroupBuy(int groupBuyId);
        public abstract int GetHistoryPoint(int userId);
        public abstract int GetOrderCount(int groupBuyId);
        public abstract OrderInfo GetOrderInfo(string orderId);
        public abstract PaymentModeInfo GetPaymentMode(int modeId);
        public abstract IList<PaymentModeInfo> GetPaymentModes();
        public abstract DataTable GetUserCoupons(int userId);
        public abstract DbQueryResult GetUserOrder(int userId, OrderQuery query);
        public abstract DbQueryResult GetUserPoints(int pageIndex);
        public static TradeProvider Instance()
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                return TradeSubsiteProvider.CreateInstance();
            }
            return TradeMasterProvider.CreateInstance();
        }

        public abstract bool SendClaimCodes(CouponItemInfo couponItem);
        public abstract bool SetGroupBuyEndUntreated(int groupBuyId);
        public abstract bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems);
        public abstract void UpdateStockPayOrder(string orderId);
        public abstract bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId);
        public abstract bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, DbTransaction dbTran);
    }
}

