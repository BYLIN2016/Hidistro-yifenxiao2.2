namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewAndReplyQuery : Pagination
    {
        
        private int _ProductId;
        
        private long _ReviewId;

        public virtual int ProductId
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

        public long ReviewId
        {
            
            get
            {
                return _ReviewId;
            }
            
            set
            {
                _ReviewId = value;
            }
        }
    }
}

