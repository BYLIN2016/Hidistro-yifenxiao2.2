namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PromotionInfo
    {
        
        private int _ActivityId;
        
        private decimal _Condition;
        
        private string _Description;
        
        private decimal _DiscountValue;
        
        private DateTime _EndDate;
        
        private IList<int> _memberGradeIds;
        
        private string _Name;
        
        private Hidistro.Entities.Promotions.PromoteType _PromoteType;
        
        private DateTime _StartDate;

        public int ActivityId
        {
            
            get
            {
                return _ActivityId;
            }
            
            set
            {
                _ActivityId = value;
            }
        }

        public decimal Condition
        {
            
            get
            {
                return _Condition;
            }
            
            set
            {
                _Condition = value;
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

        public DateTime EndDate
        {
            
            get
            {
                return _EndDate;
            }
            
            set
            {
                _EndDate = value;
            }
        }

        private IList<int> memberGradeIds
        {
            
            get
            {
                return _memberGradeIds;
            }
            
            set
            {
                _memberGradeIds = value;
            }
        }

        public IList<int> MemberGradeIds
        {
            get
            {
                if (this.memberGradeIds == null)
                {
                    this.memberGradeIds = new List<int>();
                }
                return this.memberGradeIds;
            }
            set
            {
                this.memberGradeIds = value;
            }
        }

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

        public Hidistro.Entities.Promotions.PromoteType PromoteType
        {
            
            get
            {
                return _PromoteType;
            }
            
            set
            {
                _PromoteType = value;
            }
        }

        public DateTime StartDate
        {
            
            get
            {
                return _StartDate;
            }
            
            set
            {
                _StartDate = value;
            }
        }
    }
}

