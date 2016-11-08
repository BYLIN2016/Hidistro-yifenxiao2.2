namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class MyHelpList : DistributorPage
    {
        protected Button btnSearch;
        protected WebCalendar calendarEndDataTime;
        protected WebCalendar calendarStartDataTime;
        private int? categoryId;
        protected DistributorHelpCategoryDropDownList dropHelpCategory;
        private DateTime? endTime;
        protected Grid grdHelpList;
        protected PageSize hrefPageSize;
        private string keywords = string.Empty;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        private DateTime? startTime;
        protected TextBox txtkeyWords;

        private void BindSearch()
        {
            HelpQuery helpQuery = new HelpQuery();
            helpQuery.StartArticleTime = this.startTime;
            helpQuery.EndArticleTime = this.endTime;
            helpQuery.Keywords = Globals.HtmlEncode(this.keywords);
            helpQuery.CategoryId = this.categoryId;
            helpQuery.PageIndex = this.pager.PageIndex;
            helpQuery.PageSize = this.pager.PageSize;
            helpQuery.SortBy = this.grdHelpList.SortOrderBy;
            helpQuery.SortOrder = SortAction.Desc;
            DbQueryResult helpList = SubsiteCommentsHelper.GetHelpList(helpQuery);
            this.grdHelpList.DataSource = helpList.Data;
            this.grdHelpList.DataBind();
            this.pager.TotalRecords = helpList.TotalRecords;
            this.pager1.TotalRecords = helpList.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadHelpList(true);
        }

        private void grdHelpList_ReBindData(object sender)
        {
            this.BindSearch();
        }

        private void grdHelpList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsiteCommentsHelper.DeleteHelp((int) this.grdHelpList.DataKeys[e.RowIndex].Value))
            {
                this.BindSearch();
                this.dropHelpCategory.DataBind();
                this.ShowMsg("成功删除了选择的帮助", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            IList<int> helps = new List<int>();
            int num2 = 0;
            foreach (GridViewRow row in this.grdHelpList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num2++;
                    int item = Convert.ToInt32(this.grdHelpList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    helps.Add(item);
                }
            }
            if (num2 != 0)
            {
                int num3 = SubsiteCommentsHelper.DeleteHelps(helps);
                this.BindSearch();
                this.dropHelpCategory.DataBind();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"篇帮助", new object[] { num3 }), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的帮助", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(this.Page.Request.QueryString["StartTime"], out now))
                {
                    this.startTime = new DateTime?(now);
                }
                DateTime time2 = DateTime.Now;
                if (DateTime.TryParse(this.Page.Request.QueryString["EndTime"], out time2))
                {
                    this.endTime = new DateTime?(time2);
                }
                this.txtkeyWords.Text = this.keywords;
                this.calendarStartDataTime.SelectedDate = this.startTime;
                this.calendarEndDataTime.SelectedDate = this.endTime;
            }
            else
            {
                this.keywords = this.txtkeyWords.Text;
                this.startTime = this.calendarStartDataTime.SelectedDate;
                this.endTime = this.calendarEndDataTime.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdHelpList.RowDeleting += new GridViewDeleteEventHandler(this.grdHelpList_RowDeleting);
            this.grdHelpList.ReBindData += new Grid.ReBindDataEventHandler(this.grdHelpList_ReBindData);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.dropHelpCategory.DataBind();
                this.dropHelpCategory.SelectedValue = this.categoryId;
                this.BindSearch();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", this.txtkeyWords.Text.Trim());
            queryStrings.Add("CategoryId", this.dropHelpCategory.SelectedValue.ToString());
            if (this.calendarStartDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("StartTime", this.calendarStartDataTime.SelectedDate.ToString());
            }
            if (this.calendarEndDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("EndTime", this.calendarEndDataTime.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", this.grdHelpList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

