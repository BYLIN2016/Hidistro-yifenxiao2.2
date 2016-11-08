namespace Hidistro.UI.Web.Installer
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Configuration;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Install : Page
    {
        private string action;
        protected Button btnInstall;
        protected CheckBox chkIsAddDemo;
        private string dbName;
        private string dbPassword;
        private string dbServer;
        private string dbUsername;
        private string email;
        private IList<string> errorMsgs;
        protected HtmlForm form1;
        private bool isAddDemo;
        protected Label lblErrMessage;
        protected Label litSetpErrorMessage;
        private string password;
        private string password2;
        private string siteDescription;
        private string siteName;
        private bool testSuccessed;
        protected TextBox txtDbName;
        protected TextBox txtDbPassword;
        protected TextBox txtDbServer;
        protected TextBox txtDbUsername;
        protected TextBox txtEmail;
        protected TextBox txtPassword;
        protected TextBox txtPassword2;
        protected TextBox txtSiteDescription;
        protected TextBox txtSiteName;
        protected TextBox txtUsername;
        private string username;

        private bool AddBuiltInRoles(out string errorMsg)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                using (connection = new SqlConnection(this.GetConnectionString()))
                {
                    connection.Open();
                    DbCommand command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO aspnet_Roles(RoleName, LoweredRoleName) VALUES(@RoleName, LOWER(@RoleName))";
                    DbParameter parameter = new SqlParameter("@RoleName", SqlDbType.NVarChar, 0x100);
                    command.Parameters.Add(parameter);
                    RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Distributor;
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Member;
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.SystemAdministrator;
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Underling;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();
                }
                errorMsg = null;
                return true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        errorMsg = exception2.Message;
                    }
                }
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
                return false;
            }
        }

        private bool AddDemoData(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
        }

        private bool AddInitData(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!this.ValidateUser(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.testSuccessed && !this.ExecuteTest())
            {
                this.ShowMsg("数据库链接信息有误", false);
            }
            else if (!this.CreateDataSchema(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.AddBuiltInRoles(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else if (!this.CreateAnonymous(out msg))
            {
                this.ShowMsg(msg, false);
            }
            else
            {
                int num;
                if (!this.CreateAdministrator(out num, out msg))
                {
                    this.ShowMsg(msg, false);
                }
                else if (!this.AddInitData(out msg))
                {
                    this.ShowMsg(msg, false);
                }
                else if (!this.isAddDemo || this.AddDemoData(out msg))
                {
                    if (!this.SaveSiteSettings(out msg))
                    {
                        this.ShowMsg(msg, false);
                    }
                    else if (!this.SaveConfig(out msg))
                    {
                        this.ShowMsg(msg, false);
                    }
                    else
                    {
                        this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
                    }
                }
            }
        }

        private bool CreateAdministrator(out int newUserId, out string errorMsg)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                using (connection = new SqlConnection(this.GetConnectionString()))
                {
                    connection.Open();
                    RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
                    DbCommand command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT RoleId FROM aspnet_Roles WHERE [LoweredRoleName] = LOWER(@RoleName)";
                    command.Parameters.Add(new SqlParameter("@RoleName", rolesConfiguration.SystemAdministrator));
                    Guid guid = (Guid) command.ExecuteScalar();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
                    Guid guid2 = (Guid) command.ExecuteScalar();
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, Email, LoweredEmail) VALUES (@Username, LOWER(@Username), 0, @UserRole, @CreateDate, @Password, @PasswordFormat, @PasswordSalt, 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), @Email, LOWER(@Email));SELECT @@IDENTITY";
                    command.Parameters.Add(new SqlParameter("@Username", this.username));
                    command.Parameters.Add(new SqlParameter("@UserRole", UserRole.SiteManager));
                    command.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@Password", this.password));
                    command.Parameters.Add(new SqlParameter("@PasswordFormat", MembershipPasswordFormat.Clear));
                    command.Parameters.Add(new SqlParameter("@PasswordSalt", ""));
                    command.Parameters.Add(new SqlParameter("@Email", this.email));
                    newUserId = Convert.ToInt32(command.ExecuteScalar());
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO aspnet_Managers(UserId) VALUES(@UserId)";
                    command.Parameters.Add(new SqlParameter("@UserId", (int) newUserId));
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO aspnet_UsersInRoles(UserId, RoleId) VALUES(@UserId, @RoleId)";
                    command.Parameters.Add(new SqlParameter("@RoleId", guid2));
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleId"].Value = guid;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();
                }
                errorMsg = null;
                return true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                newUserId = 0;
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        errorMsg = exception2.Message;
                    }
                }
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
                return false;
            }
        }

        private bool CreateAnonymous(out string errorMsg)
        {
            DbConnection connection = null;
            try
            {
                using (connection = new SqlConnection(this.GetConnectionString()))
                {
                    DbCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) VALUES ('Anonymous', LOWER('Anonymous'), 1, @UserRole, @CreateDate, 'DVZTktxeMzDtXR7eik7Cdw==', 0, '', 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ))";
                    command.Parameters.Add(new SqlParameter("@UserRole", UserRole.Anonymous));
                    command.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                errorMsg = null;
                return true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
                return false;
            }
        }

        private bool CreateDataSchema(out string errorMsg)
        {
            string path = base.Request.MapPath("SqlScripts/Schema.sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到数据库架构文件-Schema.sql";
                return false;
            }
            return this.ExecuteScriptFile(path, out errorMsg);
        }

        private static string CreateKey(int len)
        {
            byte[] data = new byte[len];
            new RNGCryptoServiceProvider().GetBytes(data);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));
            }
            return builder.ToString();
        }

        private bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
        {
            StreamReader reader = null;
            SqlConnection connection = null;
            try
            {
                string applicationPath = Globals.ApplicationPath;
                using (reader = new StreamReader(pathToScriptFile))
                {
                    using (connection = new SqlConnection(this.GetConnectionString()))
                    {
                        SqlCommand command2 = new SqlCommand();
                        command2.Connection = connection;
                        command2.CommandType = CommandType.Text;
                        command2.CommandTimeout = 60;
                        DbCommand command = command2;
                        connection.Open();
                        while (!reader.EndOfStream)
                        {
                            string str = NextSqlFromStream(reader);
                            if (!string.IsNullOrEmpty(str))
                            {
                                command.CommandText = str.Replace("$VirsualPath$", applicationPath);
                                command.ExecuteNonQuery();
                            }
                        }
                        connection.Close();
                    }
                    reader.Close();
                }
                errorMsg = null;
                return true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                return false;
            }
        }

        private bool ExecuteTest()
        {
            string str;
            this.errorMsgs = new List<string>();
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                if (this.ValidateConnectionStrings(out str))
                {
                    using (connection = new SqlConnection(this.GetConnectionString()))
                    {
                        connection.Open();
                        DbCommand command = connection.CreateCommand();
                        transaction = connection.BeginTransaction();
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandText = "CREATE TABLE installTest(Test bit NULL)";
                        command.ExecuteNonQuery();
                        command.CommandText = "DROP TABLE installTest";
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        connection.Close();
                        goto Label_00E4;
                    }
                }
                this.errorMsgs.Add(str);
            }
            catch (Exception exception)
            {
                this.errorMsgs.Add(exception.Message);
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        this.errorMsgs.Add(exception2.Message);
                    }
                }
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        Label_00E4:
            if (!TestFolder(base.Request.MapPath(Globals.ApplicationPath + "/config/test.txt"), out str))
            {
                this.errorMsgs.Add(str);
            }
            try
            {
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
                {
                    configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
                }
                else
                {
                    configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
                }
                configuration.Save();
            }
            catch (Exception exception3)
            {
                this.errorMsgs.Add(exception3.Message);
            }
            if (!TestFolder(base.Request.MapPath(Globals.ApplicationPath + "/storage/test.txt"), out str))
            {
                this.errorMsgs.Add(str);
            }
            return (this.errorMsgs.Count == 0);
        }

        private string GetConnectionString()
        {
            return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[] { this.dbServer, this.dbUsername, this.dbPassword, this.dbName });
        }

        private RijndaelManaged GetCryptographer()
        {
            RijndaelManaged managed = new RijndaelManaged();
            managed.KeySize = 0x80;
            managed.GenerateIV();
            managed.GenerateKey();
            return managed;
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                this.action = base.Request["action"];
                this.dbServer = base.Request["DBServer"];
                this.dbName = base.Request["DBName"];
                this.dbUsername = base.Request["DBUsername"];
                this.dbPassword = base.Request["DBPassword"];
                this.username = base.Request["Username"];
                this.email = base.Request["Email"];
                this.password = base.Request["Password"];
                this.password2 = base.Request["Password2"];
                this.isAddDemo = !string.IsNullOrEmpty(base.Request["IsAddDemo"]) && (base.Request["IsAddDemo"] == "true");
                this.testSuccessed = !string.IsNullOrEmpty(base.Request["TestSuccessed"]) && (base.Request["TestSuccessed"] == "true");
                this.siteName = (string.IsNullOrEmpty(base.Request["SiteName"]) || (base.Request["SiteName"].Trim().Length == 0)) ? "Hishop" : base.Request["SiteName"];
                this.siteDescription = (string.IsNullOrEmpty(base.Request["SiteDescription"]) || (base.Request["SiteDescription"].Trim().Length == 0)) ? "最安全，最专业的网上商店系统" : base.Request["SiteDescription"];
            }
            else
            {
                this.dbServer = this.txtDbServer.Text;
                this.dbName = this.txtDbName.Text;
                this.dbUsername = this.txtDbUsername.Text;
                this.dbPassword = this.txtDbPassword.Text;
                this.username = this.txtUsername.Text;
                this.email = this.txtEmail.Text;
                this.password = this.txtPassword.Text;
                this.password2 = this.txtPassword2.Text;
                this.isAddDemo = this.chkIsAddDemo.Checked;
                this.siteName = (this.txtSiteName.Text.Trim().Length == 0) ? "Hishop" : this.txtSiteName.Text;
                this.siteDescription = (this.txtSiteDescription.Text.Trim().Length == 0) ? "最安全，最专业的网上商店系统" : this.txtSiteDescription.Text;
            }
        }

        private static string NextSqlFromStream(StreamReader reader)
        {
            StringBuilder builder = new StringBuilder();
            string strA = reader.ReadLine().Trim();
            while (!reader.EndOfStream && (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0))
            {
                builder.Append(strA + Environment.NewLine);
                strA = reader.ReadLine();
            }
            if (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0)
            {
                builder.Append(strA + Environment.NewLine);
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            this.btnInstall.Click += new EventHandler(this.btnInstall_Click);
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string str = "无效的操作类型：" + this.action;
                bool flag2 = false;
                if (this.action == "Test")
                {
                    flag2 = this.ExecuteTest();
                }
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                if (flag2)
                {
                    base.Response.Write("{\"Status\":\"OK\"}");
                }
                else
                {
                    string str2 = "";
                    if ((this.errorMsgs != null) && (this.errorMsgs.Count > 0))
                    {
                        foreach (string str3 in this.errorMsgs)
                        {
                            str2 = str2 + "{\"Text\":\"" + str3 + "\"},";
                        }
                        str2 = str2.Substring(0, str2.Length - 1);
                        this.errorMsgs.Clear();
                    }
                    else
                    {
                        str2 = "{\"Text\":\"" + str + "\"}";
                    }
                    base.Response.Write("{\"Status\":\"Fail\",\"ErrorMsgs\":[" + str2 + "]}");
                }
                base.Response.End();
            }
        }

        private bool SaveConfig(out string errorMsg)
        {
            try
            {
                System.Configuration.Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                configuration.AppSettings.Settings.Remove("Installer");
                using (RijndaelManaged managed = this.GetCryptographer())
                {
                    configuration.AppSettings.Settings["IV"].Value = Convert.ToBase64String(managed.IV);
                    configuration.AppSettings.Settings["Key"].Value = Convert.ToBase64String(managed.Key);
                }
                MachineKeySection section = (MachineKeySection) configuration.GetSection("system.web/machineKey");
                section.ValidationKey = CreateKey(20);
                section.DecryptionKey = CreateKey(0x18);
                section.Validation = MachineKeyValidation.SHA1;
                section.Decryption = "3DES";
                configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = this.GetConnectionString();
                configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                configuration.Save();
                errorMsg = null;
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private bool SaveSiteSettings(out string errorMsg)
        {
            errorMsg = null;
            if ((this.siteName.Length > 30) || (this.siteDescription.Length > 30))
            {
                errorMsg = "网店名称和简单介绍的长度不能超过30个字符";
                return false;
            }
            try
            {
                string filename = base.Request.MapPath(Globals.ApplicationPath + "/config/SiteSettings.config");
                XmlDocument doc = new XmlDocument();
                SiteSettings settings = new SiteSettings(base.Request.Url.Host, null);
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Settings></Settings>");
                settings.SiteName = this.siteName;
                settings.SiteDescription = this.siteDescription;
                settings.WriteToXml(doc);
                doc.Save(filename);
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private void ShowMsg(string errorMsg, bool seccess)
        {
            this.lblErrMessage.Text = errorMsg;
        }

        private static bool TestFolder(string folderPath, out string errorMsg)
        {
            try
            {
                File.WriteAllText(folderPath, "Hi");
                File.AppendAllText(folderPath, ",This is a test file.");
                File.Delete(folderPath);
                errorMsg = null;
                return true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
                return false;
            }
        }

        private bool ValidateConnectionStrings(out string msg)
        {
            msg = null;
            if ((!string.IsNullOrEmpty(this.dbServer) && !string.IsNullOrEmpty(this.dbName)) && !string.IsNullOrEmpty(this.dbUsername))
            {
                return true;
            }
            msg = "数据库连接信息不完整";
            return false;
        }

        private bool ValidateUser(out string msg)
        {
            msg = null;
            if ((string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.email)) || (string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.password2)))
            {
                msg = "管理员账号信息不完整";
                return false;
            }
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((this.username.Length > config.UsernameMaxLength) || (this.username.Length < config.UsernameMinLength))
            {
                msg = string.Format("管理员用户名的长度只能在{0}和{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength);
                return false;
            }
            if (string.Compare(this.username, "anonymous", true) == 0)
            {
                msg = "不能使用anonymous作为管理员用户名";
                return false;
            }
            if (!Regex.IsMatch(this.username, config.UsernameRegex))
            {
                msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
                return false;
            }
            if (this.email.Length > 0x100)
            {
                msg = "电子邮件的长度必须小于256个字符";
                return false;
            }
            if (!Regex.IsMatch(this.email, config.EmailRegex))
            {
                msg = "电子邮件的格式错误";
                return false;
            }
            if (this.password != this.password2)
            {
                msg = "管理员登录密码两次输入不一致";
                return false;
            }
            if ((this.password.Length >= Membership.Provider.MinRequiredPasswordLength) && (this.password.Length <= config.PasswordMaxLength))
            {
                return true;
            }
            msg = string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength);
            return false;
        }
    }
}

