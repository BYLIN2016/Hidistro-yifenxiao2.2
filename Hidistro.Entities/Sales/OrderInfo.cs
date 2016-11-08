namespace Hidistro.Entities.Sales
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class OrderInfo
    {
        
        private string _Address;
        
        private decimal _AdjustedDiscount;
        
        private int _BundlingID;
        
        private int? _BundlingNum;
        
        private decimal _BundlingPrice;
        
        private string _CellPhone;
        
        private string _CloseReason;
        
        private int _CountDownBuyId;
        
        private decimal _CouponAmount;
        
        private string _CouponCode;
        
        private string _CouponName;
        
        private decimal _CouponValue;
        
        private string _EmailAddress;
        
        private string _ExpressCompanyAbb;
        
        private string _ExpressCompanyName;
        
        private DateTime _FinishDate;
        
        private decimal _Freight;
        
        private int _FreightFreePromotionId;
        
        private string _FreightFreePromotionName;
        
        private string _Gateway;
        
        private string _GatewayOrderId;
        
        private int _GroupBuyId;
        
        private Hidistro.Entities.Promotions.GroupBuyStatus _GroupBuyStatus;
        
        private string _InvoiceTitle;
        
        private bool _IsFreightFree;
        
        private bool _IsReduced;
        
        private bool _IsSendTimesPoint;
        
        private OrderMark? _ManagerMark;
        
        private string _ManagerRemark;
        
        private string _ModeName;
        
        private string _MSN;
        
        private decimal _NeedPrice;
        
        private DateTime _OrderDate;
        
        private string _OrderId;
        
        private Hidistro.Entities.Sales.OrderStatus _OrderStatus;
        
        private decimal _PayCharge;
        
        private DateTime _PayDate;
        
        private string _PaymentType;
        
        private int _PaymentTypeId;
        
        private int _Points;
        
        private string _QQ;
        
        private string _RealModeName;
        
        private string _RealName;
        
        private int _RealShippingModeId;
        
        private decimal _ReducedPromotionAmount;
        
        private int _ReducedPromotionId;
        
        private string _ReducedPromotionName;
        
        private decimal _RefundAmount;
        
        private string _RefundRemark;
        
        private Hidistro.Entities.Sales.RefundStatus _RefundStatus;
        
        private int _RegionId;
        
        private string _Remark;
        
        private string _Sender;
        
        private int _SentTimesPointPromotionId;
        
        private string _SentTimesPointPromotionName;
        
        private string _ShipOrderNumber;
        
        private DateTime _ShippingDate;
        
        private int _ShippingModeId;
        
        private string _ShippingRegion;
        
        private string _ShipTo;
        
        private string _ShipToDate;
        
        private decimal _Tax;
        
        private string _TelPhone;
        
        private decimal _TimesPoint;
        
        private int _UserId;
        
        private string _Username;
        
        private string _Wangwang;
        
        private string _ZipCode;
        private decimal adjustedFreigh;
        private static EventHandler<EventArgs> Closed;
        private static EventHandler<EventArgs> Created;
        private static EventHandler<EventArgs> Deliver;
        private IList<OrderGiftInfo> gifts;
        private Dictionary<string, LineItemInfo> lineItems;
        private static EventHandler<EventArgs> Payment;
        private static EventHandler<EventArgs> Refund;

        public static  event EventHandler<EventArgs> _Closed
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> closed = Closed;
                do
                {
                    handler2 = closed;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    closed = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Closed, handler3, handler2);
                }
                while (closed != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> closed = Closed;
                do
                {
                    handler2 = closed;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    closed = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Closed, handler3, handler2);
                }
                while (closed != handler2);
            }
        }

        public static  event EventHandler<EventArgs> _Created
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> created = Created;
                do
                {
                    handler2 = created;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    created = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Created, handler3, handler2);
                }
                while (created != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> created = Created;
                do
                {
                    handler2 = created;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    created = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Created, handler3, handler2);
                }
                while (created != handler2);
            }
        }

        public static  event EventHandler<EventArgs> _Deliver
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> deliver = Deliver;
                do
                {
                    handler2 = deliver;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    deliver = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Deliver, handler3, handler2);
                }
                while (deliver != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> deliver = Deliver;
                do
                {
                    handler2 = deliver;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    deliver = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Deliver, handler3, handler2);
                }
                while (deliver != handler2);
            }
        }

        public static  event EventHandler<EventArgs> _Payment
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> payment = Payment;
                do
                {
                    handler2 = payment;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    payment = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Payment, handler3, handler2);
                }
                while (payment != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> payment = Payment;
                do
                {
                    handler2 = payment;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    payment = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Payment, handler3, handler2);
                }
                while (payment != handler2);
            }
        }

        public static  event EventHandler<EventArgs> _Refund
        {
            add
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> refund = Refund;
                do
                {
                    handler2 = refund;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Combine(handler2, value);
                    refund = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Refund, handler3, handler2);
                }
                while (refund != handler2);
            }
            remove
            {
                EventHandler<EventArgs> handler2;
                EventHandler<EventArgs> refund = Refund;
                do
                {
                    handler2 = refund;
                    EventHandler<EventArgs> handler3 = (EventHandler<EventArgs>) Delegate.Remove(handler2, value);
                    refund = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref Refund, handler3, handler2);
                }
                while (refund != handler2);
            }
        }

        public OrderInfo()
        {
            this.OrderStatus = Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay;
            this.RefundStatus = Hidistro.Entities.Sales.RefundStatus.None;
        }

        public bool CheckAction(OrderActions action)
        {
            if ((this.OrderStatus != Hidistro.Entities.Sales.OrderStatus.Finished) && (this.OrderStatus != Hidistro.Entities.Sales.OrderStatus.Closed))
            {
                switch (action)
                {
                    case OrderActions.BUYER_PAY:
                    case OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.SELLER_CONFIRM_PAY:
                    case OrderActions.SELLER_MODIFY_TRADE:
                    case OrderActions.SELLER_CLOSE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_GIFTS:
                        return (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay);

                    case OrderActions.BUYER_CONFIRM_GOODS:
                    case OrderActions.SELLER_FINISH_TRADE:
                        return (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.SellerAlreadySent);

                    case OrderActions.SELLER_SEND_GOODS:
                        return ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid) || ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay) && (this.Gateway == "hishop.plugins.payment.podrequest")));

                    case OrderActions.SELLER_REJECT_REFUND:
                        return ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid) || (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.SellerAlreadySent));

                    case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
                        return ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay) || (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid));

                    case OrderActions.SUBSITE_CREATE_PURCHASEORDER:
                        return (((this.GroupBuyId > 0) && (this.GroupBuyStatus == Hidistro.Entities.Promotions.GroupBuyStatus.Success)) && (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid));
                }
            }
            return false;
        }

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.GetSubTotal();
            }
            return num;
        }

        public virtual decimal GetCostPrice()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.ItemCostPrice * info.ShipmentQuantity;
            }
            foreach (OrderGiftInfo info2 in this.Gifts)
            {
                num += info2.CostPrice * info2.Quantity;
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings || (HiContext.Current.User.UserRole == UserRole.Distributor))
            {
                num += this.Freight;
            }
            return num;
        }

        public int GetGroupBuyOerderNumber()
        {
            if (this.GroupBuyId > 0)
            {
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    return info.Quantity;
                }
            }
            return 0;
        }

        public static string GetOrderStatusName(Hidistro.Entities.Sales.OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay:
                    return "等待买家付款";

                case Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid:
                    return "已付款,等待发货";

                case Hidistro.Entities.Sales.OrderStatus.SellerAlreadySent:
                    return "已发货";

                case Hidistro.Entities.Sales.OrderStatus.Closed:
                    return "已关闭";

                case Hidistro.Entities.Sales.OrderStatus.Finished:
                    return "订单已完成";

                case Hidistro.Entities.Sales.OrderStatus.ApplyForRefund:
                    return "申请退款";

                case Hidistro.Entities.Sales.OrderStatus.ApplyForReturns:
                    return "申请退货";

                case Hidistro.Entities.Sales.OrderStatus.ApplyForReplacement:
                    return "申请换货";

                case Hidistro.Entities.Sales.OrderStatus.Refunded:
                    return "已退款";

                case Hidistro.Entities.Sales.OrderStatus.Returned:
                    return "已退货";

                case Hidistro.Entities.Sales.OrderStatus.History:
                    return "历史订单";
            }
            return "-";
        }

        public virtual decimal GetProfit()
        {
            return ((this.GetTotal() - this.RefundAmount) - this.GetCostPrice());
        }

        public decimal GetTotal()
        {
            decimal amount = this.GetAmount();
            if (this.BundlingID > 0)
            {
                amount = this.BundlingPrice;
            }
            if (this.IsReduced)
            {
                amount -= this.ReducedPromotionAmount;
            }
            amount += this.AdjustedFreight;
            amount += this.PayCharge;
            amount += this.Tax;
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                amount -= this.CouponValue;
            }
            return (amount + this.AdjustedDiscount);
        }

        public void OnClosed()
        {
            if (Closed != null)
            {
                Closed(this, new EventArgs());
            }
        }

        public static void OnClosed(OrderInfo order)
        {
            if (Closed != null)
            {
                Closed(order, new EventArgs());
            }
        }

        public void OnCreated()
        {
            if (Created != null)
            {
                Created(this, new EventArgs());
            }
        }

        public static void OnCreated(OrderInfo order)
        {
            if (Created != null)
            {
                Created(order, new EventArgs());
            }
        }

        public void OnDeliver()
        {
            if (Deliver != null)
            {
                Deliver(this, new EventArgs());
            }
        }

        public static void OnDeliver(OrderInfo order)
        {
            if (Deliver != null)
            {
                Deliver(order, new EventArgs());
            }
        }

        public void OnPayment()
        {
            if (Payment != null)
            {
                Payment(this, new EventArgs());
            }
        }

        public static void OnPayment(OrderInfo order)
        {
            if (Payment != null)
            {
                Payment(order, new EventArgs());
            }
        }

        public void OnRefund()
        {
            if (Refund != null)
            {
                Refund(this, new EventArgs());
            }
        }

        public static void OnRefund(OrderInfo order)
        {
            if (Refund != null)
            {
                Refund(order, new EventArgs());
            }
        }

        public string Address
        {
            
            get
            {
                return _Address;
            }
            
            set
            {
                _Address = value;
            }
        }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValOrder", MessageTemplate="订单折扣不能为空，金额大小负1000万-1000万之间")]
        public decimal AdjustedDiscount
        {
            
            get
            {
                return _AdjustedDiscount;
            }
            
            set
            {
                _AdjustedDiscount = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValOrder", MessageTemplate="运费不能为空，金额大小0-1000万之间")]
        public decimal AdjustedFreight
        {
            get
            {
                return this.adjustedFreigh;
            }
            set
            {
                this.adjustedFreigh = value;
            }
        }

        public int BundlingID
        {
            
            get
            {
                return _BundlingID;
            }
            
            set
            {
                _BundlingID = value;
            }
        }

        public int? BundlingNum
        {
            
            get
            {
                return _BundlingNum;
            }
            
            set
            {
                _BundlingNum = value;
            }
        }

        public decimal BundlingPrice
        {
            
            get
            {
                return _BundlingPrice;
            }
            
            set
            {
                _BundlingPrice = value;
            }
        }

        public string CellPhone
        {
            
            get
            {
                return _CellPhone;
            }
            
            set
            {
                _CellPhone = value;
            }
        }

        public string CloseReason
        {
            
            get
            {
                return _CloseReason;
            }
            
            set
            {
                _CloseReason = value;
            }
        }

        public int CountDownBuyId
        {
            
            get
            {
                return _CountDownBuyId;
            }
            
            set
            {
                _CountDownBuyId = value;
            }
        }

        public decimal CouponAmount
        {
            
            get
            {
                return _CouponAmount;
            }
            
            set
            {
                _CouponAmount = value;
            }
        }

        public string CouponCode
        {
            
            get
            {
                return _CouponCode;
            }
            
            set
            {
                _CouponCode = value;
            }
        }

        public string CouponName
        {
            
            get
            {
                return _CouponName;
            }
            
            set
            {
                _CouponName = value;
            }
        }

        public decimal CouponValue
        {
            
            get
            {
                return _CouponValue;
            }
            
            set
            {
                _CouponValue = value;
            }
        }

        public string EmailAddress
        {
            
            get
            {
                return _EmailAddress;
            }
            
            set
            {
                _EmailAddress = value;
            }
        }

        public string ExpressCompanyAbb
        {
            
            get
            {
                return _ExpressCompanyAbb;
            }
            
            set
            {
                _ExpressCompanyAbb = value;
            }
        }

        public string ExpressCompanyName
        {
            
            get
            {
                return _ExpressCompanyName;
            }
            
            set
            {
                _ExpressCompanyName = value;
            }
        }

        public DateTime FinishDate
        {
            
            get
            {
                return _FinishDate;
            }
            
            set
            {
                _FinishDate = value;
            }
        }

        public decimal Freight
        {
            
            get
            {
                return _Freight;
            }
            
            set
            {
                _Freight = value;
            }
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

        public string Gateway
        {
            
            get
            {
                return _Gateway;
            }
            
            set
            {
                _Gateway = value;
            }
        }

        public string GatewayOrderId
        {
            
            get
            {
                return _GatewayOrderId;
            }
            
            set
            {
                _GatewayOrderId = value;
            }
        }

        public IList<OrderGiftInfo> Gifts
        {
            get
            {
                if (this.gifts == null)
                {
                    this.gifts = new List<OrderGiftInfo>();
                }
                return this.gifts;
            }
        }

        public int GroupBuyId
        {
            
            get
            {
                return _GroupBuyId;
            }
            
            set
            {
                _GroupBuyId = value;
            }
        }

        public Hidistro.Entities.Promotions.GroupBuyStatus GroupBuyStatus
        {
            
            get
            {
                return _GroupBuyStatus;
            }
            
            set
            {
                _GroupBuyStatus = value;
            }
        }

        public string InvoiceTitle
        {
            
            get
            {
                return _InvoiceTitle;
            }
            
            set
            {
                _InvoiceTitle = value;
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

        public Dictionary<string, LineItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new Dictionary<string, LineItemInfo>();
                }
                return this.lineItems;
            }
        }

        public OrderMark? ManagerMark
        {
            
            get
            {
                return _ManagerMark;
            }
            
            set
            {
                _ManagerMark = value;
            }
        }

        public string ManagerRemark
        {
            
            get
            {
                return _ManagerRemark;
            }
            
            set
            {
                _ManagerRemark = value;
            }
        }

        public string ModeName
        {
            
            get
            {
                return _ModeName;
            }
            
            set
            {
                _ModeName = value;
            }
        }

        public string MSN
        {
            
            get
            {
                return _MSN;
            }
            
            set
            {
                _MSN = value;
            }
        }

        public decimal NeedPrice
        {
            
            get
            {
                return _NeedPrice;
            }
            
            set
            {
                _NeedPrice = value;
            }
        }

        public DateTime OrderDate
        {
            
            get
            {
                return _OrderDate;
            }
            
            set
            {
                _OrderDate = value;
            }
        }

        public string OrderId
        {
            
            get
            {
                return _OrderId;
            }
            
            set
            {
                _OrderId = value;
            }
        }

        public Hidistro.Entities.Sales.OrderStatus OrderStatus
        {
            
            get
            {
                return _OrderStatus;
            }
            
            set
            {
                _OrderStatus = value;
            }
        }

        public decimal PayCharge
        {
            
            get
            {
                return _PayCharge;
            }
            
            set
            {
                _PayCharge = value;
            }
        }

        public DateTime PayDate
        {
            
            get
            {
                return _PayDate;
            }
            
            set
            {
                _PayDate = value;
            }
        }

        public string PaymentType
        {
            
            get
            {
                return _PaymentType;
            }
            
            set
            {
                _PaymentType = value;
            }
        }

        public int PaymentTypeId
        {
            
            get
            {
                return _PaymentTypeId;
            }
            
            set
            {
                _PaymentTypeId = value;
            }
        }

        public int Points
        {
            
            get
            {
                return _Points;
            }
            
            set
            {
                _Points = value;
            }
        }

        public string QQ
        {
            
            get
            {
                return _QQ;
            }
            
            set
            {
                _QQ = value;
            }
        }

        public string RealModeName
        {
            
            get
            {
                return _RealModeName;
            }
            
            set
            {
                _RealModeName = value;
            }
        }

        public string RealName
        {
            
            get
            {
                return _RealName;
            }
            
            set
            {
                _RealName = value;
            }
        }

        public int RealShippingModeId
        {
            
            get
            {
                return _RealShippingModeId;
            }
            
            set
            {
                _RealShippingModeId = value;
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

        public decimal RefundAmount
        {
            
            get
            {
                return _RefundAmount;
            }
            
            set
            {
                _RefundAmount = value;
            }
        }

        public string RefundRemark
        {
            
            get
            {
                return _RefundRemark;
            }
            
            set
            {
                _RefundRemark = value;
            }
        }

        public Hidistro.Entities.Sales.RefundStatus RefundStatus
        {
            
            get
            {
                return _RefundStatus;
            }
            
            set
            {
                _RefundStatus = value;
            }
        }

        public int RegionId
        {
            
            get
            {
                return _RegionId;
            }
            
            set
            {
                _RegionId = value;
            }
        }

        public string Remark
        {
            
            get
            {
                return _Remark;
            }
            
            set
            {
                _Remark = value;
            }
        }

        public string Sender
        {
            
            get
            {
                return _Sender;
            }
            
            set
            {
                _Sender = value;
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

        public string ShipOrderNumber
        {
            
            get
            {
                return _ShipOrderNumber;
            }
            
            set
            {
                _ShipOrderNumber = value;
            }
        }

        public DateTime ShippingDate
        {
            
            get
            {
                return _ShippingDate;
            }
            
            set
            {
                _ShippingDate = value;
            }
        }

        public int ShippingModeId
        {
            
            get
            {
                return _ShippingModeId;
            }
            
            set
            {
                _ShippingModeId = value;
            }
        }

        public string ShippingRegion
        {
            
            get
            {
                return _ShippingRegion;
            }
            
            set
            {
                _ShippingRegion = value;
            }
        }

        public string ShipTo
        {
            
            get
            {
                return _ShipTo;
            }
            
            set
            {
                _ShipTo = value;
            }
        }

        public string ShipToDate
        {
            
            get
            {
                return _ShipToDate;
            }
            
            set
            {
                _ShipToDate = value;
            }
        }

        public decimal Tax
        {
            
            get
            {
                return _Tax;
            }
            
            set
            {
                _Tax = value;
            }
        }

        public string TelPhone
        {
            
            get
            {
                return _TelPhone;
            }
            
            set
            {
                _TelPhone = value;
            }
        }

        public decimal TimesPoint
        {
            
            get
            {
                return _TimesPoint;
            }
            
            set
            {
                _TimesPoint = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
            }
        }

        public string Username
        {
            
            get
            {
                return _Username;
            }
            
            set
            {
                _Username = value;
            }
        }

        public string Wangwang
        {
            
            get
            {
                return _Wangwang;
            }
            
            set
            {
                _Wangwang = value;
            }
        }

        public decimal Weight
        {
            get
            {
                decimal num = 0M;
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    num += info.ItemWeight * info.ShipmentQuantity;
                }
                return num;
            }
        }

        public string ZipCode
        {
            
            get
            {
                return _ZipCode;
            }
            
            set
            {
                _ZipCode = value;
            }
        }
    }
}

