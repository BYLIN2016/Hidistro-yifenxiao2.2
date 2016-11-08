namespace Hidistro.Membership.Context
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserEventArgs : EventArgs
    {
        
        private string _DealPassword;
        
        private string _Password;
        
        private string _Username;

        public UserEventArgs(string username, string password, string dealPassword)
        {
            this.Username = username;
            this.Password = password;
            this.DealPassword = dealPassword;
        }

        public string DealPassword
        {
            
            get
            {
                return _DealPassword;
            }
            
            private set
            {
                _DealPassword = value;
            }
        }

        public string Password
        {
            
            get
            {
                return _Password;
            }
            
            private set
            {
                _Password = value;
            }
        }

        public string Username
        {
            
            get
            {
                return _Username;
            }
            
            private set
            {
                _Username = value;
            }
        }
    }
}

