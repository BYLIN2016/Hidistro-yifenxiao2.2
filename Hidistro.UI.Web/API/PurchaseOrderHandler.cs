namespace Hidistro.UI.Web.API
{
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.Subsites.Commodities;
    using Hidistro.Subsites.Sales;
    using System;
    using System.Data;
    using System.Web;

    public class PurchaseOrderHandler : IHttpHandler
    {
        private void ProcessPurchaseOrderAdd(HttpContext context)
        {
            PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
            decimal totalWeight = 0M;
            if (string.IsNullOrEmpty(context.Request["Products"]))
            {
                context.Response.Write("{\"PurchaseOrderAddResponse\":\"-1\"}");
            }
            else
            {
                int distributorId = int.Parse(context.Request["distributorUserId"]);
                foreach (string str in context.Request["Products"].Split(new char[] { ';' }))
                {
                    string[] strArray2 = str.Split(new char[] { ',' });
                    DataTable table = SubSiteProducthelper.GetSkuContent(long.Parse(strArray2[0]), strArray2[1], distributorId);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                        foreach (DataRow row in table.Rows)
                        {
                            if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                            {
                                object sKUContent = item.SKUContent;
                                item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                            }
                        }
                        item.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
                        item.SkuId = (string) table.Rows[0]["SkuId"];
                        item.ProductId = (int) table.Rows[0]["ProductId"];
                        if (table.Rows[0]["SKU"] != DBNull.Value)
                        {
                            item.SKU = (string) table.Rows[0]["SKU"];
                        }
                        if (table.Rows[0]["Weight"] != DBNull.Value)
                        {
                            item.ItemWeight = (decimal) table.Rows[0]["Weight"];
                        }
                        item.ItemPurchasePrice = (decimal) table.Rows[0]["PurchasePrice"];
                        item.Quantity = int.Parse(strArray2[2]);
                        item.ItemListPrice = (decimal) table.Rows[0]["SalePrice"];
                        if (table.Rows[0]["CostPrice"] != DBNull.Value)
                        {
                            item.ItemCostPrice = (decimal) table.Rows[0]["CostPrice"];
                        }
                        item.ItemDescription = (string) table.Rows[0]["ProductName"];
                        item.ItemHomeSiteDescription = (string) table.Rows[0]["ProductName"];
                        if (table.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                        {
                            item.ThumbnailsUrl = (string) table.Rows[0]["ThumbnailUrl40"];
                        }
                        totalWeight += item.ItemWeight * item.Quantity;
                        purchaseOrderInfo.PurchaseOrderItems.Add(item);
                    }
                }
                if (purchaseOrderInfo.PurchaseOrderItems.Count <= 0)
                {
                    context.Response.Write("{\"PurchaseOrderAddResponse\":\"-3\"}");
                }
                else
                {
                    purchaseOrderInfo.Weight = totalWeight;
                    purchaseOrderInfo.TaobaoOrderId = context.Request["TaobaoOrderId"];
                    purchaseOrderInfo.PurchaseOrderId = "MPO" + purchaseOrderInfo.TaobaoOrderId;
                    purchaseOrderInfo.ShipTo = context.Request["ShipTo"];
                    string province = context.Request["ReceiverState"];
                    string city = context.Request["ReceiverCity"];
                    string county = context.Request["ReceiverDistrict"];
                    purchaseOrderInfo.ShippingRegion = province + city + county;
                    purchaseOrderInfo.RegionId = RegionHelper.GetRegionId(county, city, province);
                    purchaseOrderInfo.Address = context.Request["Address"];
                    purchaseOrderInfo.TelPhone = context.Request["TelPhone"];
                    purchaseOrderInfo.CellPhone = context.Request["CellPhone"];
                    purchaseOrderInfo.ZipCode = context.Request["ZipCode"];
                    purchaseOrderInfo.OrderTotal = decimal.Parse(context.Request["OrderTotal"]);
                    ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(HiContext.Current.SiteSettings.TaobaoShippingType, true);
                    if (shippingMode != null)
                    {
                        purchaseOrderInfo.ModeName = shippingMode.Name;
                        purchaseOrderInfo.AdjustedFreight = purchaseOrderInfo.Freight = ShoppingProcessor.CalcFreight(purchaseOrderInfo.RegionId, totalWeight, shippingMode);
                    }
                    purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
                    purchaseOrderInfo.RefundStatus = RefundStatus.None;
                    Distributor user = Users.GetUser(distributorId) as Distributor;
                    if (user != null)
                    {
                        purchaseOrderInfo.DistributorId = user.UserId;
                        purchaseOrderInfo.Distributorname = user.Username;
                        purchaseOrderInfo.DistributorEmail = user.Email;
                        purchaseOrderInfo.DistributorRealName = user.RealName;
                        purchaseOrderInfo.DistributorQQ = user.QQ;
                        purchaseOrderInfo.DistributorWangwang = user.Wangwang;
                        purchaseOrderInfo.DistributorMSN = user.MSN;
                    }
                    if (!SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
                    {
                        context.Response.Write("{\"PurchaseOrderAddResponse\":\"0\"}");
                    }
                    else
                    {
                        context.Response.Write("{\"PurchaseOrderAddResponse\":\"1\"}");
                    }
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str2;
            context.Response.ContentType = "application/json";
            GzipExtention.Gzip(context);
            string str = context.Request["action"];
            if (((str2 = str) != null) && (str2 == "PurchaseOrderAdd"))
            {
                this.ProcessPurchaseOrderAdd(context);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

