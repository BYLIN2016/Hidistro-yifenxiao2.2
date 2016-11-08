namespace Hidistro.UI.Web.Admin
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Distribution;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Plugins;
    using Ionic.Zlib;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
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

    [PrivilegeCheck(Privilege.Distributor)]
    public class ManageDistributor : AdminPage
    {
        protected Button btnExport;
        protected Button btnSearchButton;
        protected Button btnSendEmail;
        protected Button btnSendMessage;
        protected Button btnsitecontent;
        protected DistributorGradeDropDownList dropGrade;
        protected ExportFieldsCheckBoxList exportFieldsCheckBoxList;
        protected ExportFormatRadioButtonList exportFormatRadioButtonList;
        private int? gradeId;
        protected Grid grdDistributorList;
        protected HtmlInputHidden hdenableemail;
        protected HtmlInputHidden hdenablemsg;
        protected PageSize hrefPageSize;
        protected HtmlGenericControl lblAccountAmount;
        protected HtmlGenericControl lblDrawRequestBalance;
        protected HtmlGenericControl lblFreezeBalance;
        protected HtmlGenericControl lblUseableBalance;
        private int? lineId;
        protected HtmlGenericControl litName;
        protected Literal litsmscount;
        protected HtmlGenericControl litUser;
        protected Pager pager;
        protected Pager pager1;
        private string realName;
        protected HtmlGenericControl Span1;
        protected HtmlGenericControl Span2;
        protected HtmlGenericControl Span3;
        protected HtmlTextArea txtemailcontent;
        protected HtmlTextArea txtmsgcontent;
        protected HtmlTextArea txtsitecontent;
        protected TextBox txtTrueName;
        protected TextBox txtUserName;
        private string userName;

        private void BindDistributors()
        {
            DistributorQuery query = new DistributorQuery();
            query.IsApproved = true;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.GradeId = this.gradeId;
            query.LineId = this.lineId;
            query.Username = this.userName;
            query.RealName = this.realName;
            query.SortBy = this.grdDistributorList.SortOrderBy;
            if (this.grdDistributorList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult distributors = DistributorHelper.GetDistributors(query);
            this.grdDistributorList.DataSource = distributors.Data;
            this.grdDistributorList.DataBind();
            this.pager.TotalRecords = distributors.TotalRecords;
            this.pager1.TotalRecords = distributors.TotalRecords;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.exportFieldsCheckBoxList.SelectedItem == null)
            {
                this.ShowMsg("请选择需要导出的分销商信息", false);
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
                DistributorQuery query = new DistributorQuery();
                query.GradeId = this.gradeId;
                query.Username = this.userName;
                query.RealName = this.realName;
                DataTable distributorsNopage = DistributorHelper.GetDistributorsNopage(query, fields);
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
                foreach (DataRow row in distributorsNopage.Rows)
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
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.csv");
                    this.Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.txt");
                    this.Page.Response.ContentType = "application/vnd.ms-word";
                }
                this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                this.Page.EnableViewState = false;
                this.Page.Response.Write(builder.ToString());
                this.Page.Response.End();
            }
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
                        foreach (GridViewRow row in this.grdDistributorList.Rows)
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
                            catch (Exception exception)
                            {
                                this.ShowMsg(exception.Message, false);
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
                        foreach (GridViewRow row in this.grdDistributorList.Rows)
                        {
                            CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                            if (box.Checked)
                            {
                                string str6 = ((DataBoundLiteralControl) row.Controls[4].Controls[0]).Text.Trim().Replace("<div></div>", "");
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
                foreach (GridViewRow row in this.grdDistributorList.Rows)
                {
                    CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        string name = "";
                        foreach (object obj2 in row.Controls[1].Controls)
                        {
                            if (obj2 is Literal)
                            {
                                name = ((Literal) obj2).Text.Trim();
                                break;
                            }
                            if (obj2 is DataBoundLiteralControl)
                            {
                                name = ((DataBoundLiteralControl) obj2).Text.Trim();
                                break;
                            }
                        }
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
                    NoticeHelper.SendMessageToDistributor(messageBoxInfos);
                    this.ShowMsg(string.Format("成功给{0}个分销商发送了消息.", messageBoxInfos.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        private void CallBack()
        {
            bool flag = !string.IsNullOrEmpty(base.Request["showMessage"]) && (base.Request["showMessage"] == "true");
            if (!string.IsNullOrEmpty(base.Request["showDistributorAccountSummary"]) && (base.Request["showDistributorAccountSummary"] == "true"))
            {
                int result = 0;
                if (string.IsNullOrEmpty(base.Request["id"]) || !int.TryParse(base.Request["id"], out result))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(result);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"AccountAmount\":\"{0}\"", distributor.Balance);
                builder.AppendFormat(",\"UseableBalance\":\"{0}\"", distributor.Balance - distributor.RequestBalance);
                builder.AppendFormat(",\"FreezeBalance\":\"{0}\"", distributor.RequestBalance);
                builder.AppendFormat(",\"DrawRequestBalance\":\"{0}\"", distributor.RequestBalance);
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            if (flag)
            {
                int num2 = 0;
                if (string.IsNullOrEmpty(base.Request["id"]) || !int.TryParse(base.Request["id"], out num2))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                }
                else
                {
                    Distributor distributor2 = DistributorHelper.GetDistributor(num2);
                    if (distributor2 == null)
                    {
                        base.Response.Write("{\"Status\":\"0\"}");
                        base.Response.End();
                    }
                    else
                    {
                        StringBuilder builder2 = new StringBuilder();
                        builder2.AppendFormat(",\"UserName\":\"{0}\"", distributor2.Username);
                        builder2.AppendFormat(",\"RealName\":\"{0}\"", distributor2.RealName);
                        builder2.AppendFormat(",\"CompanyName\":\"{0}\"", distributor2.CompanyName);
                        builder2.AppendFormat(",\"Email\":\"{0}\"", distributor2.Email);
                        builder2.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor2.RegionId, string.Empty));
                        builder2.AppendFormat(",\"Address\":\"{0}\"", distributor2.Address);
                        builder2.AppendFormat(",\"QQ\":\"{0}\"", distributor2.QQ);
                        builder2.AppendFormat(",\"MSN\":\"{0}\"", distributor2.MSN);
                        builder2.AppendFormat(",\"PostCode\":\"{0}\"", distributor2.Zipcode);
                        builder2.AppendFormat(",\"Wangwang\":\"{0}\"", distributor2.Wangwang);
                        builder2.AppendFormat(",\"CellPhone\":\"{0}\"", distributor2.CellPhone);
                        builder2.AppendFormat(",\"Telephone\":\"{0}\"", distributor2.TelPhone);
                        builder2.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor2.CreateDate);
                        builder2.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor2.LastLoginDate);
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{\"Status\":\"1\"" + builder2.ToString() + "}");
                        base.Response.End();
                    }
                }
            }
        }

        private bool DeleteDistributorFile(int distributorUserId)
        {
            string path = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + distributorUserId;
            string str2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + distributorUserId;
            if (Directory.Exists(path) && Directory.Exists(str2))
            {
                try
                {
                    DeleteFolder(path);
                    DeleteFolder(str2);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static void DeleteFolder(string dir)
        {
            foreach (string str in Directory.GetFileSystemEntries(dir))
            {
                if (System.IO.File.Exists(str))
                {
                    FileInfo info = new FileInfo(str);
                    if (info.Attributes.ToString().IndexOf("Readonly") != 1)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    System.IO.File.Delete(str);
                }
                else
                {
                    DeleteFolder(str);
                }
            }
            Directory.Delete(dir);
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

        private void grdDistributorList_RowCommand(object serder, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int distributorUserId = (int) this.grdDistributorList.DataKeys[rowIndex].Value;
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(distributorUserId);
            if (e.CommandName == "StopCooperation")
            {
                if ((siteSettings != null) && !siteSettings.Disabled)
                {
                    this.ShowMsg("请先暂停该分销商的站点", false);
                }
                else if (DistributorHelper.Delete(distributorUserId))
                {
                    this.DeleteDistributorFile(distributorUserId);
                    this.BindDistributors();
                    this.ShowMsg("成功的清除了该分销商及该分销商下的所有数据", true);
                }
                else
                {
                    this.ShowMsg("清除去失败", false);
                }
            }
        }

        private bool IsMembers(string name)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            new Regex(pattern);
            return ((name.Length >= 2) && (name.Length <= 20));
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
                {
                    this.userName = this.Page.Request.QueryString["userName"];
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["realName"]))
                {
                    this.realName = this.Page.Request.QueryString["realName"];
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["gradeId"], out result))
                {
                    this.gradeId = new int?(result);
                }
                int num2 = 0;
                if (int.TryParse(this.Page.Request.QueryString["LineId"], out num2))
                {
                    this.lineId = new int?(num2);
                }
                this.txtUserName.Text = this.userName;
                this.txtTrueName.Text = this.realName;
                this.dropGrade.DataBind();
                this.dropGrade.SelectedValue = this.gradeId;
            }
            else
            {
                this.userName = this.txtUserName.Text;
                this.realName = this.txtTrueName.Text;
                if (this.dropGrade.SelectedValue.HasValue)
                {
                    this.gradeId = new int?(this.dropGrade.SelectedValue.Value);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this.grdDistributorList.RowCommand += new GridViewCommandEventHandler(this.grdDistributorList_RowCommand);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.btnSendMessage.Click += new EventHandler(this.btnSendMessage_Click);
            this.btnSendEmail.Click += new EventHandler(this.btnSendEmail_Click);
            this.btnsitecontent.Click += new EventHandler(this.btnsitecontent_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CallBack();
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindDistributors();
                this.exportFieldsCheckBoxList.Items.Remove(new ListItem("积分", "Points"));
                this.exportFieldsCheckBoxList.Items.Remove(new ListItem("生日", "BirthDate"));
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

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", this.txtUserName.Text.Trim());
            queryStrings.Add("realName", this.txtTrueName.Text.Trim());
            queryStrings.Add("gradeId", this.dropGrade.SelectedValue.HasValue ? this.dropGrade.SelectedValue.Value.ToString() : "");
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

