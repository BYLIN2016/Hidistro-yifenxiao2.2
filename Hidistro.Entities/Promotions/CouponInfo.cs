namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [HasSelfValidation]
    public class CouponInfo
    {
        
        private decimal? _Amount;
        
        private DateTime _ClosingTime;
        
        private int _CouponId;
        
        private string _Description;
        
        private decimal _DiscountValue;
        
        private string _Name;
        
        private int _NeedPoint;
        
        private int _SentCount;
        
        private DateTime _StartTime;
        
        private int _UsedCount;

        public CouponInfo()
        {
        }

        public CouponInfo(string name, DateTime closingTime, DateTime startTime, string description, decimal? amount, decimal discountValue)
        {
            this.Name = name;
            this.ClosingTime = closingTime;
            this.StartTime = startTime;
            this.Description = description;
            this.Amount = amount;
            this.DiscountValue = discountValue;
        }

        public CouponInfo(int couponId, string name, DateTime closingTime, DateTime startTime, string description, decimal? amount, decimal discountValue)
        {
            this.CouponId = couponId;
            this.Name = name;
            this.ClosingTime = closingTime;
            this.StartTime = startTime;
            this.Description = description;
            this.Amount = amount;
            this.DiscountValue = discountValue;
        }

        [SelfValidation(Ruleset="ValCoupon")]
        public void CompareAmount(ValidationResults result)
        {
            decimal? nullable;
            if (this.Amount.HasValue && ((this.DiscountValue > (nullable = this.Amount).GetValueOrDefault()) && nullable.HasValue))
            {
                result.AddResult(new ValidationResult("折扣值不能大于满足金额", this, "", "", null));
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValCoupon"), NotNullValidator(Negated=true, Ruleset="ValCoupon"), ValidatorComposition(CompositionType.Or, Ruleset="ValCoupon", MessageTemplate="满足金额，金额大小0.01-1000万之间")]
        public decimal? Amount
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

        public DateTime ClosingTime
        {
            
            get
            {
                return _ClosingTime;
            }
            
            set
            {
                _ClosingTime = value;
            }
        }

        public int CouponId
        {
            
            get
            {
                return _CouponId;
            }
            
            set
            {
                _CouponId = value;
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

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValCoupon", MessageTemplate="可抵扣金额不能为空，金额大小0.01-1000万之间")]
        public decimal DiscountValue
        {
            
            get
            {
                return _DiscountValue;
            }
            
            set
            {
                _DiscountValue = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValCoupon", MessageTemplate="优惠券名称不能为空，长度限制在1-60个字符之间"), HtmlCoding]
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

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValCoupon", MessageTemplate="兑换所需积分不能为空，大小0-10000之间")]
        public int NeedPoint
        {
            
            get
            {
                return _NeedPoint;
            }
            
            set
            {
                _NeedPoint = value;
            }
        }

        public int SentCount
        {
            
            get
            {
                return _SentCount;
            }
            
            set
            {
                _SentCount = value;
            }
        }

        public DateTime StartTime
        {
            
            get
            {
                return _StartTime;
            }
            
            set
            {
                _StartTime = value;
            }
        }

        public int UsedCount
        {
            
            get
            {
                return _UsedCount;
            }
            
            set
            {
                _UsedCount = value;
            }
        }
    }
}

