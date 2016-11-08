<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.DistributorRequestInstruction" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
	<!--选项卡-->
	<div class="areacolumn clearfix">
		<div class="columnright">
	<div class="title">
  <em><img src="../images/01.gif" width="32" height="32" /></em>
  <h1>分销商招募说明</h1>
  <span>设置前台分销商注册的说明文字</span>
</div>
    <div class="blank5 clearfix"></div>
    <div class="formitem validator3">
		<ul>
        	<li><span class="formitemtitle Pw_140">招募说明：</span><span><Kindeditor:KindeditorControl ID="fkFooter" runat="server"   Height="298" /></span></li>
            <li><div class="blank5 clearfix"></div></li>
            <li><span class="formitemtitle Pw_140"><em>*</em>分销商注册协议：</span><span><Kindeditor:KindeditorControl ID="fkProtocols" runat="server"    ImportLib="false" Height="298"/></span></li>
        </ul>
	</div>
	    <div class="blank5 clearfix"></div>
    <div class="btn Pa_140">
      <asp:Button ID="btnOK" runat="server" Text="保 存" CssClass="submit_DAqueding" />
</div>
</div>
</div>
<div class="databottom"></div>	
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>

