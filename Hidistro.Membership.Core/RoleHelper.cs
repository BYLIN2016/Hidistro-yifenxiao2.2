namespace Hidistro.Membership.Core
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public class RoleHelper
    {
        private static readonly string defaultRoles = null;
        private const string PrivilegeCachekey = "DataCache-ManagerPrivileges:{0}";
        private static readonly RolesConfiguration rolesConfig = null;

        static RoleHelper()
        {
            rolesConfig = HiConfiguration.GetConfig().RolesConfiguration;
            defaultRoles = rolesConfig.RoleList();
        }

        public static void AddPrivilegeInRoles(Guid roleId, string privilege)
        {
            if (string.IsNullOrEmpty(privilege))
            {
                MemberRoleProvider.Instance().DeletePrivilegeInRoles(roleId);
            }
            else
            {
                MemberRoleProvider.Instance().AddDeletePrivilegeInRoles(roleId, privilege);
            }
        }

        public static void AddRole(string roleName)
        {
            if (!Roles.RoleExists(roleName))
            {
                Roles.CreateRole(roleName);
            }
        }

        public static void AddUserToRole(string userName, string roleName)
        {
            Roles.AddUserToRole(userName, roleName);
        }

        public static void DeleteRole(RoleInfo role)
        {
            if ((role != null) && !IsBuiltInRole(role.Name))
            {
                Roles.DeleteRole(role.Name);
            }
        }

        public static IList<int> GetPrivilegeByRoles(Guid roleId)
        {
            return MemberRoleProvider.Instance().GetPrivilegeByRoles(roleId);
        }

        public static RoleInfo GetRole(Guid roleID)
        {
            return MemberRoleProvider.Instance().GetRole(roleID, null);
        }

        public static RoleInfo GetRole(string roleName)
        {
            return MemberRoleProvider.Instance().GetRole(Guid.Empty, roleName);
        }

        public static ArrayList GetRoles()
        {
            return MemberRoleProvider.Instance().GetRoles();
        }

        public static ArrayList GetRoles(int userID)
        {
            return MemberRoleProvider.Instance().GetRoles(userID);
        }

        public static ArrayList GetRolesWithOutUser(string username)
        {
            ArrayList userRoles = GetUserRoles(username);
            ArrayList roles = GetRoles();
            ArrayList list3 = new ArrayList();
            foreach (RoleInfo info in roles)
            {
                if (!userRoles.Contains(info))
                {
                    list3.Add(info);
                }
            }
            return list3;
        }

        public static IList<int> GetUserPrivileges(string username)
        {
            string key = string.Format("DataCache-ManagerPrivileges:{0}", username);
            IList<int> privilegesForUser = HiCache.Get(key) as List<int>;
            if (privilegesForUser == null)
            {
                try
                {
                    privilegesForUser = MemberRoleProvider.Instance().GetPrivilegesForUser(username);
                    HiCache.Insert(key, privilegesForUser, 360);
                }
                catch
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().Home);
                }
            }
            return privilegesForUser;
        }

        public static string[] GetUserRoleNames(string username)
        {
            string[] rolesForUser = null;
            try
            {
                rolesForUser = Roles.GetRolesForUser(username);
            }
            catch
            {
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().Home);
            }
            return rolesForUser;
        }

        public static ArrayList GetUserRoles(string username)
        {
            ArrayList list = new ArrayList();
            string[] userRoleNames = GetUserRoleNames(username);
            foreach (string str in userRoleNames)
            {
                try
                {
                    list.Add(GetRole(str));
                }
                catch
                {
                }
            }
            return list;
        }

        public static bool IsBuiltInRole(string roleName)
        {
            return Regex.IsMatch(roleName, defaultRoles, RegexOptions.IgnoreCase);
        }

        public static bool PrivilegeInRoles(Guid roleId, int privilege)
        {
            return MemberRoleProvider.Instance().PrivilegeInRoles(roleId, privilege);
        }

        public static void RemoveUserFromRole(string userName, string roleName)
        {
            Roles.RemoveUserFromRole(userName, roleName);
        }

        public static bool RoleExists(string roleName)
        {
            return Roles.RoleExists(roleName);
        }

        public static void SignOut(string username)
        {
            HiCache.Remove(string.Format("DataCache-ManagerPrivileges:{0}", username));
        }

        public static void UpdateRole(RoleInfo role)
        {
            if (((role != null) && !IsBuiltInRole(role.Name)) && ((role.Name != null) && (role.Name.Length != 0)))
            {
                MemberRoleProvider.Instance().UpdateRole(role);
            }
        }

        public static string Manager
        {
            get
            {
                return rolesConfig.Manager;
            }
        }

        public static string SystemAdministrator
        {
            get
            {
                return rolesConfig.SystemAdministrator;
            }
        }
    }
}

