namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SkuLabel : WebControl
    {
        public SkuLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "productDetails_sku");
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Controls.Add(new LiteralControl(this.Text));
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.AddAttribute("id", "productDetails_sku_v");
            writer.AddAttribute("name", "productDetails_sku_v");
            writer.AddAttribute("type", "hidden");
            writer.AddAttribute("value", this.Value);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
            }
        }

        public string Text
        {
            get
            {
                if (this.ViewState["SkuText"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["SkuText"];
            }
            set
            {
                this.ViewState["SkuText"] = value;
            }
        }

        public string Value
        {
            get
            {
                if (this.ViewState["SkuValue"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["SkuValue"];
            }
            set
            {
                this.ViewState["SkuValue"] = value;
            }
        }
    }
}

