namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SubjectListQuery : Pagination
    {
        
        private int? _BrandCategoryId;
        
        private string _CategoryIds;
        
        private string _Keywords;
        
        private int _MaxNum;
        
        private decimal? _MaxPrice;
        
        private decimal? _MinPrice;
        
        private int? _ProductTypeId;
        
        private int _TagId;
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

        public int? BrandCategoryId
        {
            
            get
            {
                return _BrandCategoryId;
            }
            
            set
            {
                _BrandCategoryId = value;
            }
        }

        public string CategoryIds
        {
            
            get
            {
                return _CategoryIds;
            }
            
            set
            {
                _CategoryIds = value;
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

        public int MaxNum
        {
            
            get
            {
                return _MaxNum;
            }
            
            set
            {
                _MaxNum = value;
            }
        }

        public decimal? MaxPrice
        {
            
            get
            {
                return _MaxPrice;
            }
            
            set
            {
                _MaxPrice = value;
            }
        }

        public decimal? MinPrice
        {
            
            get
            {
                return _MinPrice;
            }
            
            set
            {
                _MinPrice = value;
            }
        }

        public int? ProductTypeId
        {
            
            get
            {
                return _ProductTypeId;
            }
            
            set
            {
                _ProductTypeId = value;
            }
        }

        public int TagId
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
    }
}

