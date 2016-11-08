namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderGiftQuery : Pagination
    {
        
        private string _PurchaseOrderId;

        public string PurchaseOrderId
        {
            
            get
            {
                return _PurchaseOrderId;
            }
            
            set
            {
                _PurchaseOrderId = value;
            }
        }
    }
}

