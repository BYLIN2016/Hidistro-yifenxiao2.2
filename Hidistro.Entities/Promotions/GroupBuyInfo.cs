namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GroupBuyInfo
    {
        
        private string _Content;
        
        private DateTime _EndDate;
        
        private int _GroupBuyId;
        
        private int _MaxCount;
        
        private decimal _NeedPrice;
        
        private int _ProductId;
        
        private DateTime _StartDate;
        
        private GroupBuyStatus _Status;
        private IList<GropBuyConditionInfo> groupBuyConditions;

        [HtmlCoding]
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

        public IList<GropBuyConditionInfo> GroupBuyConditions
        {
            get
            {
                if (this.groupBuyConditions == null)
                {
                    this.groupBuyConditions = new List<GropBuyConditionInfo>();
                }
                return this.groupBuyConditions;
            }
        }

        public int GroupBuyId
        {
            
            get
            {
                return _GroupBuyId;
            }
            
            set
            {
                _GroupBuyId = value;
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

        public decimal NeedPrice
        {
            
            get
            {
                return _NeedPrice;
            }
            
            set
            {
                _NeedPrice = value;
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

        public GroupBuyStatus Status
        {
            
            get
            {
                return _Status;
            }
            
            set
            {
                _Status = value;
            }
        }
    }
}

