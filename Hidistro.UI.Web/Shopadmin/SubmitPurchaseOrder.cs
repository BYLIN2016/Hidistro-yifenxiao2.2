namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SubmitPurchaseOrder : DistributorPage
    {
        protected Button btnSubmit;
        protected DistributorPaymentRadioButtonList radioPaymentMode;
        protected ShippingModeRadioButtonList radioShippingMode;
        protected RegionSelector rsddlRegion;
        protected UserControl Skin1;
        protected TextBox txtAddress;
        protected HtmlGenericControl txtAddressTip;
        protected TextBox txtMobile;
        protected HtmlGenericControl txtMobileTip;
        protected TextBox txtRemark;
        protected TextBox txtShipTo;
        protected HtmlGenericControl txtShipToTip;
        protected TextBox txtTel;
        protected HtmlGenericControl txtTelTip;
        protected TextBox txtZipcode;

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.ValidateCreateOrder())
            {
                PurchaseOrderInfo purchaseOrderInfo = this.GetPurchaseOrderInfo();
                if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
                {
                    this.ShowMsg("您暂时未选择您要添加的商品", false);
                }
                else if (SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
                {
                    SubsiteSalesHelper.ClearPurchaseShoppingCart();
                    int.Parse(this.radioPaymentMode.SelectedValue);
                    PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(int.Parse(this.radioPaymentMode.SelectedValue));
                    if ((paymentMode != null) && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.podrequest"))
                    {
                        this.ShowMsg("您选择的是货到付款方式，请等待主站发货", true);
                    }
                    else if ((paymentMode != null) && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.bankrequest"))
                    {
                        this.ShowMsg("您选择的是线下付款方式，请与主站管理员联系", true);
                    }
                    else
                    {
                        base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=" + purchaseOrderInfo.PurchaseOrderId + "&PayMode=" + this.radioPaymentMode.SelectedValue);
                    }
                }
                else
                {
                    this.ShowMsg("提交采购单失败", false);
                }
            }
        }

        private string GeneratePurchaseOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char) (0x30 + ((ushort) (num % 10)))).ToString();
            }
            return ("MPO" + DateTime.Now.ToString("yyyyMMdd") + str);
        }

        private PurchaseOrderInfo GetPurchaseOrderInfo()
        {
            PurchaseOrderInfo info = new PurchaseOrderInfo();
            Distributor user = Users.GetUser(HiContext.Current.User.UserId) as Distributor;
            int modeId = int.Parse(this.radioPaymentMode.SelectedValue);
            PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(modeId);
            if (paymentMode != null)
            {
                info.PaymentTypeId = modeId;
                info.PaymentType = paymentMode.Name;
                info.Gateway = paymentMode.Gateway;
            }
            string str = this.GeneratePurchaseOrderId();
            info.PurchaseOrderId = str;
            IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
            decimal totalWeight = 0M;
            if (purchaseShoppingCartItemInfos.Count >= 1)
            {
                foreach (PurchaseShoppingCartItemInfo info3 in purchaseShoppingCartItemInfos)
                {
                    PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                    item.PurchaseOrderId = str;
                    item.SkuId = info3.SkuId;
                    item.ThumbnailsUrl = info3.ThumbnailsUrl;
                    item.SKUContent = info3.SKUContent;
                    item.SKU = info3.SKU;
                    item.Quantity = info3.Quantity;
                    item.ProductId = info3.ProductId;
                    item.ItemWeight = info3.ItemWeight;
                    item.ItemCostPrice = info3.CostPrice;
                    item.ItemPurchasePrice = info3.ItemPurchasePrice;
                    item.ItemListPrice = info3.ItemListPrice;
                    item.ItemDescription = info3.ItemDescription;
                    item.ItemHomeSiteDescription = info3.ItemDescription;
                    totalWeight += info3.ItemWeight * info3.Quantity;
                    info.PurchaseOrderItems.Add(item);
                }
                ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
                info.ShipTo = this.txtShipTo.Text.Trim();
                if (this.rsddlRegion.GetSelectedRegionId().HasValue)
                {
                    info.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                }
                info.Address = Globals.HtmlEncode(this.txtAddress.Text.Trim());
                info.TelPhone = this.txtTel.Text.Trim();
                info.ZipCode = this.txtZipcode.Text.Trim();
                info.CellPhone = this.txtMobile.Text.Trim();
                info.OrderId = null;
                info.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
                info.RealModeName = shippingMode.Name;
                info.ShippingModeId = this.radioShippingMode.SelectedValue.Value;
                info.ModeName = shippingMode.Name;
                info.AdjustedFreight = SubsiteSalesHelper.CalcFreight(info.RegionId, totalWeight, shippingMode);
                info.Freight = info.AdjustedFreight;
                info.ShippingRegion = this.rsddlRegion.SelectedRegions;
                info.Remark = Globals.HtmlEncode(this.txtRemark.Text.Trim());
                info.PurchaseStatus = OrderStatus.WaitBuyerPay;
                info.DistributorId = user.UserId;
                info.Distributorname = user.Username;
                info.DistributorEmail = user.Email;
                info.DistributorRealName = user.RealName;
                info.DistributorQQ = user.QQ;
                info.DistributorWangwang = user.Wangwang;
                info.DistributorMSN = user.MSN;
                info.RefundStatus = RefundStatus.None;
                info.Weight = totalWeight;
            }
            return info;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            if (!base.IsPostBack)
            {
                this.radioPaymentMode.DataBind();
                this.radioShippingMode.DataBind();
            }
        }

        private bool ValidateCreateOrder()
        {
            string str = string.Empty;
            if (!this.rsddlRegion.GetSelectedRegionId().HasValue || (this.rsddlRegion.GetSelectedRegionId().Value == 0))
            {
                str = str + Formatter.FormatErrorMessage("请选择收货地址");
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
            {
                str = str + Formatter.FormatErrorMessage("请输入正确的收货人姓名");
            }
            if (!string.IsNullOrEmpty(this.txtShipTo.Text) && ((this.txtShipTo.Text.Trim().Length < 2) || (this.txtShipTo.Text.Trim().Length > 20)))
            {
                str = str + Formatter.FormatErrorMessage("收货人姓名的长度限制在2-20个字符");
            }
            if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()) || (this.txtAddress.Text.Trim().Length > 100))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人详细地址,在100个字符以内");
            }
            regex = new Regex("^[0-9]*$");
            if (!string.IsNullOrEmpty(this.txtMobile.Text.Trim()) && ((!regex.IsMatch(this.txtMobile.Text.Trim()) || (this.txtMobile.Text.Trim().Length > 20)) || (this.txtMobile.Text.Trim().Length < 3)))
            {
                str = str + Formatter.FormatErrorMessage("手机号码长度限制在3-20个字符之间,只能输入数字");
            }
            regex = new Regex("^[0-9-]*$");
            if (!string.IsNullOrEmpty(this.txtTel.Text.Trim()) && ((!regex.IsMatch(this.txtTel.Text.Trim()) || (this.txtTel.Text.Trim().Length > 20)) || (this.txtTel.Text.Trim().Length < 3)))
            {
                str = str + Formatter.FormatErrorMessage("电话号码长度限制在3-20个字符之间,只能输入数字和字符“-”");
            }
            if (!this.radioShippingMode.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择配送方式");
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

