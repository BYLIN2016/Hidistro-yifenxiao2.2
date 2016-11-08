namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using Hidistro.Core;

    public abstract class MemberUserProvider
    {
        private static readonly MemberUserProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.UserData,Hidistro.Membership.Data") as MemberUserProvider);

        protected MemberUserProvider()
        {
        }

        public abstract bool BindOpenId(string username, string openId, string openIdType);
        public abstract bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer);
        public abstract CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer);
        public abstract AnonymousUser GetAnonymousUser();
        public abstract HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline);
        public abstract string GetUsernameWithOpenId(string openId, string openIdType);
        public static MemberUserProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract bool UpdateMembershipUser(HiMembershipUser user);
        public abstract bool ValidatePasswordAnswer(string username, string answer);
    }
}

