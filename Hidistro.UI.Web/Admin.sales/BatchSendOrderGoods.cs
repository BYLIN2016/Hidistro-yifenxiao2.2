namespace Hidistro.UI.Web.Admin.sales
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using System;
    using System.Web.UI.WebControls;

    public class BatchSendOrderGoods : AdminPage
    {
        protected Button btnBatchSendGoods;
        protected Button btnSetExpressComputerpe;
        protected Button btnSetShipOrderNumber;
        protected Button btnSetShippingMode;
        protected DropdownColumn dropExpress;
        protected DropDownList dropExpressComputerpe;
        protected DropdownColumn dropShippId;
        protected ShippingModeDropDownList dropShippingMode;
        protected Grid grdOrderGoods;
        private string strIds;
        protected TextBox txtStartShipOrderNumber;

        private void BindData()
        {
            DropdownColumn column = (DropdownColumn) this.grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DropdownColumn column2 = (DropdownColumn) this.grdOrderGoods.Columns[5];
            column2.DataSource = ExpressHelper.GetAllExpress();
            string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
            this.grdOrderGoods.DataSource = OrderHelper.GetSendGoodsOrders(orderIds);
            this.grdOrderGoods.DataBind();
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
                DropdownColumn column2 = (DropdownColumn) this.grdOrderGoods.Columns[5];
                ListItemCollection items2 = column2.SelectedItems;
                int num = 0;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    string orderId = (string) this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox) this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    ListItem item2 = items2[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    if ((!string.IsNullOrEmpty(box.Text.Trim()) && !string.IsNullOrEmpty(item.Value)) && ((int.Parse(item.Value) > 0) && !string.IsNullOrEmpty(item2.Value)))
                    {
                        OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                        if ((((orderInfo.GroupBuyId <= 0) || (orderInfo.GroupBuyStatus == GroupBuyStatus.Success)) && (((orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) && (orderInfo.Gateway == "hishop.plugins.payment.podrequest")) || (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))) && (((result > 0) && !string.IsNullOrEmpty(box.Text.Trim())) && (box.Text.Trim().Length <= 20)))
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(result, true);
                            orderInfo.RealShippingModeId = shippingMode.ModeId;
                            orderInfo.RealModeName = shippingMode.Name;
                            orderInfo.ExpressCompanyAbb = item2.Value;
                            orderInfo.ExpressCompanyName = item2.Text;
                            orderInfo.ShipOrderNumber = box.Text;
                            if (OrderHelper.SendGoods(orderInfo))
                            {
                                SendNote note = new SendNote();
                                note.NoteId = Globals.GetGenerateId() + num;
                                note.OrderId = orderId;
                                note.Operator = HiContext.Current.User.Username;
                                note.Remark = "后台" + note.Operator + "发货成功";
                                OrderHelper.SaveSendNote(note);
                                if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                                {
                                    PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
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
                                num++;
                            }
                        }
                    }
                }
                if (num == 0)
                {
                    this.ShowMsg("批量发货失败！", false);
                }
                else if (num > 0)
                {
                    this.BindData();
                    this.ShowMsg(string.Format("批量发货成功！发货数量{0}个", num), true);
                }
            }
        }

        private void btnSetExpressComputerpe_Click(object sender, EventArgs e)
        {
            string purchaseOrderIds = "'" + this.strIds.Replace(",", "','") + "'";
            if (!string.IsNullOrEmpty(this.dropExpressComputerpe.SelectedValue))
            {
                OrderHelper.SetOrderExpressComputerpe(purchaseOrderIds, this.dropExpressComputerpe.SelectedItem.Text, this.dropExpressComputerpe.SelectedValue);
            }
            this.BindData();
        }

        private void btnSetShipOrderNumber_Click(object sender, EventArgs e)
        {
            string[] orderIds = this.strIds.Split(new char[] { ',' });
            long result = 0L;
            if (!string.IsNullOrEmpty(this.txtStartShipOrderNumber.Text.Trim()) && long.TryParse(this.txtStartShipOrderNumber.Text.Trim(), out result))
            {
                OrderHelper.SetOrderShipNumber(orderIds, this.txtStartShipOrderNumber.Text.Trim());
                this.BindData();
            }
            else
            {
                this.ShowMsg("起始发货单号不允许为空且必须为正整数", false);
            }
        }

        private void btnSetShippingMode_Click(object sender, EventArgs e)
        {
            string purchaseOrderIds = "'" + this.strIds.Replace(",", "','") + "'";
            if (this.dropShippingMode.SelectedValue.HasValue)
            {
                OrderHelper.SetOrderShippingMode(purchaseOrderIds, this.dropShippingMode.SelectedValue.Value, this.dropShippingMode.SelectedItem.Text);
            }
            this.BindData();
        }

        private void grdOrderGoods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string orderId = (string) this.grdOrderGoods.DataKeys[e.Row.RowIndex].Value;
                DropDownList list = e.Row.FindControl("dropExpress") as DropDownList;
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                if ((orderInfo != null) && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                {
                    ExpressCompanyInfo info2 = ExpressHelper.FindNode(orderInfo.ExpressCompanyName);
                    if (info2 != null)
                    {
                        list.SelectedValue = info2.Kuaidi100Code;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.strIds = base.Request.QueryString["OrderIds"];
            this.btnSetShippingMode.Click += new EventHandler(this.btnSetShippingMode_Click);
            this.btnSetExpressComputerpe.Click += new EventHandler(this.btnSetExpressComputerpe_Click);
            this.btnSetShipOrderNumber.Click += new EventHandler(this.btnSetShipOrderNumber_Click);
            this.grdOrderGoods.RowDataBound += new GridViewRowEventHandler(this.grdOrderGoods_RowDataBound);
            this.btnBatchSendGoods.Click += new EventHandler(this.btnSendGoods_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropShippingMode.DataBind();
                this.dropExpressComputerpe.DataSource = ExpressHelper.GetAllExpress();
                this.dropExpressComputerpe.DataTextField = "name";
                this.dropExpressComputerpe.DataValueField = "Kuaidi100Code";
                this.dropExpressComputerpe.DataBind();
                this.dropExpressComputerpe.Items.Insert(0, new ListItem("", ""));
                this.BindData();
            }
        }
    }
}

