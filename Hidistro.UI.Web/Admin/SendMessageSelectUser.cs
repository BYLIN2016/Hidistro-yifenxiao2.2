namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.AddMessage)]
    public class SendMessageSelectUser : AdminPage
    {
        protected Button btnSendToRank;
        protected MemberGradeDropDownList rankList;
        protected HtmlInputRadioButton rdoName;
        protected HtmlInputRadioButton rdoRank;
        protected TextBox txtMemberNames;
        private int userId;

        private void btnSendToRank_Click(object sender, EventArgs e)
        {
            IList<MessageBoxInfo> messageBoxInfos = new List<MessageBoxInfo>();
            if (this.rdoName.Checked && !string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
            {
                string[] strArray = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (this.GetMember(strArray[i]) != null)
                    {
                        MessageBoxInfo item = new MessageBoxInfo();
                        item.Accepter = strArray[i];
                        item.Sernder = "admin";
                        item.Title = this.MessageTitle;
                        item.Content = this.Content;
                        messageBoxInfos.Add(item);
                    }
                }
                if (messageBoxInfos.Count <= 0)
                {
                    this.ShowMsg("没有要发送的对象", false);
                    return;
                }
                NoticeHelper.SendMessageToMember(messageBoxInfos);
                this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                IList<Member> list2 = new List<Member>();
                foreach (Member member2 in NoticeHelper.GetMembersByRank(this.rankList.SelectedValue))
                {
                    MessageBoxInfo info2 = new MessageBoxInfo();
                    info2.Accepter = member2.Username;
                    info2.Sernder = "admin";
                    info2.Title = this.MessageTitle;
                    info2.Content = this.Content;
                    messageBoxInfos.Add(info2);
                }
                if (messageBoxInfos.Count > 0)
                {
                    NoticeHelper.SendMessageToMember(messageBoxInfos);
                    this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        private Member GetMember(string name)
        {
            Member member = Users.FindUserByUsername(name) as Member;
            if ((member != null) && (member.UserRole == UserRole.Member))
            {
                return member;
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSendToRank.Click += new EventHandler(this.btnSendToRank_Click);
                if (!this.Page.IsPostBack)
                {
                    this.rankList.DataBind();
                    if (this.userId > 0)
                    {
                        Member user = Users.GetUser(this.userId) as Member;
                        if (user == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else
                        {
                            this.txtMemberNames.Text = user.Username;
                        }
                    }
                }
            }
        }

        public string Content
        {
            get
            {
                if (this.Session["Content"] != null)
                {
                    return Globals.UrlDecode(this.Session["Content"].ToString());
                }
                return string.Empty;
            }
        }

        public string MessageTitle
        {
            get
            {
                if (this.Session["Title"] != null)
                {
                    return Globals.UrlDecode(this.Session["Title"].ToString());
                }
                return string.Empty;
            }
        }
    }
}

