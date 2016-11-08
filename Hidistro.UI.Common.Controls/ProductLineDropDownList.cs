namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ProductLineDropDownList : DropDownList
    {
        
        private bool _IsShowNoset;
        private bool allowNull = true;
        private string nullToDisplay = "全部";

        public override void DataBind()
        {
            base.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            if (this.IsShowNoset)
            {
                base.Items.Add(new ListItem("未设置产品线", "0"));
            }
            foreach (ProductLineInfo info in ControlProvider.Instance().GetProductLineList())
            {
                base.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.LineId.ToString()));
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

        public bool IsShowNoset
        {
            
            get
            {
                return _IsShowNoset;
            }
            
            set
            {
                _IsShowNoset = value;
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
                if (!value.HasValue)
                {
                    base.SelectedValue = string.Empty;
                }
                else
                {
                    base.SelectedValue = value.ToString();
                }
            }
        }
    }
}

