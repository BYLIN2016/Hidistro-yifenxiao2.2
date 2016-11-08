<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SkuValue.aspx.cs" Inherits="Hidistro.UI.Web.Admin.SkuValue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">

 </asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">

    <%--添加规格值--%>
<div>
    <div class="frame-content">
        <div id="valueStr" runat="server">      <p><span class="frame-span frame-input90">规格值名：<em>*</em></span> <asp:TextBox ID="txtValueStr" CssClass="forminput" Width="300" runat="server"  onkeydown="javascript:this.value=this.value.replace('，',',')" />  </p>
        <b>多个规格值可用“,”号隔开，每个值的字符数最多15个字符</b>
        </div>
       
        <div id="valueImage" runat="server" visible="false">
            <p><span class="frame-span frame-input90">图片地址：<em >*</em></span><asp:FileUpload ID="fileUpload"  CssClass="input_longest" runat="server" Width="250px" onchange="PreviewImg(this)" /></p>
            <div id="newPreview"></div>
            <b>仅接受jpg、gif、png、格式的图片</b>
            <p><span class="frame-span frame-input90">图片描述：<em>*</em></span> <asp:TextBox ID="txtValueDec" CssClass="forminput" runat="server" /></p>
            <b>1到20个字符！</b>
        </div>
        <div class="frame_buttons">   <input runat="server" type="hidden" id="currentAttributeId" />
          <asp:Button ID="btnCreateValue" runat="server" Text="确 定"  CssClass="submit_sure" OnClientClick="return isFlagValue()" />
           <input type="button" value="取 消" onclick="javascript:art.dialog.close();" />
         </div>
    </div>
</div>
<script>
    function isFlagValue() {
        if ($("#ctl00_contentHolder_valueStr").val() != undefined) {
            var skuValue = document.getElementById("ctl00_contentHolder_txtValueStr").value.replace(/(^\s*)|(\s*$)/g, "");
            if (skuValue.length < 1) {
                alert("请输入规格值");
                return false;
            }
            setArryText('ctl00_contentHolder_txtValueStr', skuValue);
        }
        else {
            var imgpath = document.getElementById("ctl00_contentHolder_fileUpload").value;
            if (imgpath.length < 1) {
                alert("请浏览图片");
                return false;
            }
            var valuedesc = document.getElementById("ctl00_contentHolder_txtValueDec").value.replace(/\s/g, "");
            if (valuedesc == "") {
                alert("请输入图片描述");
                return false;
            }
            setArryText('ctl00_contentHolder_fileUpload', imgpath);
            setArryText('ctl00_contentHolder_txtValueDec', valuedesc);
        }

        return true;
    }


    //添加规格值
    function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) {
        if (useAttributeImage == "True") {
            document.getElementById("ctl00_contentHolder_valueStr").style.display = "none";
            document.getElementById("ctl00_contentHolder_valueImage").style.display = "block";
        }
        else {
            document.getElementById("ctl00_contentHolder_valueImage").style.display = "none";
            document.getElementById("ctl00_contentHolder_valueStr").style.display = "block";
            $("#ctl00_contentHolder_specificationView_currentAttributeId").val(attributeId);
            DialogShow("添加" + attributename + "的规格值", "skuvalueadd", "addSKUValue", "ctl00_contentHolder_specificationView_btnCreateValue");
        }

    }

    function PreviewImg(imgFile) {
        var newPreview = document.getElementById("newPreview");
        newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
        newPreview.style.width = "28px";
        newPreview.style.height = "26px";
    }

    function validatorForm() {
        return isFlagValue();
    }

</script>
</asp:Content>

