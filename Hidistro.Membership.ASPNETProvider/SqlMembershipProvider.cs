namespace Hidistro.Membership.ASPNETProvider
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public class SqlMembershipProvider : MembershipProvider
    {
        private string _AppName;
        private int _CommandTimeout;
        private bool _EnablePasswordReset;
        private bool _EnablePasswordRetrieval;
        private int _MaxInvalidPasswordAttempts;
        private int _MinRequiredNonalphanumericCharacters;
        private int _MinRequiredPasswordLength;
        private int _PasswordAttemptWindow;
        private MembershipPasswordFormat _PasswordFormat;
        private string _PasswordStrengthRegularExpression;
        private bool _RequiresQuestionAndAnswer;
        private bool _RequiresUniqueEmail;
        private string _sqlConnectionString;
        private const int PASSWORD_SIZE = 14;

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            int num;
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            SecUtility.CheckParameter(ref oldPassword, true, true, false, 0x80, "oldPassword");
            SecUtility.CheckParameter(ref newPassword, true, true, false, 0x80, "newPassword");
            string salt = null;
            if (!this.CheckPassword(username, oldPassword, false, false, out salt, out num))
            {
                return false;
            }
            if (newPassword.Length < this.MinRequiredPasswordLength)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The length of parameter '{0}' needs to be greater or equal to '{1}'.", "newPassword", this.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)));
            }
            int num3 = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    num3++;
                }
            }
            if (num3 < this.MinRequiredNonAlphanumericCharacters)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("Non alpha numeric characters in '{0}' needs to be greater than or equal to '{1}'.", "newPassword", this.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)));
            }
            if ((this.PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, this.PasswordStrengthRegularExpression))
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' does not match the regular expression specified in config file.", "newPassword"));
            }
            string objValue = this.EncodePassword(newPassword, num, salt);
            if (objValue.Length > 0x80)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The password is too long: it must not exceed 128 chars after encrypting."), "newPassword");
            }
            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            this.OnValidatingPassword(e);
            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The custom password validation failed."), "newPassword");
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_SetPassword", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, objValue));
                    command.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, salt));
                    command.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int status = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    if (status != 0)
                    {
                        string exceptionText = this.GetExceptionText(status);
                        if (this.IsStatusDueToBadPassword(status))
                        {
                            throw new MembershipPasswordException(exceptionText);
                        }
                        throw new ProviderException(exceptionText);
                    }
                    flag = true;
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

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            string str;
            int num;
            string str2;
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            SecUtility.CheckParameter(ref password, true, true, false, 0x80, "password");
            if (!this.CheckPassword(username, password, false, false, out str, out num))
            {
                return false;
            }
            SecUtility.CheckParameter(ref newPasswordQuestion, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 0x100, "newPasswordQuestion");
            if (newPasswordAnswer != null)
            {
                newPasswordAnswer = newPasswordAnswer.Trim();
            }
            SecUtility.CheckParameter(ref newPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 0x80, "newPasswordAnswer");
            if (!string.IsNullOrEmpty(newPasswordAnswer))
            {
                str2 = this.EncodePassword(newPasswordAnswer.ToLower(CultureInfo.InvariantCulture), num, str);
            }
            else
            {
                str2 = newPasswordAnswer;
            }
            SecUtility.CheckParameter(ref str2, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 0x80, "newPasswordAnswer");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_ChangePasswordQuestionAndAnswer", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@NewPasswordQuestion", SqlDbType.NVarChar, newPasswordQuestion));
                    command.Parameters.Add(this.CreateInputParam("@NewPasswordAnswer", SqlDbType.NVarChar, str2));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int status = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    if (status != 0)
                    {
                        throw new ProviderException(this.GetExceptionText(status));
                    }
                    flag = status == 0;
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

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
        {
            string str;
            int num;
            return this.CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out str, out num);
        }

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved, out string salt, out int passwordFormat)
        {
            SqlConnectionHolder connection = null;
            string str;
            int num;
            int num2;
            int num3;
            bool flag2;
            DateTime time;
            DateTime time2;
            this.GetPasswordWithFormat(username, updateLastLoginActivityDate, out num, out str, out passwordFormat, out salt, out num2, out num3, out flag2, out time, out time2);
            if (num != 0)
            {
                return false;
            }
            if (!flag2 && failIfNotApproved)
            {
                return false;
            }
            string str2 = this.EncodePassword(password, passwordFormat, salt);
            bool objValue = str.Equals(str2);
            if ((objValue && (num2 == 0)) && (num3 == 0))
            {
                return true;
            }
            try
            {
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UpdateUserInfo", connection.Connection);
                    DateTime now = DateTime.Now;
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@IsPasswordCorrect", SqlDbType.Bit, objValue));
                    command.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
                    command.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
                    command.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, now));
                    command.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, objValue ? now : time));
                    command.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, objValue ? now : time2));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    num = (parameter.Value != null) ? ((int) parameter.Value) : -1;
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
            return objValue;
        }

        private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter parameter = new SqlParameter(paramName, dbType);
            if (objValue == null)
            {
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
                return parameter;
            }
            parameter.Value = objValue;
            return parameter;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            string str3;
            MembershipUser user;
            if (!SecUtility.ValidateParameter(ref password, true, true, false, 0x80))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            string salt = this.GenerateSalt();
            string objValue = this.EncodePassword(password, (int) this._PasswordFormat, salt);
            if (objValue.Length > 0x80)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }
            if (!string.IsNullOrEmpty(passwordAnswer))
            {
                if (passwordAnswer.Length > 0x80)
                {
                    status = MembershipCreateStatus.InvalidAnswer;
                    return null;
                }
                str3 = this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int) this._PasswordFormat, salt);
            }
            else
            {
                str3 = passwordAnswer;
            }
            if (!SecUtility.ValidateParameter(ref str3, this.RequiresQuestionAndAnswer, true, false, 0x80))
            {
                status = MembershipCreateStatus.InvalidAnswer;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref username, true, true, true, 0x100))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref email, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 0x100))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref passwordQuestion, this.RequiresQuestionAndAnswer, true, false, 0x100))
            {
                status = MembershipCreateStatus.InvalidQuestion;
                return null;
            }
            if (password.Length < this.MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            int num = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i))
                {
                    num++;
                }
            }
            if (num < this.MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if ((this.PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(password, this.PasswordStrengthRegularExpression))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, true);
            this.OnValidatingPassword(e);
            if (e.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    DateTime time = this.RoundToSeconds(DateTime.Now);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_CreateUser", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@Password", SqlDbType.NVarChar, objValue));
                    command.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, salt));
                    command.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
                    command.Parameters.Add(this.CreateInputParam("@PasswordQuestion", SqlDbType.NVarChar, passwordQuestion));
                    command.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, str3));
                    command.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, isApproved));
                    command.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
                    command.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, (int) this.PasswordFormat));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, time));
                    SqlParameter parameter = this.CreateInputParam("@UserId", SqlDbType.Int, providerUserKey);
                    parameter.Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int num3 = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    if ((num3 < 0) || (num3 > 11))
                    {
                        num3 = 11;
                    }
                    status = (MembershipCreateStatus) num3;
                    if (num3 != 0)
                    {
                        return null;
                    }
                    providerUserKey = (int) command.Parameters["@UserId"].Value;
                    user = new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, null, isApproved, false, time, time, time, time, new DateTime(0x6da, 1, 1));
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
            return user;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("aspnet_Membership_DeleteUser", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    SqlParameter parameter = new SqlParameter("@NumTablesDeletedFrom", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int num = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    flag = num > 0;
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

        internal string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
            {
                return pass;
            }
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            if (passwordFormat == 1)
            {
                inArray = HashAlgorithm.Create(Membership.HashAlgorithmType).ComputeHash(dst);
            }
            else
            {
                inArray = this.EncryptPassword(dst);
            }
            return Convert.ToBase64String(inArray);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users2;
            SecUtility.CheckParameter(ref emailToMatch, false, false, false, 0x100, "emailToMatch");
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            try
            {
                SqlConnectionHolder connection = null;
                totalRecords = 0;
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                parameter.Direction = ParameterDirection.ReturnValue;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_FindUsersByEmail", connection.Connection);
                    MembershipUserCollection users = new MembershipUserCollection();
                    SqlDataReader reader = null;
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@EmailToMatch", SqlDbType.NVarChar, emailToMatch));
                    command.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            string nullableString = this.GetNullableString(reader, 0);
                            string email = this.GetNullableString(reader, 1);
                            string passwordQuestion = this.GetNullableString(reader, 2);
                            string comment = this.GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(this.Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                        users2 = users;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int) parameter.Value;
                        }
                    }
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
            return users2;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users2;
            SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0x100, "usernameToMatch");
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            try
            {
                SqlConnectionHolder connection = null;
                totalRecords = 0;
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                parameter.Direction = ParameterDirection.ReturnValue;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_FindUsersByName", connection.Connection);
                    MembershipUserCollection users = new MembershipUserCollection();
                    SqlDataReader reader = null;
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
                    command.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            string nullableString = this.GetNullableString(reader, 0);
                            string email = this.GetNullableString(reader, 1);
                            string passwordQuestion = this.GetNullableString(reader, 2);
                            string comment = this.GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(this.Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                        users2 = users;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int) parameter.Value;
                        }
                    }
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
            return users2;
        }

        public virtual string GeneratePassword()
        {
            return Membership.GeneratePassword((this.MinRequiredPasswordLength < 14) ? 14 : this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
        }

        internal string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetAllUsers", connection.Connection);
                    SqlDataReader reader = null;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (reader.Read())
                        {
                            string nullableString = this.GetNullableString(reader, 0);
                            string email = this.GetNullableString(reader, 1);
                            string passwordQuestion = this.GetNullableString(reader, 2);
                            string comment = this.GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(this.Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int) parameter.Value;
                        }
                    }
                    return users;
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
            return users;
        }

        private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
        {
            int num;
            int num2;
            int num3;
            int num4;
            string str;
            string str2;
            bool flag;
            DateTime time;
            DateTime time2;
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }
            if (string.IsNullOrEmpty(passwordAnswer))
            {
                return passwordAnswer;
            }
            this.GetPasswordWithFormat(username, false, out num, out str, out num2, out str2, out num3, out num4, out flag, out time, out time2);
            if (num != 0)
            {
                throw new ProviderException(this.GetExceptionText(num));
            }
            return this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num2, str2);
        }

        private string GetExceptionText(int status)
        {
            string str;
            switch (status)
            {
                case 0:
                    return string.Empty;

                case 1:
                    str = "The user was not found.";
                    break;

                case 2:
                    str = "The password supplied is wrong.";
                    break;

                case 3:
                    str = "The password-answer supplied is wrong.";
                    break;

                case 4:
                    str = "The password supplied is invalid.  Passwords must conform to the password strength requirements configured for the default provider.";
                    break;

                case 5:
                    str = "The password-question supplied is invalid.  Note that the current provider configuration requires a valid password question and answer.  As a result, a CreateUser overload that accepts question and answer parameters must also be used.";
                    break;

                case 6:
                    str = "The password-answer supplied is invalid.";
                    break;

                case 7:
                    str = "The E-mail supplied is invalid.";
                    break;

                case 0x63:
                    str = "The user account has been locked out.";
                    break;

                default:
                    str = "The Provider encountered an unknown error.";
                    break;
            }
            return Hidistro.Membership.ASPNETProvider.SR.GetString(str);
        }

        private string GetNullableString(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetString(col);
            }
            return null;
        }

        public override int GetNumberOfUsersOnline()
        {
            int num2;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetNumberOfUsersOnline", connection.Connection);
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@MinutesSinceLastInActive", SqlDbType.Int, Membership.UserIsOnlineTimeWindow));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int num = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    num2 = num;
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
            return num2;
        }

        public override string GetPassword(string username, string passwordAnswer)
        {
            if (!this.EnablePasswordRetrieval)
            {
                throw new NotSupportedException(Hidistro.Membership.ASPNETProvider.SR.GetString("This Membership Provider has not been configured to support password retrieval."));
            }
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            string encodedPasswordAnswer = this.GetEncodedPasswordAnswer(username, passwordAnswer);
            SecUtility.CheckParameter(ref encodedPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 0x80, "passwordAnswer");
            int passwordFormat = 0;
            int status = 0;
            string pass = this.GetPasswordFromDB(username, encodedPasswordAnswer, this.RequiresQuestionAndAnswer, out passwordFormat, out status);
            if (pass != null)
            {
                return this.UnEncodePassword(pass, passwordFormat);
            }
            string exceptionText = this.GetExceptionText(status);
            if (this.IsStatusDueToBadPassword(status))
            {
                throw new MembershipPasswordException(exceptionText);
            }
            throw new ProviderException(exceptionText);
        }

        private string GetPasswordFromDB(string username, string passwordAnswer, bool requiresQuestionAndAnswer, out int passwordFormat, out int status)
        {
            string str2;
            try
            {
                SqlConnectionHolder connection = null;
                SqlDataReader reader = null;
                SqlParameter parameter = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetPassword", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
                    command.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    if (requiresQuestionAndAnswer)
                    {
                        command.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, passwordAnswer));
                    }
                    parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    string str = null;
                    status = -1;
                    if (reader.Read())
                    {
                        str = reader.GetString(0);
                        passwordFormat = reader.GetInt32(1);
                    }
                    else
                    {
                        str = null;
                        passwordFormat = 0;
                    }
                    str2 = str;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader = null;
                        status = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    }
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
            return str2;
        }

        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
        {
            try
            {
                SqlConnectionHolder connection = null;
                SqlDataReader reader = null;
                SqlParameter parameter = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetPasswordWithFormat", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    status = -1;
                    if (reader.Read())
                    {
                        password = reader.GetString(0);
                        passwordFormat = reader.GetInt32(1);
                        passwordSalt = reader.GetString(2);
                        failedPasswordAttemptCount = reader.GetInt32(3);
                        failedPasswordAnswerAttemptCount = reader.GetInt32(4);
                        isApproved = reader.GetBoolean(5);
                        lastLoginDate = reader.GetDateTime(6);
                        lastActivityDate = reader.GetDateTime(7);
                    }
                    else
                    {
                        password = null;
                        passwordFormat = 0;
                        passwordSalt = null;
                        failedPasswordAttemptCount = 0;
                        failedPasswordAnswerAttemptCount = 0;
                        isApproved = false;
                        lastLoginDate = DateTime.Now;
                        lastActivityDate = DateTime.Now;
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader = null;
                        status = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    }
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

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser user;
            if (providerUserKey == null)
            {
                throw new ArgumentNullException("providerUserKey");
            }
            SqlDataReader reader = null;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByUserId", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserId", SqlDbType.Int, providerUserKey));
                    command.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string nullableString = this.GetNullableString(reader, 0);
                        string passwordQuestion = this.GetNullableString(reader, 1);
                        string comment = this.GetNullableString(reader, 2);
                        bool boolean = reader.GetBoolean(3);
                        DateTime dateTime = reader.GetDateTime(4);
                        DateTime lastLoginDate = reader.GetDateTime(5);
                        DateTime lastActivityDate = reader.GetDateTime(6);
                        DateTime lastPasswordChangedDate = reader.GetDateTime(7);
                        string name = this.GetNullableString(reader, 8);
                        bool isLockedOut = reader.GetBoolean(9);
                        return new MembershipUser(this.Name, name, providerUserKey, nullableString, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, reader.GetDateTime(10));
                    }
                    user = null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader = null;
                    }
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
            return user;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser user;
            SecUtility.CheckParameter(ref username, true, false, true, 0x100, "username");
            SqlDataReader reader = null;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByName", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        string nullableString = this.GetNullableString(reader, 0);
                        string passwordQuestion = this.GetNullableString(reader, 1);
                        string comment = this.GetNullableString(reader, 2);
                        bool boolean = reader.GetBoolean(3);
                        DateTime dateTime = reader.GetDateTime(4);
                        DateTime lastLoginDate = reader.GetDateTime(5);
                        DateTime lastActivityDate = reader.GetDateTime(6);
                        DateTime lastPasswordChangedDate = reader.GetDateTime(7);
                        int providerUserKey = reader.GetInt32(8);
                        bool isLockedOut = reader.GetBoolean(9);
                        return new MembershipUser(this.Name, username, providerUserKey, nullableString, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, reader.GetDateTime(10));
                    }
                    user = null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                        reader = null;
                    }
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
            return user;
        }

        public override string GetUserNameByEmail(string email)
        {
            string str2;
            SecUtility.CheckParameter(ref email, false, false, false, 0x100, "email");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByEmail", connection.Connection);
                    string nullableString = null;
                    SqlDataReader reader = null;
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        if (reader.Read())
                        {
                            nullableString = this.GetNullableString(reader, 0);
                            if (this.RequiresUniqueEmail && reader.Read())
                            {
                                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("More than one user has the specified e-mail address."));
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
                    str2 = nullableString;
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
            return str2;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "SqlMembershipProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", Hidistro.Membership.ASPNETProvider.SR.GetString("SQL membership provider."));
            }
            base.Initialize(name, config);
            this._EnablePasswordRetrieval = SecUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
            this._EnablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", true);
            this._RequiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            this._RequiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", true);
            this._MaxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            this._PasswordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            this._MinRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 0x80);
            this._MinRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 0x80);
            this._PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (this._PasswordStrengthRegularExpression != null)
            {
                this._PasswordStrengthRegularExpression = this._PasswordStrengthRegularExpression.Trim();
                if (this._PasswordStrengthRegularExpression.Length == 0)
                {
                    goto Label_0156;
                }
                try
                {
                    new Regex(this._PasswordStrengthRegularExpression);
                    goto Label_0156;
                }
                catch (ArgumentException exception)
                {
                    throw new ProviderException(exception.Message, exception);
                }
            }
            this._PasswordStrengthRegularExpression = string.Empty;
        Label_0156:
            if (this._MinRequiredNonalphanumericCharacters > this._MinRequiredPasswordLength)
            {
                throw new HttpException(Hidistro.Membership.ASPNETProvider.SR.GetString("The minRequiredNonalphanumericCharacters can not be greater than minRequiredPasswordLength."));
            }
            this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
            this._AppName = config["applicationName"];
            if (string.IsNullOrEmpty(this._AppName))
            {
                this._AppName = SecUtility.GetDefaultAppName();
            }
            if (this._AppName.Length > 0x100)
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
            }
            string str = config["passwordFormat"];
            if (str == null)
            {
                str = "Hashed";
            }
            string str4 = str;
            if (str4 != null)
            {
                if (!(str4 == "Clear"))
                {
                    if (str4 == "Encrypted")
                    {
                        this._PasswordFormat = MembershipPasswordFormat.Encrypted;
                        goto Label_0246;
                    }
                    if (str4 == "Hashed")
                    {
                        this._PasswordFormat = MembershipPasswordFormat.Hashed;
                        goto Label_0246;
                    }
                }
                else
                {
                    this._PasswordFormat = MembershipPasswordFormat.Clear;
                    goto Label_0246;
                }
            }
            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Password format specified is invalid."));
        Label_0246:
            if ((this.PasswordFormat == MembershipPasswordFormat.Hashed) && this.EnablePasswordRetrieval)
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set supportsPasswordRetrieval to false."));
            }
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
            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("commandTimeout");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Attribute not recognized '{0}'", key));
                }
            }
        }

        private bool IsStatusDueToBadPassword(int status)
        {
            return (((status >= 2) && (status <= 6)) || (status == 0x63));
        }

        public override string ResetPassword(string username, string passwordAnswer)
        {
            string str;
            int num;
            string str2;
            int num2;
            int num3;
            int num4;
            bool flag;
            DateTime time;
            DateTime time2;
            if (!this.EnablePasswordReset)
            {
                throw new NotSupportedException(Hidistro.Membership.ASPNETProvider.SR.GetString("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file."));
            }
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            this.GetPasswordWithFormat(username, false, out num2, out str2, out num, out str, out num3, out num4, out flag, out time, out time2);
            if (num2 == 0)
            {
                string str3;
                string str6;
                if (passwordAnswer != null)
                {
                    passwordAnswer = passwordAnswer.Trim();
                }
                if (!string.IsNullOrEmpty(passwordAnswer))
                {
                    str3 = this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num, str);
                }
                else
                {
                    str3 = passwordAnswer;
                }
                SecUtility.CheckParameter(ref str3, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 0x80, "passwordAnswer");
                string password = this.GeneratePassword();
                ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, false);
                this.OnValidatingPassword(e);
                if (e.Cancel)
                {
                    if (e.FailureInformation != null)
                    {
                        throw e.FailureInformation;
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The custom password validation failed."));
                }
                try
                {
                    SqlConnectionHolder connection = null;
                    try
                    {
                        connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                        SqlCommand command = new SqlCommand("dbo.aspnet_Membership_ResetPassword", connection.Connection);
                        command.CommandTimeout = this.CommandTimeout;
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                        command.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, this.EncodePassword(password, num, str)));
                        command.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
                        command.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
                        command.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, str));
                        command.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
                        command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                        if (this.RequiresQuestionAndAnswer)
                        {
                            command.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, str3));
                        }
                        SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                        parameter.Direction = ParameterDirection.ReturnValue;
                        command.Parameters.Add(parameter);
                        command.ExecuteNonQuery();
                        num2 = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                        if (num2 != 0)
                        {
                            string exceptionText = this.GetExceptionText(num2);
                            if (this.IsStatusDueToBadPassword(num2))
                            {
                                throw new MembershipPasswordException(exceptionText);
                            }
                            throw new ProviderException(exceptionText);
                        }
                        str6 = password;
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
                return str6;
            }
            if (this.IsStatusDueToBadPassword(num2))
            {
                throw new MembershipPasswordException(this.GetExceptionText(num2));
            }
            throw new ProviderException(this.GetExceptionText(num2));
        }

        private DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        internal string UnEncodePassword(string pass, int passwordFormat)
        {
            switch (passwordFormat)
            {
                case 0:
                    return pass;

                case 1:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Hashed passwords cannot be decoded."));
            }
            byte[] encodedPassword = Convert.FromBase64String(pass);
            byte[] bytes = this.DecryptPassword(encodedPassword);
            if (bytes == null)
            {
                return null;
            }
            return Encoding.Unicode.GetString(bytes, 0x10, bytes.Length - 0x10);
        }

        public override bool UnlockUser(string username)
        {
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UnlockUser", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    if (((parameter.Value != null) ? ((int) parameter.Value) : -1) == 0)
                    {
                        return true;
                    }
                    flag = false;
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

        public override void UpdateUser(MembershipUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            string UserName = user.UserName;
            SecUtility.CheckParameter(ref UserName, true, true, true, 0x100, "UserName");
            string email = user.Email;
            SecUtility.CheckParameter(ref email, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 0x100, "Email");
            user.Email = email;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UpdateUser", connection.Connection);
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, user.UserName));
                    command.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, user.Email));
                    command.Parameters.Add(this.CreateInputParam("@Comment", SqlDbType.NText, user.Comment));
                    command.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, user.IsApproved ? 1 : 0));
                    command.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, user.LastLoginDate));
                    command.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, user.LastActivityDate));
                    command.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
                    command.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int status = (parameter.Value != null) ? ((int) parameter.Value) : -1;
                    if (status != 0)
                    {
                        throw new ProviderException(this.GetExceptionText(status));
                    }
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

        public override bool ValidateUser(string username, string password)
        {
            return ((SecUtility.ValidateParameter(ref username, true, true, true, 0x100) && SecUtility.ValidateParameter(ref password, true, true, false, 0x80)) && this.CheckPassword(username, password, true, true));
        }

        public override string ApplicationName
        {
            get
            {
                return this._AppName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Length > 0x100)
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
                }
                this._AppName = value;
            }
        }

        private int CommandTimeout
        {
            get
            {
                return this._CommandTimeout;
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return this._EnablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return this._EnablePasswordRetrieval;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return this._MaxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return this._MinRequiredNonalphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return this._MinRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return this._PasswordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return this._PasswordFormat;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return this._PasswordStrengthRegularExpression;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return this._RequiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return this._RequiresUniqueEmail;
            }
        }
    }
}

