namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class AccountSummaryInfo
    {
        
        private decimal _AccountAmount;
        
        private decimal _DrawRequestBalance;
        
        private decimal _FreezeBalance;
        
        private decimal _UseableBalance;

        public decimal AccountAmount
        {
            
            get
            {
                return _AccountAmount;
            }
            
            set
            {
                _AccountAmount = value;
            }
        }

        public decimal DrawRequestBalance
        {
            
            get
            {
                return _DrawRequestBalance;
            }
            
            set
            {
                _DrawRequestBalance = value;
            }
        }

        public decimal FreezeBalance
        {
            
            get
            {
                return _FreezeBalance;
            }
            
            set
            {
                _FreezeBalance = value;
            }
        }

        public decimal UseableBalance
        {
            
            get
            {
                return _UseableBalance;
            }
            
            set
            {
                _UseableBalance = value;
            }
        }
    }
}

