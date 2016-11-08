namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class PluginItem
    {
        
        private string _Description;
        
        private string _DisplayName;
        
        private string _FullName;
        
        private string _Logo;
        
        private string _ShortDescription;

        public virtual string ToJsonString()
        {
            return ("{\"FullName\":\"" + this.FullName + "\",\"DisplayName\":\"" + this.DisplayName + "\",\"Logo\":\"" + this.Logo + "\",\"ShortDescription\":\"" + this.ShortDescription + "\",\"Description\":\"" + this.Description + "\"}");
        }

        public virtual string ToXmlString()
        {
            return ("<xml><FullName>" + this.FullName + "</FullName><DisplayName>" + this.DisplayName + "</DisplayName><Logo>" + this.Logo + "</Logo><ShortDescription>" + this.ShortDescription + "</ShortDescription><Description>" + this.Description + "</Description></xml>");
        }

        public virtual string Description
        {
            
            get
            {
                return _Description;
            }
            
            set
            {
                _Description = value;
            }
        }

        public virtual string DisplayName
        {
            
            get
            {
                return _DisplayName;
            }
            
            set
            {
                _DisplayName = value;
            }
        }

        public virtual string FullName
        {
            
            get
            {
                return _FullName;
            }
            
            set
            {
                _FullName = value;
            }
        }

        public virtual string Logo
        {
            
            get
            {
                return _Logo;
            }
            
            set
            {
                _Logo = value;
            }
        }

        public virtual string ShortDescription
        {
            
            get
            {
                return _ShortDescription;
            }
            
            set
            {
                _ShortDescription = value;
            }
        }
    }
}

