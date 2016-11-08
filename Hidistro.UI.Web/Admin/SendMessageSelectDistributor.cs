namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Messages;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SendMessageSelectDistributor : AdminPage
    {
        protected Button btnSendToRank;
        protected CheckBox chkIsSendEmail;
        protected DistributorGradeDropDownList rankList;
        protected HtmlInputRadioButton rdoName;
        protected HtmlInputRadioButton rdoRank;
        protected TextBox txtDistributorNames;
        private int userId;

        private void btnSendToRank_Click(object sender, EventArgs e)
        {
            IList<MessageBoxInfo> messageBoxInfos = new List<MessageBoxInfo>();
            if (this.rdoName.Checked)
            {
                if (string.IsNullOrEmpty(this.txtDistributorNames.Text.Trim()))
                {
                    this.ShowMsg("请输入您要发送的用户", false);
                    return;
                }
                string[] strArray = this.txtDistributorNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    Distributor distributor = this.GetDistributor(strArray[i]);
                    if (distributor != null)
                    {
                        MessageBoxInfo item = new MessageBoxInfo();
                        item.Accepter = strArray[i];
                        item.Sernder = "admin";
                        item.Title = this.MessageTitle;
                        item.Content = this.Content;
                        messageBoxInfos.Add(item);
                        if (this.chkIsSendEmail.Checked && Regex.IsMatch(distributor.Email, @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                        {
                            string str2;
                            Messenger.SendMail(this.MessageTitle, this.Content, distributor.Email, HiContext.Current.SiteSettings, out str2);
                        }
                    }
                }
                if (messageBoxInfos.Count <= 0)
                {
                    this.ShowMsg("没有要发送的对象", false);
                    return;
                }
                NoticeHelper.SendMessageToDistributor(messageBoxInfos);
                this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                IList<Distributor> list2 = new List<Distributor>();
                foreach (Distributor distributor2 in NoticeHelper.GetDistributorsByRank(this.rankList.SelectedValue))
                {
                    MessageBoxInfo info2 = new MessageBoxInfo();
                    info2.Accepter = distributor2.Username;
                    info2.Sernder = "admin";
                    info2.Title = this.MessageTitle;
                    info2.Content = this.Content;
                    messageBoxInfos.Add(info2);
                    if (this.chkIsSendEmail.Checked && Regex.IsMatch(distributor2.Email, @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                    {
                        string str3;
                        Messenger.SendMail(this.MessageTitle, this.Content, distributor2.Email, HiContext.Current.SiteSettings, out str3);
                    }
                }
                if (messageBoxInfos.Count > 0)
                {
                    NoticeHelper.SendMessageToDistributor(messageBoxInfos);
                    this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        private Distributor GetDistributor(string name)
        {
            Distributor distributor = null;
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if ((regex.IsMatch(name) && (name.Length >= 2)) && (name.Length <= 20))
            {
                distributor = Users.FindUserByUsername(name) as Distributor;
            }
            return distributor;
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
                        Distributor distributor = DistributorHelper.GetDistributor(this.userId);
                        if (distributor == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else
                        {
                            this.txtDistributorNames.Text = distributor.Username;
                        }
                    }
                }
            }
        }

        public string Content
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Session["Content"].ToString()))
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
                if (!string.IsNullOrEmpty(this.Session["Title"].ToString()))
                {
                    return Globals.UrlDecode(this.Session["Title"].ToString());
                }
                return string.Empty;
            }
        }
    }
}

