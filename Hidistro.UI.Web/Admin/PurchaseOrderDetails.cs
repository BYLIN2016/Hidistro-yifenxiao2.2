namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public class PurchaseOrderDetails : AdminPage
    {
        protected Button btnClosePurchaseOrder;
        protected Button btnEditOrder;
        protected Button btnMondifyShip;
        protected Button btnRemark;
        protected PurchaseOrder_Charges chargesList;
        protected ClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected ShippingModeDropDownList ddlshippingMode;
        protected HtmlInputHidden hdpurchaseorder;
        protected PurchaseOrder_Items itemsList;
        protected Label lbCloseReason;
        protected FormatedTimeLabel lblpurchaseDateForRemark;
        protected Label lblPurchaseOrderAmount;
        protected Label lblPurchaseOrderAmount1;
        protected Label lblPurchaseOrderAmount2;
        protected Label lblPurchaseOrderAmount3;
        protected PuchaseStatusLabel lblPurchaseOrderStatus;
        protected FormatedMoneyLabel lblpurchaseTotalForRemark;
        protected Label lbReason;
        protected HtmlAnchor lbtnClocsOrder;
        protected Literal litFinishTime;
        protected Literal litOrderId;
        protected Literal litPayTime;
        protected Literal litPurchaseOrderId;
        protected Literal litRealName;
        protected Literal litSendGoodTime;
        protected Literal litUserEmail;
        protected Literal litUserName;
        protected Literal litUserTel;
        protected HtmlAnchor lkbtnEditPrice;
        protected HyperLink lkbtnSendGoods;
        protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
        private PurchaseOrderInfo purchaseOrder;
        private string purchaseOrderId;
        protected PurchaseOrder_ShippingAddress shippingAddress;
        protected Literal spanOrderId;
        protected Literal spanpurcharseOrderId;
        protected TextBox txtPurchaseOrderDiscount;
        protected TextBox txtRemark;

        private void BindEditOrderPrice(PurchaseOrderInfo purchaseOrder)
        {
            this.lblPurchaseOrderAmount.Text = (purchaseOrder.GetPurchaseTotal() - purchaseOrder.AdjustedDiscount).ToString("F", CultureInfo.InvariantCulture);
            this.lblPurchaseOrderAmount1.Text = this.lblPurchaseOrderAmount.Text;
            this.lblPurchaseOrderAmount2.Text = purchaseOrder.AdjustedDiscount.ToString("F", CultureInfo.InvariantCulture);
            this.lblPurchaseOrderAmount3.Text = purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture);
        }

        private void BindRemark(PurchaseOrderInfo purchaseOrder)
        {
            this.spanOrderId.Text = purchaseOrder.OrderId;
            this.spanpurcharseOrderId.Text = purchaseOrder.PurchaseOrderId;
            this.lblpurchaseDateForRemark.Time = purchaseOrder.PurchaseDate;
            this.lblpurchaseTotalForRemark.Money = purchaseOrder.GetPurchaseTotal();
            this.txtRemark.Text = Globals.HtmlDecode(purchaseOrder.ManagerRemark);
            this.orderRemarkImageForRemark.SelectedValue = purchaseOrder.ManagerMark;
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            this.purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
            if (SalesHelper.ClosePurchaseOrder(this.purchaseOrder))
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId={0}", this.purchaseOrder.PurchaseOrderId));
            }
            else
            {
                this.ShowMsg("取消采购失败", false);
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            decimal num;
            if (!decimal.TryParse(this.txtPurchaseOrderDiscount.Text.Trim(), out num))
            {
                this.ShowMsg("请正确填写打折或者涨价金额", false);
            }
            else
            {
                this.purchaseOrder.AdjustedDiscount = num;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseOrderInfo>(this.purchaseOrder, new string[] { "ValPurchaseOrder" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                        this.ShowMsg(msg, false);
                        return;
                    }
                }
                if (this.purchaseOrder.GetPurchaseTotal() >= 0M)
                {
                    if (SalesHelper.UpdatePurchaseOrderAmount(this.purchaseOrder))
                    {
                        this.chargesList.LoadControl();
                        this.BindEditOrderPrice(this.purchaseOrder);
                        this.ShowMsg("修改成功", true);
                    }
                    else
                    {
                        this.ShowMsg("修改失败", false);
                    }
                }
                else
                {
                    this.ShowMsg("折扣值不能使得采购单总金额为负", false);
                }
            }
        }

        private void btnMondifyShip_Click(object sender, EventArgs e)
        {
            ShippingModeInfo shippingMode = new ShippingModeInfo();
            shippingMode = SalesHelper.GetShippingMode(this.ddlshippingMode.SelectedValue.Value, false);
            this.purchaseOrder.ShippingModeId = shippingMode.ModeId;
            this.purchaseOrder.ModeName = shippingMode.Name;
            if (SalesHelper.UpdatePurchaseOrderShippingMode(this.purchaseOrder))
            {
                this.chargesList.LoadControl();
                this.shippingAddress.LoadControl();
                this.ShowMsg("修改配送方式成功", true);
            }
            else
            {
                this.ShowMsg("修改配送方式失败", false);
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (this.txtRemark.Text.Length > 300)
            {
                this.ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                this.purchaseOrder.PurchaseOrderId = this.spanpurcharseOrderId.Text;
                if (this.orderRemarkImageForRemark.SelectedItem != null)
                {
                    this.purchaseOrder.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
                }
                this.purchaseOrder.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
                if (SalesHelper.SavePurchaseOrderRemark(this.purchaseOrder))
                {
                    this.BindRemark(this.purchaseOrder);
                    this.ShowMsg("保存备忘录成功", true);
                }
                else
                {
                    this.ShowMsg("保存失败", false);
                }
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
                this.btnMondifyShip.Click += new EventHandler(this.btnMondifyShip_Click);
                this.btnEditOrder.Click += new EventHandler(this.btnEditOrder_Click);
                this.btnRemark.Click += new EventHandler(this.btnRemark_Click);
                this.purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
                this.LoadUserControl(this.purchaseOrder);
                if (!base.IsPostBack)
                {
                    this.lblPurchaseOrderStatus.PuchaseStatusCode = this.purchaseOrder.PurchaseStatus;
                    this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
                    this.litUserName.Text = this.purchaseOrder.Distributorname;
                    this.litRealName.Text = this.purchaseOrder.DistributorRealName;
                    this.litUserTel.Text = this.purchaseOrder.TelPhone;
                    this.litUserEmail.Text = this.purchaseOrder.DistributorEmail;
                    if (((int) this.lblPurchaseOrderStatus.PuchaseStatusCode) != 4)
                    {
                        this.lbCloseReason.Visible = false;
                    }
                    else
                    {
                        this.lbReason.Text = this.purchaseOrder.CloseReason;
                    }
                    if (!string.IsNullOrEmpty(this.purchaseOrder.OrderId))
                    {
                        this.litOrderId.Text = "对应的子站订单编号：" + this.purchaseOrder.OrderId;
                    }
                    if (((this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay) && (this.purchaseOrder.PurchaseStatus != OrderStatus.Closed)) && (this.purchaseOrder.Gateway != "hishop.plugins.payment.podrequest"))
                    {
                        this.litPayTime.Text = "付款时间：" + this.purchaseOrder.PayDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (((this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent) || (this.purchaseOrder.PurchaseStatus == OrderStatus.Finished)) || (((this.purchaseOrder.PurchaseStatus == OrderStatus.Returned) || (this.purchaseOrder.PurchaseStatus == OrderStatus.ApplyForReturns)) || (this.purchaseOrder.PurchaseStatus == OrderStatus.ApplyForReplacement)))
                    {
                        this.litSendGoodTime.Text = "发货时间：" + this.purchaseOrder.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if (this.purchaseOrder.PurchaseStatus == OrderStatus.Finished)
                    {
                        this.litFinishTime.Text = "完成时间：" + this.purchaseOrder.FinishDate.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    if ((this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid) || ((this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) && (this.purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")))
                    {
                        this.lkbtnSendGoods.Visible = true;
                    }
                    else
                    {
                        this.lkbtnSendGoods.Visible = false;
                    }
                    if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
                    {
                        this.lbtnClocsOrder.Visible = true;
                        this.lkbtnEditPrice.Visible = true;
                    }
                    else
                    {
                        this.lbtnClocsOrder.Visible = false;
                        this.lkbtnEditPrice.Visible = false;
                    }
                    this.hdpurchaseorder.Value = this.purchaseOrderId;
                    this.BindEditOrderPrice(this.purchaseOrder);
                    this.BindRemark(this.purchaseOrder);
                    this.ddlshippingMode.DataBind();
                    this.ddlshippingMode.SelectedValue = new int?(this.purchaseOrder.ShippingModeId);
                }
            }
        }
    }
}

