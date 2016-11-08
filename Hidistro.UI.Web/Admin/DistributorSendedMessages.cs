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
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.DistributorSendedMsg)]
    public class DistributorSendedMessages : AdminPage
    {
        protected ImageLinkButton btnDeleteSelect;
        protected ImageLinkButton btnDeleteSelect1;
        protected PageSize hrefPageSize;
        protected Grid messagesList;
        protected Pager pager;
        protected Pager pager1;

        private void BindData()
        {
            MessageBoxQuery query = new MessageBoxQuery();
            query.Sernder = "admin";
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            DbQueryResult managerSendedMessages = NoticeHelper.GetManagerSendedMessages(query, UserRole.Distributor);
            this.messagesList.DataSource = managerSendedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = managerSendedMessages.TotalRecords;
            this.pager1.TotalRecords = managerSendedMessages.TotalRecords;
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
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
    }
}

