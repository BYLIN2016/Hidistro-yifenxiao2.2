namespace Hidistro.Entities.Commodities
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AttributeInfo
    {
        
        private int _AttributeId;
        
        private string _AttributeName;
        
        private int _DisplaySequence;
        
        private int _TypeId;
        
        private string _TypeName;
        
        private AttributeUseageMode _UsageMode;
        
        private bool _UseAttributeImage;
        private IList<AttributeValueInfo> attributeValues;

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

        [StringLengthValidator(1, 30, Ruleset="ValAttribute", MessageTemplate="扩展属性的名称，长度在1至30个字符之间")]
        public string AttributeName
        {
            
            get
            {
                return _AttributeName;
            }
            
            set
            {
                _AttributeName = value;
            }
        }

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
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

        public bool IsMultiView
        {
            get
            {
                return (this.UsageMode == AttributeUseageMode.MultiView);
            }
        }

        public int TypeId
        {
            
            get
            {
                return _TypeId;
            }
            
            set
            {
                _TypeId = value;
            }
        }

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

        public AttributeUseageMode UsageMode
        {
            
            get
            {
                return _UsageMode;
            }
            
            set
            {
                _UsageMode = value;
            }
        }

        public bool UseAttributeImage
        {
            
            get
            {
                return _UseAttributeImage;
            }
            
            set
            {
                _UseAttributeImage = value;
            }
        }

        public string ValuesString
        {
            get
            {
                string str = string.Empty;
                foreach (AttributeValueInfo info in this.AttributeValues)
                {
                    str = str + info.ValueStr + ",";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
    }
}

