namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Services;
    using System.Xml;

    [WebService(Namespace="http://tempuri.org/"), WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
    public class PurchaseOrderAPIHandler : IHttpHandler
    {
        private string message = "";

        public StringBuilder GetOrderDetails(string format, string orderitemfomat, PurchaseOrderInfo order)
        {
            StringBuilder builder = new StringBuilder();
            string str = string.Empty;
            if (order == null)
            {
                return builder;
            }
            string str2 = "false";
            StringBuilder builder2 = new StringBuilder();
            long num = 0L;
            foreach (PurchaseOrderItemInfo info in order.PurchaseOrderItems)
            {
                str2 = "true";
                builder2.AppendFormat(orderitemfomat, new object[] { "0", order.OrderId, info.ProductId.ToString(), info.ItemDescription, info.SKU, info.SKUContent, info.Quantity.ToString(), info.ItemListPrice.ToString("F2"), info.ItemPurchasePrice.ToString("F2"), info.ItemCostPrice.ToString("F2") });
                num += info.Quantity;
            }
            Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(order.ShippingRegion);
            builder.AppendFormat(format, new object[] { 
                order.OrderId, "0", order.Distributorname, order.DistributorEmail, order.ShipTo, shippingRegion["Province"], shippingRegion["City"].ToString(), shippingRegion["District"], order.Address, order.ZipCode, order.CellPhone, order.TelPhone, order.Remark, order.ManagerMark, order.ManagerRemark, num.ToString(), 
                order.OrderTotal.ToString("F2"), order.OrderTotal.ToString("F2"), order.AdjustedFreight.ToString("F2"), order.GetPurchaseProfit().ToString("F2"), order.GetPurchaseTotal().ToString("F2"), order.PayDate.ToString(), order.ShippingDate.ToString(), ((int) order.RefundStatus).ToString(), order.RefundAmount.ToString("F2"), order.RefundRemark, ((int) order.PurchaseStatus).ToString(), str2, builder2
             });
            if (!string.IsNullOrEmpty(order.ShippingRegion))
            {
                str = order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(order.Address))
            {
                str = str + order.Address;
            }
            if (!string.IsNullOrEmpty(order.ShipTo))
            {
                str = str + "   " + order.ShipTo;
            }
            if (!string.IsNullOrEmpty(order.ZipCode))
            {
                str = str + "   " + order.ZipCode;
            }
            if (!string.IsNullOrEmpty(order.TelPhone))
            {
                str = str + "   " + order.TelPhone;
            }
            if (!string.IsNullOrEmpty(order.CellPhone))
            {
                str = str + "   " + order.CellPhone;
            }
            string str3 = "<ShipAddress>{0}</ShipAddress><ModeName>{1}</ModeName><ShipOrderNumber>{2}</ShipOrderNumber><ExpressCompanyName>{3}</ExpressCompanyName>";
            str3 = string.Format(str3, new object[] { str, order.RealModeName, order.ShipOrderNumber, order.ExpressCompanyName });
            return builder.Replace("</Status>", "</Status>" + str3);
        }

        public StringBuilder GetOrderList(PurchaseOrderQuery query, string format, string orderitemfomat, out int totalrecords)
        {
            int totalrecord = 0;
            Globals.EntityCoding(query, true);
            DataSet aPIPurchaseOrders = SalesHelper.GetAPIPurchaseOrders(query, out totalrecord);
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in aPIPurchaseOrders.Tables[0].Rows)
            {
                string str = "false";
                StringBuilder builder2 = new StringBuilder();
                foreach (DataRow row2 in row.GetChildRows("PurchaseOrderRelation"))
                {
                    string str2 = row2["SKUContent"].ToString();
                    str = "true";
                    builder2.AppendFormat(orderitemfomat, new object[] { row2["Tid"].ToString(), row2["PurchaseOrderId"].ToString(), row2["ProductId"].ToString(), row2["ItemDescription"].ToString(), row2["SKU"].ToString(), str2, row2["Quantity"].ToString(), decimal.Parse(row2["ItemListPrice"].ToString()).ToString("F2"), decimal.Parse(row2["ItemPurchasePrice"].ToString()).ToString("F2"), decimal.Parse(row2["CostPrice"].ToString()).ToString("F2") });
                }
                Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(row["ShippingRegion"].ToString());
                builder.AppendFormat(format, new object[] { 
                    row["PurchaseOrderId"].ToString(), row["SellerUid"].ToString(), row["Username"].ToString(), row["EmailAddress"].ToString(), row["ShipTo"].ToString(), shippingRegion["Province"], shippingRegion["City"].ToString(), shippingRegion["District"], row["Address"].ToString(), row["ZipCode"].ToString(), row["CellPhone"].ToString(), row["TelPhone"].ToString(), row["Remark"].ToString(), row["ManagerMark"].ToString(), row["ManagerRemark"].ToString(), row["Nums"].ToString(), 
                    decimal.Parse(row["OrderTotal"].ToString()).ToString("F2"), decimal.Parse(row["OrderTotal"].ToString()).ToString("F2"), decimal.Parse(row["AdjustedFreight"].ToString()).ToString("F2"), decimal.Parse(row["Profit"].ToString()).ToString("F2"), decimal.Parse(row["PurchaseTotal"].ToString()).ToString("F2"), row["PayDate"].ToString(), row["ShippingDate"].ToString(), row["ReFundStatus"].ToString(), decimal.Parse(row["RefundAmount"].ToString()).ToString("F2"), row["RefundRemark"].ToString(), row["OrderStatus"].ToString(), str, builder2
                 });
            }
            totalrecords = totalrecord;
            return builder;
        }

        public void ProcessRequest(HttpContext context)
        {
            string oldValue = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str2 = "";
            string str3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str4 = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string format = "<trade><Oid>{0}</Oid><SellerUid>{1}</SellerUid><BuyerNick>{2}</BuyerNick><BuyerEmail>{3}</BuyerEmail><ReceiverName>{4}</ReceiverName><ReceiverState>{5}</ReceiverState><ReceiverCity>{6}</ReceiverCity><ReceiverDistrict>{7}</ReceiverDistrict><ReceiverAddress>{8}</ReceiverAddress><ReceiverZip>{9}</ReceiverZip><ReceiverMobile>{10}</ReceiverMobile><ReceiverPhone>{11}</ReceiverPhone><BuyerMemo>{12}</BuyerMemo><OrderMark>{13}</OrderMark><SellerMemo>{14}</SellerMemo><Nums>{15}</Nums><Price>{16}</Price><Payment>{17}</Payment><PostFee>{18}</PostFee><Profit>{19}</Profit><PurchaseTotal>{20}</PurchaseTotal><PaymentTs>{21}</PaymentTs><SentTs>{22}</SentTs><RefundStatus>{23}</RefundStatus><RefundAmount>{24}</RefundAmount><RefundRemark>{25}</RefundRemark><Status>{26}</Status><orders list=\"{27}\">{28}</orders></trade>";
            string orderitemfomat = "<order><Tid>{0}</Tid><Oid>{1}</Oid><GoodsIid>{2}</GoodsIid><Title>{3}</Title><OuterId>{4}</OuterId><SKUContent>{5}</SKUContent><Nums>{6}</Nums><Price>{7}</Price><Payment>{8}</Payment><CostPrice>{9}</CostPrice></order>";
            StringBuilder builder = new StringBuilder();
            str2 = context.Request.QueryString["action"].ToString();
            string sign = context.Request.Form["sign"];
            string checkCode = masterSettings.CheckCode;
            string str9 = context.Request.Form["format"];
            XmlDocument node = new XmlDocument();
            SortedDictionary<string, string> tmpParas = new SortedDictionary<string, string>();
            string str22 = str2;
            if (str22 != null)
            {
                if (!(str22 == "tradelist"))
                {
                    string str15;
                    if (str22 == "tradedetails")
                    {
                        str15 = "";
                        if (!string.IsNullOrEmpty(context.Request.Form["tid"].Trim()))
                        {
                            str15 = context.Request.Form["tid"].Trim();
                            tmpParas = new SortedDictionary<string, string>();
                            tmpParas.Add("tid", context.Request.Form["tid"]);
                            tmpParas.Add("format", str9);
                            if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                            {
                                string str16 = context.Request.Form["tid"].Replace("\r\n", "\n");
                                if (!string.IsNullOrEmpty(str16))
                                {
                                    str15 = str16;
                                    PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(str15);
                                    builder.Append("<trade_get_response>");
                                    builder.Append(this.GetOrderDetails(format, orderitemfomat, purchaseOrder).ToString());
                                    builder.Append("</trade_get_response>");
                                    this.message = oldValue + builder.ToString();
                                }
                                else
                                {
                                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "tid");
                                }
                            }
                            else
                            {
                                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "signature");
                            }
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid");
                        }
                    }
                    else if (str22 == "send")
                    {
                        string str17 = context.Request.Form["tid"].Trim();
                        string str18 = context.Request.Form["out_sid"].Trim();
                        string str19 = context.Request.Form["company_code"].Trim();
                        if ((!string.IsNullOrEmpty(str17) && !string.IsNullOrEmpty(str19)) && !string.IsNullOrEmpty(str18))
                        {
                            tmpParas.Add("tid", str17);
                            tmpParas.Add("out_sid", str18);
                            tmpParas.Add("company_code", str19);
                            tmpParas.Add("format", str9);
                            if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                            {
                                ExpressCompanyInfo express = ExpressHelper.FindNodeByCode(str19);
                                if (!string.IsNullOrEmpty(express.Name))
                                {
                                    ShippingModeInfo shippingModeByCompany = SalesHelper.GetShippingModeByCompany(express.Name);
                                    PurchaseOrderInfo purchaseorder = SalesHelper.GetPurchaseOrder(str17);
                                    if (purchaseorder != null)
                                    {
                                        ApiErrorCode messageenum = this.SendOrders(purchaseorder, shippingModeByCompany, str18, express);
                                        if (messageenum == ApiErrorCode.Success)
                                        {
                                            builder.Append("<trade_get_response>");
                                            purchaseorder = SalesHelper.GetPurchaseOrder(str17);
                                            builder.Append(this.GetOrderDetails(format, orderitemfomat, purchaseorder).ToString());
                                            builder.Append("</trade_get_response>");
                                            this.message = oldValue + builder.ToString();
                                        }
                                        else
                                        {
                                            str4 = MessageInfo.ShowMessageInfo(messageenum, "It");
                                        }
                                    }
                                    else
                                    {
                                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "tid");
                                    }
                                }
                                else
                                {
                                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "company_code");
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
                    }
                    else if (str22 == "mark")
                    {
                        string str20 = context.Request.Form["order_mark"].Trim();
                        string str21 = context.Request.Form["seller_memo"].Trim();
                        if ((!string.IsNullOrEmpty(context.Request.Form["tid"].Trim()) && !string.IsNullOrEmpty(str20)) && !string.IsNullOrEmpty(str21))
                        {
                            if ((Convert.ToInt32(str20) > 0) && (Convert.ToInt32(str20) < 7))
                            {
                                str15 = context.Request.Form["tid"].Trim();
                                tmpParas.Add("tid", str15);
                                tmpParas.Add("order_mark", str20);
                                tmpParas.Add("seller_memo", str21);
                                tmpParas.Add("format", str9);
                                if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                                {
                                    PurchaseOrderInfo info5 = SalesHelper.GetPurchaseOrder(str15);
                                    info5.ManagerMark = new OrderMark?((OrderMark) Enum.Parse(typeof(OrderMark), str20, true));
                                    info5.ManagerRemark = Globals.HtmlEncode(str21);
                                    if (SalesHelper.SaveAPIPurchaseOrderRemark(info5))
                                    {
                                        builder.Append("<trade_get_response>");
                                        builder.Append(this.GetOrderDetails(format, orderitemfomat, info5).ToString());
                                        builder.Append("</trade_get_response>");
                                        this.message = oldValue + builder.ToString();
                                    }
                                    else
                                    {
                                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "save is failure ");
                                    }
                                }
                                else
                                {
                                    str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                                }
                            }
                            else
                            {
                                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "order_mark");
                            }
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid or order_mark or seller_memo");
                        }
                    }
                }
                else
                {
                    PurchaseOrderQuery query2 = new PurchaseOrderQuery();
                    query2.PageSize = 100;
                    PurchaseOrderQuery query = query2;
                    int totalrecords = 0;
                    string str10 = context.Request.Form["status"].Trim();
                    string str11 = context.Request.Form["buynick"].Trim();
                    string str12 = context.Request.Form["pageindex"].Trim();
                    string str13 = context.Request.Form["starttime"].Trim();
                    string str14 = context.Request.Form["endtime"].Trim();
                    if (!string.IsNullOrEmpty(str10) && (Convert.ToInt32(str10) >= 0))
                    {
                        query.PurchaseStatus = (OrderStatus) Enum.Parse(typeof(OrderStatus), str10, true);
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "status");
                    }
                    if (!string.IsNullOrEmpty(str12) && (Convert.ToInt32(str12) > 0))
                    {
                        query.PageIndex = Convert.ToInt32(str12);
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "pageindex");
                    }
                    if (string.IsNullOrEmpty(str4))
                    {
                        tmpParas.Add("status", str10);
                        tmpParas.Add("buynick", str11);
                        tmpParas.Add("pageindex", str12);
                        tmpParas.Add("starttime", str13);
                        tmpParas.Add("endtime", str14);
                        tmpParas.Add("format", str9);
                        if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                        {
                            if (!string.IsNullOrEmpty(str11))
                            {
                                query.DistributorName = str11;
                            }
                            if (!string.IsNullOrEmpty(str13))
                            {
                                query.StartDate = new DateTime?(Convert.ToDateTime(str13));
                            }
                            if (!string.IsNullOrEmpty(str14))
                            {
                                query.EndDate = new DateTime?(Convert.ToDateTime(str14));
                            }
                            query.SortOrder = SortAction.Desc;
                            query.SortBy = "PurchaseDate";
                            builder.Append("<trade_get_response>");
                            builder.Append(this.GetOrderList(query, format, orderitemfomat, out totalrecords));
                            builder.Append("<totalrecord>" + totalrecords + "</totalrecord>");
                            builder.Append("</trade_get_response>");
                            this.message = oldValue + builder.ToString();
                        }
                        else
                        {
                            str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
                        }
                    }
                    else
                    {
                        str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramter");
                    }
                }
            }
            if (this.message == "")
            {
                this.message = this.message + str3 + str4;
            }
            context.Response.ContentType = "text/xml";
            if (str9 == "json")
            {
                this.message = this.message.Replace(oldValue, "");
                node.Load(new MemoryStream(Encoding.GetEncoding("UTF-8").GetBytes(this.message)));
                this.message = JavaScriptConvert.SerializeXmlNode(node);
                context.Response.ContentType = "text/json";
            }
            context.Response.Write(this.message);
        }

        public ApiErrorCode SendOrders(PurchaseOrderInfo purchaseorder, ShippingModeInfo shippingmode, string out_id, ExpressCompanyInfo express)
        {
            if (string.IsNullOrEmpty(out_id))
            {
                return ApiErrorCode.ShipingOrderNumber_Error;
            }
            if (purchaseorder == null)
            {
                return ApiErrorCode.NoExists_Error;
            }
            if (purchaseorder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid)
            {
                return ApiErrorCode.NoPay_Error;
            }
            if (shippingmode.ModeId <= 0)
            {
                return ApiErrorCode.NoShippingMode;
            }
            if (string.IsNullOrEmpty(express.Name))
            {
                return ApiErrorCode.Empty_Error;
            }
            purchaseorder.RealShippingModeId = shippingmode.ModeId;
            purchaseorder.RealModeName = shippingmode.Name;
            purchaseorder.ExpressCompanyName = express.Name;
            purchaseorder.ExpressCompanyAbb = express.Kuaidi100Code;
            purchaseorder.ShipOrderNumber = out_id;
            if (SalesHelper.SendAPIPurchaseOrderGoods(purchaseorder))
            {
                return ApiErrorCode.Success;
            }
            return ApiErrorCode.Unknown_Error;
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

