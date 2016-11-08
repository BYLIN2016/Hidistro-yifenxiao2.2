namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class ProductBrowseInfo
    {
        
        private string _BrandName;
        
        private string _CategoryName;
        
        private DataTable _DbAttribute;
        
        private DataTable _DBConsultations;
        
        private DataTable _DbCorrelatives;
        
        private DataTable _DBReviews;
        
        private DataTable _DbSKUs;
        
        private ProductInfo _Product;

        public string BrandName
        {
            
            get
            {
                return _BrandName;
            }
            
            set
            {
                _BrandName = value;
            }
        }

        public string CategoryName
        {
            
            get
            {
                return _CategoryName;
            }
            
            set
            {
                _CategoryName = value;
            }
        }

        public DataTable DbAttribute
        {
            
            get
            {
                return _DbAttribute;
            }
            
            set
            {
                _DbAttribute = value;
            }
        }

        public DataTable DBConsultations
        {
            
            get
            {
                return _DBConsultations;
            }
            
            set
            {
                _DBConsultations = value;
            }
        }

        public DataTable DbCorrelatives
        {
            
            get
            {
                return _DbCorrelatives;
            }
            
            set
            {
                _DbCorrelatives = value;
            }
        }

        public DataTable DBReviews
        {
            
            get
            {
                return _DBReviews;
            }
            
            set
            {
                _DBReviews = value;
            }
        }

        public DataTable DbSKUs
        {
            
            get
            {
                return _DbSKUs;
            }
            
            set
            {
                _DbSKUs = value;
            }
        }

        public ProductInfo Product
        {
            
            get
            {
                return _Product;
            }
            
            set
            {
                _Product = value;
            }
        }
    }
}

