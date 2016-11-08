namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class ShoppingProcessor
    {
        public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
        {
            decimal price = 0M;
            int topRegionId = RegionHelper.GetTopRegionId(regionId);
            decimal num3 = totalWeight;
            int num4 = 1;
            if ((num3 > shippingModeInfo.Weight) && (shippingModeInfo.AddWeight.HasValue && (shippingModeInfo.AddWeight.Value > 0M)))
            {
                decimal num6 = num3 - shippingModeInfo.Weight;
                if ((num6 % shippingModeInfo.AddWeight) == 0M)
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
                }
                else
                {
                    num4 = Convert.ToInt32(Math.Truncate((decimal) ((num3 - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value))) + 1;
                }
            }
            if ((shippingModeInfo.ModeGroup == null) || (shippingModeInfo.ModeGroup.Count == 0))
            {
                if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
                {
                    return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
                }
                return shippingModeInfo.Price;
            }
            int? nullable = null;
            foreach (ShippingModeGroupInfo info in shippingModeInfo.ModeGroup)
            {
                foreach (ShippingRegionInfo info2 in info.ModeRegions)
                {
                    if (topRegionId == info2.RegionId)
                    {
                        nullable = new int?(info2.GroupId);
                        break;
                    }
                }
                if (nullable.HasValue)
                {
                    if (num3 > shippingModeInfo.Weight)
                    {
                        price = (num4 * info.AddPrice) + info.Price;
                    }
                    else
                    {
                        price = info.Price;
                    }
                    break;
                }
            }
            if (nullable.HasValue)
            {
                return price;
            }
            if ((num3 > shippingModeInfo.Weight) && shippingModeInfo.AddPrice.HasValue)
            {
                return ((num4 * shippingModeInfo.AddPrice.Value) + shippingModeInfo.Price);
            }
            return shippingModeInfo.Price;
        }

        public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
        {
            if (!paymentModeInfo.IsPercent)
            {
                return paymentModeInfo.Charge;
            }
            return (cartMoney * (paymentModeInfo.Charge / 100M));
        }

        public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isGroupBuy, bool isCountDown, bool isSignBuy)
        {
            if ((shoppingCart.LineItems.Count == 0) && (shoppingCart.LineGifts.Count == 0))
            {
                return null;
            }
            OrderInfo info = new OrderInfo();
            info.Points = shoppingCart.GetPoint();
            info.ReducedPromotionId = shoppingCart.ReducedPromotionId;
            info.ReducedPromotionName = shoppingCart.ReducedPromotionName;
            info.ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount;
            info.IsReduced = shoppingCart.IsReduced;
            info.SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId;
            info.SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName;
            info.IsSendTimesPoint = shoppingCart.IsSendTimesPoint;
            info.TimesPoint = shoppingCart.TimesPoint;
            info.FreightFreePromotionId = shoppingCart.FreightFreePromotionId;
            info.FreightFreePromotionName = shoppingCart.FreightFreePromotionName;
            info.IsFreightFree = shoppingCart.IsFreightFree;
            string str = string.Empty;
            if (shoppingCart.LineItems.Values.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems.Values)
                {
                    str = str + string.Format("'{0}',", info2.SkuId);
                }
            }
            Dictionary<string, decimal> costPriceForItems = new Dictionary<string, decimal>();
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
                costPriceForItems = ShoppingProvider.Instance().GetCostPriceForItems(str);
            }
            if (shoppingCart.LineItems.Values.Count > 0)
            {
                foreach (ShoppingCartItemInfo info2 in shoppingCart.LineItems.Values)
                {
                    decimal costPrice = 0M;
                    if ((isGroupBuy || isCountDown) || isSignBuy)
                    {
                        costPrice = ShoppingProvider.Instance().GetCostPrice(info2.SkuId);
                    }
                    else if (costPriceForItems.ContainsKey(info2.SkuId))
                    {
                        costPrice = costPriceForItems[info2.SkuId];
                    }
                    LineItemInfo info3 = new LineItemInfo();
                    info3.SkuId = info2.SkuId;
                    info3.ProductId = info2.ProductId;
                    info3.SKU = info2.SKU;
                    info3.Quantity = info2.Quantity;
                    info3.ShipmentQuantity = info2.ShippQuantity;
                    info3.ItemCostPrice = costPrice;
                    info3.ItemListPrice = info2.MemberPrice;
                    info3.ItemAdjustedPrice = info2.AdjustedPrice;
                    info3.ItemDescription = info2.Name;
                    info3.ThumbnailsUrl = info2.ThumbnailUrl40;
                    info3.ItemWeight = info2.Weight;
                    info3.SKUContent = info2.SkuContent;
                    info3.PromotionId = info2.PromotionId;
                    info3.PromotionName = info2.PromotionName;
                    info.LineItems.Add(info3.SkuId, info3);
                }
            }
            info.Tax = 0.00M;
            info.InvoiceTitle = "";
            if (shoppingCart.LineGifts.Count > 0)
            {
                foreach (ShoppingCartGiftInfo info4 in shoppingCart.LineGifts)
                {
                    OrderGiftInfo item = new OrderGiftInfo();
                    item.GiftId = info4.GiftId;
                    item.GiftName = info4.Name;
                    item.Quantity = info4.Quantity;
                    item.ThumbnailsUrl = info4.ThumbnailUrl40;
                    item.PromoteType = info4.PromoType;
                    if (HiContext.Current.SiteSettings.IsDistributorSettings)
                    {
                        item.CostPrice = info4.PurchasePrice;
                    }
                    else
                    {
                        item.CostPrice = info4.CostPrice;
                    }
                    info.Gifts.Add(item);
                }
            }
            return info;
        }

        public static int CountDownOrderCount(int productid)
        {
            return ShoppingProvider.Instance().CountDownOrderCount(productid);
        }

        public static bool CreatOrder(OrderInfo orderInfo)
        {
            if (orderInfo == null)
            {
                return false;
            }
            return ShoppingProvider.Instance().CreatOrder(orderInfo);
        }

        public static bool CutNeedPoint(int needPoint, string orderId)
        {
            Member user = HiContext.Current.User as Member;
            UserPointInfo point = new UserPointInfo();
            point.OrderId = orderId;
            point.UserId = user.UserId;
            point.TradeDate = DateTime.Now;
            point.TradeType = UserPointTradeType.Change;
            point.Reduced = new int?(needPoint);
            point.Points = user.Points - needPoint;
            if ((point.Points > 0x7fffffff) || (point.Points < 0))
            {
                point.Points = 0;
            }
            if (ShoppingProvider.Instance().AddMemberPoint(point))
            {
                Users.ClearUserCache(user);
                return true;
            }
            return false;
        }

        public static DataTable GetCoupon(decimal orderAmount)
        {
            return ShoppingProvider.Instance().GetCoupon(orderAmount);
        }

        public static CouponInfo GetCoupon(string couponCode)
        {
            return ShoppingProvider.Instance().GetCoupon(couponCode);
        }

        public static IList<string> GetExpressCompanysByMode(int modeId)
        {
            return ShoppingProvider.Instance().GetExpressCompanysByMode(modeId);
        }

        public static OrderInfo GetOrderInfo(string orderId)
        {
            return ShoppingProvider.Instance().GetOrderInfo(orderId);
        }

        public static PaymentModeInfo GetPaymentMode(int modeId)
        {
            return ShoppingProvider.Instance().GetPaymentMode(modeId);
        }

        public static IList<PaymentModeInfo> GetPaymentModes()
        {
            return ShoppingProvider.Instance().GetPaymentModes();
        }

        public static SKUItem GetProductAndSku(int productId, string options)
        {
            return ShoppingProvider.Instance().GetProductAndSku(productId, options);
        }

        public static DataTable GetProductInfoBySku(string skuId)
        {
            return ShoppingProvider.Instance().GetProductInfoBySku(skuId);
        }

        public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
        {
            return ShoppingProvider.Instance().GetPurchaseOrder(purchaseOrderId);
        }

        public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
        {
            return ShoppingProvider.Instance().GetShippingMode(modeId, includeDetail);
        }

        public static IList<ShippingModeInfo> GetShippingModes()
        {
            return ShoppingProvider.Instance().GetShippingModes();
        }

        public static IList<string> GetSkuIdsBysku(string sku)
        {
            return ShoppingProvider.Instance().GetSkuIdsBysku(sku);
        }

        public static int GetStock(int productId, string skuId)
        {
            return ShoppingProvider.Instance().GetStock(productId, skuId);
        }

        public static DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
        {
            return ShoppingProvider.Instance().GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
        }

        public static DataTable GetYetShipOrders(int days)
        {
            return ShoppingProvider.Instance().GetYetShipOrders(days);
        }

        public static DataTable UseCoupon(decimal orderAmount)
        {
            return GetCoupon(orderAmount);
        }

        public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
        {
            if (!string.IsNullOrEmpty(claimCode))
            {
                CouponInfo coupon = GetCoupon(claimCode);
                if (coupon != null)
                {
                    decimal? amount;
                    if (coupon.Amount.HasValue)
                    {
                        amount = coupon.Amount;
                        if (!((amount.GetValueOrDefault() > 0M) && amount.HasValue) || (orderAmount < coupon.Amount.Value))
                        {
                        }
                    }
                    if (!(coupon.Amount.HasValue && (!(((amount = coupon.Amount).GetValueOrDefault() == 0M) && amount.HasValue) || (orderAmount <= coupon.DiscountValue))))
                    {
                        return coupon;
                    }
                }
            }
            return null;
        }
    }
}

