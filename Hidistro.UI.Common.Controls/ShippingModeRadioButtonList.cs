namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ShippingModeRadioButtonList : RadioButtonList
    {
        public override void DataBind()
        {
            this.Items.Clear();
            foreach (ShippingModeInfo info in ControlProvider.Instance().GetShippingModes())
            {
                string name = info.Name;
                this.Items.Add(new ListItem(name, info.ModeId.ToString(CultureInfo.InvariantCulture)));
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
                return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
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

