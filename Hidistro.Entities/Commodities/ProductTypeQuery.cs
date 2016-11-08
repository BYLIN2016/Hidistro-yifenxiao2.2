namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductTypeQuery : Pagination
    {
        
        private string _TypeName;

        public string TypeName
        {
            
            get
            {
                return _TypeName;
            }
            
            set
            {
                _TypeName = value;
            }
        }
    }
}

