namespace Hidistro.Entities.Sales
{
    using System;

    public enum PurchaseOrderActions
    {
        DISTRIBUTOR_CLOSE = 1,
        DISTRIBUTOR_CONFIRM_GOODS = 4,
        DISTRIBUTOR_CONFIRM_PAY = 3,
        DISTRIBUTOR_MODIFY_GIFTS = 2,
        MASTER__CLOSE = 5,
        MASTER__MODIFY_AMOUNT = 6,
        MASTER__MODIFY_SHIPPING_MODE = 7,
        MASTER_CONFIRM_PAY = 12,
        MASTER_FINISH_TRADE = 11,
        MASTER_MODIFY_DELIVER_ADDRESS = 8,
        MASTER_REJECT_REFUND = 10,
        MASTER_SEND_GOODS = 9
    }
}

