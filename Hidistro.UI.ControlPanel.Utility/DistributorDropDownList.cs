namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DistributorDropDownList : DropDownList
    {
        private string nullToDisplay = "--请选择分销商--";

        public override void DataBind()
        {
            this.Items.Clear();
            base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            foreach (Distributor distributor in DistributorHelper.GetDistributors())
            {
                base.Items.Add(new ListItem(distributor.Username, distributor.UserId.ToString()));
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

