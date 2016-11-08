namespace Hidistro.AccountCenter.Business
{
    using Hidistro.AccountCenter.Profile;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public static class TradeHelper
    {
        public static int AddClaimCodeToUser(string claimCode, int userId)
        {
            return TradeProvider.Instance().AddClaimCodeToUser(claimCode, userId);
        }

        public static bool CloseOrder(string orderId)
        {
            return TradeProvider.Instance().CloseOrder(orderId);
        }

        public static bool ConfirmOrderFinish(OrderInfo order)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
            {
                flag = TradeProvider.Instance().ConfirmOrderFinish(order);
            }
            return flag;
        }

        public static bool ExitCouponClaimCode(string claimCode)
        {
            return TradeProvider.Instance().ExitCouponClaimCode(claimCode);
        }

        public static DataTable GetChangeCoupons()
        {
            return TradeProvider.Instance().GetChangeCoupons();
        }

        public static CountDownInfo GetCountDownBuy(int CountDownId)
        {
            return TradeProvider.Instance().CountDownBuy(CountDownId);
        }

        public static GroupBuyInfo GetGroupBuy(int groupBuyId)
        {
            return TradeProvider.Instance().GetGroupBuy(groupBuyId);
        }

        public static int GetOrderCount(int groupBuyId)
        {
            return TradeProvider.Instance().GetOrderCount(groupBuyId);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return TradeProvider.Instance().GetOrderInfo(orderId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return TradeProvider.Instance().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return TradeProvider.Instance().GetPaymentModes();
        }

        public static DataTable GetUserCoupons(int userId)
        {
            return TradeProvider.Instance().GetUserCoupons(userId);
        }

        public static DbQueryResult GetUserOrder(int userId, OrderQuery query)
        {
            return TradeProvider.Instance().GetUserOrder(userId, query);
        }

        public static DbQueryResult GetUserPoints(int pageIndex)
        {
            return TradeProvider.Instance().GetUserPoints(pageIndex);
        }

        public static bool PointChageCoupon(int couponId, int needPoint, int currentPoint)
        {
            Member user = HiContext.Current.User as Member;
            UserPointInfo point = new UserPointInfo {
                OrderId = string.Empty,
                UserId = user.UserId,
                TradeDate = DateTime.Now,
                TradeType = UserPointTradeType.ChangeCoupon,
                Increased = null,
                Reduced = new int?(needPoint),
                Points = currentPoint - needPoint
            };
            if ((point.Points >= 0) && TradeProvider.Instance().AddMemberPoint(point))
            {
                CouponItemInfo couponItem = new CouponItemInfo {
                    CouponId = couponId,
                    UserId = new int?(user.UserId),
                    UserName = user.Username,
                    EmailAddress = user.Email,
                    ClaimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15),
                    GenerateTime = DateTime.Now
                };
                Users.ClearUserCache(user);
                if (TradeProvider.Instance().SendClaimCodes(couponItem))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SetGroupBuyEndUntreated(int groupBuyId)
        {
            return TradeProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
        }

        public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder)
        {
            bool flag = false;
            if (order.CheckAction(OrderActions.BUYER_PAY))
            {
                using (DbConnection connection = DatabaseFactory.CreateDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction dbTran = connection.BeginTransaction();
                    try
                    {
                        if (!TradeProvider.Instance().UserPayOrder(order, isBalancePayOrder, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        if ((HiContext.Current.SiteSettings.IsDistributorSettings && (order.GroupBuyId <= 0)) && !PurchaseOrderProvider.CreateInstance().CreatePurchaseOrder(order, dbTran))
                        {
                            dbTran.Rollback();
                            return false;
                        }
                        flag = true;
                        dbTran.Commit();
                    }
                    catch
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                if (!flag)
                {
                    return flag;
                }
                if (!HiContext.Current.SiteSettings.IsDistributorSettings)
                {
                    TradeProvider.Instance().UpdateStockPayOrder(order.OrderId);
                }
                TradeProvider.Instance().UpdateProductSaleCounts(order.LineItems);
                if ((order.UserId == 0) || (order.UserId == 0x44c))
                {
                    return flag;
                }
                IUser user = Users.GetUser(order.UserId, false);
                if (((user == null) || (user.UserRole != UserRole.Member)) && ((user == null) || (user.UserRole != UserRole.Underling)))
                {
                    return flag;
                }
                Member member = user as Member;
                UserPointInfo point = new UserPointInfo {
                    OrderId = order.OrderId,
                    UserId = member.UserId,
                    TradeDate = DateTime.Now,
                    TradeType = UserPointTradeType.Bounty,
                    Increased = new int?(order.Points),
                    Points = order.Points + member.Points
                };
                if ((point.Points > 0x7fffffff) || (point.Points < 0))
                {
                    point.Points = 0x7fffffff;
                }
                TradeProvider.Instance().AddMemberPoint(point);
                int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
                if (((referralDeduct > 0) && member.ReferralUserId.HasValue) && (member.ReferralUserId.Value > 0))
                {
                    IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
                    if ((user2 != null) && ((user2.UserRole == UserRole.Member) || (user2.UserRole == UserRole.Underling)))
                    {
                        Member member2 = user2 as Member;
                        if ((member.ParentUserId == member2.ParentUserId) && member2.IsOpenBalance)
                        {
                            decimal num2 = member2.Balance + ((order.GetTotal() * referralDeduct) / 100M);
                            BalanceDetailInfo balanceDetails = new BalanceDetailInfo {
                                UserId = member2.UserId,
                                UserName = member2.Username,
                                TradeDate = DateTime.Now,
                                TradeType = TradeTypes.ReferralDeduct,
                                Income = new decimal?((order.GetTotal() * referralDeduct) / 100M),
                                Balance = num2,
                                Remark = string.Format("提成来自'{0}'的订单{1}", order.Username, order.OrderId)
                            };
                            PersonalProvider.Instance().AddBalanceDetail(balanceDetails);
                        }
                    }
                }
                TradeProvider.Instance().UpdateUserAccount(order.GetTotal(), point.Points, order.UserId);
                int historyPoint = TradeProvider.Instance().GetHistoryPoint(member.UserId);
                TradeProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
                Users.ClearUserCache(user);
            }
            return flag;
        }
    }
}

