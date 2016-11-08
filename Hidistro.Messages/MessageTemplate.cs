namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageTemplate
    {
        
        private string _EmailBody;
        
        private string _EmailSubject;
        
        private string _InnerMessageBody;
        
        private string _InnerMessageSubject;
        
        private string _MessageType;
        
        private string _Name;
        
        private bool _SendEmail;
        
        private bool _SendInnerMessage;
        
        private bool _SendSMS;
        
        private string _SMSBody;
        
        private string _TagDescription;

        public MessageTemplate()
        {
        }

        public MessageTemplate(string tagDescription, string name)
        {
            this.TagDescription = tagDescription;
            this.Name = name;
        }

        public string EmailBody
        {
            
            get
            {
                return _EmailBody;
            }
            
            set
            {
                _EmailBody = value;
            }
        }

        public string EmailSubject
        {
            
            get
            {
                return _EmailSubject;
            }
            
            set
            {
                _EmailSubject = value;
            }
        }

        public string InnerMessageBody
        {
            
            get
            {
                return _InnerMessageBody;
            }
            
            set
            {
                _InnerMessageBody = value;
            }
        }

        public string InnerMessageSubject
        {
            
            get
            {
                return _InnerMessageSubject;
            }
            
            set
            {
                _InnerMessageSubject = value;
            }
        }

        public string MessageType
        {
            
            get
            {
                return _MessageType;
            }
            
            set
            {
                _MessageType = value;
            }
        }

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            private set
            {
                _Name = value;
            }
        }

        public bool SendEmail
        {
            
            get
            {
                return _SendEmail;
            }
            
            set
            {
                _SendEmail = value;
            }
        }

        public bool SendInnerMessage
        {
            
            get
            {
                return _SendInnerMessage;
            }
            
            set
            {
                _SendInnerMessage = value;
            }
        }

        public bool SendSMS
        {
            
            get
            {
                return _SendSMS;
            }
            
            set
            {
                _SendSMS = value;
            }
        }

        public string SMSBody
        {
            
            get
            {
                return _SMSBody;
            }
            
            set
            {
                _SMSBody = value;
            }
        }

        public string TagDescription
        {
            
            get
            {
                return _TagDescription;
            }
            
            private set
            {
                _TagDescription = value;
            }
        }
    }
}

