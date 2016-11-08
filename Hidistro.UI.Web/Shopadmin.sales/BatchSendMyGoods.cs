namespace Hidistro.UI.Web.Shopadmin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using System;
    using System.Web.UI.WebControls;

    public class BatchSendMyGoods : DistributorPage
    {
        protected Button btnBatchSendGoods;
        protected DropdownColumn dropShippId;
        protected Grid grdOrderGoods;
        protected Pager pager1;
        protected Pager pager2;

        private void BindData()
        {
            DropdownColumn column = (DropdownColumn) this.grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DbQueryResult sendGoodsOrders = SubsiteSalesHelper.GetSendGoodsOrders(this.GetOrderQuery());
            this.grdOrderGoods.DataSource = sendGoodsOrders.Data;
            this.grdOrderGoods.DataBind();
            this.pager2.TotalRecords = this.pager1.TotalRecords = sendGoodsOrders.TotalRecords;
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            if (this.grdOrderGoods.Rows.Count <= 0)
            {
                this.ShowMsg("没有要进行发货的订单。", false);
            }
            else
            {
                DropdownColumn column = (DropdownColumn) this.grdOrderGoods.Columns[4];
                ListItemCollection selectedItems = column.SelectedItems;
                int num = 0;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    string orderId = (string) this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox) this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
                    if (((orderInfo != null) && ((orderInfo.GroupBuyId <= 0) || (orderInfo.GroupBuyStatus == GroupBuyStatus.Success))) && (((orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && (result > 0)) && (!string.IsNullOrEmpty(box.Text) && (box.Text.Length <= 20))))
                    {
                        ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(result, true);
                        orderInfo.RealShippingModeId = shippingMode.ModeId;
                        orderInfo.RealModeName = shippingMode.Name;
                        orderInfo.ShipOrderNumber = box.Text;
                        if (SubsiteSalesHelper.SendGoods(orderInfo))
                        {
                            if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                            {
                                PaymentModeInfo paymentMode = SubsiteSalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                if (paymentMode != null)
                                {
                                    PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                }
                            }
                            int userId = orderInfo.UserId;
                            if (userId == 0x44c)
                            {
                                userId = 0;
                            }
                            IUser user = Users.GetUser(userId);
                            Messenger.OrderShipping(orderInfo, user);
                            orderInfo.OnDeliver();
                        }
                        num++;
                    }
                }
                if (num == 0)
                {
                    this.ShowMsg("批量发货失败！，发货数量0个", false);
                }
                else if (num > 0)
                {
                    this.BindData();
                    this.ShowMsg(string.Format("批量发货成功！，发货数量{0}个", num), true);
                }
            }
        }

        private OrderQuery GetOrderQuery()
        {
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
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
            }
            query.PageIndex = this.pager1.PageIndex;
            query.PageSize = this.pager1.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnBatchSendGoods.Click += new EventHandler(this.btnSendGoods_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

