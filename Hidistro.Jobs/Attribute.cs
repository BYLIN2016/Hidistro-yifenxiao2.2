namespace Hidistro.Jobs
{
    using System;
    using System.Runtime.CompilerServices;

    public class Attribute
    {
        
        private string _AttrName;
        
        private string _AttrValue;

        public string AttrName
        {
            
            get
            {
                return _AttrName;
            }
            
            set
            {
                _AttrName = value;
            }
        }

        public string AttrValue
        {
            
            get
            {
                return _AttrValue;
            }
            
            set
            {
                _AttrValue = value;
            }
        }
    }
}

