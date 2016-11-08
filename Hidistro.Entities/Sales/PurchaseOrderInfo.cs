namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderInfo
    {
        
        private string _Address;
        
        private decimal _AdjustedDiscount;
        
        private decimal _AdjustedFreight;
        
        private string _CellPhone;
        
        private string _CloseReason;
        
        private string _DistributorEmail;
        
        private int _DistributorId;
        
        private string _DistributorMSN;
        
        private string _Distributorname;
        
        private string _DistributorQQ;
        
        private string _DistributorRealName;
        
        private string _DistributorWangwang;
        
        private string _ExpressCompanyAbb;
        
        private string _ExpressCompanyName;
        
        private DateTime _FinishDate;
        
        private decimal _Freight;
        
        private string _Gateway;
        
        private string _InvoiceTitle;
        
        private OrderMark? _ManagerMark;
        
        private string _ManagerRemark;
        
        private string _ModeName;
        
        private string _OrderId;
        
        private decimal _OrderTotal;
        
        private DateTime _PayDate;
        
        private string _PaymentType;
        
        private int _PaymentTypeId;
        
        private DateTime _PurchaseDate;
        
        private string _PurchaseOrderId;
        
        private OrderStatus _PurchaseStatus;
        
        private string _RealModeName;
        
        private int _RealShippingModeId;
        
        private decimal _RefundAmount;
        
        private string _RefundRemark;
        
        private Hidistro.Entities.Sales.RefundStatus _RefundStatus;
        
        private int _RegionId;
        
        private string _Remark;
        
        private string _ShipOrderNumber;
        
        private DateTime _ShippingDate;
        
        private int _ShippingModeId;
        
        private string _ShippingRegion;
        
        private string _ShipTo;
        
        private string _ShipToDate;
        
        private string _TaobaoOrderId;
        
        private decimal _Tax;
        
        private string _TelPhone;
        
        private decimal _Weight;
        
        private string _ZipCode;
        private IList<PurchaseOrderGiftInfo> purchaseOrderGifts;
        private IList<PurchaseOrderItemInfo> purchaseOrderItems;

        public bool CheckAction(PurchaseOrderActions action)
        {
            if ((this.PurchaseStatus != OrderStatus.Finished) && (this.PurchaseStatus != OrderStatus.Closed))
            {
                switch (action)
                {
                    case PurchaseOrderActions.DISTRIBUTOR_CLOSE:
                    case PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS:
                    case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY:
                    case PurchaseOrderActions.MASTER__CLOSE:
                    case PurchaseOrderActions.MASTER__MODIFY_AMOUNT:
                    case PurchaseOrderActions.MASTER_CONFIRM_PAY:
                        return (this.PurchaseStatus == OrderStatus.WaitBuyerPay);

                    case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS:
                    case PurchaseOrderActions.MASTER_FINISH_TRADE:
                        return (this.PurchaseStatus == OrderStatus.SellerAlreadySent);

                    case PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE:
                        return ((this.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid));

                    case PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS:
                        return ((this.PurchaseStatus == OrderStatus.WaitBuyerPay) || (this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid));

                    case PurchaseOrderActions.MASTER_SEND_GOODS:
                        return ((this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid) || ((this.PurchaseStatus == OrderStatus.WaitBuyerPay) && (this.Gateway == "hishop.plugins.payment.podrequest")));

                    case PurchaseOrderActions.MASTER_REJECT_REFUND:
                        return ((this.PurchaseStatus == OrderStatus.BuyerAlreadyPaid) || (this.PurchaseStatus == OrderStatus.SellerAlreadySent));
                }
            }
            return false;
        }

        public decimal GetGiftAmount()
        {
            decimal num = 0M;
            foreach (PurchaseOrderGiftInfo info in this.PurchaseOrderGifts)
            {
                num += info.GetSubTotal();
            }
            return num;
        }

        public decimal GetProductAmount()
        {
            decimal num = 0M;
            foreach (PurchaseOrderItemInfo info in this.PurchaseOrderItems)
            {
                num += info.GetSubTotal();
            }
            return num;
        }

        public decimal GetPurchaseCostPrice()
        {
            decimal num = 0M;
            foreach (PurchaseOrderItemInfo info in this.PurchaseOrderItems)
            {
                num += info.ItemCostPrice * info.Quantity;
            }
            foreach (PurchaseOrderGiftInfo info2 in this.PurchaseOrderGifts)
            {
                num += info2.CostPrice * info2.Quantity;
            }
            return num;
        }

        public decimal GetPurchaseProfit()
        {
            return ((this.GetPurchaseTotal() - this.RefundAmount) - this.GetPurchaseCostPrice());
        }

        public decimal GetPurchaseTotal()
        {
            return ((((this.GetProductAmount() + this.GetGiftAmount()) + this.AdjustedFreight) + this.AdjustedDiscount) + this.Tax);
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

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValPurchaseOrder", MessageTemplate="采购单折扣不能为空，金额大小负1000万-1000万之间")]
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

        public decimal AdjustedFreight
        {
            
            get
            {
                return _AdjustedFreight;
            }
            
            set
            {
                _AdjustedFreight = value;
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

        public string DistributorEmail
        {
            
            get
            {
                return _DistributorEmail;
            }
            
            set
            {
                _DistributorEmail = value;
            }
        }

        public int DistributorId
        {
            
            get
            {
                return _DistributorId;
            }
            
            set
            {
                _DistributorId = value;
            }
        }

        public string DistributorMSN
        {
            
            get
            {
                return _DistributorMSN;
            }
            
            set
            {
                _DistributorMSN = value;
            }
        }

        public string Distributorname
        {
            
            get
            {
                return _Distributorname;
            }
            
            set
            {
                _Distributorname = value;
            }
        }

        public string DistributorQQ
        {
            
            get
            {
                return _DistributorQQ;
            }
            
            set
            {
                _DistributorQQ = value;
            }
        }

        public string DistributorRealName
        {
            
            get
            {
                return _DistributorRealName;
            }
            
            set
            {
                _DistributorRealName = value;
            }
        }

        public string DistributorWangwang
        {
            
            get
            {
                return _DistributorWangwang;
            }
            
            set
            {
                _DistributorWangwang = value;
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

        public bool IsManualPurchaseOrder
        {
            get
            {
                return string.IsNullOrEmpty(this.OrderId);
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

        public decimal OrderTotal
        {
            
            get
            {
                return _OrderTotal;
            }
            
            set
            {
                _OrderTotal = value;
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

        public DateTime PurchaseDate
        {
            
            get
            {
                return _PurchaseDate;
            }
            
            set
            {
                _PurchaseDate = value;
            }
        }

        public IList<PurchaseOrderGiftInfo> PurchaseOrderGifts
        {
            get
            {
                if (this.purchaseOrderGifts == null)
                {
                    this.purchaseOrderGifts = new List<PurchaseOrderGiftInfo>();
                }
                return this.purchaseOrderGifts;
            }
        }

        public string PurchaseOrderId
        {
            
            get
            {
                return _PurchaseOrderId;
            }
            
            set
            {
                _PurchaseOrderId = value;
            }
        }

        public IList<PurchaseOrderItemInfo> PurchaseOrderItems
        {
            get
            {
                if (this.purchaseOrderItems == null)
                {
                    this.purchaseOrderItems = new List<PurchaseOrderItemInfo>();
                }
                return this.purchaseOrderItems;
            }
        }

        public OrderStatus PurchaseStatus
        {
            
            get
            {
                return _PurchaseStatus;
            }
            
            set
            {
                _PurchaseStatus = value;
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

        [HtmlCoding]
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

        public string TaobaoOrderId
        {
            
            get
            {
                return _TaobaoOrderId;
            }
            
            set
            {
                _TaobaoOrderId = value;
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

        public decimal Weight
        {
            
            get
            {
                return _Weight;
            }
            
            set
            {
                _Weight = value;
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

