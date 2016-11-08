<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomContent.aspx.cs" Inherits="Hidistro.UI.Web.DialogTemplates.CustomContent" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script type="text/javascript" src="../Utility/jquery-1.6.4.min.js"></script>
    <link href="css/design.css" rel="stylesheet" type="text/css" />
    <script>

        function HTMLEncode(input) {
            return String(input).replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
        }

        function HTMLDecode(input) {
            return String(value).replace(/&quot;/g, '"').replace(/&#39;/g, "'").replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <Kindeditor:KindeditorControl FileCategoryJson="~/DialogTemplates/FileCategoryJson.ashx" UploadFileJson="~/DialogTemplates/UploadFileJson.ashx" FileManagerJson="~/DialogTemplates/FileManagerJson.ashx" ID="customDescription" runat="server" Width="960px" Height="400px"/>
    </div>
    </form>
</body>
</html>
