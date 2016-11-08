
<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="EditeMyThems.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.EditeMyThems" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Subsites.Utility" Assembly="Hidistro.UI.Subsites.Utility" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Validator" Assembly="Hidistro.UI.Common.Validator" %>
<%@ Register TagPrefix="Kindeditor" Namespace="kindeditor.Net" Assembly="kindeditor.Net" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Membership.Context"%>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<div class="optiongroup mainwidth">
      <ul>
        <li class="optionstar"><a href="ManageMyThemes.aspx" class="optionnext"><span>模板管理</span></a></li>
        <li class="menucurrent"><a href="#"><span>可视化编辑</span></a></li>
        <li><a href="SiteUrlDetails.aspx"><span>域名管理</span></a></li>
        <li class="optionend"><a href="SetMyHeaderMenu.aspx"><span>页头菜单设置</span></a></li>
      </ul>
</div>
  <div class="blank12 clearfix"></div>
<div class="dataarea mainwidth databody">
    <div class="title"> 
      <em><img src="../images/01.gif" width="32" height="32" /></em>
      <h1>您正在使用“<a href="ManageThemes.aspx"><asp:Literal ID="litThemeName" runat="server" /></a>”模板</h1>
		<span>店铺的页面风格，好比实体店面的装修，您可以从以下列表中选择您喜欢的风格</span>
    </div>

	<div class="datafrom">
      <div class="Template">
        <h1>可编辑的模板页面</h1>
            <ul class="templateedit">
                      <li class="index">
                        <a title="编辑首页" runat="server" id="alinkDefault" target="_blank"><smp></smp></a>
                      </li>  
                      <li class="login">
                        <a title="编辑登陆" runat="server" id="alinkLogin" target="_blank"><smp></smp></a>
                      </li> 
                      <li class="brand">
                        <a title="编辑品牌" runat="server" id="alinkBrand" target="_blank"><smp></smp></a>
                      </li>                                                                                                           
                 </ul> 
	   
       </div>

	</div>
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
