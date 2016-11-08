namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GiftQuery
    {
        
        private bool _IsPromotion;
        
        private string _Name;
        
        private Pagination _Page;

        public GiftQuery()
        {
            this.Page = new Pagination();
        }

        public bool IsPromotion
        {
            
            get
            {
                return _IsPromotion;
            }
            
            set
            {
                _IsPromotion = value;
            }
        }

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            set
            {
                _Name = value;
            }
        }

        public Pagination Page
        {
            
            get
            {
                return _Page;
            }
            
            set
            {
                _Page = value;
            }
        }
    }
}

