namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderQuery : Pagination
    {
        
        private string _DistributorName;
        
        private DateTime? _EndDate;
        
        private bool _IsManualPurchaseOrder;
        
        private int? _IsPrinted;
        
        private string _OrderId;
        
        private string _ProductName;
        
        private string _PurchaseOrderId;
        
        private OrderStatus _PurchaseStatus;
        
        private int? _ShippingModeId;
        
        private string _ShipTo;
        
        private DateTime? _StartDate;

        public string DistributorName
        {
            
            get
            {
                return _DistributorName;
            }
            
            set
            {
                _DistributorName = value;
            }
        }

        public DateTime? EndDate
        {
            
            get
            {
                return _EndDate;
            }
            
            set
            {
                _EndDate = value;
            }
        }

        public bool IsManualPurchaseOrder
        {
            
            get
            {
                return _IsManualPurchaseOrder;
            }
            
            set
            {
                _IsManualPurchaseOrder = value;
            }
        }

        public int? IsPrinted
        {
            
            get
            {
                return _IsPrinted;
            }
            
            set
            {
                _IsPrinted = value;
            }
        }

        public string OrderId
        {
            
            get
            {
                return _OrderId;
            }
            
            set
            {
                _OrderId = value;
            }
        }

        public string ProductName
        {
            
            get
            {
                return _ProductName;
            }
            
            set
            {
                _ProductName = value;
            }
        }

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

        public OrderStatus PurchaseStatus
        {
            
            get
            {
                return _PurchaseStatus;
            }
            
            set
            {
                _PurchaseStatus = value;
            }
        }

        public int? ShippingModeId
        {
            
            get
            {
                return _ShippingModeId;
            }
            
            set
            {
                _ShippingModeId = value;
            }
        }

        public string ShipTo
        {
            
            get
            {
                return _ShipTo;
            }
            
            set
            {
                _ShipTo = value;
            }
        }

        public DateTime? StartDate
        {
            
            get
            {
                return _StartDate;
            }
            
            set
            {
                _StartDate = value;
            }
        }
    }
}

