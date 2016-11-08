namespace Hidistro.Entities.Comments
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageBoxInfo
    {
        
        private string _Accepter;
        
        private string _Content;
        
        private long _ContentId;
        
        private DateTime _Date;
        
        private bool _IsRead;
        
        private long _MessageId;
        
        private string _Sernder;
        
        private string _Title;

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

        public long ContentId
        {
            
            get
            {
                return _ContentId;
            }
            
            set
            {
                _ContentId = value;
            }
        }

        public DateTime Date
        {
            
            get
            {
                return _Date;
            }
            
            set
            {
                _Date = value;
            }
        }

        public bool IsRead
        {
            
            get
            {
                return _IsRead;
            }
            
            set
            {
                _IsRead = value;
            }
        }

        public long MessageId
        {
            
            get
            {
                return _MessageId;
            }
            
            set
            {
                _MessageId = value;
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

