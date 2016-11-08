namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class ManagePurchaseOrderReturnNote : AdminPage
    {
        protected Button btnSearchButton;
        protected DataList dlstReturnNote;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtPurchaseOrderId;

        private void BindRefundNote()
        {
            ReturnsApplyQuery query = new ReturnsApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
            }
            query.HandleStatus = 1;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "HandleTime";
            query.SortOrder = SortAction.Desc;
            DbQueryResult purchaseReturnsApplys = SalesHelper.GetPurchaseReturnsApplys(query);
            this.dlstReturnNote.DataSource = purchaseReturnsApplys.Data;
            this.dlstReturnNote.DataBind();
            this.pager.TotalRecords = purchaseReturnsApplys.TotalRecords;
            this.pager1.TotalRecords = purchaseReturnsApplys.TotalRecords;
            this.txtPurchaseOrderId.Text = query.OrderId;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadRefundNotes(true);
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                this.ShowMsg("请选要删除的退货单", false);
            }
            else
            {
                int num;
                SalesHelper.DelPurchaseReturnsApply(str.Split(new char[] { ',' }), out num);
                this.BindRefundNote();
                this.ShowMsg(string.Format("成功删除了{0}个退货单", num), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            if (!base.IsPostBack)
            {
                this.BindRefundNote();
            }
        }

        private void ReloadRefundNotes(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
            {
                queryStrings.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

