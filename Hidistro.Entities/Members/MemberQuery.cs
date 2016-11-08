namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberQuery : Pagination
    {
        
        private string _CharSymbol;
        
        private string _ClientType;
        
        private DateTime? _EndTime;
        
        private int? _GradeId;
        
        private bool? _IsApproved;
        
        private decimal? _OrderMoney;
        
        private int? _OrderNumber;
        
        private string _Realname;
        
        private DateTime? _StartTime;
        
        private string _Username;

        public string CharSymbol
        {
            
            get
            {
                return _CharSymbol;
            }
            
            set
            {
                _CharSymbol = value;
            }
        }

        public string ClientType
        {
            
            get
            {
                return _ClientType;
            }
            
            set
            {
                _ClientType = value;
            }
        }

        public DateTime? EndTime
        {
            
            get
            {
                return _EndTime;
            }
            
            set
            {
                _EndTime = value;
            }
        }

        public int? GradeId
        {
            
            get
            {
                return _GradeId;
            }
            
            set
            {
                _GradeId = value;
            }
        }

        public bool? IsApproved
        {
            
            get
            {
                return _IsApproved;
            }
            
            set
            {
                _IsApproved = value;
            }
        }

        public decimal? OrderMoney
        {
            
            get
            {
                return _OrderMoney;
            }
            
            set
            {
                _OrderMoney = value;
            }
        }

        public int? OrderNumber
        {
            
            get
            {
                return _OrderNumber;
            }
            
            set
            {
                _OrderNumber = value;
            }
        }

        public string Realname
        {
            
            get
            {
                return _Realname;
            }
            
            set
            {
                _Realname = value;
            }
        }

        public DateTime? StartTime
        {
            
            get
            {
                return _StartTime;
            }
            
            set
            {
                _StartTime = value;
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
    }
}

