namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class CouponItemInfoQuery : Pagination
    {
        
        private string _CounponName;
        
        private int? _CouponId;
        
        private int? _CouponStatus;
        
        private string _OrderId;
        
        private string _UserName;

        public string CounponName
        {
            
            get
            {
                return _CounponName;
            }
            
            set
            {
                _CounponName = value;
            }
        }

        public int? CouponId
        {
            
            get
            {
                return _CouponId;
            }
            
            set
            {
                _CouponId = value;
            }
        }

        public int? CouponStatus
        {
            
            get
            {
                return _CouponStatus;
            }
            
            set
            {
                _CouponStatus = value;
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

        public string UserName
        {
            
            get
            {
                return _UserName;
            }
            
            set
            {
                _UserName = value;
            }
        }
    }
}

