namespace Hidistro.UI.Subsites.Utility
{
    using Hidistro.Subsites.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ConvertStatusLabel : Literal
    {
        private bool isExit;
        private long tradeId;

        protected override void OnDataBinding(EventArgs e)
        {
            object obj2 = DataBinder.Eval(this.Page.GetDataItem(), "tid");
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                this.tradeId = (long) obj2;
                this.isExit = SubsiteSalesHelper.IsExitPurchaseOrder(this.tradeId);
            }
            base.OnDataBinding(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.isExit)
            {
                writer.Write("<label style=\"color:Red;\">此订单已经生成过采购单</label>");
            }
            else
            {
                writer.Write(string.Format("<input name=\"CheckBoxGroup\" type=\"checkbox\" value='{0}'>", this.tradeId));
            }
        }
    }
}

