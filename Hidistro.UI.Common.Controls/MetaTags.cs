namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web;
    using System.Web.UI;

    [ParseChildren(false), PersistChildren(true)]
    public class MetaTags : Control
    {
        private const string metaDescriptionKey = "Hishop.Meta_Description";
        private const string metaFormat = "<meta name=\"{0}\" content=\"{1}\" />";
        private const string metaKeywordsKey = "Hishop.Meta_Keywords";

        public static void AddMetaDescription(string value, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Items["Hishop.Meta_Description"] = value;
        }

        public static void AddMetaKeywords(string value, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Items["Hishop.Meta_Keywords"] = value;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderMetaDescription(writer);
            this.RenderMetaKeywords(writer);
        }

        protected virtual void RenderMetaDescription(HtmlTextWriter writer)
        {
            if (this.Context.Items.Contains("Hishop.Meta_Description"))
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", this.Context.Items["Hishop.Meta_Description"]);
            }
            else
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", HiContext.Current.SiteSettings.SearchMetaDescription);
            }
        }

        protected virtual void RenderMetaKeywords(HtmlTextWriter writer)
        {
            if (this.Context.Items.Contains("Hishop.Meta_Keywords"))
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", this.Context.Items["Hishop.Meta_Keywords"]);
            }
            else
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", HiContext.Current.SiteSettings.SearchMetaKeywords);
            }
        }
    }
}

