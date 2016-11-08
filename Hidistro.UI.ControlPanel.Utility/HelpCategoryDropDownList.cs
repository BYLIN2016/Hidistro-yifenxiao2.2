namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class HelpCategoryDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            foreach (HelpCategoryInfo info in ArticleHelper.GetHelpCategorys())
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.Value.ToString()));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
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
    }
}

