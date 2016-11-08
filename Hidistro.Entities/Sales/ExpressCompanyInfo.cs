namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ExpressCompanyInfo
    {
        
        private string _Kuaidi100Code;
        
        private string _Name;
        
        private string _TaobaoCode;

        public string Kuaidi100Code
        {
            
            get
            {
                return _Kuaidi100Code;
            }
            
            set
            {
                _Kuaidi100Code = value;
            }
        }

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }

        public string TaobaoCode
        {
            
            get
            {
                return _TaobaoCode;
            }
            
            set
            {
                _TaobaoCode = value;
            }
        }
    }
}

