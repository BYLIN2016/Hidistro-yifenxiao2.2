namespace Hidistro.ControlPanel.Distribution
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using Hidistro.Core;

    public abstract class DistributorProvider
    {
        private static readonly DistributorProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.DistributionData,Hidistro.ControlPanel.Data") as DistributorProvider);

        protected DistributorProvider()
        {
        }

        public abstract bool AcceptSiteRequest(int siteQty, int requestId, DbTransaction dbTran);
        public abstract bool AddDistributorGrade(DistributorGradeInfo distributorGrade);
        public abstract bool AddDistributorProductLines(int userId, IList<int> lineIds);
        public abstract bool AddInitData(int distributorId, DbTransaction dbtran);
        public abstract bool AddSiteSettings(SiteSettings siteSettings, int requestId, DbTransaction dbtran);
        public abstract bool DealDistributorBalanceDrawRequest(int userId, bool agree);
        public abstract bool Delete(int userId);
        public abstract bool DeleteDistributorGrade(int gradeId);
        public abstract bool ExistDistributor(int gradeId);
        public abstract bool ExistGradeName(string gradeName);
        public abstract DataTable GetDayDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract DataTable GetDistributionProductSales(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DataTable GetDistributionProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales);
        public abstract DbQueryResult GetDistributorBalance(DistributorQuery query);
        public abstract DbQueryResult GetDistributorBalanceDetails(BalanceDetailQuery query);
        public abstract DbQueryResult GetDistributorBalanceDetailsNoPage(BalanceDetailQuery query);
        public abstract DbQueryResult GetDistributorBalanceDrawRequests(BalanceDrawRequestQuery query);
        public abstract DistributorGradeInfo GetDistributorGradeInfo(int gradId);
        public abstract DataTable GetDistributorGrades();
        public abstract IList<int> GetDistributorProductLines(int userId);
        public abstract IList<Distributor> GetDistributors();
        public abstract DbQueryResult GetDistributors(DistributorQuery query);
        public abstract DataTable GetDistributorSites(Pagination pagination, string name, string trueName, out int total);
        public abstract DataTable GetDistributorsNopage(DistributorQuery query, IList<string> fields);
        public abstract OrderStatisticsInfo GetDistributorStatistics(SaleStatisticsQuery query, out int totalDistributors);
        public abstract OrderStatisticsInfo GetDistributorStatisticsNoPage(SaleStatisticsQuery query);
        public abstract DataTable GetDomainRequests(Pagination pagination, string name, out int total);
        public abstract DataTable GetEtaoRequests(Pagination pagination, string name, string trueName, out int total);
        public abstract DataTable GetEtaoSites(Pagination pageination, string name, string trueName, out int total);
        public abstract DataTable GetMonthDistributionTotal(int year, SaleStatisticsType saleStatisticsType);
        public abstract decimal GetMonthDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType);
        public abstract OrderStatisticsInfo GetPurchaseOrders(UserOrderQuery order);
        public abstract OrderStatisticsInfo GetPurchaseOrdersNoPage(UserOrderQuery order);
        public abstract SiteRequestInfo GetSiteRequestInfo(int requestId);
        public abstract decimal GetYearDistributionTotal(int year, SaleStatisticsType saleStatisticsType);
        public abstract bool InsertBalanceDetail(BalanceDetailInfo balanceDetail, DbTransaction dbTran);
        public static DistributorProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool RefuseSiteRequest(int requestId, string reason);
        public abstract bool UpdateDistributorGrade(DistributorGradeInfo distributorGrade);
    }
}

