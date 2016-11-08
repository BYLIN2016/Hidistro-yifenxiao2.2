namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderQuery : Pagination
    {
        
        private DateTime? _EndDate;
        
        private int? _GroupBuyId;
        
        private int? _IsPrinted;
        
        private string _OrderId;
        
        private int? _PaymentType;
        
        private string _ProductName;
        
        private int? _RegionId;
        
        private string _ShipId;
        
        private int? _ShippingModeId;
        
        private string _ShipTo;
        
        private DateTime? _StartDate;
        
        private OrderStatus _Status;
        
        private string _UserName;

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

        public int? GroupBuyId
        {
            
            get
            {
                return _GroupBuyId;
            }
            
            set
            {
                _GroupBuyId = value;
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

        public int? PaymentType
        {
            
            get
            {
                return _PaymentType;
            }
            
            set
            {
                _PaymentType = value;
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

        public int? RegionId
        {
            
            get
            {
                return _RegionId;
            }
            
            set
            {
                _RegionId = value;
            }
        }

        public string ShipId
        {
            
            get
            {
                return _ShipId;
            }
            
            set
            {
                _ShipId = value;
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

        public OrderStatus Status
        {
            
            get
            {
                return _Status;
            }
            
            set
            {
                _Status = value;
            }
        }

        public string UserName
        {
            
            get
            {
                return _UserName;
            }
            
            set
            {
                _UserName = value;
            }
        }
    }
}

