namespace Hidistro.Subsites.Data
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class UnderlingData : UnderlingProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_BalanceDetails (UserId,UserName, DistributorUserId, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId,@UserName, @DistributorUserId, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balanceDetails.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balanceDetails.Expenses);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" AND DistributorUserId = {0}", HiContext.Current.User.UserId);
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
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

        private static string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" AND DistributorUserId = {0}", HiContext.Current.User.UserId);
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_distro_Members WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.FromDate.HasValue)
            {
                builder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
            }
            if (query.ToDate.HasValue)
            {
                builder.AppendFormat(" AND RequestTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
            }
            return builder.ToString();
        }

        private static string BuildUnderlingStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from distro_Orders where OrderStatus != {0} AND OrderStatus != {1} AND DistributorUserId={2}", 4, 1, HiContext.Current.User.UserId);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_distro_Members.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(OrderId) from distro_Orders where OrderStatus != {0} AND OrderStatus != {1} AND DistributorUserId={2}", 4, 1, HiContext.Current.User.UserId);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and userId = vw_distro_Members.UserId) as OrderCount ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
            }
            builder.AppendFormat(" from vw_distro_Members WHERE ParentUserId={0} AND Expenditure > 0", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public override bool CreateUnderlingGrade(MemberGradeInfo underlingGrade)
        {
            StringBuilder builder = new StringBuilder();
            if (underlingGrade.IsDefault)
            {
                builder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 0 WHERE CreateUserId = {0};", HiContext.Current.User.UserId);
            }
            builder.AppendFormat("INSERT INTO distro_MemberGrades(CreateUserId, [Name], Description, Points, IsDefault, Discount) VALUES (@CreateUserId, @Name, @Description, @Points, @IsDefault, @Discount);", new object[0]);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, underlingGrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, underlingGrade.Description);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, underlingGrade.Points);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, underlingGrade.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Currency, underlingGrade.Discount);
            this.database.AddInParameter(sqlStringCommand, "CreateUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool DealBalanceDrawRequest(int userId, bool agree)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_UnderlingBalanceDrawRequest_Update");
            this.database.AddOutParameter(storedProcCommand, "Status", DbType.Int32, 4);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "Agree", DbType.Boolean, agree);
            this.database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = this.database.GetParameterValue(storedProcCommand, "Status");
            if ((parameterValue == DBNull.Value) || (parameterValue == null))
            {
                return false;
            }
            return (((int) this.database.GetParameterValue(storedProcCommand, "Status")) == 0);
        }

        public override bool DeleteMember(int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Member_Delete");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            Member user = Users.GetUser(userId) as Member;
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, user.Username);
            this.database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            this.database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
            return (((parameterValue != null) && (parameterValue != DBNull.Value)) && (Convert.ToInt32(parameterValue) == 0));
        }

        public override void DeleteSKUMemberPrice(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_SKUMemberPrice WHERE GradeId = @GradeId AND DistributoruserId=@DistributoruserId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "DistributoruserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool DeleteUnderlingGrade(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND CreateUserId=@CreateUserId AND NOT EXISTS(SELECT * FROM distro_Members WHERE GradeId = @GradeId AND ParentUserId=@ParentUserId)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "DistributoruserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "CreateUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
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
            builder.AppendFormat("SELECT TOP {0} * FROM distro_BalanceDetails B WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY JournalNumber DESC;", str);
            }
            else
            {
                builder.AppendFormat("AND JournalNumber < (SELECT MIN(JournalNumber) FROM (SELECT TOP {0} JournalNumber FROM distro_BalanceDetails WHERE 0=0 {1} ORDER BY JournalNumber DESC ) AS T) {1} ORDER BY JournalNumber DESC;", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat("SELECT COUNT(JournalNumber) AS Total from distro_BalanceDetails WHERE 0=0 {0}", str);
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

        public override DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            if (null == query)
            {
                return null;
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDrawRequestQuery(query);
            builder.AppendFormat("SELECT Top {0} * FROM distro_BalanceDrawRequest B WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY RequestTime DESC;", str);
            }
            else
            {
                builder.AppendFormat(" AND RequestTime < (SELECT MIN(RequestTime) FROM (SELECT TOP {0} RequestTime FROM distro_BalanceDrawRequest WHERE 0=0 {1} ORDER BY RequestTime DESC ) as T) {1} ORDER BY RequestTime DESC;", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat("SELECT COUNT(*) AS Total from distro_BalanceDrawRequest WHERE 0=0 {0}", str);
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

        public override MemberGradeInfo GetMemberGrade(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MemberGrades WHERE GradeId = @GradeId");
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

        public override DbQueryResult GetMembers(MemberQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("ParentUserId={0}", HiContext.Current.User.UserId);
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat("AND GradeId = {0}", query.GradeId.Value);
            }
            if (query.IsApproved.HasValue)
            {
                builder.AppendFormat("AND IsApproved = '{0}'", query.IsApproved.Value);
            }
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat("AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                builder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_Members", "UserId", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public override DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            if (fields.Count == 0)
            {
                return null;
            }
            DataTable table = null;
            string str = string.Empty;
            foreach (string str2 in fields)
            {
                str = str + str2 + ",";
            }
            str = str.Substring(0, str.Length - 1);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT {0} FROM vw_distro_Members WHERE ParentUserId={1} ", str, HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                builder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId={0}", query.GradeId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override DbQueryResult GetUnderlingBlanceList(MemberQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = string.Format(" AND ParentUserId={0}", HiContext.Current.User.UserId);
            if (!string.IsNullOrEmpty(query.Username))
            {
                str = str + string.Format(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.Realname))
            {
                str = str + string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
            }
            builder.AppendFormat("SELECT TOP {0} * FROM vw_distro_Members WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat(" {0} ORDER BY CreateDate DESC", str);
            }
            else
            {
                builder.AppendFormat(" AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_distro_Members WHERE 0=0 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";SELECT COUNT(CreateDate) AS Total FROM vw_distro_Members WHERE 0=0 {0}", str);
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

        public override IList<MemberGradeInfo> GetUnderlingGrades()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM distro_MemberGrades WHERE CreateUserId = {0} ORDER BY GradeId DESC;", HiContext.Current.User.UserId));
            IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateMemberGrade(reader));
                }
            }
            return list;
        }

        public override DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildUnderlingStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            total = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(BuildUnderlingStatisticsQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days)
        {
            int num;
            UserStatisticsForDate date;
            int num4;
            IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_distro_Members WHERE ParentUserId = @ParentUserId AND CreateDate >=@StartDate AND  CreateDate<= @EndDate) AS UserIncrease;");
            this.database.AddInParameter(sqlStringCommand, "ParentUserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime);
            DateTime time = new DateTime();
            DateTime time2 = new DateTime();
            if (days.HasValue)
            {
                time = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double) -days.Value);
            }
            else if (year.HasValue && month.HasValue)
            {
                time = new DateTime(year.Value, month.Value, 1);
            }
            else if (!(!year.HasValue || month.HasValue))
            {
                time = new DateTime(year.Value, 1, 1);
            }
            if (!days.HasValue)
            {
                if (year.HasValue && month.HasValue)
                {
                    int num2 = DateTime.DaysInMonth(year.Value, month.Value);
                    for (num = 1; num <= num2; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddDays(1.0);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                    return list;
                }
                if (year.HasValue && !month.HasValue)
                {
                    int num3 = 12;
                    for (num = 1; num <= num3; num++)
                    {
                        date = new UserStatisticsForDate();
                        if (num > 1)
                        {
                            time = time2;
                        }
                        time2 = time.AddMonths(1);
                        this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                        this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                        date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                        date.TimePoint = num;
                        list.Add(date);
                    }
                }
                return list;
            }
            num = 1;
        Label_01D1:
            num4 = num;
            if (num4 <= days)
            {
                date = new UserStatisticsForDate();
                if (num > 1)
                {
                    time = time2;
                }
                time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(time));
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(time2));
                date.UserCounts = (int) this.database.ExecuteScalar(sqlStringCommand);
                date.TimePoint = time.Day;
                list.Add(date);
                num++;
                goto Label_01D1;
            }
            return list;
        }

        public override bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(GradeId) as Count FROM distro_MemberGrades WHERE Points=@Points AND CreateUserId = {0} AND GradeId<>@GradeId;", HiContext.Current.User.UserId));
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, memberGrade.Points);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, memberGrade.GradeId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override void SetDefalutUnderlingGrade(int gradeId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("IF EXISTS(SELECT * FROM distro_MemberGrades WHERE IsDefault=0 AND GradeId = {0}) ", gradeId);
            builder.Append("BEGIN ");
            builder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 0 WHERE CreateUserId = {0};", HiContext.Current.User.UserId);
            builder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 1 WHERE GradeId = {0} AND CreateUserId = {1}; ", gradeId, HiContext.Current.User.UserId);
            builder.Append("END");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade)
        {
            string query = "UPDATE distro_MemberGrades SET [Name] = @Name, Description = @Description, Points = @Points, Discount = @Discount WHERE GradeId = @GradeId;";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, underlingGrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, underlingGrade.Description);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, underlingGrade.Points);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, underlingGrade.GradeId);
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Currency, underlingGrade.Discount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

