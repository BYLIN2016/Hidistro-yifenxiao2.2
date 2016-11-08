namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SaleStatisticsAnalyzeInfo
    {
        
        private int _OrderCounts;
        
        private decimal _OrderTotals;
        
        private int _OrderUserCounts;
        
        private int _UserCounts;
        
        private int _VisitCounts;

        public int OrderCounts
        {
            
            get
            {
                return _OrderCounts;
            }
            
            set
            {
                _OrderCounts = value;
            }
        }

        public decimal OrderTotals
        {
            
            get
            {
                return _OrderTotals;
            }
            
            set
            {
                _OrderTotals = value;
            }
        }

        public int OrderUserCounts
        {
            
            get
            {
                return _OrderUserCounts;
            }
            
            set
            {
                _OrderUserCounts = value;
            }
        }

        public int UserCounts
        {
            
            get
            {
                return _UserCounts;
            }
            
            set
            {
                _UserCounts = value;
            }
        }

        public int VisitCounts
        {
            
            get
            {
                return _VisitCounts;
            }
            
            set
            {
                _VisitCounts = value;
            }
        }
    }
}

