namespace Hidistro.Core.Entities
{
    using Hidistro.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;

    public class Pagination
    {
        
        private bool _IsCount;
        
        private int _PageIndex;
        
        private int _PageSize;
        
        private string _SortBy;
        
        private SortAction _SortOrder;

        public Pagination()
        {
            this.IsCount = true;
            this.PageSize = 10;
        }

        public bool IsCount
        {
            
            get
            {
                return _IsCount;
            }
            
            set
            {
                _IsCount = value;
            }
        }

        public int PageIndex
        {
            
            get
            {
                return _PageIndex;
            }
            
            set
            {
                _PageIndex = value;
            }
        }

        public int PageSize
        {
            
            get
            {
                return _PageSize;
            }
            
            set
            {
                _PageSize = value;
            }
        }

        public string SortBy
        {
            
            get
            {
                return _SortBy;
            }
            
            set
            {
                _SortBy = value;
            }
        }

        public SortAction SortOrder
        {
            
            get
            {
                return _SortOrder;
            }
            
            set
            {
                _SortOrder = value;
            }
        }
    }
}

