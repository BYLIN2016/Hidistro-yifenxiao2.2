<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SiteRequest.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SiteRequest" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
<!--搜索-->
	  <!--结束-->
	  <div class="toptitle"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>申请开通分销子站</h1>
        <span>您可申请开通分销站点,并绑定域名（如www.myshop.com），您的客户就可以通过此域名访问您的网店</span>
      </div>
	  <div class="Site">
	    <ul>
	      <li class="lispan"><span class="colorB fonts">域名解析：</span>您需要进入您的域名管理后台，将域名的cname指向到下面的IP地址：<asp:Literal runat="server" ID="litServerIp"/></li>
        </ul>
      </div>
	  <!--数据列表区域-->
	  <div class="datalist">
	    <table width="200" border="0" cellspacing="0">
	      <tr>
	        <td width="20%">请输入您已经解析好的域名：</td>
	        <td width="80%">
	            <asp:TextBox runat="server" ID="txtFirstSiteUrl" class="forminput input" />
	            <p runat="server" id="txtFirstSiteUrlTip">域名不能为空,限制在30个字符以内,必须为正确格式</p>
	        </td>	        
          </tr>
        </table>
	  </div>
	  <div class="Pg_15" style="text-align:center;"><asp:Button ID="btnAddRequest" runat="server" OnClientClick="return PageIsValid();" Text="提交申请"  CssClass="submit_DAqueding" /></div>
	  <!--数据列表底部功能区域-->
</div>
<div class="databottom"></div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
<script type="text/javascript" language="javascript">
    function InitValidators() {
        initValid(new InputValidator('ctl00_contentHolder_txtFirstSiteUrl', 1, 30, false, '', '域名必填,限制在30个字符以内,必须为有效格式'))
    }
    $(document).ready(function() { InitValidators(); });
</script>
</asp:Content>
