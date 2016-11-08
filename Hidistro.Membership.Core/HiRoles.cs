namespace Hidistro.Membership.Core
{
    using System;
    using System.Security.Principal;
    using System.Web.Security;

    public static class HiRoles
    {
        public static string[] GetRolesFromPrinciple(IPrincipal user)
        {
            RolePrincipal principal = user as RolePrincipal;
            if (principal != null)
            {
                return principal.GetRoles();
            }
            return null;
        }
    }
}

