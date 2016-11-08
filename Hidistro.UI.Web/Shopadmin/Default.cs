namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Sales;
    using Hidistro.Subsites.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Default : DistributorPage
    {
        protected HyperLink allorders;
        protected HyperLink allPurchaseOrder;
        protected HyperLink allPurchaseOrder2;
        protected Button btnClosePurchaseOrder;
        protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected Grid grdOrders;
        protected Grid grdPurchaseOrders;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HtmlInputHidden hidStatus;
        protected HyperLink hpkLiuYan;
        protected HyperLink hpkMessages;
        protected HyperLink hpksendOrder;
        protected HyperLink hpkZiXun;
        protected ClassShowOnDataLitl lblDistrosBalanceTotal;
        protected ClassShowOnDataLitl lblMembersBalanceTotal;
        protected Label lblOrderNumbers;
        protected ClassShowOnDataLitl lblProductCountTotal;
        protected Label lblPurchaseOrderNumbers;
        protected FormatedTimeLabel lblTime;
        protected ClassShowOnDataLitl lblTodayOrderAmout;
        protected ClassShowOnDataLitl lblTodaySalesProfile;
        protected Literal ltrAdminName;
        protected ClassShowOnDataLitl ltrTodayAddMemberNumber;
        protected ClassShowOnDataLitl ltrWaitSendOrdersNumber;
        protected ClassShowOnDataLitl ltrWaitSendPurchaseOrdersNumber;

        private void BindBusinessInformation(StatisticsInfo statisticsInfo)
        {
            this.ltrWaitSendOrdersNumber.Text = statisticsInfo.OrderNumbWaitConsignment.ToString();
            this.hpkZiXun.Text = statisticsInfo.ProductConsultations.ToString();
            this.hpkMessages.Text = statisticsInfo.Messages.ToString();
            this.hpkLiuYan.Text = statisticsInfo.LeaveComments.ToString();
            this.lblTodayOrderAmout.Text = Globals.FormatMoney(statisticsInfo.OrderPriceToday);
            this.lblTodaySalesProfile.Text = Globals.FormatMoney(statisticsInfo.OrderProfitToday);
            this.ltrTodayAddMemberNumber.Text = statisticsInfo.UserNewAddToday.ToString();
            this.lblMembersBalanceTotal.Text = Globals.FormatMoney(statisticsInfo.Balance);
            this.lblProductCountTotal.Text = (statisticsInfo.ProductAlert > 0) ? ("<a href='" + Globals.ApplicationPath + "/Shopadmin/product/myproductonsales.aspx?isAlert=True'>" + statisticsInfo.ProductAlert.ToString() + "</a>") : "0";
            this.ltrWaitSendPurchaseOrdersNumber.Text = statisticsInfo.PurchaseOrderNumbWaitConsignment.ToString();
            this.hpkLiuYan.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/ManageMyLeaveComments.aspx?MessageStatus=3";
            this.hpkZiXun.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyProductConsultations.aspx";
            this.hpkMessages.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyReceivedMessages.aspx?IsRead=0";
        }

        private void BindLabels()
        {
            Distributor distributor = SubsiteStoreHelper.GetDistributor();
            this.ltrAdminName.Text = distributor.Username;
            this.lblTime.Time = distributor.LastLoginDate;
            AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
            this.lblDistrosBalanceTotal.Text = (myAccountSummary.AccountAmount > 0M) ? Globals.FormatMoney(myAccountSummary.AccountAmount) : string.Empty;
        }

        private void BindOrders()
        {
            int num;
            DataTable recentlyOrders = SubsiteSalesHelper.GetRecentlyOrders(out num);
            this.lblOrderNumbers.Text = recentlyOrders.Rows.Count.ToString();
            this.hpksendOrder.Text = num.ToString();
            this.hpksendOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/sales/ManageMyOrder.aspx?OrderStatus={0}", 2);
            this.allorders.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/ManageMyOrder.aspx";
            this.grdOrders.DataSource = recentlyOrders;
            this.grdOrders.DataBind();
        }

        private void BindPurchaseOrders()
        {
            int num;
            DataTable recentlyPurchaseOrders = SubsiteSalesHelper.GetRecentlyPurchaseOrders(out num);
            this.lblPurchaseOrderNumbers.Text = recentlyPurchaseOrders.Rows.Count.ToString();
            this.allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx";
            this.allPurchaseOrder2.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx";
            this.grdPurchaseOrders.DataSource = recentlyPurchaseOrders;
            this.grdPurchaseOrders.DataBind();
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hidPurchaseOrderId.Value))
            {
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
                purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
                if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
                {
                    this.BindPurchaseOrders();
                    this.ShowMsg("取消采购成功", true);
                }
                else
                {
                    this.ShowMsg("取消采购失败", false);
                }
            }
        }

        protected void grdOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Row.DataItem, "OrderStatus");
                HyperLink link = (HyperLink) e.Row.FindControl("lkbtnEditPrice");
                HyperLink link2 = (HyperLink) e.Row.FindControl("lkbtnSendGoods");
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        link.Visible = true;
                        link.Text = link.Text + "<br />";
                        break;

                    case OrderStatus.BuyerAlreadyPaid:
                    {
                        int num = (int) DataBinder.Eval(e.Row.DataItem, "GroupBuyId");
                        if (num > 0)
                        {
                            GroupBuyStatus status2 = (GroupBuyStatus) DataBinder.Eval(e.Row.DataItem, "GroupBuyStatus");
                            if (status2 == GroupBuyStatus.Success)
                            {
                                link2.Visible = true;
                                link2.Text = link2.Text + "<br />";
                                return;
                            }
                        }
                        else
                        {
                            link2.Visible = true;
                            link2.Text = link2.Text + "<br />";
                        }
                        break;
                    }
                }
            }
        }

        private void grdPurchaseOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink link = (HyperLink) e.Row.FindControl("lkbtnSendGoods");
                HtmlGenericControl control = (HtmlGenericControl) e.Row.FindControl("lkBtnCancelPurchaseOrder");
                HyperLink link2 = (HyperLink) e.Row.FindControl("lkbtnPay");
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Row.DataItem, "PurchaseStatus");
                string purchaseOrderId = (string) DataBinder.Eval(e.Row.DataItem, "PurchaseOrderId");
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                if (!purchaseOrder.IsManualPurchaseOrder && (status == OrderStatus.SellerAlreadySent))
                {
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(purchaseOrder.OrderId);
                    if ((orderInfo != null) && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                    {
                        link.Visible = true;
                    }
                }
                if (status == OrderStatus.WaitBuyerPay)
                {
                    control.Visible = true;
                    control.InnerHtml = control.InnerHtml + "<br />";
                    link2.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdOrders.RowDataBound += new GridViewRowEventHandler(this.grdOrders_RowDataBound);
            this.grdPurchaseOrders.RowDataBound += new GridViewRowEventHandler(this.grdPurchaseOrders_RowDataBound);
            this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
            if (!base.IsPostBack)
            {
                int num;
                if (int.TryParse(base.Request.QueryString["Status"], out num))
                {
                    this.hidStatus.Value = num.ToString();
                }
                this.BindLabels();
                StatisticsInfo statistics = SubsiteSalesHelper.GetStatistics();
                this.BindBusinessInformation(statistics);
                this.BindPurchaseOrders();
                this.BindOrders();
            }
        }
    }
}

