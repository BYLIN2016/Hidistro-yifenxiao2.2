namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationInfo
    {
        
        private DateTime _ConsultationDate;
        
        private int _ConsultationId;
        
        private string _ConsultationText;
        
        private int _ProductId;
        
        private DateTime? _ReplyDate;
        
        private string _ReplyText;
        
        private int? _ReplyUserId;
        
        private string _UserEmail;
        
        private int _UserId;
        
        private string _UserName;

        public DateTime ConsultationDate
        {
            
            get
            {
                return _ConsultationDate;
            }
            
            set
            {
                _ConsultationDate = value;
            }
        }

        public int ConsultationId
        {
            
            get
            {
                return _ConsultationId;
            }
            
            set
            {
                _ConsultationId = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="咨询内容为必填项，长度限制在300字符以内")]
        public string ConsultationText
        {
            
            get
            {
                return _ConsultationText;
            }
            
            set
            {
                _ConsultationText = value;
            }
        }

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

        public DateTime? ReplyDate
        {
            
            get
            {
                return _ReplyDate;
            }
            
            set
            {
                _ReplyDate = value;
            }
        }

        [NotNullValidator(Ruleset="Reply", MessageTemplate="回内容为必填项")]
        public string ReplyText
        {
            
            get
            {
                return _ReplyText;
            }
            
            set
            {
                _ReplyText = value;
            }
        }

        public int? ReplyUserId
        {
            
            get
            {
                return _ReplyUserId;
            }
            
            set
            {
                _ReplyUserId = value;
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

        [HtmlCoding, StringLengthValidator(1, 30, Ruleset="Refer", MessageTemplate="用户昵称为必填项，长度限制在30字符以内")]
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

