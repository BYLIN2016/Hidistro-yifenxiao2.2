namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartItemInfo
    {
        
        private decimal _AdjustedPrice;
        
        private bool _IsSendGift;
        
        private decimal _MemberPrice;
        
        private string _Name;
        
        private int _ProductId;
        
        private int _PromotionId;
        
        private string _PromotionName;
        
        private int _Quantity;
        
        private int _ShippQuantity;
        
        private string _SKU;
        
        private string _SkuContent;
        
        private string _SkuId;
        
        private string _ThumbnailUrl100;
        
        private string _ThumbnailUrl40;
        
        private string _ThumbnailUrl60;
        
        private int _UserId;
        
        private decimal _Weight;

        public decimal GetSubWeight()
        {
            return (this.Weight * this.Quantity);
        }

        public decimal AdjustedPrice
        {
            
            get
            {
                return _AdjustedPrice;
            }
            
            set
            {
                _AdjustedPrice = value;
            }
        }

        public bool IsSendGift
        {
            
            get
            {
                return _IsSendGift;
            }
            
            set
            {
                _IsSendGift = value;
            }
        }

        public decimal MemberPrice
        {
            
            get
            {
                return _MemberPrice;
            }
            
            set
            {
                _MemberPrice = value;
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

        public int ProductId
        {
            
            get
            {
                return _ProductId;
            }
            
            set
            {
                _ProductId = value;
            }
        }

        public int PromotionId
        {
            
            get
            {
                return _PromotionId;
            }
            
            set
            {
                _PromotionId = value;
            }
        }

        public string PromotionName
        {
            
            get
            {
                return _PromotionName;
            }
            
            set
            {
                _PromotionName = value;
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

        public int ShippQuantity
        {
            
            get
            {
                return _ShippQuantity;
            }
            
            set
            {
                _ShippQuantity = value;
            }
        }

        public string SKU
        {
            
            get
            {
                return _SKU;
            }
            
            set
            {
                _SKU = value;
            }
        }

        public string SkuContent
        {
            
            get
            {
                return _SkuContent;
            }
            
            set
            {
                _SkuContent = value;
            }
        }

        public string SkuId
        {
            
            get
            {
                return _SkuId;
            }
            
            set
            {
                _SkuId = value;
            }
        }

        public decimal SubTotal
        {
            get
            {
                return (this.AdjustedPrice * this.Quantity);
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

        public decimal Weight
        {
            
            get
            {
                return _Weight;
            }
            
            set
            {
                _Weight = value;
            }
        }
    }
}

