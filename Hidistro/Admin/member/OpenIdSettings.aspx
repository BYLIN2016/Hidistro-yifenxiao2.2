<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OpenIdSettings.aspx.cs" Inherits="Hidistro.UI.Web.Admin.member.OpenIdSettings" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
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
<div class="areacolumn clearfix  td_top_ccc">

		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/03.gif" width="32" height="32" /></em>
		    <h1>信任登录-<asp:Literal ID="lblDisplayName" runat="server"></asp:Literal></h1>
		    <span>用户只要使用<asp:Literal ID="lblDisplayName2" runat="server"></asp:Literal>账号进行登录即可注册成为网站会员，帮助您提升网站的用户注册量和提升网站访问数据</span></div>
		   <div class="formitem validator4">
                 <div style="float:left">
                   <ul>
		                  <li> <span class="formitemtitle Pw_110"><em >*</em>显示名称：</span>
		                    <asp:TextBox ID="txtName" runat="server" CssClass="forminput"></asp:TextBox>
		                     <p id="ctl00_contentHolder_txtNameTip">设置此信任登录方式在前台的显示名称</p>
	                      </li>
	                      </ul>
                          
                           <ul id="pluginContainer" class="attributeContent2">
	                      <li rowtype="attributeTemplate" style="display:none;">
	                       <span class="formitemtitle Pw_110">$Name$：</span>
		                    $Input$
	                      </li>
	                      </ul>
       
                 </div>
                                            <ul>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
         <span><Kindeditor:KindeditorControl id="fcContent" runat="server" Width="550px"  height="200px" /></span>
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