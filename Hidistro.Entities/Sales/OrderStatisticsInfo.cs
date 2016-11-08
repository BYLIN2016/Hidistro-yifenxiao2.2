namespace Hidistro.Entities.Sales
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class OrderStatisticsInfo
    {
        
        private DataTable _OrderTbl;
        
        private decimal _ProfitsOfPage;
        
        private decimal _ProfitsOfSearch;
        
        private int _TotalCount;
        
        private decimal _TotalOfPage;
        
        private decimal _TotalOfSearch;

        public DataTable OrderTbl
        {
            
            get
            {
                return _OrderTbl;
            }
            
            set
            {
                _OrderTbl = value;
            }
        }

        public decimal ProfitsOfPage
        {
            
            get
            {
                return _ProfitsOfPage;
            }
            
            set
            {
                _ProfitsOfPage = value;
            }
        }

        public decimal ProfitsOfSearch
        {
            
            get
            {
                return _ProfitsOfSearch;
            }
            
            set
            {
                _ProfitsOfSearch = value;
            }
        }

        public int TotalCount
        {
            
            get
            {
                return _TotalCount;
            }
            
            set
            {
                _TotalCount = value;
            }
        }

        public decimal TotalOfPage
        {
            
            get
            {
                return _TotalOfPage;
            }
            
            set
            {
                _TotalOfPage = value;
            }
        }

        public decimal TotalOfSearch
        {
            
            get
            {
                return _TotalOfSearch;
            }
            
            set
            {
                _TotalOfSearch = value;
            }
        }
    }
}

