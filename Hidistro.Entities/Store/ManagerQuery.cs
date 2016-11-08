namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ManagerQuery : Pagination
    {
        
        private Guid _RoleId;
        
        private string _Username;

        public Guid RoleId
        {
            
            get
            {
                return _RoleId;
            }
            
            set
            {
                _RoleId = value;
            }
        }

        public string Username
        {
            
            get
            {
                return _Username;
            }
            
            set
            {
                _Username = value;
            }
        }
    }
}

