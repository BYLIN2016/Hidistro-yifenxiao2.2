namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public sealed class PluginAttribute : Attribute
    {
        
        private string _Name;
        
        private int _Sequence;

        public PluginAttribute(string name)
        {
            this.Name = name;
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

        public int Sequence
        {
            
            get
            {
                return _Sequence;
            }
            
            set
            {
                _Sequence = value;
            }
        }
    }
}

