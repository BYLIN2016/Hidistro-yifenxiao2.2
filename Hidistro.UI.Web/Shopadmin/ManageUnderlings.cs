namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Members;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ManageUnderlings : DistributorPage
    {
        protected Button btnExport;
        protected Button btnSearch;
        protected Button btnSendEmail;
        protected Button btnSendMessage;
        protected Button btnsitecontent;
        protected ApprovedDropDownList ddlApproved;
        protected UnderlingGradeDropDownList dropMemberGrade;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        protected Grid grdUnderlings;
        protected HtmlInputHidden hdenableemail;
        protected HtmlInputHidden hdenablemsg;
        protected PageSize hrefPageSize;
        protected Literal litsmscount;
        protected ImageLinkButton lkbDelectCheck;
        protected ImageLinkButton lkbDelectCheck1;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlGenericControl Span1;
        protected HtmlGenericControl Span2;
        protected HtmlGenericControl Span3;
        protected HtmlGenericControl Span4;
        protected HtmlGenericControl Span5;
        protected HtmlGenericControl Span6;
        protected HtmlTextArea txtemailcontent;
        protected HtmlTextArea txtmsgcontent;
        protected TextBox txtRealName;
        protected HtmlTextArea txtsitecontent;
        protected TextBox txtUsername;

        private void BindData()
        {
            MemberQuery memberQuery = this.GetMemberQuery();
            DbQueryResult members = UnderlingHelper.GetMembers(memberQuery);
            this.grdUnderlings.DataSource = members.Data;
            this.grdUnderlings.DataBind();
            this.pager.TotalRecords = members.TotalRecords;
            this.pager1.TotalRecords = members.TotalRecords;
            this.txtUsername.Text = memberQuery.Username;
            this.txtRealName.Text = memberQuery.Realname;
            this.dropMemberGrade.SelectedValue = memberQuery.GradeId;
            this.ddlApproved.SelectedValue = memberQuery.IsApproved;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.exportFieldsCheckBoxList.SelectedItem == null)
            {
                this.ShowMsg("请选择需要导出的会员信息", false);
            }
            else
            {
                IList<string> fields = new List<string>();
                IList<string> list2 = new List<string>();
                foreach (ListItem item in this.exportFieldsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        fields.Add(item.Value);
                        list2.Add(item.Text);
                    }
                }
                MemberQuery query = new MemberQuery();
                query.Username = this.txtUsername.Text.Trim();
                query.Realname = this.txtRealName.Text.Trim();
                query.GradeId = this.dropMemberGrade.SelectedValue;
                DataTable membersNopage = UnderlingHelper.GetMembersNopage(query, fields);
                StringBuilder builder = new StringBuilder();
                foreach (string str in list2)
                {
                    builder.Append(str + ",");
                    if (str == list2[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
                foreach (DataRow row in membersNopage.Rows)
                {
                    foreach (string str2 in fields)
                    {
                        builder.Append(row[str2]).Append(",");
                        if (str2 == fields[list2.Count - 1])
                        {
                            builder = builder.Remove(builder.Length - 1, 1);
                            builder.Append("\r\n");
                        }
                    }
                }
                this.Page.Response.Clear();
                this.Page.Response.Buffer = false;
                this.Page.Response.Charset = "GB2312";
                if (this.exportFormatRadioButtonList.SelectedValue == "csv")
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
                    this.Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
                    this.Page.Response.ContentType = "application/vnd.ms-word";
                }
                this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                this.Page.EnableViewState = false;
                this.Page.Response.Write(builder.ToString());
                this.Page.Response.End();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadManageUnderlings(true);
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
                        foreach (GridViewRow row in this.grdUnderlings.Rows)
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
                        foreach (GridViewRow row in this.grdUnderlings.Rows)
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
                string username = HiContext.Current.User.Username;
                foreach (GridViewRow row in this.grdUnderlings.Rows)
                {
                    CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        string name = ((Literal) row.Controls[1].Controls[1]).Text.Trim();
                        if (this.IsMembers(name))
                        {
                            MessageBoxInfo item = new MessageBoxInfo();
                            item.Sernder = username;
                            item.Accepter = name;
                            item.Title = str2;
                            item.Content = str;
                            messageBoxInfos.Add(item);
                        }
                    }
                }
                if (messageBoxInfos.Count > 0)
                {
                    SubsiteCommentsHelper.SendMessageToMember(messageBoxInfos);
                    this.txtsitecontent.Value = "输入发送内容……";
                    this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", messageBoxInfos.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        private void ddlApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReloadManageUnderlings(false);
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

        private MemberQuery GetMemberQuery()
        {
            MemberQuery query = new MemberQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GradeId"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["GradeId"], out result))
                {
                    query.GradeId = new int?(result);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
            {
                query.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Realname"]))
            {
                query.Realname = base.Server.UrlDecode(this.Page.Request.QueryString["Realname"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Approved"]))
            {
                query.IsApproved = new bool?(Convert.ToBoolean(this.Page.Request.QueryString["Approved"]));
            }
            query.PageSize = this.pager.PageSize;
            query.PageIndex = this.pager.PageIndex;
            return query;
        }

        private SiteSettings GetSiteSetting()
        {
            return SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
        }

        protected void grdUnderlings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = (int) this.grdUnderlings.DataKeys[e.RowIndex].Value;
            if (UnderlingHelper.DeleteMember(userId))
            {
                this.BindData();
                this.ShowMsg("成功删除了选择的会员", true);
            }
            else
            {
                this.ShowMsg("未知错误", false);
            }
        }

        private bool IsMembers(string name)
        {
            return ((name.Length >= 2) && (name.Length <= 20));
        }

        protected void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in this.grdUnderlings.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if (box.Checked && UnderlingHelper.DeleteMember(Convert.ToInt32(this.grdUnderlings.DataKeys[row.RowIndex].Value)))
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
                this.BindData();
                this.ShowMsg("成功删除了选择的会员", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdUnderlings.RowDeleting += new GridViewDeleteEventHandler(this.grdUnderlings_RowDeleting);
            this.lkbDelectCheck.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.lkbDelectCheck1.Click += new EventHandler(this.lkbDelectCheck_Click);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnsitecontent.Click += new EventHandler(this.btnsitecontent_Click);
            this.ddlApproved.AutoPostBack = true;
            this.ddlApproved.SelectedIndexChanged += new EventHandler(this.ddlApproved_SelectedIndexChanged);
            this.btnSendMessage.Click += new EventHandler(this.btnSendMessage_Click);
            this.btnSendEmail.Click += new EventHandler(this.btnSendEmail_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropMemberGrade.DataBind();
                this.ddlApproved.DataBind();
                this.BindData();
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

        private void ReloadManageUnderlings(bool isSeach)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (this.dropMemberGrade.SelectedValue.HasValue)
            {
                queryStrings.Add("GradeId", this.dropMemberGrade.SelectedValue.Value.ToString());
            }
            queryStrings.Add("Username", this.txtUsername.Text);
            queryStrings.Add("Realname", this.txtRealName.Text);
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            queryStrings.Add("Approved", this.ddlApproved.SelectedValue.ToString());
            if (!isSeach)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

