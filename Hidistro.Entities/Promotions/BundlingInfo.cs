namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BundlingInfo
    {
        
        private DateTime _AddTime;
        
        private int _BundlingID;
        
        private List<BundlingItemInfo> _BundlingItemInfos;
        
        private int _DisplaySequence;
        
        private string _Name;
        
        private int _Num;
        
        private decimal _Price;
        
        private int _SaleStatus;
        
        private string _ShortDescription;

        public BundlingInfo()
        {
            if (this.BundlingItemInfos == null)
            {
                this.BundlingItemInfos = new List<BundlingItemInfo>();
            }
        }

        public DateTime AddTime
        {
            
            get
            {
                return _AddTime;
            }
            
            set
            {
                _AddTime = value;
            }
        }

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

        public List<BundlingItemInfo> BundlingItemInfos
        {
            
            get
            {
                return _BundlingItemInfos;
            }
            
            set
            {
                _BundlingItemInfos = value;
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

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }

        public int Num
        {
            
            get
            {
                return _Num;
            }
            
            set
            {
                _Num = value;
            }
        }

        public decimal Price
        {
            
            get
            {
                return _Price;
            }
            
            set
            {
                _Price = value;
            }
        }

        public int SaleStatus
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
    }
}

