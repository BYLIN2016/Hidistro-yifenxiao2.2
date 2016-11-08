namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.Core;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DistributorPriceDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            base.Items.Add(new ListItem("成本价", "-2"));
            base.Items.Add(new ListItem("采购价", "-4"));
            foreach (DataRow row in DistributorHelper.GetDistributorGrades().Rows)
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(row["Name"].ToString() + "采购价"), row["GradeId"].ToString()));
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
                return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
    }
}

