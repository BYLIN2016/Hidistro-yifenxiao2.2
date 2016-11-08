namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductQuery : Pagination
    {
        
        private int? _BrandId;
        
        private int? _CategoryId;
        
        private DateTime? _EndDate;
        
        private bool _IsAlert;
        
        private bool? _IsIncludeBundlingProduct;
        
        private bool? _IsIncludePromotionProduct;
        
        private int? _IsMakeTaobao;
        
        private string _Keywords;
        
        private string _MaiCategoryPath;
        
        private decimal? _MaxSalePrice;
        
        private decimal? _MinSalePrice;
        
        private Hidistro.Entities.Commodities.PenetrationStatus _PenetrationStatus;
        
        private string _ProductCode;
        
        private int? _ProductLineId;
        
        private Hidistro.Entities.Commodities.PublishStatus _PublishStatus;
        
        private ProductSaleStatus _SaleStatus;
        
        private DateTime? _StartDate;
        
        private int? _TagId;
        
        private int? _TypeId;
        
        private int? _UserId;

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

        public int? CategoryId
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

        public bool IsAlert
        {
            
            get
            {
                return _IsAlert;
            }
            
            set
            {
                _IsAlert = value;
            }
        }

        public bool? IsIncludeBundlingProduct
        {
            
            get
            {
                return _IsIncludeBundlingProduct;
            }
            
            set
            {
                _IsIncludeBundlingProduct = value;
            }
        }

        public bool? IsIncludePromotionProduct
        {
            
            get
            {
                return _IsIncludePromotionProduct;
            }
            
            set
            {
                _IsIncludePromotionProduct = value;
            }
        }

        public int? IsMakeTaobao
        {
            
            get
            {
                return _IsMakeTaobao;
            }
            
            set
            {
                _IsMakeTaobao = value;
            }
        }

        [HtmlCoding]
        public string Keywords
        {
            
            get
            {
                return _Keywords;
            }
            
            set
            {
                _Keywords = value;
            }
        }

        public string MaiCategoryPath
        {
            
            get
            {
                return _MaiCategoryPath;
            }
            
            set
            {
                _MaiCategoryPath = value;
            }
        }

        public decimal? MaxSalePrice
        {
            
            get
            {
                return _MaxSalePrice;
            }
            
            set
            {
                _MaxSalePrice = value;
            }
        }

        public decimal? MinSalePrice
        {
            
            get
            {
                return _MinSalePrice;
            }
            
            set
            {
                _MinSalePrice = value;
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

        [HtmlCoding]
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

        public int? ProductLineId
        {
            
            get
            {
                return _ProductLineId;
            }
            
            set
            {
                _ProductLineId = value;
            }
        }

        public Hidistro.Entities.Commodities.PublishStatus PublishStatus
        {
            
            get
            {
                return _PublishStatus;
            }
            
            set
            {
                _PublishStatus = value;
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

        public int? TagId
        {
            
            get
            {
                return _TagId;
            }
            
            set
            {
                _TagId = value;
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

        public int? UserId
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

