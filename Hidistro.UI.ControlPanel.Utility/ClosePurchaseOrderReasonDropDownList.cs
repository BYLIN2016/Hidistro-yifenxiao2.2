namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class ClosePurchaseOrderReasonDropDownList : DropDownList
    {
        public ClosePurchaseOrderReasonDropDownList()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("请选择退回的理由", "请选择退回的理由"));
            this.Items.Add(new ListItem("联系不到分销商", "联系不到分销商"));
            this.Items.Add(new ListItem("分销商的会员不想买了", "分销商的会员不想买了"));
            this.Items.Add(new ListItem("已同城交易", "已同城交易"));
            this.Items.Add(new ListItem("暂时缺货", "暂时缺货"));
            this.Items.Add(new ListItem("其他原因", "其他原因"));
        }
    }
}

