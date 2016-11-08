namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    public class ShippingAddressInfo
    {
        
        private string _Address;
        
        private string _CellPhone;
        
        private int _RegionId;
        
        private int _ShippingId;
        
        private string _ShipTo;
        
        private string _TelPhone;
        
        private int _UserId;
        
        private string _Zipcode;

        [HtmlCoding]
        public string Address
        {
            
            get
            {
                return _Address;
            }
            
            set
            {
                _Address = value;
            }
        }

        [HtmlCoding]
        public string CellPhone
        {
            
            get
            {
                return _CellPhone;
            }
            
            set
            {
                _CellPhone = value;
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

        public int ShippingId
        {
            
            get
            {
                return _ShippingId;
            }
            
            set
            {
                _ShippingId = value;
            }
        }

        [HtmlCoding]
        public string ShipTo
        {
            
            get
            {
                return _ShipTo;
            }
            
            set
            {
                _ShipTo = value;
            }
        }

        [HtmlCoding]
        public string TelPhone
        {
            
            get
            {
                return _TelPhone;
            }
            
            set
            {
                _TelPhone = value;
            }
        }

        public int UserId
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

        public string Zipcode
        {
            
            get
            {
                return _Zipcode;
            }
            
            set
            {
                _Zipcode = value;
            }
        }
    }
}

