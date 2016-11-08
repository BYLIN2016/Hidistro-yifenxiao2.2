namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class PenetrationStatusDropDownList : DropDownList
    {
        private bool allowNull = true;

        public PenetrationStatusDropDownList()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(string.Empty, string.Empty));
            }
            this.Items.Add(new ListItem("已铺货", "1"));
            this.Items.Add(new ListItem("未铺货", "2"));
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

        public PenetrationStatus SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return PenetrationStatus.NotSet;
                }
                return (PenetrationStatus) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

