using System;
namespace HtmlAgilityPack
{
	public interface IPermissionHelper
	{
		bool GetIsRegistryAvailable();
		bool GetIsDnsAvailable();
	}
}
