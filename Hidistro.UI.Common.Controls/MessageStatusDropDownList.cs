namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Comments;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MessageStatusDropDownList : DropDownList
    {
        public MessageStatusDropDownList()
        {
            this.Items.Clear();
            int num = 1;
            this.Items.Add(new ListItem("全部", num.ToString()));
            int num2 = 2;
            this.Items.Add(new ListItem("已回复", num2.ToString()));
            int num3 = 3;
            this.Items.Add(new ListItem("未回复", num3.ToString()));
        }

        public MessageStatus SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return MessageStatus.All;
                }
                return (MessageStatus) int.Parse(base.SelectedValue);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

