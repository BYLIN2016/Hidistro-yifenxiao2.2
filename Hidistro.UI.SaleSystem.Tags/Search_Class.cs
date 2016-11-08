namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Search_Class : DropDownList
    {
        private string nullToDisplay = "商品分类";
        public const string TagID = "drop_Search_Class";
        private string textStyle;

        public Search_Class()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            foreach (CategoryInfo info in CategoryBrowser.GetMaxMainCategories(0x3e8))
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.ToString(CultureInfo.InvariantCulture)));
            }
            base.ID = "drop_Search_Class";
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "drop_Search_Class");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.TextStyle))
            {
                this.CssClass = this.TextStyle;
            }
            base.Render(writer);
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }

        public string TextStyle
        {
            get
            {
                return this.textStyle;
            }
            set
            {
                this.textStyle = value;
            }
        }
    }
}

