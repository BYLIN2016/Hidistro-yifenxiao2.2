namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class VoteItemInfo
    {
        
        private int _ItemCount;
        
        private decimal _Percentage;
        
        private long _VoteId;
        
        private long _VoteItemId;
        
        private string _VoteItemName;

        public int ItemCount
        {
            
            get
            {
                return _ItemCount;
            }
            
            set
            {
                _ItemCount = value;
            }
        }

        public decimal Lenth
        {
            get
            {
                return this.Percentage * Convert.ToDecimal((double)4.2);
            }
        }

        public decimal Percentage
        {
            
            get
            {
                return _Percentage;
            }
            
            set
            {
                _Percentage = value;
            }
        }

        public long VoteId
        {
            
            get
            {
                return _VoteId;
            }
            
            set
            {
                _VoteId = value;
            }
        }

        public long VoteItemId
        {
            
            get
            {
                return _VoteItemId;
            }
            
            set
            {
                _VoteItemId = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="VoteItem", MessageTemplate="提供给用户选择的内容，长度限制在60个字符以内")]
        public string VoteItemName
        {
            
            get
            {
                return _VoteItemName;
            }
            
            set
            {
                _VoteItemName = value;
            }
        }
    }
}

