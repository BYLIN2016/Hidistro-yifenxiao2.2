namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseShoppingCartItemInfo
    {
        
        private decimal _CostPrice;
        
        private string _ItemDescription;
        
        private decimal _ItemListPrice;
        
        private decimal _ItemPurchasePrice;
        
        private decimal _ItemWeight;
        
        private int _ProductId;
        
        private int _Quantity;
        
        private string _SKU;
        
        private string _SKUContent;
        
        private string _SkuId;
        
        private string _ThumbnailsUrl;

        public decimal GetSubTotal()
        {
            return (this.ItemPurchasePrice * this.Quantity);
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

        public decimal ItemPurchasePrice
        {
            
            get
            {
                return _ItemPurchasePrice;
            }
            
            set
            {
                _ItemPurchasePrice = value;
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

