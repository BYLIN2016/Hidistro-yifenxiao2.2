namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentInfo
    {
        
        private DateTime _LastDate;
        
        private long _LeaveId;
        
        private string _PublishContent;
        
        private DateTime _PublishDate;
        
        private string _Title;
        
        private int? _UserId;
        
        private string _UserName;

        public DateTime LastDate
        {
            
            get
            {
                return _LastDate;
            }
            
            set
            {
                _LastDate = value;
            }
        }

        public long LeaveId
        {
            
            get
            {
                return _LeaveId;
            }
            
            set
            {
                _LeaveId = value;
            }
        }

        [StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="留言内容为必填项，长度限制在300字符以内"), HtmlCoding]
        public string PublishContent
        {
            
            get
            {
                return _PublishContent;
            }
            
            set
            {
                _PublishContent = value;
            }
        }

        public DateTime PublishDate
        {
            
            get
            {
                return _PublishDate;
            }
            
            set
            {
                _PublishDate = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="Refer", MessageTemplate="标题为必填项，长度限制在60字符以内"), HtmlCoding]
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

        public int? UserId
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="Refer", MessageTemplate="用户名为必填项，长度限制在60字符以内")]
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

