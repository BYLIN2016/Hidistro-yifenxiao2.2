namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserStatisticsInfo
    {
        
        private decimal _AllUserCounts;
        
        private long _RegionId;
        
        private string _RegionName;
        
        private int _Usercounts;

        public decimal AllUserCounts
        {
            
            get
            {
                return _AllUserCounts;
            }
            
            set
            {
                _AllUserCounts = value;
            }
        }

        public decimal Lenth
        {
            get
            {
                return (this.Percentage * 4M);
            }
        }

        public decimal Percentage
        {
            get
            {
                if (this.AllUserCounts != 0M)
                {
                    return ((this.Usercounts / this.AllUserCounts) * 100M);
                }
                return 0M;
            }
        }

        public long RegionId
        {
            
            get
            {
                return _RegionId;
            }
            
            set
            {
                _RegionId = value;
            }
        }

        public string RegionName
        {
            
            get
            {
                return _RegionName;
            }
            
            set
            {
                _RegionName = value;
            }
        }

        public int Usercounts
        {
            
            get
            {
                return _Usercounts;
            }
            
            set
            {
                _Usercounts = value;
            }
        }
    }
}

