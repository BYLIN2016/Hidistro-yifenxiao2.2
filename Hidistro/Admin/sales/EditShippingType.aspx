<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.EditShippingType" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/Hishopv5.css" Media="screen" />
    <Hi:Script runat="server" Src="/admin/js/Hishopv5.js" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="areacolumn clearfix">
		<div class="columnright">
		  <div class="title"> 
            <em><img src="../images/05.gif" width="32" height="32" /></em>
		    <h1>编辑配送方式</h1>
		    <span>修改配送方式的基本信息</span></div>
		  <div class="formitem validator4">
		    <ul>
		      <li> <span class="formitemtitle Pw_110"><em >*</em>配送方式名称：</span>
		        <asp:TextBox ID="txtModeName" runat="server" class="forminput"></asp:TextBox>
		        <p id="ctl00_contentHolder_txtModeNameTip">配送方式名称不能为空，长度限制在60字符以内</p>
	          </li>
	            <li> <span class="formitemtitle Pw_110"><em >*</em>选择物流公司：</span>
		          <span style="float:left;"><Hi:ExpressCheckBoxList ID="expressCheckBoxList" RepeatDirection="Horizontal" RepeatColumns="9" runat="server" /></span>
		        <p id="P1">请选择此配送方式对应的物流公司</p>
	          </li>
	          <li>
	            <span class="formitemtitle Pw_110"><em>*</em>选择运费模板：</span>
	            <span  class="formselect"><hi:ShippingTemplatesDropDownList runat="server" ID="shippingTemplatesDropDownList" NullToDisplay="请选择运费模板" /></span>
	            <a href="javascript:DialogFrame('sales/AddShippingTemplate.aspx?source=add','添加运费模板',null,null)">&nbsp;&nbsp;添加运费模板</a>
		        <p id="P2">请选择此配送方式对应的运费模板</p>
	          </li>
           <li class="clearfix"> <span class="formitemtitle Pw_110">备注：</span>
          <span><Kindeditor:KindeditorControl ID="fck" runat="server"  Height="200px" /></span>
          </li>
	    </ul>
    <ul class="btn Pa_110 clear">
    <asp:Button ID="btnUpDate" runat="server"  CssClass="submit_DAqueding inbnt"  OnClientClick="return ValidateAll();"  Text="确 定"/>
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
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">

<script type="text/javascript" language="javascript">
    
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtModeName', 1, 60, false, null, '配送方式名称不能为空，长度限制在60字符以内'))
    }
    $(document).ready(function() { InitValidators(); IsFlagDate(); });
    function ValidateAll() {
        if (PageIsValid()) {
            if ($("#ctl00_contentHolder_shippingTemplatesDropDownList").val() != ""
        && $("#ctl00_contentHolder_shippingTemplatesDropDownList").val() != undefined) {
                return true;
            }
            else {
                alert("必须要选择一个运费模板。");
                return false;
            }
        }
        return false;
    }
    
    </script>
 </asp:Content>
