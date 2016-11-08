<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="ShowSiteRequestStatus.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.ShowSiteRequestStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="dataarea mainwidth td_top_ccc">
	  <!--搜索-->
	  <!--结束-->
	  <div class="toptitle"> <em><img src="../images/03.gif" width="32" height="32" /></em>
	    <h1>申请开通分销子站</h1>
        <span>您可申请开通分销站点,并绑定域名（如www.myshop.com），您的客户就可以通过此独立域名访问您的网店</span>
      </div>
	  <div class="Site">
    <ul>
      <li runat="server" id="liFail" visible="false"><span class="colorA fonts"><strong>您提交的申请没有通过审核! </strong></span> <strong class="Pg_45">拒绝原因：</strong><span class="colorB"><asp:Literal runat="server" ID="litRefuseReason"></asp:Literal></span></li>
      <li class="lispan" runat="server" id="liWait" visible="false"><span class="colorA fonts"><strong>请等待审核...</strong></span></li>
      <li class="lispan" runat="server" id="liSuccess" visible="false"><span class="colorA fonts"><strong>站点申请成功</strong></span></li>
      </ul>
      </div>
	  <!--数据列表区域-->
	  <div class="datalist Pa_15">
	    <table width="200" border="0" cellspacing="0">
	      <tr class="table_title">
	        <td colspan="4" class="td_right td_right_fff">您提交的域名如下：<asp:Literal runat="server" ID="litFirstUrl" /></td>
          </tr>
        </table>
	  </div>
	  <div runat="server" id="divRequestAgain" visible="false">
	  <div class="datalist">
	    <table width="200" border="0" cellspacing="0">
	    <tr class="table_title">
	        <td colspan="4" class="td_right td_right_fff"><strong class="colorA">您可以重新提交：</strong></td>
          </tr>
	      <tr>
	       <td width="20%">请输入您已经解析好的域名：</td>
	        <td width="80%">
	            <asp:TextBox runat="server" ID="txtFirstSiteUrl" class="forminput input" />
	            <p runat="server" id="txtFirstSiteUrlTip">域名不能为空,限制在30个字符以内,必须为正确格式</p>
	        </td>	
          </tr>
        </table>
      </div>
	  <div class="Pg_15" style="text-align:center;"><asp:Button ID="btnRequestAgain" runat="server" OnClientClick="return PageIsValid();" Text="提 交"  CssClass="submit_DAqueding" /></div>
	  </div>
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
