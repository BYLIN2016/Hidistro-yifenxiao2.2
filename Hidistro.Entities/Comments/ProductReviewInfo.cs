namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewInfo
    {
        
        private int _ProductId;
        
        private DateTime _ReviewDate;
        
        private long _ReviewId;
        
        private string _ReviewText;
        
        private string _UserEmail;
        
        private int _UserId;
        
        private string _UserName;

        public int ProductId
        {
            
            get
            {
                return _ProductId;
            }
            
            set
            {
                _ProductId = value;
            }
        }

        public DateTime ReviewDate
        {
            
            get
            {
                return _ReviewDate;
            }
            
            set
            {
                _ReviewDate = value;
            }
        }

        public long ReviewId
        {
            
            get
            {
                return _ReviewId;
            }
            
            set
            {
                _ReviewId = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="评论内容为必填项，长度限制在300字符以内")]
        public string ReviewText
        {
            
            get
            {
                return _ReviewText;
            }
            
            set
            {
                _ReviewText = value;
            }
        }

        [StringLengthValidator(1, 0x100, Ruleset="Refer", MessageTemplate="邮箱不能为空，长度限制在256字符以内"), RegexValidator(@"^[a-zA-Z\.0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", Ruleset="Refer", MessageTemplate="邮箱地址必须为有效格式")]
        public string UserEmail
        {
            
            get
            {
                return _UserEmail;
            }
            
            set
            {
                _UserEmail = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="Refer", MessageTemplate="用户昵称为必填项，长度限制在30字符以内"), HtmlCoding]
        public string UserName
        {
            
            get
            {
                return _UserName;
            }
            
            set
            {
                _UserName = value;
            }
        }
    }
}

