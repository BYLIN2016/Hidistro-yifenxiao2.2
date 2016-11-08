namespace Hidistro.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Hidistro.Core;

    public abstract class MessageTemplateProvider
    {
        private static readonly MessageTemplateProvider DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.MessageTemplateData,Hidistro.Messages.Data") as MessageTemplateProvider);

        protected MessageTemplateProvider()
        {
        }

        public abstract MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId);
        public abstract IList<MessageTemplate> GetDistributorMessageTemplates();
        public abstract MessageTemplate GetMessageTemplate(string messageType);
        public abstract IList<MessageTemplate> GetMessageTemplates();
        public static MessageTemplateProvider Instance()
        {
            return DefaultInstance;
        }

        public static MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            MessageTemplate template2 = new MessageTemplate((string) reader["TagDescription"], (string) reader["Name"]);
            template2.MessageType = (string) reader["MessageType"];
            template2.SendInnerMessage = (bool) reader["SendInnerMessage"];
            template2.SendSMS = (bool) reader["SendSMS"];
            template2.SendEmail = (bool) reader["SendEmail"];
            template2.EmailSubject = (string) reader["EmailSubject"];
            template2.EmailBody = (string) reader["EmailBody"];
            template2.InnerMessageSubject = (string) reader["InnerMessageSubject"];
            template2.InnerMessageBody = (string) reader["InnerMessageBody"];
            template2.SMSBody = (string) reader["SMSBody"];
            return template2;
        }

        public abstract void UpdateDistributorSettings(IList<MessageTemplate> templates);
        public abstract void UpdateDistributorTemplate(MessageTemplate template);
        public abstract void UpdateSettings(IList<MessageTemplate> templates);
        public abstract void UpdateTemplate(MessageTemplate template);
    }
}

