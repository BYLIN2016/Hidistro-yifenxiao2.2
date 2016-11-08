namespace Hidistro.AccountCenter.Data
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ProfileData : PersonalMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            string query = "INSERT INTO Hishop_BalanceDetails(UserId, UserName, TradeDate, TradeType, Income, Balance,Remark) VALUES (@UserId, @UserName,@TradeDate, @TradeType, @Income, @Balance, @Remark)";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balanceDetails.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public override bool AddInpourBlance(InpourRequestInfo inpourRequest)
        {
            if (null == inpourRequest)
            {
                return false;
            }
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_InpourRequest_Create");
            this.database.AddInParameter(storedProcCommand, "InpourId", DbType.String, inpourRequest.InpourId);
            this.database.AddInParameter(storedProcCommand, "TradeDate", DbType.DateTime, inpourRequest.TradeDate);
            this.database.AddInParameter(storedProcCommand, "InpourBlance", DbType.Currency, inpourRequest.InpourBlance);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, inpourRequest.UserId);
            this.database.AddInParameter(storedProcCommand, "PaymentId", DbType.String, inpourRequest.PaymentId);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override int AddShippingAddress(ShippingAddressInfo shippingAddress)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,EmailAddress,TelPhone,CellPhone) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@EmailAddress,@TelPhone,@CellPhone); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, shippingAddress.UserId);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, shippingAddress.Address);
            this.database.AddInParameter(sqlStringCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
        }

        public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BalanceDrawRequest (UserId,UserName,RequestTime, Amount, AccountName, BankName, MerchantCode, Remark) VALUES (@UserId,@UserName,@RequestTime, @Amount, @AccountName, @BankName, @MerchantCode, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDrawRequest.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDrawRequest.UserName);
            this.database.AddInParameter(sqlStringCommand, "RequestTime", DbType.DateTime, balanceDrawRequest.RequestTime);
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, balanceDrawRequest.Amount);
            this.database.AddInParameter(sqlStringCommand, "AccountName", DbType.String, balanceDrawRequest.AccountName);
            this.database.AddInParameter(sqlStringCommand, "BankName", DbType.String, balanceDrawRequest.BankName);
            this.database.AddInParameter(sqlStringCommand, "MerchantCode", DbType.String, balanceDrawRequest.MerchantCode);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDrawRequest.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            if (query.TradeType != TradeTypes.NotSet)
            {
                builder.AppendFormat(" AND TradeType = {0}", (int) query.TradeType);
            }
            return builder.ToString();
        }

        public override bool CreateUpdateDeleteShippingAddress(ShippingAddressInfo shippingAddress, DataProviderAction action)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_ShippingAddress_CreateUpdateDelete");
            this.database.AddInParameter(storedProcCommand, "Action", DbType.Int32, (int) action);
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            if (action != DataProviderAction.Create)
            {
                this.database.AddInParameter(storedProcCommand, "ShippingId", DbType.Int32, shippingAddress.ShippingId);
            }
            if (action != DataProviderAction.Delete)
            {
                this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, shippingAddress.RegionId);
                this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, shippingAddress.UserId);
                this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, shippingAddress.ShipTo);
                this.database.AddInParameter(storedProcCommand, "Address", DbType.String, shippingAddress.Address);
                this.database.AddInParameter(storedProcCommand, "Zipcode", DbType.String, shippingAddress.Zipcode);
                this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, shippingAddress.TelPhone);
                this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, shippingAddress.CellPhone);
            }
            this.database.ExecuteNonQuery(storedProcCommand);
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return null;
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("SELECT TOP {0} * FROM Hishop_BalanceDetails B WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat(" {0} ORDER BY JournalNumber DESC;", str);
            }
            else
            {
                builder.AppendFormat(" AND JournalNumber < (SELECT MIN(JournalNumber) FROM (SELECT TOP {0} JournalNumber FROM Hishop_BalanceDetails WHERE 0=0 {1} ORDER BY JournalNumber DESC ) AS T) {1} ORDER BY JournalNumber DESC;", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(" SELECT COUNT(JournalNumber) AS Total from Hishop_BalanceDetails WHERE 0=0 {0}", str);
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

        public override InpourRequestInfo GetInpourBlance(string inpourId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_InpourRequest WHERE InpourId = @InpourId;");
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            InpourRequestInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateInpourRequest(reader);
                }
            }
            return info;
        }

        public override MemberGradeInfo GetMemberGrade(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            MemberGradeInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateMemberGrade(reader);
                }
            }
            return info;
        }

        public override IList<MemberGradeInfo> GetMemberGrades()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades");
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public override DbQueryResult GetMyReferralMembers(MemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ReferralUserId = {0}", HiContext.Current.User.UserId);
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId = {0}", query.GradeId.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override IList<ShippingAddressInfo> GetShippingAddress(int userId)
        {
            IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingAddress(reader));
                }
            }
            return list;
        }

        public override int GetShippingAddressCount(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(ShippingId) as Count FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            int num = 0;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = (int) reader["Count"];
                }
            }
            return num;
        }

        public override void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT COUNT(*) AS NoPayOrderNum FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus = {1};", HiContext.Current.User.UserId, 1);
            builder.AppendFormat(" SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = '{0}' AND IsRead=0 ;", HiContext.Current.User.Username);
            builder.AppendFormat(" SELECT COUNT(*) AS NoReplyLeaveCommentNum FROM Hishop_ProductConsultations WHERE UserId = {0} AND ViewDate is null AND ReplyUserId is not null;", HiContext.Current.User.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read() && (DBNull.Value != reader["NoPayOrderNum"]))
                {
                    num = (int) reader["NoPayOrderNum"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["NoReadMessageNum"]))
                {
                    num2 = (int) reader["NoReadMessageNum"];
                }
                if ((reader.NextResult() && reader.Read()) && (DBNull.Value != reader["NoReplyLeaveCommentNum"]))
                {
                    num3 = (int) reader["NoReplyLeaveCommentNum"];
                }
            }
            noPayOrderNum = num;
            noReadMessageNum = num2;
            noReplyLeaveCommentNum = num3;
        }

        public override ShippingAddressInfo GetUserShippingAddress(int shippingId)
        {
            ShippingAddressInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId");
            this.database.AddInParameter(sqlStringCommand, "ShippingId", DbType.Int32, shippingId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingAddress(reader);
                }
            }
            return info;
        }

        public override void RemoveInpourRequest(string inpourId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_InpourRequest WHERE InpourId = @InpourId");
            this.database.AddInParameter(sqlStringCommand, "InpourId", DbType.String, inpourId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool ViewProductConsultations()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" update Hishop_ProductConsultations set ViewDate=getdate() WHERE UserId = {0} AND ViewDate is null AND ReplyUserId is not null;", HiContext.Current.User.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

