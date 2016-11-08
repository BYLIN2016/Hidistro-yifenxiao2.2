namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities.Sales;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class BatchPrintSendPurchaseOrderGoods : Page
    {
        protected HtmlGenericControl divContent;
        protected HtmlHead Head1;

        private List<PurchaseOrderInfo> GetPrintData(string orderIds)
        {
            List<PurchaseOrderInfo> list = new List<PurchaseOrderInfo>();
            foreach (string str in orderIds.Split(new char[] { ',' }))
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(str);
                list.Add(purchaseOrder);
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["purchaseorderIds"]))
            {
                string orderIds = base.Request["purchaseorderIds"].Trim(new char[] { ',' });
                foreach (PurchaseOrderInfo info in this.GetPrintData(orderIds))
                {
                    HtmlGenericControl child = new HtmlGenericControl("div");
                    child.Attributes["class"] = "order print";
                    StringBuilder builder = new StringBuilder("");
                    builder.AppendFormat("<div class=\"info\"><div class=\"prime-info\" style=\"margin-right: 20px;\"><p><span><h3 style=\"font-weight: normal\">{0}</h3></span></p></div><ul class=\"sub-info\"><li><span>生成时间： </span>{1}</li><li><span>采购单编号： </span>{2}</li></ul><br class=\"clear\" /></div>", info.ShipTo, info.PurchaseDate.ToString("yyyy-MM-dd HH:mm"), info.PurchaseOrderId);
                    builder.Append("<table><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>货号</th><th>商品名称</th><th>规格</th><th>数量</th><th>单价</th><th>总价</th></tr></thead><tbody>");
                    IList<PurchaseOrderItemInfo> purchaseOrderItems = info.PurchaseOrderItems;
                    if (purchaseOrderItems != null)
                    {
                        foreach (PurchaseOrderItemInfo info2 in purchaseOrderItems)
                        {
                            builder.AppendFormat("<tr><td>{0}</td>", info2.SKU);
                            builder.AppendFormat("<td>{0}</td>", info2.ItemDescription);
                            builder.AppendFormat("<td>{0}</td>", info2.SKUContent);
                            builder.AppendFormat("<td>{0}</td>", info2.Quantity);
                            builder.AppendFormat("<td>{0}</td>", Math.Round(info2.ItemListPrice, 2));
                            builder.AppendFormat("<td>{0}</td></tr>", Math.Round(info2.GetSubTotal(), 2));
                        }
                    }
                    string str2 = "";
                    IList<PurchaseOrderGiftInfo> purchaseOrderGifts = info.PurchaseOrderGifts;
                    if ((purchaseOrderGifts != null) && (purchaseOrderGifts.Count > 0))
                    {
                        PurchaseOrderGiftInfo info3 = purchaseOrderGifts[0];
                        str2 = string.Format("<li><span>赠送礼品：</span>{0},数量：{1}</li>", info3.GiftName, info3.Quantity);
                    }
                    builder.AppendFormat("</tbody></table><ul class=\"price\"><li><span>商品总价： </span>{0}</li><li><span>运费： </span>{1}</li>", Math.Round(info.GetProductAmount(), 2), Math.Round(info.AdjustedFreight, 2));
                    decimal adjustedDiscount = info.AdjustedDiscount;
                    if (adjustedDiscount > 0M)
                    {
                        builder.AppendFormat("<li><span>管理员手工打折：</span>{0}</li>", Math.Round(adjustedDiscount, 2));
                    }
                    builder.Append(str2);
                    builder.AppendFormat("<li><span>实付金额：</span>{0}</li></ul><br class=\"clear\" /><br><br>", Math.Round(info.GetPurchaseTotal(), 2));
                    child.InnerHtml = builder.ToString();
                    this.divContent.Controls.AddAt(0, child);
                }
            }
        }
    }
}

