namespace Hidistro.Messages.Data
{
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class MessageTemplateData : MessageTemplateProvider
    {
        private readonly Database database = DatabaseFactory.CreateDatabase();

        public override MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, distributorUserId);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = MessageTemplateProvider.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public override IList<MessageTemplate> GetDistributorMessageTemplates()
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MessageTemplates WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(MessageTemplateProvider.PopulateEmailTempletFromIDataReader(reader));
                }
                reader.Close();
            }
            return list;
        }

        public override MessageTemplate GetMessageTemplate(string messageType)
        {
            MessageTemplate template = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, messageType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    template = MessageTemplateProvider.PopulateEmailTempletFromIDataReader(reader);
                }
                reader.Close();
            }
            return template;
        }

        public override IList<MessageTemplate> GetMessageTemplates()
        {
            IList<MessageTemplate> list = new List<MessageTemplate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(MessageTemplateProvider.PopulateEmailTempletFromIDataReader(reader));
                }
                reader.Close();
            }
            return list;
        }

        public override void UpdateDistributorSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override void UpdateDistributorTemplate(MessageTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, template.EmailSubject);
            this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, template.EmailBody);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", DbType.String, template.InnerMessageSubject);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", DbType.String, template.InnerMessageBody);
            this.database.AddInParameter(sqlStringCommand, "SMSBody", DbType.String, template.SMSBody);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, template.MessageType);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateSettings(IList<MessageTemplate> templates)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "SendEmail", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendSMS", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", DbType.Boolean);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String);
            foreach (MessageTemplate template in templates)
            {
                this.database.SetParameterValue(sqlStringCommand, "SendEmail", template.SendEmail);
                this.database.SetParameterValue(sqlStringCommand, "SendSMS", template.SendSMS);
                this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", template.SendInnerMessage);
                this.database.SetParameterValue(sqlStringCommand, "MessageType", template.MessageType);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override void UpdateTemplate(MessageTemplate template)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType)");
            this.database.AddInParameter(sqlStringCommand, "EmailSubject", DbType.String, template.EmailSubject);
            this.database.AddInParameter(sqlStringCommand, "EmailBody", DbType.String, template.EmailBody);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", DbType.String, template.InnerMessageSubject);
            this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", DbType.String, template.InnerMessageBody);
            this.database.AddInParameter(sqlStringCommand, "SMSBody", DbType.String, template.SMSBody);
            this.database.AddInParameter(sqlStringCommand, "MessageType", DbType.String, template.MessageType);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

