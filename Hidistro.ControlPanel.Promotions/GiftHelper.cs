namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using System;

    public static class GiftHelper
    {
        public static GiftActionStatus AddGift(GiftInfo gift)
        {
            Globals.EntityCoding(gift, true);
            return PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Create);
        }

        public static bool DeleteGift(int giftId)
        {
            GiftInfo info2 = new GiftInfo();
            info2.GiftId = giftId;
            GiftInfo gift = info2;
            return (PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Delete) == GiftActionStatus.Success);
        }

        public static GiftInfo GetGiftDetails(int giftId)
        {
            return PromotionsProvider.Instance().GetGiftDetails(giftId);
        }

        public static DbQueryResult GetGifts(GiftQuery query)
        {
            return PromotionsProvider.Instance().GetGifts(query);
        }

        public static GiftActionStatus UpdateGift(GiftInfo gift)
        {
            Globals.EntityCoding(gift, true);
            return PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Update);
        }

        public static bool UpdateIsDownLoad(int giftId, bool isdownload)
        {
            return PromotionsProvider.Instance().UpdateIsDownLoad(giftId, isdownload);
        }
    }
}

