namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.ClientNew)]
    public class MemberMarket : AdminPage
    {
        private bool? approved;
        protected Button btnExport;
        protected Button btnSearchButton;
        protected Button btnSendEmail;
        protected Button btnSendMessage;
        protected Button btnsitecontent;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected Grid grdMemberList;
        protected HtmlInputHidden hdenableemail;
        protected HtmlInputHidden hdenablemsg;
        protected PageSize hrefPageSize;
        protected Literal litsmscount;
        protected Literal litType;
        protected ImageLinkButton lkbDelectCheck;
        protected ImageLinkButton lkbDelectCheck1;
        protected Pager pager;
        protected Pager pager1;
        private int? rankId;
        protected MemberGradeDropDownList rankList;
        private string realName;
        private string searchKey;
        protected HtmlGenericControl Span1;
        protected HtmlGenericControl Span2;
        protected HtmlGenericControl Span3;
        protected HtmlGenericControl Span4;
        protected HtmlGenericControl Span5;
        protected HtmlGenericControl Span6;
        protected HtmlTextArea txtemailcontent;
        protected HtmlTextArea txtmsgcontent;
        protected TextBox txtRealName;
        protected TextBox txtSearchText;
        protected HtmlTextArea txtsitecontent;

        protected void BindClientList()
        {
            MemberQuery query = new MemberQuery();
            query.Username = this.searchKey;
            query.Realname = this.realName;
            query.GradeId = this.rankId;
            query.PageIndex = this.pager.PageIndex;
            query.IsApproved = this.approved;
            query.SortBy = this.grdMemberList.SortOrderBy;
            query.PageSize = this.pager.PageSize;
            if (this.grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult members = MemberHelper.GetMembers(this.SetClient(query));
            this.grdMemberList.DataSource = members.Data;
            this.grdMemberList.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = members.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            SiteSettings siteSetting = this.GetSiteSetting();
            string str = siteSetting.EmailSender.ToLower();
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("请先选择发送方式", false);
            }
            else
            {
                ConfigData data = null;
                if (siteSetting.EmailEnabled)
                {
                    data = new ConfigData(HiCryptographer.Decrypt(siteSetting.EmailSettings));
                }
                if (data == null)
                {
                    this.ShowMsg("请先选择发送方式并填写配置信息", false);
                }
                else if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                }
                else
                {
                    string str4 = this.txtemailcontent.Value.Trim();
                    if (string.IsNullOrEmpty(str4))
                    {
                        this.ShowMsg("请先填写发送的内容信息", false);
                    }
                    else
                    {
                        string str5 = null;
                        foreach (GridViewRow row in this.grdMemberList.Rows)
                        {
                            CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                            if (box.Checked)
                            {
                                string str6 = ((DataBoundLiteralControl) row.Controls[3].Controls[0]).Text.Trim().Replace("<div></div>", "");
                                if (!string.IsNullOrEmpty(str6) && Regex.IsMatch(str6, @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
                                {
                                    str5 = str5 + str6 + ",";
                                }
                            }
                        }
                        if (str5 == null)
                        {
                            this.ShowMsg("请先选择要发送的会员或检测邮箱格式是否正确", false);
                        }
                        else
                        {
                            str5 = str5.Substring(0, str5.Length - 1);
                            string[] strArray = null;
                            if (str5.Contains(","))
                            {
                                strArray = str5.Split(new char[] { ',' });
                            }
                            else
                            {
                                strArray = new string[] { str5 };
                            }
                            MailMessage message2 = new MailMessage();
                            message2.IsBodyHtml = true;
                            message2.Priority = MailPriority.High;
                            message2.SubjectEncoding = Encoding.UTF8;
                            message2.BodyEncoding = Encoding.UTF8;
                            message2.Body = str4;
                            message2.Subject = "来自" + siteSetting.SiteName;
                            MailMessage mail = message2;
                            foreach (string str7 in strArray)
                            {
                                mail.To.Add(str7);
                            }
                            EmailSender sender2 = EmailSender.CreateInstance(str, data.SettingsXml);
                            try
                            {
                                if (sender2.Send(mail, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                                {
                                    this.ShowMsg("发送邮件成功", true);
                                }
                                else
                                {
                                    this.ShowMsg("发送邮件失败", false);
                                }
                            }
                            catch (Exception)
                            {
                                this.ShowMsg("发送邮件成功,但存在无效的邮箱账号", true);
                            }
                            this.txtemailcontent.Value = "输入发送内容……";
                        }
                    }
                }
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SiteSettings siteSetting = this.GetSiteSetting();
            string sMSSender = siteSetting.SMSSender;
            if (string.IsNullOrEmpty(sMSSender))
            {
                this.ShowMsg("请先选择发送方式", false);
            }
            else
            {
                ConfigData data = null;
                if (siteSetting.SMSEnabled)
                {
                    data = new ConfigData(HiCryptographer.Decrypt(siteSetting.SMSSettings));
                }
                if (data == null)
                {
                    this.ShowMsg("请先选择发送方式并填写配置信息", false);
                }
                else if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    this.ShowMsg(msg, false);
                }
                else
                {
                    string str4 = this.txtmsgcontent.Value.Trim();
                    if (string.IsNullOrEmpty(str4))
                    {
                        this.ShowMsg("请先填写发送的内容信息", false);
                    }
                    else
                    {
                        int num = Convert.ToInt32(this.litsmscount.Text);
                        string str5 = null;
                        foreach (GridViewRow row in this.grdMemberList.Rows)
                        {
                            CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                            if (box.Checked)
                            {
                                string str6 = ((DataBoundLiteralControl) row.Controls[2].Controls[0]).Text.Trim().Replace("<div></div>", "");
                                if (!string.IsNullOrEmpty(str6) && Regex.IsMatch(str6, @"^(13|14|15|18)\d{9}$"))
                                {
                                    str5 = str5 + str6 + ",";
                                }
                            }
                        }
                        if (str5 == null)
                        {
                            this.ShowMsg("请先选择要发送的会员或检测所选手机号格式是否正确", false);
                        }
                        else
                        {
                            str5 = str5.Substring(0, str5.Length - 1);
                            string[] phoneNumbers = null;
                            if (str5.Contains(","))
                            {
                                phoneNumbers = str5.Split(new char[] { ',' });
                            }
                            else
                            {
                                phoneNumbers = new string[] { str5 };
                            }
                            if (num < phoneNumbers.Length)
                            {
                                this.ShowMsg("发送失败，您的剩余短信条数不足", false);
                            }
                            else
                            {
                                string str7;
                                bool success = SMSSender.CreateInstance(sMSSender, data.SettingsXml).Send(phoneNumbers, str4, out str7);
                                this.ShowMsg(str7, success);
                                this.txtmsgcontent.Value = "输入发送内容……";
                                this.litsmscount.Text = (num - phoneNumbers.Length).ToString();
                            }
                        }
                    }
                }
            }
        }

        private void btnsitecontent_Click(object sender, EventArgs e)
        {
            IList<MessageBoxInfo> messageBoxInfos = new List<MessageBoxInfo>();
            string str = this.txtsitecontent.Value.Trim();
            if (string.IsNullOrEmpty(str) || str.Equals("输入发送内容……"))
            {
                this.ShowMsg("请输入要发送的内容信息", false);
            }
            else
            {
                string str2 = str;
                if (str.Length > 10)
                {
                    str2 = str.Substring(0, 10) + "……";
                }
                foreach (GridViewRow row in this.grdMemberList.Rows)
                {
                    CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        string name = ((Literal) row.Controls[1].Controls[1]).Text.Trim();
                        if (this.IsMembers(name))
                        {
                            MessageBoxInfo item = new MessageBoxInfo();
                            item.Sernder = "Admin";
                            item.Accepter = name;
                            item.Title = str2;
                            item.Content = str;
                            messageBoxInfos.Add(item);
                        }
                    }
                }
                if (messageBoxInfos.Count > 0)
                {
                    NoticeHelper.SendMessageToMember(messageBoxInfos);
                    this.txtsitecontent.Value = "输入发送内容……";
                    this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        protected int GetAmount(SiteSettings settings)
        {
            int num = 0;
            if (!string.IsNullOrEmpty(settings.SMSSettings))
            {
                int num2;
                string xml = HiCryptographer.Decrypt(settings.SMSSettings);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                string innerText = document.SelectSingleNode("xml/Appkey").InnerText;
                string postData = "method=getAmount&Appkey=" + innerText;
                string s = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
                if (int.TryParse(s, out num2))
                {
                    num = Convert.ToInt32(s);
                }
            }
            return num;
        }

        private SiteSettings GetSiteSetting()
        {
            return SettingsManager.GetMasterSettings(false);
        }

        private void grdMemberList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int userId = (int) this.grdMemberList.DataKeys[e.RowIndex].Value;
            if (!MemberHelper.Delete(userId))
            {
                this.ShowMsg("未知错误", false);
            }
            else
            {
                this.BindClientList();
                this.ShowMsg("成功删除了选择的会员", true);
            }
        }

        private bool IsMembers(string name)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            new Regex(pattern);
            return ((name.Length >= 2) && (name.Length <= 20));
        }

        private void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int num = 0;
            foreach (GridViewRow row in this.grdMemberList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked && MemberHelper.Delete(Convert.ToInt32(this.grdMemberList.DataKeys[row.RowIndex].Value)))
                {
                    num++;
                }
            }
            if (num == 0)
            {
                this.ShowMsg("请先选择要删除的会员账号", false);
            }
            else
            {
                this.BindClientList();
                this.ShowMsg("成功删除了选择的会员", true);
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["rankId"], out result))
                {
                    this.rankId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
                {
                    this.searchKey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = base.Server.UrlDecode(this.Page.Request.QueryString["realName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
                {
                    this.approved = new bool?(Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
                }
                this.rankList.SelectedValue = this.rankId;
                this.txtSearchText.Text = this.searchKey;
                this.txtRealName.Text = this.realName;
            }
            else
            {
                this.rankId = this.rankList.SelectedValue;
                this.searchKey = this.txtSearchText.Text;
                this.realName = this.txtRealName.Text.Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindClientList();
                this.rankList.DataBind();
                this.rankList.SelectedValue = this.rankId;
                SiteSettings siteSetting = this.GetSiteSetting();
                if (siteSetting.SMSEnabled)
                {
                    this.litsmscount.Text = this.GetAmount(siteSetting).ToString();
                    this.hdenablemsg.Value = "1";
                }
                if (siteSetting.EmailEnabled)
                {
                    this.hdenableemail.Value = "1";
                }
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnSendMessage.Click += new EventHandler(this.btnSendMessage_Click);
            this.btnSendEmail.Click += new EventHandler(this.btnSendEmail_Click);
            this.btnsitecontent.Click += new EventHandler(this.btnsitecontent_Click);
            this.lkbDelectCheck.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.lkbDelectCheck1.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.grdMemberList.RowDeleting += new GridViewDeleteEventHandler(this.grdMemberList_RowDeleting);
        }

        public string PostData(string url, string postData)
        {
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.rankList.SelectedValue.HasValue)
            {
                queryStrings.Add("rankId", this.rankList.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("searchKey", this.txtSearchText.Text);
            queryStrings.Add("realName", this.txtRealName.Text);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("type", this.Page.Request.QueryString["type"]);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected MemberQuery SetClient(MemberQuery query)
        {
            Dictionary<int, MemberClientSet> memberClientSet;
            int[] numArray;
            MemberClientSet set;
            if (!string.IsNullOrEmpty(base.Request.QueryString["type"]))
            {
                memberClientSet = MemberHelper.GetMemberClientSet();
                numArray = new int[memberClientSet.Count];
                memberClientSet.Keys.CopyTo(numArray, 0);
                if (memberClientSet.Count <= 0)
                {
                    return query;
                }
                set = new MemberClientSet();
                string str = base.Request.QueryString["type"];
                query.ClientType = str;
                string str2 = str;
                if (str2 == null)
                {
                    goto Label_01AC;
                }
                if (!(str2 == "new"))
                {
                    if (str2 == "activy")
                    {
                        set = memberClientSet[numArray[1]];
                        this.litType.Text = "活跃客户";
                        if (set.ClientValue > 0M)
                        {
                            query.StartTime = new DateTime?(DateTime.Now.AddDays((double) -set.LastDay));
                            query.EndTime = new DateTime?(DateTime.Now);
                            query.CharSymbol = set.ClientChar;
                            if (set.ClientTypeId == 6)
                            {
                                query.OrderNumber = new int?((int) set.ClientValue);
                                return query;
                            }
                            query.OrderMoney = new decimal?(set.ClientValue);
                        }
                        return query;
                    }
                    goto Label_01AC;
                }
                set = memberClientSet[numArray[0]];
                this.litType.Text = "新客户";
                query.StartTime = set.StartTime;
                query.EndTime = set.EndTime;
                if (set.LastDay > 0)
                {
                    query.StartTime = new DateTime?(DateTime.Now.AddDays((double) -set.LastDay));
                    query.EndTime = new DateTime?(DateTime.Now);
                }
            }
            return query;
        Label_01AC:
            set = memberClientSet[numArray[2]];
            this.litType.Text = "睡眠客户";
            query.StartTime = new DateTime?(DateTime.Now.AddDays((double) -set.LastDay));
            query.EndTime = new DateTime?(DateTime.Now);
            return query;
        }
    }
}

