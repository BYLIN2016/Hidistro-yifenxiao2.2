<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.ImageFtp"  MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
     <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	    <!--面包屑-->
<div class="dataarea mainwidth databody">
	<div class="title">
		  <em><img src="../images/01.gif" width="32" height="32" /></em>
          <h1><strong>图片上传</strong></h1>
          <span>上传图片至服务器 </span>
	</div>
	<div class="datalist">
		<div class="searcharea clearfix br_search">
				<ul>
					<li><span>上传到：</span><span class="formselect"><Hi:ImageDataGradeDropDownList ID="dropImageFtp" runat="server" /></span></li>
				</ul>
		</div>
	<div class="imageDataRight">
       <div class="borderthin">
         <ul class="RightHead">图片分类:</ul>       
         <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" />
        <ul class="pad10"><a href="<%= Globals.GetAdminAbsolutePath("/store/ImageType.aspx")%>" class="submit_queding" style="display:block;text-align:center;">分类管理</a></ul>
      </div>
      </div> 
		<div class="imageDataLeft">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="66%" valign="top">
                    <table cellpadding="0" cellspacing="5" id="ImagesFtp" class="ImagesFtp">
                      <tr>
                        <td width="100%"><!--<input type="file" onchange="FileExtChecking(this)" name="file" />-->
                         <asp:FileUpload ID="FileUpload" runat="server" onchange="FileExtChecking(this)"/>
                        </td>
                        <td width="54" nowrap="nowrap"><a onclick="AddAttachment()" href="javascript:void(0)" class="add">添加</a></td>
                      </tr>
                    </table>
                    </td>
                    <td width="34%" rowspan="2" valign="top">
                    <div class="ImagesMsg"><strong>提示：</strong><br/>
                        1.您一次最多可以上传10张图片。<br/>
                        2.请勿重复选择同一个图片文件。<br/>
                        3.图片文件的大小建议控制在500KB以内，图片太大会影响网站打开速度
                     </div>
                    </td>
                  </tr>
                  <tr><td><div class="btn_bottom"><asp:Button ID="btnSaveImageFtp" runat="server" Text="确定上传"  CssClass="submit_DAqueding"/></div></td></tr>
                </table>

        </div>
	</div>
  
</div>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function AddAttachment() {
        var objTable = $("#ImagesFtp");
        var intCount = $("#ImagesFtp tr").children().size()/2;
        if (intCount >= 10)
		 { alert("附件不能超过10个"); return; }
        objTable.append("<tr><td width=\"100%\"><input type='file' name='fileFtp' onchange='FileExtChecking(this)' /></td><td nowrap=\"nowrap\"><a href='javascript:void(0);' onclick='DisposeTr(this)' class='del'>移除</a></td></tr>");
   }
    function DisposeTr(arg_obj_item)
	{
        var objTr = $(arg_obj_item).parent().parent();
        objTr.remove();
    }
	
    function DisposeTr(arg_obj_item) {
        var objTr = $(arg_obj_item).parent().parent();
        objTr.remove();
    }
    
    function FileExtChecking(obj)
    {
         var ErrMsg=""; 
         var AllowExt=".jpg|.gif|.png|.jpeg|.bmp|"  //允许上传的文件类型 每个扩展名后边要加一个"|" 小写 
         var FileExt=obj.value.substr(obj.value.lastIndexOf(".")).toLowerCase();
          if(AllowExt!=0&&AllowExt.indexOf(FileExt+"|")==-1)  //判断文件类型是否允许上传
          {
            ErrMsg="\n该类型不允许上传！</br>请上传:"+AllowExt+"类型的文件!";
            obj.value="";
            ShowMsg(ErrMsg,false);
            return false;
          }   
    }
    
  /*  function FileExtCheckAll()
    {
         var ErrMsg=""; 
         var AllowExt=".jpg|.gif|.png|.jpeg|"  //允许上传的文件类型 每个扩展名后边要加一个"|" 小写 
        var inputFile=document.getElementsByTagName("input");
        for(var i=0;i<inputFile.length;i++)
        {
          var FileExt=obj.value.substr(inputFile[i].value.lastIndexOf(".")).toLowerCase();
          alert(FileExt);
        if(AllowExt.indexOf(FileExt+"|")==-1)  //判断文件类型是否允许上传
          {
            ErrMsg="\n该文件类型不允许上传！</br>请上传:"+AllowExt+" 类型的文件,当前文件类型为"+FileExt;
            ShowMsg(ErrMsg,false);
            return false;
          }         
        } */  

</script>
</asp:Content>