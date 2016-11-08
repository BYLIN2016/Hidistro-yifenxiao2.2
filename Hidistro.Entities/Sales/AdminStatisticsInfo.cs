namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdminStatisticsInfo : StatisticsInfo
    {
        
        private int _DistributorApplyRequestWaitDispose;
        
        private int _DistributorBlancedrawRequest;
        
        private int _DistributorSiteRequest;
        
        private int _DistroNewAddYesterToday;
        
        private decimal _DistrosBalance;
        
        private int _MemberBlancedrawRequest;
        
        private decimal _MembersBalance;
        
        private decimal _OrderPriceMonth;
        
        private decimal _OrderPriceYesterDay;
        
        private int _ProductAlert;
        
        private int _PurchaseOrderNumbToday;
        
        private int _PurchaseOrderNumbWait;
        
        private decimal _PurchaseorderPriceMonth;
        
        private decimal _PurchaseorderPriceToDay;
        
        private decimal _PurchaseOrderPriceToday;
        
        private decimal _PurchaseorderPriceYesterDay;
        
        private decimal _PurchaseOrderProfitToday;
        
        private int _TodayFinishOrder;
        
        private int _TodayFinishPurchaseOrder;
        
        private int _TotalDistributors;
        
        private int _TotalMembers;
        
        private int _TotalProducts;
        
        private int _UserNewAddYesterToday;
        
        private int _YesterdayFinishOrder;
        
        private int _YesterdayFinishPurchaseOrder;

        public decimal BalanceTotal
        {
            get
            {
                return (this.MembersBalance + this.DistrosBalance);
            }
        }

        public int DistributorApplyRequestWaitDispose
        {
            
            get
            {
                return _DistributorApplyRequestWaitDispose;
            }
            
            set
            {
                _DistributorApplyRequestWaitDispose = value;
            }
        }

        public int DistributorBlancedrawRequest
        {
            
            get
            {
                return _DistributorBlancedrawRequest;
            }
            
            set
            {
                _DistributorBlancedrawRequest = value;
            }
        }

        public int DistributorSiteRequest
        {
            
            get
            {
                return _DistributorSiteRequest;
            }
            
            set
            {
                _DistributorSiteRequest = value;
            }
        }

        public int DistroNewAddYesterToday
        {
            
            get
            {
                return _DistroNewAddYesterToday;
            }
            
            set
            {
                _DistroNewAddYesterToday = value;
            }
        }

        public decimal DistrosBalance
        {
            
            get
            {
                return _DistrosBalance;
            }
            
            set
            {
                _DistrosBalance = value;
            }
        }

        public int MemberBlancedrawRequest
        {
            
            get
            {
                return _MemberBlancedrawRequest;
            }
            
            set
            {
                _MemberBlancedrawRequest = value;
            }
        }

        public decimal MembersBalance
        {
            
            get
            {
                return _MembersBalance;
            }
            
            set
            {
                _MembersBalance = value;
            }
        }

        public decimal OrderPriceMonth
        {
            
            get
            {
                return _OrderPriceMonth;
            }
            
            set
            {
                _OrderPriceMonth = value;
            }
        }

        public decimal OrderPriceYesterDay
        {
            
            get
            {
                return _OrderPriceYesterDay;
            }
            
            set
            {
                _OrderPriceYesterDay = value;
            }
        }

        public int ProductAlert
        {
            
            get
            {
                return _ProductAlert;
            }
            
            set
            {
                _ProductAlert = value;
            }
        }

        public decimal ProfitTotal
        {
            get
            {
                return (this.PurchaseOrderProfitToday + base.OrderProfitToday);
            }
        }

        public int PurchaseOrderNumbToday
        {
            
            get
            {
                return _PurchaseOrderNumbToday;
            }
            
            set
            {
                _PurchaseOrderNumbToday = value;
            }
        }

        public int PurchaseOrderNumbWait
        {
            
            get
            {
                return _PurchaseOrderNumbWait;
            }
            
            set
            {
                _PurchaseOrderNumbWait = value;
            }
        }

        public decimal PurchaseorderPriceMonth
        {
            
            get
            {
                return _PurchaseorderPriceMonth;
            }
            
            set
            {
                _PurchaseorderPriceMonth = value;
            }
        }

        public decimal PurchaseorderPriceToDay
        {
            
            get
            {
                return _PurchaseorderPriceToDay;
            }
            
            set
            {
                _PurchaseorderPriceToDay = value;
            }
        }

        public decimal PurchaseOrderPriceToday
        {
            
            get
            {
                return _PurchaseOrderPriceToday;
            }
            
            set
            {
                _PurchaseOrderPriceToday = value;
            }
        }

        public decimal PurchaseorderPriceYesterDay
        {
            
            get
            {
                return _PurchaseorderPriceYesterDay;
            }
            
            set
            {
                _PurchaseorderPriceYesterDay = value;
            }
        }

        public decimal PurchaseOrderProfitToday
        {
            
            get
            {
                return _PurchaseOrderProfitToday;
            }
            
            set
            {
                _PurchaseOrderProfitToday = value;
            }
        }

        public int TodayFinishOrder
        {
            
            get
            {
                return _TodayFinishOrder;
            }
            
            set
            {
                _TodayFinishOrder = value;
            }
        }

        public int TodayFinishPurchaseOrder
        {
            
            get
            {
                return _TodayFinishPurchaseOrder;
            }
            
            set
            {
                _TodayFinishPurchaseOrder = value;
            }
        }

        public int TotalDistributors
        {
            
            get
            {
                return _TotalDistributors;
            }
            
            set
            {
                _TotalDistributors = value;
            }
        }

        public int TotalMembers
        {
            
            get
            {
                return _TotalMembers;
            }
            
            set
            {
                _TotalMembers = value;
            }
        }

        public int TotalProducts
        {
            
            get
            {
                return _TotalProducts;
            }
            
            set
            {
                _TotalProducts = value;
            }
        }

        public int UserNewAddYesterToday
        {
            
            get
            {
                return _UserNewAddYesterToday;
            }
            
            set
            {
                _UserNewAddYesterToday = value;
            }
        }

        public int YesterdayFinishOrder
        {
            
            get
            {
                return _YesterdayFinishOrder;
            }
            
            set
            {
                _YesterdayFinishOrder = value;
            }
        }

        public int YesterdayFinishPurchaseOrder
        {
            
            get
            {
                return _YesterdayFinishPurchaseOrder;
            }
            
            set
            {
                _YesterdayFinishPurchaseOrder = value;
            }
        }
    }
}

