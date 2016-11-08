namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class YearDropDownList : DropDownList
    {
        public YearDropDownList()
        {
            int year = DateTime.Now.Year;
            int num2 = year - 10;
            for (int i = num2; i <= year; i++)
            {
                this.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            this.SelectedValue = year;
        }

        public int SelectedValue
        {
            get
            {
                int num;
                if (int.TryParse(base.SelectedValue, out num))
                {
                    return num;
                }
                return DateTime.Now.Year;
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

