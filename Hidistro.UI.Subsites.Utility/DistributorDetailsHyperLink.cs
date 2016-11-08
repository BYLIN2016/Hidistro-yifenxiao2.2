namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class DistributorDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.DetailsId != null) && (this.DetailsId != DBNull.Value))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                base.NavigateUrl = string.Concat(new object[] { "http://", siteSettings.SiteUrl, Globals.ApplicationPath, this.DetailsPageUrl, this.DetailsId });
            }
            if ((this.DetailsId != null) && (this.DetailsId != DBNull.Value))
            {
                base.Text = this.Title.ToString();
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public object DetailsId
        {
            get
            {
                if (this.ViewState["DetailsId"] != null)
                {
                    return this.ViewState["DetailsId"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["DetailsId"] = value;
                }
            }
        }

        public string DetailsPageUrl
        {
            get
            {
                if (this.ViewState["DetailsPageUrl"] != null)
                {
                    return this.ViewState["DetailsPageUrl"].ToString();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["DetailsPageUrl"] = value;
                }
            }
        }

        public object Title
        {
            get
            {
                if (this.ViewState["Title"] != null)
                {
                    return this.ViewState["Title"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.ViewState["Title"] = value;
                }
            }
        }
    }
}

