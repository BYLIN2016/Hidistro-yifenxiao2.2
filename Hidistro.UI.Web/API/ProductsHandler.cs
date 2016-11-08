namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Services;

    [WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1), WebService(Namespace="http://tempuri.org/")]
    public class ProductsHandler : IHttpHandler
    {
        public StringBuilder GetProductDetailsView(int pid, string localhost, string strformat, string skucontentformat)
        {
            StringBuilder builder = new StringBuilder();
            string str = "false";
            string str2 = "";
            DataSet productSkuDetials = ProductHelper.GetProductSkuDetials(pid);
            foreach (DataRow row in productSkuDetials.Tables[1].Rows)
            {
                str = "true";
                string skuContent = MessageInfo.GetSkuContent(row["SkuId"].ToString());
                str2 = str2 + string.Format(skucontentformat, new object[] { row["ProductId"].ToString(), row["SKU"], skuContent, row["Stock"], row["SalePrice"] });
            }
            foreach (DataRow row2 in productSkuDetials.Tables[0].Rows)
            {
                string str4 = localhost + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row2["ProductId"].ToString() });
                builder.AppendFormat(strformat, new object[] { row2["ProductId"].ToString(), row2["ProductCode"].ToString(), row2["ProductName"].ToString(), localhost + row2["ThumbnailUrl60"].ToString(), str4, row2["MarketPrice"].ToString(), row2["SalePrice"].ToString(), row2["Weight"].ToString(), row2["SaleStatus"].ToString(), str, str2 });
            }
            return builder;
        }

        public StringBuilder GetProductView(ProductQuery query, string localhost, string strformat, string skucontentformat, out int recordes)
        {
            StringBuilder builder = new StringBuilder();
            int totalrecord = 0;
            DataSet productsByQuery = ProductHelper.GetProductsByQuery(query, out totalrecord);
            StringBuilder builder2 = new StringBuilder();
            foreach (DataRow row in productsByQuery.Tables[0].Rows)
            {
                string str = "false";
                foreach (DataRow row2 in row.GetChildRows("ProductRealation"))
                {
                    str = "true";
                    string skuContent = MessageInfo.GetSkuContent(row2["SkuId"].ToString());
                    builder2.AppendFormat(skucontentformat, new object[] { row2["ProductId"].ToString(), row2["SKuId"].ToString(), skuContent, row2["Stock"].ToString(), row2["SalePrice"].ToString() });
                }
                string str3 = localhost + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { row["ProductId"].ToString() });
                builder.AppendFormat(strformat, new object[] { row["ProductId"].ToString(), row["ProductCode"].ToString(), row["ProductName"].ToString(), localhost + row["ThumbnailUrl60"].ToString(), str3, row["MarketPrice"].ToString(), row["SalePrice"].ToString(), row["Weight"].ToString(), row["SaleStatus"].ToString(), str, builder2 });
            }
            recordes = totalrecord;
            return builder;
        }

        public void ProcessRequest(HttpContext context)
        {
            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str2 = "";
            string str3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str4 = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            str2 = context.Request.QueryString["action"].ToString();
            string sign = context.Request.Form["sign"];
            StringBuilder builder = new StringBuilder();
            string skucontentformat = "<Item><lid>{0}</lid><OuterId>{1}</OuterId><SKUContent>{2}</SKUContent><Nums>{3}</Nums><Price>{4}</Price></Item>";
            string strformat = "<product><Iid>{0}</Iid><OuterId>{1}</OuterId><Title>{2}</Title><PicUrl>{3}</PicUrl><Url>{4}</Url><MarketPrice>{5}</MarketPrice><Price>{6}</Price><Weight>{7}</Weight><Status>{8}</Status><SkuItems list=\"{9}\">{10}</SkuItems></product>";
            SortedDictionary<string, string> tmpParas = new SortedDictionary<string, string>();
            string checkCode = masterSettings.CheckCode;
            string str9 = context.Request.Form["format"];
            try
            {
                int num2;
                if (string.IsNullOrEmpty(str2))
                {
                    goto Label_07CE;
                }
                string hostPath = HiContext.Current.HostPath;
                string str18 = str2;
                if (str18 == null)
                {
                    goto Label_07B1;
                }
                if (!(str18 == "productview"))
                {
                    if (str18 == "productsimpleview")
                    {
                        goto Label_07BE;
                    }
                    if (str18 == "stockview")
                    {
                        goto Label_02D2;
                    }
                    if (str18 == "quantity")
                    {
                        goto Label_03CB;
                    }
                    if (str18 == "statue")
                    {
                        goto Label_0608;
                    }
                    goto Label_07B1;
                }
                ProductQuery query2 = new ProductQuery();
                query2.SaleStatus = ProductSaleStatus.OnSale;
                query2.PageSize = 10;
                query2.PageIndex = 1;
                ProductQuery entity = query2;
                string str11 = context.Request.Form["state"].Trim();
                string str12 = context.Request.Form["pageindex"].Trim();
                string str13 = context.Request.Form["pagesize"].Trim();
                if (!string.IsNullOrEmpty(str11) && !string.IsNullOrEmpty(str12))
                {
                    tmpParas.Add("state", str11);
                    tmpParas.Add("pageindex", str12);
                    tmpParas.Add("pagesize", str13);
                    tmpParas.Add("format", str9);
                    if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                    {
                        if (!string.IsNullOrEmpty(str13) && (Convert.ToInt32(str13) > 0))
                        {
                            entity.PageSize = Convert.ToInt32(str13);
                        }
                        if (Convert.ToInt32(str12) > 0)
                        {
                            entity.PageIndex = Convert.ToInt32(str12);
                            entity.SaleStatus = (ProductSaleStatus) Enum.Parse(typeof(ProductSaleStatus), str11, true);
                            Globals.EntityCoding(entity, true);
                            int recordes = 0;
                            builder.Append("<trade_get_response>");
                            builder.Append(this.GetProductView(entity, hostPath, strformat, skucontentformat, out recordes).ToString());
                            builder.Append("<totalrecord>" + recordes + "</totalrecord>");
                            builder.Append("</trade_get_response>");
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "pageindex");
                        }
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                    }
                }
                else
                {
                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "state or pageindex");
                }
                goto Label_07BE;
            Label_02D2:
                num2 = 0;
                if (!string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) && (Convert.ToInt32(context.Request.Form["ProductId"].Trim()) > 0))
                {
                    num2 = Convert.ToInt32(context.Request.Form["productId"].Trim());
                    tmpParas.Add("productid", num2.ToString());
                    tmpParas.Add("format", str9);
                    if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                    {
                        builder.Append("<trade_get_response>");
                        builder.Append(this.GetProductDetailsView(num2, hostPath, strformat, skucontentformat).ToString());
                        builder.Append("</trade_get_response>");
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                    }
                }
                else
                {
                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "productId");
                }
                goto Label_07BE;
            Label_03CB:
                if (!string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) && !string.IsNullOrEmpty(context.Request.Form["quantity"].Trim()))
                {
                    string str14 = context.Request.Form["productId"];
                    string str15 = "";
                    string str16 = "";
                    int type = 1;
                    int stock = 0;
                    stock = Convert.ToInt32(context.Request.Form["quantity"].Trim());
                    if (!string.IsNullOrEmpty(context.Request.Form["sku_id"].Trim()))
                    {
                        str15 = context.Request.Form["sku_id"];
                    }
                    if (!string.IsNullOrEmpty(context.Request.Form["outer_id"].Trim()))
                    {
                        str16 = context.Request.Form["outer_id"];
                    }
                    if (!string.IsNullOrEmpty(context.Request.Form["type"]))
                    {
                        type = Convert.ToInt32(context.Request.Form["type"]);
                    }
                    tmpParas.Add("productId", str14.ToString());
                    tmpParas.Add("quantity", stock.ToString());
                    tmpParas.Add("sku_id", str15);
                    tmpParas.Add("outer_id", str16);
                    tmpParas.Add("type", type.ToString());
                    tmpParas.Add("format", str9);
                    if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                    {
                        ApiErrorCode messageenum = ProductHelper.UpdateProductStock(Convert.ToInt32(str14), str15, str16, type, stock);
                        if (ApiErrorCode.Success == messageenum)
                        {
                            builder.Append("<trade_get_response>");
                            builder.Append(this.GetProductDetailsView(Convert.ToInt32(str14), hostPath, strformat, skucontentformat).ToString());
                            builder.Append("</trade_get_response>");
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(messageenum, "paramters");
                        }
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                    }
                }
                else
                {
                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
                }
                goto Label_07BE;
            Label_0608:
                str11 = context.Request.Form["state"].Trim();
                string str17 = context.Request.Form["productId"].Trim();
                if (!string.IsNullOrEmpty(str11) && !string.IsNullOrEmpty(str17))
                {
                    ProductSaleStatus status = (ProductSaleStatus) Enum.Parse(typeof(ProductSaleStatus), str11, true);
                    num2 = Convert.ToInt32(str17);
                    if (num2 > 0)
                    {
                        tmpParas.Add("productid", str17);
                        tmpParas.Add("state", str11);
                        tmpParas.Add("format", str9);
                        if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                        {
                            bool flag = false;
                            builder.Append("<item_update_statue_response>");
                            switch (status)
                            {
                                case ProductSaleStatus.OnSale:
                                    if (ProductHelper.UpShelf(num2) > 0)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
                                    }
                                    break;

                                case ProductSaleStatus.UnSale:
                                    if (ProductHelper.OffShelf(num2) > 0)
                                    {
                                        flag = true;
                                    }
                                    else
                                    {
                                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
                                    }
                                    break;
                            }
                            if (flag)
                            {
                                builder.Append("<item><num_iid>" + str17 + "</num_iid><modified>" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "</modified></item>");
                            }
                            else
                            {
                                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, "update");
                            }
                            builder.Append("</item_update_statue_response>");
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                        }
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
                    }
                }
                else
                {
                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "state or productId");
                }
                goto Label_07BE;
            Label_07B1:
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
            Label_07BE:
                s = s + builder.ToString();
                goto Label_07F3;
            Label_07CE:
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "modeId");
            }
            catch (Exception exception)
            {
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, exception.Message);
            }
        Label_07F3:
            if (str4 != "")
            {
                s = str3 + str4;
            }
            context.Response.ContentType = "text/xml";
            context.Response.Write(s);
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

