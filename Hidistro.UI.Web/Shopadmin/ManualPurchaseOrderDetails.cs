namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManualPurchaseOrderDetails : DistributorPage
    {
        protected Button btnClosePurchaseOrder;
        protected PurchaseOrder_Charges chargesList;
        protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected HyperLink hlkOrderGifts;
        protected ManualPurchaseOrder_Items itemsList;
        protected Label lbCloseReason;
        protected FormatedMoneyLabel lblPurchaseOrderRefundMoney;
        protected PuchaseStatusLabel lblPurchaseStatus;
        protected Label lbPurchaseOrderReturn;
        protected Label lbReason;
        protected Literal litPurchaseOrderId;
        protected HtmlAnchor lkbtnClosePurchaseOrder;
        protected HyperLink lkbtnPay;
        private PurchaseOrderInfo purchaseOrder;
        private string purchaseOrderId;
        protected PurchaseOrder_ShippingAddress shippingAddress;

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = this.purchaseOrder;
            purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
            if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
            {
                this.ShowMsg("取消采购成功", true);
            }
            else
            {
                this.ShowMsg("取消采购失败", false);
            }
        }

        private void LoadUserControl(PurchaseOrderInfo purchaseOrder)
        {
            this.itemsList.PurchaseOrder = purchaseOrder;
            this.chargesList.PurchaseOrder = purchaseOrder;
            this.shippingAddress.PurchaseOrder = purchaseOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["purchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.purchaseOrderId = this.Page.Request.QueryString["purchaseOrderId"];
                this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
                this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                this.LoadUserControl(this.purchaseOrder);
                if (!base.IsPostBack)
                {
                    if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
                    {
                        this.lkbtnClosePurchaseOrder.Visible = true;
                        this.lkbtnPay.Visible = true;
                        this.lkbtnPay.NavigateUrl = string.Concat(new object[] { Globals.ApplicationPath, "/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId=", this.purchaseOrder.PurchaseOrderId, "&PayMode=", this.purchaseOrder.PaymentTypeId });
                    }
                    else
                    {
                        this.lkbtnClosePurchaseOrder.Visible = false;
                        this.lkbtnPay.Visible = false;
                    }
                    this.lblPurchaseStatus.PuchaseStatusCode = this.purchaseOrder.PurchaseStatus;
                    this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
                    if (((int) this.lblPurchaseStatus.PuchaseStatusCode) != 4)
                    {
                        this.lbCloseReason.Visible = false;
                    }
                    else
                    {
                        this.lbReason.Text = this.purchaseOrder.CloseReason;
                    }
                    if (((int) this.lblPurchaseStatus.PuchaseStatusCode) != 10)
                    {
                        this.lbPurchaseOrderReturn.Visible = false;
                    }
                    else
                    {
                        decimal num;
                        this.lblPurchaseOrderRefundMoney.Money = SubsiteSalesHelper.GetRefundMoney(this.purchaseOrder, out num);
                    }
                    if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
                    {
                        if (this.purchaseOrder.PurchaseOrderGifts.Count > 0)
                        {
                            this.hlkOrderGifts.Text = "编辑礼品";
                        }
                        this.hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/PurchaseOrderGifts.aspx?PurchaseOrderId=" + this.purchaseOrder.PurchaseOrderId;
                    }
                    else
                    {
                        this.hlkOrderGifts.Visible = false;
                    }
                }
            }
        }
    }
}

