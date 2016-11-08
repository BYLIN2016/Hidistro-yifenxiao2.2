namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Shopping;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web;

    public class ShoppingHandler : IHttpHandler
    {
        private void ClearBrowsedProduct(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            BrowsedProductQueue.ClearQueue();
            context.Response.Write("{\"Status\":\"Succes\"}");
        }

        private void ProcessAddToCartBySkus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            ShoppingCartProcessor.AddLineItem(skuId, quantity);
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.GetQuantity().ToString() + "\"}");
        }

        private void ProcessGetSkuByOptions(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, str);
                if (productAndSku == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight.ToString("F2"));
                    builder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
                    builder.AppendFormat("\"AlertStock\":\"{0}\",", productAndSku.AlertStock);
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string str = context.Request["action"];
                string str2 = str;
                if (str2 != null)
                {
                    if (!(str2 == "AddToCartBySkus"))
                    {
                        if (str2 == "GetSkuByOptions")
                        {
                            goto Label_0055;
                        }
                        if (str2 == "UnUpsellingSku")
                        {
                            goto Label_005E;
                        }
                        if (str2 == "ClearBrowsed")
                        {
                            goto Label_0067;
                        }
                    }
                    else
                    {
                        this.ProcessAddToCartBySkus(context);
                    }
                }
                return;
            Label_0055:
                this.ProcessGetSkuByOptions(context);
                return;
            Label_005E:
                this.ProcessUnUpsellingSku(context);
                return;
            Label_0067:
                this.ClearBrowsedProduct(context);
            }
            catch (Exception exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"Status\":\"" + exception.Message.Replace("\"", "'") + "\"}");
            }
        }

        private void ProcessUnUpsellingSku(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            int attributeId = int.Parse(context.Request["AttributeId"], NumberStyles.None);
            int valueId = int.Parse(context.Request["ValueId"], NumberStyles.None);
            DataTable table = ShoppingProcessor.GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
            if ((table == null) || (table.Rows.Count == 0))
            {
                context.Response.Write("{\"Status\":\"1\"}");
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                builder.Append("\"SkuItems\":[");
                foreach (DataRow row in table.Rows)
                {
                    builder.Append("{");
                    builder.AppendFormat("\"AttributeId\":\"{0}\",", row["AttributeId"].ToString());
                    builder.AppendFormat("\"ValueId\":\"{0}\"", row["ValueId"].ToString());
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
                builder.Append("]");
                builder.Append("}");
                context.Response.Write(builder.ToString());
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

