namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleQuery : Pagination
    {
        
        private int? _CategoryId;
        
        private DateTime? _EndArticleTime;
        
        private string _Keywords;
        
        private DateTime? _StartArticleTime;

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

        public DateTime? EndArticleTime
        {
            
            get
            {
                return _EndArticleTime;
            }
            
            set
            {
                _EndArticleTime = value;
            }
        }

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

        public DateTime? StartArticleTime
        {
            
            get
            {
                return _StartArticleTime;
            }
            
            set
            {
                _StartArticleTime = value;
            }
        }
    }
}

