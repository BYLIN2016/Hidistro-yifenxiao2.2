namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
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
    using System.Runtime.InteropServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.EditOrders)]
    public class EditOrder : AdminPage
    {
        protected Button btnUpdateOrderAmount;
        protected FormatedMoneyLabel couponAmount;
        protected FormatedMoneyLabel fullDiscountAmount;
        protected GridView grdOrderGift;
        protected Grid grdProducts;
        protected HyperLink hlkSentTimesPointPromotion;
        protected FormatedMoneyLabel lblAllPrice;
        protected Label lblWeight;
        protected Literal litIntegral;
        protected Literal litInvoiceTitle;
        protected Literal litPayName;
        protected Literal litShipModeName;
        protected Literal litTax;
        protected Literal litTotal;
        protected HyperLink lkbtnFullDiscount;
        protected HyperLink lkbtnFullFree;
        private OrderInfo order;
        private string orderId;
        protected TextBox txtAdjustedDiscount;
        protected TextBox txtAdjustedFreight;
        protected TextBox txtAdjustedPayCharge;

        private void BindOtherAmount(OrderInfo order)
        {
            if (order.IsReduced)
            {
                this.fullDiscountAmount.Money = order.ReducedPromotionAmount;
                this.lkbtnFullDiscount.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.ReducedPromotionId });
                this.lkbtnFullDiscount.Text = order.ReducedPromotionName;
            }
            if (order.IsFreightFree)
            {
                this.lkbtnFullFree.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.FreightFreePromotionId });
                this.lkbtnFullFree.Text = order.FreightFreePromotionName;
            }
            if (order.IsSendTimesPoint)
            {
                this.hlkSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
                this.hlkSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[] { order.SentTimesPointPromotionId });
            }
            this.txtAdjustedFreight.Text = order.AdjustedFreight.ToString("F", CultureInfo.InvariantCulture);
            this.txtAdjustedPayCharge.Text = order.PayCharge.ToString("F", CultureInfo.InvariantCulture);
            this.txtAdjustedDiscount.Text = order.AdjustedDiscount.ToString("F", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(order.PaymentType))
            {
                this.litPayName.Text = "(" + order.PaymentType + ")";
            }
            if (!string.IsNullOrEmpty(order.CouponName))
            {
                this.couponAmount.Text = "[" + order.CouponName + "]-" + Globals.FormatMoney(order.CouponValue);
            }
            else
            {
                this.couponAmount.Text = "-" + Globals.FormatMoney(order.CouponValue);
            }
            if (order.Tax > 0M)
            {
                this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(order.Tax);
                this.litTax.Text = this.litTax.Text + "</span></td></tr>";
            }
            if (order.InvoiceTitle.Length > 0)
            {
                this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + order.InvoiceTitle;
                this.litInvoiceTitle.Text = this.litInvoiceTitle.Text + "</span></td></tr>";
            }
        }

        private void BindProductList(OrderInfo order)
        {
            this.grdProducts.DataSource = order.LineItems.Values;
            this.grdProducts.DataBind();
            this.grdOrderGift.DataSource = order.Gifts;
            this.grdOrderGift.DataBind();
        }

        private void BindTatolAmount(OrderInfo order)
        {
            decimal amount = order.GetAmount();
            this.lblAllPrice.Money = amount;
            this.lblWeight.Text = order.Weight.ToString(CultureInfo.InvariantCulture);
            this.litIntegral.Text = order.Points.ToString(CultureInfo.InvariantCulture);
            this.litTotal.Text = Globals.FormatMoney(order.GetTotal());
        }

        private void btnUpdateOrderAmount_Click(object sender, EventArgs e)
        {
            if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else
            {
                decimal num;
                decimal num2;
                decimal num3;
                if (this.ValidateValues(out num, out num2, out num3))
                {
                    string msg = string.Empty;
                    this.order.AdjustedFreight = num;
                    this.order.PayCharge = num2;
                    this.order.AdjustedDiscount = num3;
                    decimal total = this.order.GetTotal();
                    ValidationResults results = Hishop.Components.Validation.Validation.Validate<OrderInfo>(this.order, new string[] { "ValOrder" });
                    if (!results.IsValid)
                    {
                        foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                        {
                            msg = msg + Formatter.FormatErrorMessage(result.Message);
                            this.ShowMsg(msg, false);
                            return;
                        }
                    }
                    if (total > 0M)
                    {
                        if (OrderHelper.UpdateOrderAmount(this.order))
                        {
                            this.BindTatolAmount(this.order);
                            this.ShowMsg("成功的修改了订单金额", true);
                        }
                        else
                        {
                            this.ShowMsg("修改订单金额失败", false);
                        }
                    }
                    else
                    {
                        this.ShowMsg("订单的应付总金额不应该是负数,请重新输入订单折扣", false);
                    }
                }
            }
        }

        private void grdOrderGift_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else
            {
                int giftId = (int) this.grdOrderGift.DataKeys[e.RowIndex].Value;
                if (OrderHelper.DeleteOrderGift(this.order, giftId))
                {
                    this.order = OrderHelper.GetOrderInfo(this.orderId);
                    this.BindProductList(this.order);
                    this.BindTatolAmount(this.order);
                    this.ShowMsg("成功删除了一件礼品", true);
                }
                else
                {
                    this.ShowMsg("删除礼品失败", false);
                }
            }
        }

        private void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            OrderInfo order = this.order;
            if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else if (e.CommandName == "setQuantity")
            {
                int num2;
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                string skuId = this.grdProducts.DataKeys[rowIndex].Value.ToString();
                TextBox box = this.grdProducts.Rows[rowIndex].FindControl("txtQuantity") as TextBox;
                int quantity = order.LineItems[skuId].Quantity;
                if (!int.TryParse(box.Text.Trim(), out num2))
                {
                    this.ShowMsg("商品数量填写错误", false);
                }
                else if (num2 > OrderHelper.GetSkuStock(skuId))
                {
                    this.ShowMsg("此商品库存不够", false);
                }
                else if (num2 <= 0)
                {
                    this.ShowMsg("商品购买数量不能为0", false);
                }
                else
                {
                    order.LineItems[skuId].Quantity = num2;
                    order.LineItems[skuId].ShipmentQuantity = num2;
                    order.LineItems[skuId].ItemAdjustedPrice = order.LineItems[skuId].ItemListPrice;
                    if (order.GetTotal() < 0M)
                    {
                        this.ShowMsg("订单总金额不允许小于0", false);
                    }
                    else if (quantity != num2)
                    {
                        if (OrderHelper.UpdateLineItem(skuId, this.order, num2))
                        {
                            this.BindProductList(this.order);
                            this.BindTatolAmount(this.order);
                            this.ShowMsg("修改商品购买数量成功", true);
                        }
                        else
                        {
                            this.ShowMsg("修改商品购买数量失败", false);
                        }
                    }
                }
            }
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
            {
                this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
            }
            else if (this.order.LineItems.Values.Count <= 1)
            {
                this.ShowMsg("订单的最后一件商品不允许删除", false);
            }
            else if (OrderHelper.DeleteLineItem(this.grdProducts.DataKeys[e.RowIndex].Value.ToString(), this.order))
            {
                this.order = OrderHelper.GetOrderInfo(this.orderId);
                this.BindProductList(this.order);
                this.BindTatolAmount(this.order);
                this.ShowMsg("成功删除了一件商品", true);
            }
            else
            {
                this.ShowMsg("删除商品失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.orderId = this.Page.Request.QueryString["OrderId"];
                this.btnUpdateOrderAmount.Click += new EventHandler(this.btnUpdateOrderAmount_Click);
                this.grdOrderGift.RowDeleting += new GridViewDeleteEventHandler(this.grdOrderGift_RowDeleting);
                this.grdProducts.RowDeleting += new GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
                this.grdProducts.RowCommand += new GridViewCommandEventHandler(this.grdProducts_RowCommand);
                this.order = OrderHelper.GetOrderInfo(this.orderId);
                if (!this.Page.IsPostBack)
                {
                    if (this.order == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        this.BindProductList(this.order);
                        this.BindOtherAmount(this.order);
                        this.BindTatolAmount(this.order);
                    }
                }
            }
        }

        private bool ValidateValues(out decimal adjustedFreight, out decimal adjustedPayCharge, out decimal discountAmout)
        {
            string str = string.Empty;
            if (!decimal.TryParse(this.txtAdjustedFreight.Text.Trim(), out adjustedFreight))
            {
                str = str + Formatter.FormatErrorMessage("运费必须在0-1000万之间");
            }
            if (!decimal.TryParse(this.txtAdjustedPayCharge.Text.Trim(), out adjustedPayCharge))
            {
                str = str + Formatter.FormatErrorMessage("支付费用必须在0-1000万之间");
            }
            int length = 0;
            if (this.txtAdjustedDiscount.Text.Trim().IndexOf(".") > 0)
            {
                length = this.txtAdjustedDiscount.Text.Trim().Substring(this.txtAdjustedDiscount.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!decimal.TryParse(this.txtAdjustedDiscount.Text.Trim(), out discountAmout) || (length > 2))
            {
                str = str + Formatter.FormatErrorMessage("订单折扣填写错误,订单折扣只能是数值，且不能超过2位小数");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

