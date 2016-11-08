namespace Hidistro.Membership.Context
{
    using Hidistro.Membership.Core;
    using System;
    using Hidistro.Core;

    public abstract class BizActorProvider
    {
        private static readonly BizActorProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.BizActorData,Hidistro.Membership.Data") as BizActorProvider);

        protected BizActorProvider()
        {
        }

        public abstract bool ChangeDistributorTradePassword(string username, string oldPassword, string newPassword);
        public abstract bool ChangeMemberTradePassword(string username, string oldPassword, string newPassword);
        public abstract bool ChangeUnderlingTradePassword(string username, string oldPassword, string newPassword);
        public abstract bool CreateDistributor(Distributor distributor);
        public abstract bool CreateManager(SiteManager manager);
        public abstract bool CreateMember(Member member);
        public abstract bool CreateUnderling(Member underling);
        public abstract Distributor GetDistributor(HiMembershipUser membershipUser);
        public abstract SiteManager GetManager(HiMembershipUser membershipUser);
        public abstract Member GetMember(HiMembershipUser membershipUser);
        public abstract Member GetUnderling(HiMembershipUser membershipUser);
        public static BizActorProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool UpdateDistributor(Distributor distributor);
        public abstract bool UpdateMember(Member member);
        public abstract bool UpdateUnderling(Member underling);
        public abstract bool ValidDistributorTradePassword(string username, string password);
        public abstract bool ValidMemberTradePassword(string username, string password);
        public abstract bool ValidUnderlingTradePassword(string username, string password);
    }
}

