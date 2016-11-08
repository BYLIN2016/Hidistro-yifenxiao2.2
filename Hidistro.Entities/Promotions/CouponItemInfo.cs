namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CouponItemInfo
    {
        
        private string _ClaimCode;
        
        private int _CouponId;
        
        private int? _CouponStatus;
        
        private string _EmailAddress;
        
        private DateTime _GenerateTime;
        
        private string _OrderId;
        
        private DateTime? _UsedTime;
        
        private int? _UserId;
        
        private string _UserName;

        public CouponItemInfo()
        {
        }

        public CouponItemInfo(int couponId, string claimCode, int? userId, string username, string emailAddress, DateTime generateTime)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.UserName = username;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
        }

        public string ClaimCode
        {
            
            get
            {
                return _ClaimCode;
            }
            
            set
            {
                _ClaimCode = value;
            }
        }

        public int CouponId
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

        public DateTime GenerateTime
        {
            
            get
            {
                return _GenerateTime;
            }
            
            set
            {
                _GenerateTime = value;
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

        public DateTime? UsedTime
        {
            
            get
            {
                return _UsedTime;
            }
            
            set
            {
                _UsedTime = value;
            }
        }

        public int? UserId
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

