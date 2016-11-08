namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.HOP;
    using Hidistro.Subsites.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;

    public class TaobaoProductHandler : IHttpHandler
    {
        private void DownloadImage(ProductInfo product, string imageUrls, HttpContext context)
        {
            imageUrls = HttpUtility.UrlDecode(imageUrls);
            string[] strArray = imageUrls.Split(new char[] { ';' });
            int num = 1;
            foreach (string str in strArray)
            {
                string str2 = string.Format("/Storage/master/product/images/{0}", Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + str.Substring(str.LastIndexOf('.')));
                string str3 = str2.Replace("/images/", "/thumbs40/40_");
                string str4 = str2.Replace("/images/", "/thumbs60/60_");
                string str5 = str2.Replace("/images/", "/thumbs100/100_");
                string str6 = str2.Replace("/images/", "/thumbs160/160_");
                string str7 = str2.Replace("/images/", "/thumbs180/180_");
                string str8 = str2.Replace("/images/", "/thumbs220/220_");
                string str9 = str2.Replace("/images/", "/thumbs310/310_");
                string str10 = str2.Replace("/images/", "/thumbs410/410_");
                string fileName = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str2);
                WebClient client = new WebClient();
                try
                {
                    client.DownloadFile(str, fileName);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str3), 40, 40);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str4), 60, 60);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str5), 100, 100);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str6), 160, 160);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str7), 180, 180);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str8), 220, 220);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str9), 310, 310);
                    ResourcesHelper.CreateThumbnail(fileName, context.Request.MapPath(Globals.ApplicationPath + str10), 410, 410);
                    switch (num)
                    {
                        case 1:
                            product.ImageUrl1 = str2;
                            product.ThumbnailUrl40 = str3;
                            product.ThumbnailUrl60 = str4;
                            product.ThumbnailUrl100 = str5;
                            product.ThumbnailUrl160 = str6;
                            product.ThumbnailUrl180 = str7;
                            product.ThumbnailUrl220 = str8;
                            product.ThumbnailUrl310 = str9;
                            product.ThumbnailUrl410 = str10;
                            break;

                        case 2:
                            product.ImageUrl2 = str2;
                            break;

                        case 3:
                            product.ImageUrl3 = str2;
                            break;

                        case 4:
                            product.ImageUrl4 = str2;
                            break;

                        case 5:
                            product.ImageUrl5 = str2;
                            break;
                    }
                    num++;
                }
                catch
                {
                }
            }
        }

        private Dictionary<string, SKUItem> GetSkus(ProductInfo product, int weight, HttpContext context)
        {
            Dictionary<string, SKUItem> dictionary = null;
            string str = context.Request.Form["SkuString"];
            if (string.IsNullOrEmpty(str))
            {
                product.HasSKU = false;
                Dictionary<string, SKUItem> dictionary2 = new Dictionary<string, SKUItem>();
                SKUItem item = new SKUItem();
                item.SkuId = "0";
                item.SKU = product.ProductCode;
                item.SalePrice = decimal.Parse(context.Request.Form["SalePrice"]);
                item.PurchasePrice = decimal.Parse(context.Request.Form["SalePrice"]);
                item.CostPrice = 0M;
                item.Stock = int.Parse(context.Request.Form["Stock"]);
                item.Weight = weight;
                dictionary2.Add("0", item);
                return dictionary2;
            }
            product.HasSKU = true;
            dictionary = new Dictionary<string, SKUItem>();
            foreach (string str2 in HttpUtility.UrlDecode(str).Split(new char[] { '|' }))
            {
                string[] strArray = str2.Split(new char[] { ',' });
                SKUItem item2 = new SKUItem();
                item2.SKU = strArray[0];
                item2.Weight = weight;
                item2.Stock = int.Parse(strArray[1]);
                item2.PurchasePrice = item2.SalePrice = decimal.Parse(strArray[2]);
                string str3 = strArray[3];
                string str4 = "";
                foreach (string str5 in str3.Split(new char[] { ';' }))
                {
                    string[] strArray2 = str5.Split(new char[] { ':' });
                    int specificationId = ProductTypeHelper.GetSpecificationId(product.TypeId.Value, strArray2[0]);
                    int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, strArray2[1]);
                    str4 = str4 + specificationValueId + "_";
                    item2.SkuItems.Add(specificationId, specificationValueId);
                }
                item2.SkuId = str4.Substring(0, str4.Length - 1);
                dictionary.Add(item2.SkuId, item2);
            }
            return dictionary;
        }

        private TaobaoProductInfo GetTaobaoProduct(HttpContext context)
        {
            TaobaoProductInfo info = new TaobaoProductInfo();
            info.Cid = long.Parse(context.Request.Form["Cid"]);
            info.StuffStatus = context.Request.Form["StuffStatus"];
            info.LocationState = context.Request.Form["LocationState"];
            info.LocationCity = context.Request.Form["LocationCity"];
            info.FreightPayer = context.Request.Form["FreightPayer"];
            if (!string.IsNullOrEmpty(context.Request.Form["PostFee"]))
            {
                info.PostFee = decimal.Parse(context.Request.Form["PostFee"]);
            }
            if (!string.IsNullOrEmpty(context.Request.Form["ExpressFee"]))
            {
                info.ExpressFee = decimal.Parse(context.Request.Form["ExpressFee"]);
            }
            if (!string.IsNullOrEmpty(context.Request.Form["EMSFee"]))
            {
                info.EMSFee = decimal.Parse(context.Request.Form["EMSFee"]);
            }
            info.HasInvoice = bool.Parse(context.Request.Form["HasInvoice"]);
            info.HasWarranty = bool.Parse(context.Request.Form["HasWarranty"]);
            info.HasDiscount = false;
            info.ListTime = DateTime.Now;
            info.PropertyAlias = context.Request.Form["PropertyAlias"];
            info.InputPids = context.Request.Form["InputPids"];
            info.InputStr = context.Request.Form["InputStr"];
            info.SkuProperties = context.Request.Form["SkuProperties"];
            info.SkuQuantities = context.Request.Form["SkuQuantities"];
            info.SkuPrices = context.Request.Form["SkuPrices"];
            info.SkuOuterIds = context.Request.Form["SkuOuterIds"];
            return info;
        }

        private string LoadImage(string path)
        {
            byte[] buffer = null;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
            }
            return Convert.ToBase64String(buffer);
        }

        private void ProcessProductDetails(HttpContext context)
        {
            string str = "http://" + HttpContext.Current.Request.Url.Host + ((HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + HttpContext.Current.Request.Url.Port));
            int distributorId = int.Parse(context.Request["distributorUserId"]);
            PublishToTaobaoProductInfo taobaoProduct = SubSiteProducthelper.GetTaobaoProduct(int.Parse(context.Request["productId"]), distributorId);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("\"Cid\":\"{0}\",", taobaoProduct.Cid);
            builder.AppendFormat("\"StuffStatus\":\"{0}\",", taobaoProduct.StuffStatus);
            builder.AppendFormat("\"ProductId\":\"{0}\",", taobaoProduct.ProductId);
            builder.AppendFormat("\"ProTitle\":\"{0}\",", taobaoProduct.ProTitle);
            builder.AppendFormat("\"Num\":\"{0}\",", taobaoProduct.Num);
            builder.AppendFormat("\"LocationState\":\"{0}\",", taobaoProduct.LocationState);
            builder.AppendFormat("\"LocationCity\":\"{0}\",", taobaoProduct.LocationCity);
            builder.AppendFormat("\"FreightPayer\":\"{0}\",", taobaoProduct.FreightPayer);
            builder.AppendFormat("\"PostFee\":\"{0}\",", taobaoProduct.PostFee.ToString("F2"));
            builder.AppendFormat("\"ExpressFee\":\"{0}\",", taobaoProduct.ExpressFee.ToString("F2"));
            builder.AppendFormat("\"EMSFee\":\"{0}\",", taobaoProduct.EMSFee.ToString("F2"));
            builder.AppendFormat("\"HasInvoice\":\"{0}\",", taobaoProduct.HasInvoice);
            builder.AppendFormat("\"HasWarranty\":\"{0}\",", taobaoProduct.HasWarranty);
            builder.AppendFormat("\"HasDiscount\":\"{0}\",", taobaoProduct.HasDiscount);
            builder.AppendFormat("\"ValidThru\":\"{0}\",", taobaoProduct.ValidThru);
            builder.AppendFormat("\"ListTime\":\"{0}\",", taobaoProduct.ListTime);
            builder.AppendFormat("\"PropertyAlias\":\"{0}\",", taobaoProduct.PropertyAlias);
            builder.AppendFormat("\"InputPids\":\"{0}\",", taobaoProduct.InputPids);
            builder.AppendFormat("\"InputStr\":\"{0}\",", taobaoProduct.InputStr);
            builder.AppendFormat("\"SkuProperties\":\"{0}\",", taobaoProduct.SkuProperties);
            builder.AppendFormat("\"SkuQuantities\":\"{0}\",", taobaoProduct.SkuQuantities);
            builder.AppendFormat("\"SkuPrices\":\"{0}\",", taobaoProduct.SkuPrices);
            builder.AppendFormat("\"SkuOuterIds\":\"{0}\",", taobaoProduct.SkuOuterIds);
            builder.AppendFormat("\"TaobaoProductId\":\"{0}\",", taobaoProduct.TaobaoProductId);
            builder.AppendFormat("\"ProductCode\":\"{0}\",", taobaoProduct.ProductCode);
            builder.AppendFormat("\"Description\":\"{0}\",", taobaoProduct.Description.Replace(string.Format("src=\"{0}/Storage/master/gallery", Globals.ApplicationPath), string.Format("src=\"{0}/Storage/master/gallery", str + Globals.ApplicationPath)).Replace('"', '“'));
            string str2 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl1;
            if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl1)) && ((str2.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || str2.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)) || (str2.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || str2.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase))))
            {
                builder.AppendFormat("\"ImageName\":\"{0}\",", str2);
            }
            if (!string.IsNullOrEmpty(taobaoProduct.ImageUrl2))
            {
                string str3 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl2;
                if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl2)) && ((str3.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || str3.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)) || (str3.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || str3.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase))))
                {
                    builder.AppendFormat("\"ImageName2\":\"{0}\",", str3);
                }
            }
            if (!string.IsNullOrEmpty(taobaoProduct.ImageUrl3))
            {
                string str4 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl3;
                if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl3)) && ((str4.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || str4.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)) || (str4.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || str4.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase))))
                {
                    builder.AppendFormat("\"ImageName3\":\"{0}\",", str4);
                }
            }
            if (!string.IsNullOrEmpty(taobaoProduct.ImageUrl4))
            {
                string str5 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl4;
                if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl4)) && ((str5.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || str5.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)) || (str5.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || str5.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase))))
                {
                    builder.AppendFormat("\"ImageName4\":\"{0}\",", str5);
                }
            }
            if (!string.IsNullOrEmpty(taobaoProduct.ImageUrl5))
            {
                string str6 = str + Globals.ApplicationPath + taobaoProduct.ImageUrl5;
                if (System.IO.File.Exists(Globals.MapPath(Globals.ApplicationPath + taobaoProduct.ImageUrl5)) && ((str6.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) || str6.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase)) || (str6.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) || str6.EndsWith(".jpeg", StringComparison.CurrentCultureIgnoreCase))))
                {
                    builder.AppendFormat("\"ImageName5\":\"{0}\",", str6);
                }
            }
            builder.AppendFormat("\"SalePrice\":\"{0}\"", taobaoProduct.SalePrice.ToString("F2"));
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void ProcessProductSearch(HttpContext context)
        {
            string str = "http://" + HttpContext.Current.Request.Url.Host + ((HttpContext.Current.Request.Url.Port == 80) ? "" : (":" + HttpContext.Current.Request.Url.Port));
            ProductQuery entity = new ProductQuery();
            entity.UserId = new int?(int.Parse(context.Request["distributorUserId"]));
            entity.PageIndex = int.Parse(context.Request["pageIndex"]);
            entity.PageSize = int.Parse(context.Request["pageSize"]);
            entity.Keywords = context.Request["productName"];
            entity.ProductCode = context.Request["productCode"];
            if (!string.IsNullOrEmpty(context.Request["publishStatus"]))
            {
                entity.PublishStatus = (PublishStatus) int.Parse(context.Request["publishStatus"]);
            }
            if (!string.IsNullOrEmpty(context.Request["startDate"]))
            {
                entity.StartDate = new DateTime?(DateTime.Parse(context.Request["startDate"]));
            }
            if (!string.IsNullOrEmpty(context.Request["endDate"]))
            {
                entity.EndDate = new DateTime?(DateTime.Parse(context.Request["endDate"]));
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult toTaobaoProducts = SubSiteProducthelper.GetToTaobaoProducts(entity);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Products\":[");
            DataTable data = (DataTable) toTaobaoProducts.Data;
            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    builder.Append("{");
                    builder.AppendFormat("\"ProductId\":{0},", row["ProductId"]);
                    builder.AppendFormat("\"ProductDetailLink\":\"{0}\",", str + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"] }));
                    builder.AppendFormat("\"DisplaySequence\":{0},", row["DisplaySequence"]);
                    builder.AppendFormat("\"ThumbnailUrl40\":\"{0}\",", (row["ThumbnailUrl40"] != DBNull.Value) ? (str + Globals.ApplicationPath + ((string) row["ThumbnailUrl40"])) : "");
                    builder.AppendFormat("\"ProductName\":\"{0}\",", row["ProductName"]);
                    builder.AppendFormat("\"ProductCode\":\"{0}\",", row["ProductCode"]);
                    builder.AppendFormat("\"Stock\":{0},", row["Stock"]);
                    builder.AppendFormat("\"MarketPrice\":{0},", (row["MarketPrice"] != DBNull.Value) ? ((decimal) row["MarketPrice"]).ToString("F2") : "0");
                    builder.AppendFormat("\"SalePrice\":{0},", (row["SalePrice"] != DBNull.Value) ? ((decimal) row["SalePrice"]).ToString("F2") : "0");
                    builder.AppendFormat("\"PurchasePrice\":{0},", (row["PurchasePrice"] != DBNull.Value) ? ((decimal) row["PurchasePrice"]).ToString("F2") : "0");
                    builder.AppendFormat("\"IsPublish\":{0}", (row["IsPublish"] != DBNull.Value) ? ((int) row["IsPublish"]) : -1);
                    builder.Append("},");
                }
                builder.Remove(builder.Length - 1, 1);
            }
            builder.Append("],");
            builder.AppendFormat("\"TotalResults\":{0}", toTaobaoProducts.TotalRecords);
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            GzipExtention.Gzip(context);
            switch (context.Request["action"])
            {
                case "ProductSearch":
                    this.ProcessProductSearch(context);
                    return;

                case "ProductDetails":
                    this.ProcessProductDetails(context);
                    return;

                case "TaobaoProductIdAdd":
                    this.ProcessTaobaoProductIdAdd(context);
                    return;

                case "TaobaoProductIsExit":
                    this.ProcessTaobaoProductIsExit(context);
                    return;

                case "TaobaoProductDown":
                    this.ProcessTaobaoProductDown(context);
                    return;
            }
            context.Response.Write("error");
        }

        private void ProcessTaobaoProductDown(HttpContext context)
        {
            ProductInfo product = new ProductInfo();
            product.CategoryId = 0;
            product.BrandId = 0;
            product.ProductName = HttpUtility.UrlDecode(context.Request.Form["ProductName"]);
            product.ProductCode = context.Request.Form["ProductCode"];
            product.Description = HttpUtility.UrlDecode(context.Request.Form["Description"]);
            if (context.Request.Form["SaleStatus"] == "onsale")
            {
                product.SaleStatus = ProductSaleStatus.OnSale;
            }
            else
            {
                product.SaleStatus = ProductSaleStatus.OnStock;
            }
            product.AddedDate = DateTime.Parse(context.Request.Form["AddedDate"]);
            product.TaobaoProductId = long.Parse(context.Request.Form["TaobaoProductId"]);
            string str = context.Request.Form["ImageUrls"];
            if (!string.IsNullOrEmpty(str))
            {
                this.DownloadImage(product, str, context);
            }
            product.TypeId = new int?(ProductTypeHelper.GetTypeId(context.Request.Form["TypeName"]));
            int weight = int.Parse(context.Request.Form["Weight"]);
            Dictionary<string, SKUItem> skus = this.GetSkus(product, weight, context);
            product.LowestSalePrice = skus.Values.First<SKUItem>().SalePrice;
            ProductActionStatus status = ProductHelper.AddProduct(product, skus, null, null);
            if (status == ProductActionStatus.Success)
            {
                TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
                taobaoProduct.ProductId = product.ProductId;
                taobaoProduct.ProTitle = product.ProductName;
                taobaoProduct.Num = product.Stock;
                ProductHelper.UpdateToaobProduct(taobaoProduct);
            }
            context.Response.Write(status.ToString());
        }

        private void ProcessTaobaoProductIdAdd(HttpContext context)
        {
            int distributorId = int.Parse(context.Request["distributorUserId"]);
            int productId = int.Parse(context.Request["productId"]);
            long taobaoProductId = long.Parse(context.Request["taobaoProductId"]);
            bool flag = SubSiteProducthelper.AddTaobaoProductId(productId, taobaoProductId, distributorId);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat("\"TaobaoProductIdAddResponse\":\"{0}\"", flag);
            builder.Append("}");
            context.Response.Write(builder.ToString());
        }

        private void ProcessTaobaoProductIsExit(HttpContext context)
        {
            bool flag = ProductHelper.IsExitTaobaoProduct(long.Parse(context.Request.Form["taobaoProductId"]));
            context.Response.Write(flag.ToString());
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

