namespace Hidistro.SaleSystem.Shopping
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Collections.Generic;

    public static class ShoppingCartProcessor
    {
        public static bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                return CookieShoppingProvider.Instance().AddGiftItem(giftId, quantity);
            }
            return ShoppingProvider.Instance().AddGiftItem(giftId, quantity, promotype);
        }

        public static void AddLineItem(string skuId, int quantity)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                quantity = 1;
            }
            if (user != null)
            {
                ShoppingProvider.Instance().AddLineItem(user, skuId, quantity);
            }
            else
            {
                CookieShoppingProvider.Instance().AddLineItem(skuId, quantity);
            }
        }

        public static void ClearShoppingCart()
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().ClearShoppingCart();
            }
            else
            {
                ShoppingProvider.Instance().ClearShoppingCart(HiContext.Current.User.UserId);
            }
        }

        public static void ConvertShoppingCartToDataBase(ShoppingCartInfo shoppingCart)
        {
            Member user = HiContext.Current.User as Member;
            if (user != null)
            {
                if (shoppingCart.LineItems.Count > 0)
                {
                    foreach (ShoppingCartItemInfo info in shoppingCart.LineItems.Values)
                    {
                        ShoppingProvider.Instance().AddLineItem(user, info.SkuId, info.Quantity);
                    }
                }
                if (shoppingCart.LineGifts.Count > 0)
                {
                    foreach (ShoppingCartGiftInfo info2 in shoppingCart.LineGifts)
                    {
                        ShoppingProvider.Instance().AddGiftItem(info2.GiftId, info2.Quantity, (PromoteType) info2.PromoType);
                    }
                }
            }
        }

        public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            Member user = HiContext.Current.User as Member;
            ShoppingCartItemInfo info2 = ShoppingProvider.Instance().GetCartItemInfo(user, productSkuId, buyAmount);
            if (info2 == null)
            {
                return null;
            }
            CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(info2.ProductId);
            if (countDownInfo == null)
            {
                return null;
            }
            ShoppingCartItemInfo info4 = new ShoppingCartItemInfo();
            info4.SkuId = info2.SkuId;
            info4.ProductId = info2.ProductId;
            info4.SKU = info2.SKU;
            info4.Name = info2.Name;
            info4.MemberPrice = info4.AdjustedPrice = countDownInfo.CountDownPrice;
            info4.SkuContent = info2.SkuContent;
            info4.Quantity = info4.ShippQuantity = buyAmount;
            info4.Weight = info2.Weight;
            info4.ThumbnailUrl40 = info2.ThumbnailUrl40;
            info4.ThumbnailUrl60 = info2.ThumbnailUrl60;
            info4.ThumbnailUrl100 = info2.ThumbnailUrl100;
            info.LineItems.Add(productSkuId, info4);
            return info;
        }

        public static int GetGiftItemQuantity(PromoteType promotype)
        {
            return ShoppingProvider.Instance().GetGiftItemQuantity(promotype);
        }

        public static ShoppingCartInfo GetGroupBuyShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            Member user = HiContext.Current.User as Member;
            ShoppingCartItemInfo info2 = ShoppingProvider.Instance().GetCartItemInfo(user, productSkuId, buyAmount);
            if (info2 == null)
            {
                return null;
            }
            GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(info2.ProductId);
            if (productGroupBuyInfo == null)
            {
                return null;
            }
            int orderCount = ProductBrowser.GetOrderCount(productGroupBuyInfo.GroupBuyId);
            decimal currentPrice = ProductBrowser.GetCurrentPrice(productGroupBuyInfo.GroupBuyId, orderCount);
            ShoppingCartItemInfo info4 = new ShoppingCartItemInfo();
            info4.SkuId = info2.SkuId;
            info4.ProductId = info2.ProductId;
            info4.SKU = info2.SKU;
            info4.Name = info2.Name;
            info4.MemberPrice = info4.AdjustedPrice = currentPrice;
            info4.SkuContent = info2.SkuContent;
            info4.Quantity = info4.ShippQuantity = buyAmount;
            info4.Weight = info2.Weight;
            info4.ThumbnailUrl40 = info2.ThumbnailUrl40;
            info4.ThumbnailUrl60 = info2.ThumbnailUrl60;
            info4.ThumbnailUrl100 = info2.ThumbnailUrl100;
            info.LineItems.Add(productSkuId, info4);
            return info;
        }

        public static ShoppingCartInfo GetShoppingCart()
        {
            Member user = HiContext.Current.User as Member;
            if (user != null)
            {
                ShoppingCartInfo shoppingCart = ShoppingProvider.Instance().GetShoppingCart(HiContext.Current.User.UserId);
                if ((shoppingCart.LineItems.Count == 0) && (shoppingCart.LineGifts.Count == 0))
                {
                    return null;
                }
                decimal reducedAmount = 0M;
                PromotionInfo info2 = ShoppingProvider.Instance().GetReducedPromotion(user, shoppingCart.GetAmount(), shoppingCart.GetQuantity(), out reducedAmount);
                if (info2 != null)
                {
                    shoppingCart.ReducedPromotionId = info2.ActivityId;
                    shoppingCart.ReducedPromotionName = info2.Name;
                    shoppingCart.ReducedPromotionAmount = reducedAmount;
                    shoppingCart.IsReduced = true;
                }
                PromotionInfo info3 = ShoppingProvider.Instance().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentGift);
                if (info3 != null)
                {
                    shoppingCart.SendGiftPromotionId = info3.ActivityId;
                    shoppingCart.SendGiftPromotionName = info3.Name;
                    shoppingCart.IsSendGift = true;
                }
                PromotionInfo info4 = ShoppingProvider.Instance().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentTimesPoint);
                if (info4 != null)
                {
                    shoppingCart.SentTimesPointPromotionId = info4.ActivityId;
                    shoppingCart.SentTimesPointPromotionName = info4.Name;
                    shoppingCart.IsSendTimesPoint = true;
                    shoppingCart.TimesPoint = info4.DiscountValue;
                }
                PromotionInfo info5 = ShoppingProvider.Instance().GetSendPromotion(user, shoppingCart.GetTotal(), PromoteType.FullAmountSentFreight);
                if (info5 != null)
                {
                    shoppingCart.FreightFreePromotionId = info5.ActivityId;
                    shoppingCart.FreightFreePromotionName = info5.Name;
                    shoppingCart.IsFreightFree = true;
                }
                return shoppingCart;
            }
            return CookieShoppingProvider.Instance().GetShoppingCart();
        }

        public static ShoppingCartInfo GetShoppingCart(int Boundlingid, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(Boundlingid);
            Member user = HiContext.Current.User as Member;
            foreach (BundlingItemInfo info2 in bundlingItemsByID)
            {
                ShoppingCartItemInfo info3 = ShoppingProvider.Instance().GetCartItemInfo(user, info2.SkuId, buyAmount * info2.ProductNum);
                if (info3 != null)
                {
                    info.LineItems.Add(info2.SkuId, info3);
                }
            }
            return info;
        }

        public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            Member user = HiContext.Current.User as Member;
            ShoppingCartItemInfo info2 = ShoppingProvider.Instance().GetCartItemInfo(user, productSkuId, buyAmount);
            if (info2 == null)
            {
                return null;
            }
            info.LineItems.Add(productSkuId, info2);
            if (user != null)
            {
                decimal reducedAmount = 0M;
                PromotionInfo info3 = ShoppingProvider.Instance().GetReducedPromotion(user, info.GetAmount(), info.GetQuantity(), out reducedAmount);
                if (info3 != null)
                {
                    info.ReducedPromotionId = info3.ActivityId;
                    info.ReducedPromotionName = info3.Name;
                    info.ReducedPromotionAmount = reducedAmount;
                    info.IsReduced = true;
                }
                PromotionInfo info4 = ShoppingProvider.Instance().GetSendPromotion(user, info.GetTotal(), PromoteType.FullAmountSentGift);
                if (info4 != null)
                {
                    info.SendGiftPromotionId = info4.ActivityId;
                    info.SendGiftPromotionName = info4.Name;
                    info.IsSendGift = true;
                }
                PromotionInfo info5 = ShoppingProvider.Instance().GetSendPromotion(user, info.GetTotal(), PromoteType.FullAmountSentTimesPoint);
                if (info5 != null)
                {
                    info.SentTimesPointPromotionId = info5.ActivityId;
                    info.SentTimesPointPromotionName = info5.Name;
                    info.IsSendTimesPoint = true;
                    info.TimesPoint = info5.DiscountValue;
                }
                PromotionInfo info6 = ShoppingProvider.Instance().GetSendPromotion(user, info.GetTotal(), PromoteType.FullAmountSentFreight);
                if (info6 != null)
                {
                    info.FreightFreePromotionId = info6.ActivityId;
                    info.FreightFreePromotionName = info6.Name;
                    info.IsFreightFree = true;
                }
            }
            return info;
        }

        public static int GetSkuStock(string skuId)
        {
            return ShoppingProvider.Instance().GetSkuStock(skuId);
        }

        public static void RemoveGiftItem(int giftId, PromoteType promotype)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().RemoveGiftItem(giftId);
            }
            else
            {
                ShoppingProvider.Instance().RemoveGiftItem(giftId, promotype);
            }
        }

        public static void RemoveLineItem(string skuId)
        {
            if (HiContext.Current.User.IsAnonymous)
            {
                CookieShoppingProvider.Instance().RemoveLineItem(skuId);
            }
            else
            {
                ShoppingProvider.Instance().RemoveLineItem(HiContext.Current.User.UserId, skuId);
            }
        }

        public static void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                RemoveGiftItem(giftId, promotype);
            }
            if (user == null)
            {
                CookieShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity);
            }
            else
            {
                ShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity, promotype);
            }
        }

        public static void UpdateLineItemQuantity(string skuId, int quantity)
        {
            Member user = HiContext.Current.User as Member;
            if (quantity <= 0)
            {
                RemoveLineItem(skuId);
            }
            if (user == null)
            {
                CookieShoppingProvider.Instance().UpdateLineItemQuantity(skuId, quantity);
            }
            else
            {
                ShoppingProvider.Instance().UpdateLineItemQuantity(user, skuId, quantity);
            }
        }
    }
}

