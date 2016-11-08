namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderStatusLabel : Label
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Text = OrderInfo.GetOrderStatusName((OrderStatus) this.OrderStatusCode);
            base.Render(writer);
        }

        public object OrderStatusCode
        {
            get
            {
                return this.ViewState["OrderStatusCode"];
            }
            set
            {
                this.ViewState["OrderStatusCode"] = value;
            }
        }
    }
}

