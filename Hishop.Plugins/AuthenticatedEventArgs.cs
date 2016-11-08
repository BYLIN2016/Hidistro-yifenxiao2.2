namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class AuthenticatedEventArgs : EventArgs
    {
        
        private string _OpenId;

        public AuthenticatedEventArgs(string openId)
        {
            this.OpenId = openId;
        }

        public string OpenId
        {
            
            get
            {
                return _OpenId;
            }
            
            private set
            {
                _OpenId = value;
            }
        }
    }
}

