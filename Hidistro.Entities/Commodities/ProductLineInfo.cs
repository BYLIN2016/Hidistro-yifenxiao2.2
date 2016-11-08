namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductLineInfo
    {
        
        private int _LineId;
        
        private string _Name;
        
        private string _SupplierName;

        public int LineId
        {
            
            get
            {
                return _LineId;
            }
            
            set
            {
                _LineId = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValProductLine", MessageTemplate="产品线名称不能为空，长度限制在1-60个字符之间"), HtmlCoding]
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

        public string SupplierName
        {
            
            get
            {
                return _SupplierName;
            }
            
            set
            {
                _SupplierName = value;
            }
        }
    }
}

