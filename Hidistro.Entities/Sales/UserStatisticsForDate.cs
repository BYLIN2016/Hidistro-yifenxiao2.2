namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserStatisticsForDate
    {
        
        private int _TimePoint;
        
        private int _UserCounts;

        public int TimePoint
        {
            
            get
            {
                return _TimePoint;
            }
            
            set
            {
                _TimePoint = value;
            }
        }

        public int UserCounts
        {
            
            get
            {
                return _UserCounts;
            }
            
            set
            {
                _UserCounts = value;
            }
        }
    }
}

