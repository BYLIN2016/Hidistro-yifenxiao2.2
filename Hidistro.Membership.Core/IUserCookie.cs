namespace Hidistro.Membership.Core
{
    using System;
    using System.Web;

    public interface IUserCookie
    {
        void DeleteCookie(HttpCookie cookie);
        void WriteCookie(HttpCookie cookie, int days, bool autoLogin);
    }
}

