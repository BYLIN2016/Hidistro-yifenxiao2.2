namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class OrderRemarkImageRadioButtonList : RadioButtonList
    {
        public OrderRemarkImageRadioButtonList()
        {
            this.Items.Clear();
            int num = 1;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconaf.gif\"></img>", num.ToString()));
            int num2 = 2;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconb.gif\"></img>", num2.ToString()));
            int num3 = 3;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconc.gif\"></img>", num3.ToString()));
            int num4 = 4;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/icona.gif\"></img>", num4.ToString()));
            int num5 = 5;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconad.gif\"></img>", num5.ToString()));
            int num6 = 6;
            this.Items.Add(new ListItem("<img src=\"" + Globals.ApplicationPath + "/Admin/images/iconae.gif\"></img>", num6.ToString()));
            this.RepeatDirection = RepeatDirection.Horizontal;
        }

        public OrderMark? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new OrderMark?((OrderMark) Enum.Parse(typeof(OrderMark), base.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(((int) value.Value).ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

