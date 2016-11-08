<%@ Page Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ImageReplace.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ImageReplace" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

 </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <%--添加规格值--%>
<div>
       <div class="frame-content">
        <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
        <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
        <p><span  class="frame-span frame-input90">上传图片：<em>*</em></span> <asp:FileUpload ID="FileUpload1" runat="server" onchange="FileExtChecking(this)"/></p>
        <div class="frame_buttons">   <input runat="server" type="hidden" id="Hidden1" />
          <asp:Button ID="btnSaveImageData" runat="server" Text="确 定"  CssClass="submit_sure" OnClientClick="return isFlagValue()" />
           <input type="button" value="取 消" onclick="javascript:art.dialog.close();" />
        </div>
    </div>
</div>
<script>
    function isFlagValue() {
        imgsrc = $("#ctl00_contentHolder_RePlaceImg").val();
        imgid = $("#ctl00_contentHolder_RePlaceId").val();
        if (imgsrc.length <= 0 || imgid.length <= 0) {
            alert("请选择要替换的图片或图片名称不允许为空！");
            return false;
        }

        return true;
    }
</script>
</asp:Content>

