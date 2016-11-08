namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:AddCartButton runat=\"server\"></{0}:AddCartButton>"), ParseChildren(false)]
    public class AddCartButton : WebControl
    {
        public AddCartButton() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "addcartButton");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this.Stock > 0) && (HiContext.Current.SiteSettings.IsOpenSiteSale || HiContext.Current.SiteSettings.IsDistributorSettings))
            {
                base.Render(writer);
            }
        }

        public int Stock
        {
            get
            {
                if (this.ViewState["Stock"] == null)
                {
                    return 0;
                }
                return (int) this.ViewState["Stock"];
            }
            set
            {
                this.ViewState["Stock"] = value;
            }
        }
    }
}

