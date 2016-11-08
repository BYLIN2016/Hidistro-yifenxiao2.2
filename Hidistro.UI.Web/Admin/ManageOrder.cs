namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Business;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Orders)]
    public class ManageOrder : AdminPage
    {
        protected Button btnAcceptRefund;
        protected Button btnAcceptReplace;
        protected Button btnAcceptReturn;
        protected Button btnCloseOrder;
        protected Button btnOrderGoods;
        protected Button btnProductGoods;
        protected Button btnRefuseRefund;
        protected Button btnRefuseReplace;
        protected Button btnRefuseReturn;
        protected Button btnRemark;
        protected Button btnSearchButton;
        protected WebCalendar calendarEndDate;
        protected WebCalendar calendarStartDate;
        protected CloseTranReasonDropDownList ddlCloseReason;
        protected DropDownList ddlIsPrinted;
        protected DataList dlstOrders;
        protected RegionSelector dropRegion;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidOrderId;
        protected HtmlInputHidden hidOrderTotal;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected HyperLink hlinkAllOrder;
        protected HyperLink hlinkClose;
        protected HyperLink hlinkHistory;
        protected HyperLink hlinkNotPay;
        protected HyperLink hlinkSendGoods;
        protected HyperLink hlinkTradeFinished;
        protected HyperLink hlinkYetPay;
        protected PageSize hrefPageSize;
        protected Label lblAddress;
        protected Label lblContacts;
        protected Label lblEmail;
        protected Label lblOrderId;
        protected Label lblOrderTotal;
        protected FormatedMoneyLabel lblOrderTotalForRemark;
        protected Label lblRefundRemark;
        protected Label lblRefundType;
        protected Label lblStatus;
        protected Label lblTelephone;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        protected Pager pager;
        protected Pager pager1;
        protected Label replace_lblAddress;
        protected Label replace_lblComments;
        protected Label replace_lblContacts;
        protected Label replace_lblEmail;
        protected Label replace_lblOrderId;
        protected Label replace_lblOrderTotal;
        protected Label replace_lblPostCode;
        protected Label replace_lblTelephone;
        protected TextBox replace_txtAdminRemark;
        protected Label return_lblAddress;
        protected Label return_lblContacts;
        protected Label return_lblEmail;
        protected Label return_lblOrderId;
        protected Label return_lblOrderTotal;
        protected Label return_lblRefundType;
        protected Label return_lblReturnRemark;
        protected Label return_lblTelephone;
        protected TextBox return_txtAdminRemark;
        protected TextBox return_txtRefundMoney;
        protected ShippingModeDropDownList shippingModeDropDownList;
        protected TextBox txtAdminRemark;
        protected TextBox txtOrderId;
        protected TextBox txtProductName;
        protected TextBox txtRemark;
        protected TextBox txtShopTo;
        protected TextBox txtUserName;

        private void BindOrders()
        {
            OrderQuery orderQuery = this.GetOrderQuery();
            DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
            this.dlstOrders.DataSource = orders.Data;
            this.dlstOrders.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;
            this.txtUserName.Text = orderQuery.UserName;
            this.txtOrderId.Text = orderQuery.OrderId;
            this.txtProductName.Text = orderQuery.ProductName;
            this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.lblStatus.Text = ((int) orderQuery.Status).ToString();
            this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
            if (orderQuery.IsPrinted.HasValue)
            {
                this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
            }
            if (orderQuery.RegionId.HasValue)
            {
                this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
            }
        }

        protected void btnAcceptRefund_Click(object sender, EventArgs e)
        {
            string username = HiContext.Current.User.Username;
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            if (OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
            {
                this.BindOrders();
                decimal total = orderInfo.GetTotal();
                if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed))
                {
                    total = orderInfo.GetTotal() - orderInfo.NeedPrice;
                }
                Member user = Users.GetUser(orderInfo.UserId) as Member;
                Messenger.OrderRefund(user, orderInfo.OrderId, total);
                this.ShowMsg("成功的确认了订单退款", true);
            }
        }

        private void btnAcceptReplace_Click(object sender, EventArgs e)
        {
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, true);
            this.BindOrders();
            this.ShowMsg("成功的确认了订单换货", true);
        }

        private void btnAcceptReturn_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.hidRefundMoney.Value, out num))
            {
                this.ShowMsg("退款金额需为数字格式！", false);
            }
            else
            {
                decimal num2;
                decimal.TryParse(this.hidOrderTotal.Value, out num2);
                if (num > num2)
                {
                    this.ShowMsg("退款金额不能大于订单金额！", false);
                }
                else
                {
                    OrderHelper.CheckReturn(OrderHelper.GetOrderInfo(this.hidOrderId.Value), HiContext.Current.User.Username, num, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
                    this.BindOrders();
                    this.ShowMsg("成功的确认了订单退货", true);
                }
            }
        }

        private void btnCloseOrder_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
            orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
            if (OrderHelper.CloseTransaction(orderInfo))
            {
                int userId = orderInfo.UserId;
                if (userId == 0x44c)
                {
                    userId = 0;
                }
                Messenger.OrderClosed(Users.GetUser(userId), orderInfo.OrderId, orderInfo.CloseReason);
                orderInfo.OnClosed();
                this.BindOrders();
                this.ShowMsg("关闭订单成功", true);
            }
            else
            {
                this.ShowMsg("关闭订单失败", false);
            }
        }

        private void btnOrderGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                builder.AppendLine("<td>订单单号</td>");
                if (orderGoods.Tables[1].Rows.Count <= 0)
                {
                    builder.AppendLine("<td>商品名称</td>");
                }
                else
                {
                    builder.AppendLine("<td>商品(礼品)名称</td>");
                }
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("<td>备注</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in orderGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["OrderId"] + "</td>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("<td>" + row["Remark"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                foreach (DataRow row2 in orderGoods.Tables[1].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row2["GiftOrderId"] + "</td>");
                    builder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td>" + row2["Quantity"] + "</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnProductGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要下载配货表的订单", false);
            }
            else
            {
                List<string> list = new List<string>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    list.Add("'" + str2 + "'");
                }
                DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
                builder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
                builder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
                builder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
                if (productGoods.Tables[1].Rows.Count <= 0)
                {
                    builder.AppendLine("<td>商品名称</td>");
                }
                else
                {
                    builder.AppendLine("<td>商品(礼品)名称</td>");
                }
                builder.AppendLine("<td>货号</td>");
                builder.AppendLine("<td>规格</td>");
                builder.AppendLine("<td>拣货数量</td>");
                builder.AppendLine("<td>现库存数</td>");
                builder.AppendLine("</tr>");
                foreach (DataRow row in productGoods.Tables[0].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row["ProductName"] + "</td>");
                    builder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + row["SKU"] + "</td>");
                    builder.AppendLine("<td>" + row["SKUContent"] + "</td>");
                    builder.AppendLine("<td>" + row["ShipmentQuantity"] + "</td>");
                    builder.AppendLine("<td>" + row["Stock"] + "</td>");
                    builder.AppendLine("</tr>");
                }
                foreach (DataRow row2 in productGoods.Tables[1].Rows)
                {
                    builder.AppendLine("<tr>");
                    builder.AppendLine("<td>" + row2["GiftName"] + "[礼品]</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("<td>" + row2["Quantity"] + "</td>");
                    builder.AppendLine("<td></td>");
                    builder.AppendLine("</tr>");
                }
                builder.AppendLine("</table>");
                builder.AppendLine("</body></html>");
                base.Response.Clear();
                base.Response.Buffer = false;
                base.Response.Charset = "GB2312";
                base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                base.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                base.Response.ContentType = "application/ms-excel";
                this.EnableViewState = false;
                base.Response.Write(builder.ToString());
                base.Response.End();
            }
        }

        private void btnRefuseRefund_Click(object sender, EventArgs e)
        {
            string username = HiContext.Current.User.Username;
            OrderHelper.CheckRefund(OrderHelper.GetOrderInfo(this.hidOrderId.Value), username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退款", true);
        }

        private void btnRefuseReplace_Click(object sender, EventArgs e)
        {
            OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单换货", true);
        }

        private void btnRefuseReturn_Click(object sender, EventArgs e)
        {
            OrderHelper.CheckReturn(OrderHelper.GetOrderInfo(this.hidOrderId.Value), HiContext.Current.User.Username, 0M, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
            this.BindOrders();
            this.ShowMsg("成功的拒绝了订单退货", true);
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                Regex regex = new Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
                if (!regex.IsMatch(this.txtRemark.Text))
                {
                    this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
                }
                else
                {
                    OrderInfo order = new OrderInfo();
                    order.OrderId = this.hidOrderId.Value;
                    if (this.orderRemarkImageForRemark.SelectedItem != null)
                    {
                        order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                    }
                    order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                    if (OrderHelper.SaveRemark(order))
                    {
                        this.BindOrders();
                        this.ShowMsg("保存备忘录成功", true);
                    }
                    else
                    {
                        this.ShowMsg("保存失败", false);
                    }
                }
            }
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadOrders(true);
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要发货的订单", false);
            }
            else
            {
                this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + str));
            }
        }

        protected void dlstOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
            if (orderInfo != null)
            {
                if ((e.CommandName == "CONFIRM_PAY") && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
                {
                    int maxCount = 0;
                    int orderCount = 0;
                    int groupBuyOerderNumber = 0;
                    if (orderInfo.CountDownBuyId > 0)
                    {
                        CountDownInfo countDownBuy = TradeHelper.GetCountDownBuy(orderInfo.CountDownBuyId);
                        if ((countDownBuy == null) || (countDownBuy.EndDate < DateTime.Now))
                        {
                            this.ShowMsg("当前的订单为限时抢购订单，此活动已结束，所以不能支付", false);
                            return;
                        }
                    }
                    if (orderInfo.GroupBuyId > 0)
                    {
                        GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
                        if ((groupBuy == null) || (groupBuy.Status != GroupBuyStatus.UnderWay))
                        {
                            this.ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
                            return;
                        }
                        orderCount = PromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
                        maxCount = groupBuy.MaxCount;
                        groupBuyOerderNumber = orderInfo.GetGroupBuyOerderNumber();
                        if (maxCount < (orderCount + groupBuyOerderNumber))
                        {
                            this.ShowMsg("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
                            return;
                        }
                    }
                    if (OrderHelper.ConfirmPay(orderInfo))
                    {
                        DebitNote note = new DebitNote();
                        note.NoteId = Globals.GetGenerateId();
                        note.OrderId = e.CommandArgument.ToString();
                        note.Operator = HiContext.Current.User.Username;
                        note.Remark = "后台" + note.Operator + "收款成功";
                        OrderHelper.SaveDebitNote(note);
                        if ((orderInfo.GroupBuyId > 0) && (maxCount == (orderCount + groupBuyOerderNumber)))
                        {
                            PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
                        }
                        this.BindOrders();
                        int userId = orderInfo.UserId;
                        if (userId == 0x44c)
                        {
                            userId = 0;
                        }
                        Messenger.OrderPayment(Users.GetUser(userId), orderInfo.OrderId, orderInfo.GetTotal());
                        orderInfo.OnPayment();
                        this.ShowMsg("成功的确认了订单收款", true);
                    }
                    else
                    {
                        this.ShowMsg("确认订单收款失败", false);
                    }
                }
                else if ((e.CommandName == "FINISH_TRADE") && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
                {
                    if (OrderHelper.ConfirmOrderFinish(orderInfo))
                    {
                        this.BindOrders();
                        this.ShowMsg("成功的完成了该订单", true);
                    }
                    else
                    {
                        this.ShowMsg("完成订单失败", false);
                    }
                }
            }
        }

        private void dlstOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Item.DataItem, "OrderStatus");
                string str = "";
                if (!(DataBinder.Eval(e.Item.DataItem, "Gateway") is DBNull))
                {
                    str = (string) DataBinder.Eval(e.Item.DataItem, "Gateway");
                }
                int num = (int) DataBinder.Eval(e.Item.DataItem, "GroupBuyId");
                HyperLink link = (HyperLink) e.Item.FindControl("lkbtnEditPrice");
                Label label = (Label) e.Item.FindControl("lkbtnSendGoods");
                ImageLinkButton button = (ImageLinkButton) e.Item.FindControl("lkbtnPayOrder");
                ImageLinkButton button2 = (ImageLinkButton) e.Item.FindControl("lkbtnConfirmOrder");
                Literal literal = (Literal) e.Item.FindControl("litCloseOrder");
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnCheckRefund");
                HtmlAnchor anchor2 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReturn");
                HtmlAnchor anchor3 = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReplace");
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        link.Visible = true;
                        literal.Visible = true;
                        if (str != "hishop.plugins.payment.podrequest")
                        {
                            button.Visible = true;
                        }
                        break;

                    case OrderStatus.ApplyForRefund:
                        anchor.Visible = true;
                        break;

                    case OrderStatus.ApplyForReturns:
                        anchor2.Visible = true;
                        break;

                    case OrderStatus.ApplyForReplacement:
                        anchor3.Visible = true;
                        break;
                }
                if (num > 0)
                {
                    GroupBuyStatus status2 = (GroupBuyStatus) DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
                    label.Visible = ((status == OrderStatus.BuyerAlreadyPaid) || ((status == OrderStatus.WaitBuyerPay) && (str == "hishop.plugins.payment.podrequest"))) && (status2 == GroupBuyStatus.Success);
                }
                else
                {
                    label.Visible = (status == OrderStatus.BuyerAlreadyPaid) || ((status == OrderStatus.WaitBuyerPay) && (str == "hishop.plugins.payment.podrequest"));
                }
                button2.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        private OrderQuery GetOrderQuery()
        {
            int num4;
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out result))
                {
                    query.Status = (OrderStatus) result;
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
            {
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
            {
                int num3 = 0;
                if (int.TryParse(this.Page.Request.QueryString["ModeId"], out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out num4))
            {
                query.RegionId = new int?(num4);
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的订单", false);
            }
            else
            {
                int num = OrderHelper.DeleteOrders("'" + str.Replace(",", "','") + "'");
                this.BindOrders();
                this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int num;
                string str;
                string str2;
                if (string.IsNullOrEmpty(base.Request["orderId"]))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
                StringBuilder builder = new StringBuilder();
                if (base.Request["type"] == "refund")
                {
                    OrderHelper.GetRefundType(base.Request["orderId"], out num, out str2);
                }
                else if (base.Request["type"] == "return")
                {
                    OrderHelper.GetRefundTypeFromReturn(base.Request["orderId"], out num, out str2);
                }
                else
                {
                    num = 0;
                    str2 = "";
                }
                if (num == 1)
                {
                    str = "退到预存款";
                }
                else
                {
                    str = "银行转帐";
                }
                builder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
                if (base.Request["type"] == "replace")
                {
                    string replaceComments = OrderHelper.GetReplaceComments(base.Request["orderId"]);
                    builder.AppendFormat(",\"Comments\":\"{0}\"", replaceComments.Replace("\r\n", ""));
                }
                else
                {
                    builder.AppendFormat(",\"RefundType\":\"{0}\"", num);
                    builder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", str);
                }
                builder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.RealName);
                builder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", orderInfo.TelPhone);
                builder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
                builder.AppendFormat(",\"Remark\":\"{0}\"", str2.Replace("\r\n", ""));
                builder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            this.dlstOrders.ItemDataBound += new DataListItemEventHandler(this.dlstOrders_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.dlstOrders.ItemCommand += new DataListCommandEventHandler(this.dlstOrders_ItemCommand);
            this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
            this.btnCloseOrder.Click += new EventHandler(this.btnCloseOrder_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnOrderGoods.Click += new EventHandler(this.btnOrderGoods_Click);
            this.btnProductGoods.Click += new EventHandler(this.btnProductGoods_Click);
            this.btnAcceptRefund.Click += new EventHandler(this.btnAcceptRefund_Click);
            this.btnRefuseRefund.Click += new EventHandler(this.btnRefuseRefund_Click);
            this.btnAcceptReturn.Click += new EventHandler(this.btnAcceptReturn_Click);
            this.btnRefuseReturn.Click += new EventHandler(this.btnRefuseReturn_Click);
            this.btnAcceptReplace.Click += new EventHandler(this.btnAcceptReplace_Click);
            this.btnRefuseReplace.Click += new EventHandler(this.btnRefuseReplace_Click);
            if (!this.Page.IsPostBack)
            {
                this.shippingModeDropDownList.DataBind();
                this.ddlIsPrinted.Items.Clear();
                this.ddlIsPrinted.Items.Add(new ListItem("全部", string.Empty));
                this.ddlIsPrinted.Items.Add(new ListItem("已打印", "1"));
                this.ddlIsPrinted.Items.Add(new ListItem("未打印", "0"));
                this.SetOrderStatusLink();
                this.BindOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("UserName", this.txtUserName.Text);
            queryStrings.Add("OrderId", this.txtOrderId.Text);
            queryStrings.Add("ProductName", this.txtProductName.Text);
            queryStrings.Add("ShipTo", this.txtShopTo.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("OrderStatus", this.lblStatus.Text);
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            if (this.shippingModeDropDownList.SelectedValue.HasValue)
            {
                queryStrings.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
            {
                queryStrings.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
            }
            if (this.dropRegion.GetSelectedRegionId().HasValue)
            {
                queryStrings.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        private void SetOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus={0}";
            this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
            this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
            this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            this.hlinkClose.NavigateUrl = string.Format(format, 4);
            this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            this.hlinkHistory.NavigateUrl = string.Format(format, 0x63);
        }
    }
}

