namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductTypeInfo
    {
        
        private string _Remark;
        
        private int _TypeId;
        
        private string _TypeName;
        private IList<int> brands;

        public IList<int> Brands
        {
            get
            {
                if (this.brands == null)
                {
                    this.brands = new List<int>();
                }
                return this.brands;
            }
            set
            {
                this.brands = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValProductType", MessageTemplate="备注的长度限制在0-100个字符之间")]
        public string Remark
        {
            
            get
            {
                return _Remark;
            }
            
            set
            {
                _Remark = value;
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

        [StringLengthValidator(1, 30, Ruleset="ValProductType", MessageTemplate="商品类型名称不能为空，长度限制在1-30个字符之间")]
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
    }
}

