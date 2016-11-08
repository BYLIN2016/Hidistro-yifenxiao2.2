namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartGiftInfo
    {
        
        private decimal _CostPrice;
        
        private int _GiftId;
        
        private string _Name;
        
        private int _NeedPoint;
        
        private int _PromoType;
        
        private decimal _PurchasePrice;
        
        private int _Quantity;
        
        private string _ThumbnailUrl100;
        
        private string _ThumbnailUrl40;
        
        private string _ThumbnailUrl60;
        
        private int _UserId;

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

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }

        public int NeedPoint
        {
            
            get
            {
                return _NeedPoint;
            }
            
            set
            {
                _NeedPoint = value;
            }
        }

        public int PromoType
        {
            
            get
            {
                return _PromoType;
            }
            
            set
            {
                _PromoType = value;
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

        public int SubPointTotal
        {
            get
            {
                if (this.PromoType <= 0)
                {
                    return (this.NeedPoint * this.Quantity);
                }
                return 0;
            }
        }

        public string ThumbnailUrl100
        {
            
            get
            {
                return _ThumbnailUrl100;
            }
            
            set
            {
                _ThumbnailUrl100 = value;
            }
        }

        public string ThumbnailUrl40
        {
            
            get
            {
                return _ThumbnailUrl40;
            }
            
            set
            {
                _ThumbnailUrl40 = value;
            }
        }

        public string ThumbnailUrl60
        {
            
            get
            {
                return _ThumbnailUrl60;
            }
            
            set
            {
                _ThumbnailUrl60 = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
            }
        }
    }
}

