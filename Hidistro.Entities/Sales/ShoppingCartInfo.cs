namespace Hidistro.Entities.Sales
{
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShoppingCartInfo
    {
        
        private int _FreightFreePromotionId;
        
        private string _FreightFreePromotionName;
        
        private bool _IsFreightFree;
        
        private bool _IsReduced;
        
        private bool _IsSendTimesPoint;
        
        private decimal _ReducedPromotionAmount;
        
        private int _ReducedPromotionId;
        
        private string _ReducedPromotionName;
        
        private int _SendGiftPromotionId;
        
        private string _SendGiftPromotionName;
        
        private int _SentTimesPointPromotionId;
        
        private string _SentTimesPointPromotionName;
        private bool isSendGift;
        private IList<ShoppingCartGiftInfo> lineGifts;
        private Dictionary<string, ShoppingCartItemInfo> lineItems;
        private decimal timesPoint = 1M;

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (ShoppingCartItemInfo info in this.lineItems.Values)
            {
                num += info.SubTotal;
            }
            return num;
        }

        public int GetPoint()
        {
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (((this.GetTotal() * this.TimesPoint) / masterSettings.PointsRate) > 2147483647M)
            {
                return 0x7fffffff;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int) Math.Round((decimal) ((this.GetTotal() * this.TimesPoint) / masterSettings.PointsRate), 0);
            }
            return num;
        }

        public int GetPoint(decimal money)
        {
            int num = 0;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (((money * this.TimesPoint) / masterSettings.PointsRate) > 2147483647M)
            {
                return 0x7fffffff;
            }
            if (masterSettings.PointsRate != 0M)
            {
                num = (int) Math.Round((decimal) ((money * this.TimesPoint) / masterSettings.PointsRate), 0);
            }
            return num;
        }

        public int GetQuantity()
        {
            int num = 0;
            foreach (ShoppingCartItemInfo info in this.lineItems.Values)
            {
                num += info.Quantity;
            }
            return num;
        }

        public decimal GetTotal()
        {
            return (this.GetAmount() - this.ReducedPromotionAmount);
        }

        public int GetTotalNeedPoint()
        {
            int num = 0;
            foreach (ShoppingCartGiftInfo info in this.LineGifts)
            {
                num += info.SubPointTotal;
            }
            return num;
        }

        public int FreightFreePromotionId
        {
            
            get
            {
                return _FreightFreePromotionId;
            }
            
            set
            {
                _FreightFreePromotionId = value;
            }
        }

        public string FreightFreePromotionName
        {
            
            get
            {
                return _FreightFreePromotionName;
            }
            
            set
            {
                _FreightFreePromotionName = value;
            }
        }

        public bool IsFreightFree
        {
            
            get
            {
                return _IsFreightFree;
            }
            
            set
            {
                _IsFreightFree = value;
            }
        }

        public bool IsReduced
        {
            
            get
            {
                return _IsReduced;
            }
            
            set
            {
                _IsReduced = value;
            }
        }

        public bool IsSendGift
        {
            get
            {
                foreach (ShoppingCartItemInfo info in this.lineItems.Values)
                {
                    if (info.IsSendGift)
                    {
                        return true;
                    }
                }
                return this.isSendGift;
            }
            set
            {
                this.isSendGift = value;
            }
        }

        public bool IsSendTimesPoint
        {
            
            get
            {
                return _IsSendTimesPoint;
            }
            
            set
            {
                _IsSendTimesPoint = value;
            }
        }

        public IList<ShoppingCartGiftInfo> LineGifts
        {
            get
            {
                if (this.lineGifts == null)
                {
                    this.lineGifts = new List<ShoppingCartGiftInfo>();
                }
                return this.lineGifts;
            }
        }

        public Dictionary<string, ShoppingCartItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new Dictionary<string, ShoppingCartItemInfo>();
                }
                return this.lineItems;
            }
        }

        public decimal ReducedPromotionAmount
        {
            
            get
            {
                return _ReducedPromotionAmount;
            }
            
            set
            {
                _ReducedPromotionAmount = value;
            }
        }

        public int ReducedPromotionId
        {
            
            get
            {
                return _ReducedPromotionId;
            }
            
            set
            {
                _ReducedPromotionId = value;
            }
        }

        public string ReducedPromotionName
        {
            
            get
            {
                return _ReducedPromotionName;
            }
            
            set
            {
                _ReducedPromotionName = value;
            }
        }

        public int SendGiftPromotionId
        {
            
            get
            {
                return _SendGiftPromotionId;
            }
            
            set
            {
                _SendGiftPromotionId = value;
            }
        }

        public string SendGiftPromotionName
        {
            
            get
            {
                return _SendGiftPromotionName;
            }
            
            set
            {
                _SendGiftPromotionName = value;
            }
        }

        public int SentTimesPointPromotionId
        {
            
            get
            {
                return _SentTimesPointPromotionId;
            }
            
            set
            {
                _SentTimesPointPromotionId = value;
            }
        }

        public string SentTimesPointPromotionName
        {
            
            get
            {
                return _SentTimesPointPromotionName;
            }
            
            set
            {
                _SentTimesPointPromotionName = value;
            }
        }

        public decimal TimesPoint
        {
            get
            {
                return this.timesPoint;
            }
            set
            {
                this.timesPoint = value;
            }
        }

        public decimal Weight
        {
            get
            {
                decimal num = 0M;
                foreach (ShoppingCartItemInfo info in this.lineItems.Values)
                {
                    num += info.GetSubWeight();
                }
                return num;
            }
        }
    }
}

