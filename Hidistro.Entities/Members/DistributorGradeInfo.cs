namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorGradeInfo
    {
        
        private string _Description;
        
        private int _Discount;
        
        private int _GradeId;
        
        private string _Name;

        [StringLengthValidator(0, 300, Ruleset="ValDistributorGrade", MessageTemplate="备注的长度限制在300个字符以内"), HtmlCoding]
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

        [RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValDistributorGrade", MessageTemplate="等级折扣必须在1-100之间")]
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

        [StringLengthValidator(1, 60, Ruleset="ValDistributorGrade", MessageTemplate="分销商等级名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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
    }
}

