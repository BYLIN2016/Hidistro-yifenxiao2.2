namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class HelpInfo
    {
        
        private DateTime _AddedDate;
        
        private int _CategoryId;
        
        private string _Content;
        
        private string _Description;
        
        private int _HelpId;
        
        private bool _IsShowFooter;
        
        private string _MetaDescription;
        
        private string _MetaKeywords;
        
        private string _Title;

        public DateTime AddedDate
        {
            
            get
            {
                return _AddedDate;
            }
            
            set
            {
                _AddedDate = value;
            }
        }

        public int CategoryId
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

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValHelpInfo", MessageTemplate="帮助内容不能为空")]
        public string Content
        {
            
            get
            {
                return _Content;
            }
            
            set
            {
                _Content = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValHelpInfo", MessageTemplate="摘要的长度限制在300个字符以内"), HtmlCoding]
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

        public int HelpId
        {
            
            get
            {
                return _HelpId;
            }
            
            set
            {
                _HelpId = value;
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

        [HtmlCoding, StringLengthValidator(0, 260, Ruleset="ValHelpInfo", MessageTemplate="告诉搜索引擎此帮助页面的主要内容，长度限制在260个字符以内")]
        public string MetaDescription
        {
            
            get
            {
                return _MetaDescription;
            }
            
            set
            {
                _MetaDescription = value;
            }
        }

        [StringLengthValidator(0, 160, Ruleset="ValHelpInfo", MessageTemplate="让用户可以通过搜索引擎搜索到此帮助的浏览页面，长度限制在160个字符以内"), HtmlCoding]
        public string MetaKeywords
        {
            
            get
            {
                return _MetaKeywords;
            }
            
            set
            {
                _MetaKeywords = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValHelpInfo", MessageTemplate="帮助主题不能为空，长度限制在60个字符以内"), HtmlCoding]
        public string Title
        {
            
            get
            {
                return _Title;
            }
            
            set
            {
                _Title = value;
            }
        }
    }
}

