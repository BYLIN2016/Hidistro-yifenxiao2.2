namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class RankPriceName : Literal
    {
        private string priceName = string.Empty;

        protected override void Render(HtmlTextWriter writer)
        {
            SiteSettings siteSettings = HiContext.Current.SiteSettings;
            if (!string.IsNullOrEmpty(siteSettings.YourPriceName))
            {
                base.Text = siteSettings.YourPriceName;
            }
            else
            {
                base.Text = this.PriceName;
            }
            base.Render(writer);
        }

        public string PriceName
        {
            get
            {
                return this.priceName;
            }
            set
            {
                this.priceName = value;
            }
        }
    }
}

