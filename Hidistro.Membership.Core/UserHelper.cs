namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Security;

    public static class UserHelper
    {
        public static bool BindOpenId(string username, string openId, string openIdType)
        {
            return MemberUserProvider.Instance().BindOpenId(username, openId, openIdType);
        }

        public static CreateUserStatus Create(HiMembershipUser userToCreate, string[] roles)
        {
            return Create(userToCreate, null, null, roles);
        }

        public static CreateUserStatus Create(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer, string[] roles)
        {
            if (userToCreate == null)
            {
                return CreateUserStatus.UnknownFailure;
            }
            MemberUserProvider provider = MemberUserProvider.Instance();
            try
            {
                if (provider.CreateMembershipUser(userToCreate, passwordQuestion, passwordAnswer) == CreateUserStatus.Created)
                {
                    Roles.AddUserToRoles(userToCreate.Username, roles);
                }
            }
            catch (CreateUserException exception)
            {
                return exception.CreateUserStatus;
            }
            return CreateUserStatus.Created;
        }

        public static string CreateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public static string EncodePassword(MembershipPasswordFormat format, string cleanString, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(salt.ToLower() + cleanString);
            switch (format)
            {
                case MembershipPasswordFormat.Clear:
                    return cleanString;

                case MembershipPasswordFormat.Hashed:
                    return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes));
            }
            return BitConverter.ToString(((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes));
        }

        public static HiMembershipUser GetMembershipUser(int userId, string username, bool userIsOnline)
        {
            return MemberUserProvider.Instance().GetMembershipUser(userId, username, userIsOnline);
        }

        public static string GetUsernameWithOpenId(string openId, string openIdType)
        {
            return MemberUserProvider.Instance().GetUsernameWithOpenId(openId, openIdType);
        }

        public static bool UpdateUser(HiMembershipUser user)
        {
            if (user == null)
            {
                return false;
            }
            return MemberUserProvider.Instance().UpdateMembershipUser(user);
        }

        public static LoginUserStatus ValidateUser(HiMembershipUser user)
        {
            if (user == null)
            {
                return LoginUserStatus.UnknownError;
            }
            if (!user.IsApproved)
            {
                return LoginUserStatus.AccountPending;
            }
            if (user.IsLockedOut)
            {
                return LoginUserStatus.AccountLockedOut;
            }
            if (!HiMembership.ValidateUser(user.Username, user.Password))
            {
                return LoginUserStatus.InvalidCredentials;
            }
            return LoginUserStatus.Success;
        }
    }
}

