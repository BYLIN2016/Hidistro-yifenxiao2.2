namespace Hidistro.Entities.Comments
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentReplyInfo
    {
        
        private long _LeaveId;
        
        private string _ReplyContent;
        
        private DateTime _ReplyDate;
        
        private long _ReplyId;
        
        private int _UserId;

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

        [NotNullValidator(Ruleset="ValLeaveCommentReply", MessageTemplate="回复内容不能为空")]
        public string ReplyContent
        {
            
            get
            {
                return _ReplyContent;
            }
            
            set
            {
                _ReplyContent = value;
            }
        }

        public DateTime ReplyDate
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

        public long ReplyId
        {
            
            get
            {
                return _ReplyId;
            }
            
            set
            {
                _ReplyId = value;
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
    }
}

