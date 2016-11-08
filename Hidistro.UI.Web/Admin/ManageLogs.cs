namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class ManageLogs : AdminPage
    {
        protected Button btnQueryLogs;
        protected WebCalendar calenderFromDate;
        protected WebCalendar calenderToDate;
        protected DataList dlstLog;
        protected LogsUserNameDropDownList dropOperationUserNames;
        protected PageSize hrefPageSize;
        protected ImageLinkButton lkbDeleteAll;
        protected ImageLinkButton lkbDeleteCheck;
        protected ImageLinkButton lkbDeleteCheck1;
        protected Pager pager;
        protected Pager pager1;

        public void BindLogs()
        {
            DbQueryResult logs = EventLogs.GetLogs(this.GetOperationLogQuery());
            this.dlstLog.DataSource = logs.Data;
            this.dlstLog.DataBind();
            this.SetSearchControl();
            this.pager.TotalRecords = logs.TotalRecords;
            this.pager1.TotalRecords = logs.TotalRecords;
        }

        private void btnQueryLogs_Click(object sender, EventArgs e)
        {
            this.ReloadManagerLogs(true);
        }

        private void DeleteCheck()
        {
            string strIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                strIds = base.Request["CheckBoxGroup"];
            }
            if (strIds.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的操作日志项", false);
            }
            else
            {
                int num = EventLogs.DeleteLogs(strIds);
                this.BindLogs();
                this.ShowMsg(string.Format("成功删除了{0}个操作日志", num), true);
            }
        }

        private void dlstLog_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            long logId = (long) this.dlstLog.DataKeys[e.Item.ItemIndex];
            if (EventLogs.DeleteLog(logId))
            {
                this.BindLogs();
                this.ShowMsg("成功删除了单个操作日志", true);
            }
            else
            {
                this.ShowMsg("在删除过程中出现未知错误", false);
            }
        }

        private OperationLogQuery GetOperationLogQuery()
        {
            OperationLogQuery query = new OperationLogQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
            {
                query.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
            {
                query.FromDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
            {
                query.ToDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
            }
            query.Page.PageIndex = this.pager.PageIndex;
            query.Page.PageSize = this.pager.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
            {
                query.Page.SortBy = this.Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            return query;
        }

        private void lkbDeleteAll_Click(object sender, EventArgs e)
        {
            if (EventLogs.DeleteAllLogs())
            {
                this.BindLogs();
                this.ShowMsg("成功删除了所有操作日志", true);
            }
            else
            {
                this.ShowMsg("在删除过程中出现未知错误", false);
            }
        }

        private void lkbDeleteCheck_Click(object sender, EventArgs e)
        {
            this.DeleteCheck();
        }

        private void lkbDeleteCheck1_Click(object sender, EventArgs e)
        {
            this.DeleteCheck();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.dlstLog.DeleteCommand += new DataListCommandEventHandler(this.dlstLog_DeleteCommand);
            this.btnQueryLogs.Click += new EventHandler(this.btnQueryLogs_Click);
            this.lkbDeleteCheck.Click += new EventHandler(this.lkbDeleteCheck_Click);
            this.lkbDeleteCheck1.Click += new EventHandler(this.lkbDeleteCheck1_Click);
            this.lkbDeleteAll.Click += new EventHandler(this.lkbDeleteAll_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropOperationUserNames.DataBind();
                this.BindLogs();
            }
        }

        private void ReloadManagerLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OperationUserName", this.dropOperationUserNames.SelectedValue);
            if (this.calenderFromDate.SelectedDate.HasValue)
            {
                queryStrings.Add("FromDate", this.calenderFromDate.SelectedDate.ToString());
            }
            if (this.calenderToDate.SelectedDate.HasValue)
            {
                queryStrings.Add("ToDate", this.calenderToDate.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsCallback)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
                {
                    this.dropOperationUserNames.SelectedValue = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
                {
                    this.calenderFromDate.SelectedDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
                {
                    this.calenderToDate.SelectedDate = new DateTime?(Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
                }
            }
        }
    }
}

