namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    public class BatchPay : DistributorPage
    {
        protected Button btnConfirmPay;
        protected ImageButton imgBtnBack;
        protected FormatedMoneyLabel lblTotalPrice;
        protected FormatedMoneyLabel lblUseableBalance;
        protected HtmlGenericControl PaySucceess;
        private string purchaseorderIds;
        protected TextBox txtTradePassword;

        private void btnBack_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("ManageMyManualPurchaseOrder.aspx");
        }

        private void btnConfirmPay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTradePassword.Text))
            {
                this.ShowMsg("请输入交易密码", false);
            }
            else if (((decimal) this.lblUseableBalance.Money) < ((decimal) this.lblTotalPrice.Money))
            {
                this.ShowMsg("您的预付款金额不足", false);
            }
            else
            {
                Distributor user = SubsiteStoreHelper.GetDistributor();
                if ((user.Balance - user.RequestBalance) < ((decimal) this.lblTotalPrice.Money))
                {
                    this.ShowMsg("您的预付款金额不足", false);
                }
                else
                {
                    BalanceDetailInfo balance = new BalanceDetailInfo();
                    balance.UserId = user.UserId;
                    balance.UserName = user.Username;
                    balance.TradeType = TradeTypes.Consume;
                    balance.TradeDate = DateTime.Now;
                    balance.Expenses = new decimal?((decimal) this.lblTotalPrice.Money);
                    balance.Balance = user.Balance - ((decimal) this.lblTotalPrice.Money);
                    balance.Remark = " 批量付款的采购单编号：" + this.purchaseorderIds.Replace(',', ' ');
                    user.TradePassword = this.txtTradePassword.Text;
                    if (Users.ValidTradePassword(user))
                    {
                        if (!SubsiteSalesHelper.BatchConfirmPay(balance, this.purchaseorderIds))
                        {
                            this.ShowMsg("付款失败", false);
                        }
                        else
                        {
                            int num = 0;
                            foreach (string str in this.purchaseorderIds.Split(new char[] { ',' }))
                            {
                                PurchaseDebitNote note = new PurchaseDebitNote();
                                note.NoteId = Globals.GetGenerateId() + num;
                                note.PurchaseOrderId = str;
                                note.Operator = HiContext.Current.User.Username;
                                note.Remark = "分销商采购单预付款支付成功";
                                SubsiteSalesHelper.SavePurchaseDebitNote(note);
                                num++;
                            }
                            this.PaySucceess.Visible = true;
                        }
                    }
                    else
                    {
                        this.ShowMsg("交易密码错误", false);
                    }
                }
            }
        }

        private decimal GetPayTotal()
        {
            decimal num = 0M;
            new List<PurchaseOrderInfo>();
            foreach (string str in this.purchaseorderIds.Split(new char[] { ',' }))
            {
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(str);
                if ((purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) && (purchaseOrder.Gateway != "hishop.plugins.payment.podrequest"))
                {
                    num += purchaseOrder.GetPurchaseTotal();
                }
            }
            return num;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["purchaseorderIds"]))
            {
                this.purchaseorderIds = base.Request["purchaseorderIds"].Trim(new char[] { ',' });
                this.lblTotalPrice.Money = this.GetPayTotal();
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                this.lblUseableBalance.Money = distributor.Balance - distributor.RequestBalance;
                this.btnConfirmPay.Click += new EventHandler(this.btnConfirmPay_Click);
                this.imgBtnBack.Click += new ImageClickEventHandler(this.btnBack_Click);
            }
        }
    }
}

