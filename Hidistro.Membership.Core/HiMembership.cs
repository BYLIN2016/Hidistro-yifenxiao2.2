namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Web.Security;

    public static class HiMembership
    {
        public static MembershipUser Create(string username, string password, string email)
        {
            MembershipUser user = null;
            CreateUserStatus unknownFailure = CreateUserStatus.UnknownFailure;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user = Membership.CreateUser(username, password, email);
                    unknownFailure = (user == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created;
                }
                else
                {
                    user = Membership.CreateUser(username, password);
                    unknownFailure = (user == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created;
                }
            }
            catch (MembershipCreateUserException exception)
            {
                unknownFailure = GetCreateUserStatus(exception.StatusCode);
            }
            catch (Exception exception2)
            {
                unknownFailure = GetCreateUserStatus(exception2);
            }
            if (unknownFailure != CreateUserStatus.Created)
            {
                throw new CreateUserException(unknownFailure);
            }
            return user;
        }

        public static bool Delete(string username)
        {
            return Membership.DeleteUser(username, true);
        }

        public static string GeneratePassword(int length, int alphaNumbericCharacters)
        {
            return Membership.GeneratePassword(length, alphaNumbericCharacters);
        }

        public static CreateUserStatus GetCreateUserStatus(Exception ex)
        {
            MembershipCreateUserException exception = ex as MembershipCreateUserException;
            if (exception != null)
            {
                return GetCreateUserStatus(exception.StatusCode);
            }
            return CreateUserStatus.UnknownFailure;
        }

        public static CreateUserStatus GetCreateUserStatus(MembershipCreateStatus msc)
        {
            switch (msc)
            {
                case MembershipCreateStatus.Success:
                    return CreateUserStatus.Created;

                case MembershipCreateStatus.InvalidUserName:
                    return CreateUserStatus.InvalidUserName;

                case MembershipCreateStatus.InvalidPassword:
                    return CreateUserStatus.InvalidPassword;

                case MembershipCreateStatus.InvalidQuestion:
                    return CreateUserStatus.InvalidQuestionAnswer;

                case MembershipCreateStatus.InvalidAnswer:
                    return CreateUserStatus.InvalidQuestionAnswer;

                case MembershipCreateStatus.InvalidEmail:
                    return CreateUserStatus.InvalidEmail;

                case MembershipCreateStatus.DuplicateUserName:
                    return CreateUserStatus.DuplicateUsername;

                case MembershipCreateStatus.DuplicateEmail:
                    return CreateUserStatus.DuplicateEmailAddress;

                case MembershipCreateStatus.UserRejected:
                    return CreateUserStatus.DisallowedUsername;

                case MembershipCreateStatus.InvalidProviderUserKey:
                    return CreateUserStatus.UnknownFailure;

                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return CreateUserStatus.UnknownFailure;

                case MembershipCreateStatus.ProviderError:
                    return CreateUserStatus.UnknownFailure;
            }
            return CreateUserStatus.UnknownFailure;
        }

        public static MembershipUser GetUser(object userId)
        {
            return GetUser(userId, false);
        }

        public static MembershipUser GetUser(string username)
        {
            return GetUser(username, false);
        }

        public static MembershipUser GetUser(object userId, bool userIsOnline)
        {
            return Membership.GetUser(userId, userIsOnline);
        }

        public static MembershipUser GetUser(string username, bool userIsOnline)
        {
            return Membership.GetUser(username, userIsOnline);
        }

        public static bool PasswordIsMembershipCompliant(string newPassword, out string errorMessage)
        {
            errorMessage = "";
            if (null == newPassword)
            {
                return false;
            }
            int minRequiredPasswordLength = Membership.MinRequiredPasswordLength;
            int minRequiredNonAlphanumericCharacters = Membership.MinRequiredNonAlphanumericCharacters;
            if (newPassword.Length < minRequiredPasswordLength)
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, "密码太短，最少需要 {0} 个字符", new object[] { Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture) });
                return false;
            }
            int num3 = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    num3++;
                }
            }
            if (num3 < minRequiredNonAlphanumericCharacters)
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, "密码包含的特殊字符太少, 最少要包含 {0} 个特殊字符", new object[] { Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture) });
                return false;
            }
            return true;
        }

        public static void Update(MembershipUser user)
        {
            if (user == null)
            {
                throw new Exception("Member can not be null");
            }
            Membership.UpdateUser(user);
        }

        public static bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }
    }
}

