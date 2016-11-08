namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(true)]
    public class HeadContainer : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            HiContext current = HiContext.Current;
            writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var applicationPath = \"{0}\";\r\n            var skinPath = \"{1}\";\r\n            var subsiteuserId = \"{2}\";\r\n        </script>", Globals.ApplicationPath, current.GetSkinPath(), current.SiteSettings.UserId.HasValue ? current.SiteSettings.UserId.Value.ToString() : "0");
            writer.WriteLine();
            this.RenderMetaCharset(writer);
            this.RenderMetaLanguage(writer);
            this.RenderFavicon(writer);
            this.RenderMetaAuthor(writer);
            this.RenderMetaGenerator(writer);
        }

        private void RenderFavicon(HtmlTextWriter writer)
        {
            string str = Globals.FullPath(Globals.GetSiteUrls().Favicon);
            writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
            writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
        }

        private void RenderMetaAuthor(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta name=\"author\" content=\"Hishop development team\" />");
        }

        private void RenderMetaCharset(HtmlTextWriter writer)
        {
            switch (HiContext.Current.Config.AppLocation.CurrentApplicationType)
            {
                case ApplicationType.Admin:
                case ApplicationType.Installer:
                    writer.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />");
                    break;
            }
        }

        private void RenderMetaGenerator(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta name=\"GENERATOR\" content=\"易分销 2.2 \" />");
        }

        private void RenderMetaLanguage(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
        }
    }
}

