namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Text;

    public class GetPurchaseOrderTaobaoInfo : DistributorPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tbOrderId = base.Request.Form["order_id"];
            if (tbOrderId == null)
            {
                tbOrderId = "";
            }
            PurchaseOrderTaobaoInfo purchaseOrderTaobaoInfo = SubsiteSalesHelper.GetPurchaseOrderTaobaoInfo(tbOrderId);
            base.Response.ContentType = "text/plain";
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"info\":");
            builder.Append(purchaseOrderTaobaoInfo.ToJson());
            builder.Append(",\"msg\":\"\",\"result\":\"success\"");
            builder.Append("}");
            base.Response.Write(builder.ToString());
        }
    }
}

