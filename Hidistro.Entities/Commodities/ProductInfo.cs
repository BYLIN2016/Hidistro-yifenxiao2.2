namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ProductInfo
    {
        
        private DateTime _AddedDate;
        
        private int? _BrandId;
        
        private int _CategoryId;
        
        private string _Description;
        
        private int _DisplaySequence;
        
        private string _ExtendCategoryPath;
        
        private bool _HasSKU;
        
        private string _ImageUrl1;
        
        private string _ImageUrl2;
        
        private string _ImageUrl3;
        
        private string _ImageUrl4;
        
        private string _ImageUrl5;
        
        private int _LineId;
        
        private decimal _LowestSalePrice;
        
        private string _MainCategoryPath;
        
        private decimal? _MarketPrice;
        
        private string _MetaDescription;
        
        private string _MetaKeywords;
        
        private Hidistro.Entities.Commodities.PenetrationStatus _PenetrationStatus;
        
        private string _ProductCode;
        
        private int _ProductId;
        
        private string _ProductName;
        
        private int _SaleCounts;
        
        private ProductSaleStatus _SaleStatus;
        
        private string _ShortDescription;
        
        private int _ShowSaleCounts;
        
        private long _TaobaoProductId;
        
        private string _ThumbnailUrl100;
        
        private string _ThumbnailUrl160;
        
        private string _ThumbnailUrl180;
        
        private string _ThumbnailUrl220;
        
        private string _ThumbnailUrl310;
        
        private string _ThumbnailUrl40;
        
        private string _ThumbnailUrl410;
        
        private string _ThumbnailUrl60;
        
        private string _Title;
        
        private int? _TypeId;
        
        private string _Unit;
        
        private int _VistiCounts;
        private SKUItem defaultSku;
        private Dictionary<string, SKUItem> skus;

        public DateTime AddedDate
        {
            
            get
            {
                return _AddedDate;
            }
            
            set
            {
                _AddedDate = value;
            }
        }

        public int AlertStock
        {
            get
            {
                return this.DefaultSku.AlertStock;
            }
        }

        public int? BrandId
        {
            
            get
            {
                return _BrandId;
            }
            
            set
            {
                _BrandId = value;
            }
        }

        public int CategoryId
        {
            
            get
            {
                return _CategoryId;
            }
            
            set
            {
                _CategoryId = value;
            }
        }

        public decimal CostPrice
        {
            get
            {
                return this.DefaultSku.CostPrice;
            }
        }

        public SKUItem DefaultSku
        {
            get
            {
                return (this.defaultSku ?? (this.defaultSku = this.Skus.Values.First<SKUItem>()));
            }
        }

        public string Description
        {
            
            get
            {
                return _Description;
            }
            
            set
            {
                _Description = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return _DisplaySequence;
            }
            
            set
            {
                _DisplaySequence = value;
            }
        }

        public string ExtendCategoryPath
        {
            
            get
            {
                return _ExtendCategoryPath;
            }
            
            set
            {
                _ExtendCategoryPath = value;
            }
        }

        public bool HasSKU
        {
            
            get
            {
                return _HasSKU;
            }
            
            set
            {
                _HasSKU = value;
            }
        }

        public string ImageUrl1
        {
            
            get
            {
                return _ImageUrl1;
            }
            
            set
            {
                _ImageUrl1 = value;
            }
        }

        public string ImageUrl2
        {
            
            get
            {
                return _ImageUrl2;
            }
            
            set
            {
                _ImageUrl2 = value;
            }
        }

        public string ImageUrl3
        {
            
            get
            {
                return _ImageUrl3;
            }
            
            set
            {
                _ImageUrl3 = value;
            }
        }

        public string ImageUrl4
        {
            
            get
            {
                return _ImageUrl4;
            }
            
            set
            {
                _ImageUrl4 = value;
            }
        }

        public string ImageUrl5
        {
            
            get
            {
                return _ImageUrl5;
            }
            
            set
            {
                _ImageUrl5 = value;
            }
        }

        public int LineId
        {
            
            get
            {
                return _LineId;
            }
            
            set
            {
                _LineId = value;
            }
        }

        public decimal LowestSalePrice
        {
            
            get
            {
                return _LowestSalePrice;
            }
            
            set
            {
                _LowestSalePrice = value;
            }
        }

        public string MainCategoryPath
        {
            
            get
            {
                return _MainCategoryPath;
            }
            
            set
            {
                _MainCategoryPath = value;
            }
        }

        public decimal? MarketPrice
        {
            
            get
            {
                return _MarketPrice;
            }
            
            set
            {
                _MarketPrice = value;
            }
        }

        public decimal MaxSalePrice
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in this.Skus.Values.Where<SKUItem>(delegate (SKUItem sku) {
                    return sku.SalePrice > maxSalePrice[0];
                }))
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
            }
        }

        [HtmlCoding]
        public string MetaDescription
        {
            
            get
            {
                return _MetaDescription;
            }
            
            set
            {
                _MetaDescription = value;
            }
        }

        [HtmlCoding]
        public string MetaKeywords
        {
            
            get
            {
                return _MetaKeywords;
            }
            
            set
            {
                _MetaKeywords = value;
            }
        }

        public decimal MinSalePrice
        {
            get
            {
                decimal[] minSalePrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in this.Skus.Values.Where<SKUItem>(delegate (SKUItem sku) {
                    return sku.SalePrice < minSalePrice[0];
                }))
                {
                    minSalePrice[0] = item.SalePrice;
                }
                return minSalePrice[0];
            }
        }

        public Hidistro.Entities.Commodities.PenetrationStatus PenetrationStatus
        {
            
            get
            {
                return _PenetrationStatus;
            }
            
            set
            {
                _PenetrationStatus = value;
            }
        }

        public string ProductCode
        {
            
            get
            {
                return _ProductCode;
            }
            
            set
            {
                _ProductCode = value;
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

        [HtmlCoding]
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

        public decimal PurchasePrice
        {
            get
            {
                return this.DefaultSku.PurchasePrice;
            }
        }

        public int SaleCounts
        {
            
            get
            {
                return _SaleCounts;
            }
            
            set
            {
                _SaleCounts = value;
            }
        }

        public ProductSaleStatus SaleStatus
        {
            
            get
            {
                return _SaleStatus;
            }
            
            set
            {
                _SaleStatus = value;
            }
        }

        [HtmlCoding]
        public string ShortDescription
        {
            
            get
            {
                return _ShortDescription;
            }
            
            set
            {
                _ShortDescription = value;
            }
        }

        public int ShowSaleCounts
        {
            
            get
            {
                return _ShowSaleCounts;
            }
            
            set
            {
                _ShowSaleCounts = value;
            }
        }

        public string SKU
        {
            get
            {
                return this.DefaultSku.SKU;
            }
        }

        public string SkuId
        {
            get
            {
                return this.DefaultSku.SkuId;
            }
        }

        public Dictionary<string, SKUItem> Skus
        {
            get
            {
                return (this.skus ?? (this.skus = new Dictionary<string, SKUItem>()));
            }
        }

        public int Stock
        {
            get
            {
                return this.Skus.Values.Sum<SKUItem>(delegate (SKUItem sku) {
                    return sku.Stock;
                });
            }
        }

        public long TaobaoProductId
        {
            
            get
            {
                return _TaobaoProductId;
            }
            
            set
            {
                _TaobaoProductId = value;
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

        public string ThumbnailUrl160
        {
            
            get
            {
                return _ThumbnailUrl160;
            }
            
            set
            {
                _ThumbnailUrl160 = value;
            }
        }

        public string ThumbnailUrl180
        {
            
            get
            {
                return _ThumbnailUrl180;
            }
            
            set
            {
                _ThumbnailUrl180 = value;
            }
        }

        public string ThumbnailUrl220
        {
            
            get
            {
                return _ThumbnailUrl220;
            }
            
            set
            {
                _ThumbnailUrl220 = value;
            }
        }

        public string ThumbnailUrl310
        {
            
            get
            {
                return _ThumbnailUrl310;
            }
            
            set
            {
                _ThumbnailUrl310 = value;
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

        public string ThumbnailUrl410
        {
            
            get
            {
                return _ThumbnailUrl410;
            }
            
            set
            {
                _ThumbnailUrl410 = value;
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

        [HtmlCoding]
        public string Title
        {
            
            get
            {
                return _Title;
            }
            
            set
            {
                _Title = value;
            }
        }

        public int? TypeId
        {
            
            get
            {
                return _TypeId;
            }
            
            set
            {
                _TypeId = value;
            }
        }

        public string Unit
        {
            
            get
            {
                return _Unit;
            }
            
            set
            {
                _Unit = value;
            }
        }

        public int VistiCounts
        {
            
            get
            {
                return _VistiCounts;
            }
            
            set
            {
                _VistiCounts = value;
            }
        }

        public decimal Weight
        {
            get
            {
                return this.DefaultSku.Weight;
            }
        }
    }
}

