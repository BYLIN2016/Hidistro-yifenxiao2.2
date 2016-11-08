namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CountDownInfo
    {
        
        private string _Content;
        
        private int _CountDownId;
        
        private decimal _CountDownPrice;
        
        private int _DisplaySequence;
        
        private DateTime _EndDate;
        
        private int _MaxCount;
        
        private int _ProductId;
        
        private DateTime _StartDate;

        public string Content
        {
            
            get
            {
                return _Content;
            }
            
            set
            {
                _Content = value;
            }
        }

        public int CountDownId
        {
            
            get
            {
                return _CountDownId;
            }
            
            set
            {
                _CountDownId = value;
            }
        }

        public decimal CountDownPrice
        {
            
            get
            {
                return _CountDownPrice;
            }
            
            set
            {
                _CountDownPrice = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return _DisplaySequence;
            }
            
            set
            {
                _DisplaySequence = value;
            }
        }

        public DateTime EndDate
        {
            
            get
            {
                return _EndDate;
            }
            
            set
            {
                _EndDate = value;
            }
        }

        public int MaxCount
        {
            
            get
            {
                return _MaxCount;
            }
            
            set
            {
                _MaxCount = value;
            }
        }

        public int ProductId
        {
            
            get
            {
                return _ProductId;
            }
            
            set
            {
                _ProductId = value;
            }
        }

        public DateTime StartDate
        {
            
            get
            {
                return _StartDate;
            }
            
            set
            {
                _StartDate = value;
            }
        }
    }
}

