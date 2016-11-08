namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public sealed class ConfigElementAttribute : Attribute
    {
        
        private string _Description;
        
        private Hishop.Plugins.InputType _InputType;
        
        private string _Name;
        
        private bool _Nullable;
        
        private string[] _Options;

        public ConfigElementAttribute(string name)
        {
            this.InputType = Hishop.Plugins.InputType.TextBox;
            this.Name = name;
            this.Nullable = true;
        }

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

        public Hishop.Plugins.InputType InputType
        {
            
            get
            {
                return _InputType;
            }
            
            set
            {
                _InputType = value;
            }
        }

        public string Name
        {
            
            get
            {
                return _Name;
            }
            
            private set
            {
                _Name = value;
            }
        }

        public bool Nullable
        {
            
            get
            {
                return _Nullable;
            }
            
            set
            {
                _Nullable = value;
            }
        }

        public string[] Options
        {
            
            get
            {
                return _Options;
            }
            
            set
            {
                _Options = value;
            }
        }
    }
}

