<%@ Page Title="" Language="C#" MasterPageFile="~/Shopadmin/ShopAdmin.Master" AutoEventWireup="true" CodeBehind="SiteUrlDetails.aspx.cs" Inherits="Hidistro.UI.Web.Shopadmin.SiteUrlDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHolder" runat="server">
<!--选项卡-->
	<div class="optiongroup mainwidth">
		<ul>
            <li class="optionstar"><a href="ManageMyThemes.aspx"><span>模板管理</span></a></li>
             <li><a href="EditeMyThems.aspx" class="optionnext"><span>可视化编辑</span></a></li>
            <li class="menucurrent"><a href="#"><span class="optioncenter">域名管理</span></a></li>
            <li class="optionend"><a href="SetMyHeaderMenu.aspx"><span>页头菜单设置</span></a></li>
		</ul>
	</div>
	<!--选项卡--> 
	<div class="dataarea mainwidth">
		<!--搜索-->
		
		<!--结束-->
		<div class="VIPbg fonts">
		  <ul>
		    <li>尊敬的：<span class="colorA"><asp:Literal ID="litUserName" runat="server"></asp:Literal></span></li>
		    <li class="colorQ">您的分销站点已开通,您可以在此查看分销站点的域名绑定情况,如果需要变更分销站点域名绑定,请联系供应商.</li>
	      </ul>
	  </div>
		<!--数据列表区域-->
	  <div class="datalist">
      <table width="200" border="0" cellspacing="0">
		    <tr class="table_title">
		      <td colspan="2" class="td_right td_left">您的分销站点域名列表</td>
	        </tr>
		    <tr>
		      <td width="9%">域名：</td>
		      <td width="66%"><asp:Literal runat="server" ID="litFirstSiteUrl"></asp:Literal></td>
	        </tr>		   
	      </table>
      <div class="blank12 clearfix"></div>
      <div class="VIPbg m_none">
        <ul>
          <li><span class="colorB">绑定独立域名生效后，您就可以通过http://您的域名/ShopAdmin来登录管理后台了</span></li>
        </ul>
      </div>
      </div>
	  <!--数据列表底部功能区域-->
	  <div class="page"></div>

	</div>
	<div class="databottom"></div>
<div class="bottomarea testArea">
  <!--顶部logo区域-->
</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="validateHolder" runat="server">
</asp:Content>
