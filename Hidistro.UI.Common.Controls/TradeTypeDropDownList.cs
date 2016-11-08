namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Members;
    using System;
    using System.Web.UI.WebControls;

    public class TradeTypeDropDownList : DropDownList
    {
        private bool allowNull = true;
        private bool isDistributor;
        private string nullToDisplay;

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            this.Items.Add(new ListItem("自助充值", "1"));
            this.Items.Add(new ListItem("后台加款", "2"));
            this.Items.Add(new ListItem("消费", "3"));
            this.Items.Add(new ListItem("提现", "4"));
            if (this.IsDistributor)
            {
                this.Items.Add(new ListItem("采购单退款", "5"));
                this.Items.Add(new ListItem("采购单退货", "7"));
            }
            else
            {
                this.Items.Add(new ListItem("订单退款", "5"));
                this.Items.Add(new ListItem("推荐人提成", "6"));
                this.Items.Add(new ListItem("订单退货", "7"));
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

        public bool IsDistributor
        {
            get
            {
                return this.isDistributor;
            }
            set
            {
                this.isDistributor = value;
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

        public TradeTypes SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return TradeTypes.NotSet;
                }
                return (TradeTypes) int.Parse(base.SelectedValue);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString()));
            }
        }
    }
}

