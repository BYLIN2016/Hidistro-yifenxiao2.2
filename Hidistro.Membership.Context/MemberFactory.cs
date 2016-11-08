namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Web.Security;

    internal class MemberFactory : UserFactory
    {
        private static readonly MemberFactory _defaultInstance = new MemberFactory();
        private BizActorProvider provider;

        static MemberFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        private MemberFactory()
        {
        }

        public override bool ChangeTradePassword(string username, string newPassword)
        {
            SiteManager user = HiContext.Current.User as SiteManager;
            if (user == null)
            {
                return false;
            }
            string oldPassword = this.ResetTradePassword(username);
            return this.ChangeTradePassword(username, oldPassword, newPassword);
        }

        public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
        {
            return this.provider.ChangeMemberTradePassword(username, oldPassword, newPassword);
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return this.provider.CreateMember(userToCreate as Member);
            }
            catch
            {
                return false;
            }
        }

        private static void GetTradePassword(string username, out int passwordFormat, out string passwordSalt)
        {
            passwordFormat = 0;
            passwordSalt = null;
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Members WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
            database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    passwordFormat = reader.GetInt32(0);
                    passwordSalt = reader.GetString(1);
                }
            }
        }

        public override IUser GetUser(HiMembershipUser membershipUser)
        {
            return this.provider.GetMember(membershipUser);
        }

        public static MemberFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET IsOpenBalance = 'true', TradePassword = @TradePassword, TradePasswordSalt = @TradePasswordSalt, TradePasswordFormat = @TradePasswordFormat WHERE UserId = @UserId");
            string salt = UserHelper.CreateSalt();
            database.AddInParameter(sqlStringCommand, "TradePassword", DbType.String, UserHelper.EncodePassword(MembershipPasswordFormat.Hashed, tradePassword, salt));
            database.AddInParameter(sqlStringCommand, "TradePasswordSalt", DbType.String, salt);
            database.AddInParameter(sqlStringCommand, "TradePasswordFormat", DbType.Int32, 1);
            database.AddInParameter(sqlStringCommand, "UserId", DbType.String, userId);
            return (database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public override string ResetTradePassword(string username)
        {
            int num;
            string str;
            string cleanString = Membership.GeneratePassword(10, 0);
            GetTradePassword(username, out num, out str);
            string str3 = UserHelper.EncodePassword((MembershipPasswordFormat) num, cleanString, str);
            if (str3.Length > 0x80)
            {
                return null;
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Members SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
            database.AddInParameter(sqlStringCommand, "NewTradePassword", DbType.String, str3);
            database.AddInParameter(sqlStringCommand, "PasswordSalt", DbType.String, str);
            database.AddInParameter(sqlStringCommand, "PasswordFormat", DbType.Int32, num);
            database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            database.ExecuteNonQuery(sqlStringCommand);
            return cleanString;
        }

        public override bool UpdateUser(IUser user)
        {
            return this.provider.UpdateMember(user as Member);
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return this.provider.ValidMemberTradePassword(username, password);
        }
    }
}

