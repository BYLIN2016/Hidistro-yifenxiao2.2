namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ShippersInfo
    {
        
        private string _Address;
        
        private string _CellPhone;
        
        private int _DistributorUserId;
        
        private bool _IsDefault;
        
        private int _RegionId;
        
        private string _Remark;
        
        private int _ShipperId;
        
        private string _ShipperName;
        
        private string _ShipperTag;
        
        private string _TelPhone;
        
        private string _Zipcode;

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Valshipper", MessageTemplate="详细地址不能为空，长度限制在300个字符以内")]
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

        [StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="手机号码的长度限制在20个字符以内"), HtmlCoding]
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

        public int DistributorUserId
        {
            
            get
            {
                return _DistributorUserId;
            }
            
            set
            {
                _DistributorUserId = value;
            }
        }

        public bool IsDefault
        {
            
            get
            {
                return _IsDefault;
            }
            
            set
            {
                _IsDefault = value;
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

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="Valshipper", MessageTemplate="备注的长度限制在300个字符以内")]
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

        public int ShipperId
        {
            
            get
            {
                return _ShipperId;
            }
            
            set
            {
                _ShipperId = value;
            }
        }

        [StringLengthValidator(2, 20, Ruleset="Valshipper", MessageTemplate="发货人姓名不能为空，长度在2-20个字符之间"), RegexValidator(@"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*", Ruleset="Valshipper", MessageTemplate="发货人姓名只能是汉字或字母开头"), HtmlCoding]
        public string ShipperName
        {
            
            get
            {
                return _ShipperName;
            }
            
            set
            {
                _ShipperName = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="Valshipper", MessageTemplate="发货点不能为空，长度限制在30个字符以内"), HtmlCoding]
        public string ShipperTag
        {
            
            get
            {
                return _ShipperTag;
            }
            
            set
            {
                _ShipperTag = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="电话号码的长度限制在20个字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="邮编的长度限制在20个字符以内"), HtmlCoding]
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

