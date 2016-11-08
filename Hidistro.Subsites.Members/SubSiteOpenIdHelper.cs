namespace Hidistro.Subsites.Members
{
    using Hidistro.Entities.Members;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;

    public static class SubSiteOpenIdHelper
    {
        public static void DeleteSettings(string openIdType)
        {
            OpenIdProvider.Instance().DeleteSettings(openIdType);
        }

        public static PluginItemCollection GetConfigedItems()
        {
            IList<string> configedTypes = OpenIdProvider.Instance().GetConfigedTypes();
            if ((configedTypes == null) || (configedTypes.Count == 0))
            {
                return null;
            }
            PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
            if ((plugins != null) && (plugins.Count > 0))
            {
                PluginItem[] items = plugins.Items;
                foreach (PluginItem item in items)
                {
                    if (!configedTypes.Contains(item.FullName.ToLower()))
                    {
                        plugins.Remove(item.FullName.ToLower());
                    }
                }
            }
            return plugins;
        }

        public static PluginItemCollection GetEmptyItems()
        {
            PluginItemCollection plugins = OpenIdPlugins.Instance().GetPlugins();
            if ((plugins == null) || (plugins.Count == 0))
            {
                return null;
            }
            IList<string> configedTypes = OpenIdProvider.Instance().GetConfigedTypes();
            if ((configedTypes != null) && (configedTypes.Count > 0))
            {
                foreach (string str in configedTypes)
                {
                    if (plugins.ContainsKey(str.ToLower()))
                    {
                        plugins.Remove(str.ToLower());
                    }
                }
            }
            return plugins;
        }

        public static OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
        {
            return OpenIdProvider.Instance().GetOpenIdSettings(openIdType);
        }

        public static void SaveSettings(OpenIdSettingsInfo settings)
        {
            OpenIdProvider.Instance().SaveSettings(settings);
        }
    }
}

