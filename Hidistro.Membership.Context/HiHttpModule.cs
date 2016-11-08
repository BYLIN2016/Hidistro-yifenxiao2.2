namespace Hidistro.Membership.Context
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Enums;
    using Hidistro.Core.Jobs;
    using Hidistro.Core.Urls;
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public class HiHttpModule : IHttpModule
    {
        private bool applicationInstalled;
        private ApplicationType currentApplicationType;
        private static readonly Regex urlReg = new Regex("(loginentry.aspx|login.aspx|logout.aspx|resourcenotfound.aspx|verifycodeimage.aspx|SendPayment.aspx|PaymentReturn_url|PaymentNotify_url|InpourReturn_url|InpourNotify_url)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private void Application_AuthorizeRequest(object source, EventArgs e)
        {
            if (this.currentApplicationType != ApplicationType.Installer)
            {
                HttpApplication application = (HttpApplication) source;
                HttpContext context = application.Context;
                HiContext current = HiContext.Current;
                if (context.Request.IsAuthenticated)
                {
                    string name = context.User.Identity.Name;
                    if (name != null)
                    {
                        string[] rolesForUser = Roles.GetRolesForUser(name);
                        if ((rolesForUser != null) && (rolesForUser.Length > 0))
                        {
                            current.RolesCacheKey = string.Join(",", rolesForUser);
                        }
                    }
                }
            }
        }

        private void Application_BeginRequest(object source, EventArgs e)
        {
            this.currentApplicationType = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;
            HttpApplication application = (HttpApplication) source;
            HttpContext context = application.Context;
            if (context.Request.RawUrl.IndexOfAny(new char[] { '<', '>', '\'', '"' }) != -1)
            {
                context.Response.Redirect(context.Request.RawUrl.Replace("<", "%3c").Replace(">", "%3e").Replace("'", "%27").Replace("\"", "%22"), false);
            }
            else
            {
                this.CheckInstall(context);
                if (this.currentApplicationType != ApplicationType.Installer)
                {
                    CheckSSL(HiConfiguration.GetConfig().SSL, context);
                    HiContext.Create(context, new UrlReWriterDelegate(HiHttpModule.ReWriteUrl));
                    if (HiContext.Current.SiteSettings.IsDistributorSettings && !((!HiContext.Current.SiteSettings.Disabled || (this.currentApplicationType != ApplicationType.Common)) || urlReg.IsMatch(context.Request.Url.AbsolutePath)))
                    {
                        context.Response.Write("站点维护中，暂停访问！");
                        context.Response.End();
                    }
                }
            }
        }

        private void CheckInstall(HttpContext context)
        {
            if ((this.currentApplicationType == ApplicationType.Installer) && this.applicationInstalled)
            {
                context.Response.Redirect(Globals.GetSiteUrls().Home, false);
            }
            else if (!(this.applicationInstalled || (this.currentApplicationType == ApplicationType.Installer)))
            {
                context.Response.Redirect(Globals.ApplicationPath + "/installer/default.aspx", false);
            }
        }

        private static void CheckSSL(SSLSettings ssl, HttpContext context)
        {
            if (ssl == SSLSettings.All)
            {
                Globals.RedirectToSSL(context);
            }
        }

        public void Dispose()
        {
            if (this.currentApplicationType != ApplicationType.Installer)
            {
                Hidistro.Core.Jobs.Jobs.Instance().Stop();
            }
        }

        public void Init(HttpApplication application)
        {
            if (null != application)
            {
                application.BeginRequest += new EventHandler(this.Application_BeginRequest);
                application.AuthorizeRequest += new EventHandler(this.Application_AuthorizeRequest);
                this.applicationInstalled = ConfigurationManager.AppSettings["Installer"] == null;
                this.currentApplicationType = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;
                this.CheckInstall(application.Context);
                if (this.currentApplicationType != ApplicationType.Installer)
                {
                    Hidistro.Core.Jobs.Jobs.Instance().Start();
                    ExtensionContainer.LoadExtensions();
                }
            }
        }

        private static bool ReWriteUrl(HttpContext context)
        {
            string path = context.Request.Path;
            string filePath = UrlReWriteProvider.Instance().RewriteUrl(path, context.Request.Url.Query);
            if (filePath != null)
            {
                string queryString = null;
                int index = filePath.IndexOf('?');
                if (index >= 0)
                {
                    queryString = (index < (filePath.Length - 1)) ? filePath.Substring(index + 1) : string.Empty;
                    filePath = filePath.Substring(0, index);
                }
                context.RewritePath(filePath, null, queryString);
            }
            return (filePath != null);
        }

        public string ModuleName
        {
            get
            {
                return "HiHttpModule";
            }
        }
    }
}

