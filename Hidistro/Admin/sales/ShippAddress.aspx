<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ShippAddress.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ShippAddress" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

 </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <%--收货地址--%>
     
 <div id="dlgShipTo">
    <div class="frame-content">
        <p><span class="frame-span frame-input90">收货人姓名：<em>*</em></span> <asp:TextBox ID="txtShipTo" runat="server" CssClass="forminput"></asp:TextBox></p>
        <p><span class="frame-span frame-input90">收货人地址：</span><Hi:RegionSelector runat="server" id="dropRegions" /></p>
        <p><span class="frame-span frame-input90">详细地址：<em>*</em></span> <asp:TextBox ID="txtAddress" runat="server" CssClass="forminput" TextMode="multiLine"></asp:TextBox></p>
        <p><span class="frame-span frame-input90">邮政编码：</span> <asp:TextBox ID="txtZipcode" runat="server" CssClass="forminput"></asp:TextBox></p>
        <p><span class="frame-span frame-input90">电话号码：</span><asp:TextBox ID="txtTelPhone" runat="server" CssClass="forminput"></asp:TextBox></p>
        <p><span class="frame-span frame-input90">手机号码：</span><asp:TextBox ID="txtCellPhone" runat="server" CssClass="forminput"></asp:TextBox></p>
    </div>
    <div class="frame_buttons"> 
    <asp:Button ID="btnMondifyAddress"  runat="server" Text="确 定" CssClass="submit_sure" OnClientClick="return ValidationAddress()"  />
     <input type="button" value="取 消" onclick="javascript:art.dialog.close();" />
    </div>
 </div>

<script>
    function ValidationAddress() {
        arrytext = null;
        var shipTo = document.getElementById("ctl00_contentHolder_txtShipTo").value;
        if (shipTo.length < 2 || shipTo.length > 20) {
            alert("收货人名字不能为空，长度在2-20个字符之间");
            return false;
        }
        var address = document.getElementById("ctl00_contentHolder_txtAddress").value;
        if (address.length < 3 || address.length > 200) {
            alert("详细地址不能为空，长度在3-200个字符之间");
            return false;
        }
        var telPhone = document.getElementById("ctl00_contentHolder_txtTelPhone").value;
        if (telPhone.length == 0) {
            return true;
        }
        else {
            if (telPhone.length < 3 || telPhone.length > 20) {
                alert("电话号码可以为空，如果填写长度在3-20个字符之间");
                return false;
            }
        }
        var cellPhone = document.getElementById("ctl00_contentHolder_txtCellPhone").value;
        if (cellPhone.length == 0) {
            return true;
        }
        else {
            if (cellPhone.length < 3 || cellPhone.length > 20) {
                alert("手机号码可以为空，如果长度在3-20个字符之间");
                return false;
            }
        }
        return true;
    }
</script>
</asp:Content>


