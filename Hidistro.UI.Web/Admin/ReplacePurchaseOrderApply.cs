namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.PurchaseOrderReplaceApply)]
    public class ReplacePurchaseOrderApply : AdminPage
    {
        protected Button btnAcceptReplace;
        protected Button btnRefuseReplace;
        protected Button btnSearchButton;
        protected DropDownList ddlHandleStatus;
        protected DataList dlstReplace;
        protected HtmlInputHidden hidAdminRemark;
        protected HtmlInputHidden hidPurchaseOrderId;
        protected HtmlInputHidden hidRefundMoney;
        protected HtmlInputHidden hidRefundType;
        protected PageSize hrefPageSize;
        protected Label lblStatus;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected Label replace_lblAddress;
        protected Label replace_lblComments;
        protected Label replace_lblContacts;
        protected Label replace_lblEmail;
        protected Label replace_lblOrderId;
        protected Label replace_lblOrderTotal;
        protected Label replace_lblPostCode;
        protected Label replace_lblTelephone;
        protected TextBox replace_txtAdminRemark;
        protected TextBox txtOrderId;

        private void BindReplace()
        {
            ReplaceApplyQuery replaceQuery = this.GetReplaceQuery();
            DbQueryResult purchaseReplaceApplys = SalesHelper.GetPurchaseReplaceApplys(replaceQuery);
            this.dlstReplace.DataSource = purchaseReplaceApplys.Data;
            this.dlstReplace.DataBind();
            this.pager.TotalRecords = purchaseReplaceApplys.TotalRecords;
            this.pager1.TotalRecords = purchaseReplaceApplys.TotalRecords;
            this.txtOrderId.Text = replaceQuery.OrderId;
            this.ddlHandleStatus.SelectedIndex = 0;
            if (replaceQuery.HandleStatus.HasValue && (replaceQuery.HandleStatus.Value > -1))
            {
                this.ddlHandleStatus.SelectedValue = replaceQuery.HandleStatus.Value.ToString();
            }
        }

        protected void btnAcceptReplace_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, true);
            this.BindReplace();
            this.ShowMsg("成功的确认了采购单换货", true);
        }

        private void btnRefuseReplace_Click(object sender, EventArgs e)
        {
            SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, false);
            this.BindReplace();
            this.ShowMsg("成功的拒绝了采购单换货", true);
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadReplaces(true);
        }

        private void dlstReplace_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HtmlAnchor anchor = (HtmlAnchor) e.Item.FindControl("lkbtnCheckReplace");
                Label label = (Label) e.Item.FindControl("lblHandleStatus");
                if (label.Text == "0")
                {
                    anchor.Visible = true;
                    label.Text = "待处理";
                }
                else if (label.Text == "1")
                {
                    label.Text = "已处理";
                }
                else
                {
                    label.Text = "已拒绝";
                }
            }
        }

        private ReplaceApplyQuery GetReplaceQuery()
        {
            ReplaceApplyQuery query = new ReplaceApplyQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["HandleStatus"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["HandleStatus"], out result) && (result > -1))
                {
                    query.HandleStatus = new int?(result);
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "ApplyForTime";
            query.SortOrder = SortAction.Desc;
            return query;
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
                this.ShowMsg("请选要删除的换货申请单", false);
            }
            else
            {
                int num;
                string format = "成功删除了{0}个换货申请单";
                if (SalesHelper.DelPurchaseReplaceApply(str.Split(new char[] { ',' }), out num))
                {
                    format = string.Format(format, num);
                }
                else
                {
                    format = string.Format(format, num) + ",待处理的申请不能删除";
                }
                this.BindReplace();
                this.ShowMsg(format, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstReplace.ItemDataBound += new DataListItemEventHandler(this.dlstReplace_ItemDataBound);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnAcceptReplace.Click += new EventHandler(this.btnAcceptReplace_Click);
            this.btnRefuseReplace.Click += new EventHandler(this.btnRefuseReplace_Click);
            if (!base.IsPostBack)
            {
                this.BindReplace();
            }
        }

        private void ReloadReplaces(bool isSearch)
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
            if (!string.IsNullOrEmpty(this.ddlHandleStatus.SelectedValue))
            {
                queryStrings.Add("HandleStatus", this.ddlHandleStatus.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

