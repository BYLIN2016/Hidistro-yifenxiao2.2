namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class MemberClientSet
    {
        
        private string _ClientChar;
        
        private int _ClientTypeId;
        
        private decimal _ClientValue;
        
        private DateTime? _EndTime;
        
        private int _LastDay;
        
        private DateTime? _StartTime;

        public string ClientChar
        {
            
            get
            {
                return _ClientChar;
            }
            
            set
            {
                _ClientChar = value;
            }
        }

        public int ClientTypeId
        {
            
            get
            {
                return _ClientTypeId;
            }
            
            set
            {
                _ClientTypeId = value;
            }
        }

        public decimal ClientValue
        {
            
            get
            {
                return _ClientValue;
            }
            
            set
            {
                _ClientValue = value;
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

        public int LastDay
        {
            
            get
            {
                return _LastDay;
            }
            
            set
            {
                _LastDay = value;
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
    }
}

