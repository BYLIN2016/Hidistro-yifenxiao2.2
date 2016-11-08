namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class BalanceDrawRequestQuery : Pagination
    {
        
        private DateTime? _FromDate;
        
        private DateTime? _ToDate;
        
        private int? _UserId;
        
        private string _UserName;

        public DateTime? FromDate
        {
            
            get
            {
                return _FromDate;
            }
            
            set
            {
                _FromDate = value;
            }
        }

        public DateTime? ToDate
        {
            
            get
            {
                return _ToDate;
            }
            
            set
            {
                _ToDate = value;
            }
        }

        public int? UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
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

