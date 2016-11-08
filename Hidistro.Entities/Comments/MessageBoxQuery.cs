namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MessageBoxQuery : Pagination
    {
        
        private string _Accepter;
        
        private Hidistro.Entities.Comments.MessageStatus _MessageStatus;
        
        private string _Sernder;

        public string Accepter
        {
            
            get
            {
                return _Accepter;
            }
            
            set
            {
                _Accepter = value;
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

        public string Sernder
        {
            
            get
            {
                return _Sernder;
            }
            
            set
            {
                _Sernder = value;
            }
        }
    }
}

