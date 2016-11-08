namespace Hidistro.Entities.Comments
{
    using System;
    using System.Runtime.CompilerServices;

    public class DesignTempleteInfo
    {
        
        private string _TempleteContent;
        
        private string _TempleteID;

        public string TempleteContent
        {
            
            get
            {
                return _TempleteContent;
            }
            
            set
            {
                _TempleteContent = value;
            }
        }

        public string TempleteID
        {
            
            get
            {
                return _TempleteID;
            }
            
            set
            {
                _TempleteID = value;
            }
        }
    }
}

