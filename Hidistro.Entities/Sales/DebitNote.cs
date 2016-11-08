namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class DebitNote
    {
        
        private string _NoteId;
        
        private string _Operator;
        
        private string _OrderId;
        
        private string _Remark;

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
    }
}

