<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintPurchaseComplete.aspx.cs"
    Inherits="Hidistro.UI.Web.Admin.purchaseOrder.PrintPurchaseComplete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Utility/jquery-1.6.4.min.js"></script>
    <script src="../../Utility/jquery.artDialog.js"></script>
    <script src="../../Utility/iframeTools.js"></script>
    <script>
        $(function () {
            art.dialog.close();
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%=script %>
    </div>
    </form>
</body>
</html>
