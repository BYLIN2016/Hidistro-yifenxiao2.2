namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using System;

    internal abstract class UserFactory
    {
        protected UserFactory()
        {
        }

        public abstract bool ChangeTradePassword(string username, string newPassword);
        public abstract bool ChangeTradePassword(string username, string oldPassword, string newPassword);
        public static UserFactory Create(UserRole role)
        {
            if (role == UserRole.Distributor)
            {
                return DistributorFactory.Instance();
            }
            if (role == UserRole.Member)
            {
                return MemberFactory.Instance();
            }
            if (role == UserRole.SiteManager)
            {
                return ManagerFactory.Instance();
            }
            if (role == UserRole.Underling)
            {
                return UnderlingFactory.Instance();
            }
            return null;
        }

        public abstract bool Create(IUser userToCreate);
        public abstract IUser GetUser(HiMembershipUser membershipUser);
        public abstract bool OpenBalance(int userId, string tradePassword);
        public abstract string ResetTradePassword(string username);
        public abstract bool UpdateUser(IUser user);
        public abstract bool ValidTradePassword(string username, string password);
    }
}

