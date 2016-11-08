namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    public class FinishedEventArgs : EventArgs
    {
        
        private bool _IsMedTrade;

        public FinishedEventArgs(bool isMedTrade)
        {
            this.IsMedTrade = isMedTrade;
        }

        public bool IsMedTrade
        {
            
            get
            {
                return _IsMedTrade;
            }
            
            private set
            {
                _IsMedTrade = value;
            }
        }
    }
}

