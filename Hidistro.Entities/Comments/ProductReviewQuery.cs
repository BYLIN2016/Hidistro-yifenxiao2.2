namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewQuery : Pagination
    {
        
        private int? _CategoryId;
        
        private string _Keywords;
        
        private string _ProductCode;

        public int? CategoryId
        {
            
            get
            {
                return _CategoryId;
            }
            
            set
            {
                _CategoryId = value;
            }
        }

        [HtmlCoding]
        public string Keywords
        {
            
            get
            {
                return _Keywords;
            }
            
            set
            {
                _Keywords = value;
            }
        }

        [HtmlCoding]
        public string ProductCode
        {
            
            get
            {
                return _ProductCode;
            }
            
            set
            {
                _ProductCode = value;
            }
        }
    }
}

