namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class ManageMyLeaveComments : DistributorPage
    {
        protected ImageLinkButton btnDeleteSelect;
        protected ImageLinkButton btnDeleteSelect1;
        protected PageSize hrefPageSize;
        protected Grid leaveList;
        protected Pager pager;
        protected Pager pager1;
        protected MessageStatusDropDownList statusList;

        private void BindList()
        {
            LeaveCommentQuery query = new LeaveCommentQuery();
            query.PageIndex = this.pager.PageIndex;
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus) int.Parse(base.Request.QueryString["MessageStatus"]);
                this.statusList.SelectedValue = query.MessageStatus;
            }
            DbQueryResult leaveComments = SubsiteCommentsHelper.GetLeaveComments(query);
            this.leaveList.DataSource = leaveComments.Data;
            this.leaveList.DataBind();
            this.pager.TotalRecords = leaveComments.TotalRecords;
            this.pager1.TotalRecords = leaveComments.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> leaveIds = new List<long>();
            foreach (GridViewRow row in this.leaveList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long) this.leaveList.DataKeys[row.RowIndex].Value;
                    leaveIds.Add(item);
                }
            }
            if (leaveIds.Count > 0)
            {
                SubsiteCommentsHelper.DeleteLeaveComments(leaveIds);
                this.ShowMsg("成功删除了选择的消息.", true);
            }
            else
            {
                this.ShowMsg("请选择需要删除的消息.", false);
            }
            this.BindList();
        }

        private void leaveList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long leaveId = (long) this.leaveList.DataKeys[e.RowIndex].Value;
            if (SubsiteCommentsHelper.DeleteLeaveComment(leaveId))
            {
                this.ShowMsg("删除成功", true);
                this.BindList();
            }
            else
            {
                this.ShowMsg("删除失败", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.leaveList.RowDeleting += new GridViewDeleteEventHandler(this.leaveList_RowDeleting);
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.btnDeleteSelect1.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.statusList.SelectedIndexChanged += new EventHandler(this.statusList_SelectedIndexChanged);
            this.statusList.AutoPostBack = true;
            if (!this.Page.IsPostBack)
            {
                this.BindList();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("MessageStatus", ((int) this.statusList.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

