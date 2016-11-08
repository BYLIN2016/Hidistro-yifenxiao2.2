namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductBrowseQuery : Pagination
    {
        
        private int? _BrandId;
        
        private int? _CategoryId;
        
        private bool _IsPrecise;
        
        private string _Keywords;
        
        private decimal? _MaxSalePrice;
        
        private decimal? _MinSalePrice;
        
        private string _ProductCode;
        
        private string _TagIds;
        private IList<AttributeValueInfo> attributeValues;

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
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

        public bool IsPrecise
        {
            
            get
            {
                return _IsPrecise;
            }
            
            set
            {
                _IsPrecise = value;
            }
        }

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

        public string TagIds
        {
            
            get
            {
                return _TagIds;
            }
            
            set
            {
                _TagIds = value;
            }
        }
    }
}

