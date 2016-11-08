namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdvancedProductQuery : ProductQuery
    {
        
        private bool _IncludeInStock;
        
        private bool _IncludeOnSales;
        
        private bool _IncludeUnSales;

        public bool IncludeInStock
        {
            
            get
            {
                return _IncludeInStock;
            }
            
            set
            {
                _IncludeInStock = value;
            }
        }

        public bool IncludeOnSales
        {
            
            get
            {
                return _IncludeOnSales;
            }
            
            set
            {
                _IncludeOnSales = value;
            }
        }

        public bool IncludeUnSales
        {
            
            get
            {
                return _IncludeUnSales;
            }
            
            set
            {
                _IncludeUnSales = value;
            }
        }
    }
}

