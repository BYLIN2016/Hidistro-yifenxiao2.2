namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ShoppingData : ShoppingMasterProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        private bool AddCouponUseRecord(OrderInfo orderinfo, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode and CouponStatus!=1");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, orderinfo.CouponCode);
            this.database.AddInParameter(sqlStringCommand, "userName", DbType.String, orderinfo.Username);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, orderinfo.UserId);
            this.database.AddInParameter(sqlStringCommand, "CouponStatus", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "EmailAddress", DbType.String, orderinfo.EmailAddress);
            this.database.AddInParameter(sqlStringCommand, "Orderid", DbType.String, orderinfo.OrderId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType) UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType) VALUES (@UserId, @GiftId, @Quantity, @AddTime,@PromoType)");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "AddTime", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int) promotype);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override void AddLineItem(Member member, string skuId, int quantity)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public override bool AddMemberPoint(UserPointInfo point)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, point.OrderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, point.UserId);
            this.database.AddInParameter(sqlStringCommand, "TradeDate", DbType.DateTime, point.TradeDate);
            this.database.AddInParameter(sqlStringCommand, "TradeType", DbType.Int32, (int) point.TradeType);
            this.database.AddInParameter(sqlStringCommand, "Increased", DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Reduced", DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
            this.database.AddInParameter(sqlStringCommand, "Points", DbType.Int32, point.Points);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, point.Remark);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        private bool AddOrderGiftItems(string orderId, IList<OrderGiftInfo> orderGifts, DbTransaction dbTran)
        {
            if ((orderGifts == null) || (orderGifts.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (OrderGiftInfo info in orderGifts)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO Hishop_OrderGifts(OrderId, GiftId, GiftName, CostPrice, ThumbnailsUrl, Quantity,PromoType) VALUES( @OrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(",@PromoType").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info.GiftId);
                this.database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info.GiftName);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.CostPrice);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "PromoType" + str, DbType.Int32, info.PromoteType);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

        private bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            if ((lineItems == null) || (lineItems.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in lineItems)
            {
                string str = num.ToString(CultureInfo.InvariantCulture);
                builder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName) VALUES( @OrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@ShipmentQuantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemAdjustedPrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(",@SKUContent").Append(str).Append(",@PromotionId").Append(str).Append(",@PromotionName").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + str, DbType.Int32, info.ShipmentQuantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + str, DbType.Currency, info.ItemAdjustedPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "PromotionId" + str, DbType.Int32, info.PromotionId);
                this.database.AddInParameter(sqlStringCommand, "PromotionName" + str, DbType.String, info.PromotionName);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length > 0)
            {
                sqlStringCommand.CommandText = builder.ToString();
                if (dbTran != null)
                {
                    return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
                }
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            return true;
        }

        public override void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId; DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override int CountDownOrderCount(int productid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Hishop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Hishop_Orders where userid=@userid and orderstatus!=4 AND ISNULL(CountDownBuyId, 0) > 0)");
            this.database.AddInParameter(sqlStringCommand, "productid", DbType.Int32, productid);
            this.database.AddInParameter(sqlStringCommand, "userid", DbType.Int32, HiContext.Current.User.UserId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public override bool CreatOrder(OrderInfo orderInfo)
        {
            bool flag = false;
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction dbTran = connection.BeginTransaction();
                try
                {
                    if (!this.CreatOrder(orderInfo, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.LineItems.Count > 0) && !this.AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if ((orderInfo.Gifts.Count > 0) && !this.AddOrderGiftItems(orderInfo.OrderId, orderInfo.Gifts, dbTran))
                    {
                        dbTran.Rollback();
                        return false;
                    }
                    if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !this.AddCouponUseRecord(orderInfo, dbTran))
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
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        private bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int) orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int) orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, orderInfo.Gateway);
            this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal());
            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.Points);
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
            this.database.AddInParameter(storedProcCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
            this.database.AddInParameter(storedProcCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
            this.database.AddInParameter(storedProcCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CountDownBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.BundlingID > 0)
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, orderInfo.BundlingID);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, orderInfo.BundlingPrice);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, DBNull.Value);
            }
            this.database.AddInParameter(storedProcCommand, "Tax", DbType.Currency, orderInfo.Tax);
            this.database.AddInParameter(storedProcCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1);
        }

        public override ShoppingCartItemInfo GetCartItemInfo(Member member, string skuId, int quantity)
        {
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (reader.Read())
                {
                    info = new ShoppingCartItemInfo();
                    info.SkuId = skuId;
                    info.Quantity = info.ShippQuantity = quantity;
                    info.ProductId = (int) reader["ProductId"];
                    if (reader["SKU"] != DBNull.Value)
                    {
                        info.SKU = (string) reader["SKU"];
                    }
                    info.Name = (string) reader["ProductName"];
                    if (DBNull.Value != reader["Weight"])
                    {
                        info.Weight = (int) reader["Weight"];
                    }
                    info.MemberPrice = info.AdjustedPrice = (decimal) reader["SalePrice"];
                    if (DBNull.Value != reader["ThumbnailUrl40"])
                    {
                        info.ThumbnailUrl40 = reader["ThumbnailUrl40"].ToString();
                    }
                    if (DBNull.Value != reader["ThumbnailUrl60"])
                    {
                        info.ThumbnailUrl60 = reader["ThumbnailUrl60"].ToString();
                    }
                    if (DBNull.Value != reader["ThumbnailUrl100"])
                    {
                        info.ThumbnailUrl100 = reader["ThumbnailUrl100"].ToString();
                    }
                    string str = string.Empty;
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) reader["ValueStr"])))
                            {
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                            }
                        }
                    }
                    info.SkuContent = str;
                    PromotionInfo info2 = null;
                    if (reader.NextResult() && reader.Read())
                    {
                        info2 = DataMapper.PopulatePromote(reader);
                    }
                    if (info2 != null)
                    {
                        switch (info2.PromoteType)
                        {
                            case PromoteType.Discount:
                                info.PromotionId = info2.ActivityId;
                                info.PromotionName = info2.Name;
                                info.AdjustedPrice = info.MemberPrice * info2.DiscountValue;
                                return info;

                            case PromoteType.Amount:
                                info.PromotionId = info2.ActivityId;
                                info.PromotionName = info2.Name;
                                info.AdjustedPrice = info2.DiscountValue;
                                return info;

                            case PromoteType.Reduced:
                                info.PromotionId = info2.ActivityId;
                                info.PromotionName = info2.Name;
                                info.AdjustedPrice = info.MemberPrice - info2.DiscountValue;
                                return info;

                            case PromoteType.QuantityDiscount:
                                if (info.Quantity >= ((int) info2.Condition))
                                {
                                    info.PromotionId = info2.ActivityId;
                                    info.PromotionName = info2.Name;
                                    info.AdjustedPrice = info.MemberPrice * info2.DiscountValue;
                                }
                                return info;

                            case PromoteType.SentGift:
                                info.PromotionId = info2.ActivityId;
                                info.PromotionName = info2.Name;
                                info.IsSendGift = true;
                                return info;

                            case PromoteType.SentProduct:
                                if ((info.Quantity / ((int) info2.Condition)) >= 1)
                                {
                                    info.PromotionId = info2.ActivityId;
                                    info.PromotionName = info2.Name;
                                    info.ShippQuantity = info.Quantity + ((info.Quantity / ((int) info2.Condition)) * ((int) info2.DiscountValue));
                                }
                                return info;
                        }
                    }
                }
                return info;
            }
        }

        public override decimal GetCostPrice(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CostPrice FROM Hishop_SKUs WHERE SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (decimal) obj2;
            }
            return 0M;
        }

        public override Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT s.SkuId, s.CostPrice FROM Hishop_SKUs s WHERE SkuId IN ({0})", skuIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    decimal num = (reader["CostPrice"] == DBNull.Value) ? 0M : ((decimal) reader["CostPrice"]);
                    dictionary.Add((string) reader["SkuId"], num);
                }
            }
            return dictionary;
        }

        public override DataTable GetCoupon(decimal orderAmount)
        {
            DataTable table = new DataTable();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ci.ClaimCode,c.DiscountValue,(ClaimCode+'　　　　　面值'+cast(DiscountValue as varchar(10))) as DisplayText FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId");
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            this.database.AddInParameter(sqlStringCommand, "orderAmount", DbType.Decimal, orderAmount);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override CouponInfo GetCoupon(string couponCode)
        {
            CouponInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.* FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where ci.ClaimCode =@ClaimCode   and  CouponStatus=0 AND  @DateTime>c.StartTime and  @DateTime <c.ClosingTime");
            this.database.AddInParameter(sqlStringCommand, "ClaimCode", DbType.String, couponCode);
            this.database.AddInParameter(sqlStringCommand, "DateTime", DbType.DateTime, DateTime.UtcNow);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateCoupon(reader);
                }
            }
            return info;
        }

        public override IList<string> GetExpressCompanysByMode(int modeId)
        {
            IList<string> list = new List<string>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        list.Add((string) reader["ExpressCompanyName"]);
                    }
                }
            }
            return list;
        }

        public override int GetGiftItemQuantity(PromoteType promotype)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(Quantity),0) as Quantity FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int) promotype);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    num = int.Parse(reader["Quantity"].ToString());
                }
            }
            return num;
        }

        public override OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            StringBuilder builder = new StringBuilder();
            builder.Append(" SELECT * FROM Hishop_Orders Where OrderId = @OrderId ");
            builder.Append(" SELECT * FROM Hishop_OrderGifts Where OrderId = @OrderId ");
            builder.Append(" SELECT o.*,(SELECT Stock FROM Hishop_SKUs WHERE SkuId=o.SkuId)  as Stock FROM Hishop_OrderItems o Where o.OrderId = @OrderId  ");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.Gifts.Add(DataMapper.PopulateOrderGift(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add((string) reader["SkuId"], DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public override PaymentModeInfo GetPaymentMode(int modeId)
        {
            PaymentModeInfo info = new PaymentModeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId;");
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

        public override IList<PaymentModeInfo> GetPaymentModes()
        {
            IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
            string query = "SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePayment(reader));
                }
            }
            return list;
        }

        public override SKUItem GetProductAndSku(int productId, string options)
        {
            if (string.IsNullOrEmpty(options))
            {
                return null;
            }
            string[] strArray = options.Split(new char[] { ',' });
            if ((strArray == null) || (strArray.Length <= 0))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            if (HiContext.Current.User.UserRole == UserRole.Member)
            {
                Member user = HiContext.Current.User as Member;
                int memberDiscount = MemberProvider.Instance().GetMemberDiscount(user.GradeId);
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
                builder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", user.GradeId);
                builder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", user.GradeId, memberDiscount);
                builder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
            }
            else
            {
                builder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
            }
            foreach (string str in strArray)
            {
                string[] strArray2 = str.Split(new char[] { ':' });
                builder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", strArray2[0], strArray2[1]);
            }
            SKUItem item = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    item = DataMapper.PopulateSKU(reader);
                }
            }
            return item;
        }

        public override DataTable GetProductInfoBySku(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            PurchaseOrderInfo info = new PurchaseOrderInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, purchaseOrderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
                if (DBNull.Value != reader["ExpressCompanyAbb"])
                {
                    info.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
                }
                if (DBNull.Value != reader["ExpressCompanyName"])
                {
                    info.ExpressCompanyName = (string) reader["ExpressCompanyName"];
                }
                if (DBNull.Value != reader["ShipOrderNumber"])
                {
                    info.ShipOrderNumber = (string) reader["ShipOrderNumber"];
                }
                if (DBNull.Value != reader["PurchaseStatus"])
                {
                    info.PurchaseStatus = (OrderStatus) reader["PurchaseStatus"];
                }
            }
            return info;
        }

        public override PromotionInfo GetReducedPromotion(Member member, decimal amount, int quantity, out decimal reducedAmount)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType BETWEEN 11 AND 14 AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            IList<PromotionInfo> list = new List<PromotionInfo>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulatePromote(reader));
                }
            }
            PromotionInfo info = null;
            reducedAmount = 0M;
            foreach (PromotionInfo info2 in list)
            {
                switch (info2.PromoteType)
                {
                    case PromoteType.FullAmountDiscount:
                        if ((amount >= info2.Condition) && ((amount - (amount * info2.DiscountValue)) > reducedAmount))
                        {
                            reducedAmount = amount - (amount * info2.DiscountValue);
                            info = info2;
                        }
                        break;

                    case PromoteType.FullAmountReduced:
                        if ((amount >= info2.Condition) && (info2.DiscountValue > reducedAmount))
                        {
                            reducedAmount = info2.DiscountValue;
                            info = info2;
                        }
                        break;

                    case PromoteType.FullQuantityDiscount:
                        if ((quantity >= ((int) info2.Condition)) && ((amount - (amount * info2.DiscountValue)) > reducedAmount))
                        {
                            reducedAmount = amount - (amount * info2.DiscountValue);
                            info = info2;
                        }
                        break;

                    case PromoteType.FullQuantityReduced:
                        if ((quantity >= ((int) info2.Condition)) && (info2.DiscountValue > reducedAmount))
                        {
                            reducedAmount = info2.DiscountValue;
                            info = info2;
                        }
                        break;
                }
            }
            return info;
        }

        public override PromotionInfo GetSendPromotion(Member member, decimal amount, PromoteType promoteType)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND Condition <= @Condition AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId) ORDER BY DiscountValue DESC");
            this.database.AddInParameter(sqlStringCommand, "PromoteType", DbType.Int32, (int) promoteType);
            this.database.AddInParameter(sqlStringCommand, "Condition", DbType.Currency, amount);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, member.GradeId);
            PromotionInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulatePromote(reader);
                }
            }
            return info;
        }

        public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            ShippingModeInfo info = null;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId;");
            if (includeDetail)
            {
                builder.Append("SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes WHERE ModeId =@ModeId);");
                builder.Append("SELECT TemplateId,GroupId,RegionId FROM Hishop_ShippingRegions Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId);");
                builder.Append(" SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ModeId", DbType.Int32, modeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateShippingMode(reader);
                }
                if (!includeDetail)
                {
                    return info;
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(reader));
                }
                reader.NextResult();
                while (reader.Read())
                {
                    foreach (ShippingModeGroupInfo info2 in info.ModeGroup)
                    {
                        if (info2.GroupId == ((int) reader["GroupId"]))
                        {
                            info2.ModeRegions.Add(DataMapper.PopulateShippingRegion(reader));
                        }
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    if (reader["ExpressCompanyName"] != DBNull.Value)
                    {
                        info.ExpressCompany.Add((string) reader["ExpressCompanyName"]);
                    }
                }
            }
            return info;
        }

        public override IList<ShippingModeInfo> GetShippingModes()
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            string query = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public override ShoppingCartInfo GetShoppingCart(int userId)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                Member user = HiContext.Current.User as Member;
                while (reader.Read())
                {
                    ShoppingCartItemInfo info2 = this.GetCartItemInfo(user, (string) reader["SkuId"], (int) reader["Quantity"]);
                    if (info2 != null)
                    {
                        info.LineItems.Add((string) reader["SkuId"], info2);
                    }
                }
                reader.NextResult();
                while (reader.Read())
                {
                    ShoppingCartGiftInfo item = DataMapper.PopulateGiftCartItem(reader);
                    item.Quantity = (int) reader["Quantity"];
                    info.LineGifts.Add(item);
                }
            }
            return info;
        }

        public override IList<string> GetSkuIdsBysku(string sku)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE SKU = @SKU");
            this.database.AddInParameter(sqlStringCommand, "SKU", DbType.String, sku);
            IList<string> list = new List<string>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    list.Add((string) reader["SkuId"]);
                }
            }
            return list;
        }

        public override int GetSkuStock(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

        public override int GetStock(int productId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE ProductId = @ProductId AND SkuId= @SkuId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                return 0;
            }
            return (int) obj2;
        }

        public override DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SKUItems WHERE SKUId IN (SELECT SKUId FROM Hishop_SKUs WHERE ProductId = @ProductId) AND (SKUId in (SELECT SKUId FROM Hishop_SKUItems WHERE AttributeId = @AttributeId AND ValueId=@ValueId) OR AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetYetShipOrders(int days)
        {
            if (days <= 0)
            {
                return null;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Hishop_Orders where OrderStatus=@OrderStatus and ShippingDate>=@FromDate and ShippingDate<=@ToDate;");
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 3);
            this.database.AddInParameter(sqlStringCommand, "FromDate", DbType.DateTime, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays((double) -days));
            this.database.AddInParameter(sqlStringCommand, "ToDate", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void RemoveGiftItem(int giftId, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int) promotype);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void RemoveLineItem(int userId, string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            this.database.AddInParameter(sqlStringCommand, "GiftId", DbType.Int32, giftId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "PromoType", DbType.Int32, (int) promotype);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public override void UpdateLineItemQuantity(Member member, string skuId, int quantity)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

