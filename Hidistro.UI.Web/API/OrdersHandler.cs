namespace Hidistro.UI.Web.API
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Services;

    [WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1), WebService(Namespace="http://tempuri.org/")]
    public class OrdersHandler : IHttpHandler
    {
        public StringBuilder GetOrderDetails(string format, string orderitemfomat, OrderInfo order)
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
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                str2 = "true";
                builder2.AppendFormat(orderitemfomat, new object[] { "0", order.OrderId, info.ProductId.ToString(), info.ItemDescription, info.SKU, info.SKUContent, info.Quantity.ToString(), info.ItemListPrice.ToString(), info.ItemAdjustedPrice.ToString() });
                num += info.Quantity;
            }
            Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(order.ShippingRegion);
            builder.AppendFormat(format, new object[] { 
                order.OrderId, "0", order.Username, order.EmailAddress, order.ShipTo, shippingRegion["Province"], shippingRegion["City"].ToString(), shippingRegion["District"], order.Address, order.ZipCode, order.CellPhone, order.TelPhone, order.Remark, order.ManagerMark, order.ManagerRemark, num.ToString(), 
                order.GetTotal().ToString(), order.GetTotal().ToString(), order.AdjustedFreight.ToString(), order.ReducedPromotionAmount.ToString(), order.AdjustedDiscount.ToString(), order.PayDate.ToString(), order.ShippingDate.ToString(), ((int) order.RefundStatus).ToString(), order.RefundAmount.ToString(), order.RefundRemark, ((int) order.OrderStatus).ToString(), str2, builder2
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

        public StringBuilder GetOrderList(OrderQuery query, string format, string orderitemfomat, out int totalrecords)
        {
            int records = 0;
            Globals.EntityCoding(query, true);
            DataSet tradeOrders = OrderHelper.GetTradeOrders(query, out records);
            StringBuilder builder = new StringBuilder();
            foreach (DataRow row in tradeOrders.Tables[0].Rows)
            {
                string str = "false";
                StringBuilder builder2 = new StringBuilder();
                foreach (DataRow row2 in row.GetChildRows("OrderRelation"))
                {
                    string str2 = row2["SKUContent"].ToString();
                    str = "true";
                    builder2.AppendFormat(orderitemfomat, new object[] { row2["Tid"].ToString(), row2["OrderId"].ToString(), row2["ProductId"].ToString(), row2["ItemDescription"].ToString(), row2["SKU"].ToString(), str2, row2["Quantity"].ToString(), decimal.Parse(row2["ItemListPrice"].ToString()).ToString("F2"), decimal.Parse(row2["ItemAdjustedPrice"].ToString()).ToString("F2") });
                }
                Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(row["ShippingRegion"].ToString());
                builder.AppendFormat(format, new object[] { 
                    row["OrderId"].ToString(), row["SellerUid"].ToString(), row["Username"].ToString(), row["EmailAddress"].ToString(), row["ShipTo"].ToString(), shippingRegion["Province"], shippingRegion["City"].ToString(), shippingRegion["District"], row["Address"].ToString(), row["ZipCode"].ToString(), row["CellPhone"].ToString(), row["TelPhone"].ToString(), row["Remark"].ToString(), row["ManagerMark"].ToString(), row["ManagerRemark"].ToString(), row["Nums"].ToString(), 
                    decimal.Parse(row["OrderTotal"].ToString()).ToString("F2"), decimal.Parse(row["OrderTotal"].ToString()).ToString("F2"), decimal.Parse(row["AdjustedFreight"].ToString()).ToString("F2"), decimal.Parse(row["DiscountValue"].ToString()).ToString("F2"), decimal.Parse(row["AdjustedDiscount"].ToString()).ToString("F2"), row["PayDate"].ToString(), row["ShippingDate"].ToString(), row["ReFundStatus"].ToString(), row["RefundAmount"].ToString(), row["RefundRemark"].ToString(), row["OrderStatus"].ToString(), str, builder2
                 });
            }
            totalrecords = records;
            return builder;
        }

        public void ProcessRequest(HttpContext context)
        {
            string s = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str2 = "";
            string str3 = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            string str4 = "";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string format = "<trade><Oid>{0}</Oid><SellerUid>{1}</SellerUid><BuyerNick>{2}</BuyerNick><BuyerEmail>{3}</BuyerEmail><ReceiverName>{4}</ReceiverName><ReceiverState>{5}</ReceiverState><ReceiverCity>{6}</ReceiverCity><ReceiverDistrict>{7}</ReceiverDistrict><ReceiverAddress>{8}</ReceiverAddress><ReceiverZip>{9}</ReceiverZip><ReceiverMobile>{10}</ReceiverMobile><ReceiverPhone>{11}</ReceiverPhone><BuyerMemo>{12}</BuyerMemo><OrderMark>{13}</OrderMark><SellerMemo>{14}</SellerMemo><Nums>{15}</Nums><Price>{16}</Price><Payment>{17}</Payment><PostFee>{18}</PostFee><DiscountFee>{19}</DiscountFee><AdjustFee>{20}</AdjustFee><PaymentTs>{21}</PaymentTs><SentTs>{22}</SentTs><RefundStatus>{23}</RefundStatus><RefundAmount>{24}</RefundAmount><RefundRemark>{25}</RefundRemark><Status>{26}</Status><orders list=\"{27}\">{28}</orders></trade>";
            string orderitemfomat = "<order><Tid>{0}</Tid><Oid>{1}</Oid><GoodsIid>{2}</GoodsIid><Title>{3}</Title><OuterId>{4}</OuterId><SKUContent>{5}</SKUContent><Nums>{6}</Nums><Price>{7}</Price><Payment>{8}</Payment></order>";
            StringBuilder builder = new StringBuilder();
            str2 = context.Request.QueryString["action"].ToString();
            string sign = context.Request.Form["sign"];
            string checkCode = masterSettings.CheckCode;
            string str9 = context.Request.Form["format"];
            new Dictionary<string, string>();
            SortedDictionary<string, string> tmpParas = new SortedDictionary<string, string>();
            try
            {
                string str15;
                string str17;
                string str20;
                if (string.IsNullOrEmpty(str2))
                {
                    goto Label_07D6;
                }
                string str22 = str2;
                if (str22 == null)
                {
                    goto Label_07B9;
                }
                if (!(str22 == "tradelist"))
                {
                    if (str22 == "tradedetails")
                    {
                        goto Label_0336;
                    }
                    if (str22 == "send")
                    {
                        goto Label_046D;
                    }
                    if (str22 == "mark")
                    {
                        goto Label_0613;
                    }
                    goto Label_07B9;
                }
                OrderQuery query2 = new OrderQuery();
                query2.PageSize = 100;
                OrderQuery query = query2;
                int totalrecords = 0;
                string str10 = context.Request.Form["status"].Trim();
                string str11 = context.Request.Form["buyernick"].Trim();
                string str12 = context.Request.Form["pageindex"].Trim();
                string str13 = context.Request.Form["starttime"].Trim();
                string str14 = context.Request.Form["endtime"].Trim();
                if (!string.IsNullOrEmpty(str10) && (Convert.ToInt32(str10) >= 0))
                {
                    query.Status = (OrderStatus) Enum.Parse(typeof(OrderStatus), str10, true);
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
                    tmpParas.Add("buyernick", str11);
                    tmpParas.Add("pageindex", str12);
                    tmpParas.Add("starttime", str13);
                    tmpParas.Add("endtime", str14);
                    tmpParas.Add("format", str9);
                    if (APIHelper.CheckSign(tmpParas, checkCode, sign))
                    {
                        if (!string.IsNullOrEmpty(str11))
                        {
                            query.UserName = str11;
                        }
                        if (!string.IsNullOrEmpty(str13))
                        {
                            query.StartDate = new DateTime?(Convert.ToDateTime(str13));
                        }
                        if (!string.IsNullOrEmpty(str14))
                        {
                            query.EndDate = new DateTime?(Convert.ToDateTime(str14));
                        }
                        builder.Append("<trade_get_response>");
                        builder.Append(this.GetOrderList(query, format, orderitemfomat, out totalrecords).ToString());
                        builder.Append("<totalrecord>" + totalrecords + "</totalrecord>");
                        builder.Append("</trade_get_response>");
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
                goto Label_07C6;
            Label_0336:
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
                            OrderInfo orderInfo = OrderHelper.GetOrderInfo(str15);
                            builder.Append("<trade_get_response>");
                            builder.Append(this.GetOrderDetails(format, orderitemfomat, orderInfo).ToString());
                            builder.Append("</trade_get_response>");
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
                goto Label_07C6;
            Label_046D:
                str17 = context.Request.Form["tid"].Trim();
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
                            OrderInfo order = OrderHelper.GetOrderInfo(str17);
                            if (order != null)
                            {
                                ApiErrorCode messageenum = this.SendOrders(order, shippingModeByCompany, str18, express);
                                if (messageenum == ApiErrorCode.Success)
                                {
                                    builder.Append("<trade_get_response>");
                                    order = OrderHelper.GetOrderInfo(str17);
                                    builder.Append(this.GetOrderDetails(format, orderitemfomat, order).ToString());
                                    builder.Append("</trade_get_response>");
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
                goto Label_07C6;
            Label_0613:
                str20 = context.Request.Form["order_mark"].Trim();
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
                            OrderInfo info5 = OrderHelper.GetOrderInfo(str15);
                            info5.ManagerMark = new OrderMark?((OrderMark) Enum.Parse(typeof(OrderMark), str20, true));
                            info5.ManagerRemark = Globals.HtmlEncode(str21);
                            if (OrderHelper.SaveRemarkAPI(info5))
                            {
                                builder.Append("<trade_get_response>");
                                builder.Append(this.GetOrderDetails(format, orderitemfomat, info5).ToString());
                                builder.Append("</trade_get_response>");
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
                goto Label_07C6;
            Label_07B9:
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
            Label_07C6:
                s = s + builder.ToString();
                goto Label_07FB;
            Label_07D6:
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "sign");
            }
            catch (Exception exception)
            {
                str4 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, exception.Message);
            }
        Label_07FB:
            if (!string.IsNullOrEmpty(str4))
            {
                s = str3 + str4;
            }
            context.Response.ContentType = "text/xml";
            context.Response.Write(s);
        }

        public ApiErrorCode SendOrders(OrderInfo order, ShippingModeInfo shippingmode, string out_id, ExpressCompanyInfo express)
        {
            if ((order.GroupBuyId > 0) && (order.GroupBuyStatus != GroupBuyStatus.Success))
            {
                return ApiErrorCode.Group_Error;
            }
            if (!order.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                return ApiErrorCode.NoPay_Error;
            }
            if (shippingmode.ModeId <= 0)
            {
                return ApiErrorCode.NoShippingMode;
            }
            if (string.IsNullOrEmpty(out_id) || (out_id.Length > 20))
            {
                return ApiErrorCode.ShipingOrderNumber_Error;
            }
            order.RealShippingModeId = shippingmode.ModeId;
            order.RealModeName = shippingmode.Name;
            order.ExpressCompanyName = express.Name;
            order.ExpressCompanyAbb = express.Kuaidi100Code;
            order.ShipOrderNumber = out_id;
            if (!OrderHelper.SendAPIGoods(order))
            {
                return ApiErrorCode.Unknown_Error;
            }
            if (!string.IsNullOrEmpty(order.GatewayOrderId) && (order.GatewayOrderId.Trim().Length > 0))
            {
                PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(order.PaymentTypeId);
                if (paymentMode != null)
                {
                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), order.OrderId, order.GetTotal(), "订单发货", "订单号-" + order.OrderId, order.EmailAddress, order.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(order.GatewayOrderId, order.RealModeName, order.ShipOrderNumber, "EXPRESS");
                }
            }
            int userId = order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            IUser user = Users.GetUser(userId);
            Messenger.OrderShipping(order, user);
            order.OnDeliver();
            return ApiErrorCode.Success;
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

