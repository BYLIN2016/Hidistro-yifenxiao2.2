namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class Managers : AdminPage
    {
        protected Button btnSearchButton;
        protected RoleDropDownList dropRolesList;
        protected Grid grdManager;
        protected Pager pager;
        protected Pager pager1;
        protected TextBox txtSearchText;

        private void BindData()
        {
            ManagerQuery managerQuery = this.GetManagerQuery();
            DbQueryResult managers = ManagerHelper.GetManagers(managerQuery);
            this.grdManager.DataSource = managers.Data;
            this.grdManager.DataBind();
            this.txtSearchText.Text = managerQuery.Username;
            this.dropRolesList.SelectedValue = managerQuery.RoleId;
            this.pager.TotalRecords = managers.TotalRecords;
            this.pager1.TotalRecords = managers.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReloadManagerLogs(true);
        }

        private ManagerQuery GetManagerQuery()
        {
            ManagerQuery query = new ManagerQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
            {
                query.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RoleId"]))
            {
                query.RoleId = new Guid(this.Page.Request.QueryString["RoleId"]);
            }
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
            {
                query.SortBy = this.Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
            {
                query.SortOrder = SortAction.Desc;
            }
            return query;
        }

        private void grdManager_ReBindData(object sender)
        {
            this.ReloadManagerLogs(false);
        }

        private void grdManager_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = (int) this.grdManager.DataKeys[e.RowIndex].Value;
            if (HiContext.Current.User.UserId == userId)
            {
                this.ShowMsg("不能删除自己", false);
            }
            else if (!ManagerHelper.Delete(ManagerHelper.GetManager(userId).UserId))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindData();
                this.ShowMsg("成功删除了一个管理员", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
            this.grdManager.RowDeleting += new GridViewDeleteEventHandler(this.grdManager_RowDeleting);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropRolesList.DataBind();
                this.BindData();
            }
        }

        private void ReloadManagerLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Username", this.txtSearchText.Text);
            queryStrings.Add("RoleId", Convert.ToString(this.dropRolesList.SelectedValue));
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("SortBy", this.grdManager.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

