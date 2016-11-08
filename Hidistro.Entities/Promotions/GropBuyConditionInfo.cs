namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GropBuyConditionInfo
    {
        
        private int _Count;
        
        private int _GroupBuyId;
        
        private decimal _Price;

        public int Count
        {
            
            get
            {
                return _Count;
            }
            
            set
            {
                _Count = value;
            }
        }

        public int GroupBuyId
        {
            
            get
            {
                return _GroupBuyId;
            }
            
            set
            {
                _GroupBuyId = value;
            }
        }

        public decimal Price
        {
            
            get
            {
                return _Price;
            }
            
            set
            {
                _Price = value;
            }
        }
    }
}

