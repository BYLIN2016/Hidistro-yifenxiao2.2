namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    public class UserReceivedMessages : MemberTemplatedWebControl
    {
        private IButton btnDeleteSelect;
        private Common_Messages_UserReceivedMessageList CmessagesList;
        private Grid messagesList;
        private Pager pager;

        protected override void AttachChildControls()
        {
            this.CmessagesList = (Common_Messages_UserReceivedMessageList) this.FindControl("Grid_Common_Messages_UserReceivedMessageList");
            this.messagesList = (Grid) this.CmessagesList.FindControl("gridMessageList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("收件箱", HiContext.Current.Context);
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void BindData()
        {
            MessageBoxQuery query = new MessageBoxQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.Accepter = HiContext.Current.User.Username;
            DbQueryResult memberReceivedMessages = CommentsHelper.GetMemberReceivedMessages(query);
            if (((DataTable) memberReceivedMessages.Data).Rows.Count <= 0)
            {
                query.PageIndex = this.messagesList.PageIndex - 1;
                memberReceivedMessages = CommentsHelper.GetMemberReceivedMessages(query);
                this.messagesList.DataSource = memberReceivedMessages.Data;
            }
            this.messagesList.DataSource = memberReceivedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = memberReceivedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> messageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    Label label = (Label) row.FindControl("lblMessage");
                    if (label != null)
                    {
                        messageList.Add(Convert.ToInt64(label.Text));
                    }
                }
            }
            if (messageList.Count > 0)
            {
                CommentsHelper.DeleteMemberMessages(messageList);
            }
            else
            {
                this.ShowMessage("请选中要删除的收件", false);
            }
            this.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserReceivedMessages.html";
            }
            base.OnInit(e);
        }
    }
}

