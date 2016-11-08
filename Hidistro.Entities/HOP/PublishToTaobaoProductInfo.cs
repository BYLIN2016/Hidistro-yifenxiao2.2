namespace Hidistro.Entities.HOP
{
    using System;
    using System.Runtime.CompilerServices;

    public class PublishToTaobaoProductInfo : TaobaoProductInfo
    {
        
        private string _Description;
        
        private string _ImageUrl1;
        
        private string _ImageUrl2;
        
        private string _ImageUrl3;
        
        private string _ImageUrl4;
        
        private string _ImageUrl5;
        
        private string _ProductCode;
        
        private decimal _SalePrice;
        
        private long _TaobaoProductId;
        
        private decimal _Weight;

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

