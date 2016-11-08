namespace Hidistro.Entities.Members
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class BalanceDetailInfo
    {
        
        private decimal _Balance;
        
        private decimal? _Expenses;
        
        private decimal? _Income;
        
        private string _InpourId;
        
        private long _JournalNumber;
        
        private string _Remark;
        
        private DateTime _TradeDate;
        
        private TradeTypes _TradeType;
        
        private int _UserId;
        
        private string _UserName;

        public decimal Balance
        {
            
            get
            {
                return _Balance;
            }
            
            set
            {
                _Balance = value;
            }
        }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDetail"), ValidatorComposition(CompositionType.Or, Ruleset="ValBalanceDetail", MessageTemplate="本次支出的金额，金额大小正负1000万之间"), NotNullValidator(Negated=true, Ruleset="ValBalanceDetail")]
        public decimal? Expenses
        {
            
            get
            {
                return _Expenses;
            }
            
            set
            {
                _Expenses = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValBalanceDetail", MessageTemplate="本次收入的金额，金额大小正负1000万之间"), RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDetail"), NotNullValidator(Negated=true, Ruleset="ValBalanceDetail")]
        public decimal? Income
        {
            
            get
            {
                return _Income;
            }
            
            set
            {
                _Income = value;
            }
        }

        public string InpourId
        {
            
            get
            {
                return _InpourId;
            }
            
            set
            {
                _InpourId = value;
            }
        }

        public long JournalNumber
        {
            
            get
            {
                return _JournalNumber;
            }
            
            set
            {
                _JournalNumber = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValBalanceDetail", MessageTemplate="备注的长度限制在300个字符以内")]
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

        public DateTime TradeDate
        {
            
            get
            {
                return _TradeDate;
            }
            
            set
            {
                _TradeDate = value;
            }
        }

        public TradeTypes TradeType
        {
            
            get
            {
                return _TradeType;
            }
            
            set
            {
                _TradeType = value;
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

