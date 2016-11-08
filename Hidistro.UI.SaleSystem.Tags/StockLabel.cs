namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class StockLabel : WebControl
    {
        public StockLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "productDetails_Stock");
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(this.Stock.ToString()));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.AddAttribute("id", "productDetails_AlertStock");
            writer.AddAttribute("name", "productDetails_AlertStock");
            writer.AddAttribute("type", "hidden");
            writer.AddAttribute("value", this.AlertStock.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public int AlertStock
        {
            get
            {
                if (this.ViewState["AlertStock"] == null)
                {
                    return 0;
                }
                return (int) this.ViewState["AlertStock"];
            }
            set
            {
                this.ViewState["AlertStock"] = value;
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
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

