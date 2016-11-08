namespace Hidistro.Membership.ASPNETProvider
{
    using System;
    using System.Configuration;

    internal static class SqlConnectionHelper
    {
        private static object s_lock = new object();
        internal const string s_strUpperDataDirWithToken = "|DATADIRECTORY|";

        internal static SqlConnectionHolder GetConnection(string connectionString, bool revertImpersonation)
        {
            connectionString.ToUpperInvariant();
            SqlConnectionHolder holder = new SqlConnectionHolder(connectionString);
            bool flag = true;
            try
            {
                try
                {
                    holder.Open(null, revertImpersonation);
                    flag = false;
                }
                finally
                {
                    if (flag)
                    {
                        holder.Close();
                        holder = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return holder;
        }

        internal static string GetConnectionString(string specifiedConnectionString, bool lookupConnectionString, bool appLevel)
        {
            if ((specifiedConnectionString == null) || (specifiedConnectionString.Length < 1))
            {
                return null;
            }
            string connectionString = null;
            if (lookupConnectionString)
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[specifiedConnectionString];
                if (settings != null)
                {
                    connectionString = settings.ConnectionString;
                }
                if (connectionString != null)
                {
                    return connectionString;
                }
                return null;
            }
            return specifiedConnectionString;
        }
    }
}

