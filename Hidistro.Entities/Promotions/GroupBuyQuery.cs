namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GroupBuyQuery : Pagination
    {
        
        private string _ProductName;

        public string ProductName
        {
            
            get
            {
                return _ProductName;
            }
            
            set
            {
                _ProductName = value;
            }
        }
    }
}

