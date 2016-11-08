namespace Hidistro.Entities.Promotions
{
    using System;

    public enum PromoteType
    {
        Amount = 2,
        Discount = 1,
        FullAmountDiscount = 11,
        FullAmountReduced = 12,
        FullAmountSentFreight = 0x11,
        FullAmountSentGift = 15,
        FullAmountSentTimesPoint = 0x10,
        FullQuantityDiscount = 13,
        FullQuantityReduced = 14,
        NotSet = 0,
        QuantityDiscount = 4,
        Reduced = 3,
        SentGift = 5,
        SentProduct = 6
    }
}

