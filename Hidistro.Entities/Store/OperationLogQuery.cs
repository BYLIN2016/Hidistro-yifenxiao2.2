namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogQuery
    {
        
        private DateTime? _FromDate;
        
        private string _OperationUserName;
        
        private Pagination _Page;
        
        private DateTime? _ToDate;

        public OperationLogQuery()
        {
            this.Page = new Pagination();
        }

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

        public string OperationUserName
        {
            
            get
            {
                return _OperationUserName;
            }
            
            set
            {
                _OperationUserName = value;
            }
        }

        public Pagination Page
        {
            
            get
            {
                return _Page;
            }
            
            set
            {
                _Page = value;
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
    }
}

