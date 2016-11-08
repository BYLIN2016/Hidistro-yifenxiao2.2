namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class HelpCategoryInfo
    {
        
        private int? _CategoryId;
        
        private string _Description;
        
        private int _DisplaySequence;
        
        private string _IconUrl;
        
        private string _IndexChar;
        
        private bool _IsShowFooter;
        
        private string _Name;

        public int? CategoryId
        {
            
            get
            {
                return _CategoryId;
            }
            
            set
            {
                _CategoryId = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="ValHelpCategoryInfo", MessageTemplate="分类介绍最多只能输入300个字符")]
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

        public string IconUrl
        {
            
            get
            {
                return _IconUrl;
            }
            
            set
            {
                _IconUrl = value;
            }
        }

        public string IndexChar
        {
            
            get
            {
                return _IndexChar;
            }
            
            set
            {
                _IndexChar = value;
            }
        }

        public bool IsShowFooter
        {
            
            get
            {
                return _IsShowFooter;
            }
            
            set
            {
                _IsShowFooter = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValHelpCategoryInfo", MessageTemplate="分类名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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

