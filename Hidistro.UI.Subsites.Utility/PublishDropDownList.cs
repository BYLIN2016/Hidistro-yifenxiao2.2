namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class PublishDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "-请选择-";

        public PublishDropDownList()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            this.Items.Add(new ListItem("已发布", "1"));
            this.Items.Add(new ListItem("未发布", "2"));
            this.Items.Add(new ListItem("有更新", "3"));
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

        public PublishStatus SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return PublishStatus.NotSet;
                }
                return (PublishStatus) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

