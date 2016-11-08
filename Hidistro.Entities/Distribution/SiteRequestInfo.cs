namespace Hidistro.Entities.Distribution
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [HasSelfValidation]
    public class SiteRequestInfo
    {
        
        private string _FirstSiteUrl;
        
        private string _RefuseReason;
        
        private int _RequestId;
        
        private SiteRequestStatus _RequestStatus;
        
        private DateTime _RequestTime;
        
        private int _UserId;

        [StringLengthValidator(1, 30, Ruleset="ValSiteRequest", MessageTemplate="域名不能为空,长度限制在30个字符以内,必须为有效格式")]
        public string FirstSiteUrl
        {
            
            get
            {
                return _FirstSiteUrl;
            }
            
            set
            {
                _FirstSiteUrl = value;
            }
        }

        public string RefuseReason
        {
            
            get
            {
                return _RefuseReason;
            }
            
            set
            {
                _RefuseReason = value;
            }
        }

        public int RequestId
        {
            
            get
            {
                return _RequestId;
            }
            
            set
            {
                _RequestId = value;
            }
        }

        public SiteRequestStatus RequestStatus
        {
            
            get
            {
                return _RequestStatus;
            }
            
            set
            {
                _RequestStatus = value;
            }
        }

        public DateTime RequestTime
        {
            
            get
            {
                return _RequestTime;
            }
            
            set
            {
                _RequestTime = value;
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
    }
}

