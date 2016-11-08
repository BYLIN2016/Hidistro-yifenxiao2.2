namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ApprovedDropDownList : DropDownList
    {
        private bool allowNull = true;

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem("全部", string.Empty));
            }
            this.Items.Add(new ListItem("通过", "True"));
            this.Items.Add(new ListItem("禁止", "False"));
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

        public bool? SelectedValue
        {
            get
            {
                bool result = true;
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                bool.TryParse(base.SelectedValue, out result);
                return new bool?(result);
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedValue = value.Value.ToString();
                }
            }
        }
    }
}

