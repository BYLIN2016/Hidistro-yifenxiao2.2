<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RedirectLogin.aspx.cs" Inherits="Hidistro.UI.Web.OpenID.RedirectLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>信任登录</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" ID="lblMsg" Text="正在转到第三方登录页面，请稍候..."></asp:Label>
    </div>
    </form>
</body>
</html>
