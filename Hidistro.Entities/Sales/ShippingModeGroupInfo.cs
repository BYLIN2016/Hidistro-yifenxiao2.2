namespace Hidistro.Entities.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingModeGroupInfo
    {
        
        private decimal _AddPrice;
        
        private int _GroupId;
        
        private decimal _Price;
        
        private int _TemplateId;
        private IList<ShippingRegionInfo> modeRegions = new List<ShippingRegionInfo>();

        public decimal AddPrice
        {
            
            get
            {
                return _AddPrice;
            }
            
            set
            {
                _AddPrice = value;
            }
        }

        public int GroupId
        {
            
            get
            {
                return _GroupId;
            }
            
            set
            {
                _GroupId = value;
            }
        }

        public IList<ShippingRegionInfo> ModeRegions
        {
            get
            {
                return this.modeRegions;
            }
            set
            {
                this.modeRegions = value;
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

        public int TemplateId
        {
            
            get
            {
                return _TemplateId;
            }
            
            set
            {
                _TemplateId = value;
            }
        }
    }
}

