namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Net;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PurchaseorderSendGood)]
    public class SendPurchaseOrderGoods : AdminPage
    {
        protected Button btnSendGoods;
        protected ExpressRadioButtonList expressRadioButtonList;
        protected PurchaseOrder_Items itemsList;
        protected Literal lblModeName;
        protected Literal litDiscount;
        protected Literal litFreight;
        protected Literal litReceivingInfo;
        protected Literal litRemark;
        protected Literal litShippingModeName;
        protected Label litShipToDate;
        protected Literal litTotalPrice;
        private string purchaseorderId;
        protected ShippingModeRadioButtonList radioShippingMode;
        protected TextBox txtShipOrderNumber;

        private void BindCharges(PurchaseOrderInfo purchaseOrder)
        {
            this.lblModeName.Text = purchaseOrder.ModeName;
            this.litFreight.Text = Globals.FormatMoney(purchaseOrder.AdjustedFreight);
            this.litDiscount.Text = Globals.FormatMoney(purchaseOrder.AdjustedDiscount);
            this.litTotalPrice.Text = Globals.FormatMoney(purchaseOrder.GetPurchaseTotal());
        }

        private void BindExpressCompany(int modeId)
        {
            this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
            this.expressRadioButtonList.DataBind();
        }

        private void BindShippingAddress(PurchaseOrderInfo purchaseorder)
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(purchaseorder.ShippingRegion))
            {
                shippingRegion = purchaseorder.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(purchaseorder.Address))
            {
                shippingRegion = shippingRegion + purchaseorder.Address;
            }
            if (!string.IsNullOrEmpty(purchaseorder.ZipCode))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.ZipCode;
            }
            if (!string.IsNullOrEmpty(purchaseorder.ShipTo))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.ShipTo;
            }
            if (!string.IsNullOrEmpty(purchaseorder.TelPhone))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.TelPhone;
            }
            if (!string.IsNullOrEmpty(purchaseorder.CellPhone))
            {
                shippingRegion = shippingRegion + "," + purchaseorder.CellPhone;
            }
            this.litReceivingInfo.Text = shippingRegion;
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()))
            {
                this.ShowMsg("请填写运单号", false);
            }
            else
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
                if (purchaseOrder != null)
                {
                    if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
                    {
                        this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                    }
                    else if (!this.radioShippingMode.SelectedValue.HasValue)
                    {
                        this.ShowMsg("请选择配送方式", false);
                    }
                    else if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
                    {
                        this.ShowMsg("请选择物流配送公司", false);
                    }
                    else
                    {
                        ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
                        purchaseOrder.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                        purchaseOrder.RealModeName = shippingMode.Name;
                        ExpressCompanyInfo info3 = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
                        if (info3 != null)
                        {
                            purchaseOrder.ExpressCompanyAbb = info3.Kuaidi100Code;
                            purchaseOrder.ExpressCompanyName = info3.Name;
                        }
                        purchaseOrder.ShipOrderNumber = this.txtShipOrderNumber.Text;
                        if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
                        {
                            SendNote note = new SendNote();
                            note.NoteId = Globals.GetGenerateId();
                            note.OrderId = this.purchaseorderId;
                            note.Operator = HiContext.Current.User.Username;
                            note.Remark = "后台" + note.Operator + "发货成功";
                            SalesHelper.SavePurchaseSendNote(note);
                            if (!string.IsNullOrEmpty(purchaseOrder.TaobaoOrderId))
                            {
                                try
                                {
                                    ExpressCompanyInfo info4 = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
                                    WebRequest.Create(string.Format("http://order1.kuaidiangtong.com/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", purchaseOrder.TaobaoOrderId, info4.TaobaoCode, purchaseOrder.ShipOrderNumber)).GetResponse();
                                }
                                catch
                                {
                                }
                            }
                            this.ShowMsg("发货成功", true);
                        }
                        else
                        {
                            this.ShowMsg("发货失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseorderId = this.Page.Request.QueryString["PurchaseOrderId"];
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
                if (purchaseOrder == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.itemsList.PurchaseOrder = purchaseOrder;
                    this.btnSendGoods.Click += new EventHandler(this.btnSendGoods_Click);
                    this.radioShippingMode.SelectedIndexChanged += new EventHandler(this.radioShippingMode_SelectedIndexChanged);
                    if (!this.Page.IsPostBack)
                    {
                        this.radioShippingMode.DataBind();
                        this.radioShippingMode.SelectedValue = new int?(purchaseOrder.ShippingModeId);
                        this.BindExpressCompany(purchaseOrder.ShippingModeId);
                        this.expressRadioButtonList.SelectedValue = purchaseOrder.ExpressCompanyAbb;
                        this.BindShippingAddress(purchaseOrder);
                        this.BindCharges(purchaseOrder);
                        this.txtShipOrderNumber.Text = purchaseOrder.ShipOrderNumber;
                        this.litShippingModeName.Text = purchaseOrder.ModeName;
                        this.litShipToDate.Text = purchaseOrder.ShipToDate;
                        this.litRemark.Text = purchaseOrder.Remark;
                    }
                }
            }
        }

        private void radioShippingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioShippingMode.SelectedValue.HasValue)
            {
                this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
            }
        }
    }
}

