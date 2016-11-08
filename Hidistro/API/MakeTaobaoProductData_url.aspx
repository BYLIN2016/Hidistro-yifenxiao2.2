<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MakeTaobaoProductData_url.aspx.cs" Inherits="Hidistro.UI.Web.API.MakeTaobaoProductData_url" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <div  style="background-color:#FFFFC6;width:630px;height:250px; margin:200px auto;border:1px solid #FFFFC6">
        <div style="margin:90px auto;width:500px;font-weight:700; font-size:15px;">
            <div style="float:left; background:url(../Admin/images/ico.gif) no-repeat left top; width:50px;height:50px;"></div>
            <asp:Literal ID="litmsg" runat="server" Text="制作淘宝格式的商品数据成功"></asp:Literal> 
            <div style="margin:5px 5px;font-size:12px;font-weight:normal;">您可以　<a href="../admin/product/maketaobaoproducts.aspx">继续制作淘宝数据</a></div>
        </div>
    </div>
</body>
</html>
