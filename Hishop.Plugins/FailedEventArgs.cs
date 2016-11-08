namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class FailedEventArgs : EventArgs
    {
        
        private string _Message;

        public FailedEventArgs(string message)
        {
            this.Message = message;
        }

        public string Message
        {
            
            get
            {
                return _Message;
            }
            
            private set
            {
                _Message = value;
            }
        }
    }
}

