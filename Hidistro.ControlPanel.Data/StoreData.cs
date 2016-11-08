namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;

    public class StoreData : StoreProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override void AddHotkeywords(int categoryId, string Keywords)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Hotkeywords_Log");
            this.database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public override string BackupData(string path)
        {
            string database;
            using (DbConnection connection = this.database.CreateConnection())
            {
                database = connection.Database;
            }
            string str2 = database + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", database, path + str2));
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return str2;
            }
            catch
            {
                return string.Empty;
            }
        }

        public override void ClearRolePrivilege(Guid roleId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT UserName FROM vw_aspnet_Managers WHERE UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", roleId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    RoleHelper.SignOut((string) reader["UserName"]);
                }
            }
        }

        public override bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_FriendlyLink_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "LinkId", DbType.Int32, friendlyLink.LinkId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "ImageUrl", DbType.String, friendlyLink.ImageUrl);
                this.database.AddInParameter(storedProcCommand, "LinkUrl", DbType.String, friendlyLink.LinkUrl);
                this.database.AddInParameter(storedProcCommand, "Title", DbType.String, friendlyLink.Title);
                this.database.AddInParameter(storedProcCommand, "Visible", DbType.Boolean, friendlyLink.Visible);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override long CreateVote(VoteInfo vote)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_Create");
            this.database.AddInParameter(storedProcCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(storedProcCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            this.database.AddInParameter(storedProcCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddOutParameter(storedProcCommand, "VoteId", DbType.Int64, 8);
            long parameterValue = 0L;
            if (this.database.ExecuteNonQuery(storedProcCommand) > 0)
            {
                parameterValue = (long) this.database.GetParameterValue(storedProcCommand, "VoteId");
            }
            return parameterValue;
        }

        public override int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
            this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
            this.database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
            if (dbTran == null)
            {
                return this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
        }

        public override bool DeleteAllLogs()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
            try
            {
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override void DeleteHotKeywords(int hId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" Delete FROM Hishop_Hotkeywords Where Hid =@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteLog(long logId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Logs WHERE LogId = @LogId");
            this.database.AddInParameter(sqlStringCommand, "LogId", DbType.Int64, logId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override int DeleteLogs(string strIds)
        {
            if (strIds.Length <= 0)
            {
                return 0;
            }
            string query = string.Format("DELETE FROM Hishop_Logs WHERE LogId IN ({0})", strIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteManager(int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Manager_Delete");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            this.database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
            return (((parameterValue != null) && (parameterValue != DBNull.Value)) && (Convert.ToInt32(parameterValue) == 0));
        }

        public override void DeleteQueuedCellphone(Guid cellphoneId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CellphoneQueue WHERE CellphoneId = @CellphoneId");
            this.database.AddInParameter(sqlStringCommand, "CellphoneId", DbType.Guid, cellphoneId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteVote(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Votes WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteVoteItem(long voteId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public override DataTable DequeueCellphone()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CellphoneQueue WHERE NextTryTime < getdate()");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void EnqueuCellphone(string cellphoneNumber, string subject)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CellphoneQueue (CellphoneId, CellphoneNumber, Subject, NextTryTime, NumberOfTries)VALUES(newid(), @CellphoneNumber,@Subject, getdate(), 0)");
            this.database.AddInParameter(sqlStringCommand, "CellphoneNumber", DbType.String, cellphoneNumber);
            this.database.AddInParameter(sqlStringCommand, "Subject", DbType.String, subject);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int FriendlyLinkDelete(int linkId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_FriendlyLinks WHERE linkid = @linkid");
            this.database.AddInParameter(sqlStringCommand, "Linkid", DbType.Int32, linkId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            FriendlyLinksInfo info = new FriendlyLinksInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks WHERE LinkId=@LinkId");
            this.database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateFriendlyLink(reader);
                }
            }
            return info;
        }

        public override IList<FriendlyLinksInfo> GetFriendlyLinks()
        {
            IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks ORDER BY DisplaySequence DESC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateFriendlyLink(reader));
                }
            }
            return list;
        }

        public override string GetHotkeyword(int id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Keywords FROM Hishop_Hotkeywords WHERE Hid=@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, id);
            return this.database.ExecuteScalar(sqlStringCommand).ToString();
        }

        public override DataTable GetHotKeywords()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Hishop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Hishop_Hotkeywords h ORDER BY Frequency DESC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DbQueryResult GetLogs(OperationLogQuery query)
        {
            StringBuilder builder = new StringBuilder();
            Pagination page = query.Page;
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat("AddedTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" AddedTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            if (!string.IsNullOrEmpty(query.OperationUserName))
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                {
                    builder.Append(" AND");
                }
                builder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
            }
            return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", builder.ToString(), "*");
        }

        public override DbQueryResult GetManagers(ManagerQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            if (query.RoleId != Guid.Empty)
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", query.RoleId);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Managers", "UserId", builder.ToString(), "*");
        }

        public override IList<string> GetOperationUserNames()
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM Hishop_Logs");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(reader["UserName"].ToString());
                }
            }
            return list;
        }

        public override VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            VoteInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateVote(reader);
                }
            }
            return info;
        }

        public override int GetVoteCounts(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            IList<VoteItemInfo> list = new List<VoteItemInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                VoteItemInfo item = null;
                while (reader.Read())
                {
                    item = DataMapper.PopulateVoteItem(reader);
                    list.Add(item);
                }
            }
            return list;
        }

        public override DataSet GetVotes()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = Hishop_Votes.VoteId) AS VoteCounts FROM Hishop_Votes");
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override void QueueSendingFailure(IList<Guid> cellphoneIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CellphoneQueue SET NextTryTime = getdate(), NumberOfTries = NumberOfTries +1 WHERE CellphoneId = @CellphoneId DELETE FROM Hishop_CellphoneQueue WHERE NumberOfTries > 10");
            this.database.AddInParameter(sqlStringCommand, "CellphoneId", DbType.Guid);
            foreach (Guid guid in cellphoneIds)
            {
                this.database.SetParameterValue(sqlStringCommand, "CellphoneId", guid);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public override void Restor()
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
            catch
            {
            }
        }

        public override bool RestoreData(string bakFullName)
        {
            string dataSource;
            string database;
            bool flag;
            using (DbConnection connection = this.database.CreateConnection())
            {
                database = connection.Database;
                dataSource = connection.DataSource;
            }
            SqlConnection connection2 = new SqlConnection(string.Format("Data Source={0};Initial Catalog=master;Integrated Security=SSPI", dataSource));
            try
            {
                connection2.Open();
                SqlCommand command = new SqlCommand(string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", database), connection2);
                ArrayList list = new ArrayList();
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetInt16(0));
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    new SqlCommand(string.Format("KILL {0}", list[i].ToString()), connection2).ExecuteNonQuery();
                }
                new SqlCommand(string.Format("RESTORE DATABASE [{0}]  FROM DISK = '{1}' WITH REPLACE", database, bakFullName), connection2).ExecuteNonQuery();
                flag = true;
            }
            catch
            {
                flag = false;
            }
            finally
            {
                connection2.Close();
            }
            return flag;
        }

        public override int SetVoteIsBackup(long voteId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_IsBackup");
            this.database.AddInParameter(storedProcCommand, "VoteId", DbType.Int64, voteId);
            return this.database.ExecuteNonQuery(storedProcCommand);
        }

        private string StringCut(string str, string bg, string ed)
        {
            string str2 = str.Substring(str.IndexOf(bg) + bg.Length);
            return str2.Substring(0, str2.IndexOf(ed));
        }

        public override void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_Hotkeywords", "Hid", "Frequency", hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateVote(VoteInfo vote, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Votes SET VoteName = @VoteName, MaxCheck = @MaxCheck WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
        }

        public override void WriteOperationLogEntry(OperationLogEntry entry)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
            this.database.AddInParameter(sqlStringCommand, "PageUrl", DbType.String, entry.PageUrl);
            this.database.AddInParameter(sqlStringCommand, "AddedTime", DbType.DateTime, entry.AddedTime);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, entry.UserName);
            this.database.AddInParameter(sqlStringCommand, "IPAddress", DbType.String, entry.IpAddress);
            this.database.AddInParameter(sqlStringCommand, "Privilege", DbType.Int32, (int) entry.Privilege);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, entry.Description);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

