namespace Hidistro.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net.Mail;
    using Hidistro.Core;

    public abstract class EmailQueueProvider
    {
        private static readonly EmailQueueProvider DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.EmailQueueData,Hidistro.Messages.Data") as EmailQueueProvider);

        protected EmailQueueProvider()
        {
        }

        public abstract void DeleteDistributorQueuedEmail(Guid emailId);
        public abstract void DeleteQueuedEmail(Guid emailId);
        public abstract Dictionary<Guid, SubsiteMailMessage> DequeueDistributorEmail();
        public abstract Dictionary<Guid, MailMessage> DequeueEmail();
        public static EmailQueueProvider Instance()
        {
            return DefaultInstance;
        }

        public static MailMessage PopulateEmailFromIDataReader(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            try
            {
                MailMessage message2 = new MailMessage();
                message2.Priority = (MailPriority) ((int) reader["EmailPriority"]);
                message2.IsBodyHtml = (bool) reader["IsBodyHtml"];
                MailMessage message = message2;
                if (reader["EmailSubject"] != DBNull.Value)
                {
                    message.Subject = (string) reader["EmailSubject"];
                }
                if (reader["EmailTo"] != DBNull.Value)
                {
                    message.To.Add((string) reader["EmailTo"]);
                }
                if (reader["EmailBody"] != DBNull.Value)
                {
                    message.Body = (string) reader["EmailBody"];
                }
                if (reader["EmailCc"] != DBNull.Value)
                {
                    string[] strArray = ((string) reader["EmailCc"]).Split(new char[] { ',' });
                    foreach (string str in strArray)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            message.CC.Add(new MailAddress(str));
                        }
                    }
                }
                if (reader["EmailBcc"] != DBNull.Value)
                {
                    string[] strArray2 = ((string) reader["EmailBcc"]).Split(new char[] { ',' });
                    foreach (string str2 in strArray2)
                    {
                        if (!string.IsNullOrEmpty(str2))
                        {
                            message.Bcc.Add(new MailAddress(str2));
                        }
                    }
                }
                return message;
            }
            catch
            {
                return null;
            }
        }

        public abstract void QueueDistributorEmail(MailMessage message, int userId);
        public abstract void QueueDistributorSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries);
        public abstract void QueueEmail(MailMessage message);
        public abstract void QueueSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries);
    }
}

