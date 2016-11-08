namespace Hidistro.Entities.Sales
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingModeInfo
    {
        
        private decimal? _AddPrice;
        
        private decimal? _AddWeight;
        
        private string _Description;
        
        private int _DisplaySequence;
        
        private int _ModeId;
        
        private string _Name;
        
        private decimal _Price;
        
        private int _TemplateId;
        
        private string _TemplateName;
        
        private decimal _Weight;
        private IList<string> expressCompany;
        private IList<ShippingModeGroupInfo> modeGroup;

        [ValidatorComposition(CompositionType.Or, Ruleset="ValShippingModeInfo", MessageTemplate="默认加价必须限制在1000万以内"), NotNullValidator(Negated=true, Ruleset="ValShippingModeInfo"), RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo")]
        public decimal? AddPrice
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

        [ValidatorComposition(CompositionType.Or, Ruleset="ValShippingModeInfo", MessageTemplate="加价重量必须限制在100千克以内"), RangeValidator(0, RangeBoundaryType.Inclusive, 0x186a0, RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo"), NotNullValidator(Negated=true, Ruleset="ValShippingModeInfo")]
        public decimal? AddWeight
        {
            
            get
            {
                return _AddWeight;
            }
            
            set
            {
                _AddWeight = value;
            }
        }

        public string Description
        {
            
            get
            {
                return _Description;
            }
            
            set
            {
                _Description = value;
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

        public IList<string> ExpressCompany
        {
            get
            {
                if (this.expressCompany == null)
                {
                    this.expressCompany = new List<string>();
                }
                return this.expressCompany;
            }
            set
            {
                this.expressCompany = value;
            }
        }

        public IList<ShippingModeGroupInfo> ModeGroup
        {
            get
            {
                if (this.modeGroup == null)
                {
                    this.modeGroup = new List<ShippingModeGroupInfo>();
                }
                return this.modeGroup;
            }
            set
            {
                this.modeGroup = value;
            }
        }

        public int ModeId
        {
            
            get
            {
                return _ModeId;
            }
            
            set
            {
                _ModeId = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValShippingModeInfo", MessageTemplate="配送方式名称不能为空，长度限制在60字符以内")]
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

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo", MessageTemplate="默认起步价不能为空,限制在1000万以内")]
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

        public string TemplateName
        {
            
            get
            {
                return _TemplateName;
            }
            
            set
            {
                _TemplateName = value;
            }
        }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x186a0, RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo", MessageTemplate="起步重量不能为空,限制在100千克以内")]
        public decimal Weight
        {
            
            get
            {
                return _Weight;
            }
            
            set
            {
                _Weight = value;
            }
        }
    }
}

