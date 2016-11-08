namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Net;
    using System.Web.UI.WebControls;

    public class BatchSendPurchaseOrderGoods : AdminPage
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
            string purchaseOrderIds = "'" + this.strIds.Replace(",", "','") + "'";
            this.grdOrderGoods.DataSource = SalesHelper.GetSendGoodsPurchaseOrders(purchaseOrderIds);
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
                    string purchaseOrderId = (string) this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox) this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    ListItem item2 = items2[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    if ((!string.IsNullOrEmpty(box.Text.Trim()) && (box.Text.Length <= 20)) && (result > 0))
                    {
                        PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                        if (((purchaseOrder != null) && ((purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid) || ((purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) && (purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")))) && !string.IsNullOrEmpty(item2.Value))
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(int.Parse(item.Value), true);
                            purchaseOrder.RealShippingModeId = shippingMode.ModeId;
                            purchaseOrder.RealModeName = shippingMode.Name;
                            purchaseOrder.ExpressCompanyAbb = item2.Value;
                            purchaseOrder.ExpressCompanyName = item2.Text;
                            purchaseOrder.ShipOrderNumber = box.Text;
                            if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
                            {
                                SendNote note = new SendNote();
                                note.NoteId = Globals.GetGenerateId() + num;
                                note.OrderId = purchaseOrderId;
                                note.Operator = HiContext.Current.User.Username;
                                note.Remark = "后台" + note.Operator + "发货成功";
                                SalesHelper.SavePurchaseSendNote(note);
                                if (!string.IsNullOrEmpty(purchaseOrder.TaobaoOrderId))
                                {
                                    try
                                    {
                                        ExpressCompanyInfo info3 = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
                                        WebRequest.Create(string.Format("http://order1.kuaidiangtong.com/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", purchaseOrder.TaobaoOrderId, info3.TaobaoCode, purchaseOrder.ShipOrderNumber)).GetResponse();
                                    }
                                    catch
                                    {
                                    }
                                }
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
            string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
            if (!string.IsNullOrEmpty(this.dropExpressComputerpe.SelectedValue))
            {
                SalesHelper.SetPurchaseOrderExpressComputerpe(orderIds, this.dropExpressComputerpe.SelectedItem.Text, this.dropExpressComputerpe.SelectedValue);
            }
            this.BindData();
        }

        private void btnSetShipOrderNumber_Click(object sender, EventArgs e)
        {
            string[] orderIds = this.strIds.Split(new char[] { ',' });
            long result = 0L;
            if (!string.IsNullOrEmpty(this.txtStartShipOrderNumber.Text.Trim()) && long.TryParse(this.txtStartShipOrderNumber.Text.Trim(), out result))
            {
                SalesHelper.SetPurchaseOrderShipNumber(orderIds, this.txtStartShipOrderNumber.Text.Trim());
                this.BindData();
            }
            else
            {
                this.ShowMsg("起始发货单号不允许为空且必须为正整数", false);
            }
        }

        private void btnSetShippingMode_Click(object sender, EventArgs e)
        {
            string orderIds = "'" + this.strIds.Replace(",", "','") + "'";
            if (this.dropShippingMode.SelectedValue.HasValue)
            {
                SalesHelper.SetPurchaseOrderShippingMode(orderIds, this.dropShippingMode.SelectedValue.Value, this.dropShippingMode.SelectedItem.Text);
            }
            this.BindData();
        }

        private void grdOrderGoods_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string purchaseOrderId = (string) this.grdOrderGoods.DataKeys[e.Row.RowIndex].Value;
                DropDownList list = e.Row.FindControl("dropExpress") as DropDownList;
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                if ((purchaseOrder != null) && (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid))
                {
                    ExpressCompanyInfo info2 = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
                    if (info2 != null)
                    {
                        list.SelectedValue = info2.Kuaidi100Code;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderIds"]))
            {
                this.strIds = base.Request.QueryString["PurchaseOrderIds"];
            }
            if (this.strIds.Length > 0)
            {
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
}

