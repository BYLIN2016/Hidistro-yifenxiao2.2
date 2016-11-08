namespace Hidistro.Membership.Core.Enums
{
    using System;

    public enum CreateUserStatus
    {
        UnknownFailure,
        Created,
        DuplicateUsername,
        DuplicateEmailAddress,
        InvalidFirstCharacter,
        DisallowedUsername,
        Updated,
        Deleted,
        InvalidQuestionAnswer,
        InvalidPassword,
        InvalidEmail,
        InvalidUserName
    }
}

