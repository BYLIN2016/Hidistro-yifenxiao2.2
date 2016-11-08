namespace Hidistro.AccountCenter.DistributionData
{
    using Hidistro.AccountCenter.Business;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class PurchaseOrderData : PurchaseOrderProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        private PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order)
        {
            if (order == null)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            string query = "";
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                builder.AppendFormat("'" + info.SkuId + "',", new object[0]);
            }
            if (builder.Length > 0)
            {
                builder = builder.Remove(builder.Length - 1, 1);
                query = string.Format("SELECT S.SkuId, S.CostPrice, p.ProductName FROM Hishop_Products P JOIN Hishop_SKUs S ON P.ProductId = S.ProductId WHERE S.SkuId IN({0});", builder);
            }
            if (order.Gifts.Count > 0)
            {
                StringBuilder builder2 = new StringBuilder();
                foreach (OrderGiftInfo info2 in order.Gifts)
                {
                    builder2.AppendFormat(info2.GiftId.ToString() + ",", new object[0]);
                }
                builder2.Remove(builder2.Length - 1, 1);
                query = query + string.Format(" SELECT GiftId, CostPrice FROM Hishop_Gifts WHERE GiftId IN({0});", builder2.ToString());
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            Dictionary<string, PurchaseOrderItemInfo> dictionary = new Dictionary<string, PurchaseOrderItemInfo>();
            Dictionary<int, decimal> dictionary2 = new Dictionary<int, decimal>();
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (order.LineItems.Values.Count > 0)
                {
                    while (reader.Read())
                    {
                        PurchaseOrderItemInfo info3 = new PurchaseOrderItemInfo();
                        if (reader["CostPrice"] != DBNull.Value)
                        {
                            info3.ItemCostPrice = (decimal) reader["CostPrice"];
                        }
                        info3.ItemHomeSiteDescription = (string) reader["ProductName"];
                        dictionary.Add((string) reader["SkuId"], info3);
                    }
                }
                if (order.Gifts.Count > 0)
                {
                    if (order.LineItems.Count > 0)
                    {
                        reader.NextResult();
                    }
                    while (reader.Read())
                    {
                        dictionary2.Add((int) reader["GiftId"], (DBNull.Value == reader["CostPrice"]) ? 0M : Convert.ToDecimal(reader["CostPrice"]));
                    }
                }
            }
            IUser user = Users.GetUser(HiContext.Current.SiteSettings.UserId.Value, false);
            if ((user == null) || (user.UserRole != UserRole.Distributor))
            {
                return null;
            }
            Distributor distributor = user as Distributor;
            PurchaseOrderInfo info4 = new PurchaseOrderInfo {
                PurchaseOrderId = "PO" + order.OrderId,
                OrderId = order.OrderId,
                Remark = order.Remark,
                PurchaseStatus = OrderStatus.WaitBuyerPay,
                DistributorId = distributor.UserId,
                Distributorname = distributor.Username,
                DistributorEmail = distributor.Email,
                DistributorRealName = distributor.RealName,
                DistributorQQ = distributor.QQ,
                DistributorWangwang = distributor.Wangwang,
                DistributorMSN = distributor.MSN,
                ShippingRegion = order.ShippingRegion,
                Address = order.Address,
                ZipCode = order.ZipCode,
                ShipTo = order.ShipTo,
                TelPhone = order.TelPhone,
                CellPhone = order.CellPhone,
                ShipToDate = order.ShipToDate,
                ShippingModeId = order.ShippingModeId,
                ModeName = order.ModeName,
                RegionId = order.RegionId,
                Freight = order.Freight,
                AdjustedFreight = order.Freight,
                ShipOrderNumber = order.ShipOrderNumber,
                Weight = order.Weight,
                RefundStatus = RefundStatus.None,
                OrderTotal = order.GetTotal(),
                ExpressCompanyName = order.ExpressCompanyName,
                ExpressCompanyAbb = order.ExpressCompanyAbb,
                Tax = order.Tax,
                InvoiceTitle = order.InvoiceTitle
            };
            foreach (LineItemInfo info5 in order.LineItems.Values)
            {
                PurchaseOrderItemInfo item = new PurchaseOrderItemInfo {
                    PurchaseOrderId = info4.PurchaseOrderId,
                    SkuId = info5.SkuId,
                    ProductId = info5.ProductId,
                    SKU = info5.SKU,
                    Quantity = info5.ShipmentQuantity
                };
                foreach (KeyValuePair<string, PurchaseOrderItemInfo> pair in dictionary)
                {
                    if (pair.Key == info5.SkuId)
                    {
                        item.ItemCostPrice = pair.Value.ItemCostPrice;
                        item.ItemHomeSiteDescription = pair.Value.ItemHomeSiteDescription;
                    }
                }
                item.ItemPurchasePrice = info5.ItemCostPrice;
                item.ItemListPrice = info5.ItemListPrice;
                item.ItemDescription = info5.ItemDescription;
                item.SKUContent = info5.SKUContent;
                item.ThumbnailsUrl = info5.ThumbnailsUrl;
                item.ItemWeight = info5.ItemWeight;
                if (string.IsNullOrEmpty(item.ItemHomeSiteDescription))
                {
                    item.ItemHomeSiteDescription = item.ItemDescription;
                }
                info4.PurchaseOrderItems.Add(item);
            }
            foreach (OrderGiftInfo info7 in order.Gifts)
            {
                PurchaseOrderGiftInfo info8 = new PurchaseOrderGiftInfo {
                    PurchaseOrderId = info4.PurchaseOrderId
                };
                foreach (KeyValuePair<int, decimal> pair2 in dictionary2)
                {
                    if (pair2.Key == info7.GiftId)
                    {
                        info8.CostPrice = pair2.Value;
                    }
                }
                info8.PurchasePrice = info7.CostPrice;
                info8.GiftId = info7.GiftId;
                info8.GiftName = info7.GiftName;
                info8.Quantity = info7.Quantity;
                info8.ThumbnailsUrl = info7.ThumbnailsUrl;
                info4.PurchaseOrderGifts.Add(info8);
            }
            return info4;
        }

        public override bool CreatePurchaseOrder(OrderInfo order, DbTransaction dbTran)
        {
            string str;
            PurchaseOrderInfo info = this.ConvertOrderToPurchaseOrder(order);
            if (info == null)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_PurchaseOrders(PurchaseOrderId, OrderId, Remark, ManagerMark, ManagerRemark, AdjustedDiscount,PurchaseStatus, CloseReason, PurchaseDate, DistributorId, Distributorname,DistributorEmail, DistributorRealName, DistributorQQ, DistributorWangwang, DistributorMSN,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName,RealShippingModeId, RealModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, Weight,ExpressCompanyName,ExpressCompanyAbb,RefundStatus, RefundAmount, RefundRemark, OrderTotal, PurchaseProfit, PurchaseTotal,Tax,InvoiceTitle)VALUES (@PurchaseOrderId, @OrderId, @Remark, @ManagerMark, @ManagerRemark, @AdjustedDiscount,@PurchaseStatus, @CloseReason, @PurchaseDate, @DistributorId, @Distributorname,@DistributorEmail, @DistributorRealName, @DistributorQQ, @DistributorWangwang, @DistributorMSN,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName,@RealShippingModeId, @RealModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @PurchaseWeight,@ExpressCompanyName,@ExpressCompanyAbb,@RefundStatus, @RefundAmount, @RefundRemark, @OrderTotal, @PurchaseProfit, @PurchaseTotal,@Tax,@InvoiceTitle);");
            this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", DbType.String, info.PurchaseOrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, info.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Remark", DbType.String, info.Remark);
            if (info.ManagerMark.HasValue)
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, (int) info.ManagerMark.Value);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, DBNull.Value);
            }
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, info.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, info.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", DbType.Int32, (int) info.PurchaseStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, info.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PurchaseDate", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "DistributorId", DbType.Int32, info.DistributorId);
            this.database.AddInParameter(sqlStringCommand, "Distributorname", DbType.String, info.Distributorname);
            this.database.AddInParameter(sqlStringCommand, "DistributorEmail", DbType.String, info.DistributorEmail);
            this.database.AddInParameter(sqlStringCommand, "DistributorRealName", DbType.String, info.DistributorRealName);
            this.database.AddInParameter(sqlStringCommand, "DistributorQQ", DbType.String, info.DistributorQQ);
            this.database.AddInParameter(sqlStringCommand, "DistributorWangwang", DbType.String, info.DistributorWangwang);
            this.database.AddInParameter(sqlStringCommand, "DistributorMSN", DbType.String, info.DistributorMSN);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, info.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, info.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, info.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, info.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, info.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, info.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShipToDate", DbType.String, info.ShipToDate);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, info.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, info.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, info.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, info.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.Int32, info.RegionId);
            this.database.AddInParameter(sqlStringCommand, "Freight", DbType.Currency, info.Freight);
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, info.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, info.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "PurchaseWeight", DbType.Int32, info.Weight);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, info.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, info.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "RefundStatus", DbType.Int32, (int) info.RefundStatus);
            this.database.AddInParameter(sqlStringCommand, "RefundAmount", DbType.Currency, info.RefundAmount);
            this.database.AddInParameter(sqlStringCommand, "RefundRemark", DbType.String, info.RefundRemark);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, info.OrderTotal);
            this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", DbType.Currency, info.GetPurchaseProfit());
            this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", DbType.Currency, info.GetPurchaseTotal());
            this.database.AddInParameter(sqlStringCommand, "Tax", DbType.Currency, info.Tax);
            this.database.AddInParameter(sqlStringCommand, "InvoiceTitle", DbType.String, info.InvoiceTitle);
            int num = 0;
            foreach (PurchaseOrderItemInfo info2 in info.PurchaseOrderItems)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderItems(PurchaseOrderId, SkuId, ProductId, SKU, Quantity,  CostPrice, ").Append("ItemListPrice, ItemPurchasePrice, ItemDescription, ItemHomeSiteDescription, SKUContent, ThumbnailsUrl, Weight) VALUES( @PurchaseOrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemPurchasePrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ItemHomeSiteDescription").Append(str).Append(",@SKUContent").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info2.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info2.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info2.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info2.Quantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info2.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info2.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice" + str, DbType.Currency, info2.ItemPurchasePrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info2.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription" + str, DbType.String, info2.ItemHomeSiteDescription);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info2.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info2.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info2.ItemWeight);
                num++;
            }
            foreach (PurchaseOrderGiftInfo info3 in info.PurchaseOrderGifts)
            {
                str = num.ToString();
                builder.Append("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId, GiftId, GiftName, CostPrice, PurchasePrice, ").Append("ThumbnailsUrl, Quantity) VALUES( @PurchaseOrderId,").Append("@GiftId").Append(str).Append(",@GiftName").Append(str).Append(",@CostPrice").Append(str).Append(",@PurchasePrice").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Quantity").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "GiftId" + str, DbType.Int32, info3.GiftId);
                this.database.AddInParameter(sqlStringCommand, "GiftName" + str, DbType.String, info3.GiftName);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info3.Quantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info3.CostPrice);
                this.database.AddInParameter(sqlStringCommand, "PurchasePrice" + str, DbType.Currency, info3.PurchasePrice);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info3.ThumbnailsUrl);
                num++;
            }
            sqlStringCommand.CommandText = builder.ToString().Remove(builder.Length - 1);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

