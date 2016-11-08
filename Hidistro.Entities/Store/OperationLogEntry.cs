namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogEntry
    {
        
        private DateTime _AddedTime;
        
        private string _Description;
        
        private string _IpAddress;
        
        private long _LogId;
        
        private string _PageUrl;
        
        private Hidistro.Entities.Store.Privilege _Privilege;
        
        private string _UserName;

        public DateTime AddedTime
        {
            
            get
            {
                return _AddedTime;
            }
            
            set
            {
                _AddedTime = value;
            }
        }

        public string Description
        {
            
            get
            {
                return _Description;
            }
            
            set
            {
                _Description = value;
            }
        }

        public string IpAddress
        {
            
            get
            {
                return _IpAddress;
            }
            
            set
            {
                _IpAddress = value;
            }
        }

        public long LogId
        {
            
            get
            {
                return _LogId;
            }
            
            set
            {
                _LogId = value;
            }
        }

        public string PageUrl
        {
            
            get
            {
                return _PageUrl;
            }
            
            set
            {
                _PageUrl = value;
            }
        }

        public Hidistro.Entities.Store.Privilege Privilege
        {
            
            get
            {
                return _Privilege;
            }
            
            set
            {
                _Privilege = value;
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

