namespace Hidistro.Membership.ASPNETProvider
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Diagnostics;
    using System.Globalization;
    using System.Web.Hosting;
    using System.Xml;

    internal static class SecUtility
    {
        internal const int Infinite = 0x7fffffff;

        internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The array parameter '{0}' should not be empty.", paramName), paramName);
            }
            Hashtable hashtable = new Hashtable(param.Length);
            for (int i = param.Length - 1; i >= 0; i--)
            {
                CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
                if (hashtable.Contains(param[i]))
                {
                    throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The array '{0}' should not contain duplicate values.", paramName), paramName);
                }
                hashtable.Add(param[i], param[i]);
            }
        }

        internal static void CheckForbiddenAttribute(XmlNode node, string attrib)
        {
            XmlAttribute attribute = node.Attributes[attrib];
            if (attribute != null)
            {
                throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("Unrecognized attribute '{0}'. Note that attribute names are case-sensitive.", attrib), attribute);
            }
        }

        internal static void CheckForNonCommentChildNodes(XmlNode node)
        {
            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.NodeType != XmlNodeType.Comment)
                {
                    throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("Child nodes are not allowed."), node2);
                }
            }
        }

        internal static void CheckForUnrecognizedAttributes(XmlNode node)
        {
            if (node.Attributes.Count != 0)
            {
                throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("Unrecognized attribute '{0}'. Note that attribute names are case-sensitive.", node.Attributes[0].Name), node.Attributes[0]);
            }
        }

        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
            else
            {
                if (checkIfEmpty && (param.Length < 1))
                {
                    throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' must not be empty.", paramName), paramName);
                }
                if ((maxSize > 0) && (param.Length > maxSize))
                {
                    throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
                }
                if (checkForCommas && param.Contains(","))
                {
                    throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' must not contain commas.", paramName), paramName);
                }
            }
        }

        internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' must not be empty.", paramName), paramName);
            }
            if ((maxSize > 0) && (param.Length > maxSize))
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
            }
        }

        internal static void CheckUnrecognizedAttributes(NameValueCollection config, string providerName)
        {
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute '{0}' is unexpected in the configuration of the '{1}' provider.", key, providerName));
                }
            }
        }

        private static XmlNode GetAndRemoveAttribute(XmlNode node, string attrib, bool fRequired)
        {
            XmlNode node2 = node.Attributes.RemoveNamedItem(attrib);
            if (fRequired && (node2 == null))
            {
                throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The '{0}' attribute must be specified on the '{1}' tag.", attrib, node.Name), node);
            }
            return node2;
        }

        internal static XmlNode GetAndRemoveBooleanAttribute(XmlNode node, string attrib, ref bool val)
        {
            return GetAndRemoveBooleanAttributeInternal(node, attrib, false, ref val);
        }

        private static XmlNode GetAndRemoveBooleanAttributeInternal(XmlNode node, string attrib, bool fRequired, ref bool val)
        {
            XmlNode node2 = GetAndRemoveAttribute(node, attrib, fRequired);
            if (node2 != null)
            {
                if (node2.Value == "true")
                {
                    val = true;
                    return node2;
                }
                if (node2.Value != "false")
                {
                    throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The '{0}' attribute must be set to 'true' or 'false'.", node2.Name), node2);
                }
                val = false;
            }
            return node2;
        }

        internal static XmlNode GetAndRemoveNonEmptyStringAttribute(XmlNode node, string attrib, ref string val)
        {
            return GetAndRemoveNonEmptyStringAttributeInternal(node, attrib, false, ref val);
        }

        private static XmlNode GetAndRemoveNonEmptyStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
        {
            XmlNode node2 = GetAndRemoveStringAttributeInternal(node, attrib, fRequired, ref val);
            if ((node2 != null) && (val.Length == 0))
            {
                throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The '{0}' attribute cannot be an empty string.", attrib), node2);
            }
            return node2;
        }

        internal static void GetAndRemovePositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
        {
            GetPositiveAttribute(config, attrib, providerName, ref val);
            config.Remove(attrib);
        }

        internal static void GetAndRemovePositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
        {
            GetPositiveOrInfiniteAttribute(config, attrib, providerName, ref val);
            config.Remove(attrib);
        }

        internal static XmlNode GetAndRemoveStringAttribute(XmlNode node, string attrib, ref string val)
        {
            return GetAndRemoveStringAttributeInternal(node, attrib, false, ref val);
        }

        internal static void GetAndRemoveStringAttribute(NameValueCollection config, string attrib, string providerName, ref string val)
        {
            val = config.Get(attrib);
            config.Remove(attrib);
        }

        private static XmlNode GetAndRemoveStringAttributeInternal(XmlNode node, string attrib, bool fRequired, ref string val)
        {
            XmlNode node2 = GetAndRemoveAttribute(node, attrib, fRequired);
            if (node2 != null)
            {
                val = node2.Value;
            }
            return node2;
        }

        internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            bool flag;
            string str = config[valueName];
            if (str == null)
            {
                return defaultValue;
            }
            if (!bool.TryParse(str, out flag))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value must be boolean (true or false) for property '{0}'.", valueName));
            }
            return flag;
        }

        internal static string GetDefaultAppName()
        {
            try
            {
                string applicationVirtualPath = HostingEnvironment.ApplicationVirtualPath;
                if (string.IsNullOrEmpty(applicationVirtualPath))
                {
                    applicationVirtualPath = Process.GetCurrentProcess().MainModule.ModuleName;
                    int index = applicationVirtualPath.IndexOf('.');
                    if (index != -1)
                    {
                        applicationVirtualPath = applicationVirtualPath.Remove(index);
                    }
                }
                if (string.IsNullOrEmpty(applicationVirtualPath))
                {
                    return "/";
                }
                return applicationVirtualPath;
            }
            catch
            {
                return "/";
            }
        }

        internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            int num;
            string s = config[valueName];
            if (s == null)
            {
                return defaultValue;
            }
            if (!int.TryParse(s, out num))
            {
                if (zeroAllowed)
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value must be a non-negative 32-bit integer for property '{0}'.", valueName));
                }
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value must be a positive 32-bit integer for property '{0}'.", valueName));
            }
            if (zeroAllowed && (num < 0))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value must be a non-negative 32-bit integer for property '{0}'.", valueName));
            }
            if (!zeroAllowed && (num <= 0))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value must be a positive 32-bit integer for property '{0}'.", valueName));
            }
            if ((maxValueAllowed > 0) && (num > maxValueAllowed))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The value '{0}' can not be greater than '{1}'.", valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
            }
            return num;
        }

        internal static void GetPositiveAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
        {
            string str = config.Get(attrib);
            if (str != null)
            {
                int num;
                try
                {
                    num = Convert.ToInt32(str, CultureInfo.InvariantCulture);
                }
                catch (Exception exception)
                {
                    if (((exception is ArgumentException) || (exception is FormatException)) || (exception is OverflowException))
                    {
                        throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
                    }
                    throw;
                }
                if (num < 0)
                {
                    throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
                }
                val = num;
            }
        }

        internal static void GetPositiveOrInfiniteAttribute(NameValueCollection config, string attrib, string providerName, ref int val)
        {
            string str = config.Get(attrib);
            if (str != null)
            {
                int num;
                if (str == "Infinite")
                {
                    num = 0x7fffffff;
                }
                else
                {
                    try
                    {
                        num = Convert.ToInt32(str, CultureInfo.InvariantCulture);
                    }
                    catch (Exception exception)
                    {
                        if (((exception is ArgumentException) || (exception is FormatException)) || (exception is OverflowException))
                        {
                            throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
                        }
                        throw;
                    }
                    if (num < 0)
                    {
                        throw new ConfigurationErrorsException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute '{0}' is invalid in the configuration of the '{1}' provider. The attribute must be set to a non-negative integer.", attrib, providerName));
                    }
                }
                val = num;
            }
        }

        internal static string GetStringFromBool(bool flag)
        {
            if (!flag)
            {
                return "false";
            }
            return "true";
        }

        internal static bool IsAbsolutePhysicalPath(string path)
        {
            if ((path == null) || (path.Length < 3))
            {
                return false;
            }
            return (((path[1] == ':') && IsDirectorySeparatorChar(path[2])) || IsUncSharePath(path));
        }

        private static bool IsDirectorySeparatorChar(char ch)
        {
            if (ch != '\\')
            {
                return (ch == '/');
            }
            return true;
        }

        internal static bool IsRelativeUrl(string virtualPath)
        {
            if (virtualPath.IndexOf(":", StringComparison.Ordinal) != -1)
            {
                return false;
            }
            return !IsRooted(virtualPath);
        }

        internal static bool IsRooted(string basepath)
        {
            if (!string.IsNullOrEmpty(basepath) && (basepath[0] != '/'))
            {
                return (basepath[0] == '\\');
            }
            return true;
        }

        internal static bool IsUncSharePath(string path)
        {
            return (((path.Length > 2) && IsDirectorySeparatorChar(path[0])) && IsDirectorySeparatorChar(path[1]));
        }

        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param == null)
            {
                return !checkForNull;
            }
            return (((!checkIfEmpty || (param.Length >= 1)) && ((maxSize <= 0) || (param.Length <= maxSize))) && (!checkForCommas || !param.Contains(",")));
        }

        internal static bool ValidatePasswordParameter(ref string param, int maxSize)
        {
            if (param == null)
            {
                return false;
            }
            if (param.Length < 1)
            {
                return false;
            }
            if ((maxSize > 0) && (param.Length > maxSize))
            {
                return false;
            }
            return true;
        }
    }
}

