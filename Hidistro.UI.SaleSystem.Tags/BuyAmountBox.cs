namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ToolboxData("<{0}:BuyAmountBox runat=\"server\"></{0}:BuyAmountBox>"), ParseChildren(false)]
    public class BuyAmountBox : WebControl
    {
        private string boxStyle;

        public BuyAmountBox() : base(HtmlTextWriterTag.Span)
        {
            this.boxStyle = "width:30px;";
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            WebControl child = new WebControl(HtmlTextWriterTag.Input);
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                child.Attributes.Add("class", base.CssClass);
            }
            if (!string.IsNullOrEmpty(this.BoxStyle))
            {
                child.Attributes.Add("style", this.BoxStyle);
            }
            child.Attributes.Add("id", "buyAmount");
            child.Attributes.Add("type", "text");
            child.Attributes.Add("value", this.Quantity.ToString(CultureInfo.InvariantCulture));
            WebControl control2 = new WebControl(HtmlTextWriterTag.Input);
            control2.Attributes.Add("id", "oldBuyNumHidden");
            control2.Attributes.Add("type", "hidden");
            control2.Attributes.Add("value", this.Quantity.ToString(CultureInfo.InvariantCulture));
            this.Controls.Add(child);
            this.Controls.Add(control2);
        }

        public string BoxStyle
        {
            get
            {
                return this.boxStyle;
            }
            set
            {
                this.boxStyle = value;
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

        public int? MaxQuantity
        {
            get
            {
                if (this.ViewState["MaxQuantity"] == null)
                {
                    return null;
                }
                return new int?((int) this.ViewState["MaxQuantity"]);
            }
            set
            {
                if (value.HasValue)
                {
                    this.ViewState["MaxQuantity"] = value.Value;
                }
                else
                {
                    this.ViewState["MaxQuantity"] = null;
                }
            }
        }

        public int MinQuantity
        {
            get
            {
                if (this.ViewState["MinQuantity"] == null)
                {
                    return 1;
                }
                return (int) this.ViewState["MinQuantity"];
            }
            set
            {
                this.ViewState["MinQuantity"] = value;
            }
        }

        public int Quantity
        {
            get
            {
                if (this.ViewState["Quantity"] == null)
                {
                    return 1;
                }
                return (int) this.ViewState["Quantity"];
            }
            set
            {
                this.ViewState["Quantity"] = value;
            }
        }
    }
}

