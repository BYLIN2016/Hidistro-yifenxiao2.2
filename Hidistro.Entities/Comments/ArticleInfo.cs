namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleInfo
    {
        
        private DateTime _AddedDate;
        
        private int _ArticleId;
        
        private int _CategoryId;
        
        private string _CategoryName;
        
        private string _Content;
        
        private string _Description;
        
        private string _IconUrl;
        
        private bool _IsRelease;
        
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

        public int ArticleId
        {
            
            get
            {
                return _ArticleId;
            }
            
            set
            {
                _ArticleId = value;
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

        public string CategoryName
        {
            
            get
            {
                return _CategoryName;
            }
            
            set
            {
                _CategoryName = value;
            }
        }

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValArticleInfo", MessageTemplate="文章内容不能为空")]
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

        [StringLengthValidator(0, 300, Ruleset="ValArticleInfo", MessageTemplate="文章摘要的长度限制在300个字符以内"), HtmlCoding]
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

        public bool IsRelease
        {
            
            get
            {
                return _IsRelease;
            }
            
            set
            {
                _IsRelease = value;
            }
        }

        [StringLengthValidator(0, 260, Ruleset="ValArticleInfo", MessageTemplate="告诉搜索引擎此文章页面的主要内容，长度限制在260个字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 160, Ruleset="ValArticleInfo", MessageTemplate="让用户可以通过搜索引擎搜索到此文章的浏览页面，长度限制在160个字符以内"), HtmlCoding]
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValArticleInfo", MessageTemplate="文章标题不能为空，长度限制在60个字符以内")]
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

