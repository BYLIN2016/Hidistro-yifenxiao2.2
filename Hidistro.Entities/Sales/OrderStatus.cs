namespace Hidistro.Entities.Sales
{
    using System;

    public enum OrderStatus
    {
        All = 0,
        ApplyForRefund = 6,
        ApplyForReplacement = 8,
        ApplyForReturns = 7,
        BuyerAlreadyPaid = 2,
        Closed = 4,
        Finished = 5,
        History = 0x63,
        Refunded = 9,
        Returned = 10,
        SellerAlreadySent = 3,
        WaitBuyerPay = 1
    }
}

