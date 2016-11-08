namespace Hidistro.Subsites.Data
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Subsites.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class StoreData : SubsiteStoreProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName, TradeDate, TradeType, Income, Balance, Remark, InpourId) VALUES (@UserId, @UserName, @TradeDate, @TradeType, @Income, @Balance, @Remark, @InpourId)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balanceDetails.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, balanceDetails.InpourId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override void AddHotkeywords(int categoryId, string Keywords)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Hotkeywords_Log");
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(storedProcCommand, "Keywords", DbType.String, Keywords);
            this.database.AddInParameter(storedProcCommand, "SearchTime", DbType.DateTime, DateTime.Now);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool AddInpourBlance(InpourRequestInfo inpourRequest)
        {
            if (null == inpourRequest)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_DistributorInpourRequest_Create");
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "InpourId", DbType.String, inpourRequest.InpourId);
            this.database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, inpourRequest.TradeDate);
            this.database.AddInParameter(storedProcCommand, "InpourBlance", DbType.Currency, inpourRequest.InpourBlance);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, inpourRequest.UserId);
            this.database.AddInParameter(storedProcCommand, "PaymentId", DbType.String, inpourRequest.PaymentId);
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool AddSiteRequest(SiteRequestInfo siteRequest)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_SiteRequest(UserId,FirstSiteUrl,RequestTime,RequestStatus,RefuseReason)VALUES(@UserId,@FirstSiteUrl,@RequestTime,@RequestStatus,@RefuseReason)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "FirstSiteUrl", DbType.String, siteRequest.FirstSiteUrl);
            this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, siteRequest.RequestTime);
            this.database.AddInParameter(sqlStringCommand, "RequestStatus", DbType.Int32, (int) siteRequest.RequestStatus);
            this.database.AddInParameter(sqlStringCommand, "RefuseReason", DbType.String, siteRequest.RefuseReason);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDrawRequest VALUES(@UserId,@UserName,@RequestTime,@Amount,@AccountName,@BankName,@MerchantCode)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDrawRequest.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDrawRequest.UserName);
            this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, balanceDrawRequest.RequestTime);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, balanceDrawRequest.Amount);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, balanceDrawRequest.MerchantCode);
            this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, balanceDrawRequest.BankName);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, balanceDrawRequest.AccountName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET IsUserHomeSiteApp = @IsUserHomeSiteApp, Topkey = @Topkey, TopSecret = @TopSecret WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "IsUserHomeSiteApp", DbType.Boolean, isUserHomeSiteApp);
            this.database.AddInParameter(sqlStringCommand, "Topkey", DbType.String, topkey);
            this.database.AddInParameter(sqlStringCommand, "TopSecret", DbType.String, topSecret);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            IUser user = HiContext.Current.User;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" AND UserId = {0}", user.UserId);
            if (query.TradeType != TradeTypes.NotSet)
            {
                builder.AppendFormat(" AND TradeType = {0}", (int) query.TradeType);
            }
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
        {
            if (null == friendlyLink)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_FriendlyLink_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "LinkId", DbType.Int32, friendlyLink.LinkId);
            }
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Votes_Create");
            this.database.AddInParameter(storedProcCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(storedProcCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            this.database.AddInParameter(storedProcCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
            this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
            this.database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
            if (dbTran == null)
            {
                return this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
        }

        public override void DeleteHotKeywords(int hId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" Delete FROM distro_Hotkeywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void DeleteSiteRequest()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_SiteRequest WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int DeleteVote(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteVoteItem(long voteId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public override bool DistroHasDrawRequest()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            return (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1);
        }

        public override int FriendlyLinkDelete(int linkId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_FriendlyLinks WHERE LinkId = @LinkId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
        {
            DistributorGradeInfo info = new DistributorGradeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulDistributorGrade(reader);
                }
            }
            return info;
        }

        public override IList<PaymentModeInfo> GetDistributorPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes where IsUseInDistributor=1 Order by DisplaySequence desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override FriendlyLinksInfo GetFriendlyLink(int linkId)
        {
            FriendlyLinksInfo info = new FriendlyLinksInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE LinkId=@LinkId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "LinkId", DbType.Int32, linkId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateFriendlyLink(reader));
                }
            }
            return list;
        }

        public override DataTable GetHotKeywords()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT Name FROM distro_Categories WHERE CategoryId = h.CategoryId AND DistributorUserId=@DistributorUserId)  AS CategoryName FROM distro_Hotkeywords h WHERE DistributorUserId=@DistributorUserId ORDER BY Frequency DESC");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override InpourRequestInfo GetInpouRequest(string inpourId)
        {
            InpourRequestInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    info = DataMapper.PopulateInpourRequest(reader);
                }
            }
            return info;
        }

        public override AccountSummaryInfo GetMyAccountSummary()
        {
            AccountSummaryInfo info = new AccountSummaryInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Amount) AS FreezeBalance FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId; SELECT TOP 1 Balance AS AccountAmount FROM Hishop_DistributorBalanceDetails WHERE UserId= @UserId ORDER BY JournalNumber DESC;");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read() && (DBNull.Value != reader["FreezeBalance"]))
                {
                    info.DrawRequestBalance = info.FreezeBalance = (decimal) reader["FreezeBalance"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["AccountAmount"]))
                {
                    info.AccountAmount = (decimal) reader["AccountAmount"];
                }
            }
            info.UseableBalance = info.AccountAmount - info.FreezeBalance;
            return info;
        }

        public override DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("select top {0} B.JournalNumber,B.UserId,B.UserName, B.TradeDate,B.TradeType,B.Income,B.Expenses,B.Balance,B.Remark", query.PageSize);
            builder.Append(" from Hishop_DistributorBalanceDetails B where 0=0 ");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY JournalNumber DESC", str);
            }
            else
            {
                builder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_DistributorBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", str);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                result.Data = DataHelper.ConverDataReaderToDataTable(reader);
                if (query.IsCount && reader.NextResult())
                {
                    reader.Read();
                    result.TotalRecords = reader.GetInt32(0);
                }
            }
            return result;
        }

        public override SiteRequestInfo GetMySiteRequest()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SiteRequest WHERE UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            SiteRequestInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulSiteRequest(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(string gateway)
        {
            PaymentModeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, gateway);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePayment(reader);
                }
            }
            return info;
        }

        public override IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_VoteItems WHERE VoteId = @VoteId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = distro_Votes.VoteId) AS VoteCounts FROM distro_Votes WHERE DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public override bool IsExitSiteUrl(string siteUrl)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_Settings WHERE SiteUrl = @SiteUrl");
            this.database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, siteUrl);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool IsRecharge(string inpourId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_DistributorBalanceDetails WHERE InpourId = @InpourId");
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override void RemoveInpourRequest(string inpourId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int SetVoteIsBackup(long voteId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Votes_IsBackup");
            this.database.AddInParameter(storedProcCommand, "VoteId", DbType.Int64, voteId);
            return this.database.ExecuteNonQuery(storedProcCommand);
        }

        public override void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
        }

        public override void SwapHotWordsSequence(int hid, int replaceHid, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("distro_Hotkeywords", "Hid", "Frequency", hid, replaceHid, displaySequence, replaceDisplaySequence);
        }

        public override void UpdateHotWords(int hid, int categoryId, string hotKeyWords)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update distro_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "Hid", DbType.Int32, hid);
            this.database.AddInParameter(sqlStringCommand, "CategoryId", DbType.Int32, categoryId);
            this.database.AddInParameter(sqlStringCommand, "Keywords", DbType.String, hotKeyWords);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateVote(VoteInfo vote, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Votes SET VoteName = @VoteName, IsBackup = @IsBackup, MaxCheck = @MaxCheck WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
            this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(sqlStringCommand, "IsBackup", DbType.Boolean, vote.IsBackup);
            this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
        }
    }
}

