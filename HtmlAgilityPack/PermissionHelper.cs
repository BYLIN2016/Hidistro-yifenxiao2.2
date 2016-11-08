using System;
using System.Net;
using System.Security;
using System.Security.Permissions;
namespace HtmlAgilityPack
{
	public class PermissionHelper : IPermissionHelper
	{
		public bool GetIsRegistryAvailable()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			RegistryPermission perm = new RegistryPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(perm);
			return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
		}
		public bool GetIsDnsAvailable()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			DnsPermission perm = new DnsPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(perm);
			return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
		}
	}
}
