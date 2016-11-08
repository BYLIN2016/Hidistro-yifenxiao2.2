namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseDebitNote
    {
        
        private string _NoteId;
        
        private string _Operator;
        
        private decimal? _PayGateMoney;
        
        private string _PayMethod;
        
        private decimal _PayMoney;
        
        private DateTime _PayTime;
        
        private string _PurchaseOrderId;
        
        private string _Remark;
        
        private string _Username;

        public string NoteId
        {
            
            get
            {
                return _NoteId;
            }
            
            set
            {
                _NoteId = value;
            }
        }

        public string Operator
        {
            
            get
            {
                return _Operator;
            }
            
            set
            {
                _Operator = value;
            }
        }

        public decimal? PayGateMoney
        {
            
            get
            {
                return _PayGateMoney;
            }
            
            set
            {
                _PayGateMoney = value;
            }
        }

        public string PayMethod
        {
            
            get
            {
                return _PayMethod;
            }
            
            set
            {
                _PayMethod = value;
            }
        }

        public decimal PayMoney
        {
            
            get
            {
                return _PayMoney;
            }
            
            set
            {
                _PayMoney = value;
            }
        }

        public DateTime PayTime
        {
            
            get
            {
                return _PayTime;
            }
            
            set
            {
                _PayTime = value;
            }
        }

        public string PurchaseOrderId
        {
            
            get
            {
                return _PurchaseOrderId;
            }
            
            set
            {
                _PurchaseOrderId = value;
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

