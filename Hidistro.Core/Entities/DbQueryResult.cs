namespace Hidistro.Core.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class DbQueryResult
    {
        
        private object _Data;
        
        private int _TotalRecords;

        public object Data
        {
            
            get
            {
                return _Data;
            }
            
            set
            {
                _Data = value;
            }
        }

        public int TotalRecords
        {
            
            get
            {
                return _TotalRecords;
            }
            
            set
            {
                _TotalRecords = value;
            }
        }
    }
}

