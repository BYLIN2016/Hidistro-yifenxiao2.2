namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorAcceptMsg)]
    public class DistributorReceivedMessages : AdminPage
    {
        protected ImageLinkButton btnDeleteSelect;
        protected ImageLinkButton btnDeleteSelect1;
        protected PageSize hrefPageSize;
        protected Grid messagesList;
        protected Pager pager;
        protected Pager pager1;
        protected MessageStatusDropDownList statusList;

        private void BindData()
        {
            MessageBoxQuery query = new MessageBoxQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.Accepter = "admin";
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus) int.Parse(base.Request.QueryString["MessageStatus"]);
                this.statusList.SelectedValue = query.MessageStatus;
            }
            DbQueryResult managerReceivedMessages = NoticeHelper.GetManagerReceivedMessages(query, UserRole.Distributor);
            this.messagesList.DataSource = managerReceivedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = managerReceivedMessages.TotalRecords;
            this.pager1.TotalRecords = managerReceivedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> messageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long) this.messagesList.DataKeys[row.RowIndex].Value;
                    messageList.Add(item);
                }
            }
            if (messageList.Count > 0)
            {
                NoticeHelper.DeleteManagerMessages(messageList);
                this.ShowMsg("成功删除了选择的消息.", true);
            }
            else
            {
                this.ShowMsg("请选择需要删除的消息.", false);
            }
            this.BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.btnDeleteSelect1.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.statusList.SelectedIndexChanged += new EventHandler(this.statusList_SelectedIndexChanged);
            if (!this.Page.IsPostBack)
            {
                this.statusList.DataBind();
                this.statusList.AutoPostBack = true;
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("MessageStatus", ((int) this.statusList.SelectedValue).ToString());
            if (!string.IsNullOrEmpty(base.Request.QueryString["IsRead"]))
            {
                queryStrings.Add("IsRead", base.Request.QueryString["IsRead"]);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

