namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [HasSelfValidation]
    public class CategoryInfo
    {
        
        private int? _AssociatedProductType;
        
        private int _CategoryId;
        
        private int _Depth;
        
        private int _DisplaySequence;
        
        private bool _HasChildren;
        
        private string _MetaDescription;
        
        private string _MetaKeywords;
        
        private string _MetaTitle;
        
        private string _Name;
        
        private string _Notes1;
        
        private string _Notes2;
        
        private string _Notes3;
        
        private string _Notes4;
        
        private string _Notes5;
        
        private int? _ParentCategoryId;
        
        private string _Path;
        
        private string _RewriteName;
        
        private string _SKUPrefix;
        
        private string _Theme;

        [SelfValidation(Ruleset="ValCategory")]
        public void CheckCategory(ValidationResults results)
        {
            if (!(string.IsNullOrEmpty(this.SKUPrefix) || ((this.SKUPrefix.Length <= 5) && Regex.IsMatch(this.SKUPrefix, "(?!_)(?!-)[a-zA-Z0-9_-]+"))))
            {
                results.AddResult(new ValidationResult("商家编码前缀长度限制在5个字符以内,只能以字母或数字开头", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.RewriteName) || ((this.RewriteName.Length <= 60) && Regex.IsMatch(this.RewriteName, "(^[-_a-zA-Z0-9]+$)"))))
            {
                results.AddResult(new ValidationResult("使用URL重写长度限制在60个字符以内，必须为字母数字-和_", this, "", "", null));
            }
        }

        public int? AssociatedProductType
        {
            
            get
            {
                return _AssociatedProductType;
            }
            
            set
            {
                _AssociatedProductType = value;
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

        public int Depth
        {
            
            get
            {
                return _Depth;
            }
            
            set
            {
                _Depth = value;
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

        public bool HasChildren
        {
            
            get
            {
                return _HasChildren;
            }
            
            set
            {
                _HasChildren = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内"), HtmlCoding]
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

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内")]
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

        [HtmlCoding, StringLengthValidator(0, 50, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的标题，长度限制在50个字符以内")]
        public string MetaTitle
        {
            
            get
            {
                return _MetaTitle;
            }
            
            set
            {
                _MetaTitle = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValCategory", MessageTemplate="分类名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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

        public string Notes1
        {
            
            get
            {
                return _Notes1;
            }
            
            set
            {
                _Notes1 = value;
            }
        }

        public string Notes2
        {
            
            get
            {
                return _Notes2;
            }
            
            set
            {
                _Notes2 = value;
            }
        }

        public string Notes3
        {
            
            get
            {
                return _Notes3;
            }
            
            set
            {
                _Notes3 = value;
            }
        }

        public string Notes4
        {
            
            get
            {
                return _Notes4;
            }
            
            set
            {
                _Notes4 = value;
            }
        }

        public string Notes5
        {
            
            get
            {
                return _Notes5;
            }
            
            set
            {
                _Notes5 = value;
            }
        }

        public int? ParentCategoryId
        {
            
            get
            {
                return _ParentCategoryId;
            }
            
            set
            {
                _ParentCategoryId = value;
            }
        }

        public string Path
        {
            
            get
            {
                return _Path;
            }
            
            set
            {
                _Path = value;
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

        [HtmlCoding]
        public string SKUPrefix
        {
            
            get
            {
                return _SKUPrefix;
            }
            
            set
            {
                _SKUPrefix = value;
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

        public int TopCategoryId
        {
            get
            {
                if (this.Depth == 1)
                {
                    return this.CategoryId;
                }
                return int.Parse(this.Path.Substring(0, this.Path.IndexOf("|")));
            }
        }
    }
}

