namespace Hidistro.UI.Subsites.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class BankDropDownList : DropDownList
    {
        public BankDropDownList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("请选择", "请选择"));
            this.Items.Add(new ListItem("中国工商银行", "中国工商银行"));
            this.Items.Add(new ListItem("中国建设银行", "中国建设银行"));
            this.Items.Add(new ListItem("中国农业银行", "中国农业银行"));
        }
    }
}

