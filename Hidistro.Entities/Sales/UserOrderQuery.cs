namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserOrderQuery : Pagination
    {
        
        private DateTime? _EndDate;
        
        private string _OrderId;
        
        private string _ShipTo;
        
        private DateTime? _StartDate;
        
        private string _UserName;

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

        public string OrderId
        {
            
            get
            {
                return _OrderId;
            }
            
            set
            {
                _OrderId = value;
            }
        }

        public string ShipTo
        {
            
            get
            {
                return _ShipTo;
            }
            
            set
            {
                _ShipTo = value;
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

        public string UserName
        {
            
            get
            {
                return _UserName;
            }
            
            set
            {
                _UserName = value;
            }
        }
    }
}

