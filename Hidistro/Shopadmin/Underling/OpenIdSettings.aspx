<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="OpenIdSettings.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.OpenIdSettings" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        // 显示名称
        initValid(new InputValidator('ctl00_contentHolder_txtName', 1, 50, false, null, '名称不能为空，长度限制在50个字符以内'));
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix validator4">
		<div class="columnleft clearfix">
                  <ul>
                        <li><a href="OpenIdServices.aspx"><span>信任登录列表</span></a></li>
                  </ul>
  </div>
		<div class="columnright">
		  <div class="title title_height"> 
		    <h1>配置信任登录-<asp:Literal runat="server" ID="lblDisplayName"></asp:Literal></h1>
		  </div>
		  <div class="formitem">
		    <ul>
		      <li> <span class="formitemtitle Pw_110">显示名称：<em >*</em></span>
		        <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox>
		         <p id="ctl00_contentHolder_txtNameTip">名称不能为空，长度限制在50个字符以内</p>
	          </li>
	          </ul>
	          </div>
	          <div class="formitem">
	          <ul id="pluginContainer">
	          <li rowtype="attributeTemplate" style="display:none;">
	           <span class="formitemtitle Pw_110">$Name$：</span>
		        $Input$
	          </li>
	          </ul>
	          </div>
	          <div class="formitem">
	          <ul>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
         <span><Kindeditor:KindeditorControl FileCategoryJson="~/Shopadmin/FileCategoryJson.aspx" UploadFileJson="~/Shopadmin/UploadFileJson.aspx" FileManagerJson="~/Shopadmin/FileManagerJson.aspx" ID="fcContent" runat="server" Width="550px"  height="200px" /></span>
          </li>
	        </ul>
    <ul class="btn Pa_110 clear">
     <asp:Button ID="btnSave" runat="server" OnClientClick="return PageIsValid();" Text="保 存"  CssClass="submit_DAqueding"/>
            </ul>
	      </div>
  </div>
</div>
<div class="databottom">
  <div class="databottom_bg"></div>
</div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
 <asp:HiddenField runat="server" ID="txtSelectedName" />
  <asp:HiddenField runat="server" ID="txtConfigData" />
  <Hi:Script ID="Script1" runat="server" Src="/utility/plugin.js" />
  <script type="text/javascript">
      $(document).ready(function() {
          pluginContainer = $("#pluginContainer");
          templateRow = $(pluginContainer).find("[rowType=attributeTemplate]");
          selectedNameCtl = $("#<%=txtSelectedName.ClientID %>");
          configDataCtl = $("#<%=txtConfigData.ClientID %>");
          ResetContainer($(selectedNameCtl).val(), "OpenIdService");
      });
</script>
</asp:Content>
