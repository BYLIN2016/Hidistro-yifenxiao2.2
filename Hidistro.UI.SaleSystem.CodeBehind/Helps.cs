namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class Helps : HtmlTemplatedWebControl
    {
        private Pager pager;
        private ThemedTemplatedRepeater rptHelps;

        protected override void AttachChildControls()
        {
            this.rptHelps = (ThemedTemplatedRepeater) this.FindControl("rptHelps");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]))
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["CategoryId"], out result);
                    HelpCategoryInfo helpCategory = CommentBrowser.GetHelpCategory(result);
                    if (helpCategory != null)
                    {
                        PageTitle.AddSiteNameTitle(helpCategory.Name, HiContext.Current.Context);
                    }
                }
                else
                {
                    PageTitle.AddSiteNameTitle("帮助中心", HiContext.Current.Context);
                }
                this.BindList();
            }
        }

        private void BindList()
        {
            HelpQuery helpQuery = this.GetHelpQuery();
            DbQueryResult helpList = new DbQueryResult();
            helpList = CommentBrowser.GetHelpList(helpQuery);
            this.rptHelps.DataSource = helpList.Data;
            this.rptHelps.DataBind();
            this.pager.TotalRecords = helpList.TotalRecords;
        }

        private HelpQuery GetHelpQuery()
        {
            HelpQuery query = new HelpQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
                {
                    query.CategoryId = new int?(result);
                }
            }
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.SortBy = "AddedDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Helps.html";
            }
            base.OnInit(e);
        }
    }
}

