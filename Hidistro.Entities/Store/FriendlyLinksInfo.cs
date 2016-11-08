namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendlyLinksInfo
    {
        
        private int _DisplaySequence;
        
        private string _ImageUrl;
        
        private int? _LinkId;
        
        private string _LinkUrl;
        
        private string _Title;
        
        private bool _Visible;

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

        public string ImageUrl
        {
            
            get
            {
                return _ImageUrl;
            }
            
            set
            {
                _ImageUrl = value;
            }
        }

        public int? LinkId
        {
            
            get
            {
                return _LinkId;
            }
            
            set
            {
                _LinkId = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValFriendlyLinksInfo", MessageTemplate="网站地址必须为有效格式"), RegexValidator(@"^(http://).*[\.]+.*", Ruleset="ValFriendlyLinksInfo"), StringLengthValidator(0, Ruleset="ValFriendlyLinksInfo"), IgnoreNulls]
        public string LinkUrl
        {
            
            get
            {
                return _LinkUrl;
            }
            
            set
            {
                _LinkUrl = value;
            }
        }

        [StringLengthValidator(0, 60, Ruleset="ValFriendlyLinksInfo", MessageTemplate="网站名称长度限制在60个字符以内")]
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

        public bool Visible
        {
            
            get
            {
                return _Visible;
            }
            
            set
            {
                _Visible = value;
            }
        }
    }
}

