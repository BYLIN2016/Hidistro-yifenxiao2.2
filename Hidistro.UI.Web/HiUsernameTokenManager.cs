namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Microsoft.Web.Services3.Security.Tokens;
    using System;
    using System.Xml;

    public class HiUsernameTokenManager : UsernameTokenManager
    {
        public HiUsernameTokenManager()
        {
        }

        public HiUsernameTokenManager(XmlNodeList nodes) : base(nodes)
        {
        }

        protected override string AuthenticateToken(UsernameToken token)
        {
            LoginUserStatus invalidCredentials;
            try
            {
                SiteManager user = Users.GetUser(0, token.Identity.Name, false, false) as SiteManager;
                if ((user != null) && user.IsAdministrator)
                {
                    HiContext current = HiContext.Current;
                    user.Password = HiCryptographer.Decrypt(token.Password);
                    invalidCredentials = Users.ValidateUser(user);
                }
                else
                {
                    invalidCredentials = LoginUserStatus.InvalidCredentials;
                }
            }
            catch
            {
                invalidCredentials = LoginUserStatus.InvalidCredentials;
            }
            if (invalidCredentials == LoginUserStatus.Success)
            {
                return token.Password;
            }
            return HiCryptographer.CreateHash(token.Password);
        }
    }
}

