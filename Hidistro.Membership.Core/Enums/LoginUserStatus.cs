namespace Hidistro.Membership.Core.Enums
{
    using System;

    public enum LoginUserStatus
    {
        AccountLockedOut = 5,
        AccountPending = 2,
        InvalidCredentials = 0,
        Success = 1,
        UnknownError = 100
    }
}

