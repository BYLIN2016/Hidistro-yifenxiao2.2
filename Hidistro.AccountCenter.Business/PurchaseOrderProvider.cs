namespace Hidistro.AccountCenter.Business
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Data.Common;

    public abstract class PurchaseOrderProvider
    {
        protected PurchaseOrderProvider()
        {
        }

        public static PurchaseOrderProvider CreateInstance()
        {
            return (PurchaseOrderProvider) DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.PurchaseOrderData,Hidistro.AccountCenter.DistributionData");
        }

        public abstract bool CreatePurchaseOrder(OrderInfo order, DbTransaction dbTran);
    }
}

