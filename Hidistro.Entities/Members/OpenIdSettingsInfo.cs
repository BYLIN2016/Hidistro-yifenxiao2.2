namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class OpenIdSettingsInfo
    {
        
        private string _Description;
        
        private string _Name;
        
        private string _OpenIdType;
        
        private string _Settings;

        public string Description
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

        public string OpenIdType
        {
            
            get
            {
                return _OpenIdType;
            }
            
            set
            {
                _OpenIdType = value;
            }
        }

        public string Settings
        {
            
            get
            {
                return _Settings;
            }
            
            set
            {
                _Settings = value;
            }
        }
    }
}

