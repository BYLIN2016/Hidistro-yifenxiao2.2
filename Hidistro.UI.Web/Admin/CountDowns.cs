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
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.CountDown)]
    public class CountDowns : AdminPage
    {
        protected LinkButton btnOrder;
        protected Button btnSearch;
        protected Grid grdCountDownsList;
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        private string productName = string.Empty;
        protected TextBox txtProductName;

        private void BindCountDown()
        {
            GroupBuyQuery query = new GroupBuyQuery();
            query.ProductName = this.productName;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult countDownList = PromoteHelper.GetCountDownList(query);
            this.grdCountDownsList.DataSource = countDownList.Data;
            this.grdCountDownsList.DataBind();
            this.pager.TotalRecords = countDownList.TotalRecords;
            this.pager1.TotalRecords = countDownList.TotalRecords;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in this.grdCountDownsList.Rows)
            {
                int result = 0;
                TextBox box = (TextBox) row.FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int countDownId = (int) this.grdCountDownsList.DataKeys[row.RowIndex].Value;
                    PromoteHelper.SwapCountDownSequence(countDownId, result);
                }
            }
            this.BindCountDown();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadHelpList(true);
        }

        private void grdGroupBuyList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (PromoteHelper.DeleteCountDown((int) this.grdCountDownsList.DataKeys[e.RowIndex].Value))
            {
                this.BindCountDown();
                this.ShowMsg("成功删除了选择的限时抢购活动", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num2 = 0;
            foreach (GridViewRow row in this.grdCountDownsList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    PromoteHelper.DeleteCountDown(Convert.ToInt32(this.grdCountDownsList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture));
                }
            }
            if (num2 != 0)
            {
                this.BindCountDown();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"条限时抢购活动", new object[] { num2 }), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的限时抢购活动", false);
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
            this.grdCountDownsList.RowDeleting += new GridViewDeleteEventHandler(this.grdGroupBuyList_RowDeleting);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindCountDown();
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
            queryStrings.Add("SortBy", this.grdCountDownsList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

