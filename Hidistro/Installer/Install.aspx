<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Install.aspx.cs" Inherits="Hidistro.UI.Web.Installer.Install" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>易分销 2.2 安装向导</title>
     <script type="text/javascript" language="javascript" src="jquery-1.6.4.min.js"></script>
    <link href="Images/install.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#btnTest").bind("click", function () { RunTest(); });
        });

        var dbServer, dbName, dbUsername, dbPassword;
        var username, email, password, password2;
        var isAddDemo, testSuccessed = false;
        var siteName, siteDescription;

        function GetValues() {
            dbServer = $("#txtDbServer").val();
            dbName = $("#txtDbName").val();
            dbUsername = $("#txtDbUsername").val();
            dbPassword = $("#txtDbPassword").val();

            username = $("#txtUsername").val();
            email = $("#txtEmail").val();
            password = $("#txtPassword").val();
            password2 = $("#txtPassword2").val();

            isAddDemo = $("#chkIsAddDemo").attr("checked");

            siteName = $("#txtSiteName").val();
            siteDescription = $("#txtSiteDescription").val();
        }

        function Callback(action) {
            var resultData;

            $.ajax({
                url: "Install.aspx",
                type: 'post', dataType: 'json', timeout: 10000,
                data: {
                    isCallback: "true",
                    action: action,
                    DBServer: dbServer,
                    DBName: dbName,
                    DBUsername: dbUsername,
                    DBPassword: dbPassword,
                    Username: username,
                    Email: email,
                    Password: password,
                    Password2: password2,
                    IsAddDemo: isAddDemo,
                    SiteName: siteName,
                    SiteDescription: siteDescription,
                    TestSuccessed: testSuccessed
                },
                async: false,
                success: function (result) {
                    resultData = result;
                }
            });

            return resultData;
        }

        function RunTest() {
            if (testSuccessed && (confirm("上一次的安装环境测试已成功，您确定要再次测试吗？") == false)) {
                return;
            }

            DisableButtons();
            GetValues();
            var resultData = Callback("Test")

            if (resultData.Status == "OK") {
                testSuccessed = true;
                alert("测试成功，当前环境符合安装要求");
            }
            else {
                testSuccessed = false;
                ShowErrors(resultData);
            }

            EnableButtons();
        }

        function ShowErrors(resultData) {
            var msg = "";
            $.each(resultData.ErrorMsgs, function (i, item) {
                msg += item.Text + "\r\n";
            });
            alert(msg);
        }

        function EnableButtons() {
            $("#btnTest").removeAttr("disabled");
            $("#btnInstall").removeAttr("disabled");
        }

        function DisableButtons() {
            $("#btnTest").attr({ "disabled": "disabled" });
            $("#btnInstall").attr({ "disabled": "disabled" });
        }

        </script>
</head>
<body>
 <form id="form1" runat="server">
<div  class="db">
<div class="wrap">
	<div class="main">
	<div class="form">
	    <p><asp:Label ID="lblErrMessage" runat="server" CssClass="exp"></asp:Label></p>
		<p class="ipt"><label>数据库地址：</label><asp:TextBox ID="txtDbServer" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p class="exp">如：202.103.87.3,12075(端口号)</p>
		<p><label>数据库名称：</label><asp:TextBox ID="txtDbName" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p><label>数据库登录名：</label><asp:TextBox ID="txtDbUsername" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p><label>数据库密码：</label><asp:TextBox ID="txtDbPassword" TextMode="Password" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p class="ipt"><label>管理员用户名：</label><asp:TextBox ID="txtUsername" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p><label>电子邮件：</label><asp:TextBox ID="txtEmail" runat="server" CssClass="txt" /> <span class="errorfont">*</span></p>
		<p><label>登录密码：</label><asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="txt" /> <span class="errorfont">*</span></p>
		<p><label>确认密码：</label><asp:TextBox ID="txtPassword2" TextMode="Password" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p class="check"><label>添加演示数据：</label><asp:CheckBox ID="chkIsAddDemo" runat="server" /><span class="meta">用于演示的数据，实际应用可删除</span></p>
		<p><label>网址名称：</label> <asp:TextBox ID="txtSiteName" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p><label>简单介绍：</label><asp:TextBox ID="txtSiteDescription" runat="server" CssClass="txt" /><span class="errorfont">*</span></p>
		<p>
		    <input id="btnTest" name="btnTest" type="button" value="测试安装环境" class="test" />
		    <asp:Button ID="btnInstall" runat="server" Text="确认，提交" CssClass="done" />
		</p>
		 <p style="padding-left:10px;"><asp:Label ID="litSetpErrorMessage" runat="server"></asp:Label></p>
	</div>
</div>

</div><div class="footer">
Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 海商网络技术有限公司 版权所有
</div>
</div>
</form>
</body>
</html>