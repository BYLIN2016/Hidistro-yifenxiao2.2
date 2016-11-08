namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.GroupBuy)]
    public class GroupBuys : AdminPage
    {
        protected LinkButton btnOrder;
        protected Button btnSearch;
        protected Grid grdGroupBuyList;
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        private string productName = string.Empty;
        protected TextBox txtProductName;

        private void BindGroupBuy()
        {
            GroupBuyQuery query = new GroupBuyQuery();
            query.ProductName = this.productName;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult groupBuyList = PromoteHelper.GetGroupBuyList(query);
            this.grdGroupBuyList.DataSource = groupBuyList.Data;
            this.grdGroupBuyList.DataBind();
            this.pager.TotalRecords = groupBuyList.TotalRecords;
            this.pager1.TotalRecords = groupBuyList.TotalRecords;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdGroupBuyList.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int groupBuyId = (int) this.grdGroupBuyList.DataKeys[row.RowIndex].Value;
                    PromoteHelper.SwapGroupBuySequence(groupBuyId, result);
                }
            }
            this.BindGroupBuy();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadHelpList(true);
        }

        private void grdGroupBuyList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FormatedMoneyLabel label = (FormatedMoneyLabel) e.Row.FindControl("lblCurrentPrice");
                int groupBuyId = Convert.ToInt32(this.grdGroupBuyList.DataKeys[e.Row.RowIndex].Value.ToString());
                int prodcutQuantity = int.Parse(DataBinder.Eval(e.Row.DataItem, "ProdcutQuantity").ToString());
                label.Money = PromoteHelper.GetCurrentPrice(groupBuyId, prodcutQuantity);
            }
        }

        private void grdGroupBuyList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy((int) this.grdGroupBuyList.DataKeys[e.RowIndex].Value);
            if ((groupBuy.Status == GroupBuyStatus.UnderWay) || (groupBuy.Status == GroupBuyStatus.EndUntreated))
            {
                this.ShowMsg("团购活动正在进行中或结束未处理，不允许删除", false);
            }
            else if (PromoteHelper.DeleteGroupBuy((int) this.grdGroupBuyList.DataKeys[e.RowIndex].Value))
            {
                this.BindGroupBuy();
                this.ShowMsg("成功删除了选择的团购活动", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num2 = 0;
            foreach (GridViewRow row in this.grdGroupBuyList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    PromoteHelper.DeleteGroupBuy(Convert.ToInt32(this.grdGroupBuyList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture));
                }
            }
            if (num2 != 0)
            {
                this.BindGroupBuy();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"条团购活动", new object[] { num2 }), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的团购活动", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
                {
                    this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
                }
                this.txtProductName.Text = this.productName;
            }
            else
            {
                this.productName = this.txtProductName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            this.grdGroupBuyList.RowDeleting += new GridViewDeleteEventHandler(this.grdGroupBuyList_RowDeleting);
            this.grdGroupBuyList.RowDataBound += new GridViewRowEventHandler(this.grdGroupBuyList_RowDataBound);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindGroupBuy();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(this.txtProductName.Text.Trim()));
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", this.grdGroupBuyList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

