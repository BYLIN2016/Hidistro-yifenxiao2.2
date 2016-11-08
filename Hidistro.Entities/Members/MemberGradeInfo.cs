namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberGradeInfo
    {
        
        private string _Description;
        
        private int _Discount;
        
        private int _GradeId;
        
        private bool _IsDefault;
        
        private string _Name;
        
        private int _Points;

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValMemberGrade", MessageTemplate="备注的长度限制在100个字符以内")]
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

        [RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValMemberGrade", MessageTemplate="等级折扣只能是1-100之间的整数")]
        public int Discount
        {
            
            get
            {
                return _Discount;
            }
            
            set
            {
                _Discount = value;
            }
        }

        public int GradeId
        {
            
            get
            {
                return _GradeId;
            }
            
            set
            {
                _GradeId = value;
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValMemberGrade", MessageTemplate="会员等级名称不能为空，长度限制在60个字符以内")]
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

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x7fffffff, RangeBoundaryType.Inclusive, Ruleset="ValMemberGrade", MessageTemplate="满足积分为大于等于0的整数")]
        public int Points
        {
            
            get
            {
                return _Points;
            }
            
            set
            {
                _Points = value;
            }
        }
    }
}

