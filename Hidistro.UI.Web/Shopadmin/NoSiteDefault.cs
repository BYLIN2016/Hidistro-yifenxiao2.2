namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
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

    public class NoSiteDefault : DistributorPage
    {
        protected HyperLink allPurchaseOrder;
        protected Button btnClosePurchaseOrder;
        protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
        protected Grid grdPurchaseOrders;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HyperLink hpkWaitPayPurchaseOrder;
        protected Label lblPurchaseOrderNumbers;
        protected FormatedTimeLabel lblTime;
        protected Literal ltrAdminName;

        private void BindPurchaseOrders()
        {
            int num;
            DataTable recentlyManualPurchaseOrders = SubsiteSalesHelper.GetRecentlyManualPurchaseOrders(out num);
            this.lblPurchaseOrderNumbers.Text = recentlyManualPurchaseOrders.Rows.Count.ToString();
            this.hpkWaitPayPurchaseOrder.Text = num.ToString();
            this.allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx";
            this.hpkWaitPayPurchaseOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx?PurchaseStatus={0}", 1);
            this.grdPurchaseOrders.DataSource = recentlyManualPurchaseOrders;
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

        private void grdPurchaseOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl control = (HtmlGenericControl) e.Row.FindControl("lkBtnCancelPurchaseOrder");
                HyperLink link = (HyperLink) e.Row.FindControl("lkbtnPay");
                OrderStatus status = (OrderStatus) DataBinder.Eval(e.Row.DataItem, "PurchaseStatus");
                if (status == OrderStatus.WaitBuyerPay)
                {
                    control.Visible = true;
                    control.InnerHtml = control.InnerHtml + "<br />";
                    link.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdPurchaseOrders.RowDataBound += new GridViewRowEventHandler(this.grdPurchaseOrders_RowDataBound);
            this.btnClosePurchaseOrder.Click += new EventHandler(this.btnClosePurchaseOrder_Click);
            if (!base.IsPostBack)
            {
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                this.ltrAdminName.Text = distributor.Username;
                this.lblTime.Time = distributor.LastLoginDate;
                this.BindPurchaseOrders();
            }
        }
    }
}

