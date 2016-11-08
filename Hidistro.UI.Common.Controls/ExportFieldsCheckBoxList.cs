namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFieldsCheckBoxList : CheckBoxList
    {
        private int repeatColumns = 7;
        private System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFieldsCheckBoxList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("用户名", "UserName"));
            this.Items.Add(new ListItem("真实姓名", "RealName"));
            this.Items.Add(new ListItem("邮箱", "Email"));
            this.Items.Add(new ListItem("QQ", "QQ"));
            this.Items.Add(new ListItem("MSN", "MSN"));
            this.Items.Add(new ListItem("旺旺", "Wangwang"));
            this.Items.Add(new ListItem("邮编", "Zipcode"));
            this.Items.Add(new ListItem("手机号", "CellPhone"));
            this.Items.Add(new ListItem("电话", "TelPhone"));
            this.Items.Add(new ListItem("积分", "Points"));
            this.Items.Add(new ListItem("生日", "BirthDate"));
            this.Items.Add(new ListItem("详细地址", "Address"));
            this.Items.Add(new ListItem("消费金额", "Expenditure"));
            this.Items.Add(new ListItem("预付款余额", "Balance"));
        }

        public override int RepeatColumns
        {
            get
            {
                return this.repeatColumns;
            }
            set
            {
                this.repeatColumns = value;
            }
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return this.repeatDirection;
            }
            set
            {
                this.repeatDirection = value;
            }
        }
    }
}

