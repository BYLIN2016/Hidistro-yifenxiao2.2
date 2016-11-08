namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationAndReplyQuery : Pagination
    {
        
        private int? _CategoryId;
        
        private int _ConsultationId;
        
        private string _Keywords;
        
        private string _ProductCode;
        
        private int _ProductId;
        
        private ConsultationReplyType _Type;
        
        private int _UserId;

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

        public int ConsultationId
        {
            
            get
            {
                return _ConsultationId;
            }
            
            set
            {
                _ConsultationId = value;
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

        public ConsultationReplyType Type
        {
            
            get
            {
                return _Type;
            }
            
            set
            {
                _Type = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return _UserId;
            }
            
            set
            {
                _UserId = value;
            }
        }
    }
}

