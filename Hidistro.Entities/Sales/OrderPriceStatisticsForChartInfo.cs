namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderPriceStatisticsForChartInfo
    {
        
        private decimal _Price;
        
        private int _TimePoint;

        public decimal Price
        {
            
            get
            {
                return _Price;
            }
            
            set
            {
                _Price = value;
            }
        }

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
    }
}

