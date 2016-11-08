namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using System.Web.UI;

    public class AdminPage : Page
    {
        private void CheckPageAccess()
        {
            IUser user = HiContext.Current.User;
            if (user.UserRole != UserRole.SiteManager)
            {
                this.Page.Response.Redirect(Globals.GetSiteUrls().Login, true);
            }
            else
            {
                SiteManager manager = user as SiteManager;
                if (!manager.IsAdministrator)
                {
                    AdministerCheckAttribute customAttribute = (AdministerCheckAttribute) Attribute.GetCustomAttribute(base.GetType(), typeof(AdministerCheckAttribute));
                    if ((customAttribute != null) && customAttribute.AdministratorOnly)
                    {
                        this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/AccessDenied.aspx"));
                    }
                    PrivilegeCheckAttribute attribute2 = (PrivilegeCheckAttribute) Attribute.GetCustomAttribute(base.GetType(), typeof(PrivilegeCheckAttribute));
                    if ((attribute2 != null) && !manager.HasPrivilege((int) attribute2.Privilege))
                    {
                        this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + attribute2.Privilege.ToString()));
                    }
                }
            }
        }

        protected virtual void CloseWindow()
        {
            string str = "var win = art.dialog.open.origin; win.location.reload();";
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
            }
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return base.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                string str = queryStrings[str2].Trim().Replace("'", "");
                if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                {
                    builder.Append(str2).Append("=").Append(base.Server.UrlEncode(str)).Append("&");
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void GotoResourceNotFound()
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("ResourceNotFound.aspx"));
        }

        protected override void OnInit(EventArgs e)
        {
            this.RegisterFrameScript();
            this.CheckPageAccess();
            base.OnInit(e);
        }

        protected virtual void RegisterFrameScript()
        {
            string key = "admin-frame";
            string script = string.Format("<script>if(window.parent.frames.length == 0) window.location.href=\"{0}\";</script>", Globals.ApplicationPath + "/admin/default.html");
            ClientScriptManager clientScript = this.Page.ClientScript;
            if (!clientScript.IsClientScriptBlockRegistered(key))
            {
                clientScript.RegisterClientScriptBlock(base.GetType(), key, script);
            }
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        protected virtual void ShowMsg(ValidationResults validateResults)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult result in (IEnumerable<ValidationResult>) validateResults)
            {
                builder.Append(Formatter.FormatErrorMessage(result.Message));
            }
            this.ShowMsg(builder.ToString(), false);
        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }
    }
}

