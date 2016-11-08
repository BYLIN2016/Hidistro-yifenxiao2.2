namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SKUItem : IComparable
    {
        
        private int _AlertStock;
        
        private decimal _CostPrice;
        
        private int _ProductId;
        
        private decimal _PurchasePrice;
        
        private decimal _SalePrice;
        
        private string _SKU;
        
        private string _SkuId;
        
        private int _Stock;
        
        private decimal _Weight;
        private Dictionary<int, decimal> distributorPrices;
        private Dictionary<int, decimal> memberPrices;
        private Dictionary<int, int> skuItems;

        public int CompareTo(object obj)
        {
            SKUItem item = obj as SKUItem;
            if (item == null)
            {
                return -1;
            }
            if (item.SkuItems.Count != this.SkuItems.Count)
            {
                return -1;
            }
            foreach (int num in item.SkuItems.Keys)
            {
                if (item.SkuItems[num] != this.SkuItems[num])
                {
                    return -1;
                }
            }
            return 0;
        }

        public int AlertStock
        {
            
            get
            {
                return _AlertStock;
            }
            
            set
            {
                _AlertStock = value;
            }
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

        public Dictionary<int, decimal> DistributorPrices
        {
            get
            {
                return (this.distributorPrices ?? (this.distributorPrices = new Dictionary<int, decimal>()));
            }
        }

        public Dictionary<int, decimal> MemberPrices
        {
            get
            {
                return (this.memberPrices ?? (this.memberPrices = new Dictionary<int, decimal>()));
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

        public decimal SalePrice
        {
            
            get
            {
                return _SalePrice;
            }
            
            set
            {
                _SalePrice = value;
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

        public Dictionary<int, int> SkuItems
        {
            get
            {
                return (this.skuItems ?? (this.skuItems = new Dictionary<int, int>()));
            }
        }

        public int Stock
        {
            
            get
            {
                return _Stock;
            }
            
            set
            {
                _Stock = value;
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

