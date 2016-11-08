namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserPointInfo
    {
        
        private int? _Increased;
        
        private long _JournalNumber;
        
        private string _OrderId;
        
        private int _Points;
        
        private int? _Reduced;
        
        private string _Remark;
        
        private DateTime _TradeDate;
        
        private UserPointTradeType _TradeType;
        
        private int _UserId;

        public int? Increased
        {
            
            get
            {
                return _Increased;
            }
            
            set
            {
                _Increased = value;
            }
        }

        public long JournalNumber
        {
            
            get
            {
                return _JournalNumber;
            }
            
            set
            {
                _JournalNumber = value;
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

        public int? Reduced
        {
            
            get
            {
                return _Reduced;
            }
            
            set
            {
                _Reduced = value;
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

        public DateTime TradeDate
        {
            
            get
            {
                return _TradeDate;
            }
            
            set
            {
                _TradeDate = value;
            }
        }

        public UserPointTradeType TradeType
        {
            
            get
            {
                return _TradeType;
            }
            
            set
            {
                _TradeType = value;
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

