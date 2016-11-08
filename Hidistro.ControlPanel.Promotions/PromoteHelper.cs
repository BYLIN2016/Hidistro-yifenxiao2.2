namespace Hidistro.ControlPanel.Promotions
{
    using Hidistro.Core;
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

    public static class PromoteHelper
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
                    int bundlingID = PromotionsProvider.Instance().AddBundlingProduct(bundlingInfo, dbTran);
                    if (bundlingID <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddBundlingProductItems(bundlingID, bundlingInfo.BundlingItemInfos, dbTran))
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
            return PromotionsProvider.Instance().AddCountDown(countDownInfo);
        }

        public static bool AddGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    int groupBuyId = PromotionsProvider.Instance().AddGroupBuy(groupBuy, dbTran);
                    if (groupBuyId <= 0)
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddGroupBuyCondition(groupBuyId, groupBuy.GroupBuyConditions, dbTran))
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
                    int activityId = PromotionsProvider.Instance().AddPromotion(promotion, dbTran);
                    if (activityId <= 0)
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!PromotionsProvider.Instance().AddPromotionMemberGrades(activityId, promotion.MemberGradeIds, dbTran))
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
            return PromotionsProvider.Instance().AddPromotionProducts(activityId, productIds);
        }

        public static bool DeleteBundlingProduct(int bundlingID)
        {
            return PromotionsProvider.Instance().DeleteBundlingProduct(bundlingID);
        }

        public static bool DeleteCountDown(int countDownId)
        {
            return PromotionsProvider.Instance().DeleteCountDown(countDownId);
        }

        public static bool DeleteGroupBuy(int groupBuyId)
        {
            return PromotionsProvider.Instance().DeleteGroupBuy(groupBuyId);
        }

        public static bool DeletePromotion(int activityId)
        {
            return PromotionsProvider.Instance().DeletePromotion(activityId);
        }

        public static bool DeletePromotionProducts(int activityId, int? productId)
        {
            return PromotionsProvider.Instance().DeletePromotionProducts(activityId, productId);
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
                    if (!PromotionsProvider.Instance().EditPromotion(promotion, dbTran))
                    {
                        dbTran.Rollback();
                        return -1;
                    }
                    if (!PromotionsProvider.Instance().AddPromotionMemberGrades(promotion.ActivityId, promotion.MemberGradeIds, dbTran))
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
            return PromotionsProvider.Instance().GetBundlingInfo(bundlingID);
        }

        public static DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
        {
            return PromotionsProvider.Instance().GetBundlingProducts(query);
        }

        public static CountDownInfo GetCountDownInfo(int countDownId)
        {
            return PromotionsProvider.Instance().GetCountDownInfo(countDownId);
        }

        public static DbQueryResult GetCountDownList(GroupBuyQuery query)
        {
            return PromotionsProvider.Instance().GetCountDownList(query);
        }

        public static DbQueryResult GetCouponsList(CouponItemInfoQuery couponquery)
        {
            return PromotionsProvider.Instance().GetCouponsList(couponquery);
        }

        public static decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
        {
            return PromotionsProvider.Instance().GetCurrentPrice(groupBuyId, prodcutQuantity);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return PromotionsProvider.Instance().GetGroupBuy(groupBuyId);
        }

        public static DbQueryResult GetGroupBuyList(GroupBuyQuery query)
        {
            return PromotionsProvider.Instance().GetGroupBuyList(query);
        }

        public static IList<Member> GetMembersByRank(int? gradeId)
        {
            return PromotionsProvider.Instance().GetMembersByRank(gradeId);
        }

        public static IList<Member> GetMemdersByNames(IList<string> names)
        {
            IList<Member> list = new List<Member>();
            foreach (string str in names)
            {
                IUser user = Users.GetUser(0, str, false, false);
                if ((user != null) && (user.UserRole == UserRole.Member))
                {
                    list.Add(user as Member);
                }
            }
            return list;
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return PromotionsProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static string GetPriceByProductId(int productId)
        {
            return PromotionsProvider.Instance().GetPriceByProductId(productId);
        }

        public static IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
        {
            return PromotionsProvider.Instance().GetPromoteMemberGrades(activityId);
        }

        public static PromotionInfo GetPromotion(int activityId)
        {
            return PromotionsProvider.Instance().GetPromotion(activityId);
        }

        public static DataTable GetPromotionProducts(int activityId)
        {
            return PromotionsProvider.Instance().GetPromotionProducts(activityId);
        }

        public static DataTable GetPromotions(bool isProductPromote, bool isWholesale)
        {
            return PromotionsProvider.Instance().GetPromotions(isProductPromote, isWholesale);
        }

        public static bool ProductCountDownExist(int productId)
        {
            return PromotionsProvider.Instance().ProductCountDownExist(productId);
        }

        public static bool ProductGroupBuyExist(int productId)
        {
            return PromotionsProvider.Instance().ProductGroupBuyExist(productId);
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return PromotionsProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
        {
            return PromotionsProvider.Instance().SetGroupBuyStatus(groupBuyId, status);
        }

        public static void SwapCountDownSequence(int countDownId, int displaySequence)
        {
            PromotionsProvider.Instance().SwapCountDownSequence(countDownId, displaySequence);
        }

        public static void SwapGroupBuySequence(int groupBuyId, int displaySequence)
        {
            PromotionsProvider.Instance().SwapGroupBuySequence(groupBuyId, displaySequence);
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
                    if (!PromotionsProvider.Instance().UpdateBundlingProduct(bundlingInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().DeleteBundlingByID(bundlingInfo.BundlingID, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddBundlingProductItems(bundlingInfo.BundlingID, bundlingInfo.BundlingItemInfos, dbTran))
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
            return PromotionsProvider.Instance().UpdateCountDown(countDownInfo);
        }

        public static bool UpdateGroupBuy(GroupBuyInfo groupBuy)
        {
            bool flag;
            Globals.EntityCoding(groupBuy, true);
            using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!PromotionsProvider.Instance().UpdateGroupBuy(groupBuy, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().DeleteGroupBuyCondition(groupBuy.GroupBuyId, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!PromotionsProvider.Instance().AddGroupBuyCondition(groupBuy.GroupBuyId, groupBuy.GroupBuyConditions, dbTran))
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

