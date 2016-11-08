namespace Hidistro.ControlPanel.Distribution
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public static class DistributorHelper
    {
        public static bool AcceptDistributorRequest(Distributor distributor, IList<int> lineIds)
        {
            if (Users.UpdateUser(distributor))
            {
                DistributorProvider.Instance().AddDistributorProductLines(distributor.UserId, lineIds);
                EventLogs.WriteOperationLog(Privilege.DistributorRequests, string.Format(CultureInfo.InvariantCulture, "接受了用户名为 “{0}” 的分销商", new object[] { distributor.Username }));
                return true;
            }
            return false;
        }

        public static bool AddBalance(BalanceDetailInfo balanceDetails, decimal addmoney)
        {
            if (null == balanceDetails)
            {
                return false;
            }
            EventLogs.WriteOperationLog(Privilege.DistributorReCharge, string.Format(CultureInfo.InvariantCulture, "给分销商\"{0}\"添加预付款\"{1}\"", new object[] { balanceDetails.UserName, addmoney }));
            return DistributorProvider.Instance().InsertBalanceDetail(balanceDetails, null);
        }

        public static bool AddDistributorGrade(DistributorGradeInfo distributorGrade)
        {
            Globals.EntityCoding(distributorGrade, true);
            bool flag = DistributorProvider.Instance().AddDistributorGrade(distributorGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddDistributorGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的分销商等级", new object[] { distributorGrade.Name }));
            }
            return flag;
        }

        public static bool AddDistributorProductLines(int userId, IList<int> lineIds)
        {
            return DistributorProvider.Instance().AddDistributorProductLines(userId, lineIds);
        }

        public static bool AddSiteSettings(SiteSettings siteSettings, int requestId, int siteQty)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    DistributorProvider provider = DistributorProvider.Instance();
                    if (!provider.AcceptSiteRequest(siteQty, requestId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!provider.AddSiteSettings(siteSettings, requestId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!DistributorProvider.Instance().AddInitData(siteSettings.UserId.Value, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch
                {
                    dbTran.Rollback();
                    flag = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public static bool CloseEtao(int userId)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                return false;
            }
            siteSettings.IsOpenEtao = false;
            SettingsManager.Save(siteSettings);
            return true;
        }

        public static bool CloseSite(int userId)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                return false;
            }
            siteSettings.Disabled = true;
            SettingsManager.Save(siteSettings);
            return true;
        }

        public static bool DealDistributorBalanceDrawRequest(int userId, bool agree)
        {
            bool flag = DistributorProvider.Instance().DealDistributorBalanceDrawRequest(userId, agree);
            if (flag)
            {
                Users.ClearUserCache(Users.GetUser(userId));
            }
            return flag;
        }

        public static bool Delete(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteDistributor);
            IUser user = Users.GetUser(userId);
            bool flag = DistributorProvider.Instance().Delete(userId);
            if (flag)
            {
                Users.ClearUserCache(user);
                EventLogs.WriteOperationLog(Privilege.DeleteDistributor, string.Format(CultureInfo.InvariantCulture, "终止了编号为 “{0}”的分销商的合作", new object[] { userId }));
            }
            return flag;
        }

        public static bool DeleteDistributorGrade(int gradeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteDistributorGrade);
            bool flag = false;
            if (!DistributorProvider.Instance().ExistDistributor(gradeId))
            {
                flag = DistributorProvider.Instance().DeleteDistributorGrade(gradeId);
            }
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteDistributorGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的分销商等级", new object[] { gradeId }));
            }
            return flag;
        }

        public static bool ExistGradeName(string gradeName)
        {
            return DistributorProvider.Instance().ExistGradeName(gradeName);
        }

        public static DataTable GetDayDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return DistributorProvider.Instance().GetDayDistributionTotal(year, month, saleStatisticsType);
        }

        public static DataTable GetDistributionProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return DistributorProvider.Instance().GetDistributionProductSales(productSale, out totalProductSales);
        }

        public static DataTable GetDistributionProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
        {
            if (productSale == null)
            {
                totalProductSales = 0;
                return null;
            }
            return DistributorProvider.Instance().GetDistributionProductSalesNoPage(productSale, out totalProductSales);
        }

        public static Distributor GetDistributor(int userId)
        {
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Distributor))
            {
                return (user as Distributor);
            }
            return null;
        }

        public static DbQueryResult GetDistributorBalance(DistributorQuery query)
        {
            return DistributorProvider.Instance().GetDistributorBalance(query);
        }

        public static DbQueryResult GetDistributorBalanceDetails(BalanceDetailQuery query)
        {
            return DistributorProvider.Instance().GetDistributorBalanceDetails(query);
        }

        public static DbQueryResult GetDistributorBalanceDetailsNoPage(BalanceDetailQuery query)
        {
            return DistributorProvider.Instance().GetDistributorBalanceDetailsNoPage(query);
        }

        public static DbQueryResult GetDistributorBalanceDrawRequests(BalanceDrawRequestQuery query)
        {
            return DistributorProvider.Instance().GetDistributorBalanceDrawRequests(query);
        }

        public static DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
        {
            return DistributorProvider.Instance().GetDistributorGradeInfo(gradeId);
        }

        public static DataTable GetDistributorGrades()
        {
            return DistributorProvider.Instance().GetDistributorGrades();
        }

        public static IList<int> GetDistributorProductLines(int userId)
        {
            return DistributorProvider.Instance().GetDistributorProductLines(userId);
        }

        public static IList<Distributor> GetDistributors()
        {
            return DistributorProvider.Instance().GetDistributors();
        }

        public static DbQueryResult GetDistributors(DistributorQuery query)
        {
            return DistributorProvider.Instance().GetDistributors(query);
        }

        public static DataTable GetDistributorSites(Pagination pagination, string name, string trueName, out int total)
        {
            return DistributorProvider.Instance().GetDistributorSites(pagination, name, trueName, out total);
        }

        public static DataTable GetDistributorsNopage(DistributorQuery query, IList<string> fields)
        {
            return DistributorProvider.Instance().GetDistributorsNopage(query, fields);
        }

        public static OrderStatisticsInfo GetDistributorStatistics(SaleStatisticsQuery query, out int totalDistributors)
        {
            return DistributorProvider.Instance().GetDistributorStatistics(query, out totalDistributors);
        }

        public static OrderStatisticsInfo GetDistributorStatisticsNoPage(SaleStatisticsQuery query)
        {
            return DistributorProvider.Instance().GetDistributorStatisticsNoPage(query);
        }

        public static DataTable GetDomainRequests(Pagination pagination, string name, out int total)
        {
            return DistributorProvider.Instance().GetDomainRequests(pagination, name, out total);
        }

        public static DataTable GetEtaoRequests(Pagination pagination, string name, string trueName, out int total)
        {
            return DistributorProvider.Instance().GetEtaoRequests(pagination, name, trueName, out total);
        }

        public static DataTable GetEtaoSites(Pagination pagination, string name, string trueName, out int total)
        {
            return DistributorProvider.Instance().GetEtaoSites(pagination, name, trueName, out total);
        }

        public static DataTable GetMonthDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return DistributorProvider.Instance().GetMonthDistributionTotal(year, saleStatisticsType);
        }

        public static decimal GetMonthDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
        {
            return DistributorProvider.Instance().GetMonthDistributionTotal(year, month, saleStatisticsType);
        }

        public static OrderStatisticsInfo GetPurchaseOrders(UserOrderQuery order)
        {
            return DistributorProvider.Instance().GetPurchaseOrders(order);
        }

        public static OrderStatisticsInfo GetPurchaseOrdersNoPage(UserOrderQuery order)
        {
            return DistributorProvider.Instance().GetPurchaseOrdersNoPage(order);
        }

        public static SiteRequestInfo GetSiteRequestInfo(int requestId)
        {
            return DistributorProvider.Instance().GetSiteRequestInfo(requestId);
        }

        public static decimal GetYearDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
        {
            return DistributorProvider.Instance().GetYearDistributionTotal(year, saleStatisticsType);
        }

        public static bool OpenEtao(int userId)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                return false;
            }
            siteSettings.IsOpenEtao = true;
            siteSettings.EtaoStatus = 1;
            SettingsManager.Save(siteSettings);
            return true;
        }

        public static bool OpenSite(int userId)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                return false;
            }
            siteSettings.Disabled = false;
            SettingsManager.Save(siteSettings);
            return true;
        }

        public static bool RefuseDistributorRequest(int userId)
        {
            return DistributorProvider.Instance().Delete(userId);
        }

        public static bool RefuseEtao(int userId)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(userId);
            if (siteSettings == null)
            {
                return false;
            }
            siteSettings.IsOpenEtao = false;
            siteSettings.EtaoStatus = 0;
            SettingsManager.Save(siteSettings);
            return true;
        }

        public static bool RefuseSiteRequest(int requestId, string reason)
        {
            return DistributorProvider.Instance().RefuseSiteRequest(requestId, reason);
        }

        public static bool UpdateDistributorGrade(DistributorGradeInfo distributorGrade)
        {
            Globals.EntityCoding(distributorGrade, true);
            bool flag = DistributorProvider.Instance().UpdateDistributorGrade(distributorGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditDistributorGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的分销商等级", new object[] { distributorGrade.GradeId }));
            }
            return flag;
        }

        public static bool UpdateDistributorSettings(int userId, int gradeId, string remark)
        {
            IUser user = Users.GetUser(userId, false);
            if ((user != null) && (user.UserRole == UserRole.Distributor))
            {
                Distributor distributor = user as Distributor;
                distributor.GradeId = gradeId;
                distributor.Remark = remark;
                return Users.UpdateUser(distributor);
            }
            return false;
        }
    }
}

