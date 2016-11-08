namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class BundlingItemInfo
    {
        
        private int _BundlingID;
        
        private int _BundlingItemId;
        
        private int _ProductID;
        
        private string _ProductName;
        
        private int _ProductNum;
        
        private decimal _ProductPrice;
        
        private string _SkuId;

        public int BundlingID
        {
            
            get
            {
                return _BundlingID;
            }
            
            set
            {
                _BundlingID = value;
            }
        }

        public int BundlingItemId
        {
            
            get
            {
                return _BundlingItemId;
            }
            
            set
            {
                _BundlingItemId = value;
            }
        }

        public int ProductID
        {
            
            get
            {
                return _ProductID;
            }
            
            set
            {
                _ProductID = value;
            }
        }

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

        public int ProductNum
        {
            
            get
            {
                return _ProductNum;
            }
            
            set
            {
                _ProductNum = value;
            }
        }

        public decimal ProductPrice
        {
            
            get
            {
                return _ProductPrice;
            }
            
            set
            {
                _ProductPrice = value;
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
    }
}

