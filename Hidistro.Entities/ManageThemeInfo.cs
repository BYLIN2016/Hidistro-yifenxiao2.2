namespace Hidistro.Entities
{
    using System;
    using System.Runtime.CompilerServices;

    public class ManageThemeInfo
    {
        
        private string _Name;
        
        private string _ThemeImgUrl;
        
        private string _ThemeName;

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

        public string ThemeImgUrl
        {
            
            get
            {
                return _ThemeImgUrl;
            }
            
            set
            {
                _ThemeImgUrl = value;
            }
        }

        public string ThemeName
        {
            
            get
            {
                return _ThemeName;
            }
            
            set
            {
                _ThemeName = value;
            }
        }
    }
}

