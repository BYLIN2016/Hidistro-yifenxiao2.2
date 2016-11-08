namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AttributeValueInfo
    {
        
        private int _AttributeId;
        
        private int _DisplaySequence;
        
        private string _ImageUrl;
        
        private int _ValueId;
        
        private string _ValueStr;

        public int AttributeId
        {
            
            get
            {
                return _AttributeId;
            }
            
            set
            {
                _AttributeId = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return _DisplaySequence;
            }
            
            set
            {
                _DisplaySequence = value;
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return _ImageUrl;
            }
            
            set
            {
                _ImageUrl = value;
            }
        }

        public int ValueId
        {
            
            get
            {
                return _ValueId;
            }
            
            set
            {
                _ValueId = value;
            }
        }

        public string ValueStr
        {
            
            get
            {
                return _ValueStr;
            }
            
            set
            {
                _ValueStr = value;
            }
        }
    }
}

