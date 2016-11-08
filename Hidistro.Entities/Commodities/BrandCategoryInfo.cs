namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BrandCategoryInfo
    {
        
        private int _BrandId;
        
        private string _BrandName;
        
        private string _CompanyUrl;
        
        private string _Description;
        
        private int _DisplaySequence;
        
        private string _Logo;
        
        private string _MetaDescription;
        
        private string _MetaKeywords;
        
        private string _RewriteName;
        
        private string _Theme;
        private IList<int> productTypes;

        public int BrandId
        {
            
            get
            {
                return _BrandId;
            }
            
            set
            {
                _BrandId = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="ValBrandCategory", MessageTemplate="品牌名称不能为空，长度限制在30个字符以内")]
        public string BrandName
        {
            
            get
            {
                return _BrandName;
            }
            
            set
            {
                _BrandName = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValBrandCategory", MessageTemplate="品牌官方网站的网址必须以http://开头，长度限制在100个字符以内"), RegexValidator("^(http)://.*", Ruleset="ValBrandCategory"), NotNullValidator(Negated=true, Ruleset="ValBrandCategory")]
        public string CompanyUrl
        {
            
            get
            {
                return _CompanyUrl;
            }
            
            set
            {
                _CompanyUrl = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValCategory", MessageTemplate="品牌介绍的长度限制在300个字符以内")]
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

        public string Logo
        {
            
            get
            {
                return _Logo;
            }
            
            set
            {
                _Logo = value;
            }
        }

        [StringLengthValidator(0, 260, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在260个字符以内"), HtmlCoding]
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

        [HtmlCoding, StringLengthValidator(0, 160, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在160个字符以内")]
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

        public IList<int> ProductTypes
        {
            get
            {
                if (this.productTypes == null)
                {
                    this.productTypes = new List<int>();
                }
                return this.productTypes;
            }
            set
            {
                this.productTypes = value;
            }
        }

        public string RewriteName
        {
            
            get
            {
                return _RewriteName;
            }
            
            set
            {
                _RewriteName = value;
            }
        }

        public string Theme
        {
            
            get
            {
                return _Theme;
            }
            
            set
            {
                _Theme = value;
            }
        }
    }
}

