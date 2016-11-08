namespace Hidistro.UI.Web.Shopadmin.sales
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Subsites.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ManageMyRefundNote : DistributorPage
    {
        protected Button btnSearchButton;
        protected DataList dlstRefundNote;
        protected HtmlInputHidden hidOrderId;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtOrderId;

        private void BindRefundNote()
        {
            RefundApplyQuery query = new RefundApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            query.HandleStatus = 1;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "HandleTime";
            query.SortOrder = SortAction.Desc;
            DbQueryResult refundApplys = SubsiteSalesHelper.GetRefundApplys(query);
            this.dlstRefundNote.DataSource = refundApplys.Data;
            this.dlstRefundNote.DataBind();
            this.pager.TotalRecords = refundApplys.TotalRecords;
            this.pager1.TotalRecords = refundApplys.TotalRecords;
            this.txtOrderId.Text = query.OrderId;
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
                this.ShowMsg("请选要删除的退款单", false);
            }
            else
            {
                int num;
                SubsiteSalesHelper.DelRefundApply(str.Split(new char[] { ',' }), out num);
                this.BindRefundNote();
                this.ShowMsg(string.Format("成功删除了{0}个退款单", num), true);
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
            queryStrings.Add("OrderId", this.txtOrderId.Text);
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

