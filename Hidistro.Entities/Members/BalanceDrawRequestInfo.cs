namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [HasSelfValidation]
    public class BalanceDrawRequestInfo
    {
        
        private string _AccountName;
        
        private decimal _Amount;
        
        private string _BankName;
        
        private string _MerchantCode;
        
        private string _Remark;
        
        private DateTime _RequestTime;
        
        private int _UserId;
        
        private string _UserName;

        [StringLengthValidator(1, 30, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="开户人真实姓名不能为空,长度限制在30字符以内"), HtmlCoding]
        public string AccountName
        {
            
            get
            {
                return _AccountName;
            }
            
            set
            {
                _AccountName = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="提现金额不能为空，金额大小0.01-1000万之间")]
        public decimal Amount
        {
            
            get
            {
                return _Amount;
            }
            
            set
            {
                _Amount = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="开银行名称不能为空,长度限制在60字符以内")]
        public string BankName
        {
            
            get
            {
                return _BankName;
            }
            
            set
            {
                _BankName = value;
            }
        }

        [RegexValidator("^[0-9]*$", Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="个人银行帐号只允许输入数字"), StringLengthValidator(1, 100, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="个人银行帐号不能为空,限制在100个字符以内")]
        public string MerchantCode
        {
            
            get
            {
                return _MerchantCode;
            }
            
            set
            {
                _MerchantCode = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="备注长度限制在300字符以内"), HtmlCoding]
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

        public DateTime RequestTime
        {
            
            get
            {
                return _RequestTime;
            }
            
            set
            {
                _RequestTime = value;
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

        [HtmlCoding]
        public string UserName
        {
            
            get
            {
                return _UserName;
            }
            
            set
            {
                _UserName = value;
            }
        }
    }
}

