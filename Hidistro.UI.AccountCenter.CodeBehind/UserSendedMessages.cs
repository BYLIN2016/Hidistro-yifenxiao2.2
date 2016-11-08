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
    using System.Web.UI.WebControls;

    public class UserSendedMessages : MemberTemplatedWebControl
    {
        private IButton btnDeleteSelect;
        private Common_Messages_UserSendedMessageList CmessagesList;
        private Grid messagesList;
        private Pager pager;

        protected override void AttachChildControls()
        {
            this.CmessagesList = (Common_Messages_UserSendedMessageList) this.FindControl("Grid_Common_Messages_UserSendedMessageList");
            this.messagesList = (Grid) this.CmessagesList.FindControl("messagesList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void BindData()
        {
            MessageBoxQuery query = new MessageBoxQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.Sernder = HiContext.Current.User.Username;
            DbQueryResult memberSendedMessages = CommentsHelper.GetMemberSendedMessages(query);
            this.messagesList.DataSource = memberSendedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = memberSendedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> messageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    messageList.Add(Convert.ToInt64(this.messagesList.DataKeys[row.RowIndex].Value));
                }
            }
            if (messageList.Count > 0)
            {
                CommentsHelper.DeleteMemberMessages(messageList);
                this.BindData();
            }
            else
            {
                this.ShowMessage("请选中要删除的信息", false);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserSendedMessages.html";
            }
            base.OnInit(e);
        }
    }
}

