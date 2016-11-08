namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderGiftInfo
    {
        
        private decimal _CostPrice;
        
        private int _GiftId;
        
        private string _GiftName;
        
        private string _OrderId;
        
        private int _PromoteType;
        
        private int _Quantity;
        
        private string _ThumbnailsUrl;

        public decimal CostPrice
        {
            
            get
            {
                return _CostPrice;
            }
            
            set
            {
                _CostPrice = value;
            }
        }

        public int GiftId
        {
            
            get
            {
                return _GiftId;
            }
            
            set
            {
                _GiftId = value;
            }
        }

        public string GiftName
        {
            
            get
            {
                return _GiftName;
            }
            
            set
            {
                _GiftName = value;
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

        public int PromoteType
        {
            
            get
            {
                return _PromoteType;
            }
            
            set
            {
                _PromoteType = value;
            }
        }

        public int Quantity
        {
            
            get
            {
                return _Quantity;
            }
            
            set
            {
                _Quantity = value;
            }
        }

        public string ThumbnailsUrl
        {
            
            get
            {
                return _ThumbnailsUrl;
            }
            
            set
            {
                _ThumbnailsUrl = value;
            }
        }
    }
}

