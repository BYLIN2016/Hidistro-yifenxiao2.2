namespace Hidistro.Messages.Data
{
    using Hidistro.Messages;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class InnerMessageData : InnerMessageProvider
    {
        private readonly Database database = DatabaseFactory.CreateDatabase();

        public override bool SendDistributorMessage(string subject, string message, string distributor, string sendto)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,@Sernder ,@Accepter,0)");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, message);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Sernder", DbType.String, distributor);
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, sendto);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool SendMessage(string subject, string message, string sendto)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,'admin' ,@Accepter,0)");
            this.database.AddInParameter(sqlStringCommand, "Title", DbType.String, subject);
            this.database.AddInParameter(sqlStringCommand, "Content", DbType.String, message);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "Accepter", DbType.String, sendto);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }
    }
}

