namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class MyReceivedMessages : DistributorPage
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
            query.Accepter = HiContext.Current.User.Username;
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus) int.Parse(base.Request.QueryString["MessageStatus"]);
                this.statusList.SelectedValue = query.MessageStatus;
            }
            DbQueryResult receivedMessages = SubsiteCommentsHelper.GetReceivedMessages(query, UserRole.Member);
            this.messagesList.DataSource = receivedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = receivedMessages.TotalRecords;
            this.pager1.TotalRecords = receivedMessages.TotalRecords;
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
                SubsiteCommentsHelper.DeleteMessages(messageList);
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
            this.statusList.AutoPostBack = true;
            if (!this.Page.IsPostBack)
            {
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

