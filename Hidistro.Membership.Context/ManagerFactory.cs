namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using System;

    internal class ManagerFactory : UserFactory
    {
        private static readonly ManagerFactory _defaultInstance = new ManagerFactory();
        private BizActorProvider provider;

        static ManagerFactory()
        {
            _defaultInstance.provider = BizActorProvider.Instance();
        }

        private ManagerFactory()
        {
        }

        public override bool ChangeTradePassword(string username, string newPassword)
        {
            return true;
        }

        public override bool ChangeTradePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        public override bool Create(IUser userToCreate)
        {
            try
            {
                return this.provider.CreateManager(userToCreate as SiteManager);
            }
            catch
            {
                return false;
            }
        }

        public override IUser GetUser(HiMembershipUser membershipUser)
        {
            return this.provider.GetManager(membershipUser);
        }

        public static ManagerFactory Instance()
        {
            return _defaultInstance;
        }

        public override bool OpenBalance(int userId, string tradePassword)
        {
            return true;
        }

        public override string ResetTradePassword(string username)
        {
            return "000000";
        }

        public override bool UpdateUser(IUser user)
        {
            return true;
        }

        public override bool ValidTradePassword(string username, string password)
        {
            return true;
        }
    }
}

