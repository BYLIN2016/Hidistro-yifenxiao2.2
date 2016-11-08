namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingRegionInfo
    {
        
        private int _GroupId;
        
        private int _RegionId;
        
        private int _TemplateId;

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

        public int RegionId
        {
            
            get
            {
                return _RegionId;
            }
            
            set
            {
                _RegionId = value;
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

