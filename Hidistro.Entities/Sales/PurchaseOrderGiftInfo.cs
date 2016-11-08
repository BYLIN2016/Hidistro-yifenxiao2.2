namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderGiftInfo
    {
        
        private decimal _CostPrice;
        
        private int _GiftId;
        
        private string _GiftName;
        
        private string _PurchaseOrderId;
        
        private decimal _PurchasePrice;
        
        private int _Quantity;
        
        private string _ThumbnailsUrl;

        public decimal GetSubTotal()
        {
            return (this.PurchasePrice * this.Quantity);
        }

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

        public decimal PurchasePrice
        {
            
            get
            {
                return _PurchasePrice;
            }
            
            set
            {
                _PurchasePrice = value;
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

