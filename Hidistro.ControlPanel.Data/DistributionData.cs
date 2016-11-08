namespace Hidistro.ControlPanel.Data
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Distribution;
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

    public class DistributionData : DistributorProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool AcceptSiteRequest(int siteQty, int requestId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_SiteRequest SET RequestStatus=@RequestStatus WHERE RequestId=@RequestId AND (SELECT COUNT(UserId) FROM distro_Settings)<@SiteQty");
            this.database.AddInParameter(sqlStringCommand, "RequestStatus", DbType.Int32, 2);
            this.database.AddInParameter(sqlStringCommand, "RequestId", DbType.Int32, requestId);
            this.database.AddInParameter(sqlStringCommand, "SiteQty", DbType.Int32, siteQty);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddDistributorGrade(DistributorGradeInfo distributorGrade)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorGrades(Name, Description, Discount) VALUES(@Name,@Description,@Discount)");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, distributorGrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, distributorGrade.Description);
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, distributorGrade.Discount);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool AddDistributorProductLines(int userId, IList<int> lineIds)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("DELETE FROM Hishop_DistributorProductLines WHERE UserId = {0}", userId);
            foreach (int num in lineIds)
            {
                builder.AppendFormat(" INSERT INTO Hishop_DistributorProductLines(LineId, UserId) VALUES ({0}, {1})", num, userId);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddInitData(int distributorId, DbTransaction dbtran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionInitData_Create");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, distributorId);
            if (dbtran != null)
            {
                return (this.database.ExecuteNonQuery(storedProcCommand, dbtran) >= 1);
            }
            return (this.database.ExecuteNonQuery(storedProcCommand) >= 1);
        }

        public override bool AddSiteSettings(SiteSettings settings, int requestId, DbTransaction dbtran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Settings(UserId,SiteUrl,Disabled, RequestDate,CreateDate,LogoUrl,SiteDescription,SiteName,Theme,Footer,SearchMetaKeywords,SearchMetaDescription,DecimalLength,YourPriceName,DefaultProductImage,PointsRate,OrderShowDays,HtmlOnlineServiceCode) VALUES(@UserId,@SiteUrl,@Disabled,@RequestDate,@CreateDate,@LogoUrl,@SiteDescription,@SiteName,@Theme,@Footer,@SearchMetaKeywords,@SearchMetaDescription,@DecimalLength,@YourPriceName,@DefaultProductImage,@PointsRate,@OrderShowDays,@HtmlOnlineServiceCode)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, settings.UserId);
            this.database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, settings.SiteUrl);
            this.database.AddInParameter(sqlStringCommand, "LogoUrl", DbType.String, settings.LogoUrl);
            this.database.AddInParameter(sqlStringCommand, "SiteDescription", DbType.String, settings.SiteDescription);
            this.database.AddInParameter(sqlStringCommand, "SiteName", DbType.String, settings.SiteName);
            this.database.AddInParameter(sqlStringCommand, "Theme", DbType.String, settings.Theme);
            this.database.AddInParameter(sqlStringCommand, "Footer", DbType.String, settings.Footer);
            this.database.AddInParameter(sqlStringCommand, "SearchMetaKeywords", DbType.String, settings.SearchMetaKeywords);
            this.database.AddInParameter(sqlStringCommand, "SearchMetaDescription", DbType.String, settings.SearchMetaDescription);
            this.database.AddInParameter(sqlStringCommand, "DecimalLength", DbType.Int32, settings.DecimalLength);
            this.database.AddInParameter(sqlStringCommand, "YourPriceName", DbType.String, settings.YourPriceName);
            this.database.AddInParameter(sqlStringCommand, "Disabled", DbType.Boolean, settings.Disabled);
            this.database.AddInParameter(sqlStringCommand, "DefaultProductImage", DbType.String, settings.DefaultProductImage);
            this.database.AddInParameter(sqlStringCommand, "PointsRate", DbType.Decimal, settings.PointsRate);
            this.database.AddInParameter(sqlStringCommand, "OrderShowDays", DbType.Int32, settings.OrderShowDays);
            this.database.AddInParameter(sqlStringCommand, "HtmlOnlineServiceCode", DbType.String, settings.HtmlOnlineServiceCode);
            this.database.AddInParameter(sqlStringCommand, "RequestDate", DbType.DateTime, settings.RequestDate);
            this.database.AddInParameter(sqlStringCommand, "CreateDate", DbType.DateTime, settings.CreateDate);
            if (dbtran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbtran) >= 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
        {
            StringBuilder builder = new StringBuilder();
            switch (saleStatisticsType)
            {
                case SaleStatisticsType.SaleCounts:
                    builder.Append("SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE (PurchaseDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.SaleTotal:
                    builder.Append("SELECT Isnull(SUM(PurchaseTotal),0)");
                    builder.Append(" FROM Hishop_PurchaseOrders WHERE  (PurchaseDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                    break;

                case SaleStatisticsType.Profits:
                    builder.Append("SELECT IsNull(SUM(PurchaseProfit),0) FROM Hishop_PurchaseOrders WHERE (PurchaseDate BETWEEN @StartDate AND @EndDate)");
                    builder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                    break;
            }
            return builder.ToString();
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
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
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

        private static string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_aspnet_Distributors WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
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

        private static string BuildDistributorStatisticsQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT UserId, UserName ");
            if (query.StartDate.HasValue || query.EndDate.HasValue)
            {
                builder.AppendFormat(",  ( select isnull(SUM(PurchaseTotal),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as SaleTotals");
                builder.AppendFormat(",(select Count(PurchaseOrderId) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as PurchaseOrderCount ");
                builder.AppendFormat(",(select isnull(SUM(PurchaseProfit),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                if (query.StartDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
                }
                if (query.EndDate.HasValue)
                {
                    builder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
                }
                builder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as Profits ");
            }
            else
            {
                builder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(PurchaseOrder,0) as PurchaseOrderCount");
                builder.AppendFormat(",(select isnull(SUM(PurchaseProfit),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
                builder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as Profits ");
            }
            builder.Append(" from vw_aspnet_Distributors WHERE IsApproved = 1");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildProductSaleQuery(SaleStatisticsQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemPurchasePrice * o.Quantity) AS ProductSaleTotals,");
            builder.Append("  (SUM(o.ItemPurchasePrice * o.Quantity) - SUM(o.CostPrice * o.Quantity) )AS ProductProfitsTotals ");
            builder.AppendFormat(" FROM Hishop_PurchaseOrderItems o  WHERE 0=0 ", new object[0]);
            builder.AppendFormat(" AND PurchaseOrderId IN (SELECT  PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2} )", 1, 4, 9);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            builder.Append(" GROUP BY ProductId HAVING ProductId IN");
            builder.Append(" (SELECT ProductId FROM Hishop_Products)");
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private static string BuildPurchaseOrderQuery(UserOrderQuery query)
        {
            if (null == query)
            {
                throw new ArgumentNullException("query");
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND Distributorname LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND  PurchaseDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND  PurchaseDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        private DataTable CreateTable()
        {
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("Date", typeof(int));
            DataColumn column2 = new DataColumn("SaleTotal", typeof(decimal));
            DataColumn column3 = new DataColumn("Percentage", typeof(decimal));
            DataColumn column4 = new DataColumn("Lenth", typeof(decimal));
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            return table;
        }

        public override bool DealDistributorBalanceDrawRequest(int userId, bool agree)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorBalanceDrawRequest_Update");
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

        public override bool Delete(int userId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Distribution_Delete");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddParameter(storedProcCommand, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
            this.database.ExecuteNonQuery(storedProcCommand);
            object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
            return (((parameterValue != null) && (parameterValue != DBNull.Value)) && (Convert.ToInt32(parameterValue) == 0));
        }

        public override bool DeleteDistributorGrade(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_DistributorGrades  WHERE GradeId = @GradeId AND not exists (select GradeId from dbo.aspnet_Distributors where GradeId=@GradeId)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Currency, gradeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool ExistDistributor(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Distributors  WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Currency, gradeId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public override bool ExistGradeName(string gradeName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_DistributorGrades WHERE Name=@Name");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, gradeName);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        private int GetDayCount(int year, int month)
        {
            if (month == 2)
            {
                if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0))
                {
                    return 0x1d;
                }
                return 0x1c;
            }
            if (((((month == 1) || (month == 3)) || ((month == 5) || (month == 7))) || ((month == 8) || (month == 10))) || (month == 12))
            {
                return 0x1f;
            }
            return 30;
        }

        public override DataTable GetDayDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            decimal allSalesTotal = this.GetMonthDistributionTotal(year, month, saleStatisticsType);
            int dayCount = this.GetDayCount(year, month);
            int num3 = ((year == DateTime.Now.Year) && (month == DateTime.Now.Month)) ? DateTime.Now.Day : dayCount;
            for (int i = 1; i <= num3; i++)
            {
                DateTime time = new DateTime(year, month, i);
                DateTime time2 = time.AddDays(1.0);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                this.InsertToTable(table, i, salesTotal, allSalesTotal);
            }
            return table;
        }

        public override DataTable GetDistributionProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionProductSales_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, productSale.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, productSale.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, productSale.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DataTable GetDistributionProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionProductSalesNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildProductSaleQuery(productSale));
            this.database.AddOutParameter(storedProcCommand, "TotalProductSales", DbType.Int32, 4);
            DataTable table = null;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            totalProductSales = (int) this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
            return table;
        }

        public override DbQueryResult GetDistributorBalance(DistributorQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = string.Empty;
            if (!string.IsNullOrEmpty(query.Username))
            {
                str = string.Format("AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                str = str + string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            builder.AppendFormat("SELECT TOP {0} *", query.PageSize);
            builder.Append(" FROM vw_aspnet_Distributors WHERE IsApproved = 1");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat(" {0} ORDER BY CreateDate DESC", str);
            }
            else
            {
                builder.AppendFormat(" AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_aspnet_Distributors WHERE IsApproved = 1 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(" ;SELECT COUNT(CreateDate) AS Total FROM vw_aspnet_Distributors WHERE  IsApproved = 1 {0}", str);
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

        public override DbQueryResult GetDistributorBalanceDetails(BalanceDetailQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.AppendFormat("SELECT TOP {0} * FROM Hishop_DistributorBalanceDetails B WHERE 0=0", query.PageSize);
            if (query.PageIndex == 1)
            {
                builder.AppendFormat(" {0} ORDER BY JournalNumber DESC", str);
            }
            else
            {
                builder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_DistributorBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(" ;select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", str);
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

        public override DbQueryResult GetDistributorBalanceDetailsNoPage(BalanceDetailQuery query)
        {
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDetailsQuery(query);
            builder.Append("SELECT * FROM Hishop_DistributorBalanceDetails WHERE 0=0 ");
            builder.AppendFormat("{0} ORDER BY JournalNumber DESC", str);
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

        public override DbQueryResult GetDistributorBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            if (null == query)
            {
                return new DbQueryResult();
            }
            DbQueryResult result = new DbQueryResult();
            StringBuilder builder = new StringBuilder();
            string str = BuildBalanceDrawRequestQuery(query);
            builder.AppendFormat("select top {0} *", query.PageSize);
            builder.Append(" from Hishop_DistributorBalanceDrawRequest B where 0=0 ");
            if (query.PageIndex == 1)
            {
                builder.AppendFormat("{0} ORDER BY RequestTime DESC", str);
            }
            else
            {
                builder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_DistributorBalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, str);
            }
            if (query.IsCount)
            {
                builder.AppendFormat(";select count(*) as Total from Hishop_DistributorBalanceDrawRequest where 0=0 {0}", str);
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

        public override DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Currency, gradeId);
            DistributorGradeInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulDistributorGrade(reader);
                }
            }
            return info;
        }

        public override DataTable GetDistributorGrades()
        {
            DataTable table;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override IList<int> GetDistributorProductLines(int userId)
        {
            IList<int> list = new List<int>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((int) reader["LineId"]);
                }
            }
            return list;
        }

        public override IList<Distributor> GetDistributors()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors");
            IList<Distributor> list = new List<Distributor>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                Distributor item = null;
                while (reader.Read())
                {
                    item = new Distributor();
                    item.UserId = (int) reader["UserId"];
                    item.Username = (string) reader["UserName"];
                    list.Add(item);
                }
            }
            return list;
        }

        public override DbQueryResult GetDistributors(DistributorQuery query)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("IsApproved = {0}", query.IsApproved ? 1 : 0);
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                builder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId = {0}", query.GradeId);
            }
            if (query.LineId.HasValue)
            {
                builder.AppendFormat(" AND UserId IN (SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId={0})", query.LineId.Value);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Distributors", "UserId", builder.ToString(), "*");
        }

        public override DataTable GetDistributorSites(Pagination pagination, string name, string trueName, out int total)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
            }
            if (!string.IsNullOrEmpty(trueName))
            {
                str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
            }
            string query = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE 1=1 ";
            query = query + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str3 = string.Empty;
            if (pagination.PageIndex == 1)
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,Disabled ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  1=1 ", pagination.PageSize);
            }
            else
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,Disabled,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings ORDER BY RequestDate DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            str3 = str3 + str + " ORDER BY RequestDate DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetDistributorsNopage(DistributorQuery query, IList<string> fields)
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
            builder.AppendFormat("SELECT {0} FROM vw_aspnet_Distributors WHERE IsApproved=1 ", str);
            if (!string.IsNullOrEmpty(query.Username))
            {
                builder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
            }
            if (query.GradeId.HasValue)
            {
                builder.AppendFormat(" AND GradeId={0}", query.GradeId);
            }
            if (!string.IsNullOrEmpty(query.RealName))
            {
                builder.AppendFormat(" AND RealName LIKE '%{0}%'", query.RealName);
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
                reader.Close();
            }
            return table;
        }

        public override OrderStatisticsInfo GetDistributorStatistics(SaleStatisticsQuery query, out int totalDistributors)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildDistributorStatisticsQuery(query));
            this.database.AddOutParameter(storedProcCommand, "TotalDistributors", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["SaleTotals"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["SaleTotals"];
                    }
                    if (reader["Profits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["Profits"];
                    }
                }
            }
            totalDistributors = (int) this.database.GetParameterValue(storedProcCommand, "TotalDistributors");
            return info;
        }

        public override OrderStatisticsInfo GetDistributorStatisticsNoPage(SaleStatisticsQuery query)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildDistributorStatisticsQuery(query));
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (!reader.NextResult())
                {
                    return info;
                }
                reader.Read();
                if (reader["SaleTotals"] != DBNull.Value)
                {
                    info.TotalOfSearch += (decimal) reader["SaleTotals"];
                }
                if (reader["Profits"] != DBNull.Value)
                {
                    info.ProfitsOfSearch += (decimal) reader["Profits"];
                }
            }
            return info;
        }

        public override DataTable GetDomainRequests(Pagination pagination, string name, out int total)
        {
            string query = "SELECT COUNT(*) FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE  Hishop_SiteRequest.RequestStatus=@RequestStatus";
            string str2 = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                str2 = str2 + string.Format(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(name));
            }
            query = query + str2;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "RequestStatus", DbType.Int32, 1);
            total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str3 = string.Empty;
            if (pagination.PageIndex == 1)
            {
                str3 = string.Format("SELECT TOP {0} UserName,Wangwang,FirstSiteUrl,RequestTime,Email,RequestStatus,RequestId,Hishop_SiteRequest.UserId FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE  Hishop_SiteRequest.RequestStatus={1} ", pagination.PageSize, 1);
            }
            else
            {
                str3 = string.Format("SELECT TOP {0} UserName,Wangwang,FirstSiteUrl,RequestTime,Email,RequestStatus,RequestId,Hishop_SiteRequest.UserId FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE Hishop_SiteRequest.RequestStatus={2} AND RequestId NOT IN (SELECT TOP {1} RequestId FROM Hishop_SiteRequest ORDER BY RequestId DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1), 1);
            }
            str3 = str3 + str2 + " ORDER BY RequestId DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetEtaoRequests(Pagination pagination, string name, string trueName, out int total)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
            }
            if (!string.IsNullOrEmpty(trueName))
            {
                str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
            }
            string query = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE EtaoStatus=-1 ";
            query = query + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str3 = string.Empty;
            if (pagination.PageIndex == 1)
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled] ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=-1   ", pagination.PageSize);
            }
            else
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,[Disabled],[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=-1 and  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings  where EtaoStatus=-1  ORDER BY EtaoApplyTime DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            str3 = str3 + str + " ORDER BY EtaoApplyTime DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetEtaoSites(Pagination pagination, string name, string trueName, out int total)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
            }
            if (!string.IsNullOrEmpty(trueName))
            {
                str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
            }
            string query = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1 ";
            query = query + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
            string str3 = string.Empty;
            if (pagination.PageIndex == 1)
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled] ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1  ", pagination.PageSize);
            }
            else
            {
                str3 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled],[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1 and  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings  where  EtaoStatus=1 ORDER BY EtaoApplyTime DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
            }
            str3 = str3 + str + " ORDER BY EtaoApplyTime DESC";
            sqlStringCommand = this.database.GetSqlStringCommand(str3);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetMonthDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime);
            DataTable table = this.CreateTable();
            int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
            for (int i = 1; i <= num; i++)
            {
                DateTime time = new DateTime(year, i, 1);
                DateTime time2 = time.AddMonths(1);
                this.database.SetParameterValue(sqlStringCommand, "@StartDate", time);
                this.database.SetParameterValue(sqlStringCommand, "@EndDate", time2);
                object obj2 = this.database.ExecuteScalar(sqlStringCommand);
                decimal salesTotal = (obj2 == null) ? 0M : Convert.ToDecimal(obj2);
                decimal yearDistributionTotal = this.GetYearDistributionTotal(year, saleStatisticsType);
                this.InsertToTable(table, i, salesTotal, yearDistributionTotal);
            }
            return table;
        }

        public override decimal GetMonthDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, month, 1);
            DateTime time2 = time.AddMonths(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public override OrderStatisticsInfo GetPurchaseOrders(UserOrderQuery order)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PurchaseOrderStatistics_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, order.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, order.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, order.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildPurchaseOrderQuery(order));
            this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["PurchaseTotal"] != DBNull.Value)
                    {
                        info.TotalOfPage += (decimal) reader["PurchaseTotal"];
                    }
                    if (reader["PurchaseProfits"] != DBNull.Value)
                    {
                        info.ProfitsOfPage += (decimal) reader["PurchaseProfits"];
                    }
                }
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["PurchaseTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["PurchaseTotal"];
                    }
                    if (reader["PurchaseProfits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["PurchaseProfits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
            return info;
        }

        public override OrderStatisticsInfo GetPurchaseOrdersNoPage(UserOrderQuery order)
        {
            OrderStatisticsInfo info = new OrderStatisticsInfo();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PurchaseOrderStatisticsNoPage_Get");
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, BuildPurchaseOrderQuery(order));
            this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", DbType.Int32, 4);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                info.OrderTbl = DataHelper.ConverDataReaderToDataTable(reader);
                if (reader.NextResult())
                {
                    reader.Read();
                    if (reader["PurchaseTotal"] != DBNull.Value)
                    {
                        info.TotalOfSearch += (decimal) reader["PurchaseTotal"];
                    }
                    if (reader["PurchaseProfits"] != DBNull.Value)
                    {
                        info.ProfitsOfSearch += (decimal) reader["PurchaseProfits"];
                    }
                }
            }
            info.TotalCount = (int) this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
            return info;
        }

        public override SiteRequestInfo GetSiteRequestInfo(int requestId)
        {
            SiteRequestInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SiteRequest WHERE RequestId=@RequestId");
            this.database.AddInParameter(sqlStringCommand, "RequestId", DbType.Int32, requestId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulSiteRequest(reader);
                }
            }
            return info;
        }

        public override decimal GetYearDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            string query = this.BuiderSqlStringByType(saleStatisticsType);
            if (query == null)
            {
                return 0M;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DateTime time = new DateTime(year, 1, 1);
            DateTime time2 = time.AddYears(1);
            this.database.AddInParameter(sqlStringCommand, "@StartDate", DbType.DateTime, time);
            this.database.AddInParameter(sqlStringCommand, "@EndDate", DbType.DateTime, time2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            decimal num = 0M;
            if (obj2 != null)
            {
                num = Convert.ToDecimal(obj2);
            }
            return num;
        }

        public override bool InsertBalanceDetail(BalanceDetailInfo balanceDetails, DbTransaction dbTran)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails (UserId,UserName, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId,@UserName, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark);UPDATE aspnet_Distributors SET Balance = @Balance WHERE UserId = @UserId;");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, balanceDetails.UserId);
            this.database.AddInParameter(sqlStringCommand, "UserName", DbType.String, balanceDetails.UserName);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, balanceDetails.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) balanceDetails.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Income", DbType.Currency, balanceDetails.Income);
            this.database.AddInParameter(sqlStringCommand, "Expenses", DbType.Currency, balanceDetails.Expenses);
            this.database.AddInParameter(sqlStringCommand, "Balance", DbType.Currency, balanceDetails.Balance);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, balanceDetails.Remark);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private void InsertToTable(DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
        {
            DataRow row = table.NewRow();
            row["Date"] = date;
            row["SaleTotal"] = salesTotal;
            if (allSalesTotal != 0M)
            {
                row["Percentage"] = (salesTotal / allSalesTotal) * 100M;
            }
            else
            {
                row["Percentage"] = 0;
            }
            row["Lenth"] = ((decimal) row["Percentage"]) * 4M;
            table.Rows.Add(row);
        }

        public override bool RefuseSiteRequest(int requestId, string reason)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_SiteRequest SET RequestStatus=@RequestStatus,RefuseReason=@RefuseReason WHERE RequestId=@RequestId");
            this.database.AddInParameter(sqlStringCommand, "RequestId", DbType.Int32, requestId);
            this.database.AddInParameter(sqlStringCommand, "RequestStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "RefuseReason", DbType.String, reason);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool UpdateDistributorGrade(DistributorGradeInfo distributorGrade)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorGrades SET Name =@Name,Description = @Description,Discount = @Discount WHERE GradeId = @GradeId");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, distributorGrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, distributorGrade.Description);
            this.database.AddInParameter(sqlStringCommand, "Discount", DbType.Int32, distributorGrade.Discount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Currency, distributorGrade.GradeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }
    }
}

