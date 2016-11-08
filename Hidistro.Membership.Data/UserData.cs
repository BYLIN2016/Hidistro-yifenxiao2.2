namespace Hidistro.Membership.Data
{
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Web.Security;

    public class UserData : MemberUserProvider
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public override bool BindOpenId(string username, string openId, string openIdType)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenId_Bind");
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "OpenId", DbType.String, openId);
            this.database.AddInParameter(storedProcCommand, "OpenIdType", DbType.String, openIdType);
            return (this.database.ExecuteNonQuery(storedProcCommand) == 1);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer)
        {
            int num;
            int num2;
            string str;
            this.GetPasswordWithFormat(username, true, out num2, out num, out str);
            if (num2 != 0)
            {
                return false;
            }
            string str2 = UserHelper.EncodePassword((MembershipPasswordFormat) num, newAnswer, str);
            if (str2.Length > 0x80)
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer WHERE LOWER(@Username) = LoweredUserName");
            this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", DbType.String, newQuestion);
            this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", DbType.String, str2);
            this.database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer)
        {
            CreateUserStatus unknownFailure = CreateUserStatus.UnknownFailure;
            if (userToCreate == null)
            {
                return CreateUserStatus.UnknownFailure;
            }
            bool flag = false;
            if (!string.IsNullOrEmpty(passwordQuestion) && !string.IsNullOrEmpty(passwordAnswer))
            {
                flag = true;
                if ((passwordAnswer.Length > 0x80) || (passwordQuestion.Length > 0x100))
                {
                    throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
                }
            }
            MembershipUser user = HiMembership.Create(userToCreate.Username, userToCreate.Password, userToCreate.Email);
            if (user != null)
            {
                userToCreate.UserId = (int) user.ProviderUserKey;
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET IsAnonymous = @IsAnonymous, IsApproved = @IsApproved, PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer, Gender = @Gender, BirthDate = @BirthDate, UserRole = @UserRole WHERE UserId = @UserId");
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userToCreate.UserId);
                this.database.AddInParameter(sqlStringCommand, "IsAnonymous", DbType.Boolean, userToCreate.IsAnonymous);
                this.database.AddInParameter(sqlStringCommand, "IsApproved", DbType.Boolean, userToCreate.IsApproved);
                this.database.AddInParameter(sqlStringCommand, "Gender", DbType.Int32, (int) userToCreate.Gender);
                this.database.AddInParameter(sqlStringCommand, "BirthDate", DbType.DateTime, null);
                this.database.AddInParameter(sqlStringCommand, "UserRole", DbType.Int32, (int) userToCreate.UserRole);
                this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", DbType.String, null);
                this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", DbType.String, null);
                if (userToCreate.BirthDate.HasValue)
                {
                    this.database.SetParameterValue(sqlStringCommand, "BirthDate", userToCreate.BirthDate.Value);
                }
                if (flag)
                {
                    string str2 = null;
                    try
                    {
                        int num;
                        int num2;
                        string str;
                        this.GetPasswordWithFormat(userToCreate.Username, false, out num2, out num, out str);
                        if (num2 == 0)
                        {
                            str2 = UserHelper.EncodePassword((MembershipPasswordFormat) num, passwordAnswer, str);
                            this.database.SetParameterValue(sqlStringCommand, "PasswordQuestion", passwordQuestion);
                            this.database.SetParameterValue(sqlStringCommand, "PasswordAnswer", str2);
                        }
                        if ((num2 != 0) || (!string.IsNullOrEmpty(str2) && (str2.Length > 0x80)))
                        {
                            HiMembership.Delete(userToCreate.Username);
                            throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
                        }
                    }
                    catch
                    {
                        HiMembership.Delete(userToCreate.Username);
                        throw new CreateUserException(CreateUserStatus.UnknownFailure);
                    }
                }
                if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
                {
                    unknownFailure = CreateUserStatus.Created;
                }
                else
                {
                    HiMembership.Delete(userToCreate.Username);
                    throw new CreateUserException(unknownFailure);
                }
            }
            return unknownFailure;
        }

        public override AnonymousUser GetAnonymousUser()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT @UserId = UserId FROM aspnet_Users WHERE IsAnonymous = 1");
            this.database.AddOutParameter(sqlStringCommand, "UserId", DbType.Int32, 4);
            this.database.ExecuteNonQuery(sqlStringCommand);
            int parameterValue = (int) this.database.GetParameterValue(sqlStringCommand, "UserId");
            return new AnonymousUser(this.GetMembershipUser(parameterValue, "Anonymous", true));
        }

        public override HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline)
        {
            MembershipUser mu = string.IsNullOrEmpty(username) ? HiMembership.GetUser(userId, isOnline) : HiMembership.GetUser(username, isOnline);
            if (mu == null)
            {
                return null;
            }
            HiMembershipUser user2 = null;
            DbCommand sqlStringCommand = null;
            if (!string.IsNullOrEmpty(username))
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole FROM aspnet_Users WHERE LoweredUserName = LOWER(@Username)");
                this.database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            }
            else
            {
                sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole FROM aspnet_Users WHERE UserId = @UserId");
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    user2 = new HiMembershipUser((bool) reader["IsAnonymous"], (UserRole) Convert.ToInt32(reader["UserRole"]), mu);
                    if (reader["MobilePIN"] != DBNull.Value)
                    {
                        user2.MobilePIN = (string) reader["MobilePIN"];
                    }
                    if (reader["Gender"] != DBNull.Value)
                    {
                        user2.Gender = (Gender) Convert.ToInt32(reader["Gender"]);
                    }
                    if (reader["BirthDate"] != DBNull.Value)
                    {
                        user2.BirthDate = new DateTime?((DateTime) reader["BirthDate"]);
                    }
                }
                reader.Close();
            }
            return user2;
        }

        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out int passwordFormat, out string passwordSalt)
        {
            passwordFormat = 0;
            passwordSalt = null;
            status = -1;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", DbType.Boolean, updateLastLoginActivityDate);
            this.database.AddInParameter(storedProcCommand, "CurrentTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (reader.Read())
                {
                    passwordFormat = reader.GetInt32(1);
                    passwordSalt = reader.GetString(2);
                    status = 0;
                }
            }
        }

        private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
        {
            password = null;
            passwordFormat = 0;
            passwordSalt = null;
            failedPasswordAttemptCount = 0;
            failedPasswordAnswerAttemptCount = 0;
            isApproved = false;
            lastLoginDate = DateTime.Now;
            lastActivityDate = DateTime.Now;
            status = -1;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, username);
            this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", DbType.Boolean, updateLastLoginActivityDate);
            this.database.AddInParameter(storedProcCommand, "CurrentTime", DbType.DateTime, DateTime.Now);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
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
                    status = 0;
                }
            }
        }

        public override string GetUsernameWithOpenId(string openId, string openIdType)
        {
            string str = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserName FROM aspnet_Users WHERE LOWER(OpenId)=LOWER(@OpenId) AND LOWER(OpenIdType)=LOWER(@OpenIdType)");
            this.database.AddInParameter(sqlStringCommand, "OpenId", DbType.String, openId);
            this.database.AddInParameter(sqlStringCommand, "OpenIdType", DbType.String, openIdType);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    str = reader.GetString(0);
                }
            }
            return str;
        }

        public override bool UpdateMembershipUser(HiMembershipUser user)
        {
            if (user == null)
            {
                return false;
            }
            try
            {
                HiMembership.Update(user.Membership);
            }
            catch
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET MobilePIN = @MobilePIN, Gender = @Gender, BirthDate = @BirthDate WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "MobilePIN", DbType.String, user.MobilePIN);
            this.database.AddInParameter(sqlStringCommand, "Gender", DbType.Int32, (int) user.Gender);
            this.database.AddInParameter(sqlStringCommand, "BirthDate", DbType.DateTime, user.BirthDate);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, user.UserId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public override bool ValidatePasswordAnswer(string username, string answer)
        {
            int num;
            int num2;
            string str;
            this.GetPasswordWithFormat(username, true, out num2, out num, out str);
            if (num2 != 0)
            {
                return false;
            }
            string str2 = UserHelper.EncodePassword((MembershipPasswordFormat) num, answer, str);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName AND (PasswordAnswer = @PasswordAnswer OR (PasswordQuestion IS NULL AND PasswordAnswer IS NULL))");
            this.database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", DbType.String, str2);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            return ((obj2 != null) && (obj2 != DBNull.Value));
        }
    }
}

