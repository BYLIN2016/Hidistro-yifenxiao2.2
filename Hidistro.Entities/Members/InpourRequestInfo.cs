namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class InpourRequestInfo
    {
        
        private decimal _InpourBlance;
        
        private string _InpourId;
        
        private int _PaymentId;
        
        private DateTime _TradeDate;
        
        private int _UserId;

        public decimal InpourBlance
        {
            
            get
            {
                return _InpourBlance;
            }
            
            set
            {
                _InpourBlance = value;
            }
        }

        public string InpourId
        {
            
            get
            {
                return _InpourId;
            }
            
            set
            {
                _InpourId = value;
            }
        }

        public int PaymentId
        {
            
            get
            {
                return _PaymentId;
            }
            
            set
            {
                _PaymentId = value;
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

