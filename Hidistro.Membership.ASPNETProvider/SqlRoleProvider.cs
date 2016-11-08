namespace Hidistro.Membership.ASPNETProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Security;

    public class SqlRoleProvider : RoleProvider
    {
        private string _AppName;
        private int _CommandTimeout;
        private string _sqlConnectionString;

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0x100, "roleNames");
            SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0x100, "usernames");
            bool flag = false;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    int length = usernames.Length;
                    while (length > 0)
                    {
                        string str = usernames[usernames.Length - length];
                        length--;
                        int index = usernames.Length - length;
                        while (index < usernames.Length)
                        {
                            if (((str.Length + usernames[index].Length) + 1) >= 0xfa0)
                            {
                                break;
                            }
                            str = str + "," + usernames[index];
                            length--;
                            index++;
                        }
                        int num3 = roleNames.Length;
                        while (num3 > 0)
                        {
                            string str2 = roleNames[roleNames.Length - num3];
                            num3--;
                            for (index = roleNames.Length - num3; index < roleNames.Length; index++)
                            {
                                if (((str2.Length + roleNames[index].Length) + 1) >= 0xfa0)
                                {
                                    break;
                                }
                                str2 = str2 + "," + roleNames[index];
                                num3--;
                            }
                            if (!flag && ((length > 0) || (num3 > 0)))
                            {
                                new SqlCommand("BEGIN TRANSACTION", connection.Connection).ExecuteNonQuery();
                                flag = true;
                            }
                            this.AddUsersToRolesCore(connection.Connection, str, str2);
                        }
                    }
                    if (flag)
                    {
                        new SqlCommand("COMMIT TRANSACTION", connection.Connection).ExecuteNonQuery();
                        flag = false;
                    }
                }
                catch
                {
                    if (flag)
                    {
                        try
                        {
                            new SqlCommand("ROLLBACK TRANSACTION", connection.Connection).ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                        flag = false;
                    }
                    throw;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void AddUsersToRolesCore(SqlConnection conn, string usernames, string roleNames)
        {
            SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_AddUsersToRoles", conn);
            SqlDataReader reader = null;
            SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
            string str = string.Empty;
            string str2 = string.Empty;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = this.CommandTimeout;
            parameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(parameter);
            cmd.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
            cmd.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
            cmd.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    if (reader.FieldCount > 0)
                    {
                        str = reader.GetString(0);
                    }
                    if (reader.FieldCount > 1)
                    {
                        str2 = reader.GetString(1);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            switch (this.GetReturnValue(cmd))
            {
                case 0:
                    return;

                case 1:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The user '{0}' was not found.", str));

                case 2:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The role '{0}' was not found.", str));

                case 3:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The user '{0}' is already in role '{1}'.", str, str2));
            }
            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
        }

        private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter parameter = new SqlParameter(paramName, dbType);
            if (objValue == null)
            {
                objValue = string.Empty;
            }
            parameter.Value = objValue;
            return parameter;
        }

        public override void CreateRole(string roleName)
        {
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_Roles_CreateRole", connection.Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    cmd.ExecuteNonQuery();
                    switch (this.GetReturnValue(cmd))
                    {
                        case 0:
                            return;

                        case 1:
                            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The role '{0}' already exists.", roleName));
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            bool flag;
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_Roles_DeleteRole", connection.Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    cmd.Parameters.Add(this.CreateInputParam("@DeleteOnlyIfRoleIsEmpty", SqlDbType.Bit, throwOnPopulatedRole ? 1 : 0));
                    cmd.ExecuteNonQuery();
                    int returnValue = this.GetReturnValue(cmd);
                    if (returnValue == 2)
                    {
                        throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("This role cannot be deleted because there are users present in it."));
                    }
                    flag = returnValue == 0;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            string[] strArray2;
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0x100, "usernameToMatch");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_FindUsersInRole", connection.Connection);
                    SqlDataReader reader = null;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    StringCollection strings = new StringCollection();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    cmd.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
                    try
                    {
                        reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            strings.Add(reader.GetString(0));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                    if (strings.Count < 1)
                    {
                        switch (this.GetReturnValue(cmd))
                        {
                            case 0:
                                return new string[0];

                            case 1:
                                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The role '{0}' was not found.", roleName));
                        }
                        throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                    }
                    string[] array = new string[strings.Count];
                    strings.CopyTo(array, 0);
                    strArray2 = array;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return strArray2;
        }

        public override string[] GetAllRoles()
        {
            string[] strArray2;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Roles_GetAllRoles", connection.Connection);
                    StringCollection strings = new StringCollection();
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    SqlDataReader reader = null;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = this.CommandTimeout;
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            strings.Add(reader.GetString(0));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                    string[] array = new string[strings.Count];
                    strings.CopyTo(array, 0);
                    strArray2 = array;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return strArray2;
        }

        private int GetReturnValue(SqlCommand cmd)
        {
            foreach (SqlParameter parameter in cmd.Parameters)
            {
                if (((parameter.Direction == ParameterDirection.ReturnValue) && (parameter.Value != null)) && (parameter.Value is int))
                {
                    return (int) parameter.Value;
                }
            }
            return -1;
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] strArray2;
            SecUtility.CheckParameter(ref username, true, false, true, 0x100, "username");
            if (username.Length < 1)
            {
                return new string[0];
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_GetRolesForUser", connection.Connection);
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    SqlDataReader reader = null;
                    StringCollection strings = new StringCollection();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    try
                    {
                        reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            strings.Add(reader.GetString(0));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                    if (strings.Count > 0)
                    {
                        string[] array = new string[strings.Count];
                        strings.CopyTo(array, 0);
                        return array;
                    }
                    switch (this.GetReturnValue(cmd))
                    {
                        case 0:
                            return new string[0];

                        case 1:
                            return new string[0];
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return strArray2;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            string[] strArray2;
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_GetUsersInRoles", connection.Connection);
                    SqlDataReader reader = null;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    StringCollection strings = new StringCollection();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    try
                    {
                        reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            strings.Add(reader.GetString(0));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                    }
                    if (strings.Count < 1)
                    {
                        switch (this.GetReturnValue(cmd))
                        {
                            case 0:
                                return new string[0];

                            case 1:
                                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The role '{0}' was not found.", roleName));
                        }
                        throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                    }
                    string[] array = new string[strings.Count];
                    strings.CopyTo(array, 0);
                    strArray2 = array;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return strArray2;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "SqlRoleProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", Hidistro.Membership.ASPNETProvider.SR.GetString("SQL role provider."));
            }
            base.Initialize(name, config);
            this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
            string specifiedConnectionString = config["connectionStringName"];
            if ((specifiedConnectionString == null) || (specifiedConnectionString.Length < 1))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute 'connectionStringName' is missing or empty."));
            }
            this._sqlConnectionString = SqlConnectionHelper.GetConnectionString(specifiedConnectionString, true, true);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The connection name '{0}' was not found in the applications configuration or the connection string is empty.", specifiedConnectionString));
            }
            this._AppName = config["applicationName"];
            if (string.IsNullOrEmpty(this._AppName))
            {
                this._AppName = SecUtility.GetDefaultAppName();
            }
            if (this._AppName.Length > 0x100)
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
            }
            config.Remove("connectionStringName");
            config.Remove("applicationName");
            config.Remove("commandTimeout");
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Attribute not recognized '{0}'", key));
                }
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool flag;
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            SecUtility.CheckParameter(ref username, true, false, true, 0x100, "username");
            if (username.Length < 1)
            {
                return false;
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_IsUserInRole", connection.Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    cmd.ExecuteNonQuery();
                    switch (this.GetReturnValue(cmd))
                    {
                        case 0:
                            return false;

                        case 1:
                            return true;

                        case 2:
                            return false;

                        case 3:
                            return false;
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 0x100, "roleNames");
            SecUtility.CheckArrayParameter(ref usernames, true, true, true, 0x100, "usernames");
            bool flag = false;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    int length = usernames.Length;
                    while (length > 0)
                    {
                        string str = usernames[usernames.Length - length];
                        length--;
                        int index = usernames.Length - length;
                        while (index < usernames.Length)
                        {
                            if (((str.Length + usernames[index].Length) + 1) >= 0xfa0)
                            {
                                break;
                            }
                            str = str + "," + usernames[index];
                            length--;
                            index++;
                        }
                        int num3 = roleNames.Length;
                        while (num3 > 0)
                        {
                            string str2 = roleNames[roleNames.Length - num3];
                            num3--;
                            for (index = roleNames.Length - num3; index < roleNames.Length; index++)
                            {
                                if (((str2.Length + roleNames[index].Length) + 1) >= 0xfa0)
                                {
                                    break;
                                }
                                str2 = str2 + "," + roleNames[index];
                                num3--;
                            }
                            if (!flag && ((length > 0) || (num3 > 0)))
                            {
                                new SqlCommand("BEGIN TRANSACTION", connection.Connection).ExecuteNonQuery();
                                flag = true;
                            }
                            this.RemoveUsersFromRolesCore(connection.Connection, str, str2);
                        }
                    }
                    if (flag)
                    {
                        new SqlCommand("COMMIT TRANSACTION", connection.Connection).ExecuteNonQuery();
                        flag = false;
                    }
                }
                catch
                {
                    if (flag)
                    {
                        new SqlCommand("ROLLBACK TRANSACTION", connection.Connection).ExecuteNonQuery();
                        flag = false;
                    }
                    throw;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void RemoveUsersFromRolesCore(SqlConnection conn, string usernames, string roleNames)
        {
            SqlCommand cmd = new SqlCommand("dbo.aspnet_UsersInRoles_RemoveUsersFromRoles", conn);
            SqlDataReader reader = null;
            SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
            string str = string.Empty;
            string str2 = string.Empty;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = this.CommandTimeout;
            parameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(parameter);
            cmd.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
            cmd.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    if (reader.FieldCount > 0)
                    {
                        str = reader.GetString(0);
                    }
                    if (reader.FieldCount > 1)
                    {
                        str2 = reader.GetString(1);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            switch (this.GetReturnValue(cmd))
            {
                case 0:
                    return;

                case 1:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The user '{0}' was not found.", str));

                case 2:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The role '{0}' was not found.", str2));

                case 3:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The user '{0}' is already not in role '{1}'.", str, str2));
            }
            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
        }

        public override bool RoleExists(string roleName)
        {
            bool flag;
            SecUtility.CheckParameter(ref roleName, true, true, true, 0x100, "roleName");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand cmd = new SqlCommand("dbo.aspnet_Roles_RoleExists", connection.Connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.CommandTimeout;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(parameter);
                    cmd.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
                    cmd.ExecuteNonQuery();
                    switch (this.GetReturnValue(cmd))
                    {
                        case 0:
                            return false;

                        case 1:
                            return true;
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Stored procedure call failed."));
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

        public override string ApplicationName
        {
            get
            {
                return this._AppName;
            }
            set
            {
                this._AppName = value;
                if (this._AppName.Length > 0x100)
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
                }
            }
        }

        private int CommandTimeout
        {
            get
            {
                return this._CommandTimeout;
            }
        }
    }
}

