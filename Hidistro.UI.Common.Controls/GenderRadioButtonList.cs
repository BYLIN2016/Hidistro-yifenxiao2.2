namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class GenderRadioButtonList : RadioButtonList
    {
        public GenderRadioButtonList()
        {
            int num = 0;
            this.Items.Add(new ListItem("保密", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 1;
            this.Items.Add(new ListItem("男性", num2.ToString(CultureInfo.InvariantCulture)));
            int num3 = 2;
            this.Items.Add(new ListItem("女性", num3.ToString(CultureInfo.InvariantCulture)));
        }

        public Gender SelectedValue
        {
            get
            {
                return (Gender) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

