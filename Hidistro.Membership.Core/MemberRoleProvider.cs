namespace Hidistro.Membership.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Hidistro.Core;

    public abstract class MemberRoleProvider
    {
        private static readonly MemberRoleProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.RoleData,Hidistro.Membership.Data") as MemberRoleProvider);

        protected MemberRoleProvider()
        {
        }

        public abstract void AddDeletePrivilegeInRoles(Guid roleId, string privilege);
        public abstract void DeletePrivilegeInRoles(Guid roleId);
        public abstract IList<int> GetPrivilegeByRoles(Guid roleId);
        public abstract IList<int> GetPrivilegesForUser(string userName);
        public abstract RoleInfo GetRole(Guid roleId, string roleName);
        public abstract ArrayList GetRoles();
        public abstract ArrayList GetRoles(int userId);
        public static MemberRoleProvider Instance()
        {
            return _defaultInstance;
        }

        public static RoleInfo PopulateRoleFromIDataReader(IDataReader reader)
        {
            RoleInfo info = new RoleInfo();
            info.RoleID = (Guid) reader["RoleID"];
            info.Name = (string) reader["Name"];
            info.Description = reader["Description"] as string;
            return info;
        }

        public abstract bool PrivilegeInRoles(Guid roleId, int privilege);
        public abstract void UpdateRole(RoleInfo role);
    }
}

