namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Web.Security;

    internal class DistributorFactory : UserFactory
    {
        private static readonly DistributorFactory _defaultInstance = new DistributorFactory();
        private BizActorProvider provider;

        static DistributorFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        private DistributorFactory()
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
            return this.provider.ChangeDistributorTradePassword(username, oldPassword, newPassword);
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return this.provider.CreateDistributor(userToCreate as Distributor);
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
            DbCommand sqlStringCommand = database.GetSqlStringCommand("SELECT TradePasswordFormat, TradePasswordSalt FROM aspnet_Distributors WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
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
            return this.provider.GetDistributor(membershipUser);
        }

        public static DistributorFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            return true;
        }

        public override string ResetTradePassword(string username)
        {
            int num;
            string str;
            SiteManager user = HiContext.Current.User as SiteManager;
            if (user == null)
            {
                return null;
            }
            string cleanString = Membership.GeneratePassword(10, 0);
            GetTradePassword(username, out num, out str);
            string str3 = UserHelper.EncodePassword((MembershipPasswordFormat) num, cleanString, str);
            if (str3.Length > 0x80)
            {
                return null;
            }
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE aspnet_Distributors SET TradePassword = @NewTradePassword, TradePasswordSalt = @PasswordSalt, TradePasswordFormat = @PasswordFormat WHERE UserId = (SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName)");
            database.AddInParameter(sqlStringCommand, "NewTradePassword", DbType.String, str3);
            database.AddInParameter(sqlStringCommand, "PasswordSalt", DbType.String, str);
            database.AddInParameter(sqlStringCommand, "PasswordFormat", DbType.Int32, num);
            database.AddInParameter(sqlStringCommand, "Username", DbType.String, username);
            database.ExecuteNonQuery(sqlStringCommand);
            return cleanString;
        }

        public override bool UpdateUser(IUser user)
        {
            return this.provider.UpdateDistributor(user as Distributor);
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return this.provider.ValidDistributorTradePassword(username, password);
        }
    }
}

