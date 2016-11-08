namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.Web.UI;

    public class DisplayThemesImages : Control
    {
        private string imageFormat = "<a><img border=\"0\" src=\"{0}\" /></a>";
        private bool isDistributorThemes;

        protected string GetImagePath()
        {
            if (this.IsDistributorThemes)
            {
                return string.Concat(new object[] { Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", this.ThemeName });
            }
            return (Globals.ApplicationPath + "/Templates/library/" + this.ThemeName);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Src.StartsWith("~"))
            {
                this.Src = base.ResolveUrl(this.Src);
            }
            else if (this.Src.StartsWith("/"))
            {
                this.Src = this.GetImagePath() + this.Src;
            }
            else
            {
                this.Src = this.GetImagePath() + "/" + this.Src;
            }
            writer.Write(string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[] { this.Src }));
        }

        public bool IsDistributorThemes
        {
            get
            {
                return this.isDistributorThemes;
            }
            set
            {
                this.isDistributorThemes = value;
            }
        }

        public string Src
        {
            get
            {
                if (this.ViewState["Src"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["Src"];
            }
            set
            {
                this.ViewState["Src"] = value;
            }
        }

        public string ThemeName
        {
            get
            {
                if (this.ViewState["ThemeName"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["ThemeName"];
            }
            set
            {
                this.ViewState["ThemeName"] = value;
            }
        }
    }
}

