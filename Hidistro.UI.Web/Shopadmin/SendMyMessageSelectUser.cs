namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Subsites.Comments;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendMyMessageSelectUser : DistributorPage
    {
        protected Button btnSendToRank;
        protected UnderlingGradeDropDownList rankList;
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
                        item.Sernder = HiContext.Current.User.Username;
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
                SubsiteCommentsHelper.SendMessageToMember(messageBoxInfos);
                this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                IList<Member> list2 = new List<Member>();
                foreach (Member member2 in SubsitePromoteHelper.GetMembersByRank(this.rankList.SelectedValue))
                {
                    MessageBoxInfo info2 = new MessageBoxInfo();
                    info2.Accepter = member2.Username;
                    info2.Sernder = HiContext.Current.User.Username;
                    info2.Title = this.MessageTitle;
                    info2.Content = this.Content;
                    messageBoxInfos.Add(info2);
                }
                if (messageBoxInfos.Count > 0)
                {
                    SubsiteCommentsHelper.SendMessageToMember(messageBoxInfos);
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
            if (((member != null) && (member.UserRole == UserRole.Underling)) && (member.ParentUserId.Value == HiContext.Current.User.UserId))
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
                            return;
                        }
                        this.txtMemberNames.Text = user.Username;
                    }
                }
                CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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

