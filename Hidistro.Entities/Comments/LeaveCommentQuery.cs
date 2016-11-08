namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentQuery : Pagination
    {
        
        private int? _AgentId;
        
        private Hidistro.Entities.Comments.MessageStatus _MessageStatus;

        public int? AgentId
        {
            
            get
            {
                return _AgentId;
            }
            
            set
            {
                _AgentId = value;
            }
        }

        public Hidistro.Entities.Comments.MessageStatus MessageStatus
        {
            
            get
            {
                return _MessageStatus;
            }
            
            set
            {
                _MessageStatus = value;
            }
        }
    }
}

