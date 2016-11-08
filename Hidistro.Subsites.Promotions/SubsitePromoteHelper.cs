namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Hidistro.Entities.Members;

    public static class SubsitePromoteHelper
    {
        public static bool AddBundlingProduct(BundlingInfo bundlingInfo)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int bundlingID = SubsitePromotionsProvider.Instance().AddBundlingProduct(bundlingInfo, dbTran);
                    if (bundlingID <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddBundlingProductItems(bundlingID, bundlingInfo.BundlingItemInfos, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
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

        public static bool AddCountDown(CountDownInfo countDownInfo)
        {
            return SubsitePromotionsProvider.Instance().AddCountDown(countDownInfo);
        }

        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int groupBuyId = SubsitePromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
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

        public static int AddPromotion(PromotionInfo promotion)
        {
            int num2;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int activityId = SubsitePromotionsProvider.Instance().AddPromotion(promotion, dbTran);
                    if (activityId <= 0)
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddPromotionMemberGrades(activityId, promotion.MemberGradeIds, dbTran))
                    {
                        dbTran.Rollback();
                        return -2;
                    }
                    dbTran.Commit();
                    num2 = activityId;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    num2 = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return num2;
        }

        public static bool AddPromotionProducts(int activityId, string productIds)
        {
            return SubsitePromotionsProvider.Instance().AddPromotionProducts(activityId, productIds);
        }

        public static bool DeleteBundlingProduct(int bundlingID)
        {
            return SubsitePromotionsProvider.Instance().DeleteBundlingProduct(bundlingID);
        }

        public static bool DeleteCountDown(int countDownId)
        {
            return SubsitePromotionsProvider.Instance().DeleteCountDown(countDownId);
        }

        public static bool DeleteGroupBuy(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
        }

        public static bool DeletePromotion(int activityId)
        {
            return SubsitePromotionsProvider.Instance().DeletePromotion(activityId);
        }

        public static bool DeletePromotionProducts(int activityId, int? productId)
        {
            return SubsitePromotionsProvider.Instance().DeletePromotionProducts(activityId, productId);
        }

        public static int EditPromotion(PromotionInfo promotion)
        {
            int num;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsitePromotionsProvider.Instance().EditPromotion(promotion, dbTran))
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTran))
                    {
                        dbTran.Rollback();
                        return -2;
                    }
                    dbTran.Commit();
                    num = 1;
                }
                catch (Exception)
                {
                    dbTran.Rollback();
                    num = 0;
                }
                finally
                {
                    connection.Close();
                }
            }
            return num;
        }

        public static BundlingInfo GetBundlingInfo(int bundlingID)
        {
            return SubsitePromotionsProvider.Instance().GetBundlingInfo(bundlingID);
        }

        public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetBundlingProducts(query);
        }

        public static CountDownInfo GetCountDownInfo(int countDownId)
        {
            return SubsitePromotionsProvider.Instance().GetCountDownInfo(countDownId);
        }

        public static DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetCountDownList(query);
        }

        public static DbQueryResult GetCouponsList(CouponItemInfoQuery couponquery)
        {
            return SubsitePromotionsProvider.Instance().GetCouponsList(couponquery);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return SubsitePromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().GetGroupBuy(groupBuyId);
        }

        public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            return SubsitePromotionsProvider.Instance().GetGroupBuyList(query);
        }

        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return SubsitePromotionsProvider.Instance().GetMembersByRank(gradeId);
        }

        public static IList<Member> GetMemdersByNames(IList<string> names)
        {
            IList<Member> list = new List<Member>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Underling))
                {
                    list.Add(user as Member);
                }
            }
            return list;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static string GetPriceByProductId(int productId)
        {
            return SubsitePromotionsProvider.Instance().GetPriceByProductId(productId);
        }

        public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            return SubsitePromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
        }

        public static PromotionInfo GetPromotion(int activityId)
        {
            return SubsitePromotionsProvider.Instance().GetPromotion(activityId);
        }

        public static DataTable GetPromotionProducts(int activityId)
        {
            return SubsitePromotionsProvider.Instance().GetPromotionProducts(activityId);
        }

        public static DataTable GetPromotions(bool isProductPromote)
        {
            return SubsitePromotionsProvider.Instance().GetPromotions(isProductPromote);
        }

        public static bool ProductCountDownExist(int productId)
        {
            return SubsitePromotionsProvider.Instance().ProductCountDownExist(productId);
        }

        public static bool ProductGroupBuyExist(int productId)
        {
            return SubsitePromotionsProvider.Instance().ProductGroupBuyExist(productId);
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return SubsitePromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            return SubsitePromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
        }

        public static void SwapCountDownSequence(int countDownId, int displaySequence)
        {
            SubsitePromotionsProvider.Instance().SwapCountDownSequence(countDownId, displaySequence);
        }

        public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
        {
            SubsitePromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, displaySequence);
        }

        public static bool UpdateBundlingProduct(BundlingInfo bundlingInfo)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsitePromotionsProvider.Instance().UpdateBundlingProduct(bundlingInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().DeleteBundlingByID(bundlingInfo.BundlingID, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
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

        public static bool UpdateCountDown(CountDownInfo countDownInfo)
        {
            return SubsitePromotionsProvider.Instance().UpdateCountDown(countDownInfo);
        }

        public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!SubsitePromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!SubsitePromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    dbTran.Commit();
                    flag = true;
                }
                catch (Exception)
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
    }
}

