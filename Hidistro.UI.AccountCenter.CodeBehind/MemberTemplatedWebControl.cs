namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [PersistChildren(false), ParseChildren(true)]
    public abstract class MemberTemplatedWebControl : HtmlTemplatedWebControl
    {
        protected MemberTemplatedWebControl()
        {
            if ((HiContext.Current.User.UserRole != UserRole.Member) && (HiContext.Current.User.UserRole != UserRole.Underling))
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("login", new object[] { this.Page.Request.RawUrl }), true);
            }
        }
    }
}

