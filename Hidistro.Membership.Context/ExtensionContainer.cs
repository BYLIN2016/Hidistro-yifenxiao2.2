namespace Hidistro.Membership.Context
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Xml;

    internal class ExtensionContainer
    {
        private static readonly Hashtable Extensions = new Hashtable();
        private static volatile ExtensionContainer instance = null;
        private static readonly object Sync = new object();

        private ExtensionContainer()
        {
            Extensions.Clear();
            XmlNode configSection = HiContext.Current.Config.GetConfigSection("Hishop/Extensions");
            if (configSection != null)
            {
                foreach (XmlNode node2 in configSection.ChildNodes)
                {
                    if ((node2.NodeType != XmlNodeType.Comment) && node2.Name.Equals("add"))
                    {
                        string key = node2.Attributes["name"].Value;
                        string typeName = node2.Attributes["type"].Value;
                        XmlAttribute attribute = node2.Attributes["enabled"];
                        if ((attribute != null) && (attribute.Value == "false"))
                        {
                            continue;
                        }
                        Type type = Type.GetType(typeName);
                        if (type == null)
                        {
                            throw new Exception(typeName + " does not exist");
                        }
                        IExtension extension = Activator.CreateInstance(type) as IExtension;
                        if (extension == null)
                        {
                            throw new Exception(typeName + " does not implement IExtension or is not configured correctly");
                        }
                        extension.Init();
                        Extensions.Add(key, extension);
                    }
                }
            }
        }

        internal static void LoadExtensions()
        {
            if (instance == null)
            {
                lock (Sync)
                {
                    if (instance == null)
                    {
                        instance = new ExtensionContainer();
                    }
                }
            }
        }
    }
}

