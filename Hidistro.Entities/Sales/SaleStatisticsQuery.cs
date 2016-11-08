namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SaleStatisticsQuery : Pagination
    {
        
        private DateTime? _EndDate;
        
        private string _QueryKey;
        
        private DateTime? _StartDate;

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

        public string QueryKey
        {
            
            get
            {
                return _QueryKey;
            }
            
            set
            {
                _QueryKey = value;
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
    }
}

