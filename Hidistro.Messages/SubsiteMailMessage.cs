namespace Hidistro.Messages
{
    using System;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;

    public class SubsiteMailMessage
    {
        
        private int _DistributorUserId;
        
        private MailMessage _Mail;

        public SubsiteMailMessage(int distributorUserId, MailMessage mail)
        {
            this.Mail = mail;
            this.DistributorUserId = distributorUserId;
        }

        public int DistributorUserId
        {
            
            get
            {
                return _DistributorUserId;
            }
            
            private set
            {
                _DistributorUserId = value;
            }
        }

        public MailMessage Mail
        {
            
            get
            {
                return _Mail;
            }
            
            private set
            {
                _Mail = value;
            }
        }
    }
}

