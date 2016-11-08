namespace Hidistro.Membership.Core
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Web.Security;

    public interface IUser
    {
        bool ChangePassword(string newPassword);
        bool ChangePassword(string oldPassword, string newPassword);
        bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer);
        bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer);
        bool ChangePasswordWithAnswer(string answer, string newPassword);
        bool ChangeTradePassword(string newPassword);
        bool ChangeTradePassword(string oldPassword, string newPassword);
        IUserCookie GetUserCookie();
        bool IsInRole(string roleName);
        string ResetPassword(string answer);
        bool ValidatePasswordAnswer(string answer);

        DateTime? BirthDate { get; set; }

        string Comment { get; set; }

        DateTime CreateDate { get; }

        string Email { get; set; }

        Hidistro.Membership.Core.Enums.Gender Gender { get; set; }

        bool IsAnonymous { get; }

        bool IsApproved { get; set; }

        bool IsLockedOut { get; }

        DateTime LastActivityDate { get; set; }

        DateTime LastLockoutDate { get; }

        DateTime LastLoginDate { get; }

        DateTime LastPasswordChangedDate { get; }

        HiMembershipUser MembershipUser { get; }

        string MobilePIN { get; set; }

        string Password { get; set; }

        MembershipPasswordFormat PasswordFormat { get; set; }

        string PasswordQuestion { get; }

        string TradePassword { get; set; }

        MembershipPasswordFormat TradePasswordFormat { get; set; }

        int UserId { get; set; }

        string Username { get; set; }

        Hidistro.Membership.Core.Enums.UserRole UserRole { get; }
    }
}

