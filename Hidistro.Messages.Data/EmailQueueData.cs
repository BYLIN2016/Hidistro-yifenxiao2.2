namespace Hidistro.Messages.Data
{
    using Hidistro.Core;
    using Hidistro.Messages;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Net.Mail;

    public class EmailQueueData : EmailQueueProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override void DeleteDistributorQueuedEmail(Guid emailId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_EmailQueue WHERE EmailId = @EmailId");
            this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeleteQueuedEmail(Guid emailId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_EmailQueue WHERE EmailId = @EmailId");
            this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, emailId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override Dictionary<Guid, SubsiteMailMessage> DequeueDistributorEmail()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_EmailQueue WHERE NextTryTime < getdate()");
            Dictionary<Guid, SubsiteMailMessage> dictionary = new Dictionary<Guid, SubsiteMailMessage>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    MailMessage mail = EmailQueueProvider.PopulateEmailFromIDataReader(reader);
                    if (mail != null)
                    {
                        dictionary.Add((Guid) reader["EmailId"], new SubsiteMailMessage((int) reader["UserId"], mail));
                    }
                    else
                    {
                        this.DeleteDistributorQueuedEmail((Guid) reader["EmailId"]);
                    }
                }
                reader.Close();
            }
            return dictionary;
        }

        public override Dictionary<Guid, MailMessage> DequeueEmail()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_EmailQueue WHERE NextTryTime < getdate()");
            Dictionary<Guid, MailMessage> dictionary = new Dictionary<Guid, MailMessage>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    MailMessage message = EmailQueueProvider.PopulateEmailFromIDataReader(reader);
                    if (message != null)
                    {
                        dictionary.Add((Guid) reader["EmailId"], message);
                    }
                    else
                    {
                        this.DeleteQueuedEmail((Guid) reader["EmailId"]);
                    }
                }
                reader.Close();
            }
            return dictionary;
        }

        public override void QueueDistributorEmail(MailMessage message, int userId)
        {
            if (message != null)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_EmailQueue(UserId, EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@UserId, @EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, Guid.NewGuid());
                this.database.AddInParameter(sqlStringCommand, "EmailTo", DbType.String, Globals.ToDelimitedString(message.To, ","));
                if (message.CC != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, Globals.ToDelimitedString(message.CC, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, DBNull.Value);
                }
                if (message.Bcc != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, Globals.ToDelimitedString(message.Bcc, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, DBNull.Value);
                }
                this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, message.Subject);
                this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, message.Body);
                this.database.AddInParameter(sqlStringCommand, "EmailPriority", DbType.Int32, (int) message.Priority);
                this.database.AddInParameter(sqlStringCommand, "IsBodyHtml", DbType.Boolean, message.IsBodyHtml);
                this.database.AddInParameter(sqlStringCommand, "NextTryTime", DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
                this.database.AddInParameter(sqlStringCommand, "NumberOfTries", DbType.Int32, 0);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override void QueueDistributorSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_EmailQueue_Failure");
            this.database.AddInParameter(storedProcCommand, "EmailId", DbType.Guid);
            this.database.AddInParameter(storedProcCommand, "FailureInterval", DbType.Int32, failureInterval);
            this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", DbType.Int32, maxNumberOfTries);
            foreach (Guid guid in list)
            {
                storedProcCommand.Parameters[0].Value = guid;
                this.database.ExecuteNonQuery(storedProcCommand);
            }
        }

        public override void QueueEmail(MailMessage message)
        {
            if (message != null)
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_EmailQueue(EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
                this.database.AddInParameter(sqlStringCommand, "EmailId", DbType.Guid, Guid.NewGuid());
                this.database.AddInParameter(sqlStringCommand, "EmailTo", DbType.String, Globals.ToDelimitedString(message.To, ","));
                if (message.CC != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, Globals.ToDelimitedString(message.CC, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailCc", DbType.String, DBNull.Value);
                }
                if (message.Bcc != null)
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, Globals.ToDelimitedString(message.Bcc, ","));
                }
                else
                {
                    this.database.AddInParameter(sqlStringCommand, "EmailBcc", DbType.String, DBNull.Value);
                }
                this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, message.Subject);
                this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, message.Body);
                this.database.AddInParameter(sqlStringCommand, "EmailPriority", DbType.Int32, (int) message.Priority);
                this.database.AddInParameter(sqlStringCommand, "IsBodyHtml", DbType.Boolean, message.IsBodyHtml);
                this.database.AddInParameter(sqlStringCommand, "NextTryTime", DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
                this.database.AddInParameter(sqlStringCommand, "NumberOfTries", DbType.Int32, 0);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override void QueueSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_EmailQueue_Failure");
            this.database.AddInParameter(storedProcCommand, "EmailId", DbType.Guid);
            this.database.AddInParameter(storedProcCommand, "FailureInterval", DbType.Int32, failureInterval);
            this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", DbType.Int32, maxNumberOfTries);
            foreach (Guid guid in list)
            {
                storedProcCommand.Parameters[0].Value = guid;
                this.database.ExecuteNonQuery(storedProcCommand);
            }
        }
    }
}

