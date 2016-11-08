namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.Summary)]
    public class Main : AdminPage
    {
        protected HiControls HiControlsId;
        protected HyperLink hpkLiuYan;
        protected HyperLink hpkMessages;
        protected HyperLink hpkZiXun;
        protected ClassShowOnDataLitl lblDistributorBlancedrawRequest;
        protected ClassShowOnDataLitl lblDistributorSiteRequest;
        protected ClassShowOnDataLitl lblDistroNewAddYesterToday;
        protected ClassShowOnDataLitl lblDistrosBalanceTotal;
        protected ClassShowOnDataLitl lblMemberBlancedrawRequest;
        protected ClassShowOnDataLitl lblMembersBalanceTotal;
        protected ClassShowOnDataLitl lblOrderPriceMonth;
        protected ClassShowOnDataLitl lblOrderPriceYesterDay;
        protected ClassShowOnDataLitl lblProductCountTotal;
        protected ClassShowOnDataLitl lblPurchaseOrderNumbWait;
        protected ClassShowOnDataLitl lblPurchaseorderPriceMonth;
        protected ClassShowOnDataLitl lblPurchaseorderPriceToDay;
        protected ClassShowOnDataLitl lblPurchaseorderPriceYesterDay;
        protected ClassShowOnDataLitl lblTodayFinishOrder;
        protected ClassShowOnDataLitl lblTodayFinishPurchaseOrder;
        protected ClassShowOnDataLitl lblTodayOrderAmout;
        protected ClassShowOnDataLitl lblTotalDistributors;
        protected ClassShowOnDataLitl lblTotalMembers;
        protected ClassShowOnDataLitl lblTotalProducts;
        protected ClassShowOnDataLitl lblUserNewAddYesterToday;
        protected ClassShowOnDataLitl lblYesterdayFinishOrder;
        protected ClassShowOnDataLitl lblYesterdayFinishPurchaseOrder;
        protected ClassShowOnDataLitl ltrTodayAddDistroNumber;
        protected ClassShowOnDataLitl ltrTodayAddMemberNumber;
        protected HyperLink ltrWaitSendOrdersNumber;
        protected HyperLink ltrWaitSendPurchaseOrdersNumber;

        private void BindStatistics(AdminStatisticsInfo statistics)
        {
            Users.GetUser(HiContext.Current.User.UserId);
            if (statistics.OrderNumbWaitConsignment > 0)
            {
                this.ltrWaitSendOrdersNumber.NavigateUrl = "javascript:ShowSecondMenuLeft('订 单','sales/manageorder.aspx','" + Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus=2')";
            }
            this.ltrWaitSendOrdersNumber.Text = (statistics.OrderNumbWaitConsignment > 0) ? (statistics.OrderNumbWaitConsignment.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>";
            this.ltrWaitSendPurchaseOrdersNumber.Text = (statistics.PurchaseOrderNumbWaitConsignment > 0) ? (statistics.PurchaseOrderNumbWaitConsignment.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>";
            if (statistics.PurchaseOrderNumbWaitConsignment > 0)
            {
                this.ltrWaitSendPurchaseOrdersNumber.NavigateUrl = "javascript:ShowSecondMenuLeft('采购单','purchaseOrder/managepurchaseorder.aspx','" + Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx?PurchaseStatus=2')";
            }
            this.hpkLiuYan.Text = (statistics.LeaveComments > 0) ? (statistics.LeaveComments.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>";
            this.hpkZiXun.Text = (statistics.ProductConsultations > 0) ? (statistics.ProductConsultations.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>";
            this.hpkMessages.Text = (statistics.Messages > 0) ? (statistics.Messages.ToString() + "条") : "<font style=\"color:#2d2d2d\">0条</font>";
            this.hpkLiuYan.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/manageleavecomments.aspx','" + Globals.ApplicationPath + "/Admin/comment/ManageLeaveComments.aspx?MessageStatus=3')";
            this.hpkZiXun.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/productconsultations.aspx',null)";
            this.hpkMessages.NavigateUrl = "javascript:ShowSecondMenuLeft('CRM管理','comment/receivedmessages.aspx','" + Globals.ApplicationPath + "/Admin/comment/ReceivedMessages.aspx?IsRead=0')";
            this.lblTodayOrderAmout.Text = (statistics.OrderPriceToday > 0M) ? ("￥" + Globals.FormatMoney(statistics.OrderPriceToday)) : string.Empty;
            this.ltrTodayAddMemberNumber.Text = (statistics.UserNewAddToday > 0) ? statistics.UserNewAddToday.ToString() : string.Empty;
            this.ltrTodayAddDistroNumber.Text = (statistics.DistroButorsNewAddToday > 0) ? (statistics.DistroButorsNewAddToday.ToString() + "位") : string.Empty;
            this.lblMembersBalanceTotal.Text = "￥" + Globals.FormatMoney(statistics.MembersBalance);
            this.lblDistrosBalanceTotal.Text = "￥" + Globals.FormatMoney(statistics.DistrosBalance);
            this.lblProductCountTotal.Text = (statistics.ProductAlert > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('商 品','product/productonsales.aspx?isAlert=True',null);\">" + statistics.ProductAlert.ToString() + "条</a>") : "<font style=\"color:#2d2d2d\">0条</font>";
            this.lblPurchaseOrderNumbWait.Text = (statistics.PurchaseOrderNumbWait > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('采购单','purchaseorder/managepurchaseorder.aspx','purchaseorder/managepurchaseorder.aspx?purchasestatus=1')\">" + statistics.PurchaseOrderNumbWait.ToString() + "条</a>") : string.Empty;
            this.lblMemberBlancedrawRequest.Text = (statistics.MemberBlancedrawRequest > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('财务管理','member/balancedrawrequest.aspx','member/balancedrawrequest.aspx')\">" + statistics.MemberBlancedrawRequest.ToString() + "条</a>") : string.Empty;
            this.lblDistributorBlancedrawRequest.Text = (statistics.DistributorBlancedrawRequest > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('财务管理','distribution/distributorbalancedrawrequest.aspx',null)\">" + statistics.DistributorBlancedrawRequest.ToString() + "条</a>") : string.Empty;
            this.lblDistributorSiteRequest.Text = (statistics.DistributorSiteRequest > 0) ? ("<a href=\"javascript:ShowSecondMenuLeft('分销管理','distribution/siterequests.aspx','distribution/siterequests.aspx')\">" + statistics.DistributorSiteRequest.ToString() + "条</a>") : string.Empty;
            this.lblTodayFinishOrder.Text = (statistics.TodayFinishOrder > 0) ? statistics.TodayFinishOrder.ToString() : string.Empty;
            this.lblYesterdayFinishOrder.Text = (statistics.YesterdayFinishOrder > 0) ? statistics.YesterdayFinishOrder.ToString() : string.Empty;
            this.lblOrderPriceYesterDay.Text = (statistics.OrderPriceYesterDay > 0M) ? ("￥" + statistics.OrderPriceYesterDay.ToString("F2")) : string.Empty;
            this.lblTodayFinishPurchaseOrder.Text = (statistics.TodayFinishPurchaseOrder > 0) ? statistics.TodayFinishPurchaseOrder.ToString() : string.Empty;
            this.lblYesterdayFinishPurchaseOrder.Text = (statistics.YesterdayFinishPurchaseOrder > 0) ? statistics.YesterdayFinishPurchaseOrder.ToString() : string.Empty;
            this.lblPurchaseorderPriceToDay.Text = (statistics.PurchaseorderPriceToDay > 0M) ? ("￥" + statistics.PurchaseorderPriceToDay.ToString("F2")) : string.Empty;
            this.lblPurchaseorderPriceYesterDay.Text = (statistics.PurchaseorderPriceYesterDay > 0M) ? ("￥" + statistics.PurchaseorderPriceYesterDay.ToString("F2")) : string.Empty;
            this.lblUserNewAddYesterToday.Text = (statistics.UserNewAddYesterToday > 0) ? (statistics.UserNewAddYesterToday.ToString() + "位") : string.Empty;
            this.lblDistroNewAddYesterToday.Text = (statistics.DistroNewAddYesterToday > 0) ? (statistics.DistroNewAddYesterToday.ToString() + "位") : string.Empty;
            this.lblTotalMembers.Text = (statistics.TotalMembers > 0) ? (statistics.TotalMembers.ToString() + "位") : string.Empty;
            this.lblTotalDistributors.Text = (statistics.TotalDistributors > 0) ? (statistics.TotalDistributors.ToString() + "位") : string.Empty;
            this.lblTotalProducts.Text = (statistics.TotalProducts > 0) ? (statistics.TotalProducts.ToString() + "条") : string.Empty;
            this.lblPurchaseorderPriceMonth.Text = (statistics.PurchaseorderPriceMonth > 0M) ? ("￥" + statistics.PurchaseorderPriceMonth.ToString("F2")) : string.Empty;
            this.lblOrderPriceMonth.Text = (statistics.OrderPriceMonth > 0M) ? ("￥" + statistics.OrderPriceMonth.ToString("F2")) : string.Empty;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                AdminStatisticsInfo statistics = SalesHelper.GetStatistics();
                this.BindStatistics(statistics);
            }
        }
    }
}

