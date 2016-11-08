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
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class MyArticleList : DistributorPage
    {
        protected Button btnSearch;
        protected WebCalendar calendarEndDataTime;
        protected WebCalendar calendarStartDataTime;
        private int? categoryId;
        protected DistributorArticleCategoryDropDownList dropArticleCategory;
        private DateTime? endArticleTime;
        protected Grid grdArticleList;
        protected PageSize hrefPageSize;
        private string keywords = string.Empty;
        protected ImageLinkButton lkbtnDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        private DateTime? startArticleTime;
        protected TextBox txtKeywords;

        private void BindSearch()
        {
            ArticleQuery articleQuery = new ArticleQuery();
            articleQuery.StartArticleTime = this.startArticleTime;
            articleQuery.EndArticleTime = this.endArticleTime;
            articleQuery.Keywords = Globals.HtmlEncode(this.keywords);
            articleQuery.CategoryId = this.categoryId;
            articleQuery.PageIndex = this.pager.PageIndex;
            articleQuery.PageSize = this.pager.PageSize;
            articleQuery.SortBy = this.grdArticleList.SortOrderBy;
            articleQuery.SortOrder = SortAction.Desc;
            DbQueryResult articleList = SubsiteCommentsHelper.GetArticleList(articleQuery);
            this.grdArticleList.DataSource = articleList.Data;
            this.grdArticleList.DataBind();
            this.pager.TotalRecords = articleList.TotalRecords;
            this.pager1.TotalRecords = articleList.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadActicleList(true);
        }

        private void grdArticleList_ReBindData(object sender)
        {
            this.BindSearch();
        }

        private void grdArticleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Release")
            {
                int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int articlId = (int) this.grdArticleList.DataKeys[rowIndex].Value;
                bool isrealse = false;
                string str = "取消";
                if (e.CommandArgument.ToString().ToLower() == "false")
                {
                    isrealse = true;
                    str = "发布";
                }
                if (SubsiteCommentsHelper.UpdateMyArticRelease(articlId, isrealse))
                {
                    this.ShowMsg(str + "当前文章成功！", true);
                }
                else
                {
                    this.ShowMsg(str + "当前文章失败！", false);
                }
            }
            else if (e.CommandName == "Delete")
            {
                int num3 = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
                int articleId = (int) this.grdArticleList.DataKeys[num3].Value;
                if (SubsiteCommentsHelper.DeleteArticle(articleId))
                {
                    this.BindSearch();
                    this.ShowMsg("成功删除了一篇文章", true);
                }
                else
                {
                    this.ShowMsg("删除失败", false);
                }
            }
            this.ReloadActicleList(false);
        }

        private void grdArticleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int articleId = (int) this.grdArticleList.DataKeys[e.RowIndex].Value;
            if (SubsiteCommentsHelper.DeleteArticle(articleId))
            {
                this.BindSearch();
                this.ShowMsg("成功删除了一篇文章", true);
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            IList<int> articles = new List<int>();
            int num = 0;
            foreach (GridViewRow row in this.grdArticleList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    int item = Convert.ToInt32(this.grdArticleList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    articles.Add(item);
                }
            }
            if (num != 0)
            {
                int num3 = SubsiteCommentsHelper.DeleteArticles(articles);
                this.BindSearch();
                this.ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除{0}篇文章", new object[] { num3 }), true);
            }
            else
            {
                this.ShowMsg("请先选择需要删除的文章", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = Globals.UrlDecode(this.Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(this.Page.Request.QueryString["StartArticleTime"], out now))
                {
                    this.startArticleTime = new DateTime?(now);
                }
                DateTime time2 = DateTime.Now;
                if (DateTime.TryParse(this.Page.Request.QueryString["EndArticleTime"], out time2))
                {
                    this.endArticleTime = new DateTime?(time2);
                }
                this.txtKeywords.Text = this.keywords;
                this.dropArticleCategory.SelectedValue = this.categoryId;
                this.calendarStartDataTime.SelectedDate = this.startArticleTime;
                this.calendarEndDataTime.SelectedDate = this.endArticleTime;
            }
            else
            {
                this.keywords = this.txtKeywords.Text;
                this.categoryId = this.dropArticleCategory.SelectedValue;
                this.startArticleTime = this.calendarStartDataTime.SelectedDate;
                this.endArticleTime = this.calendarEndDataTime.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdArticleList.RowDeleting += new GridViewDeleteEventHandler(this.grdArticleList_RowDeleting);
            this.lkbtnDeleteCheck.Click += new EventHandler(this.lkbtnDeleteCheck_Click);
            this.grdArticleList.ReBindData += new Grid.ReBindDataEventHandler(this.grdArticleList_ReBindData);
            this.grdArticleList.RowCommand += new GridViewCommandEventHandler(this.grdArticleList_RowCommand);
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.dropArticleCategory.DataBind();
                this.dropArticleCategory.SelectedValue = this.categoryId;
                this.BindSearch();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void ReloadActicleList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", Globals.UrlEncode(this.txtKeywords.Text.Trim()));
            queryStrings.Add("CategoryId", this.dropArticleCategory.SelectedValue.ToString());
            if (this.calendarStartDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("StartArticleTime", this.calendarStartDataTime.SelectedDate.ToString());
            }
            if (this.calendarEndDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("EndArticleTime", this.calendarEndDataTime.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("SortBy", this.grdArticleList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

