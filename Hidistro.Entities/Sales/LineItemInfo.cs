namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class LineItemInfo
    {
        
        private decimal _ItemAdjustedPrice;
        
        private decimal _ItemCostPrice;
        
        private string _ItemDescription;
        
        private decimal _ItemListPrice;
        
        private decimal _ItemWeight;
        
        private int _ProductId;
        
        private int _PromotionId;
        
        private string _PromotionName;
        
        private int _Quantity;
        
        private int _ShipmentQuantity;
        
        private string _SKU;
        
        private string _SKUContent;
        
        private string _SkuId;
        
        private string _ThumbnailsUrl;

        public decimal GetSubTotal()
        {
            return (this.ItemAdjustedPrice * this.Quantity);
        }

        public decimal ItemAdjustedPrice
        {
            
            get
            {
                return _ItemAdjustedPrice;
            }
            
            set
            {
                _ItemAdjustedPrice = value;
            }
        }

        public decimal ItemCostPrice
        {
            
            get
            {
                return _ItemCostPrice;
            }
            
            set
            {
                _ItemCostPrice = value;
            }
        }

        public string ItemDescription
        {
            
            get
            {
                return _ItemDescription;
            }
            
            set
            {
                _ItemDescription = value;
            }
        }

        public decimal ItemListPrice
        {
            
            get
            {
                return _ItemListPrice;
            }
            
            set
            {
                _ItemListPrice = value;
            }
        }

        public decimal ItemWeight
        {
            
            get
            {
                return _ItemWeight;
            }
            
            set
            {
                _ItemWeight = value;
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

        public int ShipmentQuantity
        {
            
            get
            {
                return _ShipmentQuantity;
            }
            
            set
            {
                _ShipmentQuantity = value;
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

        public string SKUContent
        {
            
            get
            {
                return _SKUContent;
            }
            
            set
            {
                _SKUContent = value;
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

