<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEidt.aspx.cs" Inherits="Hidistro.UI.Web.Admin.store.TestEidt" %>
<%@ Register assembly="kindeditor.Net" namespace="kindeditor.Net" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>   
       <cc1:KindeditorControl ID="ServerControl11" runat="server"></cc1:KindeditorControl>
    </div>
    </form>
    
</body>
</html>
