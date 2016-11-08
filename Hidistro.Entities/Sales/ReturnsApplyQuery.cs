namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ReturnsApplyQuery : Pagination
    {
        
        private int? _HandleStatus;
        
        private string _OrderId;

        public int? HandleStatus
        {
            
            get
            {
                return _HandleStatus;
            }
            
            set
            {
                _HandleStatus = value;
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
    }
}

