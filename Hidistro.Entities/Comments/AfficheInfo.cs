namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class AfficheInfo
    {
        
        private DateTime _AddedDate;
        
        private int _AfficheId;
        
        private string _Content;
        
        private string _Title;

        public DateTime AddedDate
        {
            
            get
            {
                return _AddedDate;
            }
            
            set
            {
                _AddedDate = value;
            }
        }

        public int AfficheId
        {
            
            get
            {
                return _AfficheId;
            }
            
            set
            {
                _AfficheId = value;
            }
        }

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValAfficheInfo", MessageTemplate="公告内容不能为空")]
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

        [StringLengthValidator(1, 60, Ruleset="ValAfficheInfo", MessageTemplate="公告标题不能为空，长度限制在60个字符以内"), HtmlCoding]
        public string Title
        {
            
            get
            {
                return _Title;
            }
            
            set
            {
                _Title = value;
            }
        }
    }
}

